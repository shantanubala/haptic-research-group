using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using HapticDriver;

namespace Haptikos
{
    public partial class MainForm : Form
    {
        HapticBelt wirelessBelt;// = new HapticBelt();

        public enum dataTypes { MTR, RHY, MAG };
        public enum demoTypes { SCAN, SWEEP, HEARTBEATS };

        private string inboundPort = "";
        private string outboundPort = "";
        private string baud_string = "9600";
        private string parity_string = "None";
        private string stopbits_string = "1";
        private string databits_string = "8";
        private string readTimeout_string = "1000";

        protected UInt16 newDataAvail = 0;
        private string[] magnitude_table;

        //Demo elements
        DemoForm demoForm;
        TempSpatForm tempSpatForm;
        bool stop_demo = false;
        private string demoMotor = "";
        private string demoRhy = "";
        private int demoMag = 0;
        private int demoCycles = 0;
        private demoTypes demoType = demoTypes.SWEEP;


        // Threading elements
        bool closeRequested = false;
        bool disconnectRequested = false;
        int numThreads = 0;
        //Thread mainCtrlThread;
        //Thread demoThread;

        // Use delegates to point to the main thread's fucntions
        //private delegate void _demoThread();
        private delegate void updateText(string s);
        //private delegate void closeDel();
        private delegate void disconnectDel();

        public MainForm() {
            InitializeComponent();
            labelStatusMsg.Text = "Ports not set.";

            comboBoxCycles.Items.Add("1");
            comboBoxCycles.Items.Add("2");
            comboBoxCycles.Items.Add("3");
            comboBoxCycles.Items.Add("4");
            comboBoxCycles.Items.Add("5");
            comboBoxCycles.Items.Add("6");
            comboBoxCycles.Items.Add("Run");

            //comboBoxCycles2.Items.Add("1");
            //comboBoxCycles2.Items.Add("2");
            //comboBoxCycles2.Items.Add("3");
            //comboBoxCycles2.Items.Add("4");
            //comboBoxCycles2.Items.Add("5");
            //comboBoxCycles2.Items.Add("6");
            //comboBoxCycles2.Items.Add("Run");

            //comboBoxCycles3.Items.Add("1");
            //comboBoxCycles3.Items.Add("2");
            //comboBoxCycles3.Items.Add("3");
            //comboBoxCycles3.Items.Add("4");
            //comboBoxCycles3.Items.Add("5");
            //comboBoxCycles3.Items.Add("6");
            ////comboBoxCycles3.Items.Add("Run"); not used. 

            wirelessBelt = new HapticBelt();
            magnitude_table = new string[HapticBelt.MAG_MAX_NO];

            // Subscribe event handler to the function ReceiveData( )
            // This works like a C++ function pointer
            wirelessBelt.DataReceivedFxn = this.UpdateTxtLog;

        }

        // This function invokes the main thread's UpdateText function
        // in a loop while waiting for a request to close the application.
        protected void MainControlThread() {
            do {
                Thread.Sleep(2000);
            } while (!closeRequested && !disconnectRequested);

            if (closeRequested)
                closeMe();
            if (disconnectRequested)
                //Invoke main thread function
                this.Invoke(new disconnectDel(onDisconnect));
        }

        // This function invokes the main thread's UpdateText function
        // in a loop while waiting for a request to close the application.
        protected void UpdateTxtLog() {
            if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.INCOMING) {
                try {
                    string line = wirelessBelt.getDataRecvBuffer();
                    if (line.CompareTo("quit$$$") == 0) {
                        disconnectRequested = true;
                    }
                    //Invoke main thread function
                    txtLog.Invoke(new updateText(UpdateText), line);
                }
                catch {

                }
            }
        }
        private void onDisconnect() {
            wirelessBelt.ClosePorts();

            menuDisconnect.Enabled = false;
            menuConnect.Enabled = true;
            menuSettings.Enabled = true;
            menuQryVer.Enabled = false;
            menuQryMtr.Enabled = false;
            menuQryRhy.Enabled = false;
            menuQryMag.Enabled = false;
            menuQryTempSpat.Enabled = false;
            menuSetupRhyMag.Enabled = false;

            txtMess.Enabled = false;
            btnSend.Enabled = false;

            menuDemo.Enabled = false;
            btnActivate.Enabled = false;
            btnQuery.Enabled = false;
            btnStop.Enabled = false;
            labelStatusMsg.Text = "Disconected.\r\nPorts not set.";
            numThreads--;
        }

