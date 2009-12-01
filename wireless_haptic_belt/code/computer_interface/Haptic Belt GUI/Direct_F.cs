using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using HapticDriver;

/*TODO
 * 
 * Display Error Prompts in Main.
 * Display Error Prompt if Name Field is empty when trying to ADD/RENAME
 * 
 * 
 * 
 * 
 * 
 * 
 */



namespace HapticGUI
{
    partial class GUI
    {
        Group[] _group;
        int _current_set = 0;
        int _current_group = 0;
        bool _done_activating = true; // Used to make sure we are done activating motors before issuing a stop command
        bool _stop = false; //Used to break activation loops, (_done_activating should be set to true after breaking out of these loops)
        
        //These two variables are similar in use however, seperate so that upon activating, we get no performance loss
        //Otherwise if we used the same variable would send activations to unattached motors.
        int _motorcount = 0;
        int _viewableMotors = 0;

        const int _maxmotors = 16; //Max amount of motors possible for this application

//Data structures 
        //Set Data Structure has a name String, a motor String[], and delay int[].
        private struct Set
        {
            public String name;
            public String[] motor;
            public int[] delay;
            
            //Allocates and copies into new memory with a deep copy
            public Set(Set toClone)
            {
                motor = (String[])toClone.motor.Clone();
                delay = (int[])toClone.delay.Clone();
                name = (String)toClone.name.Clone();
            }
        }
        //Group Data Structure, has a name and a String[]
        private struct Group
        {
            public String name;
            public Set[] set;
            
            //Allocates and copies into new memory with a deep copy
            public Group(Group toClone)
            {
                set = (Set[])toClone.set.Clone();
                name = (String)toClone.name.Clone();
            }
        }
//Data structure functions
        //Increases the given Set[] size by 1, and copies all data from old_set into the returned set.
        private Set[] increaseSet(Set[] old_set, String name)
        {
            Set[] new_set;

            if (old_set == null)
            {
                new_set = new Set[1];

                //Instantiate the Name and motor array of the last indexed newly created set
                new_set[0].delay = new int[_maxmotors]; /*Automatically initialaized to 0*/
                new_set[0].motor = new String[_maxmotors];
                new_set[0].name = name;

                //Initialize newly created Set's motor array to "";
                for (int i = 0; i < _maxmotors; i++)
                    new_set[0].motor[i] = "";
            }
            else
            {
                new_set = new Set[old_set.Length + 1];

                //Copy old contents into new contents (uses deep copy)
                for (int i = 0; i < old_set.Length; i++)
                    new_set[i] = new Set(old_set[i]);

                //Instantiate the Name and motor array of the last indexed newly created set
                new_set[new_set.Length - 1].delay = new int[_maxmotors]; /*Automatically initialaized to 0*/
                new_set[new_set.Length - 1].motor = new String[_maxmotors];
                new_set[new_set.Length - 1].name = name;

                //Initialize newly created Set's motor array to "";
                for (int i = 0; i < _maxmotors; i++)
                    new_set[new_set.Length - 1].motor[i] = "";       
            }
            return new_set;
        }
        //Increases the given Group[] size by 1, and copies all data from old_set into the returned set.
        private Group[] increaseGroup(Group[] old_group, String name)
        {
            Group[] new_group;

            if (old_group == null)
            {
                new_group = new Group[1];

                //Initialize the name of the new group
                new_group[0].name = name;
                new_group[0].set = new Set[0];
            }
            else
            {
                new_group = new Group[old_group.Length + 1];
                //Copy old contents into new contents (uses deep copy)
                for (int i = 0; i < old_group.Length; i++)
                    new_group[i] = new Group(old_group[i]);

                //Initialize the name of the new group
                new_group[new_group.Length - 1].name = name;
                new_group[new_group.Length - 1].set = new Set[0];
            }
            
            return new_group;
        }
        //Deletes the specified Set via index from the specified Set[], and shifts and shrinks the Set[] data to fill gap.
        private Set[] decreaseSet(Set[] old_set, int index)
        {
            Set[] new_set = new Set[old_set.Length - 1];

            //Copy old contents into new contents up to index (using deep copy)
            for (int i = 0; i < index; i++)
                new_set[i] = new Set(old_set[i]);
            //Copy old contents into new contents starting after index (using deep copy)
            for (int i = index + 1; i < old_set.Length; i++)
                new_set[i-1] = new Set(old_set[i]);

            return new_set;
        }

