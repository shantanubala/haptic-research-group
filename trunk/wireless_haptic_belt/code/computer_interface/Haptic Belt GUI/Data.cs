using System;
using System.IO;
using System.ComponentModel;
using HapticDriver;

namespace HapticGUI
{
    partial class GUI
    {
        static Group[] _group;

//Data structures
        //Activations represent the data to be sent as a vibrate command to the belt
        public struct Activation
        {
            public byte motor;
            public byte rhythm;
            public byte magnitude;
            public byte cycles;
            public Int32 delay;

            public Activation(Activation toClone)
            {
                motor = toClone.motor;
                rhythm = toClone.rhythm;
                magnitude = toClone.magnitude;
                cycles = toClone.cycles;
                delay = toClone.delay;
            }
        }

        //Represents the data required to specify a Rhythm
        public struct Rhythm
        {
            public String pattern;
            public Int16 time;
        }
        //Represents the data required to specify a Magnitude
        public struct Magnitude
        {
            public UInt16 period;
            public UInt16 dutycycle;
        }
        /*Represents an Event, in time which may have multiple Activations
         *Maintains all activations in ascending motor number order
         *The reason for this allow for sorted viewing in the GUI as well as allowing 
         *swaps on activations in the GUI more simple.
         */
        public struct Event
        {
            public Int32 time;
            public Activation[] activations;

            public Event(Event toClone)
            {
                time = toClone.time;
                activations = (Activation[])toClone.activations.Clone();
            }
            //Replaces previous matching Activation with the same motor number
            //If one doesn't exist we make room for it
            //Return: index of where motor was replaced, or -1 if it wasnt
            public void addActivation(Activation add)
            {
                Activation[] new_activations;

                int i;
                //Search for motor in the activations
                for (i = 0; i < activations.Length; i++)
                {
                    if (activations[i].motor == add.motor)
                    {
                        //motor activation found in this event instance, replace the actiavtion
                        activations[i] = new Activation(add);
                        break;
                    }
                }
                if (i == activations.Length) //no matched motor found, insert at end
                {
                    new_activations = new Activation[activations.Length + 1];

                    for (i = 0; i < activations.Length; i++)
                    {
                        //Copy elements until we find inserting point
                        if (activations[i].motor > add.motor)
                            break;

                        new_activations[i] = activations[i];
                    }

                    //Insert at spot i in the new array - must be outside of last for loop
                    new_activations[i] = add;

                    //Continue copying the rest of the array
                    while (i < activations.Length)
                    {
                        new_activations[i + 1] = activations[i];
                        i++;
                    }

                    activations = new_activations;
                }
            }

            public void removeActivation(int motor_num)
            {
                int i, index;

                Activation[] new_activations;

                //Search through all activations matching motor parameter
                for (index = 0; index < activations.Length; index++)
                {
                    if (activations[index].motor == (byte)motor_num) //Delete activation at index j
                    {
                        new_activations = new Activation[activations.Length - 1];

                        //Copy old contents into new contents up to j (not including j)
                        for (i = 0; i < index; i++)
                            new_activations[i] = activations[i];

                        //Copy old contents into new contents starting after j
                        for (i = i + 1; i < activations.Length; i++)
                            new_activations[i - 1] = activations[i];

                        activations = new_activations;

                        break; // Item removed, break
                    }
                }
            }
        }
        
        /*Represents all activations on a single motor
         *Activations are inserted in ascending delay order and maintained this way.
         *The advantage of this is so we can determine the endTime of the specific motor.
         *Due to the properties of the haptic belt, a new activation on the same motor will
         *replace the old one, thus the last motor determines the final end running time.
         */
        public struct Motor
        {
            public Int32 endTime;
            public Activation[] activations;

            public Motor(Motor toClone)
            {
                endTime = toClone.endTime;
                activations = (Activation[])toClone.activations;
            }

