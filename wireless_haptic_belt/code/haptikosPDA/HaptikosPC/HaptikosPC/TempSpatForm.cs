using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using HapticDriver;

namespace Haptikos
{
    public partial class TempSpatForm : Form
    {
        HapticBelt wirelessBelt;
        private bool _run = false;
        private int _tempo = 1;
        private List<PatternElement> patternPlayback;
        Thread play;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="belt"></param>
        public TempSpatForm(MainForm mainForm, HapticBelt belt) {
            InitializeComponent();

            wirelessBelt = belt;
            patternPlayback = new List<PatternElement>(2); // start with 2 element and grow

            try {
                error_t response = wirelessBelt.Query_All();
                if (response != error_t.ESUCCESS)
                    MessageBox.Show(wirelessBelt.getErrorMsg(response));
                else {
                    // brackets reqd for casting int array to string array
                    String[] motor = { wirelessBelt.getMotors(QueryType.PREVIOUS).ToString() };
                    String[] rhythm = wirelessBelt.getRhythm(false, QueryType.PREVIOUS);
                    String[] magnitude = wirelessBelt.getMagnitude(false, QueryType.PREVIOUS);

                    // Reset Combo Boxes
                    mainForm.ResetAllComboBoxes();

                    // Add to TempSpatForm
                    mainForm.AddToComboBox(MainForm.dataTypes.MTR, motor, comboBoxMotor);
                    mainForm.AddToComboBox(MainForm.dataTypes.RHY, rhythm, comboBoxRhy);
                    mainForm.AddToComboBox(MainForm.dataTypes.MAG, magnitude, comboBoxMag);

                    comboBoxCycles.Items.Add("1");
                    comboBoxCycles.Items.Add("2");
                    comboBoxCycles.Items.Add("3");
                    comboBoxCycles.Items.Add("4");
                    comboBoxCycles.Items.Add("5");
                    comboBoxCycles.Items.Add("6");
                    comboBoxCycles.Items.Add("Run");

                    comboBoxTempo.Items.Add("5"); // Fastest Tempo (div by 1.5)
                    comboBoxTempo.Items.Add("4"); // Faster Tempo (div by 1.25)
                    comboBoxTempo.Items.Add("3"); // Default as entered by user
                    comboBoxTempo.Items.Add("2"); // Slower Tempo (multiply by 1.25)
                    comboBoxTempo.Items.Add("1"); // Slowest Tempo (multiply by 1.5)

                    comboBoxRhy.SelectedIndex = 0;
                    comboBoxMag.SelectedIndex = 0;
                    comboBoxCycles.SelectedIndex = 0;
                    comboBoxTempo.SelectedIndex = 2; //midline for default tempo
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnPatternExist_Click(object sender, EventArgs e) {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select Haptic Pattern File";
            fdlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fdlg.Filter = "Haptic Patterns (*.pattern)|*.pattern|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK) {
                textBoxPatternExist.Text = fdlg.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Save Haptic Pattern File";
            saveFileDialog.FileName = textBoxPatternName.Text + ".pattern";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Haptic Patterns (*.pattern)|*.pattern|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                if ((myStream = saveFileDialog.OpenFile()) != null) {
                    //StreamWriter writeText = new StreamWriter(myStream);
                    TextWriter writeText = new StreamWriter(myStream);

                    writeText.Write(patternDesign.Text.Trim());
                    // close the stream
                    writeText.Close();
                    myStream.Close();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            string dlm = ":";
            string patternCmd = "";

            if (radioBtnVibrate.Checked) {
                string motor = comboBoxMotor.SelectedItem.ToString();
                string rhy_id = comboBoxRhy.SelectedItem.ToString();
                string mag_id = comboBoxMag.SelectedItem.ToString().TrimEnd('%');
                string cycles = comboBoxCycles.SelectedItem.ToString();

                patternCmd = "VIBRATE"
                    + dlm + motor
                    + dlm + rhy_id
                    + dlm + mag_id
                    + dlm + cycles;

                //patternPlayback.Add(new PatternElement("VIBRATE", motor, rhy_id, mag_id, cycles));
            }
            else if (radioBtnWait.Checked) {
                string waitTime = textBoxWaitTime.Text.Trim();

                if (MainForm.verifyDecDigits(waitTime)) {
                    patternCmd = "WAIT" + dlm + waitTime;
                    //patternPlayback.Add(new PatternElement("WAIT", waitTime, "", "", ""));
                }
                else
                    MessageBox.Show("Invalid Wait Time - must be Integers");
            }
            else if (radioBtnStop.Checked) {
                string motor = comboBoxMotor.SelectedItem.ToString();
                patternCmd = "STOP" + dlm + motor;
                //patternPlayback.Add(new PatternElement("STOP", motor, "", "", ""));
            }
            else if (radioBtnStopAll.Checked) {
                patternCmd = "STOP" + dlm + "ALL";
                //patternPlayback.Add(new PatternElement("STOP", "ALL", "", "", ""));
            }
            else if (radioBtnPattern.Checked) {
                string pattern = textBoxPatternExist.Text.Trim();

                if (pattern != null && pattern != "")
                    patternCmd = "PATTERN" + dlm + pattern;
                //patternPlayback.Add(new PatternElement("PATTERN", pattern, "", "", ""));
            }
            else if (radioBtnComment.Checked) {
                string comment = textBoxComment.Text.Trim();
                patternCmd = "//" + comment;
                // Do not add to playback list
            }

            // Add command if not empty
            if (patternCmd != "") {
                patternDesign.Text += patternCmd + "\r\n";
                patternDesign.Select(patternDesign.TextLength, 0);
                patternDesign.ScrollToCaret();
            }
        }

        private void btnPatternLoad_Click(object sender, EventArgs e) {
            string filename = "";

            // Select file
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select Haptic Pattern File";
            fdlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fdlg.Filter = "Haptic Patterns (*.pattern)|*.pattern|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK) {
                filename = fdlg.FileName;
                // Add to title at top, does not include path or extension
                textBoxPatternName.Text = fdlg.SafeFileName.Split('.')[0];
            }

            // Load into TextBox
            readData(filename);
        }

        private void readData(string filename) {

            if (filename != null && filename != "") {

                // Specify file, instructions, and privelegdes
                FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);

                // Create a new stream to read from a file
                StreamReader sr = new StreamReader(file);

                // Read contents of file,
                string s = sr.ReadToEnd();

                // add to display
                patternDesign.Text = s;
                patternDesign.Select(patternDesign.TextLength, 0);
                patternDesign.ScrollToCaret();

                // Close StreamReader and file
                sr.Close();
                file.Close();
            }
        }

        private void loadPatternData(string[] data) {
            // Same No. of elements as PatternElement class
            string[] element = new string[5];

            patternPlayback.Clear();

            for (int line = 0; line < data.Length; line++) {
                element = data[line].Split(':');

                if (element[0].Trim().Equals("VIBRATE")) {
                    if (element[4].Trim().Equals("Run"))
                        patternPlayback.Add(new PatternElement(element[0].Trim(),
                           element[1].Trim(), element[2].Trim(), element[3].Trim(), "7"));
                    else
                        patternPlayback.Add(new PatternElement(element[0].Trim(),
                            element[1].Trim(), element[2].Trim(), element[3].Trim(),
                            element[4].Trim()));
                }
                else if (element[0].Trim().Equals("WAIT")) {
                    patternPlayback.Add(new PatternElement(element[0].Trim(), element[1].Trim(), "", "", ""));
                }
                else if (element[0].Trim().Equals("STOP") && MainForm.verifyDecDigits(element[1])) {
                    patternPlayback.Add(new PatternElement(element[0].Trim(), element[1].Trim(), "", "", ""));
                }
                else if (element[0].Trim().Equals("STOP") && element[1].Equals("ALL")) {
                    patternPlayback.Add(new PatternElement(element[0].Trim(), element[1].Trim(), "", "", ""));
                }
                else if (element[0].Trim().Equals("PATTERN")) {
                    if (element[1] != null && element[1] != "") {

                        // Makes a Recursive call and loads data in order
                        readData(element[1]);
                    }
                }
                else {
                    // Do not add to playback list
                    ;
                }
            }
        }

        private void comboBoxTempo_SelectedIndexChanged(object sender, EventArgs e) {
            int factor = comboBoxTempo.SelectedIndex;

            if (factor == 0)
                //"5" Fastest Tempo (div by 1.5)
                _tempo = (int)(_tempo / 1.5);
            else if (factor == 1)
                //"4" Faster Tempo (div by 1.25)
                _tempo = (int)(_tempo / 1.25);
            else if (factor == 3)
                //"2" // Slower Tempo (multiply by 1.25)
                _tempo = (int)(_tempo * 1.25);
            else if (factor == 4)
                //"1" Slowest Tempo (multiply by 1.5)
                _tempo = (int)(_tempo * 1.5);
            else
                _tempo = 1;
        }

        private void btnTmpSpatStop_Click(object sender, EventArgs e) {

            // Stop thread if _run = false
            _run = false;            
            if (play.IsAlive)
                play.Abort();
            
            // stop actuations
            wirelessBelt.StopAll();
        }

        private void btnTmpSpatStart_Click(object sender, EventArgs e) {
            int max = comboBoxCycles.SelectedIndex;
            char[] separator = { '\r', '\n', ' ' }; // space chars too.

            //Populate internal data structure            
            loadPatternData(patternDesign.Text.Split(separator));

            _run = true;
            play = new Thread(new ParameterizedThreadStart(this.playbackRun), max);
            play.Name = "Pattern Playback";
            play.IsBackground = true;
            play.Start(max);        
            
        }

        private void playbackRun(object max) {
            // local variable
            error_t msg = error_t.EMAX;
            int count = 0;
            PatternElement element = new PatternElement();
            int maxCycles = (int)max;

            while (_run) {
                for (int command = 0; command < patternPlayback.Count; command++) {
                    element = patternPlayback[command];

                    if (element.name.Equals("VIBRATE")) {
                        byte motor = (byte)(Convert.ToByte(element.mtr_time_file) - 1); //use offset
                        string rhy_id = element.rhythm;
                        string mag_id = element.magnitude;
                        byte cycles = (byte)Convert.ToByte(element.cycles);

                        // Send Vibrate Motor Command
                        msg = wirelessBelt.Vibrate_Motor(motor, rhy_id, mag_id, cycles);
                    }
                    else if (element.name.Equals("WAIT")) {
                        int waitTime = Int16.Parse(element.mtr_time_file) * _tempo;

                        System.Threading.Thread.Sleep(waitTime); //give other threads a chance
                        //HapticBelt.Wait(waitTime);
                        msg = error_t.ESUCCESS;
                    }
                    else if (element.name.Equals("STOP") && MainForm.verifyDecDigits(element.mtr_time_file)) {
                        msg = wirelessBelt.Stop((byte)Convert.ToByte(element.mtr_time_file));
                    }
                    else if (element.name.Equals("STOP") && element.mtr_time_file.Equals("ALL")) {
                        msg = wirelessBelt.StopAll();
                    }
                    else {
                        // Do nothing
                        ;
                    }
                    if (msg != error_t.ESUCCESS)
                        MessageBox.Show(wirelessBelt.getErrorMsg(msg));
                }
                count++;
                // Check for cycles if not continuous run == index[6] or 7
                if (count > maxCycles && maxCycles != 6)
                    _run = false;
            }
        }
    }
}