        //Deletes the specified Group via index from the specified Group[], and shifts and shrinks the Group[] data to fill gap.
        private Group[] decreaseGroup(Group[] old_group, int index)
        {
            Group[] new_group = new Group[old_group.Length - 1];

            //Copy old contents into new contents up to index (using deep copy)
            for (int i = 0; i < index; i++)
                new_group[i] = new Group(old_group[i]);
            //Copy old contents into new contents starting after index (using deep copy)
            for (int i = index + 1; i < old_group.Length; i++)
                new_group[i - 1] = new Group(old_group[i]);

            return new_group;
        }

//Initialization Functions - Called when entering from Main menu.
        private void Initialize_Operation_Mode()
        {
            //Query Belt
            _motorcount = belt.getMotors(QueryType.SINGLE);
            if (hasError(belt.getStatus(), "getMotors()"))
            {
                //Handle Error
            }
            //Set viewability to only include attached motors (sets off action event handler)
            DirectOption.Checked = true;
        }

        private void Initialize_Program_Mode()
        {
            //Set viewablilty to include all motors (sets off action event handler)
            DirectOption.Checked = false; 
        }
//Functions that manipulate Text Labels
        //Populates 3 lables based on selected motor in "AddedList"
        private void Change_Labels()
        {
            String[] breakUp = new String[3];
            int index;

            if(AddedList.SelectedIndex > -1)
            {
                //Convert.ToInt16(AddedList.SelectedItem.ToString()) - 1, Converts selected Motor (1-16) to the proper index
                index = Convert.ToInt16(AddedList.SelectedItem.ToString()) - 1;
               
                breakUp = _group[_current_group].set[_current_set].motor[index].Split(',');
                AddedRhythmLabel.Text = breakUp[0];
                AddedMagLabel.Text = breakUp[1];
                //Change cycle 7 to Infinity notation for user visability
                if (breakUp[2].Equals("7"))
                    AddedCycleLabel.Text = "Inf";
                else
                    AddedCycleLabel.Text = breakUp[2];
                AddedDelayLabel.Text = _group[_current_group].set[_current_set].delay[index].ToString();
                
            }
            else
            {
                AddedRhythmLabel.Text = "N/A";
                AddedMagLabel.Text = "N/A";
                AddedCycleLabel.Text = "N/A";
                AddedDelayLabel.Text = "N/A";
            }
        }
        //Re-populates AvailableList and AddedList. Called when a setList index change occurs
        private void Change_Set()
        {
            if (SetList.SelectedIndex > -1)
            {
                _current_set = SetList.SelectedIndex;
                AvailableList.Items.Clear();
                AddedList.Items.Clear();
                //Populate Availible Motor List for selected set
                for (int i = 0; i < _viewableMotors; i++)
                {
                    if (_group[_current_group].set[_current_set].motor[i].Equals("")) //Add to AvailableList Items
                        AvailableList.Items.Add((i + 1).ToString());
                    else //Add to AddedList Items
                        AddedList.Items.Add((i + 1).ToString());   
                }
            }
        }

