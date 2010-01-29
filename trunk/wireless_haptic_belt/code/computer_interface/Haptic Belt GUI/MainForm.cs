using System;
using System.Threading;
using System.Windows.Forms;
using HapticDriver;

/* MainForm - Handles all the user actions by calling the appropriate functions in MainForm_A or MainForm_F.
 */
namespace HapticGUI
{
    partial class GUI
    {
 //Button Events: Activations: Add, Delete, Clear
        //Adds a new activation to selected set, based on comboBox parameters
        private void SetActivation_Click(object sender, EventArgs e)
        {
            if(ActivationList.SelectedIndex > -1)
                Set_Activation(AddRhythmBox.SelectedIndex, AddMagBox.SelectedIndex, AddCyclesBox.SelectedIndex);
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
//Button Events: Events: Add, Delete, Clear 
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
//Button Events: Groups: Add, Delete, Clear   
        private void AddGroup_Click(object sender, EventArgs e)
        {
            Add_Group();
        }

        private void DeleteGroup_Click(object sender, EventArgs e)
        {
            Delete_Group();
        }

        private void ClearGroups_Click(object sender, EventArgs e)
        {
            Clear_Groups();
        } 
//Other Button Events: OK's, Rename's, Stop
        private void Configure_Click(object sender, EventArgs e)
        {
            if (GroupList.SelectedIndex > -1)
            {
                Configure_Motors();
                ActivationList.Items.Clear();
            }
        }
        
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
        
//Activations and Stop functions on threads
        //Create a new thread to activate a single motor from ActivationList
        private void ActivateActivation_Click(object sender, EventArgs e)
        {
            if (!activate_trd.IsAlive && ActivationList.SelectedIndex > -1) //Prevents multiple activation commands being issued
            {
                activate_trd = new Thread(new ThreadStart(this.Activate_Activation));
                activate_trd.Start();
            }
        }

        private void ActivateEvent_Click(object sender, EventArgs e)
        {
            if (!activate_trd.IsAlive && EventList.SelectedIndex > -1) //Prevents multiple activation commands being issued
            {
                activate_trd = new Thread(new ThreadStart(this.Activate_Event));
                activate_trd.Start();
            }
        }

        //Create a new thread to activate the selected group from GroupList
        private void ActivateGroup_Click(object sender, EventArgs e)
        {
            if (!activate_trd.IsAlive && GroupList.SelectedIndex > -1) //Prevents multiple activation commands being issued
            {
                activate_trd = new Thread(new ThreadStart(this.Activate_Group));
                activate_trd.Start();
            }
        }

        //Stops all motors from vibrating
        private void Stop_Click(object sender, EventArgs e)
        {
            if (!stop_trd.IsAlive && activate_trd.IsAlive) //Prevents multiple stop commands being issued
            {
                stop_trd = new Thread(new ThreadStart(this.Stop_Activations));
                stop_trd.Start();
            }
        }

        //Renames a group with any characters in the RenameField (text field).
        private void RenameGroup_Click(object sender, EventArgs e)
        {
            Rename_Group();
        }
//Selected Index Changed Events
        private void ActivationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActivationList.SelectedIndices.Count == 1)
                Change_Activation();
            else if (ActivationList.SelectedIndices.Count == 2)
                Swap_Activations();
        }

        private void EventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EventList.SelectedIndices.Count == 0)
                ActivationList.Items.Clear();
            else if (EventList.SelectedIndices.Count == 1)
                Change_Event();
            else if (EventList.SelectedIndices.Count == 2)
                Swap_Events();
        }
        
        //Refreshes EventList and ActiavtionList according to the selected group
        private void GroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GroupList.SelectedIndices.Count == 0)
            {
                ActivationList.Items.Clear();
                EventList.Items.Clear();
            }
            else if (GroupList.SelectedIndices.Count == 1)
                Change_Group();
            else if (GroupList.SelectedIndices.Count == 2)
                Swap_Groups();
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
        private void DelayField_ValueChanged(object sender, EventArgs e)
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
