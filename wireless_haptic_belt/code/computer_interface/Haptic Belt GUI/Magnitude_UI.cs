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
    partial class GUI
    {
        //Handles all necessary conditions to display this panel 
        private void Show_Magnitude_Mode()
        {
            Populate_Magnitude(MagComboBox.SelectedItem.ToString());
            MagPanel.Show();
        }
        //Handles all necessary conditions to hide this panel
        private void Hide_Magnitude_Mode()
        {
            MagPanel.Hide();
        }
        //Displays the Selection Mode Panel (Main Menu)
        private void MagBack_Click(object sender, EventArgs e)
        {
            Hide_Magnitude_Mode();
            Show_Select_Mode();
        }
        //Learns and plays Magnitude on belt using Magnitude "A", replaces original Magnitude "A" when finished. Learns 100% on Rhythm onto the belt memory "H" as well.
        //Note that we must use TimeSpan, so that we can replace the original Magnitude upon completion
        private void MagTest_Click(object sender, EventArgs e)
        {
            if (pairs != 0)
            {
                DateTime start;
                DateTime now;
                String hold_magnitude;
                String[] split_magnitude = new String[2];
                //Hide Rhythm Buttons so no interference will occur
                MagTest.Hide();
                MagBack.Hide();
                MagLearn.Hide();

                //Set pattern to full on, 64 1's in binary, or 16 F's in hex
                String pattern = "FFFFFFFFFFFFFFFF";

                //Learn Rhythm to temp spot "H"
                if (hasError(belt.Learn_Rhythm("H", pattern, 64, false), "Learn_Rhythm()"))
                {
                    //Handle Error
                }
                //Store the current Magnitude
                hold_magnitude = belt.getMagnitude("A", true, QueryType.SINGLE);
                if (hasError(belt.getError(), "getMagnitude()"))
                {
                    //Handle Error
                }
                //Learn a the test Magnitude setting
                if (hasError(belt.Learn_Magnitude(MagComboBox.SelectedItem.ToString(), Convert.ToUInt16(Period.Value), Convert.ToUInt16(DutyCycle.Value)), "Learn Magnitude()"))
                {
                    //Handle Error
                }
                //Vibrate all available motors on belt with test Rhythm and 100% Magnitude and 7 cycles
                for (int i = 0; i < 16; i++)
                {
                    //Ignore the error of a motor not being found
                    error_t[] ignore = { error_t.ENOMOTOR };
                    if (hasError(belt.Vibrate_Motor(i, "H", "A", 7), "Vibrate_Motor()", ignore))
                    {
                        //Handle Error
                    }
                }
                //Wait for motors to finish vibrating or user to click "Stop" on RhythmTestStop Button
                MagTestStop.Show();

                //Note: 1 Tick in Timespan(long ticks) = 100ns. Thus 1ms = 10000 ticks.
                //Timespan wait = new TimeSpan(Rhythm Length(ms)*10000(ticks/ms)*cycles)
                wait = new TimeSpan(Convert.ToInt16(pattern[1]) * 10000 * 7);
                start = DateTime.Now;
                now = DateTime.Now;
                while (now - start < wait)
                {
                    //This function allows user clicks to still register while in this while loop
                    Application.DoEvents();
                    now = DateTime.Now;
                }
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
                MagTest.Show();
                MagBack.Show();
                MagLearn.Show();
                MagTestStop.Hide();
            }
        }
        private void MagTestStop_Click(object sender, EventArgs e)
        {
            //This sets wait = 0, breaking from the while loop prematurely in RhythmTest_Click().
            wait -= wait;
        }
        //Calls Library function to learn Magnitude based on user values
        private void MagLearn_Click(object sender, EventArgs e)
        {
            //Learn Magnitude
            if(hasError(belt.Learn_Magnitude(MagComboBox.SelectedItem.ToString(), Convert.ToUInt16(Period.Value), Convert.ToUInt16(DutyCycle.Value)),"Learn Magnitude()"))
            {
                //Handle Error
            }
        }
        //Duty Cycle can be at most the value of Period, we must uphold this here
        private void Period_ValueChanged(object sender, EventArgs e)
        {
            Change_Maximum_DutyCycle();         
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
            {
                Show_Options();
            }
            else
            {
                Hide_Options();
            }
        }
    }
}
