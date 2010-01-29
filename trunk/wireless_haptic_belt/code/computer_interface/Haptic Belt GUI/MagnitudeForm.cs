using System;
using System.Windows.Forms;
using HapticDriver;

namespace HapticGUI
{
    partial class MagnitudeForm : Form
    {
        String hold_magnitude; //Variable to hold belt magnitude while testing
        GUI.Magnitude[] magnitude;
        HapticBelt belt;
        int _motorcount;
        
        public MagnitudeForm(GUI.Magnitude[] incoming_mags, HapticBelt incoming_belt, Boolean connected)
        {
            InitializeComponent();
            
            //Sets Globals from parameters
            magnitude = incoming_mags;
            belt = incoming_belt;
            _motorcount = 0;

            //Disables/Enables functions based on connectivity boolean passed
            MagTest.Enabled = connected;
            MagTestStop.Enabled = connected;
        }

        public GUI.Magnitude[] getMagnitudes()
        {
            return magnitude;
        }

        //Checks for error based on given error_t param, errorLOC param is used for specificying the location of the error for debugging
        private bool hasError(error_t error, String errorLOC)
        {
            if (error == error_t.ESUCCESS)
                return false;
            else
            {
                ErrorForm errorForm = new ErrorForm(belt.getErrorMsg(error), errorLOC, true);
                errorForm.ShowDialog();
                return true;
            }
        }

        //Exits
        private void MagDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Learns and plays Magnitude on belt using Magnitude "A", replaces original Magnitude "A" when finished. Learns 100% on Rhythm onto the belt memory "H" as well.
        //Note that we must use TimeSpan, so that we can replace the original Magnitude upon completion
        private void MagTest_Click(object sender, EventArgs e)
        {
            Test_Magnitude();
        }
        private void MagTestStop_Click(object sender, EventArgs e)
        {
            Stop_Magnitude_Test();
        }
        //Calls Library function to learn Magnitude based on user values
        private void MagSet_Click(object sender, EventArgs e)
        {
            magnitude[MagComboBox.SelectedIndex].period = Convert.ToUInt16(Period.Value);
            magnitude[MagComboBox.SelectedIndex].dutycycle = Convert.ToUInt16(DutyCycle.Value);
        }
        //Duty Cycle < Period, we must uphold this truth here, as well as update percentage
        private void Period_ValueChanged(object sender, EventArgs e)
        {
            Change_Period();
        }
        //A change in duty cycle relates to a change in the percentage, update percentage
        private void DutyCycle_ValueChanged(object sender, EventArgs e)
        {
            Percentage.Value = (DutyCycle.Value / Period.Value) * 100;
        }
        //Converts the percentage entered into a DutyCyle value
        private void Percentage_ValueChanged(object sender, EventArgs e)
        {
            DutyCycle.Value = Period.Value * (Percentage.Value / 100);
        }
        //Shows/Hides Advanced Options Fields/Parameters
        private void MagOption_CheckedChanged(object sender, EventArgs e)
        {
            if (MagOption.Checked)
                Add_Options();
            else
                Remove_Options();
        }
        //Updates visable duty cycle, period and percentage box based on which mag is selected
        private void MagComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Populate_Magnitude();
        }
    }
}
