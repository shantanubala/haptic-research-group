using System;
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
        int sleepTime;

        public GUI()
        {
            InitializeComponent();
            //Initialize internal data structure
            _group = new Group[0];
            //Initialize Values
            belt = new HapticBelt();
            Port_Open = false;
            COM_Available = false;
            sleepTime = 1;
        }
        //Populates COM comboBox upon GUI load
        private void GUI_Load(object sender, EventArgs e)
        {
            //Clear ComboBox
            outgoingCOMComboBox.Items.Clear();
            incomingCOMComboBox.Items.Clear();
            //Populate ComboBox w/ COM port list
            
            String[] ports = belt.GetSerialPortNames();
            if (ports.Length > 0)
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    outgoingCOMComboBox.Items.Add(ports[i]);
                    incomingCOMComboBox.Items.Add(ports[i]);
                }
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
                connect.Enabled = true;
                disconnect.Enabled = false;
                refreshPorts.Enabled = true;
                outgoingCOMComboBox.Enabled = true;
                incomingCOMComboBox.Enabled = true;
                ActivateActivation.Enabled = false;
                ActivateGroup.Enabled = false;
                Stop.Enabled = false;
                Initialize.Enabled = false;
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
        //Calls library to Initialize and open a port, if none are open, and one is available
        private void connect_Click(object sender, EventArgs e)
        {
            if (!Port_Open && COM_Available && (outgoingCOMComboBox.SelectedIndex > -1) && (incomingCOMComboBox.SelectedIndex > -1))
            {     
                String out_com = outgoingCOMComboBox.SelectedItem.ToString();
                String in_com = incomingCOMComboBox.SelectedItem.ToString();

                if (hasError(belt.SetupPorts(in_com, out_com, "9600", "8", "1", "None", 1000), "belt.SetupPorts()"))
                {
                    //Handle Error
                }
                else
                {
                    if (hasError(belt.OpenPorts(), "belt.OpenPorts()"))
                    {
                        //Handle Error
                    }
                    else
                    {
                        Port_Open = true;

                        //Enable/Disable  Corresponding options to an open port
                        connect.Enabled = false;
                        disconnect.Enabled = true;
                        refreshPorts.Enabled = false;
                        outgoingCOMComboBox.Enabled = false;
                        incomingCOMComboBox.Enabled = false;
                        ActivateActivation.Enabled = true;
                        ActivateGroup.Enabled = true;
                        Stop.Enabled = true;
                        Initialize.Enabled = true;
                    }
                }
            }
        }

        private void realTimeDelayValueMenu_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sleepTime = Convert.ToInt32(realTimeDelayValueMenu.Text);
            }
            catch(FormatException)
            {
                realTimeDelayValueMenu.Text = "";
            }
        }      
    }
}