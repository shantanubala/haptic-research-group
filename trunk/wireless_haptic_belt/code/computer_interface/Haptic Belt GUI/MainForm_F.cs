using System;
using System.Data;
using HapticDriver;


namespace HapticGUI
{
    partial class GUI
    {
        int _current_activation = 0;
        int _current_event = 0;
        int _current_group = 0;

        //These two variables are similar in use however, seperate so that upon activating, we get no performance loss
        //Otherwise if we used the same variable would send activations to unattached motors.
        int _motorcount = 0;
        int _viewableMotors = 0;

        const int _maxmotors = 16; //Max amount of motors possible for this application
        const int _maxrhythms = 5; //Max amount of rhythms possible for this application
        const int _maxmagnitudes = 4; //Max amount of magnitudes possible for this applications

        private void Configure_Motors()
        {
            ConfigForm configForm = new ConfigForm(_group[_current_group].motors);
            configForm.ShowDialog();
            _group[_current_group].motors = configForm.getMotors();
            //Now we need to match the motors[] activations with the events[] activations
            _group[_current_group].refreshEvents();
        }


        private void Set_Rhythms()
        {
            if (EditBox.Text.Equals("Edit"))
            {
                RhythmForm rhythmForm = new RhythmForm(_group[_current_group].rhythm, belt, Port_Open);
                rhythmForm.ShowDialog();
                _group[_current_group].rhythm = rhythmForm.getRhythms();
            }
            else
            {
                _group[_current_group].rhythm = (Rhythm[])_group[EditBox.SelectedIndex].rhythm.Clone();
            }
            
        }

        private void Set_Magnitudes()
        {
            if (EditBox.Text.Equals("Edit"))
            {
                MagnitudeForm magnitudeForm = new MagnitudeForm(_group[_current_group].magnitude, belt, Port_Open);
                magnitudeForm.ShowDialog();
                _group[_current_group].magnitude = magnitudeForm.getMagnitudes();
            }
            else
            {
                _group[_current_group].magnitude = (Magnitude[])_group[EditBox.SelectedIndex].magnitude.Clone();
            }
        }

//Functions that are called from selections in ListBoxes
        //Change File - called when a file is loaded
        private void Change_File()
        {
            EventList.Items.Clear();
            ActivationList.Items.Clear();
            GroupList.Items.Clear();
            for (int i = 0; i < _group.Length; i++)
                GroupList.Items.Add(_group[i].name);
        }


        //Populates 3 lables based on selected motor in "AddedList"
        private void Change_Activation()
        {
            if (ActivationList.SelectedIndex > -1)
            {
                _current_activation = ActivationList.SelectedIndex;

                //Change comboBoxes
                AddMotorBox.SelectedIndex = _group[_current_group].events[_current_event].activations[_current_activation].motor;
                AddRhythmBox.SelectedIndex = _group[_current_group].events[_current_event].activations[_current_activation].rhythm;
                AddMagBox.SelectedIndex = _group[_current_group].events[_current_event].activations[_current_activation].magnitude;
                AddCyclesBox.SelectedIndex = _group[_current_group].events[_current_event].activations[_current_activation].cycles;
                DelayField.Value = _group[_current_group].events[_current_event].activations[_current_activation].delay; //== _group[_current_group].events[_current_event].time
            }
        }

        private void Change_Event()
        {
            Activation[] selected;

            if (EventList.SelectedIndex > -1)
            {
                _current_event = EventList.SelectedIndex;

                DelayField.Value = _group[_current_group].events[_current_event].time;
                
                selected = _group[_current_group].events[_current_event].activations; 

                ActivationList.Items.Clear();
                //Populate Availible Motor List for selected set
                for (int i = 0; i < selected.Length; i++) // Motor,Rhythm,Magnitude,Cycles
                {
                    String label = "";
                    if (selected[i].motor < _viewableMotors) //If we are able to view this motor display it
                    {
                        label = ((int)selected[i].motor + 1).ToString() + "," + ((char)((int)selected[i].rhythm + 65)).ToString() + "," + ((char)((int)selected[i].magnitude + 65)).ToString();
                        if (selected[i].cycles == 0)
                            label = label + ",Stop";
                        else if (selected[i].cycles == 7)
                            label = label + ",Inf";
                        else
                            label = label + "," + selected[i].cycles.ToString();
                        ActivationList.Items.Add(label);
                    }
                }
            }
        }

