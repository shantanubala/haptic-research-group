using System;
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
 //Button Events: Activations: Add, Delete, Clear, Activate
        //Adds a new activation to selected set, based on comboBox parameters
        private void SetActivation_Click(object sender, EventArgs e)
        {
            if(AddMotorBox.SelectedIndex > -1 && ActivationList.SelectedIndex > -1)
                Set_Activation(AddMotorBox.SelectedIndex, AddRhythmBox.SelectedIndex, AddMagBox.SelectedIndex, AddCyclesBox.SelectedIndex);
        }
        //Removes selected activation request from selected set
        private void DeleteActivation_Click(object sender, EventArgs e)
        {
            Delete_Activation();
        }
        //This event is deleted, since no activations are present it cannot exist
        private void ClearActivation_Click(object sender, EventArgs e)
        {
            Delete_Event();
        }
        
        //Activate a single motor from ActivationList
        private void ActivateActivation_Click(object sender, EventArgs e)
        {
            Activate_Motor();
        }
//Button Events: Events: Add, Delete, Clear, Activate 
        private void AddEvent_Click(object sender, EventArgs e)
        {
            if(AddMotorBox.SelectedIndex > -1)
                Add_Event(AddMotorBox.SelectedIndex ,AddRhythmBox.SelectedIndex, AddMagBox.SelectedIndex, AddCyclesBox.SelectedIndex, Convert.ToInt32(DelayField.Value));
        }

        private void DeleteEvent_Click(object sender, EventArgs e)
        {
            Delete_Event();
        }

        private void ClearEvent_Click(object sender, EventArgs e)
        {
            Clear_Events();
        }

        private void ActivateEvent_Click(object sender, EventArgs e)
        {
            Activate_Event();
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

        //Renames a set with any characters in the DirectRenameField (text field).
        private void RenameGroup_Click(object sender, EventArgs e)
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

        //Changes the display labels on the GUI
        private void EventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Change_Event();
        }
        //Limits viewing to available motors only if checked
        private void showOnlyConnectedMotorsMenu_Click(object sender, EventArgs e)
        {
            if (showOnlyConnectedMotorsMenu.Checked)
                _viewableMotors = _motorcount;
            else
                _viewableMotors = _maxmotors;

            //Refresh AddMotorBox items
            AddMotorBox.Items.Clear();

            for(int i = 0; i < _viewableMotors; i++)
                AddMotorBox.Items.Add((i + 1).ToString());

            Change_Event();
        }
        //Assures that the value is a multiple of 50
        private void DirectDelayField_ValueChanged(object sender, EventArgs e)
        {
            if (DelayField.Value % 50 != 0)
                DelayField.Value = Convert.ToInt32(DelayField.Value) / 50 * 50;
        }

        private void GroupRepeatField_ValueChanged(object sender, EventArgs e)
        {
            if (GroupList.SelectedIndex > -1)
            {
                _group[_current_group].cycles = Convert.ToInt32(RepetitionsField.Value);
            }
        }   
    }
}
