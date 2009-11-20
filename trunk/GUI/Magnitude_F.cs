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
        //Queries Library to get the Learned magnitudes, and populate fields
        private void Populate_Magnitude(string sel)
        {
ErrorStatus.Text = "Error Status: " + "Waiting for Query_Magnitude() to respond";
ErrorLocation.Text = "Error Location: " + "Calling Query_Magnitude()";
            response = belt.Query_Magnitude(sel);
ErrorStatus.Text = "Error Status: " + response[0];
            if(!(response[0].Equals("")))
            { 
//ERROR
            }
            else
            {
ErrorLocation.Text = "Error Location: ";
                String[] splitMag = new String[2];
                splitMag = response[1].Split(',');
                Period.Value = Convert.ToInt32(splitMag[0]);
                DutyCycle.Value = Convert.ToInt32(splitMag[1]);
                Percentage.Value = (DutyCycle.Value / Period.Value) * 100;
            }
        }
        //Upholds the truth DutyCycle must be <= Period at all times
        private void Change_Maximum_DutyCycle()
        {
            //make sure user cant enter a duty cycle > period 
            if (Period.Value < DutyCycle.Value)
            {
                DutyCycle.Value = Period.Value;
            }
            DutyCycle.Maximum = Period.Value;
        }
        //Show Advanced Options Content
        private void Show_Options()
        {
            //Show advanced options (duty cycle and period)
            DutyCycle.Show();
            DutyLabel.Show();
            Period.Show();
            PeriodLabel.Show();
            PeriodDefaultLabel.Show();
            //Hide Percentage
            Percentage.Hide();
            PercentLabel.Hide();
        }
        //Hides Advanced Options Content
        private void Hide_Options()
        {
            //Hide advanced options
            DutyCycle.Hide();
            DutyLabel.Hide();
            Period.Hide();
            PeriodLabel.Hide();
            PeriodDefaultLabel.Hide();
            //Show Percentage
            Percentage.Show();
            PercentLabel.Show();
        }
    }
}
