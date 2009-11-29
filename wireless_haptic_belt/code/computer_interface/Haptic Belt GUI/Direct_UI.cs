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
            //Must be done before start we allow user control.
            Initialize_DirectMode();

            DirectPanel.Show();
            
            //Load Saved Sets later?
        }
        //Triggers stop button and goes back to Menu
        private void Hide_Direct_Mode()
        {
            //Stop All Motors before going back
            StopMotors();

            DirectPanel.Hide();
        }
//Button Events: Save and Load
        private void DirectLoad_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void DirectSave_Click(object sender, EventArgs e)
        {
            //TODO
        } 
 //Button Events: Motors: Add, Delete, Clear, Activate
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
        //Activate a single motor from AddedList
        private void DirectActivateMotor_Click(object sender, EventArgs e)
        {
            Activate_Motor();
        }
//Button Events: Sets: Add, Delete, Clear, Activate 
        //Adds a new set to the selected group
        private void DirectAddSet_Click(object sender, EventArgs e)
        {
            Add_Set();
        }

        private void DirectDeleteSet_Click(object sender, EventArgs e)
        {
            Delete_Set();
        }

        private void DirectClearSet_Click(object sender, EventArgs e)
        {
            Clear_Set();
        }

        //Activates all motors in a set from a selected group
        private void DirectActivateSet_Click(object sender, EventArgs e)
        {
            Activate_Set();
        }

//Button Events: Groups: Add, Delete, Clear, Activate    
        private void DirectAddGroup_Click(object sender, EventArgs e)
        {
            Add_Group();
        }

        private void DirectDeleteGroup_Click(object sender, EventArgs e)
        {
            Delete_Group();
        }

        private void DirectClearGroup_Click(object sender, EventArgs e)
        {
            Clear_Group();
        }

        private void DirectActivateGroup_Click(object sender, EventArgs e)
        {
            Activate_Group();
        }      
//Other Button Events
        //Go back to menu
        private void DirectBack_Click(object sender, EventArgs e)
        {
            Hide_Direct_Mode();
        }
        //Stops all motors from vibrating
        private void DirectStop_Click(object sender, EventArgs e)
        {
            StopMotors();
        }
        //Renames a set with any characters in the Text Field.
        private void DirectRenameSet_Click(object sender, EventArgs e)
        {
            RenameSet(); 
        }
        //Renames a set with any characters in the DirectRenameField (text field).
        private void DirectRenameGroup_Click(object sender, EventArgs e)
        {
            RenameGroup();
        }
//Selected Index Changed Events
        //Refreshes SetList, Available List and Added List according to the selected group
        private void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Change_Group();
        }
        //Refreshes AvailableList and AddedList with according to the selected set
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
