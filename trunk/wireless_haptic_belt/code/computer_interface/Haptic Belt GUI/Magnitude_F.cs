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
        //Queries Library to get the Learned magnitudes, and populate fields
        private void Populate_Magnitude(string sel)
        {
            String[] splitMag = new String[2];
            splitMag = belt.getMagnitude(sel,true,QueryType.SINGLE).Split(',');
            if (hasError(belt.getStatus(), "getMagnitude()"))
            {
                //Handle Error
            }
            else
            {
                Period.Value = Convert.ToInt32(splitMag[0]);
                DutyCycle.Value = Convert.ToInt32(splitMag[1]);
                Percentage.Value = (DutyCycle.Value / Period.Value) * 100;
            }
        }
        //Upholds the truth DutyCycle must be <= Period at all times, and update percentage
        private void Change_Period()
        {
            //make sure user cant enter a duty cycle > period 
            if (Period.Value < DutyCycle.Value)
            {
                DutyCycle.Value = Period.Value;
                Percentage.Value = 100;
            }
            else
            {
                Percentage.Value = (DutyCycle.Value / Period.Value) * 100;
            }
            DutyCycle.Maximum = Period.Value;
        }
        //Show Advanced Options Content
        private void Add_Options()
        {
            //Show advanced options (duty cycle and period)
            DutyCycle.Show();
            DutyLabel.Show();
            Period.Show();
            PeriodLabel.Show();
            PeriodDefaultLabel.Show();
        }
        //Hides Advanced Options Content
        private void Remove_Options()
        {
            //Hide advanced options (duty cycle and period)
            DutyCycle.Hide();
            DutyLabel.Hide();
            Period.Hide();
            PeriodLabel.Hide();
            PeriodDefaultLabel.Hide();
        }
    }
}