            //Replaces previous matching Activation with the same delay time
            //If one doesn't exist we make room for it, and insert in order
            //Return: true if the inserted activation was inserted at the end, false otherwise
            //The return value will signal the caller to update endTime, since at this level it cannot be calculated
            public Boolean addActivation(Activation add)
            {
                //Insert activation into motor list
                int i;
                Boolean result = false;
                Activation[] new_activations;

                //Search for delay in the activations
                for (i = 0; i < activations.Length; i++)
                {
                    if (activations[i].delay == add.delay)
                    {
                        //Motor with matching delay found, replace motor
                        activations[i] = new Activation(add);
                        break; 
                    }
                }
                if (i == activations.Length) //no matched delay found, insert in ascending order
                {
                    new_activations = new Activation[activations.Length + 1];

                    for (i = 0; i < activations.Length; i++)
                    {
                        //Copy elements until we find inserting point
                        if (activations[i].delay > add.delay)
                            break;

                        new_activations[i] = activations[i];
                    }

                    //Insert at spot i in the new array - must be outside of last for loop
                    new_activations[i] = add;

                    //Inserted at the end, caller will know to update endTime
                    if (i == activations.Length)
                        result = true;

                    //Continue copying the rest of the array
                    while (i < activations.Length)
                    {
                        new_activations[i + 1] = activations[i];
                        i++;
                    }

                    activations = new_activations;
                }
                return result;
            }

            //Matches specified Activation's delay with one currently in the activations array
            //If a match is found it is removed, otherwise this does nothing
            public Boolean removeActivation(int delay)
            {
                int i,index;
                Boolean result = false;

                Activation[] new_activations; 

                //Find the index of the matching delays
                for (index = 0; index < activations.Length; index++)
                {
                    if (activations[index].delay == delay)
                    {
                        new_activations = new Activation[activations.Length - 1];

                        for (i = 0; i < index; i++)
                            new_activations[i] = activations[i];

                        for (i = i + 1; i < activations.Length; i++)
                            new_activations[i - 1] = activations[i];

                        activations = new_activations;

                        //True when removing the last element of the array, note that we already updated activations at this point.
                        if (index == activations.Length)
                            result = true;

                        break; // Item removed, break
                    }
                }
                return result;
            }
        }
       
        //Group Data Structure, has a name and a String[]
        private struct Group
        {
            public String name;
            public Int32 cycles;
            public Motor[] motors;
            public Event[] events;
            public Rhythm[] rhythm;
            public Magnitude[] magnitude;

            //Allocates and copies into new memory with a deep copy
            public Group(Group toClone)
            {
                name = (String)toClone.name.Clone();
                cycles = toClone.cycles;
                events = (Event[])toClone.events.Clone();
                motors = (Motor[])toClone.motors.Clone();
                rhythm = (Rhythm[])toClone.rhythm.Clone();
                magnitude = (Magnitude[])toClone.magnitude.Clone();
            }
            //adds activation to motors and events arrays, or replaces the motor activation
            //if there is already a motor # activation for a particular event (for both motors[] and events[])
            //Return: True: activation was replaced in events[], False: activation wasn't replaced.
            public void addActivation(int motor_num, int rhythm, int magnitude, int cycles ,int delay)
            {
                int j, k;
                Activation insert;

                //Setup the activation to insert into the event array
                insert.motor = (byte)motor_num;
                insert.rhythm = (byte)rhythm;
                insert.magnitude = (byte)magnitude;
                insert.cycles = (byte)cycles;
                insert.delay = delay;

                //Pass a new copy of the activation insert, so the system does a deep copy.
                //If this statement is true, update endTime
                if (motors[motor_num].addActivation(new Activation(insert)))
                {
                    if (cycles != 7)
                        motors[motor_num].endTime = this.rhythm[rhythm].time * 50 * cycles + delay;
                    else
                        motors[motor_num].endTime = -1; //-1 stands for infinity
                }

                //Try to find a pre-existing event time that matches this request and insert it
                //If a time event is found, and an activation already exists for this specific motor, replace it
                for (j = 0; j < events.Length; j++)
                {
                    if (events[j].time == insert.delay) // There is already an instance of this time add activation
                    {
                        events[j].addActivation(new Activation(insert));
                        break;
                    }
                }
                if (j == events.Length) //Event time didn't exist, we must create a new event and insert it
                {
                    //Create space for a new event
                    Event[] new_events = new Event[events.Length + 1];

                    //Maintain property of first time to last time (ascending time property)
                    //Find inserting point
                    for (k = 0; k < j; k++)
                    {
                        //Copy elements until we find inserting point
                        if (events[k].time > delay) 
                            break;
                        
                        new_events[k] = events[k];
                    }

                    //Insert at spot k in the new array - must be outside of loop
                    new_events[k].time = delay;
                    new_events[k].activations = new Activation[1];
                    new_events[k].activations[0] = insert;

                    //Continue copying the rest of the array
                    while (k < j)
                    {
                        new_events[k + 1] = events[k];
                        k++;
                    }

                    events = new_events;
                }
            }

