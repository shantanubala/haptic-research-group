using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace HapticDriver
{
    public partial class HapticBelt
    {
        public string[] Query_Rhythm() {
            return Query("QRY RHY\r", 150);
        }

        //FIXME not sure we want to return all rhythms
        // should have get Rhythm that accepts Int for the rhythm requested.  Returns Structure object for 
        // an individual rhythm (use firmware header).  Should have C++ wrapper class that has several accessor functions
        // Do we want Rhy mutable? Easiest
        // Compile DLL into MATLAB???
        //
        //This function queries the belt using the private Query_All() function and returns a specific rhythm value
        public string[] getRhythm(bool binary) {
            string[] return_values = new string[RHY_MAX_NO + 1];
            return_values[0] = "NONE DEFINED";
            int rhyCount = 0;

            try { //Convert.ToInt16 can cause exception
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("RHY")) {
                            //Populate Return Values
                            return_values[rhyCount + 1] = split[2];

                            //return the rhythm pattern as hex (default) or as a binary string
                            if (!binary) {
                                return_values[rhyCount + 1] += "," + split[3];
                            }
                            else {
                                string binary_pattern = "";
                                binary_pattern = HexToBinary(split[3]);
                                if (String.Equals(binary_pattern, "Error")) {
                                    return_values[0] = "Invalid rhythm return, rhythm from query did not contain hex values";
                                }
                                return_values[rhyCount + 1] += "," + binary_pattern;
                            }

                            // Check RHY length
                            if (Convert.ToInt32(split[4]) == 0)
                                return_values[0] = "This rhythm is currently empty";
                            else {
                                return_values[rhyCount + 1] += "," + split[4];

                            }
                            rhyCount++; // count of defined rhythms
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return_values[0] = rhyCount.ToString(); // count of defined magnitudes

            return return_values; // returns Rhythm "<A>,<hex/binary pattern>,<length>"
        }

        /*
         * This function sends a learn rhythm command to the belt in 
         * the form LEARN RHYTHM <id> <pattern> <length> 
         */
        //Belt Command: 
        //    LEARN RHYTHM <id> <rhythm> <lenth>
        //Belt Returns:
        //    a response in the form
        //        STATUS <error num> [<info>]
        //
        //***string[] Learn_Rhythm(string id, string pattern_string)
        //        Returns:
        //            string array of length 2
        //                where 
        //                    string[0] = error response
        //                    string[1] = command sent
        //        Parameters: 
        //            string id = ID of rhythm to be learned ("A" through "H")
        //            string pattern = rhythm to be learned (64 bit binary string)
        public string[] Learn_Rhythm(string id, string pattern_string) {
            string[] return_values = new string[2];

            if (serialOut.IsOpen()) {
                //convert the string into an array of integers
                int[] pattern = new int[pattern_string.Length];
                int i = 0;
                foreach (char c in pattern_string) {
                    pattern[i] = Convert.ToInt32(c.ToString());
                    i++;
                }

                //verify that the rhythm ID is between A and H
                if (String.Compare(id, "H") > 0 || String.Compare(id, "A") < 0) {
                    //invalid rhythm ID
                    return_values[0] = "Invalid rhythm ID provided as argument to function";
                    return_values[1] = "";
                }
                //make sure the length of the binary pattern is 64 bits or less
                else if (pattern.Length > 64) {
                    //invalid pattern length
                    return_values[0] = "Invalid Pattern Length provided as argument to function";
                    return_values[1] = "";
                }
                //copy the contents of the pattern array into a 64 bit array for conversion
                else {
                    int[] internal_pattern = new int[64];
                    for (int pattern_index = 0; pattern_index < pattern.Length; pattern_index++) {
                        //error check for 0 or 1 in the pattern array and copy to 64 bit pattern
                        if (pattern[pattern_index] != 0 && pattern[pattern_index] != 1) {
                            //pattern not a list of ones and zeros
                            return_values[0] = "Pattern not a list of zeros and ones, invalid pattern provided as argument to function";
                            return_values[1] = "";
                            return return_values;
                        }
                        internal_pattern[pattern_index] = pattern[pattern_index];
                    }
                    //put zeros in the remaining contents of the array
                    for (int pattern_index2 = pattern.Length; pattern_index2 < 64; pattern_index2++) {
                        internal_pattern[pattern_index2] = 0;
                    }

                    //convert the array of values into a 16-character hex string
                    int temp_decimal_value = 0;
                    string hex_string = "";
                    for (int count = 0; count < 64; count = count + 4) {
                        for (int index = 0; index < 4; index++) {
                            temp_decimal_value = (temp_decimal_value * 2) + internal_pattern[index + count];
                        }
                        hex_string += temp_decimal_value.ToString("X");
                        temp_decimal_value = 0;
                    }

                    //at this point hex_string holds a string containing the 16-character hex code to be passed to belt

                    string instruction = String.Concat("LRN RHY ", id.ToString(), " ", hex_string, " ", pattern.Length.ToString());

                    //send this output to the belt
                    try {
                        if (acmd_mode != acmd_mode_t.ACM_LRN) {
                            acmd_mode = acmd_mode_t.ACM_LRN;

                            //return to learning mode, should be ASCII "00"
                            byte[] returnState = { 0x30, 0x30 };
                            serialOut.WriteData(returnState, 50);
                        }

                        serialOut.WriteData(instruction, 200);
                        //check for STATUS <error number> [<info>]
                        checkBeltStatus();
                        if (belt_error != 0) {
                            return_values[0] = "No response from belt or belt error";
                            return_values[1] = "";
                        }
                        else {
                            //if everything completes successfully
                            return_values[0] = "";
                            return_values[1] = "";
                        }
                    }
                    catch {
                        return_values[0] = "Error sending command over wireless";
                        return_values[1] = "";
                    }
                }
            }
            else {
                return_values[0] = "Serial port not open";
                return_values[1] = "";
            }
            return return_values;
        }


    }



}