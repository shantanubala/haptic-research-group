using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HapticDriver;

/* Handle's both Direct Program Mode and Direct Operation Mode.
 * The reason for this is because of the similarities of the two modes.
 * The only difference between these two modes is the ability to interact
 * with the belt via the COM port, which Program Mode lacks. The reason
 * for this mode is to allow the GUI user to program motors and without
 * having to have the belt connected to the GUI.
 */
namespace HapticGUI
{
    partial class GUI
    {
        //Handles all necessary conditions to display this panel
        //if we can Query the belt w/o conflict, we enter this mode
        //otherwise we stay in Select Mode.
        private void Show_Operation_Mode()
        {
            //Must be done before start we allow user control.
            Initialize_Operation_Mode();
            DirectPanel.Show();
        }
        //Triggers stop button and goes back to Main Menu
        private void Hide_Operation_Mode()
        {
            //Stop All Motors before going back
            StopMotors();
            DirectPanel.Hide();
        }
        //Enters program mode
        private void Show_Program_Mode()
        {
            _viewableMotors = 16;

            //Set proper buttons for this mode since its shared with Operation mode
            DirectActivateGroup.Hide();
            DirectActivateSet.Hide();
            DirectActivateMotor.Hide();
            DirectStop.Hide();
            DirectShowOption.Hide();
            DirectShowOptionLabel.Hide();
            DirectProgramBack.Show();

            Initialize_Program_Mode();

            DirectPanel.Show();
        }
        //Goes back to Main Menu
        private void Hide_Program_Mode()
        {
            //Reset buttons back to original states since its shared with Operation Mode
            DirectActivateGroup.Show();
            DirectActivateSet.Show();
            DirectActivateMotor.Show();
            DirectStop.Show();
            DirectShowOption.Show();
            DirectShowOptionLabel.Show();
            DirectProgramBack.Hide();

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
        private void DirectSetMotor_Click(object sender, EventArgs e)
        {
            Set_Activation(DirectRhythmBox.SelectedItem.ToString(), DirectMagBox.SelectedItem.ToString(), DirectCyclesBox.SelectedItem.ToString(), Convert.ToInt16(DirectDelayField.Value));
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
        //Go back to menu from Operation Mode
        private void DirectOperationBack_Click(object sender, EventArgs e)
        {
            Hide_Operation_Mode();
            Show_Select_Mode();
        }
        //Go back to menu from Program Mode
        private void DirectProgramBack_Click(object sender, EventArgs e)
        {
            Hide_Program_Mode();
            Show_Select_Mode();
        } 
        //Stops all motors from vibrating
        private void DirectStop_Click(object sender, EventArgs e)
        {
            StopMotors();
        }
        //Renames a set with any characters in the Text Field.
        private void DirectRenameSet_Click(object sender, EventArgs e)
        {
            Rename_Set(); 
        }
        //Renames a set with any characters in the DirectRenameField (text field).
        private void DirectRenameGroup_Click(object sender, EventArgs e)
        {
            Rename_Group();
        }
//Selected Index Changed Events
        //Refreshes SetList, Available List and Added List according to the selected group
        private void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GroupList.SelectedIndices.Count == 1)
                Change_Group();
            else if (GroupList.SelectedIndices.Count == 2)
                Swap_Groups();
        }

        //Refreshes AvailableList and AddedList with according to the selected set
        private void SetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SetList.SelectedIndices.Count == 1)
                Change_Set();
            else if (SetList.SelectedIndices.Count == 2)
                Swap_Sets();
        }
        //Changes the display labels on the GUI
        private void MotorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MotorList.SelectedIndices.Count == 1)
                Change_Motor();
            else if (MotorList.SelectedIndices.Count == 2)
                Swap_Motors(DirectSwapOption.Checked);
        }
        //Limits viewing to available motors only if checked
        private void DirectSwapOption_CheckedChanged(object sender, EventArgs e)
        {
            if (DirectShowOption.Checked)
                _viewableMotors = _motorcount;
            else
                _viewableMotors = _maxmotors;

            Change_Set();
        }
    }
}
