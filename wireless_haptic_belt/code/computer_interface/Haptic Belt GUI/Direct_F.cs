using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using HapticDriver;


namespace HapticGUI
{
    partial class GUI
    {
        int _current_set = 0;
        int _current_group = 0;
        bool _done_activating = true; // Used to make sure we are done activating motors before issuing a stop command
        bool _stop = false; //Used to break activation loops, (_done_activating should be set to true after breaking out of these loops)
        
        //These two variables are similar in use however, seperate so that upon activating, we get no performance loss
        //Otherwise if we used the same variable would send activations to unattached motors.
        int _motorcount = 0;
        int _viewableMotors = 0;

        const int _maxmotors = 16; //Max amount of motors possible for this application



//Functions that manipulate Text Labels
        //Change File - called when a file is loaded
        private void Change_File()
        {
            MotorList.Items.Clear();
            SetList.Items.Clear();
            GroupList.Items.Clear();
            for (int i = 0; i < _group.Length; i++)
                GroupList.Items.Add(_group[i].name);
        }
        //Populates 3 lables based on selected motor in "AddedList"
        private void Change_Motor()
        {
            String[] breakUp;
            int index = MotorList.SelectedIndex;

            if(index > -1)
            {
                breakUp = _group[_current_group].set[_current_set].motor[index].Split(',');

                if (breakUp.Length == 4)
                {
                    AddedRhythmLabel.Text = breakUp[0];
                    AddedMagLabel.Text = breakUp[1];
                    //Change cycle 7 to Infinity notation for user visability
                    if (breakUp[2].Equals("7"))
                        AddedCycleLabel.Text = "Inf";
                    else
                        AddedCycleLabel.Text = breakUp[2];

                    AddedDelayLabel.Text = breakUp[3];
                }
                else
                {
                    AddedRhythmLabel.Text = "N/A";
                    AddedMagLabel.Text = "N/A";
                    AddedCycleLabel.Text = "N/A";
                    AddedDelayLabel.Text = "N/A";
                }
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
                MotorList.Items.Clear();
                //Populate Availible Motor List for selected set
                for (int i = 0; i < _viewableMotors; i++)
                {
                    if (_group[_current_group].set[_current_set].motor[i].Equals("")) //Add to Off Position
                        MotorList.Items.Add((i + 1).ToString());
                    else //Add to On Position
                        MotorList.Items.Add("        " + (i + 1).ToString());   
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
                MotorList.Items.Clear();
                //Populate SetList based on newly selected group
                if (_group[_current_group].set != null)
                {
                    for (int i = 0; i < _group[_current_group].set.Length; i++)
                        SetList.Items.Add(_group[_current_group].set[i].name);
                }
            }
        }
        //Swaps the two selected sets on all groups/sets if true, otherwise just the current set if false
        private void Swap_Motors(bool swapAll)
        {
            int swapA, swapB;
            String storeMotor;

            //Store First Selected Index and deselect
            swapA = MotorList.SelectedIndex;
            MotorList.SelectedIndices.Remove(swapA);

            //Store Second Selected Index and deselect
            swapB = MotorList.SelectedIndex;
            MotorList.SelectedIndices.Remove(swapB);

            //Swap on all sets
            if (swapAll)
            {
                for (int j = 0; j < _group.Length; j++)
                {
                    if (_group[j].set != null)
                    {
                        for (int i = 0; i < _group[j].set.Length; i++)
                        {
                            //Use store to hold set[swapA]'s data. Then switch data.
                            storeMotor = (String)_group[j].set[i].motor[swapA].Clone();
                            //Set swapA
                            _group[j].set[i].motor[swapA] = (String)_group[j].set[i].motor[swapB].Clone();
                            //Set swapB
                            _group[j].set[i].motor[swapB] = storeMotor;
                        }
                    }
                }
            }
            else //swap only on current set
            {
                //Use store to hold set[swapA]'s data. Then switch data.
                storeMotor = (String)_group[_current_group].set[_current_set].motor[swapA].Clone();
                //Set swapA
                _group[_current_group].set[_current_set].motor[swapA] = (String)_group[_current_group].set[_current_set].motor[swapB].Clone();
                //Set swapB
                _group[_current_group].set[_current_set].motor[swapB] = storeMotor;
            }

            Change_Set();
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
            SetList.Items.Clear();
            MotorList.Items.Clear();
            //Populate GroupList based on newly selected group
            for (int i = 0; i < _group.Length; i++)
                GroupList.Items.Add(_group[i].name);
        }

//Functions on Activations
        //Adds an activation request to the selected set
        private void Set_Activation(String rhythm, String mag, String cycles, int delay)
        {
            int index = MotorList.SelectedIndex;
            //must have a selected index
            if (index > -1)
            {              
                //User selected infinity, which is really code 7.
                if(cycles.Equals("Inf"))
                    cycles = "7";
                //Add motor to the activation chain
                _group[_current_group].set[_current_set].motor[index] = rhythm + "," + mag + "," + cycles + "," + delay.ToString();

                //Set motor to ON position
                MotorList.Items.RemoveAt(index);
                MotorList.Items.Insert(index, "        " + (index + 1).ToString());

                //Reselects the index, and updates labels through MotorListSelectedIndexChanged Event
                MotorList.SelectedIndex = index;
            }
        }
        //Deletes selected activation request from selected set
        private void Delete_Activation()
        {
            int index = MotorList.SelectedIndex;
            //must have a selected index
            if (index > -1)
            {
                //Delete motor from activation chain
                _group[_current_group].set[_current_set].motor[index] = "";

                //Set motor to OFF position
                MotorList.Items.RemoveAt(index);
                MotorList.Items.Insert(index, (index + 1).ToString());

                //Updates labels associated with the change
                Change_Motor();
            }
        }
        // Removes all activation requests from the selected set
        private void Clear_Activation()
        {
            //Clear MotorList
            MotorList.Items.Clear();
            //Set all motors to OFF position
            for (int i = 0; i < _viewableMotors; i++)
            {
                MotorList.Items.Add((i + 1).ToString());
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
            if (MotorList.SelectedIndex > -1 && _done_activating)
            {
                //We don't care about performance of a single motor so just use the String version of Vibrate_Motors()

                breakUp = _group[_current_group].set[_current_set].motor[MotorList.SelectedIndex].Split(',');

                //Vibrate Motor
                if (hasError(belt.Vibrate_Motor(MotorList.SelectedIndex, breakUp[0], breakUp[1], Convert.ToInt16(breakUp[2])), "Vibrate_Motor()"))
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
            else
            {
                ErrorForm errorForm = new ErrorForm("Name field blank, you must have a name for each set", "Add_Set()", false);
                errorForm.ShowDialog();  
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
                MotorList.Items.Clear();
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
            _group[_current_group].set = new Set[0];
            
            //Clear out SetList, AddedList and AvailableList
            MotorList.Items.Clear();
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
            int i,j;

            byte[] motor = new byte[_motorcount];
            byte[] rhythm = new byte[_motorcount];
            byte[] magnitude = new byte[_motorcount];
            byte[] cycles = new byte[_motorcount];
            int[] delay = new int[_motorcount];
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
                        delay[i] = Convert.ToInt16(breakUp[3]);
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
                        //Each 1 delay = 50ms, we do this so we can break every 50ms when thread wakes
                        for (j = 0; j < delay[i]/50 && !_stop; j++)
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
                //Remove Selected Item to SetList
                GroupList.Items.RemoveAt(GroupList.SelectedIndex);
                //Clear out SetList, AddedList and AvailableList
                SetList.Items.Clear();
                MotorList.Items.Clear();
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
            _group = new Group[0];
           
            //Clear all ListBoxes
            MotorList.Items.Clear();
            SetList.Items.Clear();
            GroupList.Items.Clear();
        }
        //Activates all sets sequentially within the selected group
        private void Activate_Group()
        {
            int i, j, k;
            //Make sure a group is selected
            if (GroupList.SelectedIndex > -1 && _group[_current_group].set != null && _done_activating)
            {
                //Initialize byte arrays for quick accessing while sending time crucial vibrate commands
                byte[,] motor = new byte[_motorcount, _group[_current_group].set.Length];
                byte[,] rhythm = new byte[_motorcount, _group[_current_group].set.Length];
                byte[,] magnitude = new byte[_motorcount, _group[_current_group].set.Length];
                byte[,] cycles = new byte[_motorcount, _group[_current_group].set.Length];
                int[,] delay = new int[_motorcount, _group[_current_group].set.Length];
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
                            delay[i, j] = Convert.ToInt16(breakUp[3]);
                            go[i, j] = true;

                            //Calculate Time  = Cycles * Rhythm Length + Delay, ignore Cycle = 7 (infinity)
                            if (!breakUp[2].Equals("7"))
                            {
                                //Rhythm is converted to a Char, then casted to an int and subtracted by 65 to make it an index.
                                //Eg: "A" = 0, "B" = 1, "C" = 2, "D" = 3, "E" = 4.
                                calculated_time = Convert.ToInt16(breakUp[2]) * rhythm_times[(int)Convert.ToChar(breakUp[0]) - 65] + delay[i, j];
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
                        //When this breaks, the last activated set is done vibrating or stop has been issued.
                        for (i = 0; i < (set_times[j - 1] / 50) && !_stop; i++) //Note we won't use the last index of set_time[], because we don't need to wait on the last set
                            Thread.Sleep(50);
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
                        //Each 1 delay = 50ms, we do this so we can break every 50ms when thread wakes
                        for (k = 0; k < (delay[i, j] / 50) && !_stop; k++)
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
