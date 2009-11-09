using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

namespace HapticDriver
{
    public partial class HapticBelt
    {
        public int Query_Magnitude() {
            return Query("QRY MAG\r", 150);
        }

        // This function returns all magnitude values stored on belt, 
        // QRY ALL or QRY RHY must be used before this command to get current data
        public string[] getMagnitude(bool dutyCycleFormat) {
            double Period, DutyCycle;
            int Percentage;

            string[] return_values = new string[MAG_MAX_NO + 1];
            return_values[0] = "NONE DEFINED";
            int magCount = 0;

            try { //Convert.ToInt16 can cause exception

                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("MAG")) {
                            if (dutyCycleFormat == true) {
                                //Populate Return Values --> Equals "Mag letter,period,dutyCycle"
                                return_values[magCount + 1] = split[2] + "," + split[3] + "," + split[4];
                            }
                            else {
                                //Populate Return Values --> Equals "Mag letter, percent magnitude"
                                Period = Convert.ToInt32(split[3]);
                                DutyCycle = Convert.ToInt32(split[4]);
                                Percentage = (int)((DutyCycle / Period) * 100);
                                return_values[magCount + 1] = split[2] + "," + Percentage;
                            }
                            magCount++; // count of defined magnitudes
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("ERROR" + ex.Message);
            }
            return_values[0] = magCount.ToString(); // count of defined magnitudes
            return return_values;
        }

        // Function returns a specific rhythm pattern stored on the belt
        // QRY ALL or QRY RHY must be used before this command to get current data
        public string getMagnitude(string mag_id, bool dutyCycleFormat) {
            
            double Period, DutyCycle;
            int Percentage;
            string return_values = "";
            //return_values[0] = "NONE DEFINED";

            try { //Convert.ToInt16 can cause exception

                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("MAG") && split[2].Equals(mag_id)) {
                            if (dutyCycleFormat == true) {
                                //Populate Return Values --> Equals period,dutyCycle"
                                return_values = split[3] + "," + split[4];
                            }
                            else {
                                //Populate Return Values --> Equals "Mag letter, percent magnitude"
                                Period = Convert.ToInt32(split[3]);
                                DutyCycle = Convert.ToInt32(split[4]);
                                Percentage = (int)((DutyCycle / Period) * 100);
                                return_values =  "" + Percentage;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("ERROR" + ex.Message);
            }
            //return_values[0] = magCount.ToString(); // count of defined magnitudes
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
        public int Learn_Magnitude(string mag_id, UInt16 period, UInt16 duty_cycle) {

            error_t return_error = error_t.EMAX;

            if (String.Compare(mag_id, "A") < 0 || String.Compare(mag_id, "D") > 0) {
                //invalid magnitude ID
                //return_values[0] = "Invalid magnitude ID provided as argument to function";
                //return_values[1] = "";
            }
            // ensure minimum duty because PWM TOP cannot be too small
            // duty cycle in microseconds must be < period
            else if (period > Constants.PERIOD_MAX 
                || duty_cycle > period 
                || duty_cycle < Constants.DUTY_CYCLE_MIN) {
                return_error = error_t.EINVM;
            }
            else if (!serialOut.IsOpen()) {
                //return_error = status_msg.COMPRTNOTOPEN; //FIXME
            }
            else {
                string instruction = "LRN MAG " + mag_id + " " 
                    + period + " " + duty_cycle + "\r";

                //send this output to the belt
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

        // Wrapper to convert percentage to period and duty cycle
        public int Learn_Magnitude(string rhy_id, int percentage) {
            UInt16 period, duty_cycle, percent;

            // ensure that the percentage is a system minimum of 2%
            if (percentage < 1)
                percent = 2;
            else
                percent = (UInt16)percentage;

            // Use PERIOD_MAX for belt's resolution at 2% magnitude to calculate 
            // the duty cylce from the percent parameter.
            period = Constants.PERIOD_MAX;
            duty_cycle = (UInt16)((percent * period) / 100);

            return Learn_Magnitude(rhy_id, period, duty_cycle); //returns error code
        }

    }
}