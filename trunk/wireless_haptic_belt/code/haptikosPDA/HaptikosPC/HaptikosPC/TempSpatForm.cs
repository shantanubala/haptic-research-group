using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using HapticDriver;

namespace Haptikos
{
    public partial class TempSpatForm : Form
    {
        HapticBelt wirelessBelt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="belt"></param>
        public TempSpatForm(MainForm mainForm, HapticBelt belt) {
            InitializeComponent();

            wirelessBelt = belt;

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
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }

            comboBoxCycles.Items.Add("1");
            comboBoxCycles.Items.Add("2");
            comboBoxCycles.Items.Add("3");
            comboBoxCycles.Items.Add("4");
            comboBoxCycles.Items.Add("5");
            comboBoxCycles.Items.Add("6");
            comboBoxCycles.Items.Add("Run");

            comboBoxTempo.Items.Add("1");
            comboBoxTempo.Items.Add("2");
            comboBoxTempo.Items.Add("3");
            comboBoxTempo.Items.Add("4");
            comboBoxTempo.Items.Add("5");

            comboBoxRhy.SelectedIndex = 0;
            comboBoxMag.SelectedIndex = 0;
            comboBoxCycles.SelectedIndex = 0;
            comboBoxTempo.SelectedIndex = 2; //midline for default tempo

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

                    writeText.Write(patternDesign.Text);
                    // close the stream
                    writeText.Close();
                    myStream.Close();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            string delim = ":";
            string patternCmd = "";
            
            if (radioBtnVibrate.Checked) {
                // Vibrate command
            }
            else if (radioBtnWait.Checked) {
                string waitTime = textBoxWaitTime.Text.Trim();

                if (MainForm.verifyDecDigits(waitTime))
                    patternCmd = "WAIT" + delim + waitTime +" x TEMPO";
                else
                    MessageBox.Show("Invalid Wait Time - must be integers");
            }
            else if (radioBtnStop.Checked) {
                //Stop Command
            }
            else if (radioBtnStopAll.Checked) {
                error_t msg = wirelessBelt.StopAll();

                if (msg != error_t.ESUCCESS)
                    MessageBox.Show(wirelessBelt.getErrorMsg(msg));
            }
            else if (radioBtnPattern.Checked) {
                string pattern = textBoxPatternExist.Text.Trim();

                if (pattern != null && pattern != "")
                    patternCmd = "PATTERN" + delim + pattern;
            }
            else if (radioBtnComment.Checked) {
                string comment = textBoxComment.Text.Trim();
                patternCmd = "//" + comment;
            }

            // Add command if not empty
            if (patternCmd != "") {
                patternDesign.Text += patternCmd + "\n";
                patternDesign.Select(patternDesign.TextLength, 0);
                patternDesign.ScrollToCaret();
            }
        }
    }
}