        //Re-populates ActivationList from selected Group, also learns rhythms/mags onto belt if connected
        private void Change_Group()
        {
            if (GroupList.SelectedIndex > -1)
            {
                _current_group = GroupList.SelectedIndex;

                //Display the Cycles value in the RepitionsField for the selected group
                RepetitionsField.Value = _group[_current_group].cycles;

                EventList.Items.Clear();
                ActivationList.Items.Clear();

                for (int i = 0; i < _group[_current_group].events.Length; i++)
                    EventList.Items.Add(_group[_current_group].events[i].time);

                //Learn Magnitudes and Rhythms onto belt
                LoadBelt();
            } 
        }

        //Swaps the two selected Activations primarily just for organization
        private void Swap_Activations()
        {
            int swapA, swapB;
            Activation store;
          
            //Store First Selected Index and deselect
            swapA = ActivationList.SelectedIndex;
            ActivationList.SelectedIndices.Remove(swapA);

            //Store Second Selected Index and deselect
            swapB = ActivationList.SelectedIndex;
            ActivationList.SelectedIndices.Remove(swapB);

            //Swap rhythm's, magnitude's, and cycle's in events[] array manually because it is simple the way the data structure is set up. 
            //Use store to hold set[swapA]'s data. Then switch data.
            store = new Activation(_group[_current_group].events[_current_event].activations[swapA]);
            //Set swapA
            _group[_current_group].events[_current_event].activations[swapA].rhythm = _group[_current_group].events[_current_event].activations[swapB].rhythm;
            _group[_current_group].events[_current_event].activations[swapA].magnitude = _group[_current_group].events[_current_event].activations[swapB].magnitude;
            _group[_current_group].events[_current_event].activations[swapA].cycles = _group[_current_group].events[_current_event].activations[swapB].cycles;
            //Set swapB
            _group[_current_group].events[_current_event].activations[swapB].rhythm = store.rhythm;
            _group[_current_group].events[_current_event].activations[swapB].magnitude = store.magnitude;
            _group[_current_group].events[_current_event].activations[swapB].cycles = store.cycles;
            
            //Swap in motors[] activation list, note the use of the already set Activation in the events[] array, to update the motors[] array.
            //Note when adding an activation in a motors[] array with the same delay attribute it replaces the old activation.
            _group[_current_group].motors[(_group[_current_group].events[_current_event].activations[swapA].motor)].addActivation(new Activation(_group[_current_group].events[_current_event].activations[swapA]));
            _group[_current_group].motors[(_group[_current_group].events[_current_event].activations[swapB].motor)].addActivation(new Activation(_group[_current_group].events[_current_event].activations[swapB]));

            //Change Event to update the ActivationList changes from the newly updated datastructure
            Change_Event();
        }

        private void Swap_Events()
        {
            int swapA, swapB;
            Activation[] store;

            //Store First Selected Index and deselect
            swapA = EventList.SelectedIndex;
            EventList.SelectedIndices.Remove(swapA);

            //Store Second Selected Index and deselect
            swapB = EventList.SelectedIndex;
            EventList.SelectedIndices.Remove(swapB);

            //Store A's activations into Store, a temp variable
            store = (Activation[])_group[_current_group].events[swapA].activations.Clone();
            //Change A's activations over to B's
            _group[_current_group].events[swapA].activations = (Activation[])_group[_current_group].events[swapB].activations.Clone();
            //Match each newly swapped activation to its event time, and delete the correspoing old event from motors[]
            for (int i = 0; i < _group[_current_group].events[swapA].activations.Length; i++)
            {
                //Remove from motors array
                _group[_current_group].motors[(_group[_current_group].events[swapA].activations[i].motor)].removeActivation(_group[_current_group].events[swapA].time);
                //Match delay to the newly swapped event
                _group[_current_group].events[swapA].activations[i].delay = _group[_current_group].events[swapA].time;
                //Add to motors array after updating delay
                _group[_current_group].motors[(_group[_current_group].events[swapA].activations[i].motor)].addActivation(new Activation(_group[_current_group].events[swapA].activations[i]));

            }

            //Change B's activations over to A's (via Store)
            _group[_current_group].events[swapB].activations = store;
            //Match each newly swapped activation to its event time, and delete the correspoing old event from motors[]
            for (int i = 0; i < _group[_current_group].events[swapB].activations.Length; i++)
            {
                //Remove from motors array
                _group[_current_group].motors[(_group[_current_group].events[swapB].activations[i].motor)].removeActivation(_group[_current_group].events[swapB].time);
                //Match delay to the newly swapped event
                _group[_current_group].events[swapB].activations[i].delay = _group[_current_group].events[swapB].time;
                //Add to motors array after updating delay
                _group[_current_group].motors[(_group[_current_group].events[swapB].activations[i].motor)].addActivation(new Activation(_group[_current_group].events[swapB].activations[i]));
            }

            //Change Group to update the EventList changes from the newly updated datastructure
            Change_Group();
        }