            //Remove activation from motors, and events arrays much quicker than previous method
            public void removeActivation(int event_num, int activation_num)
            {
                int i, motor_num, delay, cycles, rhythm;
                Activation[] new_activations;

                //Check to see if the numbers are valid parameters
                if (events.Length > event_num) 
                {
                    if (events[event_num].activations.Length > activation_num)
                    {
                        //allocate the selected activation's parameters to local variables for easy readibility
                        motor_num = events[event_num].activations[activation_num].motor; 
                        delay = events[event_num].activations[activation_num].delay;
                        
                        //Remove from motors array using the two fields we just got, if we removed the last element: recalculate endTime
                        if (motors[motor_num].removeActivation(delay))
                        {
                            //allocate the last motors[] activation's parameters to local variables for easy readibility
                            delay = motors[motor_num].activations[motors[motor_num].activations.Length - 1].delay;
                            cycles = motors[motor_num].activations[motors[motor_num].activations.Length - 1].cycles;
                            rhythm = motors[motor_num].activations[motors[motor_num].activations.Length - 1].rhythm;

                            if (cycles != 7)
                                motors[motor_num].endTime = this.rhythm[rhythm].time * 50 * cycles + delay;
                            else
                                motors[motor_num].endTime = -1; //-1 stands for infinity
                        }

                        //Remove from events array
                        new_activations = new Activation[events[event_num].activations.Length - 1];

                        for (i = 0; i < activation_num; i++)
                            new_activations[i] = events[event_num].activations[i];

                        for (i = i + 1; i < events[event_num].activations.Length; i++)
                            new_activations[i - 1] = events[event_num].activations[i];

                        events[event_num].activations = new_activations;
                    }
                }
            }
            //Deletes the specified event, and resizes events array
            public void deleteEvent(int event_num)
            {
                int i;
                Event[] new_events = new Event[events.Length - 1];

                // Delete all activations from motors[]
                for (i = 0; i < events[event_num].activations.Length; i++)
                    motors[events[event_num].activations[i].motor].removeActivation(events[event_num].activations[i].delay);

                //Shrink event array
                for (i = 0; i < event_num; i++)
                    new_events[i] = events[i];

                for (i = i + 1; i < events.Length; i++)
                    new_events[i - 1] = events[i];

                events = new_events;

            }
            //Clears out all events
            public void clearEvents()
            {
                //Clear out all activations
                motors = new Motor[16];

                for (int i = 0; i < 16; i++)
                {
                    //Motor initialization
                    motors[i].endTime = 0;
                    motors[i].activations = new Activation[0];
                }

                events = new Event[0];
            }
            //Calls refreshActivation for each independent activation in this.motors[] data,a renewing this.events[] data.
            //Should be only called after the user reconfigures the motor order.
            public void refreshEvents()
            {
                int i, j;
                events = new Event[0];

                for (i = 0; i < motors.Length; i++)
                {
                    for (j = 0; j < motors[i].activations.Length; j++ )
                    {
                        addEventActivation(i, motors[i].activations[j].rhythm, motors[i].activations[j].magnitude, motors[i].activations[j].cycles, motors[i].activations[j].delay);
                    }
                }
            }

