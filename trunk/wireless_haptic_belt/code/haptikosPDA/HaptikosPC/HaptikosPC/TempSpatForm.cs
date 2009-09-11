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
    public partial class TempSpatForm : Form
    {
        public TempSpatForm(string[] rhyItems, string[] magItems) {
            InitializeComponent();

            // Setup combo boxes
            for (int i = 0; i < rhyItems.Length; i++) {
                comboBoxRhy2.Items.Add(rhyItems[i]);
            }
            for (int i = 0; i < magItems.Length; i++) {
                comboBoxMag2.Items.Add(magItems[i]);
            }

            comboBoxCycles2.Items.Add("1");
            comboBoxCycles2.Items.Add("2");
            comboBoxCycles2.Items.Add("3");
            comboBoxCycles2.Items.Add("4");
            comboBoxCycles2.Items.Add("5");
            comboBoxCycles2.Items.Add("6");
            //comboBoxCycles2.Items.Add("Run"); not used. 

            comboBoxRhy2.SelectedIndex = 0;
            comboBoxMag2.SelectedIndex = 0;
            comboBoxCycles2.SelectedIndex = 0;

        }
    }
}