        //Swaps the two selected groups
        private void Swap_Groups()
        {
            int swapA, swapB;
            Group store;

            //Store First Selected Index and deselect
            swapA = GroupList.SelectedIndex;
            GroupList.SelectedIndices.Remove(swapA);

            //Store Second Selected Index and deselect
            swapB = GroupList.SelectedIndex;
            GroupList.SelectedIndices.Remove(swapB);

            //Use store to hold set[swapA]'s data. Then switch data.
            store = new Group(_group[swapA]);
            _group[swapA] = new Group(_group[swapB]);
            _group[swapB] = store;

            //Refreshes GroupList
            GroupList.Items.Clear();
            EventList.Items.Clear();
            ActivationList.Items.Clear();
            //Populate GroupList based on newly selected group
            for (int i = 0; i < _group.Length; i++)
                GroupList.Items.Add(_group[i].name);
        }

//Functions on Activations
        //Adds an activation request to the selected set
        private void Set_Activation(int rhythm, int mag, int cycles)
        {
            String label;
            Activation selected;
            //must have a selected index
            if (EventList.SelectedIndex > -1)
            {
                //Set delay field to the current selected event time
                DelayField.Value = _group[_current_group].events[_current_event].time;
                
                //Replace motor to the activation chain
                _group[_current_group].addActivation(_group[_current_group].events[_current_event].activations[_current_activation].motor, rhythm, mag, cycles, _group[_current_group].events[_current_event].time);

                ActivationList.Items.RemoveAt(_current_activation);

                selected = _group[_current_group].events[_current_event].activations[_current_activation];

                label = ((int)selected.motor + 1).ToString() + "," + ((char)((int)selected.rhythm + 65)).ToString() + "," + ((char)((int)selected.magnitude + 65)).ToString();
                if (selected.cycles == 0)
                    label = label + ",Stop";
                else if (selected.cycles == 7)
                    label = label + ",Inf";
                else
                    label = label + "," + selected.cycles.ToString();

                ActivationList.Items.Insert(_current_activation, label);

                //Reselects the index, and updates labels through MotorListSelectedIndexChanged Event
                ActivationList.SelectedIndex = _current_activation;
            }
        }
        //Deletes selected activation request from selected set
        private void Delete_Activation()
        {
            //must have a selected index
            if (ActivationList.SelectedIndex > -1 && EventList.SelectedIndex > -1)
            {
                //Delete motor from activation data
                _group[_current_group].removeActivation(_current_event, _current_activation);

                //Set motor to OFF position
                ActivationList.Items.RemoveAt(_current_activation);
            }
        }
       
//Functions on events
        private void Add_Event(int motor, int rhythm, int mag, int cycles, int delay)
        {
            //must have a selected index
            if (GroupList.SelectedIndex > -1)
            {
                //Add motor to the activation chain, adds event if needed automatically too
                _group[_current_group].addActivation(motor, rhythm, mag, cycles, delay);
                
                //Searches through event list and inserts new event at proper spot
                if (!EventList.Items.Contains(delay))
                {
                    for (int index = 0; index < _group[_current_group].events.Length; index++)
                    {
                        int value = Convert.ToInt32(DelayField.Value);

                        if (_group[_current_group].events[index].time == value)
                        {
                            EventList.Items.Insert(index, value);
                            break;
                        }
                    }
                }

                Change_Event();
            }
        }
        
