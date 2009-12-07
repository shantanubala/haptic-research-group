using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HapticDriver;
/* Description: This class, along with all its partial classes provides a
 * front end to the Haptic_Belt_Library to program the Haptic Belt. A user
 * may define multiple Magnitudes, Rhythms, and activate motors in group
 * configurations.
 * 
 * Style: There are multiple Partial Classes for the GUI Class. There are two
 * partial classes per panel (with the exception of the main panel having one).
 * 
 * User Interaction(UI): action events, listeners, call learning functions (DLL).
 * Functionality(F): parsing, maintain visuals, call querry functions (DLL).
 * 
 * The purpose of this style is so somone can view the (UI) extension and
 * obtain a good idea of how the class works without understanding the
 * implementation (F).
 */
namespace HapticGUI
{        
    public partial class GUI : Form
    { 
        HapticBelt belt = new HapticBelt(); //Library functionality
        error_t response = new error_t(); //Return array for all Library calls
        Boolean COM_Available = false; //represents availablity of a COM port
        Boolean Port_Open = true; //represents if a port has been initialized
        String hold_magnitude; //Variable to hold belt magnitude while testing

        public GUI()
        {
            InitializeComponent();
        }
        //Populates COM comboBox upon GUI load
        private void GUI_Load(object sender, EventArgs e)
        {
            //Clear ComboBox
            COMComboBox.Items.Clear();
            //Populate ComboBox w/ COM port list
            String[] ports = belt.GetSerialPortNames();
            if (ports.Length > 0)
            {
                for (int i = 0; i < ports.Length; i++)
                    COMComboBox.Items.Add(ports[i]);
                COM_Available = true;
            }
        }
        //Displays Main Panel
        private void Show_Select_Mode()
        {
            MainPanel.Show();
        }
        //Hides Main Panel
        private void Hide_Select_Mode()
        {
            MainPanel.Hide();
        }
        //Allows access to other modes (panels), if a port is open
        private void ModeGo_Click(object sender, EventArgs e)
        {
            String mode = ModeComboBox.SelectedItem.ToString();

            if (Port_Open)
            {
                Hide_Select_Mode();

                if (mode.Equals("Rhythm Mode"))
                    Show_Rhythm_Mode();
                else if (mode.Equals("Magnitude Mode"))
                    Show_Magnitude_Mode();
                else if (mode.Equals("Direct Operation Mode"))
                    Show_Operation_Mode();
                else
                    Show_Program_Mode();
            }
            else if (mode.Equals("Direct Program Mode"))
                Show_Program_Mode();
        }
        //Calls library to Initialize a port, if none are open, and one is available
        private void OpenPort_Click(object sender, EventArgs e)
        {
            //If there is a COM available, and no port is open, open a port!
            if (!Port_Open && COM_Available && (COMComboBox.SelectedIndex > -1))
            {
                String com = COMComboBox.SelectedItem.ToString();
                if (hasError(belt.SetupPorts(com, com, "9600", "8", "1", "None", "1000"), "SetupPorts()"))
                {
                    //Handle Error
                }
                else
                {
                    Port_Open = true;
                    OpenPort.Hide();
                    ClosePort.Show();
                }
            }
        }

        private void ClosePort_Click(object sender, EventArgs e)
        {
            if (Port_Open)
            {
                belt.ClosePorts();
                Port_Open = false;
                OpenPort.Show();
                ClosePort.Hide();
            }
        }

        private void RefreshPorts_Click(object sender, EventArgs e)
        {
            GUI_Load(sender, e);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        //Checks for error based on given error_t param, errorLOC param is used for specificying the location of the error for debugging
        private bool hasError(error_t error, String errorLOC)
        {
            if (error == error_t.ESUCCESS)
                return false;
            else
            {
                ErrorLocation.Text = "Error Location: " + errorLOC;
                ErrorStatus.Text = belt.getErrorMsg(response);
                return true;
            }
        }
        private void dispError(String errorLOC)
        {
           ErrorLocation.Text = "Error Location: " + errorLOC;
           ErrorStatus.Text = belt.getErrorMsg(response);     
        }   
        
        //Check for a particular error
        private bool hasError(error_t error, String errorLOC, error_t check)
        {
            if (error != check)
                return false;
            else
            {
                ErrorLocation.Text = "Error Location: " + errorLOC;
                ErrorStatus.Text = belt.getErrorMsg(response);
                return true;
            }
        }

        private void DirectDelayField_ValueChanged(object sender, EventArgs e)
        {
            if(DirectDelayField.Value%50 != 0)
                DirectDelayField.Value = Convert.ToInt32(DirectDelayField.Value) / 50 * 50;
        }

        
    }
}