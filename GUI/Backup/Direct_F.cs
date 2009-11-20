using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HapticBelt
{
    partial class GUI
    {
        int current_set = 0;
        //contains all available motors (up to 16, based on value of count)
        String[] available = new String[16];
        //array indexed: [set][0-1][count]
        //[0] - available motors
        //[1] - added motors
        //[2] - rhythm,magnitude,cycles (exists if a motor at [1] is present)
        String[,,] setItems = new String[16,3,16];

        /* Depending on what set is selected, all motors in sed set
         * are activated based on the parameters entered by the user
         * upon adding that motor to the set.
         */
        private void Activate_Set()
        {
            //Used for breaking up rhythm,magnitude,cycles with split(',')
            String[] breakUp = new String[3];
            String motor;
            for (int i = 0; i < 16; i++)
            {
                motor = setItems[current_set, 1, i];
                if (!motor.Equals(""))
                {
                    breakUp = setItems[current_set, 2, i].Split(',');
ErrorStatus.Text = "Error Status: " + "Waiting on Vibrate_Motor() to respond";
ErrorLocation.Text = "Error Location: " + "Calling Vibrate_Motor()";
                    response = belt.Vibrate_Motor(motor,breakUp[0],breakUp[1],breakUp[2]);
ErrorStatus.Text = "Error Status: " + response[0];
                    if (!response[0].Equals(""))
                    {
                        //ERROR
                    }
                    else
                    {
ErrorLocation.Text = "Error Location: ";
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
                    setItems[current_set, 2, i] = "";
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
            String[] breakUp = new String[3];
            if(AddedList.SelectedIndex > -1)
            {
                breakUp = setItems[current_set,2,Convert.ToInt32(AddedList.SelectedItem.ToString()) - 1].Split(',');
                AddedRhythmLabel.Text = breakUp[0];
                AddedMagLabel.Text = breakUp[1];
                AddedCycleLabel.Text = breakUp[2];
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
                        AddedList.Items.Add(temp);
                }
            }
        }
        //Checks to see if any motors are attached, and initialzes all data
        //Uses a Query so we return an int for error response
        //if return = 0 ok, else bad
        private int Populate_AvailibleList()
        {
            int status = 0;
            //Query Belt
ErrorStatus.Text = "Error Status: " + "Waiting on Query_Motors() to respond";
ErrorLocation.Text = "Error Location: " + "Calling Query_Motors()";
            response = belt.Query_Motors();
ErrorStatus.Text = "Error Status: " + response[0];
            if (!response[0].Equals(""))
            {
                status = -1;
                //ERROR
            }
            else
            {
ErrorLocation.Text = "Error Location: ";
            }
            if (response[1].Equals(""))
            {
//CRITICAL ERROR: No motors are attached (Maybe use boolean to de-activate buttons)
                //Initialize setItems & available array to "" (No motors attached)
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        setItems[j, 0, i] = "";
                        setItems[j, 1, i] = "";
                        setItems[j, 2, i] = "";
                    }
                }                
            }
            else
            {
                //Convert response to integer
                int motors = Convert.ToInt32(response[1]); 
                //Initialize setItems array
                for (int i = 0; i < motors; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        setItems[j, 0, i] = (i+1).ToString();
                        setItems[j, 1, i] = "";
                        setItems[j, 2, i] = "";
                    }
                }
                //Initialize setItems array with the rest ""
                for (int i = motors; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        setItems[j, 0, i] = "";
                        setItems[j, 1, i] = "";
                        setItems[j, 2, i] = "";
                    }
                }
            }
            return status;
        }
        //Adds an activation request to the selected set
        private void Add_Activation(String rhythm, String mag, String cycles)
        {
            //must have a selectable index
            if (AvailableList.SelectedIndex > -1)
            {
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
                setItems[current_set, 1, item - 1] = item.ToString();
                //Add other attributes to setItems
                setItems[current_set, 2, item - 1] = rhythm + "," + mag + "," + cycles;
                //Clear AddedList Items (old list)
                AddedList.Items.Clear(); 
                //Populate AddedList Items (fresh list)
                String add;
                for (int i = 0; i < 16; i++)
                {
                    add = setItems[current_set, 1, i];
                    if (!add.Equals(""))
                        AddedList.Items.Add(add);
                }
            }
        }
        //Deletes selected activation request from selected set
        private void Delete_Activation()
        {
            //must have a selectable index
            if (AddedList.SelectedIndex > -1)
            {
                //Put the selected item to add into a string
                //Convert to int, (item - 1) will be its array index location
                int item = Convert.ToInt32(AddedList.SelectedItem.ToString());
            //Remove from Added Section
                //Remove item from GUI visible list 
                AddedList.Items.Remove(item.ToString());
                //Remove motor from setItems
                setItems[current_set, 1, item - 1] = "";
                //Remove other attributes from setItems
                setItems[current_set, 2, item - 1] = "";
            //Add to Available Section
                //Add motor to available setItems
                setItems[current_set, 0, item - 1] = item.ToString();
                //Clear AvailableList Items (old list)
                AvailableList.Items.Clear();
                //Populate AvailableList Items (fresh list)
                String add;
                for (int i = 0; i < 16; i++)
                {
                    add = setItems[current_set, 0, i];
                    if (!add.Equals(""))
                    {
                        AvailableList.Items.Add(add);
                    }
                }
            }
        }
    }
}
