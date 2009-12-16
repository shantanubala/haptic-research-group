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
            Set_Activation(AddRhythmBox.SelectedItem.ToString(), AddMagBox.SelectedItem.ToString(), AddCyclesBox.SelectedItem.ToString(), Convert.ToInt16(AddDelayField.Value));
        }
        //Removes selected activation request from selected set
        private void DirectDeleteMotor_Click(object sender, EventArgs e)
        {
            Delete_Activation();
        }
        //Removes all activation request from the selected set
        private void DirectClearMotor_Click(object sender, EventArgs e)
        {
            Clear_Activations();
        }
        //Activate a single motor from AddedList
        private void DirectActivateMotor_Click(object sender, EventArgs e)
        {
            Activate_Motor();
        }
//Button Events: Sets: Add, Delete, Clear, Activate 
        private void DirectAddSet_Click(object sender, EventArgs e)
        {
            Add_Set();
        }

        private void DirectDeleteSet_Click(object sender, EventArgs e)
        {
            Delete_Set();
        }

        private void DirectClearSets_Click(object sender, EventArgs e)
        {
            Clear_Sets();
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

        private void DirectClearGroups_Click(object sender, EventArgs e)
        {
            Clear_Groups();
        }
        //Activates the selected group
        private void DirectActivateGroup_Click(object sender, EventArgs e)
        {
            Activate_Group();
        }      
//Other Button Events: OK's, Rename's, Stop
        private void RhythmEditOK_Click(object sender, EventArgs e)
        {
            if (GroupList.SelectedIndex > -1)
            {
                Set_Rhythms();
            }
            else
            {
                ErrorForm errorForm = new ErrorForm("You must have a group created and selected to use this function", "RhythmEditOK_Click()", false);
                errorForm.ShowDialog();
            } 
        }

        private void MagnitudeEditOK_Click(object sender, EventArgs e)
        {
            if (GroupList.SelectedIndex > -1)
            {
                Set_Magnitudes();
            }
            else
            {
                ErrorForm errorForm = new ErrorForm("You must have a group created and selected to use this function", "MagnitudeEditOK_Click()", false);
                errorForm.ShowDialog();
            } 
        }
        //Updates _motorCount, loads rhythms and magnitudes, and updates the version of the belt
        private void Initialize_Click(object sender, EventArgs e)
        {
            GetMotors();
            GetVersion();

            if(GroupList.SelectedIndex > -1)
                LoadBelt();
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
        //Assures that the value is a multiple of 50
        private void DirectDelayField_ValueChanged(object sender, EventArgs e)
        {
            if (AddDelayField.Value % 50 != 0)
                AddDelayField.Value = Convert.ToInt32(AddDelayField.Value) / 50 * 50;
        }
    }
}