        //Deletes the selected event (aka clears all activations)
        private void Delete_Event()
        {
            if (EventList.SelectedIndex > -1)
            {
                _group[_current_group].deleteEvent(_current_event);
                //Remove Selected Item to SetList
                EventList.Items.RemoveAt(_current_event);
                //Clear out ActivationList
                ActivationList.Items.Clear();
            }
        }
        //Deletes all events
        private void Clear_Events()
        {
            _group[_current_group].clearEvents();

            //Clear out EventList, and ActivationList
            EventList.Items.Clear();
            ActivationList.Items.Clear();
        }

//Functions on groups
        //Adds a group with the inputed name parameter from DirectRenameField
        private void Add_Group()
        {
            if (!RenameField.Text.Equals(""))
            {
                _group = increaseGroup(_group, RenameField.Text);
                //Add item to GroupList
                GroupList.Items.Add(RenameField.Text);

                RenameField.Clear();

                //Update EditComboBox
                //Clear ComboBox
                EditBox.Items.Clear();

                //Populate ComboBox
                for (int i = 0; i < _group.Length; i++)
                    EditBox.Items.Add(_group[i].name);

                EditBox.Items.Add("Edit");
            }
            else
            {
                ErrorForm errorForm = new ErrorForm("Name field blank, you must have a name for each group", "Add_Group()", false);
                errorForm.ShowDialog();
            }
        }
        //Deletes selected group
        private void Delete_Group()
        {
            if (GroupList.SelectedIndex > -1)
            {
                _group = decreaseGroup(_group, GroupList.SelectedIndex);
                //Remove Selected Item from GroupList
                GroupList.Items.RemoveAt(GroupList.SelectedIndex);
                //Clear out EventList and ActivationList
                EventList.Items.Clear();
                ActivationList.Items.Clear();
            }
        }
        //Deletes all groups
        private void Clear_Groups()
        {
            //Initialize group->set->motor data structures
            _group = new Group[0];

            //Clear all ListBoxes
            ActivationList.Items.Clear();
            EventList.Items.Clear();
            GroupList.Items.Clear();
        }
        
        //Other Functions: Stop and Rename button functions
        //Issues a break to stop all activations, then waits for activations to stop
        //Note that if we don't wait till we are done activating we can issue a stop command
        //and then have another activation come along and start them back up!
        private void GetMotors()
        {
            _motorcount = belt.getMotors(QueryType.SINGLE);
            if (hasError(belt.getStatus(), "belt.getMotors()"))
            {
                //Handle Error
            }
        }

        private void GetVersion()
        {
            firmwareVersionMenu.Text = belt.getVersion(QueryType.SINGLE);
            if (hasError(belt.getStatus(), "belt.getVersion()"))
            {
                //Handle Error
            }
        }

        private void LoadBelt()
        {
            //Group selected when connected, load current group into belt
            if (Port_Open)
            {
                Magnitude[] magnitude = _group[_current_group].magnitude;
                Rhythm[] rhythm = _group[_current_group].rhythm;
                String id;
                for (int i = 0; i < 5; i++)
                {
                    id = ((char)(i + 65)).ToString();
                    //Last parameter true means "pattern" is in binary string format (0's and 1's only)
                    belt.Learn_Rhythm(id, rhythm[i].pattern, rhythm[i].time, true);
                    if (i != 4)
                        belt.Learn_Magnitude(id, magnitude[i].period, magnitude[i].dutycycle);

                }
            }
        }

        //Renames the selected group
        private void Rename_Group()
        {
            if (GroupList.SelectedIndex > -1 && !RenameField.Text.Equals(""))
            {
                int index;
                //Record this change in the Data Structure
                _group[_current_group].name = RenameField.Text;

                //Update GroupList Item's list
                GroupList.Items.Insert(GroupList.SelectedIndex, RenameField.Text);
                index = GroupList.SelectedIndex - 1;
                GroupList.Items.RemoveAt(GroupList.SelectedIndex);

                //Clear the DirectRenameField.Text
                RenameField.Clear();

                //Reset the SelectedIndex
                GroupList.SelectedIndex = index;
            }
        }
    }
}