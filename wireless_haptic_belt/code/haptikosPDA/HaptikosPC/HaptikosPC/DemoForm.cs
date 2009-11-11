using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
namespace Haptikos
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DemoForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rhyItems"></param>
        /// <param name="magItems"></param>
        public DemoForm(string[] rhyItems, string[] magItems)
        {
            InitializeComponent();
            
            // Setup combo boxes
            for (int i = 0; i < rhyItems.Length; i++) {
                comboBoxRhy3.Items.Add(rhyItems[i]);
            }
            for (int i = 0; i < magItems.Length; i++) {
                comboBoxMag3.Items.Add(magItems[i]);
            }

            comboBoxCycles3.Items.Add("1");
            comboBoxCycles3.Items.Add("2");
            comboBoxCycles3.Items.Add("3");
            comboBoxCycles3.Items.Add("4");
            comboBoxCycles3.Items.Add("5");
            comboBoxCycles3.Items.Add("6");
            //comboBoxCycles3.Items.Add("Run"); not used. 

            comboBoxRhy3.SelectedIndex = 0;
            comboBoxMag3.SelectedIndex = 0;
            comboBoxCycles3.SelectedIndex = 0;
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSelectedRhy()     {
            return (string)comboBoxRhy3.SelectedItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSelectedMag()      {
            return (int)comboBoxMag3.SelectedIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSelectedCycles() {
            return (int)(comboBoxCycles3.SelectedIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MainForm.demoTypes GetDemoType() {
            if (checkBoxSweep.Checked == true)
                return MainForm.demoTypes.SWEEP;
            else if (checkBoxScan.Checked == true)
                return MainForm.demoTypes.SCAN;
            else if (checkBoxHeartbeats.Checked == true)
                return MainForm.demoTypes.HEARTBEATS;
            else //Default
                return MainForm.demoTypes.SWEEP;
        }

        private void checkBoxDemo_CheckStateChanged(object sender, EventArgs e) {
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