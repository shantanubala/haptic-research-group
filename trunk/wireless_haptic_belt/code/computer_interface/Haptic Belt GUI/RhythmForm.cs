using System;
using System.Windows.Forms;
using HapticDriver;

namespace HapticGUI
{
    partial class RhythmForm : Form
    {
        String hold_magnitude; //Variable to hold belt magnitude while testing
        GUI.Rhythm[] rhythm;
        HapticBelt belt;
        int _motorcount;
        
        public RhythmForm(GUI.Rhythm[] incoming_rhythms,HapticBelt incoming_belt, Boolean connected)
        {
            InitializeComponent();

            //Sets Globals from parameters
            rhythm = incoming_rhythms;
            belt = incoming_belt;
            _motorcount = 0;

            //Disables/Enables functions based on connectivity boolean passed
            RhythmTest.Enabled = connected;
            RhythmTestStop.Enabled = connected;     
        }

        //graphics can only be written when the Form is visable, thus Shown event is perfect for such a case. 
        private void RhythmForm_Shown(object sender, EventArgs e)
        {
            //Initiates the Rhythm Panel by Clearing then Populating the 
            //selection box and painting the represented patern.     
            Clear_Rhythm();
            Populate_Rhythm();
            Paint_Rythm();
        }


        public GUI.Rhythm[] getRhythms()
        {
            return rhythm;
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
        //Exits
        private void RhythmDone_Click(object sender, EventArgs e)
        {
            Close();
        }
        //Calls the Library to learn the Rhythm specified by the listBox
        private void RhythmSet_Click(object sender, EventArgs e)
        {
            if (pairs != 0)
            {
                String[] pattern = Get_Pattern().Split(',');
                rhythm[RhythmComboBox.SelectedIndex].pattern = pattern[0];
                rhythm[RhythmComboBox.SelectedIndex].time = Convert.ToInt16(pattern[1]);
            }
            else
            {
                ErrorForm errorForm = new ErrorForm("Rhythm length must be at least 50ms", "RhythmSet_Click()", false);
                errorForm.ShowDialog(); 
            }
        }
        //Learns and plays Rhythm on belt using Rhythm H and Learns 100% Magnitude on setting A, and replaces A at the end
        //Note that we must use TimeSpan, so that we can replace the original Magnitude upon completion
        private void RhythmTest_Click(object sender, EventArgs e)
        {
            if (pairs != 0)
            {
                //Hide Rhythm Buttons so no interference will occur
                RhythmTest.Hide();
                ControlBox = false;
                RhythmDone.Enabled = false;

                //Get the User Inputed Pattern
                String[] pattern = Get_Pattern().Split(',');
                
                //Learn the test Rhythm to temp spot "H"
                if(hasError(belt.Learn_Rhythm("H", pattern[0],Convert.ToInt16(pattern[1]),true),"Learn_Rhythm()"))
                {
                    //Handle Error
                }

                //Store the current Magnitude
                hold_magnitude = belt.getMagnitude("A",true, QueryType.SINGLE);
                if(hasError(belt.getStatus(),"getMagnitude()"))
                {
                    //Handle Error
                }
                //Learn a 100% Magnitude setting
                if(hasError(belt.Learn_Magnitude("A",100),"Learn_Magnitude()"))
                {
                    //Handle Error
                }
                //Get motor count
                _motorcount = belt.getMotors(QueryType.SINGLE);
                if (hasError(belt.getStatus(), "getMotors()"))
                {
                    //Handle Error
                }
                //Vibrate all available motors on belt with test Rhythm and 100% Magnitude indefinately
                for(int i = 0; i < _motorcount; i++)
                {
                    if(hasError(belt.Vibrate_Motor(i,"H","A",7),"Vibrate_Motor()"))
                    {
                        //Handle Error
                    }
                }
                //Wait for motors to finish vibrating or user to click "Stop" on RhythmTestStop Button
                RhythmTestStop.Show();
            }            
        }
        private void RhythmTestStop_Click(object sender, EventArgs e)
        {
            String[] split_magnitude = new String[2];
            //Issue a stop command to all motors on the belt
            if (hasError(belt.StopAll(), "belt.StopAll()"))
            {
                //Handle Error
            }
            //Reset original state of magnitude "A"
            split_magnitude = hold_magnitude.Split(',');
            if (hasError(belt.Learn_Magnitude("A", Convert.ToUInt16(split_magnitude[0]), Convert.ToUInt16(split_magnitude[1])), "Learn_Magnitude()"))
            {
                //Handle Error
            }
            //Reset button visability to original states
            ControlBox = true;
            RhythmDone.Enabled = true;
            RhythmTest.Show();
            RhythmTestStop.Hide();
        }
        //Changes the PatternBox, called only upon changes in the ComboBox
        private void RhythmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear_Rhythm();
            Populate_Rhythm();
            Paint_Rythm();
        }
        //When a change occurs, we want to place markers around selected pattern
        private void RhythmPatternList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Paint_Rythm();
        }  

        //Primary paint function auto-called on startup, also screen changes. 
        private void RhythmPaint_Paint(object sender, PaintEventArgs e)
        {
            Paint_Rythm();
        }
        //Appends new value to the end, and repaints the graphics
        private void RhythmAdd_Click(object sender, EventArgs e)
        {
            Add_Pair(Convert.ToInt32(RhythmOn.Value), Convert.ToInt32(RhythmOff.Value));
            Paint_Rythm();
        }
        //Inserts pair before selected index, and repairs the graphics
        private void RhythmInsert_Click(object sender, EventArgs e)
        {
            Insert_Pair(RhythmPatternList.SelectedIndex, Convert.ToInt32(RhythmOn.Value), Convert.ToInt32(RhythmOff.Value));
            Paint_Rythm();
        }
        //Removes all rhythms, and paints the Cleared Rhythm
        private void RhythmClear_Click(object sender, EventArgs e)
        {
            Clear_Rhythm();
            Paint_Rythm();
        }

        private void RhythmReplace_Click(object sender, EventArgs e)
        {
            Replace_Pair(RhythmPatternList.SelectedIndex, Convert.ToInt32(RhythmOn.Value), Convert.ToInt32(RhythmOff.Value));
            Paint_Rythm();
        }

        private void RhythmDelete_Click(object sender, EventArgs e)
        {
            Delete_Pair(RhythmPatternList.SelectedIndex);
            Paint_Rythm();
        }
        //Forces user to enter a value that is divisible by 50
        private void RhythmOn_ValueChanged(object sender, EventArgs e)
        {
            if (RhythmOn.Value % 50 != 0)
                RhythmOn.Value = Convert.ToInt32(RhythmOn.Value) / 50 * 50;
        }
        //Forces user to enter a value that is divisible by 50
        private void RhythmOff_ValueChanged(object sender, EventArgs e)
        {
            if (RhythmOff.Value % 50 != 0)
                RhythmOff.Value = Convert.ToInt32(RhythmOff.Value) / 50 * 50;
        }
    }
}