            private void addEventActivation(int motor_num, int rhythm, int magnitude, int cycles ,int delay)
            {
                int j, k;
                Activation insert;

                //Setup the activation to insert into the event array
                insert.motor = (byte)motor_num;
                insert.rhythm = (byte)rhythm;
                insert.magnitude = (byte)magnitude;
                insert.cycles = (byte)cycles;
                insert.delay = delay;

                //Try to find a pre-existing event time that matches this request and insert it
                //If a time event is found, and an activation already exists for this specific motor, replace it
                for (j = 0; j < events.Length; j++)
                {
                    if (events[j].time == insert.delay) // There is already an instance of this time add activation
                    {
                        events[j].addActivation(new Activation(insert));
                        break;
                    }
                }
                if (j == events.Length) //Event time didn't exist, we must insert it somewhere
                {
                    //Create space for a new event
                    Event[] new_events = new Event[events.Length + 1];

                    //Maintain property of first time to last time (ascending time property)
                    //Find inserting point
                    for (k = 0; k < j; k++)
                    {
                        //Copy elements until we find inserting point
                        if (events[k].time > delay)
                            break;

                        new_events[k] = events[k];
                    }

                    //Insert at spot k in the new array - must be outside of loop
                    new_events[k].time = delay;
                    new_events[k].activations = new Activation[1];
                    new_events[k].activations[0] = insert;

                    //Continue copying the rest of the array
                    while (k < j)
                    {
                        new_events[k + 1] = events[k];
                        k++;
                    }

                    events = new_events;
                }
            }
        }
        
        //Data structure functions
        
