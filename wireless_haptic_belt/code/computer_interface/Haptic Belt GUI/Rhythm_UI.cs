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
        private void Show_Rhythm_Mode()
        {
            //Initiates the Rhythm Panel by Clearing then Populating the 
            //selection box and painting the represented patern.
            Clear_Rhythm();
            Populate_Rhythm(RhythmComboBox.SelectedItem.ToString());
            Paint_Rythm();
            RhythmPanel.Show();
        }
        //Handles all necessary conditions to hide this panel
        private void Hide_Rhythm_Mode()
        {
            RhythmPanel.Hide();
        }
        //Sends user back to Select Mode (Main Menu)
        private void RhythmBack_Click(object sender, EventArgs e)
        {
            Hide_Rhythm_Mode();
            Show_Select_Mode();
        }
        //Calls the Library to learn the Rhythm specified by the listBox
        private void RhythmLearn_Click(object sender, EventArgs e)
        {
            if (pairs != 0)
            {
                String[] pattern = Get_Pattern().Split(',');

                if(hasError(belt.Learn_Rhythm(RhythmComboBox.SelectedItem.ToString(), pattern[0],Convert.ToInt16(pattern[1]),true),"Learn_Rhythm()"))
                {
                    //Handle Error
                }
            }
        }
        //Learns and plays Rhythm on belt using Rhythm H and Learns 100% Magnitude on setting A, and replaces A at the end
        //Note that we must use TimeSpan, so that we can replace the original Magnitude upon completion
        private void RhythmTest_Click(object sender, EventArgs e)
        {
            if (pairs != 0)
            {
                DateTime start;
                DateTime now;
                String hold_magnitude;
                String[] split_magnitude = new String[2];
                //Hide Rhythm Buttons so no interference will occur
                RhythmTest.Hide();
                RhythmBack.Hide();
                RhythmLearn.Hide();

                //Get the User Inputed Pattern
                String[] pattern = Get_Pattern().Split(',');
                
                //Learn the test Rhythm to temp spot "H"
                if(hasError(belt.Learn_Rhythm("H", pattern[0],Convert.ToInt16(pattern[1]),true),"Learn_Rhythm()"))
                {
                    //Handle Error
                }

                //Store the current Magnitude
                hold_magnitude = belt.getMagnitude("A",true, QueryType.SINGLE);
                if(hasError(belt.getError(),"getMagnitude()"))
                {
                    //Handle Error
                }
                //Learn a 100% Magnitude setting
                if(hasError(belt.Learn_Magnitude("A",100),"Learn_Magnitude()"))
                {
                    //Handle Error
                }
                //Vibrate all available motors on belt with test Rhythm and 100% Magnitude and 7 cycles
                for(int i = 0; i < 16; i++)
                {
                    //Ignore the error of a motor not being found
                    error_t[] ignore = {error_t.ENOMOTOR};
                    if(hasError(belt.Vibrate_Motor(i,"H","A",7),"Vibrate_Motor()",ignore))
                    {
                        //Handle Error
                    }
                }
                //Wait for motors to finish vibrating or user to click "Stop" on RhythmTestStop Button
                RhythmTestStop.Show();
                
                //Note: 1 Tick in Timespan(long ticks) = 100ns. Thus 1ms = 10000 ticks.
                //Timespan wait = new TimeSpan(Rhythm Length(ms)*10000(ticks/ms)*cycles)
                wait = new TimeSpan(Convert.ToInt16(pattern[1])*10000*7);
                start = DateTime.Now;
                now = DateTime.Now;
                while (now - start < wait)
                {
                    //This function allows user clicks to still register while in this while loop
                    Application.DoEvents();
                    now = DateTime.Now;
                }
                //Issue a stop command to all motors on the belt
                if(hasError(belt.StopAll(), "belt.StopAll()"))
                {
                    //Handle Error
                }
                //Reset original state of magnitude "A"
                split_magnitude = hold_magnitude.Split(',');
                if(hasError(belt.Learn_Magnitude("A",Convert.ToUInt16(split_magnitude[0]),Convert.ToUInt16(split_magnitude[1])),"Learn_Magnitude()"))
                {
                    //Handle Error
                }
                //Reset button visability to original states
                RhythmTest.Show();
                RhythmBack.Show();
                RhythmLearn.Show();
                RhythmTestStop.Hide();
            }            
        }
        private void RhythmTestStop_Click(object sender, EventArgs e)
        {
            //This sets wait = 0, breaking from the while loop prematurely in RhythmTest_Click().
            wait -= wait;
        }
        //Changes the PatternBox, called only upon changes in the ComboBox
        private void RhythmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear_Rhythm();
            Populate_Rhythm(RhythmComboBox.SelectedItem.ToString());
            Paint_Rythm();
        }
        //When a change occurs, we want to place markers around selected pattern
        private void RhythmPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            Paint_Rythm();
        }  

        //Primary paint function auto-called on startup, also screen changes. 
        private void RhythmPaint_Paint(object sender, PaintEventArgs e)
        {
            Paint_Rythm();
        }
        //Appends new value to the end, and repaints the graphics
        private void RhythmAdd_Click(object sender, EventArgs e)
        {
            Add_Pair(Convert.ToInt32(RhythmOn.Value), Convert.ToInt32(RhythmOff.Value));
            Paint_Rythm();
        }
        //Inserts pair before selected index, and repairs the graphics
        private void RhythmInsert_Click(object sender, EventArgs e)
        {
            Insert_Pair(RhythmPatternList.SelectedIndex, Convert.ToInt32(RhythmOn.Value), Convert.ToInt32(RhythmOff.Value));
            Paint_Rythm();
        }
        //Removes all rhythms, and paints the Cleared Rhythm
        private void RhythmClear_Click(object sender, EventArgs e)
        {
            Clear_Rhythm();
            Paint_Rythm();
        }

        private void RhythmReplace_Click(object sender, EventArgs e)
        {
            Replace_Pair(RhythmPatternList.SelectedIndex, Convert.ToInt32(RhythmOn.Value), Convert.ToInt32(RhythmOff.Value));
            Paint_Rythm();
        }

        private void RhythmDelete_Click(object sender, EventArgs e)
        {
            Delete_Pair(RhythmPatternList.SelectedIndex);
            Paint_Rythm();
        }
    }
}
