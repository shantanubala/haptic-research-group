using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace HapticDriver
{
    public partial class HapticBelt
    {
        public int Query_Rhythm() {
            return Query("QRY RHY\r", 200);
        }

        // This function returns all rhythm values stored on belt, 
        // QRY ALL or QRY RHY must be used before this command to get current data
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

        // Function returns a specific rhythm pattern stored on the belt
        // QRY ALL or QRY RHY must be used before this command to get current data
        public string getRhythmPattern(string rhy_id, bool binary) {

            string return_values = "";
            //return_values[0] = "NOT DEFINED";

            try { //Convert.ToInt16 can cause exception
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("RHY") && split[2].Equals(rhy_id)) {

                            //Populate Return Values
                            //return the rhythm pattern as hex (default) or as a binary string
                            if (!binary) {
                                return_values = split[3];
                            }
                            else {
                                string binary_pattern = "";
                                binary_pattern = HexToBinary(split[3]);
                                if (String.Equals(binary_pattern, "Error")) {
                                    //return_values[0] = "Invalid rhythm return, rhythm from query did not contain hex values";
                                }
                                return_values = binary_pattern;
                            }
                            // Check RHY length
                            if (Convert.ToInt32(split[4]) == 0)
                                ;//return_values[0] = "This rhythm is currently empty";
                            else
                                ;
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return return_values; // returns Rhythm "<hex/binary pattern>"
        }

        // Function returns a specific rhythm time stored on the belt
        // which is the number of bits argument specifies how many of the 64 bits
        // specified by the pattern are actually used in the rhythm.
        // QRY ALL or QRY RHY must be used before this command to get current data
        public string getRhythmTime(string rhy_id) {

            string return_value = "";
            //return_values[0] = "NOT DEFINED";

            try { //Convert.ToInt16 can cause exception
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("RHY") && split[2].Equals(rhy_id)) {

                            //Populate Return Values
                            return_value = split[4];
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return return_value; // returns Rhythm "<hex/binary pattern>"
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
        public int Learn_Rhythm(string rhy_id, string pattern_str, int rhy_time, bool binary) {

            error_t return_error = error_t.EMAX;

            string hex_string = "";
            string binary_string = "";

            // "rhy_time" is the number of bits argument specifies how many of the 64 bits
            // specified by the pattern are actually used in the rhythm.

            //verify that the rhythm ID is between A and H
            if (String.Compare(rhy_id, "H") > 0 || String.Compare(rhy_id, "A") < 0) {
                //invalid rhythm ID
                //return_values[0] = "Invalid rhythm ID provided as argument to function";
                //return_values[1] = "";
            }
            //make sure the pattern uses hex characters only
            else if (binary == false && !verifyHexDigits(pattern_str.Trim())) {
                //pattern not a list of hex values
                //return_values[0] = "Pattern not a list of hex values, invalid pattern provided as argument to function";
                //return_values[1] = "";
            }
            //make sure the length of the hex pattern is 64 bits or less
            else if (binary == false && pattern_str.Trim().Length > 16) {
                //invalid pattern length
                //return_values[0] = "Invalid Pattern Length provided as argument to function";
                //return_values[1] = "";
            }
            else if (binary == true && !verifyBinaryDigits(pattern_str.Trim())) {
                //pattern not a list of ones and zeros
                //return_values[0] = "Pattern not a list of zeros and ones, invalid pattern provided as argument to function";
                //return_values[1] = "";
            }
            //make sure the length of the binary pattern is 64 bits or less
            else if (binary == true && pattern_str.Trim().Length > 64) {
                //invalid pattern length
                //return_values[0] = "Invalid Pattern Length provided as argument to function";
                //return_values[1] = "";
            }
            else if (rhy_time > 64 || rhy_time < 0) {
                //invalid pattern length
                //return_values[0] = "Invalid Pattern Length provided as argument to function";
                //return_values[1] = "";
            }
            else if (!serialOut.IsOpen()) {
                //return_error = status_msg.COMPRTNOTOPEN; //FIXME
            }
            else { // Process normal LRN RHY
                if (binary) {
                    binary_string = pattern_str.Trim();

                    //put zeros in the remaining contents of the array
                    for (int ix = binary_string.Length; ix < 64; ix++) {
                        binary_string += "0";
                    }
                    //convert the array of values into a 16-character hex string
                    hex_string = BinaryToHex(binary_string);
                }
                else {
                    hex_string = pattern_str.Trim();

                    //put zeros in the remaining contents of the array
                    for (int ix = hex_string.Length; ix < 16; ix++) {
                        hex_string += "0";
                    }
                }
                // hex_string holds a string containing the 16-character hex code to be passed to belt
                string instruction = "LRN RHY " + rhy_id + " "
                    + hex_string + " " + rhy_time + "\r"; 

                //send output to the belt
                try {
                    change_acmd_mode(acmd_mode_t.ACM_LRN);
                    if (acmd_mode == acmd_mode_t.ACM_LRN) {
                        // Send command with wait time for belt to respond back.
                        serialOut.WriteData(instruction, 200);
                    }

                    //check for STATUS <error number> [<info>]
                    checkBeltStatus();
                    if (belt_error != 0) {
                        return_error = belt_error;
                        //return_values[0] = "No response from belt or belt error";
                        //return_values[1] = "";
                    }
                    else {
                        return_error = error_t.ESUCCESS;
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                    return_error = error_t.EMAX;
                }
            }
            return (int)return_error;
        }
    }
}