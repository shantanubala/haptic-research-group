using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//namespaces are a lot like a definition of scope
//for easy importing
namespace GloveFeedbackInterface
{
    //partial classes can receive definition from two
    //different files - in this case, Form1.Designer.cs acts as 
    //the additional set of definitions
    //this also defines HandForm as an extension of System.Windows.Forms.Form
    public partial class HandForm : Form
    {
        //this holds whether the buttons are checked or unchecked
        //for starting a particular motor
        //each motor (as shown in the button labels) has an index between 0 and 13
        //if more motors are added, make sure the size of this array is increased
        private bool[] checkedMotors = new bool[14];

        public HandForm()
        {
            InitializeComponent();
            //this loop defaults every value of the checkedMotors array to false
            //if this is changed, make sure to change the default colors of the
            //buttons before they are clicked
            //see the event handlers for more info
            for (int i = 0; i < checkedMotors.Length; i++)
            {
                checkedMotors[i] = false;
            }
            this.HapticGlove.Open();

        }

        //the following 14 functions provide click event handlers
        //for each of the 14 buttons

        //on click, it changes the appropriate value in the checkedMotors
        //array to the opposite of its current value

        //the functions also change the colors of the buttons
        //if the motor is checked, the button color is green
        //if not, the button color is red
        //note that the button color is red by default,
        //and all checkedMotors values are false by default

        //if more motors are added, create an event handler for each
        
        /* EVENT HANDLERS */
        private void button1_Click(object sender, EventArgs e)
        {
            checkedMotors[0] = !checkedMotors[0];
            if (checkedMotors[0])
            {
                this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkedMotors[1] = !checkedMotors[1];
            if (checkedMotors[1])
            {
                this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkedMotors[2] = !checkedMotors[2];
            if (checkedMotors[2])
            {
                this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkedMotors[3] = !checkedMotors[3];
            if (checkedMotors[3])
            {
                this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            checkedMotors[4] = !checkedMotors[4];
            if (checkedMotors[4])
            {
                this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            checkedMotors[5] = !checkedMotors[5];
            if (checkedMotors[5])
            {
                this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            checkedMotors[6] = !checkedMotors[6];
            if (checkedMotors[6])
            {
                this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            checkedMotors[7] = !checkedMotors[7];
            if (checkedMotors[7])
            {
                this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            checkedMotors[8] = !checkedMotors[8];
            if (checkedMotors[8])
            {
                this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            checkedMotors[9] = !checkedMotors[9];
            if (checkedMotors[9])
            {
                this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            checkedMotors[10] = !checkedMotors[10];
            if (checkedMotors[10])
            {
                this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            checkedMotors[11] = !checkedMotors[11];
            if (checkedMotors[11])
            {
                this.button14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            checkedMotors[12] = !checkedMotors[12];
            if (checkedMotors[12])
            {
                this.button13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            checkedMotors[13] = !checkedMotors[13];
            if (checkedMotors[13])
            {
                this.button12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(138)))));
            }
            else
            {
                this.button12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(153)))), ((int)(((byte)(137)))));
            }
        }
        /* END OF BUTTON EVENT HANDLERS */

        private void buzz_Click(object sender, EventArgs e)
        {
            //converts the options to numbers
            int repeat = Convert.ToInt32(this.repeatBox.Text);
            int duration = Convert.ToInt32(this.durationBox.Text);
            this.start_buzz(repeat, duration);
        }

        /* END OF ALL EVENT HANDLERS */

        //executed when "Buzz" is clicked
        private void start_buzz(int repeat, int duration)
        {
            //this loops through our checkedMotors array to see which motors
            //should be started by the program

            bool firstLoop = true;
            string status = "Just Buzzed: ";
            string motor_switch = "~";
            for (int i = 0; i < checkedMotors.Length; i++)
            {
                if (checkedMotors[i])
                {
                    if (firstLoop)
                    {
                        status += (char)(65 + i);
                        if (motor_switch.Length < 4)
                        {
                            motor_switch += (char)(65 + i);
                        }
                        firstLoop = false;
                    }
                    else
                    {
                        status +=  ", " + (char)(65 + i);
                        if (motor_switch.Length < 4)
                        {
                            motor_switch += (char)(65 + i);
                        }
                    }
                }
            }
            this.HapticGlove.Write(motor_switch);
            this.statusLabel.Text = status + " for " + duration + " second(s) " + repeat + " time(s).";
        }
    }
}
