using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            //Hide Rhythm Buttons so no interference will occur
            MagTest.Hide();
            ControlBox = false;
            MagDone.Enabled = false;

            //Set pattern to full on, 64 1's in binary, or 16 F's in hex
            String pattern = "FFFFFFFFFFFFFFFF";

            //Learn Rhythm to temp spot "H"
            if (hasError(belt.Learn_Rhythm("H", pattern, 64, false), "Learn_Rhythm()"))
            {
                //Handle Error
            }
            //Store the current Magnitude
            hold_magnitude = belt.getMagnitude("A", true, QueryType.SINGLE);
            if (hasError(belt.getStatus(), "getMagnitude()"))
            {
                //Handle Error
            }
            //Learn a the test Magnitude setting
            if (hasError(belt.Learn_Magnitude(MagComboBox.SelectedItem.ToString(), Convert.ToUInt16(Period.Value), Convert.ToUInt16(DutyCycle.Value)), "Learn Magnitude()"))
            {
                //Handle Error
            }
            //Get motor count
            _motorcount = belt.getMotors(QueryType.SINGLE);
            if (hasError(belt.getStatus(), "getMotors()"))
            {
                //Handle Error
            }
            //Vibrate all available motors on belt with test Rhythm and 100% Magnitude indefinately
            for (int i = 0; i < _motorcount; i++)
            {
                if (hasError(belt.Vibrate_Motor(i, "H", "A", 7), "Vibrate_Motor()"))
                {
                    //Handle Error
                }
            }
            //Wait for motors to finish vibrating or user to click "Stop" on MagTestStop Button
            MagTestStop.Show();
        }
        private void MagTestStop_Click(object sender, EventArgs e)
        {
            String[] split_magnitude = new String[2];
            //Issue a stop command to all motors on the belt
            if (hasError(belt.StopAll(), "belt.StopAll()"))
            {
                //Handle Error
            }
            //Reset original state of magnitude "A"
            split_magnitude = hold_magnitude.Split(',');
            if (hasError(belt.Learn_Magnitude("A", Convert.ToUInt16(split_magnitude[0]), Convert.ToUInt16(split_magnitude[1])), "Learn_Magnitude()"))
            {
                //Handle Error
            }
            //Reset button visability to original states
            ControlBox = true;
            MagDone.Enabled = true;
            MagTest.Show();
            MagTestStop.Hide();
        }
        //Calls Library function to learn Magnitude based on user values
        private void MagSet_Click(object sender, EventArgs e)
        {
            magnitude[MagComboBox.SelectedIndex].period = Convert.ToInt16(Period.Value);
            magnitude[MagComboBox.SelectedIndex].dutycycle = Convert.ToInt16(DutyCycle.Value);
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
