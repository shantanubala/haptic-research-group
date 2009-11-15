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
        /// Function queries all haptic belt magnitude setting configurations
        /// </summary>
        /// <returns>error code resulting from Query command</returns>
        private error_t Query_Magnitude() {
            return Query("QRY MAG\r", 150);
        }

        /// <summary>
        /// This function returns all magnitude settings stored on belt from the last 
        /// QRY ALL or QRY MAG operation
        /// </summary>
        /// <param name="dutyCycleFormat">determines if the returned string is in 
        /// period/duty cycle format</param>
        /// <param name="query_type">specifies which type of query to execute</param>
        /// <returns> If dutyCycleFormat = true, then Magnitudes "[ID],[period],[dutyCycle]"
        /// If dutyCycleFormat = false, then Magnitudes "[ID],[percentage]"</returns>
        public string[] getMagnitude(bool dutyCycleFormat, QueryType query_type) {

            string[] return_values = new string[MAG_MAX_NO + 1];
            double Period, DutyCycle;
            int Percentage;
            int magCount = 0;
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            return_error = QuerySelect("QRY MAG\r", query_type);

            // Search and process data
            if (return_error == error_t.ESUCCESS) {
                return_error = error_t.NOTFOUND;
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("MAG")) {
                            return_error = error_t.ESUCCESS;

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
            return_values[0] = magCount.ToString(); // count of defined magnitudes 
            _belt_error = return_error;

            return return_values;
        }

        /// <summary>
        /// This function returns a specific magnitude setting stored on belt from the last 
        /// QRY ALL or QRY RHY operation.
        /// </summary>
        /// <param name="mag_id">magnitude ID is between A and D</param>
        /// <param name="dutyCycleFormat">determines if the returned string is in 
        /// period/duty cycle format</param>
        /// <param name="query_type">specifies which type of query to execute</param>
        /// <returns> If dutyCycleFormat = true, then Magnitudes "[period],[dutyCycle]"
        /// If dutyCycleFormat = false, then Magnitudes "[percentage]"</returns>
        public string getMagnitude(string mag_id, bool dutyCycleFormat, QueryType query_type) {

            double Period, DutyCycle;
            int Percentage;
            string return_values = "";
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            return_error = QuerySelect("QRY MAG\r", query_type);

            // Search and process data
            if (return_error == error_t.ESUCCESS) {
                return_error = error_t.NOTFOUND;
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("MAG") && split[2].Equals(mag_id)) {
                            return_error = error_t.ESUCCESS;

                            if (dutyCycleFormat == true) {
                                //Populate Return Values --> Equals period,dutyCycle"
                                return_values = split[3] + "," + split[4];
                            }
                            else {
                                //Populate Return Values --> Equals "Mag letter, percent magnitude"
                                Period = Convert.ToInt32(split[3]);
                                DutyCycle = Convert.ToInt32(split[4]);
                                Percentage = (int)((DutyCycle / Period) * 100);
                                return_values = "" + Percentage;
                            }
                        }
                    }
                }
            }
            _belt_error = return_error;
            return return_values;
        }

        /// <summary>
        /// This function is used to program a specific magnitude on the belt
        /// It sends a learn magnitude command to the belt in the form 
        /// LRN MAG [id] [period] [duty_cycle]
        /// </summary>
        /// <param name="mag_id">magnitude ID is between "A" and "D"</param>
        /// <param name="period">period of magnitude to be learned.  Maxmimum of 
        /// 2000 microseconds</param>
        /// <param name="duty_cycle">duty_cycle of magnitude to be learned.  
        /// Minimum of 2 microseconds</param>
        /// <returns>error code resulting from Learn Magnitude command</returns>
        public error_t Learn_Magnitude(string mag_id, UInt16 period, UInt16 duty_cycle) {

            error_t return_error = error_t.EINVM;

            if (String.Compare(mag_id, "A") < 0 || String.Compare(mag_id, "D") > 0) {
                //invalid magnitude ID
                return_error = error_t.INVMAGID;
            }
            // ensure minimum duty because PWM TOP cannot be too small
            // duty cycle in microseconds must be < period
            else if (period > Constants.PERIOD_MAX
                || duty_cycle > period
                || duty_cycle < Constants.DUTY_CYCLE_MIN) {
                return_error = error_t.EINVM;
            }
            else {
                string instruction = "LRN MAG " + mag_id + " "
                    + period + " " + duty_cycle + "\r";

                //send this output to the belt
                try {
                    change_acmd_mode(acmd_mode_t.ACM_LRN);
                    if (acmd_mode != acmd_mode_t.ACM_LRN) {
                        return_error = _belt_error;
                    }
                    else {
                        // Send command with wait time for belt to respond back.
                        return_error = SerialPortWriteData(instruction, MAX_RESPONSE_TIMEOUT);

                        if (return_error == error_t.ESUCCESS) {
                            // Query configuration data from belt to ensure settings
                            return_error = QuerySelect("QRY MAG\r", QueryType.SINGLE);
                        }
                    }
                }
                catch {
                    return_error = error_t.EXCLRNMAG;
                }
            }
            return return_error;
        }

        /// <summary>
        /// Overload function is used to program a specific magnitude on the belt
        /// by using a percent magnituded instead of a period and duty cycle
        /// </summary>
        /// <param name="mag_id">magnitude ID is between "A" and "D"</param>
        /// <param name="percentage"></param>
        /// <returns>error code resulting from Learn Magnitude command</returns>
        public error_t Learn_Magnitude(string mag_id, int percentage) {

            UInt16 period, duty_cycle, percent;
            error_t return_error = error_t.EINVM;

            // ensure that the percentage is a system minimum of 2%
            if (percentage < 1)
                percent = 2;
            else if (percentage > 100)
                return_error = error_t.INVMAGHIGH;
            else {
                percent = (UInt16)percentage;

                // Use PERIOD_MAX for belt's resolution at 2% magnitude to calculate 
                // the duty cylce from the percent parameter.
                period = Constants.PERIOD_MAX;
                duty_cycle = (UInt16)((percent * period) / 100);

                return_error = (error_t)Learn_Magnitude(mag_id, period, duty_cycle);
            }
            return return_error;
        }

    }
}