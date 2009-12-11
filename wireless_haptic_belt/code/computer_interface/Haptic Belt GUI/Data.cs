using System;
using System.IO;
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
        Group[] _group;

//Data structures
        //Represents the data required to specify a Rhythm
        public struct Rhythm
        {
            public String id;
            public String pattern;
            public Int16 time;
        }
        //Represents the data required to specify a Magnitude
        public struct Magnitude
        {
            public String id;
            public Int16 period;
            public Int16 dutycycle;
        }
        //Set Data Structure has a name String, a motor String[], and delay int[].
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
            public Rhythm[] rhythm;
            public Magnitude[] magnitude;
            public Set[] set;

            //Allocates and copies into new memory with a deep copy
            public Group(Group toClone)
            {
                set = (Set[])toClone.set.Clone();
                name = (String)toClone.name.Clone();
                rhythm = (Rhythm[])toClone.rhythm.Clone();
                magnitude = (Magnitude[])toClone.magnitude.Clone();
            }
        }
        //Data structure functions
        //Increases the given Set[] size by 1, and copies all data from old_set into the returned set.
        private Set[] increaseSet(Set[] old_set, String name)
        {
            int last_index;

            Set[] new_set = new Set[old_set.Length + 1];

            //Copy old contents into new contents (uses deep copy)
            for (int i = 0; i < old_set.Length; i++)
                new_set[i] = new Set(old_set[i]);

            //last index of the new array
            last_index = new_set.Length - 1;
            
            //Instantiate the Name and motor array of the last indexed newly created set
            new_set[last_index].motor = new String[_maxmotors];
            new_set[last_index].name = name;

            //Initialize newly created Set's motor array to "";
            for (int i = 0; i < _maxmotors; i++)
                new_set[last_index].motor[i] = "";

            return new_set;
        }
        //Increases the given Group[] size by 1, and copies all data from old_set into the returned set.
        private Group[] increaseGroup(Group[] old_group, String name)
        {
            int last_index;
            char id;

            Group[] new_group = new Group[old_group.Length + 1];

            //Copy old contents into new contents (uses deep copy)
            for (int i = 0; i < old_group.Length; i++)
                new_group[i] = new Group(old_group[i]);

            //last index of the new array
            last_index = new_group.Length - 1;

            //Initialize the name of the new group
            new_group[last_index].name = name;
            new_group[last_index].set = new Set[0];
            new_group[last_index].rhythm = new Rhythm[5];
            new_group[last_index].magnitude = new Magnitude[4];

            for (int i = 0; i < 5; i++)
            {
                id = (char)(i + 65); //ASCII 65-69 = A-E

                new_group[last_index].rhythm[i].id = id.ToString(); //A B C D E
                new_group[last_index].rhythm[i].pattern = "0"; /* Default to Rythm of OFF for 50ms */
                new_group[last_index].rhythm[i].time = 1;
                if (i != 4) //Magnitude is only of size 4, so skip on last index
                {
                    new_group[last_index].magnitude[i].id = id.ToString(); //A B C D
                    new_group[last_index].magnitude[i].period = 2000; // Default to Magnitude of 0% 
                    new_group[last_index].magnitude[i].dutycycle = 0;
                }
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
                new_set[i - 1] = new Set(old_set[i]);

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
                     * Save/Load Order Format:
                     * 
                     * Int32 - Size of saved _group[] array (variable length)
                     *  String - "name" of _group[i] array
                     *  Int32 - Size of saved _group[i].rhythm[] array (should always be 5)
                     *      String - "id" of _group[i].rhythm[j]
                     *      String - "pattern" of _group[i].rhythm[j]
                     *      Int16 - "time" of _group[i].rhythm[j]
                     *  Int32 - Size of saved _group[i].magnitude[] array (should always be 4)
                     *      String - "id" of _group[i].magnitude[j]
                     *      Int16 - "period" of _group[i].magnitude[j]
                     *      Int16 - "dutycycle" of _group[i].magnitude[j]
                     *  Int32 - Size of saved _group[i].set[] array (variable length)
                     *      String - "name" of _group[i].set[j]
                     *      Int32 - Size of saved _group[i].set[j].motor[] array (should always be 16)
                     *          String - "motor" of _group[i].set[j].motor[k] 
                     */

                    _group = new Group[readBinary.ReadInt32()];
                    //Populate Group array
                    for (int i = 0; i < _group.Length; i++)
                    {
                        _group[i].name = readBinary.ReadString();
                        _group[i].rhythm = new Rhythm[readBinary.ReadInt32()];
                        //Populate Rhythm Array
                        for (int j = 0; j < _group[i].rhythm.Length; j++)
                        {
                            _group[i].rhythm[j].id = readBinary.ReadString();
                            _group[i].rhythm[j].pattern = readBinary.ReadString();
                            _group[i].rhythm[j].time = readBinary.ReadInt16();
                        }
                        _group[i].magnitude = new Magnitude[readBinary.ReadInt32()];
                        //Populate Magnitude Array
                        for (int j = 0; j < _group[i].magnitude.Length; j++)
                        {
                            _group[i].magnitude[j].id = readBinary.ReadString();
                            _group[i].magnitude[j].period = readBinary.ReadInt16();
                            _group[i].magnitude[j].dutycycle = readBinary.ReadInt16();
                        }
                        _group[i].set = new Set[readBinary.ReadInt32()];
                        //Populate Sets
                        for (int j = 0; j < _group[i].set.Length; j++)
                        {
                            _group[i].set[j].name = readBinary.ReadString();
                            _group[i].set[j].motor = new String[readBinary.ReadInt32()];
                            //Populate Motors
                            for (int k = 0; k < _group[i].set[j].motor.Length; k++)
                                _group[i].set[j].motor[k] = readBinary.ReadString();
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
                     * Save/Load Order Format:
                     * 
                     * Int32 - Size of saved _group[] array (variable length)
                     *  String - "name" of _group[i] array
                     *  Int32 - Size of saved _group[i].rhythm[] array (should always be 5)
                     *      String - "id" of _group[i].rhythm[j]
                     *      String - "pattern" of _group[i].rhythm[j]
                     *      Int16 - "time" of _group[i].rhythm[j]
                     *  Int32 - Size of saved _group[i].magnitude[] array (should always be 4)
                     *      String - "id" of _group[i].magnitude[j]
                     *      Int16 - "period" of _group[i].magnitude[j]
                     *      Int16 - "dutycycle" of _group[i].magnitude[j]
                     *  Int32 - Size of saved _group[i].set[] array (variable length)
                     *      String - "name" of _group[i].set[j]
                     *      Int32 - Size of saved _group[i].set[j].motor[] array (should always be 16)
                     *          String - "motor" of _group[i].set[j].motor[k] 
                     */

                    writeBinary.Write(_group.Length);
                    //Populate Group array
                    for (int i = 0; i < _group.Length; i++)
                    {
                        writeBinary.Write(_group[i].name);
                        writeBinary.Write(_group[i].rhythm.Length);
                        //Populate Rhythm Array
                        for (int j = 0; j < _group[i].rhythm.Length; j++)
                        {
                            writeBinary.Write(_group[i].rhythm[j].id);
                            writeBinary.Write(_group[i].rhythm[j].pattern);
                            writeBinary.Write(_group[i].rhythm[j].time);
                        }
                        writeBinary.Write(_group[i].magnitude.Length);
                        //Populate Magnitude Array
                        for (int j = 0; j < _group[i].magnitude.Length; j++)
                        {
                            writeBinary.Write(_group[i].magnitude[j].id);
                            writeBinary.Write(_group[i].magnitude[j].period);
                            writeBinary.Write(_group[i].magnitude[j].dutycycle);
                        }
                        writeBinary.Write(_group[i].set.Length);
                        //Populate Sets
                        for (int j = 0; j < _group[i].set.Length; j++)
                        {
                            writeBinary.Write(_group[i].set[j].name);
                            writeBinary.Write(_group[i].set[j].motor.Length);
                            //Populate Motors
                            for (int k = 0; k < _group[i].set[j].motor.Length; k++)
                                writeBinary.Write(_group[i].set[j].motor[k]);
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