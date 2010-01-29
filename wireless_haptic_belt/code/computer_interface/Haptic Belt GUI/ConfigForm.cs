using System;
using System.IO;
using System.Windows.Forms;

/* ConfigForm - Used to switch motors in the GUI internal memory to try new configurations or to match up a previously intended 
 * order that may have been changed by physically moving the motors on the belt.
 */ 

namespace HapticGUI
{
    public partial class ConfigForm : Form
    {
        GUI.Motor[] original_motors; //used for backup of original state
        GUI.Motor[] motors; //used to modify

        public ConfigForm(GUI.Motor[] incomming_motors)
        {
            InitializeComponent();
            original_motors = (GUI.Motor[])incomming_motors.Clone();
            motors = (GUI.Motor[])incomming_motors.Clone();
            
        }

        public GUI.Motor[] getMotors()
        {
            return motors;
        }

        private void MotorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int swapA, swapB;
            String store_name;
            GUI.Activation[] store;

            if (MotorList.SelectedIndices.Count == 2)
            {
                //Store First Selected Index, remove and insert
                swapA = MotorList.SelectedIndex;
                store_name = MotorList.SelectedItem.ToString();
                
                MotorList.Items.RemoveAt(swapA);
                MotorList.Items.Insert(swapA, MotorList.SelectedItem.ToString());

                //Store Second Selected Index, remove and insert
                swapB = MotorList.SelectedIndex;

                MotorList.Items.RemoveAt(swapB);

                MotorList.Items.Insert(swapB, store_name);

                //Swap the data in the data structure
                store = (GUI.Activation[])motors[swapA].activations.Clone();
                motors[swapA].activations = (GUI.Activation[])motors[swapB].activations.Clone();
                motors[swapB].activations = store;
            }
        }

        private void ConfigDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfigReset_Click(object sender, EventArgs e)
        {
            motors = (GUI.Motor[])original_motors.Clone();
            MotorList.Items.Clear();
            for (int i = 0; i < motors.Length; i++)
                MotorList.Items.Add((i + 1).ToString());
        }
    }
}
