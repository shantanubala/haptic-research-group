using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace HapticDriver
{
    public partial class HapticBelt
    {
        public string[] Query_Magnitude() {
            return Query("QRY MAG\r", 150);
        }

        //This function queries the belt using the private Query_All() function and returns a specific magnitude value
        public string[] getMagnitude() {
            string[] return_values = new string[MAG_MAX_NO + 1];
            return_values[0] = "NONE DEFINED";
            int magCount = 0;

            try { //Convert.ToInt16 can cause exception
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("MAG")) {
                            //Populate Return Values --> Equals "Rhy,period,dutyCycle"
                            return_values[magCount + 1] = split[2] + "," + split[3] + "," + split[4];
                            magCount++; // count of defined magnitudes
                        }
                    }
                }
            } //[System.NullReferenceException] = {"Object reference not set to an instance of an object."}
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return_values[0] = magCount.ToString(); // count of defined magnitudes
            return return_values;
        }

        /* 
         * This function sends a learn magnitude command to the belt in the form 
         * LEARN MAGNITUDE <period> <duty cycle> 
         */
        //Belt Command: 
        //    LEARN MAGNITUDE <period> <duty cycle>
        //Belt Returns:
        //    a response in the form
        //        STATUS <error num> [<info>]
        //
        //*** string[] Learn_Magnitude(string period, string duty_cycle)
        //        Returns:
        //            string array of length 2
        //                where 
        //                    string[0] = error response
        //                    string[1] = command sent
        //        Parameters: 
        //            string period = period of magnitude to be learned
        //            string duty_cycle = duty_cycle of magnitude to be learned
        public string[] Learn_Magnitude(string id, string period, string duty_cycle) {
            string[] return_values = new string[2];

            if (serialOut.IsOpen()) {
                if (String.Compare(id, "A") < 0 || String.Compare(id, "D") > 0) {
                    //invalid magnitude ID
                    return_values[0] = "Invalid magnitude ID provided as argument to function";
                    return_values[1] = "";
                }
                else {
                    string instruction = String.Concat("LRN MAG ", id, " ", period, " ", duty_cycle);

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