        private void UpdateText(string s) {
            txtLog.Text += "them:\r\n" + s;
            txtLog.Select(txtLog.TextLength, 0);
            txtLog.ScrollToCaret();
        }

        private void ResetAllComboBoxes() {

            comboBoxMotor.SelectedIndex = -1;
            comboBoxRhy.SelectedIndex = -1;
            comboBoxMag.SelectedIndex = -1;
            comboBoxCycles.SelectedIndex = -1;

            comboBoxMotor.Items.Clear();
            comboBoxRhy.Items.Clear();
            comboBoxMag.Items.Clear();
        }

        private void AddToComboBox(dataTypes queryType, string[] stringArray, ComboBox comboBoxName) {
            TextBox cboxitem = new TextBox();

            if (stringArray[0].Equals("NONE DEFINED") || stringArray[0].Equals("0")) {
                MessageBox.Show("No " + queryType.ToString() + " values returned");//ERROR
            }
            else if (queryType == dataTypes.MTR) {
                Int16 motor_count = Int16.Parse(stringArray[0]);
                if (motor_count != 0) {
                    for (int i = 1; i <= motor_count; i++) {
                        comboBoxName.Items.Add(i);
                    }
                }
                // sets defaul to first index
                comboBoxName.SelectedIndex = 0;
            }
            else if (queryType == dataTypes.RHY) // same as above for now, may change in near future
            {
                String[] splitRhy = new String[3];
                for (int i = 1; i < stringArray.Length; i++) {
                    if (stringArray[i] != null) {
                        splitRhy = stringArray[i].Split(',');
                        comboBoxName.Items.Add(splitRhy[0]);
                    }
                }
                // sets default to first index
                 comboBoxName.SelectedIndex = 0;
            }
            else if (queryType == dataTypes.MAG) {
                String[] splitMag = new String[2];

                // clear magnitude_table
                for (int i = 0; i < magnitude_table.Length; i++) {
                    magnitude_table[i] = null;
                }
                
                try { //Convert.ToInt16 can cause exception
                    for (int i = 1; i < stringArray.Length; i++) {
                        if (stringArray[i] != null) {
                            splitMag = stringArray[i].Split(',');
                            
                            // Ensure that the string array conforms to expected format before processing
                            if (splitMag.Length == 2) {
                                magnitude_table[i - 1] = splitMag[0]; // records Alpha character of each magnitude
                                string Percentage = splitMag[1];
                                cboxitem.Text = Percentage;
                                comboBoxName.Items.Add(Percentage + "%");
                            }
                            // Else -> process period and duty cycle
                            else if (splitMag.Length == 3) {
                                magnitude_table[i - 1] = splitMag[0]; // records Alpha character of each magnitude;
                                double Period = Convert.ToInt32(splitMag[1]);
                                double DutyCycle = Convert.ToInt32(splitMag[2]);
                                int Percentage = (int)((DutyCycle / Period) * 100);
                                cboxitem.Text = Percentage.ToString();
                                comboBoxName.Items.Add(Percentage.ToString() + "%");
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("ERROR" + ex.Message);
                }
                // sets default to first index
                comboBoxName.SelectedIndex = 0;
            }
        }

        private void btnSend_Click(object sender, EventArgs e) {
            try {
                wirelessBelt.WriteData(txtMess.Text.ToUpper());
                txtLog.Text += "you:" + txtMess.Text + "\r\n";
                txtMess.Text = "";
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeMe() {
            numThreads--;
            this.Close();
        }

        private void MainForm_Closing(object sender, CancelEventArgs e) {
            if (this.numThreads > 0) {
                e.Cancel = true; // cancel Close event if there are still threads
            }
            else {
                e.Cancel = false;
                wirelessBelt.ClosePorts();
                int response = wirelessBelt.ResetHapticBelt();
                if (response != 0)
                    MessageBox.Show(wirelessBelt.getErrorMsg(response)
                        + "\n\r Application will be closed when you click OK.");
            }
            // Once this flag is set the UpdateTxtLog thread will catch
            // and handle the request to close.
            // **** THREADS NOT USED
            this.closeRequested = true;
            this.Close();
        }
        private void mnuSettings_Click(object sender, EventArgs e) {
            SettingsForm form = new SettingsForm(inboundPort, outboundPort);
            if (form.ShowDialog() == DialogResult.OK) {
                if (form.GetInboundPort().CompareTo("NO PORT SELECTED") == 0 ||
                    form.GetOutboundPort().CompareTo("NO PORT SELECTED") == 0) {
                    MessageBox.Show("The ports were not set properly.");
                    labelStatusMsg.Text = "Ports not set.";
                    menuConnect.Enabled = false;
                }
                //else if (form. ){
                //    MessageBox.Show("The Bluetooth or Serial Device is not turned on!");
                //    labelStatusMsg.Text = "Ports not set.";
                //    mnuConnect.Enabled = false;
                //}
                else {
                    inboundPort = form.GetInboundPort();
                    outboundPort = form.GetOutboundPort();
                    menuConnect.Enabled = true;
                    labelStatusMsg.Text = "Ports set. in:" + inboundPort + "; out:" + outboundPort + "; Waiting to press Connect...";
                }
            }
            form.Close();
            MessageBox.Show("Please make sure the Bluetooth device and PDA are turned on!");
            // autoconnect
            mnuConnect_Click(sender, e);
        }

        private void mnuSetupRhyMag_Click(object sender, EventArgs e) {
            PatternProgForm form = new PatternProgForm(wirelessBelt);
            if (form.ShowDialog() == DialogResult.OK) {
                labelStatusMsg.Text = "Ports not set.";

            }
            //else if (form. ){
            //    MessageBox.Show("The Bluetooth or Serial Device is not turned on!");
            //    labelStatusMsg.Text = "Ports not set.";
            //    mnuConnect.Enabled = false;
            //}
            else {
                labelStatusMsg.Text = "Ports set. in:";
            }
            form.Close();
            btnQuery_Click(sender,e); // Send a Query Command to refresh menus
        }

        private void mnuConnect_Click(object sender, EventArgs e) {
            // Setting up serial ports
            wirelessBelt.SetupPorts(inboundPort, outboundPort, baud_string, databits_string, stopbits_string, parity_string, readTimeout_string);

            disconnectRequested = false;
            // Do not allow more than 5 threads (no particular reason)
            if (this.numThreads >= 5) {
                MessageBox.Show("4 Threads already running.  Please try again.");
                return;
            }
            try {
                // Open Serial ports & Start Thread
                labelStatusMsg.Text = "Opening input port...";
                wirelessBelt.OpenPorts();

                // Check for success
                if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.NORMAL)
                    labelStatusMsg.Text = "Input port opened.";
                else if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.ERROR)
                    labelStatusMsg.Text = "Error: " + wirelessBelt.getStatusBuffer();

                // Try Output port
                if (inboundPort != outboundPort) {
                    // Check for success
                    if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.NORMAL)
                        labelStatusMsg.Text = "Output ports opened.";
                    else if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.ERROR)
                        labelStatusMsg.Text = "Error: " + wirelessBelt.getStatusBuffer();
                }
                else {
                    labelStatusMsg.Text = "Input port = Output port.  Already open!";
                }
                //// Text Log Thread
                //this.numThreads++;
                //mainCtrlThread = new Thread(new ThreadStart(MainControlThread));
                //mainCtrlThread.Start();

                labelStatusMsg.Text = "Listener handle started.";

                btnSend.Enabled = true;
                labelStatusMsg.Text = "Connected. Inbound:" + inboundPort + "; Outbound:" + outboundPort;
                menuConnect.Enabled = false;
                menuSettings.Enabled = false;
                menuDisconnect.Enabled = true;
                menuQryVer.Enabled = true;
                menuQryMtr.Enabled = true;
                menuQryRhy.Enabled = true;
                menuQryMag.Enabled = true;
                menuQryTempSpat.Enabled = true;
                menuSetupRhyMag.Enabled = true;
                txtMess.Enabled = true;
                txtLog.Enabled = true;
                btnSend.Enabled = true;

                // Haptic belt Buttons
                mnuTempSpat.Enabled = true;
                menuDemo.Enabled = true;
                btnActivate.Enabled = true;
                btnQuery.Enabled = true;
                btnStop.Enabled = true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                labelStatusMsg.Text = "error.\r\nPorts not set.";
                menuConnect.Enabled = false;
            }
        }

        private void mnuDisconnect_Click(object sender, EventArgs e) {
            try {
                // Once this flag is set the UpdateTxtLog thread will catch
                // and handle the request to disconnect.
                //*** THREADS NOT USED CURRENTLY
                disconnectRequested = true;
                onDisconnect();
                //wirelessBelt.WriteData("quit$$$"); Belt does not want any data.
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuClose_Click(object sender, EventArgs e) {
            // Old methods...
            ////closeMe();
            //this.Close();

            // See also MainForm_Closing(object sender, CancelEventArgs e) {
            if (this.numThreads > 0) {
                //e.Cancel = true; // cancel Close event if there are still threads
            }
            else {
                //e.Cancel = false;
                wirelessBelt.ClosePorts();
                int response = wirelessBelt.ResetHapticBelt();
                if (response != 0)
                    MessageBox.Show(wirelessBelt.getErrorMsg(response)
                        + "\n\r Application will be closed when you click OK.");
            }
            // Once this flag is set the UpdateTxtLog thread will catch
            // and handle the request to close.
            // **** THREADS NOT USED
            this.closeRequested = true;
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e) {

            try {
                int response = wirelessBelt.Query_All();
                if (response != 0)
                    labelStatusMsg.Text = wirelessBelt.getErrorMsg(response);
                else {
                    // brackets reqd for casting int array to string array
                    String[] motor = { wirelessBelt.getMotors().ToString() };
                    String[] rhythm = wirelessBelt.getRhythm(false);
                    String[] magnitude = wirelessBelt.getMagnitude(false);

                    // Reset Combo Boxes
                    ResetAllComboBoxes();

                    AddToComboBox(dataTypes.MTR, motor, comboBoxMotor);
                    AddToComboBox(dataTypes.RHY, rhythm, comboBoxRhy);
                    AddToComboBox(dataTypes.MAG, magnitude, comboBoxMag);

                    // sets default to first index
                    comboBoxCycles.SelectedIndex = 0;

                    // Update status message
                    labelStatusMsg.Text = wirelessBelt.getStatusBufferStr() + ".  ";
                        //+ wirelessBelt.getCommStatusMsg(); TODO
                }
            }
            catch (Exception ex) {
                labelStatusMsg.Text = wirelessBelt.getStatusBufferStr() + " " + ex.Message;
            }
        }


        private void menuItemQryVer_Click(object sender, EventArgs e) {
            try {
                int response = wirelessBelt.Query_Version();
                if (response != 0)
                    labelStatusMsg.Text = wirelessBelt.getErrorMsg(response);
                else {
                    String version = wirelessBelt.getVersion();

                    // Reset Combo Boxes
                    ResetAllComboBoxes();

                    // Update status message
                    labelStatusMsg.Text = wirelessBelt.getStatusBufferStr()
                        + " Firmware Version: " + version;
                }

            }
            catch (Exception ex) {
                labelStatusMsg.Text = wirelessBelt.getStatusBufferStr() + " " + ex.Message;
            }
        }

        private void menuItemQryMtr_Click(object sender, EventArgs e) {
            try {
                int response = wirelessBelt.Query_Motor();
                if (response != 0)
                    labelStatusMsg.Text = wirelessBelt.getErrorMsg(response);
                else {
                    String[] motor = { wirelessBelt.getMotors().ToString() }; // brackets reqd for string array

                    // Reset Combo Boxes
                    ResetAllComboBoxes();

                    AddToComboBox(dataTypes.MTR, motor, comboBoxMotor);

                    // Update status message
                    labelStatusMsg.Text = wirelessBelt.getStatusBufferStr();
                }

            }
            catch (Exception ex) {
                labelStatusMsg.Text = wirelessBelt.getStatusBufferStr() + " " + ex.Message;
            }
        }

        private void menuItemQryRhy_Click(object sender, EventArgs e) {
            try {
                int response = wirelessBelt.Query_Rhythm();
                if (response != 0)
                    labelStatusMsg.Text = wirelessBelt.getErrorMsg(response);
                else {
                    String[] rhythm = wirelessBelt.getRhythm(false);

                    // Reset Combo Boxes
                    ResetAllComboBoxes();

                    AddToComboBox(dataTypes.RHY, rhythm, comboBoxRhy);

                    // Update status message
                    labelStatusMsg.Text = wirelessBelt.getStatusBufferStr();
                }
            }
            catch (Exception ex) {
                labelStatusMsg.Text = wirelessBelt.getStatusBufferStr() + " " + ex.Message;
            }
        }

        private void menuItemQryMag_Click(object sender, EventArgs e) {
            try {
                int response = wirelessBelt.Query_Magnitude();
                if (response != 0)
                    labelStatusMsg.Text = wirelessBelt.getErrorMsg(response);
                else {
                    String[] magnitude = wirelessBelt.getMagnitude(false);

                    // Reset Combo Boxes
                    ResetAllComboBoxes();

                    AddToComboBox(dataTypes.MAG, magnitude, comboBoxMag);

                    // Update status message
                    labelStatusMsg.Text = wirelessBelt.getStatusBufferStr();
                }
            }
            catch (Exception ex) {
                labelStatusMsg.Text = wirelessBelt.getStatusBufferStr() + " " + ex.Message;
            }
        }

        private void menuItemQryTempSpat_Click(object sender, EventArgs e) {
            try {
                int response = wirelessBelt.Query_SpatioTemporal();
                if (response != 0)
                    labelStatusMsg.Text = wirelessBelt.getStatusBufferStr()
                        + " " + wirelessBelt.getErrorMsg(response);

                // Reset Combo Boxes
                ResetAllComboBoxes();
            }
            catch (Exception ex) {
                labelStatusMsg.Text = wirelessBelt.getStatusBufferStr() + " " + ex.Message;
            }
        }

        private void btnStop_Click(object sender, EventArgs e) {
            try {
                int response = wirelessBelt.Stop((byte)comboBoxMotor.SelectedIndex);
                
                labelStatusMsg.Text = "Stop motor " + comboBoxMotor.SelectedItem.ToString()
                    + ".  ";// +wirelessBelt.getCommStatusMsg(); //TODO
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStopAll_Click(object sender, EventArgs e) {
            try {
                stop_demo = true;
                int response = wirelessBelt.StopAll();

                labelStatusMsg.Text = "Stoping All Motors.  " ; //TODO
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnActivate_Click(object sender, EventArgs e) {
            try {

                // Send Vibrate Motor Command
                int response = wirelessBelt.Vibrate_Motor((byte)comboBoxMotor.SelectedIndex,
                    comboBoxRhy.SelectedItem.ToString(), magnitude_table[comboBoxMag.SelectedIndex],
                    (byte)(comboBoxCycles.SelectedIndex + 1));

                if (response != 0)
                    labelStatusMsg.Text = wirelessBelt.getErrorMsg(response);
                //+ " " + wirelessBelt.getCommStatusMsg(); //TODO
                else
                    labelStatusMsg.Text = "Activating motor " + comboBoxMotor.SelectedItem.ToString()
                        + ".  ";//+wirelessBelt.getStatusBufferStr(); //TODO

                // TODO this is a temporary comment out -> sending binary encode does not work at Belt.
                //response = wirelessBelt.Start();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActivateTmpSpat_Click(object sender, EventArgs e) {
            try {
                labelStatusMsg.Text = "Activating Temporal Spatial Pattern";

                // TODO Need to add timing elements in iteritive loop here
                //MessageBox.Show("These settings are not yet available");
                //int response = wirelessBelt.Vibrate_Motor((byte)comboBoxMotor2.SelectedIndex,
                //      comboBoxRhy2.SelectedItem.ToString(), magnitude_table[comboBoxMag2.SelectedIndex],
                //      (byte)(comboBoxCycles2.SelectedIndex + 1));

                //if (response != 0)
                //    labelStatusMsg.Text = "Error: " + wirelessBelt.getErrorMsg(response)
                //        + " " + wirelessBelt.getCommStatusMsg(); //TODO
                //else { }

                // TODO this is a temporary comment out -> sending binary encode does not work at Belt.
                //response = wirelessBelt.Start();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActivateDemo_Click(object sender, EventArgs e) {
            //string motor_no, string rhy_string, string mag_string, int rhy_cycles) {//object sender, EventArgs e) {
            stop_demo = false;
            int response;
            int motor_total = comboBoxMotor.Items.Count;

            //int index = 1;

            demoMotor = "1";
            demoRhy = demoForm.GetSelectedRhy();
            demoMag = demoForm.GetSelectedMag();
            demoCycles = demoForm.GetSelectedCycles() +1; // list is zero based, 0 = stop
            demoType = demoForm.GetDemoType();

            try {

                //if (this.numThreads > 1) {
                //    stop_demo = true;
                //    response = wirelessBelt.StopAll();
                //}
                //// Demo Thread
                //this.numThreads++;
                //demoThread = new Thread(new ThreadStart(this.TempSpatDemo(demoCycles)));
                //demoThread.IsBackground = true;
                //demoThread.Start();
                //do { // creates 1 thread for each iteration

                int i_current = 0;
                int i_previous = 0;

                if (demoType == demoTypes.SCAN) {
                    for (int index = 1; index <= (motor_total * demoCycles); index++) {
                        i_previous = i_current;
                        i_current = (index % motor_total);

                        // Temporary method using strings
                        response = wirelessBelt.Vibrate_Motor(i_current,
                            demoRhy, magnitude_table[demoMag], demoCycles);

                        // Delayed stop
                        response = wirelessBelt.Stop((byte)i_previous);
                        //System.Threading.Thread.Sleep(50);
                    }
                }
                // Multiple activations of this can put the belt in an unknown state
                // May need to re-QUERY ALL to reset the state.
                else if (demoType == demoTypes.SWEEP) {
                    for (int index = 1; index <= (motor_total * demoCycles); index++) {
                        for (int i = 1; i <= motor_total; i++) {

                            // Temporary method using strings
                            response = wirelessBelt.Vibrate_Motor(i,
                                demoRhy, magnitude_table[demoMag], demoCycles);

                            if (i > 1)// Delayed stop
                                response = wirelessBelt.Stop((byte)i_previous);
                            i_previous = i;
                        }
                        for (int r = i_previous; r > 0; r--) {
                            // Temporary method using strings
                            response = wirelessBelt.Vibrate_Motor(r,
                                demoRhy, magnitude_table[demoMag], demoCycles);

                            if (r < i_previous)// Delayed stop
                                response = wirelessBelt.Stop((byte)i_previous);
                            i_previous = r;
                            //System.Threading.Thread.Sleep(50);
                        }
                    }
                }
                // HEARTBEATS DOES NOT WORK
                else if (demoType == demoTypes.HEARTBEATS) {
                    for (int index = 1; index <= (motor_total * demoCycles); index += 2) {
                        i_previous = i_current;
                        i_current = (index % motor_total) + 1;

                        // Temporary method using strings
                        response = wirelessBelt.Vibrate_Motor(i_current,
                            demoRhy, magnitude_table[demoMag], demoCycles);

                        // Temporary method using strings
                        response = wirelessBelt.Vibrate_Motor((i_current + 1),
                            demoRhy, magnitude_table[demoMag], demoCycles);

                        if (i_current > 1) {
                            // Delayed stop
                            // Temporary method using strings
                            response = wirelessBelt.Stop((byte)i_previous);
                            response = wirelessBelt.Stop((byte)(i_previous - 1));
                        }
                        //System.Threading.Thread.Sleep(50);
                    }
                }
                //    if (demoCycles != 7) // 7 equals continuous runtime
                //        demoCycles--;
                //} while (!stop_demo && (demoCycles > 0));

                //// Close the thread
                //this.Invoke(new _demoThread(closeMe));
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuDemo_Click(object sender, EventArgs e) {

            string[] rhyItems = new string[comboBoxRhy.Items.Count];
            string[] magItems = new string[comboBoxMag.Items.Count];
            int tempRhySelect = comboBoxMag.SelectedIndex;
            int tempMagSelect = comboBoxMag.SelectedIndex;
            int selectedIndex = 0;

            // Get Rhythm options
            comboBoxRhy.Visible = false;
            do {
                if (comboBoxRhy != null)
                    comboBoxRhy.SelectedIndex = selectedIndex;
                rhyItems[selectedIndex] = comboBoxRhy.SelectedItem.ToString();
                selectedIndex++;
            } while (selectedIndex < comboBoxRhy.Items.Count);

            // Get Magnitude options
            comboBoxMag.Visible = false;
            selectedIndex = 0;
            do {
                if (comboBoxMag != null)
                    comboBoxMag.SelectedIndex = selectedIndex;
                magItems[selectedIndex] = comboBoxMag.SelectedItem.ToString();
                selectedIndex++;
            } while (selectedIndex < comboBoxMag.Items.Count);


            demoForm = new DemoForm(rhyItems, magItems);
            demoForm.btnActivateDemo.Click += new System.EventHandler(this.btnActivateDemo_Click);
            demoForm.btnStopAll2.Click += new System.EventHandler(this.btnStopAll_Click);

            if (demoForm.ShowDialog() == DialogResult.OK) {
                demoForm.Close();
                demoForm.Dispose();
            }
            // Restore previous setting
            comboBoxRhy.SelectedIndex = tempRhySelect;
            comboBoxMag.SelectedIndex = tempMagSelect;
            comboBoxRhy.Visible = true;
            comboBoxMag.Visible = true;
        }

        private void mnuTempSpat_Click(object sender, EventArgs e) {

            string[] rhyItems = new string[comboBoxRhy.Items.Count];
            string[] magItems = new string[comboBoxMag.Items.Count];
            int tempRhySelect = comboBoxMag.SelectedIndex;
            int tempMagSelect = comboBoxMag.SelectedIndex;
            int selectedIndex = 0;

            // Get Rhythm options
            comboBoxRhy.Visible = false;
            do {
                if (comboBoxRhy != null)
                    comboBoxRhy.SelectedIndex = selectedIndex;
                rhyItems[selectedIndex] = comboBoxRhy.SelectedItem.ToString();
                selectedIndex++;
            } while (selectedIndex < comboBoxRhy.Items.Count);

            // Get Magnitude options
            comboBoxMag.Visible = false;
            selectedIndex = 0;
            do {
                if (comboBoxMag != null)
                    comboBoxMag.SelectedIndex = selectedIndex;
            } while (selectedIndex < comboBoxMag.Items.Count);


            tempSpatForm = new TempSpatForm(rhyItems, magItems);
            tempSpatForm.btnActivateTmpSpat.Click += new System.EventHandler(this.btnActivateTmpSpat_Click);
            tempSpatForm.btnStopAll.Click += new System.EventHandler(this.btnStopAll_Click);

            if (tempSpatForm.ShowDialog() == DialogResult.OK) {
                tempSpatForm.Close();
                tempSpatForm.Dispose();
            }
            // Restore previous setting
            comboBoxRhy.SelectedIndex = tempRhySelect;
            comboBoxMag.SelectedIndex = tempMagSelect;
            comboBoxRhy.Visible = true;
            comboBoxMag.Visible = true;

        }
    }
}