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
            if (wirelessBelt.getMsgBufferType() == "Incoming") {
                try {
                    string line = wirelessBelt.getMsgBuffer();
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

            mnuDisconnect.Enabled = false;
            mnuConnect.Enabled = true;
            mnuSettings.Enabled = true;
            txtMess.Enabled = false;
            btnSend.Enabled = false;

            mnuDemo.Enabled = false;
            btnActivate.Enabled = false;
            btnQuery.Enabled = false;
            btnStop.Enabled = false;
            labelStatusMsg.Text = "Disconected.\r\nPorts not set.";
            numThreads--;
        }

        private void UpdateText(string s) {
            txtLog.Text += "them:" + s;
            txtLog.Select(txtLog.TextLength, 0);
            txtLog.ScrollToCaret();
        }

        private void ResetAllComboBoxes() {
            comboBoxMotor.Items.Clear();
            comboBoxRhy.Items.Clear();
            comboBoxMag.Items.Clear();

        }

        private void AddToComboBox(dataTypes queryType, string[] stringArray, ComboBox comboBoxName) {
            TextBox cboxitem = new TextBox();

            if (stringArray[0].Equals("NONE DEFINED")) {
                MessageBox.Show("No " + queryType.ToString() + " values returned");//ERROR
            }
            else if (queryType == dataTypes.MTR) {
                for (int i = 1; i < stringArray.Length; i++) {
                    if (stringArray[i] != null) {
                        comboBoxName.Items.Add(stringArray[i]);
                    }
                }
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
            }
            else if (queryType == dataTypes.MAG) {
                double Period, DutyCycle;
                int Percentage;
                try { //Convert.ToInt16 can cause exception
                    String[] splitMag = new String[2];
                    for (int i = 1; i < stringArray.Length; i++) {
                        if (stringArray[i] != null) {
                            splitMag = stringArray[i].Split(',');

                            if (splitMag.Length == 3) {
                                magnitude_table[i - 1] = splitMag[0]; // records Alpha character of magnitude
                                Period = Convert.ToInt32(splitMag[1]);
                                DutyCycle = Convert.ToInt32(splitMag[2]);
                                Percentage = (int)((DutyCycle / Period) * 100);

                                cboxitem.Text = Percentage.ToString();
                                comboBoxName.Items.Add(Percentage.ToString() + "%");
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("ERROR" + ex);
                }
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

        private void Form1_Closing(object sender, CancelEventArgs e) {
            if (this.numThreads > 0) {
                e.Cancel = true; // cancel Close event if there are still threads
            }
            else {
                e.Cancel = false;
                wirelessBelt.ClosePorts();
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
                    mnuConnect.Enabled = false;
                }
                //else if (form. ){
                //    MessageBox.Show("The Bluetooth or Serial Device is not turned on!");
                //    labelStatusMsg.Text = "Ports not set.";
                //    mnuConnect.Enabled = false;
                //}
                else {
                    inboundPort = form.GetInboundPort();
                    outboundPort = form.GetOutboundPort();
                    mnuConnect.Enabled = true;
                    labelStatusMsg.Text = "Ports set. in:" + inboundPort + "; out:" + outboundPort + "; Waiting to press Connect...";
                }
            }
            form.Close();
            MessageBox.Show("Please make sure the Bluetooth device and PDA are turned on!");
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
                if (wirelessBelt.getStatusBufferType() == "Normal")
                    labelStatusMsg.Text = "Input port opened.";
                else if (wirelessBelt.getStatusBufferType() == "Error")
                    labelStatusMsg.Text = "Error: " + wirelessBelt.getStatusBuffer();

                // Try Output port
                if (inboundPort != outboundPort) {
                    // Check for success
                    if (wirelessBelt.getStatusBufferType() == "Normal")
                        labelStatusMsg.Text = "Output ports opened.";
                    else if (wirelessBelt.getStatusBufferType() == "Error")
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
                mnuConnect.Enabled = false;
                mnuSettings.Enabled = false;
                mnuDisconnect.Enabled = true;
                txtMess.Enabled = true;
                txtLog.Enabled = true;
                btnSend.Enabled = true;

                // Haptic belt Buttons
                mnuTempSpat.Enabled = true;
                mnuDemo.Enabled = true;
                btnActivate.Enabled = true;
                btnQuery.Enabled = true;
                btnStop.Enabled = true;

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                labelStatusMsg.Text = "error.\r\nPorts not set.";
                mnuConnect.Enabled = false;
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
            //closeMe();
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e) {

            try {
                String[] response = wirelessBelt.Query_All();
                String[] motor = wirelessBelt.getMotors();
                String[] rhythm = wirelessBelt.getRhythm(false);
                String[] magnitude = wirelessBelt.getMagnitude();

                // Reset Combo Boxes
                ResetAllComboBoxes();

                AddToComboBox(dataTypes.MTR, motor, comboBoxMotor);
                AddToComboBox(dataTypes.RHY, rhythm, comboBoxRhy);
                AddToComboBox(dataTypes.MAG, magnitude, comboBoxMag);

                // sets defaults to "1" cycle
                comboBoxMotor.SelectedIndex = 0;
                comboBoxRhy.SelectedIndex = 0;
                comboBoxMag.SelectedIndex = 0;
                comboBoxCycles.SelectedIndex = 0;

            }
            catch {
            }

        }

        private void btnStop_Click(object sender, EventArgs e) {
            try {
                String[] response = wirelessBelt.Stop(comboBoxMotor.SelectedItem.ToString());
                labelStatusMsg.Text = "Stop motor " + comboBoxMotor.SelectedItem.ToString() + ".  " + response[0];
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStopAll_Click(object sender, EventArgs e) {
            try {
                stop_demo = true;
                String[] response = wirelessBelt.StopAll();

                labelStatusMsg.Text = "Stoping All Motors.  " + response[0];
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnActivate_Click(object sender, EventArgs e) {
            try {

                String[] response = wirelessBelt.Vibrate_Motor(comboBoxMotor.SelectedItem.ToString(),
                    comboBoxRhy.SelectedItem.ToString(), magnitude_table[comboBoxMag.SelectedIndex],
                    (comboBoxCycles.SelectedIndex + 1));

                labelStatusMsg.Text = "Activating motor " + comboBoxMotor.SelectedItem.ToString() + ".  " + response[0];

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
                MessageBox.Show("These settings are not yet available");
                //String[] response = wirelessBelt.Vibrate_Motor(comboBoxMotor2.SelectedItem.ToString(),
                //        comboBoxRhy2.SelectedItem.ToString(), magnitude_table[comboBoxMag2.SelectedIndex],
                //        (comboBoxCycles2.SelectedIndex+1));

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
            String[] response;
            int motor_total = comboBoxMotor.Items.Count;

            //int index = 1;

            demoMotor = "1";
            demoRhy = demoForm.GetSelectedRhy();
            demoMag = demoForm.GetSelectedMag();
            demoCycles = demoForm.GetSelectedCycles();
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

                int i_current = 1;
                int i_previous = 1;

                if (demoType == demoTypes.SCAN) {
                    for (int index = 1; index <= (motor_total * demoCycles); index++) {
                        i_previous = i_current;
                        i_current = (index % motor_total) + 1;

                        response = wirelessBelt.Vibrate_Motor(i_current.ToString(),
                            demoRhy, magnitude_table[demoMag], demoCycles);
                        // Delayed stop
                        response = wirelessBelt.Stop(i_previous.ToString());
                        //System.Threading.Thread.Sleep(50);
                    }
                }
                else if (demoType == demoTypes.SWEEP) {
                    for (int index = 1; index <= (motor_total * demoCycles); index++) {
                        for (int i = 1; i <= motor_total; i++) {
                            response = wirelessBelt.Vibrate_Motor(i.ToString(),
                                demoRhy, magnitude_table[demoMag], demoCycles);
                            if (i > 1)// Delayed stop
                                response = wirelessBelt.Stop(i_previous.ToString());
                            i_previous = i;
                        }
                        for (int r = i_previous; r > 0; r--) {
                            response = wirelessBelt.Vibrate_Motor(r.ToString(),
                                demoRhy, magnitude_table[demoMag], demoCycles);
                            if (r < i_previous)// Delayed stop
                                response = wirelessBelt.Stop(i_previous.ToString());
                            i_previous = r;
                            //System.Threading.Thread.Sleep(50);
                        }
                    }
                }
                else if (demoType == demoTypes.HEARTBEATS) {
                    for (int index = 1; index <= (motor_total * demoCycles); index += 2) {
                        i_previous = i_current;
                        i_current = (index % motor_total) + 1;

                        response = wirelessBelt.Vibrate_Motor(i_current.ToString(),
                            demoRhy, magnitude_table[demoMag], demoCycles);

                        response = wirelessBelt.Vibrate_Motor((i_current + 1).ToString(),
                            demoRhy, magnitude_table[demoMag], demoCycles);

                        if (i_current > 1) {
                            // Delayed stop
                            response = wirelessBelt.Stop(i_previous.ToString());
                            response = wirelessBelt.Stop((i_previous - 1).ToString());
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