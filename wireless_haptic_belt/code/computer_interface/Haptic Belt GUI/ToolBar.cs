using System;
using System.Windows.Forms;
using HapticDriver;
using System.Threading;

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
        
        //For these next two variables the larger their value the better response the application has, but it will take more CPU resources consequently.
        //These values represent int parameters used in Thread.Sleep(), these Thread.Sleep()'s are used for busy waiting around a while loop.
        int responseTime; //Used for checking when to break out of a loop to activate the next Event in a Group activation (recommended value < 25)
        int sleepTime; //Used for waiting for the user to hit the "Stop" button, this parameter should be set to roughly 1/2 the time it would take for a user to click "Stop" and then click "Activate".

        Thread activate_trd; //Thread used for all activations, note that only one activation can be present at a time
        Thread stop_trd; //Thread used to wait for activate_trd to stop, this insures syncronization

        public GUI()
        {
            InitializeComponent();
            //Initialize internal data structure
            _group = new Group[0];
            //Initialize Values
            belt = new HapticBelt();
            Port_Open = false;
            COM_Available = false;
            responseTime = 5;
            sleepTime = 100;

            //Initialize threads to something, so they will not throw null exceptions, thus eliminating the need for checking if they are null.
            activate_trd = new Thread(new ThreadStart(this.Activate_Activation));
            stop_trd = new Thread(new ThreadStart(this.Stop_Activations));
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
        //Called as the form is closing, ensures all threads are closed properly and belt is no longer vibrating
        private void GUI_FormClosing(object sender, EventArgs e)
        {
            if (activate_trd.IsAlive && !stop_trd.IsAlive) //If true a motor is currently in an activation state, and no stop command has been issued
                Stop_Activations(); //Send out the stop command, this will close activate_trd and block until it is closed
            else if (stop_trd.IsAlive) //Stop command was in process, this may never happen on human reaction time
                stop_trd.Join(); //Blocks until stop_trd thread is done 
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
                    }
                }
            }
        }

        private void realTimeDelayValueMenu_TextChanged(object sender, EventArgs e)
        {
            try
            {
                responseTime = Convert.ToInt32(realTimeDelayValueMenu.Text);
            }
            catch(FormatException)
            {
                realTimeDelayValueMenu.Text = "";
            }
        }
        //Updates _motorCount, loads rhythms and magnitudes, and updates the version of the belt
        private void reinitializeBeltMenu_Click(object sender, EventArgs e)
        {
            belt.ResetHapticBelt();
            
            GetMotors();
            GetVersion();

            if (GroupList.SelectedIndex > -1)
                LoadBelt();
        }
    }
}