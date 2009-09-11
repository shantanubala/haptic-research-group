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

        enum dataType { MTR, RHY, MAG };

        private string inboundPort = "";
        private string outboundPort = "";
        private string baud_string = "9600";
        private string parity_string = "None";
        private string stopbits_string = "1";
        private string databits_string = "8";
        private string readTimeout_string = "1000";

        private string[] magnitude_table;

        bool closeRequested = false;
        bool disconnectRequested = false;
        int numThreads = 0;
        protected UInt16 newDataAvail = 0;

        private delegate void updateText(string s);
        private delegate void closeDel();
        private delegate void disconnectDel();

        public MainForm() {
            InitializeComponent();
            lblStatus.Text = "Ports not set.";

            comboBoxCycles.Items.Add("1");
            comboBoxCycles.Items.Add("2");
            comboBoxCycles.Items.Add("3");
            comboBoxCycles.Items.Add("4");
            comboBoxCycles.Items.Add("5");
            comboBoxCycles.Items.Add("6");
            comboBoxCycles.Items.Add("Run");

            comboBoxCycles2.Items.Add("1");
            comboBoxCycles2.Items.Add("2");
            comboBoxCycles2.Items.Add("3");
            comboBoxCycles2.Items.Add("4");
            comboBoxCycles2.Items.Add("5");
            comboBoxCycles2.Items.Add("6");
            comboBoxCycles2.Items.Add("Run");

            comboBoxCycles3.Items.Add("1");
            comboBoxCycles3.Items.Add("2");
            comboBoxCycles3.Items.Add("3");
            comboBoxCycles3.Items.Add("4");
            comboBoxCycles3.Items.Add("5");
            comboBoxCycles3.Items.Add("6");
            comboBoxCycles3.Items.Add("Run");

            wirelessBelt = new HapticBelt();
            magnitude_table = new string[HapticBelt.MAG_MAX_NO];

            // Subscribe event handler to the function ReceiveData( )
            // This works like a C++ function pointer
            wirelessBelt.DataReceivedFxn = ReceiveData; //+= new HapticBelt.DataRecievedHandler(ReceiveData);

        }

        //private void ReceiveData(object sender, EventArgs e)
        protected void ReceiveData() {
            if (!closeRequested && !disconnectRequested) {
                string incoming = wirelessBelt.getMsgBufferType();
                if (incoming == "Incoming") {
                    try {
                        string line = wirelessBelt.getMsgBuffer();
                        if (line.CompareTo("quit$$$") == 0) {
                            disconnectRequested = true;
                            //continue;
                        }
                        txtLog.Invoke(new updateText(UpdateText), line);
                    }
                    catch {

                    }
                }
            }
            if (closeRequested)
                closeMe();
            if (disconnectRequested)
                this.Invoke(new disconnectDel(onDisconnect));
        }
        private void onDisconnect() {
            wirelessBelt.ClosePorts();

            mnuDisconnect.Enabled = false;
            mnuConnect.Enabled = false;
            mnuSettings.Enabled = true;
            txtMess.Enabled = false;
            btnSend.Enabled = false;

            btnActivate.Enabled = false;
            btnQuery.Enabled = false;
            btnStop.Enabled = false;
            btnActivateTmpSpat.Enabled = false;
            btnStopAll.Enabled = false;
            lblStatus.Text = "Disconected.\r\nPorts not set.";
            numThreads--;
        }

        private void UpdateText(string s) {
            txtLog.Text += "them:" + s;
            txtLog.Select(txtLog.TextLength, 0);
            txtLog.ScrollToCaret();
        }

        private void ResetAllComboBoxes() {
            comboBoxMotor.Items.Clear();
            comboBoxMotor2.Items.Clear();
            comboBoxMotor3.Items.Clear();
            comboBoxMotor4.Items.Clear();
            comboBoxMotor5.Items.Clear();
            comboBoxRhy.Items.Clear();
            comboBoxRhy2.Items.Clear();
            comboBoxMag.Items.Clear();
            comboBoxMag2.Items.Clear();
        }

        private void AddToComboBox(dataType queryType, string[] stringArray, ComboBox comboBoxName) {
            TextBox cboxitem = new TextBox();

            if (stringArray[0].Equals("NONE DEFINED")) {
                MessageBox.Show("No " + queryType.ToString() + " values returned");//ERROR
            }
            else if (queryType == dataType.MTR) {
                for (int i = 1; i < stringArray.Length; i++) {
                    if (stringArray[i] != null) {
                        comboBoxName.Items.Add(stringArray[i]);
                        comboBoxMotor2.Items.Add(stringArray[i]);
                        comboBoxMotor3.Items.Add(stringArray[i]);
                        comboBoxMotor4.Items.Add(stringArray[i]);
                        comboBoxMotor5.Items.Add(stringArray[i]);
                    }
                }
            }
            else if (queryType == dataType.RHY) // same as above for now, may change in near future
            {
                String[] splitRhy = new String[3];
                for (int i = 1; i < stringArray.Length; i++) {
                    if (stringArray[i] != null) {
                        splitRhy = stringArray[i].Split(',');
                        comboBoxName.Items.Add(splitRhy[0]);
                        comboBoxRhy2.Items.Add(splitRhy[0]);
                        comboBoxRhy3.Items.Add(splitRhy[0]);
                    }
                }
            }
            else if (queryType == dataType.MAG) {
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
                                comboBoxMag2.Items.Add(Percentage.ToString() + "%");
                                comboBoxMag3.Items.Add(Percentage.ToString() + "%");
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
            this.Invoke(new closeDel(this.Close)); //TODO cannot close if serial port thread is sleeping or waiting.
        }

        private void Form1_Closing(object sender, CancelEventArgs e) {
            if (this.numThreads > 0) {
                e.Cancel = true;
            }
            else {
                e.Cancel = false;
                wirelessBelt.ClosePorts();
            }
            this.closeRequested = true;
        }
        private void mnuSettings_Click(object sender, EventArgs e) {
            SettingsForm form = new SettingsForm(inboundPort, outboundPort);
            if (form.ShowDialog() == DialogResult.OK) {
                if (form.GetInboundPort().CompareTo("NO PORT SELECTED") == 0 ||
                    form.GetOutboundPort().CompareTo("NO PORT SELECTED") == 0) {
                    MessageBox.Show("The ports were not set properly.");
                    lblStatus.Text = "Ports not set.";
                    mnuConnect.Enabled = false;
                }
                else {
                    inboundPort = form.GetInboundPort();
                    outboundPort = form.GetOutboundPort();
                    mnuConnect.Enabled = true;
                    lblStatus.Text = "Ports set. in:" + inboundPort + "; out:" + outboundPort + "; Waiting to press Connect...";
                }
            }
        }

        private void mnuConnect_Click(object sender, EventArgs e) {
            // Setting up serial ports
            wirelessBelt.SetupPorts(inboundPort, outboundPort, baud_string, databits_string, stopbits_string, parity_string, readTimeout_string);

            disconnectRequested = false;
            try {
                // Open Serial ports
                lblStatus.Text = "Opening input port...";
                wirelessBelt.OpenPorts();

                // Check for success
                if (wirelessBelt.getStatusBufferType() == "Normal")
                    lblStatus.Text = "Input port opened.";
                else if (wirelessBelt.getStatusBufferType() == "Error")
                    lblStatus.Text = "Error: " + wirelessBelt.getStatusBuffer();

                // Try Output port
                if (inboundPort != outboundPort) {
                    // Check for success
                    if (wirelessBelt.getStatusBufferType() == "Normal")
                        lblStatus.Text = "Output ports opened.";
                    else if (wirelessBelt.getStatusBufferType() == "Error")
                        lblStatus.Text = "Error: " + wirelessBelt.getStatusBuffer();
                }
                else {
                    lblStatus.Text = "Input port = Output port.  Already open!";
                }
                numThreads++;
                lblStatus.Text = "Listener handle started.";

                btnSend.Enabled = true;
                lblStatus.Text = "Connected. Inbound:" + inboundPort + "; Outbound:" + outboundPort;
                mnuConnect.Enabled = false;
                mnuSettings.Enabled = false;
                mnuDisconnect.Enabled = true;
                txtMess.Enabled = true;
                txtLog.Enabled = true;
                btnSend.Enabled = true;

                // Haptic belt Buttons
                btnActivate.Enabled = true;
                btnQuery.Enabled = true;
                btnStop.Enabled = true;
                btnActivateTmpSpat.Enabled = true;
                btnStopAll.Enabled = true;
                btnActivateDemo.Enabled = true;
                btnStopAll2.Enabled = true;

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                lblStatus.Text = "error.\r\nPorts not set.";
                mnuConnect.Enabled = false;
            }
        }

        private void mnuDisconnect_Click(object sender, EventArgs e) {
            try { //TODO cannot disconnect if serial port thread is sleeping or waiting.
                // TODO what about the onClose method???
                disconnectRequested = true;
                wirelessBelt.WriteData("quit$$$");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuClose_Click(object sender, EventArgs e) {
            this.Close(); //TODO cannot close if serial port thread is sleeping or waiting.
        }

        private void btnQuery_Click(object sender, EventArgs e) {
            String[] response = wirelessBelt.Query_All();
            String[] motor = wirelessBelt.getMotors();
            String[] rhythm = wirelessBelt.getRhythm(false);
            String[] magnitude = wirelessBelt.getMagnitude();

            // Reset Combo Boxes
            ResetAllComboBoxes();

            AddToComboBox(dataType.MTR, motor, comboBoxMotor);
            AddToComboBox(dataType.RHY, rhythm, comboBoxRhy);
            AddToComboBox(dataType.MAG, magnitude, comboBoxMag);

            // sets defaults to "1" cycle
            comboBoxCycles.SelectedIndex = 0;
            comboBoxCycles2.SelectedIndex = 0;

        }

        private void btnStop_Click(object sender, EventArgs e) {
            try {
                String[] response = wirelessBelt.Stop(comboBoxMotor.SelectedItem.ToString());
                lblStatus.Text = "Stop motor " + comboBoxMotor.SelectedItem.ToString() + ".  " + response[0];
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStopAll_Click(object sender, EventArgs e) {
            try {
                String[] response = wirelessBelt.StopAll();

                lblStatus.Text = "Stoping All Motors.  " + response[0];
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnStopAll2_Click(object sender, EventArgs e) {
            try {
                String[] response = wirelessBelt.StopAll();

                lblStatus.Text = "Stoping All Motors.  " + response[0];
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

                lblStatus.Text = "Activating motor " + comboBoxMotor.SelectedItem.ToString() + ".  " + response[0];

                // TODO this is a temporary comment out -> sending binary encode does not work at Belt.
                //response = wirelessBelt.Start();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActivateTmpSpat_Click(object sender, EventArgs e) {
            try {
                lblStatus.Text = "Activating Temporal Spatial Pattern";

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
            String[] response;
            int motor_total = comboBoxMotor.Items.Count;
            bool stop_demo = false;

            if (checkBoxScan.Checked == true) {
                for (int i = 1; i <= motor_total; i++) {
                    response = wirelessBelt.Vibrate_Motor(i.ToString(),
                        comboBoxRhy3.SelectedItem.ToString(), magnitude_table[comboBoxMag3.SelectedIndex],
                        (comboBoxCycles3.SelectedIndex + 1));
                    if (i > 1) {
                        // Delayed stop
                        System.Threading.Thread.Sleep(50);
                        response = wirelessBelt.Stop((i - 1).ToString());
                    }

                    System.Threading.Thread.Sleep(250);
                }
            }

            else if (checkBoxSweep.Checked == true) {
                for (int i = 1; i <= motor_total; i++) {
                    response = wirelessBelt.Vibrate_Motor(i.ToString(),
                        comboBoxRhy3.SelectedItem.ToString(), magnitude_table[comboBoxMag3.SelectedIndex],
                        (comboBoxCycles3.SelectedIndex + 1));
                    if (i > 1) {
                        // Delayed stop
                        System.Threading.Thread.Sleep(50);
                        response = wirelessBelt.Stop((i - 1).ToString());
                    }

                    System.Threading.Thread.Sleep(250);
                }
                for (int i = motor_total; i > 0; i--) {
                    response = wirelessBelt.Vibrate_Motor(i.ToString(),
                        comboBoxRhy3.SelectedItem.ToString(), magnitude_table[comboBoxMag3.SelectedIndex],
                        (comboBoxCycles3.SelectedIndex + 1));
                    if (i > 1) {
                        // Delayed stop
                        System.Threading.Thread.Sleep(50);
                        response = wirelessBelt.Stop((i - 1).ToString());
                    }

                    System.Threading.Thread.Sleep(250);
                }
            }

            else if (checkBoxHeartbeats.Checked == true) {
                for (int i = 1; i <= motor_total; i+=2) {
                    response = wirelessBelt.Vibrate_Motor(i.ToString(),
                        comboBoxRhy3.SelectedItem.ToString(), magnitude_table[comboBoxMag3.SelectedIndex],
                        (comboBoxCycles3.SelectedIndex + 1));

                    response = wirelessBelt.Vibrate_Motor((i+1).ToString(),
                        comboBoxRhy3.SelectedItem.ToString(), magnitude_table[comboBoxMag3.SelectedIndex],
                        (comboBoxCycles3.SelectedIndex + 1));

                    if (i > 1) {
                        // Delayed stop
                        System.Threading.Thread.Sleep(50);
                        response = wirelessBelt.Stop((i - 1).ToString());
                        response = wirelessBelt.Stop((i - 2).ToString());
                    }

                    System.Threading.Thread.Sleep(250);
                }

            }

        }

        private void checkBoxDemo_CheckedChanged(object sender, EventArgs e) {
            if (sender == checkBoxSweep && checkBoxSweep.Checked == true) {
                checkBoxScan.Checked = false;
                checkBoxHeartbeats.Checked = false;
            }
            if (sender == checkBoxScan && checkBoxScan.Checked == true) {
                checkBoxSweep.Checked = false;
                checkBoxHeartbeats.Checked = false;
            }
            if (sender == checkBoxHeartbeats && checkBoxHeartbeats.Checked == true) {
                checkBoxScan.Checked = false;
                checkBoxSweep.Checked = false;
            }

        }


    }
}