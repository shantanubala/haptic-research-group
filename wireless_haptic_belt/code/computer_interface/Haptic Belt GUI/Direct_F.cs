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
        int current_set = 0;
        //contains all available motors (up to 16, based on value of count)
        String[] available = new String[16];
        //array indexed: [set][availabe/added][motor]
        //[0] - available motors = # (1-16)
        //[1] - added motors = # (1-16), rhythm (A-E), magnitude (A-D), cycles (0-7)
        String[,,] setItems = new String[16,2,16];

        /* Depending on what set is selected, all motors in sed set
         * are activated based on the parameters entered by the user
         * upon adding that motor to the set.
         */
        private void Activate_Set()
        {
            //Used for breaking up motor,rhythm,magnitude,cycles with split(',')
            String[] breakUp = new String[4];
            for (int i = 0; i < 16; i++)
            {
                breakUp = setItems[current_set, 1, i].Split(',');
                if (!breakUp[0].Equals("")) //if(motor is available)
                {
                    if (hasError(belt.Vibrate_Motor(Convert.ToInt16(breakUp[0]),breakUp[1],breakUp[2],Convert.ToInt16(breakUp[3])),"Vibrate_Motor()"))
                    {
                        //Handle Error
                    }
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
            for (int i = 0; i < 16; i++)
            {
                String temp = available[i];
                //Only null when 0 motors are connected to belt
                if (temp != null)
                {
                    AvailableList.Items.Add(temp);
                    setItems[current_set, 0, i] = temp;
                    setItems[current_set, 1, i] = "";
                }
            }                
            //Set labels to N/A
            AddedRhythmLabel.Text = "N/A";
            AddedCycleLabel.Text = "N/A";
            AddedMagLabel.Text = "N/A";
            
        }
        //Populates 3 lables based on selected motor in "AddedList"
        private void Change_Labels()
        {
            String[] breakUp = new String[4];
            if(AddedList.SelectedIndex > -1)
            {
                breakUp = setItems[current_set,1,Convert.ToInt32(AddedList.SelectedItem.ToString()) - 1].Split(',');
                AddedRhythmLabel.Text = breakUp[1];
                AddedMagLabel.Text = breakUp[2];
                AddedCycleLabel.Text = breakUp[3];
            }
            else
            {
                AddedRhythmLabel.Text = "N/A";
                AddedMagLabel.Text = "N/A";
                AddedCycleLabel.Text = "N/A";
            }
        }
        //Re-populates AvailableList and AddedList upon a setList change
        private void Change_Set()
        {
            if (SetList.SelectedIndex > -1)
            {
                String temp;
                current_set = SetList.SelectedIndex;
                AvailableList.Items.Clear();
                AddedList.Items.Clear();
                //Populate Availible Motor List for selected set
                for (int i = 0; i < 16; i++)
                {
                    temp = setItems[current_set, 0, i];
                    if (!temp.Equals(""))
                        AvailableList.Items.Add(temp);
                }
                //Populate Added Motor List for selected set
                for (int i = 0; i < 16; i++)
                {
                    temp = setItems[current_set, 1, i];
                    if (!temp.Equals(""))
                    {
                        //Isolate motor parameter (quicker than split function)
                        temp = temp.Remove(2); //String may be "##" or "#," after this call
                        if (temp.EndsWith(",")) //If string is "#,"
                            temp = temp.Remove(1); //Remove the ","
                        AddedList.Items.Add(temp);
                    }
                }
            }
        }
        //Checks to see if any motors are attached, and initialzes all data
        //Uses a Query so we return an int for error response
        //Returns # of attached motors on belt
        private int Populate_AvailibleList()
        {
            //Query Belt
            int motorcount = belt.getMotors(QueryType.SINGLE);
            if (hasError(belt.getError(),"getMotors()"))
            {
                //Handle Error
            }
            if (motorcount > 0)
            {
                //Initialize setItems array
                for (int i = 0; i < motorcount; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        setItems[j, 0, i] = (i+1).ToString();
                        setItems[j, 1, i] = "";
                    }
                }
                //Initialize setItems array with the rest ""
                for (int i = motorcount; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        setItems[j, 0, i] = "";
                        setItems[j, 1, i] = "";
                    }
                }
            }
            return motorcount;
        }
        //Adds an activation request to the selected set
        private void Add_Activation(String rhythm, String mag, String cycles)
        {
            //must have a selectable index
            if (AvailableList.SelectedIndex > -1)
            {
                String add;
                //Put the selected item to add into a string
                //Convert to int, (item - 1) will be its array index location
                int item = Convert.ToInt32(AvailableList.SelectedItem.ToString());
            //Remove from Available Section
                //Remove item from GUI visible list 
                AvailableList.Items.Remove(item.ToString());
                //Remove from setItems
                setItems[current_set, 0, item - 1] = "";
            //Add to Added Section
                //Add motor to setItems
                setItems[current_set, 1, item - 1] = item.ToString() + "," + rhythm + "," + mag + "," + cycles;
                //Clear AddedList Items (old list) (to preserve number order)
                AddedList.Items.Clear(); 
                //Populate AddedList Items (fresh list)
                for (int i = 0; i < 16; i++)
                {
                    add = setItems[current_set, 1, i];
                    if (!add.Equals(""))
                    {
                        //Isolate motor parameter (quicker than split function)
                        add = add.Remove(2); //String may be "##" or "#," after this call
                        if (add.EndsWith(",")) //If string is "#,"
                            add = add.Remove(1); //Remove the ","
                        AddedList.Items.Add(add);
                    }
                }
            }
        }
        //Deletes selected activation request from selected set
        private void Delete_Activation()
        {
            //must have a selectable index
            if (AddedList.SelectedIndex > -1)
            {
                String add;
                //Put the selected item to add into a string
                //Convert to int, (item - 1) will be its array index location
                int item = Convert.ToInt32(AddedList.SelectedItem.ToString());
            //Remove from Added Section
                //Remove item from GUI visible list 
                AddedList.Items.Remove(item.ToString());
                //Remove motor from setItems
                setItems[current_set, 1, item - 1] = "";
            //Add to Available Section
                //Add motor to available setItems
                setItems[current_set, 0, item - 1] = item.ToString();
                //Clear AvailableList Items (old list) (to preserve number order)
                AvailableList.Items.Clear();
                //Populate AvailableList Items (fresh list)
                for (int i = 0; i < 16; i++)
                {
                    add = setItems[current_set, 1, i];
                    if (!add.Equals(""))
                    {
                        //Isolate motor parameter (quicker than split function)
                        add = add.Remove(2); //String may be "##" or "#," after this call
                        if (add.EndsWith(",")) //If string is "#,"
                            add = add.Remove(1); //Remove the ","
                        AddedList.Items.Add(add);
                    }
                }
            }
        }
    }
}