        //Increases the given Group[] size by 1, and copies all data from old_set into the returned set.
        private Group[] increaseGroup(Group[] old_group, String name)
        {
            int i;
            char id;

            Group[] new_group = new Group[old_group.Length + 1];

            //Copy old contents into new contents (uses deep copy)
            for (i = 0; i < old_group.Length; i++)
                new_group[i] = new Group(old_group[i]);

            //Initialize the name of the new group
            new_group[i].name = name;
            new_group[i].cycles = 1;
            new_group[i].motors = new Motor[_maxmotors];
            new_group[i].events = new Event[0];
            new_group[i].rhythm = new Rhythm[_maxrhythms];
            new_group[i].magnitude = new Magnitude[_maxmagnitudes];

            //Initialize rhythm and magnitude arrays
            for (int j = 0; j < _maxmotors; j++)
            {
                id = (char)(j + 65); //ASCII 65-69 = A-E
                
                //Motor initialization
                new_group[i].motors[j].endTime = 0;
                new_group[i].motors[j].activations = new Activation[0];

                if (j < _maxrhythms) //Rhythm initializations
                {
                    new_group[i].rhythm[j].pattern = "0"; /* Default to Rythm of OFF for 50ms */
                    new_group[i].rhythm[j].time = 1; 
                }
                if (j < _maxmagnitudes) //Magnitude initialization
                {
                    new_group[i].magnitude[j].period = 2000; // Default to Magnitude of 0% 
                    new_group[i].magnitude[j].dutycycle = 0;
                }
            }

            return new_group;
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
//Save/Load Data
        //Load the data structure "_group" from the saved file in pieces
        private void loadBinaryFile_FileOk(object sender, CancelEventArgs e)
        {
            FileStream readStream;

            try
            {
                readStream = new FileStream(loadBinaryFile.FileName, FileMode.Open);
                BinaryReader readBinary = new BinaryReader(readStream);

                try
                {
                    /* Begin Reading data, all array sizes are read as a 32 bit interger
                     * before the array data is passed, so we can create a loop around
                     * the array.
                     * 
                       Save/Load Order Format:
                      
                        String name;
                        Int32 cycles;
                        Motor[_maxmotors] motors
                            Int32 endTime
                            Activation[activations.Length] activations
                                byte motor
                                byte rhythm
                                byte magnitude
                                byte cycles
                                Int32 delay
                        Event[events.Length] events
                            Int32 time
                            Activation[activations.Length] activations
                                byte motor
                                byte rhythm
                                byte magnitude
                                byte cycles
                                Int32 delay
                        Rhythm[_maxrhythms] rhythm;
                            String id
                            String pattern
                            UInt16 time
                        Magnitude[_maxmagnitudes] magnitude;
                            String id
                            UInt16 period
                            UInt16 dutycycle
 
                     */

                    _group = new Group[readBinary.ReadInt32()];
                    //Populate Group array
                    for (int i = 0; i < _group.Length; i++)
                    {
                        _group[i].name = readBinary.ReadString();
                        _group[i].cycles = readBinary.ReadInt32();
                        //Initialize _group[i]'s arrays
                        _group[i].motors = new Motor[_maxmotors]; //const of 16 motors unless changed
                        _group[i].events = new Event[readBinary.ReadInt32()];
                        _group[i].rhythm = new Rhythm[_maxrhythms]; //const of 5 rhythms (belt supports up to 8)
                        _group[i].magnitude = new Magnitude[_maxmagnitudes]; //const of 4 rhythms (belt supports only 4)
                        //Populate Motors array
                        for (int j = 0; j < _maxmotors; j++)
                        {
                            _group[i].motors[j].endTime = readBinary.ReadInt32();
                            _group[i].motors[j].activations = new Activation[readBinary.ReadInt32()];
                            for (int k = 0; k < _group[i].motors[j].activations.Length; k++)
                            {
                                _group[i].motors[j].activations[k].motor = readBinary.ReadByte();
                                _group[i].motors[j].activations[k].rhythm = readBinary.ReadByte();
                                _group[i].motors[j].activations[k].magnitude = readBinary.ReadByte();
                                _group[i].motors[j].activations[k].cycles = readBinary.ReadByte();
                                _group[i].motors[j].activations[k].delay = readBinary.ReadInt32();
                            }
                        }
                        _group[i].events = new Event[readBinary.ReadInt32()];
                        //Populate Events array
                        for (int j = 0; j < _group[i].events.Length; j++)
                        {
                            _group[i].events[j].time = readBinary.ReadInt32();
                            _group[i].events[j].activations = new Activation[readBinary.ReadInt32()];
                            for (int k = 0; k < _group[i].motors[j].activations.Length; k++)
                            {
                                _group[i].events[j].activations[k].motor = readBinary.ReadByte();
                                _group[i].events[j].activations[k].rhythm = readBinary.ReadByte();
                                _group[i].events[j].activations[k].magnitude = readBinary.ReadByte();
                                _group[i].events[j].activations[k].cycles = readBinary.ReadByte();
                                _group[i].events[j].activations[k].delay = readBinary.ReadInt32();
                            }
                        }
                        //Populate Rhythm Array
                        for (int j = 0; j < _maxrhythms; j++)
                        {
                            _group[i].rhythm[j].pattern = readBinary.ReadString();
                            _group[i].rhythm[j].time = readBinary.ReadInt16();
                        }
                        //Populate Magnitude Array
                        for (int j = 0; j < _maxmagnitudes; j++)
                        {
                            _group[i].magnitude[j].period = readBinary.ReadUInt16();
                            _group[i].magnitude[j].dutycycle = readBinary.ReadUInt16();
                        } 
                    }
                    //Update the List Boxes to display the newly loaded data
                    Change_File();
                }
                catch (Exception exception)
                {
                    ErrorForm errorDiag = new ErrorForm((exception.ToString())/*.Split(':'))[0]*/, "loadBinaryFile_FileOk()", true);
                    errorDiag.ShowDialog();
                }
                finally //Always close files
                {
                    readBinary.Close();
                    readStream.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorForm errorDiag = new ErrorForm((exception.ToString())/*.Split(':'))[0]*/, "loadBinaryFile_FileOk()", true);
                errorDiag.ShowDialog();
            }

        }
        //Save the data structure "_group" into pieces
        private void saveBinaryFile_FileOk(object sender, CancelEventArgs e)
        {
            FileStream writeStream;
            try
            {
                writeStream = new FileStream(saveBinaryFile.FileName, FileMode.Create);
                BinaryWriter writeBinary = new BinaryWriter(writeStream);

                try
                {
                    /* Begin Writing data data, all array sizes are written as a 32 bit intergers
                     * before the array data is written, so loader know the amount of the data
                     * to load.
                     * 
                       Save/Load Order Format:
                      
                        String name;
                        Int32 cycles;
                        Motor[_maxmotors] motors
                            Int32 endTime
                            Activation[activations.Length] activations
                                byte motor
                                byte rhythm
                                byte magnitude
                                byte cycles
                                Int32 delay
                        Event[events.Length] events
                            Int32 time
                            Activation[activations.Length] activations
                                byte motor
                                byte rhythm
                                byte magnitude
                                byte cycles
                                Int32 delay
                        Rhythm[_maxrhythms] rhythm;
                            String id
                            String pattern
                            UInt16 time
                        Magnitude[_maxmagnitudes] magnitude;
                            String id
                            UInt16 period
                            UInt16 dutycycle
                     */

                    writeBinary.Write(_group.Length);
                    //Populate Group array
                    for (int i = 0; i < _group.Length; i++)
                    {
                        writeBinary.Write(_group[i].name);
                        writeBinary.Write(_group[i].cycles);
                        //Initialize _group[i]'s arrays

                        //motors[].Length const of 16 motors unless changed
                        writeBinary.Write(_group[i].events.Length);
                        //rhythm[].Length const of 5 rhythms (belt supports up to 8)
                        //magnitude[].Length const of 4 rhythms (belt supports only 4)

                        //Populate Motors array
                        for (int j = 0; j < _maxmotors; j++)
                        {
                            writeBinary.Write(_group[i].motors[j].endTime);
                            writeBinary.Write(_group[i].motors[j].activations.Length);
                            for (int k = 0; k < _group[i].motors[j].activations.Length; k++)
                            {
                                writeBinary.Write(_group[i].motors[j].activations[k].motor);
                                writeBinary.Write(_group[i].motors[j].activations[k].rhythm);
                                writeBinary.Write(_group[i].motors[j].activations[k].magnitude);
                                writeBinary.Write(_group[i].motors[j].activations[k].cycles);
                                writeBinary.Write(_group[i].motors[j].activations[k].delay);
                            }
                        }
                        writeBinary.Write(_group[i].events.Length);
                        //Populate Events array                        
                        for (int j = 0; j < _group[i].events.Length; j++)
                        {
                            writeBinary.Write(_group[i].events[j].time);
                            writeBinary.Write(_group[i].events[j].activations.Length);
                            for (int k = 0; k < _group[i].motors[j].activations.Length; k++)
                            {
                                writeBinary.Write(_group[i].events[j].activations[k].motor);
                                writeBinary.Write(_group[i].events[j].activations[k].rhythm);
                                writeBinary.Write(_group[i].events[j].activations[k].magnitude);
                                writeBinary.Write(_group[i].events[j].activations[k].cycles);
                                writeBinary.Write(_group[i].events[j].activations[k].delay);
                            }
                        }
                        //Populate Rhythm Array
                        for (int j = 0; j < _maxrhythms; j++)
                        {
                            writeBinary.Write(_group[i].rhythm[j].pattern);
                            writeBinary.Write(_group[i].rhythm[j].time);
                        }
                        //Populate Magnitude Array
                        for (int j = 0; j < _maxmagnitudes; j++)
                        {
                            writeBinary.Write(_group[i].magnitude[j].period);
                            writeBinary.Write(_group[i].magnitude[j].dutycycle);
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorForm errorDiag = new ErrorForm((exception.ToString())/*.Split(':'))[0]*/, "saveBinaryFile_FileOk()", true);
                    errorDiag.ShowDialog();
                }
                finally //Always close files
                {
                    writeBinary.Close();
                    writeStream.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorForm errorDiag = new ErrorForm((exception.ToString())/*.Split(':'))[0]*/, "saveBinaryFile_FileOk()", true);
                errorDiag.ShowDialog();
            }
        }
    }
}