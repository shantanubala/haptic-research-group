using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HapticDriver;

namespace HapticGUI
{
    partial class MagnitudeForm
    {
        //Queries Library to get the Learned magnitudes, and populate fields
        private void Populate_Magnitude()
        {
            Period.Value = magnitude[MagComboBox.SelectedIndex].period;
            DutyCycle.Value = magnitude[MagComboBox.SelectedIndex].dutycycle;
            Percentage.Value = (DutyCycle.Value / Period.Value) * 100;
        }
        //Upholds the truth DutyCycle must be <= Period at all times, and update percentage
        private void Change_Period()
        {
            //make sure user cant enter a duty cycle > period 
            if (Period.Value < DutyCycle.Value)
            {
                DutyCycle.Value = Period.Value;
                Percentage.Value = 100;
            }
            else
            {
                Percentage.Value = (DutyCycle.Value / Period.Value) * 100;
            }
            DutyCycle.Maximum = Period.Value;
        }
        //Show Advanced Options Content
        private void Add_Options()
        {
            //Show advanced options (duty cycle and period)
            DutyCycle.Show();
            DutyLabel.Show();
            Period.Show();
            PeriodLabel.Show();
            PeriodDefaultLabel.Show();
        }
        //Hides Advanced Options Content
        private void Remove_Options()
        {
            //Hide advanced options (duty cycle and period)
            DutyCycle.Hide();
            DutyLabel.Hide();
            Period.Hide();
            PeriodLabel.Hide();
            PeriodDefaultLabel.Hide();
        }

        private void Test_Magnitude()
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

        private void Stop_Magnitude_Test()
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
    }
}
