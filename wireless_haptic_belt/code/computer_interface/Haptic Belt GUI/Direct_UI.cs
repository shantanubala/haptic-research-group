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
        //if we can Query the belt w/o conflict, we enter this mode
        //otherwise we stay in Select Mode.
        private void Show_Direct_Mode()
        {
            //Must be done before start, if error occurs go back
            if (Populate_AvailibleList() > 0)
            {
                //Send belt start command
                if (hasError(belt.StartHapticBelt(),"StartHapticBelt.()"))
                {
                    //Handle Error
                }
                DirectPanel.Show();
                //Load Saved Sets later?
            }
            else
            {
                Show_Select_Mode();
            }
        }
        //Adds a new activation to selected set, based on comboBox parameters
        private void DirectAddMotor_Click(object sender, EventArgs e)
        {
            Add_Activation(DirectRhythmBox.SelectedItem.ToString(), DirectMagBox.SelectedItem.ToString(), DirectCyclesBox.SelectedItem.ToString());
        }
        //Removes selected activation request from selected set
        private void DirectDeleteMotor_Click(object sender, EventArgs e)
        {
            Delete_Activation();
        }
        //Removes all activation request from the selected set
        private void DirectClearMotor_Click(object sender, EventArgs e)
        {
            Clear_Activation();
        }
        //Triggers stop button and goes back to Menu
        private void DirectBack_Click(object sender, EventArgs e)
        {
            //Stop All Motors before going back
            if (hasError(belt.StopAll(), "StopAll()"))
            {
                //Handle Error
            }
            //Go back to menu
            if (hasError(belt.ResetHapticBelt(),"ResetHapticBelt()"))
            {
                //Handle Error
            }
        }
        //Stops all motors from vibrating
        private void DirectStop_Click(object sender, EventArgs e)
        {
            //Stop All Motors
            if (hasError(belt.StopAll(),"StopAll()"))
            {
                //Handle Error
            }
        }
        //Renames a set with any characters in the Text Field.
        private void RenameSet_Click(object sender, EventArgs e)
        {
            if (SetList.SelectedIndex > -1)
            {
                SetList.Items.Insert(SetList.SelectedIndex, RenameSetField.Text);
                SetList.Items.RemoveAt(SetList.SelectedIndex);
            }
        }
        //Activates all motors in a set
        private void DirectActivate_Click(object sender, EventArgs e)
        {
            Activate_Set();
        }
        //Refreshes AvailableList and AddedList with new set contents
        private void SetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Change_Set();
        }
        //Changes the display labels on the GUI
        private void AddedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Change_Labels();
        }
    }
}
