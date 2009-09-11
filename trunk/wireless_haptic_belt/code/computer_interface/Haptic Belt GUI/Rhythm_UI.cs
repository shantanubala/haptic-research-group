using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HapticBelt
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
ErrorStatus.Text = "Error Status: " + "Waiting for Learn_Rhythm() to respond";
ErrorLocation.Text = "Error Location: " + "Calling Learn_Rhythm()";
                response = belt.Learn_Rhythm(RhythmComboBox.SelectedItem.ToString(), Get_Pattern());
ErrorStatus.Text = "Error Status: " + response[0];
                if (!response[0].Equals(""))
                {
                    //ERROR
                }
                else
                {
ErrorLocation.Text = "Error Location: ";
                }
            }
        }
        //FIXME not yet implemented in the library/arduino
        private void RhythmTest_Click(object sender, EventArgs e)
        {
//          belt.Test_Rhythm();
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
