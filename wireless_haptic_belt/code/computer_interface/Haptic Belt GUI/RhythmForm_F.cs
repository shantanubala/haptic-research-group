using System;
using System.Drawing;
using HapticDriver;

namespace HapticGUI
{
    partial class RhythmForm
    {
        Graphics RhythmGraphics;
        String[] rhythmItems = new String[64];
        int pairs = 0; //Pairs of On,Off durations
        int total_duration = 0; // >= 3200, equal to sum of pair durations
        
        /*This method adds a pair to the end of the ListBox
         *and its representative at the end of rhythmItems.
         */
        private void Add_Pair(int on, int off)
        {
            //Check for non multiples of 50ms, or on = off = 0
            if (off == 0 && on == 0)
            {
                //Do Nothing
            }
            //Rhythm Time cannot exceed 3200ms
            else if (on + off + total_duration > 3200)
            {
                ErrorForm errorForm = new ErrorForm("Rhythm length cannot exceed 3200ms", "Add_Pair()", false);
                errorForm.ShowDialog(); 
            }
            else
            {
                if (pairs != 0)
                {
                    String[] breakUp = rhythmItems[pairs - 1].Split(',');
                    int last_on = Convert.ToInt32(breakUp[0]);
                    int last_off = Convert.ToInt32(breakUp[1]);

                    if ((last_on == 0) && (on == 0))
                    {
                        off += last_off;
                        rhythmItems[pairs - 1] = "0," + off.ToString();
                        total_duration += (off - last_off);
                        RhythmPatternList.Items.RemoveAt(pairs - 1);
                        RhythmPatternList.Items.Add(rhythmItems[pairs - 1]);
                        RhythmPatternList.SelectedIndex = pairs - 1;
                    }
                    else if (last_off == 0)
                    {
                        on += last_on;
                        rhythmItems[pairs - 1] = on.ToString() + "," + off.ToString();
                        total_duration += (on - last_on) + off;
                        RhythmPatternList.Items.RemoveAt(pairs - 1);
                        RhythmPatternList.Items.Add(rhythmItems[pairs - 1]);
                        RhythmPatternList.SelectedIndex = pairs - 1;
                    }
                    else
                    {
                        rhythmItems[pairs] = on.ToString() + "," + off.ToString();
                        total_duration += on + off;
                        RhythmPatternList.Items.Add(rhythmItems[pairs]);
                        pairs++;
                    }
                }
                else
                {
                    rhythmItems[pairs] = on.ToString() + "," + off.ToString();
                    total_duration += on + off;
                    RhythmPatternList.Items.Add(rhythmItems[pairs]);
                    pairs++;
                } 
            }
        }
        /*This method inserts before the selected pair in the ListBox
         *as well as before its representative pair in rhythmItems.
         */
        private void Insert_Pair(int index, int on, int off)
        {
            if(index > -1)
            {
                //Check for non multiples of 50ms, or on = off = 0
                if (off == 0 && on == 0)
                {
                    //Do Nothing
                }
                //Rhythm Time cannot exceed 3200ms
                else if ((on + off + total_duration) > 3200)
                {
                    ErrorForm errorForm = new ErrorForm("Rhythm length cannot exceed 3200ms", "Insert_Pair()", false);
                    errorForm.ShowDialog(); 
                }
                else
                {
                    String[] breakUp = rhythmItems[index].Split(',');
                    int last_on = Convert.ToInt32(breakUp[0]);
                    int last_off = Convert.ToInt32(breakUp[1]);                        

                    if ((last_on == 0) && (on == 0))
                    {
                        off += last_off;
                        rhythmItems[index] = "0," + off.ToString();
                        total_duration += (off - last_off);
                        RhythmPatternList.Items.RemoveAt(index);
                        RhythmPatternList.Items.Insert(index, rhythmItems[index]);
                        RhythmPatternList.SelectedIndex = index;
                    }
                    else if (last_off == 0)
                    {
                        on += last_on;
                        rhythmItems[index] = on.ToString() + "," + off.ToString();
                        total_duration += (on - last_on) + off;
                        RhythmPatternList.Items.RemoveAt(index);
                        RhythmPatternList.Items.Insert(index, rhythmItems[index]);
                        RhythmPatternList.SelectedIndex = index;
                    }
                    else
                    {
                        //Shifts elements one space to the right of the index
                        for (int i = pairs; i > index; i--)
                        {
                            rhythmItems[i] = rhythmItems[i - 1];
                        }

                        rhythmItems[index] = on.ToString() + "," + off.ToString();
                        total_duration += on + off;
                        RhythmPatternList.Items.Insert(index, rhythmItems[index]);
                        pairs++;
                    }
                }
            }
        }
        /*This method replaces the selected pair in the ListBox
         *as well as its representative pair in rhythmItems.
         */
        private void Replace_Pair(int index, int on, int off)
        {
            if (index > -1)
            {
                String[] splitPair = new String[2];
                splitPair = rhythmItems[index].Split(',');
                //Check for on = off = 0
                if (off == 0 && on == 0)
                {
                    //Do nothing
                }
                //Rhythm Time cannot exceed 3200ms, compares difference in values against limit
                else if ((on + off + total_duration - Convert.ToInt32(splitPair[0]) - Convert.ToInt32(splitPair[1])) > 3200)
                {
                    ErrorForm errorForm = new ErrorForm("Rhythm length cannot exceed 3200ms", "Replace_Pair()", false);
                    errorForm.ShowDialog();                                                                                                                                                                                                                                                            
                }
                else
                {
                    Delete_Pair(index);
                    
                    if (pairs > index)
                        Insert_Pair(index, on, off);
                    else
                        Add_Pair(on, off);

                    RhythmPatternList.SelectedIndex = index;
                }
            }
        }
        /*This method deletes the selected pair in the ListBox
         *as well as its representative pair in rhythmItems.
         */
        private void Delete_Pair(int index)
        {
            if (index > -1)
            {
                String[] splitPair = new String[2];
                RhythmPatternList.Items.RemoveAt(index);
                splitPair = rhythmItems[index].Split(',');
                total_duration = total_duration - (Convert.ToInt32(splitPair[0]) + Convert.ToInt32(splitPair[1]));
                //We are shifting left into the index, the last element remains,
                //but is unaccessible due to the decrement of pairs.
                pairs--;
                for (int i = index; i < pairs; i++)
                {
                    rhythmItems[i] = rhythmItems[i + 1];
                }
                
            }
        }
        /* Clears the ListBox in Rhythm Panel as well as
         * sets the String[] rhythmItem's length (aka pairs)
         * to 0. As well as the total duration counter, which
         * accumulates the total time added into rhythmItems.
         */
        private void Clear_Rhythm()
        {
            //Clear PatternBox and associated parameters
            RhythmPatternList.Items.Clear();
            pairs = 0;
            total_duration = 0;
        }
        /*Queries the Library belt to return a pattern of 
         *Length 1-64 of multiple characters "1" or "0".
         *Based upon this pattern, the function will populate
         *the String[] rhythmItems, a different format that
         *represents On,Off durations in pairs.
         */
        private void Populate_Rhythm()
        {
            int i = 0;
            String pattern = rhythm[RhythmComboBox.SelectedIndex].pattern;
            int length = rhythm[RhythmComboBox.SelectedIndex].time;
            int on_duration = 0;
            int off_duration = 0;

            rhythmItems = new String[64]; //Clear old rhythmItems
            pairs = 0;       //Number of On,Off pairs
            //Set Total Duration Time
            total_duration = length * 50;

            //Creates a string array of pairs of on,off durations, based on the String pattern
            while (i < length)
            {
                on_duration = 0;
                off_duration = 0;

                while(i < length)
                {
                    //This if statement is not appart of the above while loop to avoid an indexing out of bounds error
                    if(pattern.Substring(i, 1).Equals("1"))
                    {
                        on_duration += 50;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                while (i < length)
                {
                    //This if statement is not appart of the above while loop to avoid an indexing out of bounds error
                    if (pattern.Substring(i, 1).Equals("0"))
                    {
                        off_duration += 50;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                rhythmItems[pairs] = on_duration.ToString() + "," + off_duration.ToString();
                RhythmPatternList.Items.Add(rhythmItems[pairs]);
                pairs++;
            }
        }
        /*Paints Rythm based on String[] rhythmItems's contents
         *RhythmPaint box dimensions (0-255,0-33) (256x34)
         *To graphically represent up to 64 increments of 50ms
         *every 4 horizontal pixels is 50ms.
         */
        private void Paint_Rythm()
        {
            int i;
            //Create array for breaking up the pairs (on,off)
            String[] splitPair = new String[2];
            int right_x = 0;
            int left_x = 0;
            //set to uninitialized values (must be < 0)
            int on = -1; 
            int off = -1; 
            Pen pen = new Pen(Color.Black);

            RhythmGraphics = RhythmPaint.CreateGraphics();
            //Start painting with a clean work area
            RhythmGraphics.Clear(BackColor);
            
            for (i = 0; i < pairs; i++)
            {
                //splits into array on and off times
                splitPair = rhythmItems[i].Split(',');
                //Get on part
                on = Convert.ToInt32(splitPair[0]);
                //converts on period to pixels horizontal
                right_x += (on / 50) * 4;
                //paint off to on edge
                if (i != 0)
                {
                    //Special case where a line should be drawn (off to on edge)
                    if ((off > 0 && on > 0) || (on == 0 && off == 0))
                        RhythmGraphics.DrawLine(pen, new Point(left_x, 36), new Point(left_x, 4));
                    //paint left brace if selected (covers off to on edge)
                    if (i == RhythmPatternList.SelectedIndex)
                    {
                        pen = new Pen(Color.Red);
                        RhythmGraphics.DrawLine(pen, new Point(left_x, 40), new Point(left_x, 0));
                    }
                    else if (i == RhythmPatternList.SelectedIndex + 1)
                    {
                        pen = new Pen(Color.Red);
                        RhythmGraphics.DrawLine(pen, new Point(left_x, 40), new Point(left_x, 0));
                    }
                    pen = new Pen(Color.Black);
                }
                else
                {
                    pen = new Pen(Color.Blue);
                    RhythmGraphics.DrawLine(pen, new Point(0, 40), new Point(0, 0));
                    RhythmGraphics.DrawLine(pen, new Point(256, 40), new Point(256, 0));
                    
                    if (i == RhythmPatternList.SelectedIndex)
                    {
                        pen = new Pen(Color.Red);
                        RhythmGraphics.DrawLine(pen, new Point(left_x, 40), new Point(left_x, 0));
                    }

                    pen = new Pen(Color.Black);
                }
                //Get off value (must be after off to on edge painting)
                off = Convert.ToInt32(splitPair[1]);
                //paint on part
                RhythmGraphics.DrawLine(pen, new Point(left_x, 4), new Point(right_x, 4));
                //paint on to off edge
                if ( on > 0 && off > 0)
                    RhythmGraphics.DrawLine(pen, new Point(right_x, 4), new Point(right_x, 36));
                //done painting on cycle, now paint off cycle (Swap vars)
                left_x = right_x;
                //converts off period to pixels
                right_x += (off / 50) * 4;
                //paint off part
                RhythmGraphics.DrawLine(pen, new Point(left_x, 36), new Point(right_x, 36));
                //done painting off cycle, swap vars
                left_x = right_x;
            }
            if (i == RhythmPatternList.SelectedIndex + 1)
            {
                pen = new Pen(Color.Red);
                RhythmGraphics.DrawLine(pen, new Point(left_x, 40), new Point(left_x, 0));
            }
            //set rhythm time
            RhythmTime.Text = total_duration.ToString() + "ms";
        }
        /*Takes all rhythmItems[], specified by variable 'pairs' (its length)
         *and convert them into a string pattern which consists of multiple
         *"1"'s or "0"'s.
         * 
         * Return String - Pattern (consecutive 1's and 0's), Length (1-64) of pattern. Output: Pattern,Length
         */
        private String Get_Pattern()
        {
            int on = 0;
            int off = 0;
            int length = 0;
            String[] splitPair = new String[2];
            String pattern = "";
            /*Uninitalized values*/
            
            for (int i = 0; i < pairs; i++)
            {
                splitPair = rhythmItems[i].Split(',');
                on = Convert.ToInt32(splitPair[0]);
                off = Convert.ToInt32(splitPair[1]);
                int j = 0;
                //Appends multiple 1's dependent on the value of ON/50
                for (j = 0; j < (on / 50); j++)
                {
                    pattern += "1";
                    length++;
                }
                //Appends multiple 0's dependent on the value of ON/50
                for (j = 0; j < (off / 50); j++)
                {
                    pattern += "0";
                    length++;
                }
            }
            pattern += "," + length.ToString();
            return pattern;
        }

        private void Test_Rhythm()
        {
            //Hide Rhythm Buttons so no interference will occur
            RhythmTest.Hide();
            ControlBox = false;
            RhythmDone.Enabled = false;

            //Get the User Inputed Pattern
            String[] pattern = Get_Pattern().Split(',');

            //Learn the test Rhythm to temp spot "H"
            if (hasError(belt.Learn_Rhythm("H", pattern[0], Convert.ToInt16(pattern[1]), true), "Learn_Rhythm()"))
            {
                //Handle Error
            }

            //Store the current Magnitude
            hold_magnitude = belt.getMagnitude("A", true, QueryType.SINGLE);
            if (hasError(belt.getStatus(), "getMagnitude()"))
            {
                //Handle Error
            }
            //Learn a 100% Magnitude setting
            if (hasError(belt.Learn_Magnitude("A", 100), "Learn_Magnitude()"))
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
            for (int i = 0; i < _motorcount; i++)
            {
                if (hasError(belt.Vibrate_Motor(i, "H", "A", 7), "Vibrate_Motor()"))
                {
                    //Handle Error
                }
            }
            //Wait for motors to finish vibrating or user to click "Stop" on RhythmTestStop Button
            RhythmTestStop.Show();
        }

        private void Stop_Rhythm_Test()
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
    }
}
