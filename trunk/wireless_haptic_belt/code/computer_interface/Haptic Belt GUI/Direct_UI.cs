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
                Swap_Motors(motorSwapingOnAllGroupsSetsMenu.Checked);
        }
        //Limits viewing to available motors only if checked
        private void showOnlyConnectedMotorsMenu_Click(object sender, EventArgs e)
        {
            if (showOnlyConnectedMotorsMenu.Checked)
                _viewableMotors = _motorcount;
            else
                _viewableMotors = _maxmotors;

            Change_Set();
        }
 

        private void DirectDelayField_ValueChanged(object sender, EventArgs e)
        {
            if (DirectDelayField.Value % 50 != 0)
                DirectDelayField.Value = Convert.ToInt32(DirectDelayField.Value) / 50 * 50;
        }
    }
}
