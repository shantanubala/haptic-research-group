using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using HapticDriver;
namespace Haptikos
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PatternProgForm : Form
    {
        HapticBelt wirelessBelt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="belt"></param>
        public PatternProgForm(HapticBelt belt) {
            InitializeComponent();

            wirelessBelt = belt;
            txtLogProg.Enabled = true;

            // Display current belt information
            try {
                error_t response = wirelessBelt.Query_All();
                if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.INCOMING) {
                    string line = wirelessBelt.getDataRecvBuffer();
                    UpdateTxtLog(line);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            //comboBoxMagSel.Items.Add(" ");
            comboBoxMagSel.SelectedIndex = 0;
            comboBoxRhySel.SelectedIndex = 0;
        }

        /// <summary>
        /// This function invokes the main thread's UpdateText function 
        /// in a loop while waiting for a request to close the application
        /// </summary>
        /// <param name="line"></param>
        protected void UpdateTxtLog(string line) {
            txtLogProg.Text += line + "\r\n-----------\r\n";
            txtLogProg.Select(txtLogProg.TextLength, 0);
            txtLogProg.ScrollToCaret();
        }

        private void comboBoxRhySel_SelectedIndexChanged(object sender, EventArgs e) {
            string rhy_id = comboBoxRhySel.SelectedItem.ToString();
            textBoxRhyPattern.Text = wirelessBelt.getRhythmPattern(rhy_id, false, QueryType.PREVIOUS);
            textBoxRhyTime.Text = wirelessBelt.getRhythmTime(rhy_id, QueryType.PREVIOUS);
        }

        private void comboBoxMagSel_SelectedIndexChanged(object sender, EventArgs e) {
            string mag_id = comboBoxMagSel.SelectedItem.ToString();
            textBoxMagPercent.Text = wirelessBelt.getMagnitude(mag_id, false, QueryType.PREVIOUS);
        }

        private void btnQryAll_Click(object sender, EventArgs e) {
            try {
                error_t response = wirelessBelt.Query_All();
                if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.INCOMING) {
                    string line = wirelessBelt.getDataRecvBuffer();
                    UpdateTxtLog(line);
                }
                comboBoxMagSel_SelectedIndexChanged(sender, e);
                comboBoxRhySel_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQryRhy_Click(object sender, EventArgs e) {
            try {
                String[] rhythm = wirelessBelt.getRhythm(false, QueryType.SINGLE);

                if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.INCOMING) {
                    string line = wirelessBelt.getDataRecvBuffer();
                    UpdateTxtLog(line);
                }
                comboBoxRhySel_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQryMag_Click(object sender, EventArgs e) {
            try {
                String[] magnitude = wirelessBelt.getMagnitude(false, QueryType.SINGLE);

                if (wirelessBelt.getDataRecvType() == (byte)HapticDriver.MessageType.INCOMING) {
                    string line = wirelessBelt.getDataRecvBuffer();
                    UpdateTxtLog(line);
                }
                comboBoxMagSel_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProgMag_Click(object sender, EventArgs e) {
            string mag_id = comboBoxMagSel.SelectedItem.ToString();
            int percentage = Int16.Parse(textBoxMagPercent.Text.Trim());

            error_t return_code = wirelessBelt.Learn_Magnitude(mag_id, percentage);

            UpdateTxtLog(wirelessBelt.getErrorMsg(return_code));
        }

        private void btnProgRhy_Click(object sender, EventArgs e) {
            string rhy_id = comboBoxRhySel.SelectedItem.ToString();

            // validation of the 16 hex chars entered is handled by driver
            string pattern_str = textBoxRhyPattern.Text.Trim().ToUpper();
            int rhy_time = Int16.Parse(textBoxRhyTime.Text.Trim());

            error_t return_code = wirelessBelt.Learn_Rhythm(rhy_id, pattern_str, rhy_time, false);

            UpdateTxtLog(wirelessBelt.getErrorMsg(return_code));
        }

        private void btnZap_Click(object sender, EventArgs e) {
            
            error_t return_code = wirelessBelt.Erase_All();

            UpdateTxtLog(wirelessBelt.getErrorMsg(return_code));
        }
    }
}