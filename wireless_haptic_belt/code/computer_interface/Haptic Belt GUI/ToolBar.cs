using System;
using System.IO;
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
        HapticBelt belt; //Library functionality
        Boolean COM_Available; //represents availablity of a COM port
        Boolean Port_Open; //represents if a port has been initialized

        public GUI()
        {
            InitializeComponent();
            //Initialize internal data structure
            _group = new Group[0];
            //Initialize Values
            belt = new HapticBelt();
            Port_Open = false;
            COM_Available = false;
        }
        //Populates COM comboBox upon GUI load
        private void GUI_Load(object sender, EventArgs e)
        {
            //Clear ComboBox
            COMComboBoxMenu.Items.Clear();
            //Populate ComboBox w/ COM port list
            String[] ports = belt.GetSerialPortNames();
            if (ports.Length > 0)
            {
                for (int i = 0; i < ports.Length; i++)
                    COMComboBoxMenu.Items.Add(ports[i]);
                COM_Available = true;
            }
        }

        //Checks for error based on given error_t param, errorLOC param is used for specificying the location of the error for debugging
        private bool hasError(error_t error, String errorLOC)
        {
            if (error == error_t.ESUCCESS)
                return false;
            else
            {
                ErrorForm errorForm = new ErrorForm(belt.getErrorMsg(error), errorLOC, true);
                errorForm.ShowDialog();
                return true;
            }
        }   

        private void refreshPortsMenu_Click(object sender, EventArgs e)
        {
            GUI_Load(sender, e);
        }

        private void disconnectMenu_Click(object sender, EventArgs e)
        {
            if (Port_Open)
            {
                belt.ClosePorts();
                Port_Open = false;
                
                //Enable/Disable Corresponding options to having no port open
                disconnectMenu.Enabled = false;
                refreshPortsMenu.Enabled = true;
                COMComboBoxMenu.Enabled = true;
                ActivateMotor.Enabled = false;
                ActivateSet.Enabled = false;
                ActivateGroup.Enabled = false;
                Stop.Enabled = false;
            }
        }
        //Calls library to Initialize a port, if none are open, and one is available
        private void COMComboBoxMenu_Click(object sender, EventArgs e)
        {
            //If there is a COM available, and no port is open, open a port!
            if (!Port_Open && COM_Available && (COMComboBoxMenu.SelectedIndex > -1))
            {
                String com = COMComboBoxMenu.SelectedItem.ToString();
                if (hasError(belt.SetupPorts(com, com, "9600", "8", "1", "None", "1000"), "SetupPorts()"))
                {
                    //Handle Error
                }
                else
                {
                    _motorcount = belt.getMotors(QueryType.SINGLE);
                    if (hasError(belt.getStatus(), "belt.getMotors()"))
                    {
                        //Handle Error
                    }
                    else
                    {
                        Port_Open = true;
                        
                        //Enable/Disable Corresponding options to an open port
                        disconnectMenu.Enabled = true;
                        refreshPortsMenu.Enabled = false;
                        COMComboBoxMenu.Enabled = false;
                        ActivateMotor.Enabled = true;
                        ActivateSet.Enabled = true;
                        ActivateGroup.Enabled = true;
                        Stop.Enabled = true;

                        firmwareVersionMenu.Text = belt.getVersion(QueryType.SINGLE);
                        if (hasError(belt.getStatus(), "belt.getVersion()"))
                        {
                            //Handle Error
                        }
                    }
                }
            }
        }

        private void loadMenu_Click(object sender, EventArgs e)
        {
            loadBinaryFile.ShowDialog();
        }

        private void saveMenu_Click(object sender, EventArgs e)
        {
            saveBinaryFile.ShowDialog();
        }
    }
}