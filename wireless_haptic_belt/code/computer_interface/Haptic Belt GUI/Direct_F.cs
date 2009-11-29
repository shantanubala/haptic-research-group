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
        Group[] _group;
        int _current_set = 0;
        int _current_group = 0;
        bool _done_activating = true; // Used to make sure we are done activating motors before issuing a stop command
        bool _stop = false; //Used to break activation loops, (_done_activating should be set to true after breaking out of these loops)
        int _motorcount = 0;
//Data structures 
        //Set Data Structure, has a name and a String[]
        private struct Set
        {
            public String name;
            public String[] motor;
            
            //Allocates and copies into new memory with a deep copy
            public Set(Set toClone)
            {
                motor = (String[])toClone.motor.Clone();
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
        private Set[] increaseSet(Set[] old_set)
        {
            Set[] new_set = new Set[old_set.Length + 1];
            
            //Copy old contents into new contents (uses deep copy)
            for (int i = 0; i < old_set.Length; i++)
                new_set[i] = new Set(old_set[i]);
            
            //Instantiate the Name and motor array of the last indexed newly created set
            new_set[new_set.Length - 1].motor = new String[_motorcount];
            new_set[new_set.Length - 1].name = "New Set";
             
            //Initialize newly created Set's motor array to "";
            for (int i = 0; i < _motorcount; i++)
                new_set[new_set.Length - 1].motor[i] = "";

            return new_set;
        }
        //Increases the given Group[] size by 1, and copies all data from old_set into the returned set.
        private Group[] increaseGroup(Group[] old_group)
        {
            Group[] new_group = new Group[old_group.Length + 1];

            //Copy old contents into new contents (uses deep copy)
            for (int i = 0; i < old_group.Length; i++)
                new_group[i] = new Group(old_group[i]);

            //Initialize the last indexed group's set, and the set's motor array.
            new_group[new_group.Length - 1].set = new Set[1];
            new_group[new_group.Length - 1].set[0].motor = new String[_motorcount];

            //Initialize the names of the new group and set
            new_group[new_group.Length - 1].name = "New Group";
            new_group[new_group.Length - 1].set[0].name = "New Set";

            //Initialize the last indexed group set's motor array to "";
            for (int i = 0; i < _motorcount; i++)
                new_group[new_group.Length - 1].set[0].motor[i] = "";
            
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

//Initialization Function - Called when entering from Main menu.
        private void Initialize_DirectMode()
        {
            //Query Belt
            _motorcount = belt.getMotors(QueryType.SINGLE);
            if (hasError(belt.getError(), "getMotors()"))
            {
                //Handle Error
//For Testing with disconnected belt
                _motorcount = 16;
            }
            if (_motorcount > 0)
            {
                //Initialize group->set->motor data structures
                _group = new Group[1];
                _group[0].set = new Set[1];
                _group[0].set[0].motor = new String[_motorcount];
                _group[0].name = "New Group";
                _group[0].set[0].name = "New Set";
                //Initialize motor String aray to "", "" means the motor is available
                for (int i = 0; i < _motorcount; i++)
                {
                    _group[0].set[0].motor[i] = "";
                    AvailableList.Items.Add((i + 1).ToString());
                }
                //Add the set and group to the list and select the first item in group, set and available list
                SetList.Items.Add(_group[0].set[0].name);
                GroupList.Items.Add(_group[0].name);
                GroupList.SelectedIndex = 0;
                SetList.SelectedIndex = 0;
                AvailableList.SelectedIndex = 0;

                //Reset global variables to 0
                _current_set = 0;
                _current_group = 0;
            }
        }
//Functions that manipulate Text Labels
        //Populates 3 lables based on selected motor in "AddedList"
        private void Change_Labels()
        {
            String[] breakUp = new String[3];
            if(AddedList.SelectedIndex > -1)
            {
                ErrorStatus.Text = _group[_current_group].set[_current_set].motor.Length.ToString();
                //Convert.ToInt16(AddedList.SelectedItem.ToString()) - 1, Converts selected Motor (1-16) to the proper index
                breakUp = _group[_current_group].set[_current_set].motor[Convert.ToInt16(AddedList.SelectedItem.ToString()) - 1].Split(',');
                AddedRhythmLabel.Text = breakUp[0];
                AddedMagLabel.Text = breakUp[1];
                //Change cycle 7 to Infinity notation for user visability
                if (breakUp[2].Equals("7"))
                    AddedCycleLabel.Text = "Inf";
                else
                    AddedCycleLabel.Text = breakUp[2];
            }
            else
            {
                AddedRhythmLabel.Text = "N/A";
                AddedMagLabel.Text = "N/A";
                AddedCycleLabel.Text = "N/A";
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
                for (int i = 0; i < _motorcount; i++)
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
                for (int i = 0; i < _group[_current_group].set.Length; i++)
                    SetList.Items.Add(_group[_current_group].set[i].name);
            }
        }
//Functions on Activations
        //Adds an activation request to the selected set
        private void Add_Activation(String rhythm, String mag, String cycles)
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

            //Clear AddedList Items (old list) and re-add and preserve motor number order
                AddedList.Items.Clear(); 
                //Populate AddedList Items (fresh list)
                for (int i = 0; i < _motorcount; i++)
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

                //Clear AvailableList Items (old list) to re-add and preserve number order
                AvailableList.Items.Clear();

                //Populate AvailableList Items (fresh list)
                for (int i = 0; i < _motorcount; i++)
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
            for (int i = 0; i < _motorcount; i++)
            {
                AvailableList.Items.Add((i + 1).ToString());
                _group[_current_group].set[_current_set].motor[i] = "";
            }
            //Set labels to N/A
            AddedRhythmLabel.Text = "N/A";
            AddedCycleLabel.Text = "N/A";
            AddedMagLabel.Text = "N/A";
        }
        //Activates selected motor from AddedList
        private void Activate_Motor()
        {
            String[] breakUp = new String[3];

            //Make sure a motor is selected
            if (AddedList.SelectedIndex > -1)
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
        private void Add_Set()
        {
            _group[_current_group].set = increaseSet(_group[_current_group].set);
            //Add item to SetList
            SetList.Items.Add("New Set");
        }

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
            }
        }
        private void Clear_Set()
        {
            _group[_current_group].set = new Set[1];
            _group[_current_group].set[0].motor = new String[_motorcount];
            _group[_current_group].set[0].name = "New Set";
            //Initialize motor String aray to "", "" means the motor is available
            for (int i = 0; i < _motorcount; i++)
            {
                _group[_current_group].set[0].motor[i] = "";
                AvailableList.Items.Add((i + 1).ToString());
            }
            //Clear out SetList, AddedList and AvailableList
            AddedList.Items.Clear();
            AvailableList.Items.Clear();
            SetList.Items.Clear();
            //Add back the newly created set, and select the new set
            SetList.Items.Add("New Set");
            SetList.SelectedIndex = 0;
            //Set labels to N/A
            AddedRhythmLabel.Text = "N/A";
            AddedCycleLabel.Text = "N/A";
            AddedMagLabel.Text = "N/A";

            //Reset global variable to 0, as now there is only 1 bank set in this group
            _current_set = 0;
        }
        // Activates selected set from SetList, all motors in sed set are activated based
        // on the parameters entered by the user upon adding that motor to the set.
        private void Activate_Set()
        {
            int i; //loop variable

            byte[] motor = new byte[_motorcount];
            byte[] rhythm = new byte[_motorcount];
            byte[] magnitude = new byte[_motorcount];
            byte[] cycles = new byte[_motorcount];
            bool[] go = new bool[_motorcount]; //this array is used to singal a bypass for a particular motor activation

            //Used for breaking up rhythm,magnitude,cycles with split(',')
            String[] breakUp = new String[3];

            //Make sure a set is selected
            if (SetList.SelectedIndex > -1)
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
                        //Vibrate Motor
                        if (hasError(belt.Vibrate_Motor(motor[i], rhythm[i], magnitude[i], cycles[i]), "Vibrate_Motor()"))
                        {
                            //Handle Error
                        }
                    }
                }
                _done_activating = true;
            }
        }

