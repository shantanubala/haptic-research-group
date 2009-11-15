using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace HapticDriver
{
    public partial class HapticBelt
    {
        /// <summary>
        /// Function queries all haptic belt rhythm pattern configurations
        /// </summary>
        /// <returns>error code resulting from Query command</returns>
        private error_t Query_Rhythm() {
            return Query("QRY RHY\r", 200);
        }

        /// <summary>
        /// This function returns all rhythm values stored on belt from the last 
        /// QRY ALL or QRY RHY operation
        /// </summary>
        /// <param name="binary">determines if the returned string is in binary format</param>
        /// <param name="query_type">specifies which type of query to execute</param>
        /// <returns> Rhythm "[ID],[hex/binary pattern],[length]"</returns>
        public string[] getRhythm(bool binary, QueryType query_type) {
            string[] return_values = new string[RHY_MAX_NO + 1];
            int rhyCount = 0;
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            return_error = QuerySelect("QRY RHY\r", query_type);

            // Search and process data
            if (return_error == error_t.ESUCCESS) {
                return_error = error_t.NOTFOUND;
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("RHY")) {
                            //Populate Return Values
                            return_values[rhyCount + 1] = split[2];
                            return_error = error_t.ESUCCESS;

                            //return the rhythm pattern as hex (default) or as a binary string
                            if (!binary) {
                                return_values[rhyCount + 1] += "," + split[3];
                            }
                            else {
                                string binary_pattern = "";
                                binary_pattern = HexToBinary(split[3]);
                                if (String.Equals(binary_pattern, "Error")) {
                                    return_values[0] = "Invalid rhythm query - does not contain hex values";
                                }
                                return_values[rhyCount + 1] += "," + binary_pattern;
                            }
                            // Check RHY length
                            if (Convert.ToInt32(split[4]) == 0)
                                return_values[0] = "rhythm is currently empty";
                            else {
                                return_values[rhyCount + 1] += "," + split[4];
                            }
                            rhyCount++; // count of defined rhythms
                        }
                    }
                }
            }
            return_values[0] = rhyCount.ToString(); // count of defined magnitudes
            _belt_error = return_error;

            return return_values;
        }


        /// <summary>
        /// This function returns a specific rhythm pattern stored on belt from the last 
        /// QRY ALL or QRY RHY operation.
        /// </summary>
        /// <param name="rhy_id">rhythm ID is between "A" and "H"</param>
        /// <param name="binary">determines if the returned string is in binary format</param>
        /// <param name="query_type">specifies which type of query to execute</param>
        /// <returns> Rhythm "[hex/binary pattern]" or blank if not defined</returns>
        public string getRhythmPattern(string rhy_id, bool binary, QueryType query_type) {

            string return_values = "";
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            return_error = QuerySelect("QRY RHY\r", query_type);

            // Search and process data
            if (return_error == error_t.ESUCCESS) {
                return_error = error_t.NOTFOUND;
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("RHY") && split[2].Equals(rhy_id)) {
                            return_error = error_t.ESUCCESS;

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
                        }
                    }
                }
            }
            _belt_error = return_error;
            return return_values; // returns Rhythm "<hex/binary pattern>"
        }

        /// <summary>
        /// Function returns a specific rhythm time stored on the belt from the last 
        /// QRY ALL or QRY RHY operation. This time is the number of bits argument 
        /// specifies how many of the 64 bits specified by the pattern are actually 
        /// used in the rhythm.
        /// </summary>
        /// <param name="rhy_id">rhythm ID is between "A" and "H"</param>
        /// <param name="query_type">specifies which type of query to execute</param>
        /// <returns>rhythm time of rhythm in increments of 50 milliseconds</returns>
        public string getRhythmTime(string rhy_id, QueryType query_type) {

            string return_value = "";
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            return_error = QuerySelect("QRY RHY\r", query_type);

            // Search and process data
            if (return_error == error_t.ESUCCESS) {
                return_error = error_t.NOTFOUND;
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("RHY") && split[2].Equals(rhy_id)) {
                            return_error = error_t.ESUCCESS;

                            //Populate Return Values
                            return_value = split[4];
                        }
                    }
                }
            }
            _belt_error = return_error;
            return return_value; // returns Rhythm "time"
        }

        /// <summary>
        /// This function is used to program a specific rhythm pattern on the belt
        /// It sends a learn rhythm command to the belt in the form 
        /// LRN RHY [id] [pattern] [length] 
        /// </summary>
        /// <param name="rhy_id">rhythm ID is between "A" and "H"</param>
        /// <param name="pattern_str">rhythm to be learned (64 bit binary/hex string).  Each
        /// set bit is a 50 millisecond time slot where the vibrate motor will be activated</param>
        /// <param name="rhy_time">The number of bits argument specifies how many 
        /// of the 64 bits specified by the pattern are actually used in the rhythm.</param>
        /// <param name="binary">Set TRUE if the rhythm pattern string is in binary format</param>
        /// <returns>error code resulting from Learn Rhythm command</returns>
        public error_t Learn_Rhythm(string rhy_id, string pattern_str, int rhy_time, bool binary) {

            error_t return_error = error_t.EINVR;

            string hex_string = "";
            string binary_string = "";

            //verify that the rhythm ID is between A and H
            if (String.Compare(rhy_id, "H") > 0 || String.Compare(rhy_id, "A") < 0) {
                //invalid rhythm ID
                return_error = error_t.INVRHYID;
            }
            //make sure the pattern uses hex characters only
            else if (binary == false && !verifyHexDigits(pattern_str.Trim())) {
                //pattern not a list of hex values
                return_error = error_t.INVRHYPATHEX;
            }
            //make sure the length of the hex pattern is 64 bits or less
            else if (binary == false && pattern_str.Trim().Length > 16) {
                //invalid pattern length
                return_error = error_t.INVRHYPATLEN;
                //return_values[0] = "Invalid Pattern Length provided as argument to function";
                //return_values[1] = "";
            }
            else if (binary == true && !verifyBinaryDigits(pattern_str.Trim())) {
                //pattern not a list of ones and zeros
                return_error = error_t.INVRHYPATBIN;
            }
            //make sure the length of the binary pattern is 64 bits or less
            else if (binary == true && pattern_str.Trim().Length > 64) {
                //invalid pattern length
                return_error = error_t.INVRHYPATLEN;
            }
            else if (rhy_time > 64 || rhy_time < 0) {
                //invalid pattern length
                return_error = error_t.INVRHYTIME;
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
                    if (acmd_mode != acmd_mode_t.ACM_LRN) {
                        return_error = _belt_error;
                    }
                    else {
                        // Send command with wait time for belt to respond back.
                        return_error = SerialPortWriteData(instruction, MAX_RESPONSE_TIMEOUT);

                        if (return_error == error_t.ESUCCESS){
                            // Query configuration data from belt to ensure settings
                            return_error = QuerySelect("QRY RHY\r", QueryType.SINGLE);
                        }
                    }
                }
                catch {
                    return_error = error_t.EXCLRNRHY;
                }
            }
            return return_error;
        }
    }
}