        //Re-populates SetList, AvailableList and AddedList from selected Group
        private void Change_Group()
        {
            if (GroupList.SelectedIndex > -1)
            {
                _current_group = GroupList.SelectedIndex;
                SetList.Items.Clear();
                AvailableList.Items.Clear();
                AddedList.Items.Clear();
                //Populate SetList based on newly selected group
                if (_group[_current_group].set != null)
                {
                    for (int i = 0; i < _group[_current_group].set.Length; i++)
                        SetList.Items.Add(_group[_current_group].set[i].name);
                }
            }
        }
        //Swaps the two selected sets
        private void Swap_Sets()
        {
            int swapA, swapB;
            Set store;

            //Store First Selected Index and deselect
            swapA = SetList.SelectedIndex;
            SetList.SelectedIndices.Remove(swapA);

            //Store Second Selected Index and deselect
            swapB = SetList.SelectedIndex;
            SetList.SelectedIndices.Remove(swapB);

            //Use store to hold set[swapA]'s data. Then switch data.
            store = new Set(_group[_current_group].set[swapA]);
            _group[_current_group].set[swapA] = new Set(_group[_current_group].set[swapB]);
            _group[_current_group].set[swapB] = store;

            //This refreshes setList
            SetList.Items.Clear();
            AvailableList.Items.Clear();
            AddedList.Items.Clear();
            //Populate SetList based on newly selected group
            for (int i = 0; i < _group[_current_group].set.Length; i++)
                SetList.Items.Add(_group[_current_group].set[i].name);
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
            SetList.Items.Clear();
            AvailableList.Items.Clear();
            AddedList.Items.Clear();
            //Populate GroupList based on newly selected group
            for (int i = 0; i < _group.Length; i++)
                GroupList.Items.Add(_group[i].name);
        }

//Functions on Activations
        //Adds an activation request to the selected set
        private void Add_Activation(String rhythm, String mag, String cycles, int delay)
        {
            //must have a selected index
            if (AvailableList.SelectedIndex > -1)
            {
                //Store current Selected Motor's Index before deleting the item out of the list.
                int motor_index = Convert.ToInt16(AvailableList.SelectedItem.ToString()) - 1;
                
                //Remove from AvailableList Section
                AvailableList.Items.RemoveAt(AvailableList.SelectedIndex);
                
                //User selected infinity, which is really code 7.
                if(cycles.Equals("Inf"))
                    cycles = "7";
                //Add motor to the activation chain
                _group[_current_group].set[_current_set].motor[motor_index] = rhythm + "," + mag + "," + cycles;
                _group[_current_group].set[_current_set].delay[motor_index] = delay;
            //Clear AddedList Items (old list) and re-add and preserve motor number order
                AddedList.Items.Clear(); 
                //Populate AddedList Items (fresh list)
                for (int i = 0; i < _viewableMotors; i++)
                {
                    //If the motor entry is not "", it exists so put it in the AddedList
                    if (!_group[_current_group].set[_current_set].motor[i].Equals(""))
                        AddedList.Items.Add((i + 1).ToString());
                }
            }
        }
        //Deletes selected activation request from selected set
        private void Delete_Activation()
        {
            //must have a selected index
            if (AddedList.SelectedIndex > -1)
            {
                //Store current Selected Motor's Index before deleting the item out of the list.
                int motor_index = Convert.ToInt32(AddedList.SelectedItem.ToString()) - 1 ;
                
                //Remove from AddedList Section
                AddedList.Items.RemoveAt(AddedList.SelectedIndex);
                
                //Delete motor from activation chain
                _group[_current_group].set[_current_set].motor[motor_index] = "";
                _group[_current_group].set[_current_set].delay[motor_index] = 0;

                //Clear AvailableList Items (old list) to re-add and preserve number order
                AvailableList.Items.Clear();

                //Populate AvailableList Items (fresh list)
                for (int i = 0; i < _viewableMotors; i++)
                {
                    //If the motor entry is "", it is not Activated so put it in the Available
                    if (_group[_current_group].set[_current_set].motor[i].Equals(""))
                        AvailableList.Items.Add((i + 1).ToString());  
                }
            }
        }
        // Removes all activation requests from the selected set
        private void Clear_Activation()
        {
            //Clear GUI list boxes
            AvailableList.Items.Clear();
            AddedList.Items.Clear();
            //Populate GUI AvailableList and Available setItems 
            //Nullify Added setItems
            for (int i = 0; i < _viewableMotors; i++)
            {
                AvailableList.Items.Add((i + 1).ToString());
                _group[_current_group].set[_current_set].motor[i] = "";
            }
            //Set labels to N/A
            AddedRhythmLabel.Text = "N/A";
            AddedCycleLabel.Text = "N/A";
            AddedMagLabel.Text = "N/A";
            AddedDelayLabel.Text = "N/A";
        }
        //Activates selected motor from AddedList
        private void Activate_Motor()
        {
            String[] breakUp = new String[3];

            //Make sure a motor is selected
            if (AddedList.SelectedIndex > -1 && _done_activating)
            {
                //We don't care about performance of a single motor so just use the String version of Vibrate_Motors()

                breakUp = _group[_current_group].set[_current_set].motor[AddedList.SelectedIndex].Split(',');

                //Vibrate Motor
                if (hasError(belt.Vibrate_Motor(AddedList.SelectedIndex, breakUp[0], breakUp[1], Convert.ToInt16(breakUp[2])), "Vibrate_Motor()"))
                {
                    //Handle Error
                }
            }
        }
//Functions on sets
        //Adds a set with the inputed name parameter from DirectRenameField
        private void Add_Set()
        {
            if (!DirectRenameField.Text.Equals(""))
            {
                _group[_current_group].set = increaseSet(_group[_current_group].set, DirectRenameField.Text);
                //Add item to SetList
                SetList.Items.Add(DirectRenameField.Text);

                DirectRenameField.Clear();
            }
        }
        //Deletes the selected set
        private void Delete_Set()
        {
            if(SetList.SelectedIndex > -1)
            {
                _group[_current_group].set = decreaseSet(_group[_current_group].set, SetList.SelectedIndex);
                //Remove Selected Item to SetList
                SetList.Items.RemoveAt(SetList.SelectedIndex);
                //Clear out AddedList and AvailableList
                AddedList.Items.Clear();
                AvailableList.Items.Clear();
                //Set labels to N/A
                AddedRhythmLabel.Text = "N/A";
                AddedCycleLabel.Text = "N/A";
                AddedMagLabel.Text = "N/A";
                AddedDelayLabel.Text = "N/A";
            }
        }
        //Deletes all sets
        private void Clear_Set()
        {
            _group[_current_group].set = null;
            
            //Clear out SetList, AddedList and AvailableList
            AddedList.Items.Clear();
            AvailableList.Items.Clear();
            SetList.Items.Clear();
            
            //Set labels to N/A
            AddedRhythmLabel.Text = "N/A";
            AddedCycleLabel.Text = "N/A";
            AddedMagLabel.Text = "N/A";
            AddedDelayLabel.Text = "N/A";
        }
        // Activates selected set from SetList, all motors in sed set are activated based
        // on the parameters entered by the user upon adding that motor to the set.
        private void Activate_Set()
        {
            int i,j, delay;

            byte[] motor = new byte[_motorcount];
            byte[] rhythm = new byte[_motorcount];
            byte[] magnitude = new byte[_motorcount];
            byte[] cycles = new byte[_motorcount];
            bool[] go = new bool[_motorcount]; //this array is used to singal a bypass for a particular motor activation

            //Used for breaking up rhythm,magnitude,cycles with split(',')
            String[] breakUp = new String[3];

            //Make sure a set is selected
            if (SetList.SelectedIndex > -1 && _done_activating)
            {
                //Convert all activation records into bytes for quick activation.
                for (i = 0; i < _motorcount; i++)
                {
                    breakUp = _group[_current_group].set[_current_set].motor[i].Split(',');

                    if (!breakUp[0].Equals("")) //if(motor is available)
                    {
                        //Convert all parameters to bytes
                        motor[i] = (byte)i;
                        rhythm[i] = VibStrToByte(breakUp[0]);
                        magnitude[i] = VibStrToByte(breakUp[1]);
                        cycles[i] = (byte)(Convert.ToInt16(breakUp[2]));
                        go[i] = true;
                    }
                    else
                        go[i] = false; //With go = false, we don't need to initialize the other arrays at index i, because this will bypass them
                }

                _done_activating = false;
                //Activate Motors, note that _stop variable is used to break out of activation with the Stop Button
                for (i = 0; (i < _motorcount) && !_stop; i++)
                {
                    if (go[i]) //if(motor is a go, meaning it has an activation record)
                    {
                        delay = _group[_current_group].set[_current_set].delay[i] / 50;
                        //Vibrate Motor
                        if (hasError(belt.Vibrate_Motor(motor[i], rhythm[i], magnitude[i], cycles[i]), "Vibrate_Motor()"))
                        {
                            //Handle Error
                        }
                        //Each 1 delay = 50ms, we do this so we can break every 50ms when thread wakes
                        for (j = 0; j < delay && !_stop; j++)
                            Thread.Sleep(50);
                    }
                }
                _done_activating = true;
            }
        }

//Functions on groups
        //Adds a group with the inputed name parameter from DirectRenameField
        private void Add_Group()
        {
            if (!DirectRenameField.Text.Equals(""))
            {
                _group = increaseGroup(_group, DirectRenameField.Text);
                //Add item to GroupList
                GroupList.Items.Add(DirectRenameField.Text);

                DirectRenameField.Clear();
            }
        }
        //Deletes selected group
        private void Delete_Group()
        {
            if (GroupList.SelectedIndex > -1)
            {
                _group = decreaseGroup(_group, GroupList.SelectedIndex);
                //Remove Selected Item to SetList
                GroupList.Items.RemoveAt(GroupList.SelectedIndex);
                //Clear out SetList, AddedList and AvailableList
                SetList.Items.Clear();
                AddedList.Items.Clear();
                AvailableList.Items.Clear();
                //Set labels to N/A
                AddedRhythmLabel.Text = "N/A";
                AddedCycleLabel.Text = "N/A";
                AddedMagLabel.Text = "N/A";
                AddedDelayLabel.Text = "N/A";
            }
        }
        //Deletes all groups
        private void Clear_Group()
        {  
            //Initialize group->set->motor data structures
            _group = null;
           
            //Clear all ListBoxes
            AvailableList.Items.Clear();
            AddedList.Items.Clear();
            SetList.Items.Clear();
            GroupList.Items.Clear();
        }
        //Activates all sets sequentially within the selected group
        private void Activate_Group()
        {
            int i, j, k, delay;
            //Make sure a group is selected
            if (GroupList.SelectedIndex > -1 && _group[_current_group].set != null && _done_activating)
            {
                //Initialize byte arrays for quick accessing while sending time crucial vibrate commands
                byte[,] motor = new byte[_motorcount, _group[_current_group].set.Length];
                byte[,] rhythm = new byte[_motorcount, _group[_current_group].set.Length];
                byte[,] magnitude = new byte[_motorcount, _group[_current_group].set.Length];
                byte[,] cycles = new byte[_motorcount, _group[_current_group].set.Length];
                bool[,] go = new bool[_motorcount, _group[_current_group].set.Length]; //this array is used to singal a bypass for a particular motor activation

                //Used for breaking up rhythm,magnitude,cycles with split(',')
                String[] breakUp = new String[3];

                //This variable is used for storing and calculating max running times of each set
                int[] rhythm_times = new int[5];
                //Used for queing sets and playing them, initialized through calculations with max_rhythm_time[]
                
                int[] set_times = new int[_group[_current_group].set.Length - 1];
           
                for (i = 0; i < 5; i++) //Get Rhythms Times for Rhythms: A,B,C,D,and E.
                {
                    String temp = belt.getRhythmTime(DirectRhythmBox.Items.IndexOf(i).ToString(), QueryType.SINGLE);
                    if (hasError(belt.getStatus(), "getRhythmTime"))
                    {
                        //Handle Error
                    }
                    rhythm_times[i] = Convert.ToInt16(temp);
                }

                /* Calculate the running time of each set, to sequentially activate sets. Ignores motors with cycle = 7 (infinity).
                 * Such a motor will only change its state if issued another vibrate command or by hitting the stop button.
                 * Note that we don't calculate the last set, because we have no preceeding sets after that one to care about timing.
                 * 
                 * Also convert strings and integers to bytes before sending vibrate commands for quick on-the-fly sending.
                 */
                //This loop is not time crucial, so have it seperate from the activate motor loop section.
                for (j = 0; j < _group[_current_group].set.Length; j++)
                {
                    int max_set_time = 0;
                    //Due to the properties of the data structure motor i+1 == _group[_current_group].set[_current_set].motor[i]
                    for (i = 0; i < _motorcount; i++)
                    {
                        breakUp = _group[_current_group].set[_current_set].motor[i].Split(',');

                        if (!breakUp[0].Equals("")) //if(motor has an activation)
                        {
                            int calculated_time;
                            //Convert all parameters to bytes
                            motor[i, j] = (byte)i;
                            rhythm[i, j] = VibStrToByte(breakUp[0]);
                            magnitude[i, j] = VibStrToByte(breakUp[1]);
                            cycles[i, j] = (byte)(Convert.ToInt16(breakUp[2]));
                            go[i, j] = true;

                            //Calculate Time  = Cycles * Rhythm Length, ignore Cycle = 7 (infinity)
                            if (!breakUp[2].Equals("7"))
                            {
                                //Rhythm is converted to a Char, then casted to an int and subtracted by 65 to make it an index.
                                //Eg: "A" = 0, "B" = 1, "C" = 2, "D" = 3, "E" = 4.
                                calculated_time = Convert.ToInt16(breakUp[2]) * rhythm_times[(int)Convert.ToChar(breakUp[0]) - 65];
                                //Update the max_set_time for this particular set, if this motor activation is the longest thus far.
                                if (max_set_time < calculated_time)
                                    max_set_time = calculated_time;
                            }
                        }
                        else
                            go[i, j] = false; //With go = false, we don't need to initialize the other arrays at index i, because this boolean will bypass them
                    }  
                    set_times[j] = max_set_time;
                }

                _done_activating = false;
                //Start activating motors! Note the _stop variable, that breaks the loops in case a the Stop Button is pressed.
                for (j = 0; (j < _group[_current_group].set.Length) && !_stop; j++)
                {
                    //Wait Here Until last set finishes, ignore if first set through
                    if (j != 0)
                    {
                        delay = set_times[j - 1] / 50;
                        //When this breaks, the last activated set is done vibrating or stop has been issued.
                        for (i = 0; i < delay && !_stop; i++) //Note we won't use the last index of set_time[], because we don't need to wait on the last set
                            Thread.Sleep(50);
                    }

                    //Due to the properties of the belt a motor # <= _motorcount, thus our array is index 0 to _motorcount - 1.
                    for (i = 0; (i < _motorcount) && !_stop; i++)
                    {
                        delay = _group[_current_group].set[j].delay[i] / 50;

                        if (go[i, j]) //if(motor is a go, meaning it has an activation record)
                        {
                            //Vibrate Motor
                            if (hasError(belt.Vibrate_Motor(motor[i, j], rhythm[i, j], magnitude[i, j], cycles[i, j]), "Vibrate_Motor()"))
                            {
                                //Handle Error
                            }
                        }
                        //Each 1 delay = 50ms, we do this so we can break every 50ms when thread wakes
                        for (k = 0; k < delay && !_stop; k++)
                            Thread.Sleep(50);
                    }
                }
                _done_activating = true;
            }
        }
//Other Functions: Stop and Rename button functions
        //Issues a break to stop all activations, then waits for activations to stop
        //Note that if we don't wait till we are done activating we can issue a stop command
        //and then have another activation come along and start them back up!
        private void StopMotors()
        {
            _stop = true; //activate stop command
            while (!_done_activating) { } //wait for activations to stop, may hang application up to 50ms
            _stop = false; //resets stop
            //Stop All Motors
            if (hasError(belt.StopAll(), "StopAll()"))
            {
                //Handle Error
            }
        }
        //Renames the selected set
        private void Rename_Set()
        {
            if (SetList.SelectedIndex > -1 && !DirectRenameField.Text.Equals(""))
            {
                int index;
                //Record this change in the Data Structure
                _group[_current_group].set[_current_set].name = DirectRenameField.Text;

                //Update GroupList Item's list
                SetList.Items.Insert(SetList.SelectedIndex, DirectRenameField.Text);
                index = SetList.SelectedIndex - 1;
                SetList.Items.RemoveAt(SetList.SelectedIndex);

                //Clear the DirectRenameField.Text
                DirectRenameField.Clear();

                //Reset the SelectedIndex
                SetList.SelectedIndex = index;
            }
        }
        //Renames the selected group
        private void Rename_Group()
        {
            if (GroupList.SelectedIndex > -1 && !DirectRenameField.Text.Equals(""))
            {
                int index;
                //Record this change in the Data Structure
                _group[_current_group].name = DirectRenameField.Text;
                
                //Update GroupList Item's list
                GroupList.Items.Insert(GroupList.SelectedIndex, DirectRenameField.Text);
                index = GroupList.SelectedIndex - 1;
                GroupList.Items.RemoveAt(GroupList.SelectedIndex);

                //Clear the DirectRenameField.Text
                DirectRenameField.Clear();

                //Reset the SelectedIndex
                GroupList.SelectedIndex = index;
            }
        }
//Private conversion function
        //Same function as within HapticDriver to convert strings to bytes
        private byte VibStrToByte(string alphaStr)
        {
            byte byteValue = 8;
            switch (alphaStr)
            {
                case "A":
                    byteValue = 0;
                    break;
                case "B":
                    byteValue = 1;
                    break;
                case "C":
                    byteValue = 2;
                    break;
                case "D":
                    byteValue = 3;
                    break;
                case "E":
                    byteValue = 4;
                    break;
                case "F":
                    byteValue = 5;
                    break;
                case "G":
                    byteValue = 6;
                    break;
                case "H":
                    byteValue = 7;
                    break;
                default:
                    byteValue = 8;
                    break;
            }
            return byteValue;
        }
    }
}
