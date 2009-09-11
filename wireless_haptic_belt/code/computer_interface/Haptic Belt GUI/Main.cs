using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Haptic_Belt_Library;
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
namespace HapticBelt
{        
    public partial class GUI : Form
    { 
        Library belt = new Library(); //Library functionality
        String[] response = new String[2]; //Return array for all Library calls
        Boolean COM_Available = false; //represents availablity of a COM port
        Boolean Port_Open = false; //represents if a port has been initialized

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
            String[] ports = belt.Get_Available_Ports();
            for (int i = 0; i < ports.Length; i++)
            {
//QUICK FIX for some reason on connection of BT comports have an additional o.
//EG: COM6o is returned...
                if(ports[i].Contains("o"))
                    COMComboBox.Items.Add(ports[i].Substring(0,ports[i].Length - 1));
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
            if (Port_Open)
            {
                String mode = ModeComboBox.SelectedItem.ToString();
                if (mode.Equals("Rhythm Mode"))
                {
                    Hide_Select_Mode();
                    Show_Rhythm_Mode();
                }
                else if (mode.Equals("Magnitude Mode"))
                {
                    Hide_Select_Mode();
                    Show_Magnitude_Mode();
                }
                else if (mode.Equals("Direct Operation Mode"))
                {
                    Hide_Select_Mode();
                    Show_Direct_Mode();
                }
            }
        }
        //Calls library to Initialize a port, if none are open, and one is available
        private void OpenPort_Click(object sender, EventArgs e)
        {
            //If there is a COM available, and no port is open, open a port!
            if (!Port_Open && COM_Available && (COMComboBox.SelectedIndex > -1))
            {
ErrorStatus.Text = "Error Status: " + "Waiting for Initialize_Serial_Port() to respond";
ErrorLocation.Text = "Error Location: " + "Calling Initialize_Serial_Port()";
                response = belt.Initialize_Serial_Port(COMComboBox.SelectedItem.ToString(), "9600", "None", "1", "8");
ErrorStatus.Text = "Error Status: " + response[0];
                if (!response[0].Equals(""))
                {
                    //ERROR
                }
                else
                {
ErrorLocation.Text = "Error Location: ";
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
                belt.Close_Serial_Port();
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
    }
}