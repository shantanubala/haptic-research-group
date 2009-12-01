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
                dispError("Rhythm length cannot exceed 3200ms"); 
            }
            else
            {
                rhythmItems[pairs] = on.ToString() + "," + off.ToString();
                total_duration += on + off;
                RhythmPatternList.Items.Add(rhythmItems[pairs]);
                pairs++;
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
                    dispError("Rhythm length cannot exceed 3200ms"); 
                }
                else
                {
                    //Shifts elements one space to the right of the index
                    for (int i = pairs; i > index ; i--)
                    {
                        rhythmItems[i] = rhythmItems[i-1];
                    }
                    rhythmItems[index] = on.ToString() + "," + off.ToString();
                    total_duration += on + off;
                    pairs++;
                    //Insert into ListBox RhythmPattern
                    RhythmPatternList.Items.Insert(index, rhythmItems[index]);
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
                    dispError("Rhythm length cannot exceed 3200ms");                                                                                                                                                                                                                                                            
                }
                else
                {
                    rhythmItems[index] = on.ToString() + "," + off.ToString();
                    RhythmPatternList.Items.RemoveAt(index);
                    RhythmPatternList.Items.Insert(index, rhythmItems[index]);
                    total_duration = total_duration - (Convert.ToInt32(splitPair[0]) + Convert.ToInt32(splitPair[1])) + on + off;
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
        private void Populate_Rhythm(String sel)
        {           
            String pattern = belt.getRhythmPattern(sel,true,QueryType.SINGLE);
            String currChar = "";
            String prevChar = "";

            if (hasError(belt.getStatus(), "getRhythmPattern()"))
            {
                //Handle Error
            }
            else
            {
                int duration = 0;    //Accumulates successive 1's or 0's as 50ms increments
                rhythmItems = new String[64];
                pairs = 0;       //Number of On,Off pairs
                //We set these parameters to make the loop logic more simple
                prevChar = pattern.Substring(0, 1);
                //Creates a string array of pairs of on,off durations, based on string pattern
                for (int i = 0; i < pattern.Length; i++)
                {
                    currChar = pattern.Substring(i, 1);
                    if (prevChar == currChar)
                    {
                        duration += 50;
                    }
                    else
                    {
                        if (currChar == "1")
                        {
                            rhythmItems[pairs] = rhythmItems[pairs] + duration.ToString();
                            pairs++;
                        }
                        else
                        {
                            rhythmItems[pairs] = duration.ToString() + ",";
                        }
                        total_duration += duration; //accumulate duration
                        duration = 50; //reset duration
                    }
                    prevChar = currChar; //Set the new previous character
                }
                //Adds last of the pairs after loop completes
                rhythmItems[pairs] = rhythmItems[pairs] + duration.ToString();
                total_duration += duration;
                pairs++;
                //Adds items to collection of RhythmPattern Box Display
                for (int i = 0; i < pairs; i++)
                {
                    RhythmPatternList.Items.Add(rhythmItems[i]);
                }
            }
        }
        /*Paints Rythm based on String[] rhythmItems's contents
         *RhythmPaint box dimensions (0-255,0-33) (256x34)
         *To graphically represent up to 64 increments of 50ms
         *every 4 horizontal pixels is 50ms.
         */
        private void Paint_Rythm()
        {
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

            for (int i = 0; i < pairs; i++)
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
    }
}