//Functions on groups
        private void Add_Group()
        {
            _group = increaseGroup(_group);
            //Add item to GroupList
            GroupList.Items.Add("New Group");
        }

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
            }
        }
        private void Clear_Group()
        {  
            //Initialize group->set->motor data structures
            _group = new Group[1];
            _group[0].set = new Set[1];
            _group[0].set[0].motor = new String[_motorcount];
            _group[0].name = "New Group";
            _group[0].set[0].name = "New Set";
            //Initialize motor String aray to "", "" means the motor is available
            for (int i = 0; i < _motorcount; i++)
            {
                _group[0].set[0].motor[i] = "";
                AvailableList.Items.Add((i + 1).ToString());
            }
            //Clear all ListBoxes
            AvailableList.Items.Clear();
            AddedList.Items.Clear();
            SetList.Items.Clear();
            GroupList.Items.Clear();
            
            //Add the set and group to the list and select the first item in group, set and available list
            SetList.Items.Add(_group[0].set[0].name);
            GroupList.Items.Add(_group[0].name);
            SetList.SelectedIndex = 0;
            GroupList.SelectedIndex = 0;
            AvailableList.SelectedIndex = 0;

            //Reset global variables to 0
            _current_set = 0;
            _current_group = 0;
        }


        private void Activate_Group()
        {
            int i, j; //Initialize loop variables

            //Initialize DateTimes used to syncronize the activation of sets back to back.
            DateTime start = DateTime.Now;
            DateTime now = DateTime.Now;

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
            TimeSpan[] set_times = new TimeSpan[_group[_current_group].set.Length - 1];

            //Make sure a group is selected
            if (GroupList.SelectedIndex > -1)
            {
                for (i = 0; i < 5; i++) //Get Rhythms Times for Rhythms: A,B,C,D,and E.
                {
                    String temp = belt.getRhythmTime(DirectRhythmBox.Items.IndexOf(i).ToString(), QueryType.SINGLE);
                    if (hasError(belt.getError(), "getRhythmTime"))
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
                    //Populate TimeSpan array set_times(100ns), from max_set_time(ms). 
                    //new TimeSpan(Tick), A tick is 100ns. 1ms = 1000000 ns. So use 10000 as a conversion to 100ns's.
                    set_times[j] = new TimeSpan(max_set_time * 10000);
                }

                _done_activating = false;
                //Start activating motors! Note the _stop variable, that breaks the loops in case a the Stop Button is pressed.
                for (j = 0; (j < _group[_current_group].set.Length) && !_stop; j++)
                {
                    //Wait Here Until last set finishes, ignore if first set through
                    if (j != 0)
                    {
                        now = DateTime.Now;
                        //When this breaks, the last activated set is done vibrating!
                        while ((now - start < set_times[j - 1]) && !_stop) //Note we won't use the last index of max_set_time[], because we don't need to wait on the last set
                        {
                            Application.DoEvents(); //Prevents the GUI from hanging, allows button events to continue!
                            now = DateTime.Now; //Update counter(time)
                        }
                    }

                    //Due to the properties of the belt a motor # <= _motorcount, thus our array is index 0 to _motorcount - 1.
                    for (i = 0; (i < _motorcount) && !_stop; i++)
                    {
                        if (go[i, j]) //if(motor is a go, meaning it has an activation record)
                        {
                            //Vibrate Motor
                            if (hasError(belt.Vibrate_Motor(motor[i, j], rhythm[i, j], magnitude[i, j], cycles[i, j]), "Vibrate_Motor()"))
                            {
                                //Handle Error
                            }
                        }
                    }
                    start = DateTime.Now; //Set start time after all motors in the set are activated
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
            while (!_done_activating) { } //wait for activations to stop, hangs application very briefly < 1ms
            _stop = false; //resets stop
            //Stop All Motors
            if (hasError(belt.StopAll(), "StopAll()"))
            {
                //Handle Error
            }
        }
        //Renames the selected set
        private void RenameSet()
        {
            if (SetList.SelectedIndex > -1)
            {
                //Record this change in the Data Structure
                _group[_current_group].set[_current_set].name = DirectRenameField.Text;

                //Update GroupList Item's list
                SetList.Items.Insert(SetList.SelectedIndex, DirectRenameField.Text);
                SetList.SelectedIndex = SetList.SelectedIndex - 1;
                SetList.Items.RemoveAt(SetList.SelectedIndex + 1);

                //Clear the DirectRenameField.Text
                DirectRenameField.Clear();
            }
        }
        //Renames the selected group
        private void RenameGroup()
        {
            if (GroupList.SelectedIndex > -1)
            {
                //Record this change in the Data Structure
                _group[_current_group].name = DirectRenameField.Text;
                
                //Update GroupList Item's list
                GroupList.Items.Insert(GroupList.SelectedIndex, DirectRenameField.Text);
                GroupList.SelectedIndex = GroupList.SelectedIndex - 1;
                GroupList.Items.RemoveAt(GroupList.SelectedIndex + 1);

                //Clear the DirectRenameField.Text
                DirectRenameField.Clear();
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
