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
        //Handles all necessary conditions to display this panel 
        private void Show_Magnitude_Mode()
        {
            Populate_Magnitude(MagComboBox.SelectedItem.ToString());
            MagPanel.Show();
        }
        //Handles all necessary conditions to hide this panel
        private void Hide_Magnitude_Mode()
        {
            MagPanel.Hide();
        }
        //Displays the Selection Mode Panel (Main Menu)
        private void MagBack_Click(object sender, EventArgs e)
        {
            Hide_Magnitude_Mode();
            Show_Select_Mode();
        }
        //FIXME Library function not yet implemented
        private void MagTest_Click(object sender, EventArgs e)
        {
 //           belt.Test_Magnitude(Period.Value.toString(),DutyCycle.Value.toString());
        }
        //Calls Library function to learn Magnitude based on user values
        private void MagLearn_Click(object sender, EventArgs e)
        {
ErrorStatus.Text = "Error Status: " + "Waiting on Learn_Magnitude() to respond";
ErrorLocation.Text = "Error Location: " + "Calling Learn_Magnitude()";
            response = belt.Learn_Magnitude(MagComboBox.SelectedItem.ToString(),Convert.ToInt32(Period.Value).ToString(),Convert.ToInt32(DutyCycle.Value).ToString());
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
        //Duty Cycle can be at most the value of Period, we must uphold this here
        private void Period_ValueChanged(object sender, EventArgs e)
        {
            Change_Maximum_DutyCycle();         
        }
        //Converts the percentage entered into a DutyCyle value
        private void Percentage_ValueChanged(object sender, EventArgs e)
        {
            DutyCycle.Value = Period.Value * (Percentage.Value / 100);
        }
        //Shows/Hides Advanced Options Fields/Parameters
        private void MagOption_CheckedChanged(object sender, EventArgs e)
        {
            if (MagOption.Checked)
            {
                Show_Options();
            }
            else
            {
                Hide_Options();
            }
        }
    }
}
