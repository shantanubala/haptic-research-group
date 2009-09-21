//Haptic Belt Accessor Functions for Sending belt commands from C# GUI interface
//Author: Daniel Moberly
//Date: April 20th, 2009

// *
// * MODIFIED to work on Windows Mobile 5.0 or Microsoft CF 2.5
// * July 17, 2009
// * Nathan J. Edwards (nathan.edwards@asu.edu)
// * 
// * 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;

// ERROR CODES
//invalid or no belt response to query = -1, this is returned in the starting point of the array at [belt location,0]
//invalid rhythm ID = -3
//invalid pattern length = -4;
//pattern is something other than binary string = -5

namespace HapticDriver
{
    public class HapticBelt //: SerialPortManager // Derived  "extends" from SerialPortMananger
    {
        //Responses:
        //VER x 1, MAG x 4, RHY x 8, MTR x 1 = 14
        private static int TOTAL_RESPONSE_COUNT = 14;
        public static int MTR_MAX_NO = 16;
        public static int MAG_MAX_NO = 4;
        public static int RHY_MAX_NO = 5;

        // port variables used for SerialPort communications
        private SerialPortManager serialIn;
        private SerialPortManager serialOut;

        public enum PortType { IN, OUT };

        private bool port_setup = false;
        private string _portInName = "";
        private string _portOutName = "";

        // parent class's data recieved function pointer
        public delegate void DataRecievedHandler();
        public DataRecievedHandler DataReceivedFxn; //event???

        //this is the array of results which will be returned to the calling function
        private string[] qry_resp;

        // enum for current firmware mode
        private acmd_mode_t acmd_mode = acmd_mode_t.ACM_LRN;
        private error_t belt_error = error_t.EMAX;
        private mode_t glbl_mode = mode_t.M_LEARN;


        #region Manager Constructors
        /// <summary>
        /// Constructor to set the properties of our Driver Class
        /// </summary>
        public HapticBelt() {
            // PLACEHOLDERS FOR FUTURE
            //serialIn = new SerialPortManager(); 
            //serialOut = new SerialPortManager();

            //// Function pointer
            //if (DataReceivedFxn != null) // Always seems to be null
            //{
            //    serialIn.DataReceivedFxn += new SerialPortManager.DataRecievedHandler(DataReceivedFxn);
            //}

            qry_resp = new string[TOTAL_RESPONSE_COUNT + 1];
        }
        #endregion

        //*** string[] Initialize_Serial_Port(string portno, string baud_string, string parity_string, string stopbits_string, string databits_string)
        //        Returns: 
        //            string array of length 2
        //                where 
        //                    string[0] = error response
        //                    string[1] = NULL
        //        Parameters: 
        //            string iportno = Port number of the COM port the Serial_Port is connected to, i.e. "9" for COM9
        //            string baud_string = baud rate of connection
        //            string parity_string = parity bits to be used (None, Odd, Even, Mark, Space)
        //            string stopbits_string = stop bits to be used (0, 1, 1.5, 2)
        //            string databits_string =  number of data bits to be sent at a time     
        public bool SetupPorts(string portInName, string portOutName, string baud, string dBits, string sBits, string par, string timeout) {

            _portInName = portInName;
            _portOutName = portOutName;

            try {
                // Set default Port Manager parameters
                serialIn = new SerialPortManager(portInName, baud, dBits, sBits, par, timeout);
                serialIn.EchoBack = true;

                if (portInName != portOutName) {
                    serialOut = new SerialPortManager(portOutName, baud, dBits, sBits, par, timeout);
                }
                else    serialOut = serialIn;

                //// this.DataRecievedFxn is already Method = {Void MethodName()} from parent class
                //if (DataReceivedFxn != null) {
                //    // serialIn.DataRecievedFxn becomes Method = {Void Invoke()}
                //    serialIn.DataReceivedFxn += new SerialPortManager.DataRecievedHandler(DataReceivedFxn);
                //}
                port_setup = true;
            }
            catch {

            }
            return port_setup;
        }

        public void OpenPorts() {
            if (port_setup) {
                // this.DataRecievedFxn is already Method = {Void MethodName()} from parent class
                if (DataReceivedFxn != null) {
                    // serialIn.DataRecievedFxn becomes Method = {Void Invoke()}
                    serialIn.DataReceivedFxn += new SerialPortManager.DataRecievedHandler(DataReceivedFxn);
                }
                serialIn.OpenPort();
                if (_portInName != _portOutName)
                    serialOut.OpenPort();
            }
        }

        public void ClosePorts() {
            if (serialIn != null && serialOut != null) {
                serialIn.ClosePort();
                serialOut.ClosePort();
            }
        }

        public void WriteData(string dataString) {
            if (port_setup && serialOut.IsOpen())
                serialOut.WriteData(dataString);
        }

        public void WriteData(byte[] data) {
            if (port_setup && serialOut.IsOpen())
                serialOut.WriteData(data);
        }

        public byte getDataRecvType() {
            return serialIn.DataRecvBufferType();
        }

        public string getDataRecvBuffer() {
            return ByteToAscii(serialIn.DataRecvBuffer());
        }

        public byte getStatusType() {
            return serialOut.StatusBufferType();
        }
        public string getStatusBuffer() {
            return ByteToAscii(serialOut.StatusBuffer());
        }

        public string getStatusBufferStr() {
            string byteString = "";
            byte[] status = serialOut.StatusBuffer();
            for (int i = 0; i < status.Length; i++) {
                byteString += Constants.status_msg_names[status[i]] + "::";
            }
            return byteString;
        }

        /* Temporary overload to handle the prototyped method that uses strings */
        public string[] Vibrate_Motor(string motor_number, string rhythm_string, string magnitude_string, int rhythm_cycles) {

            byte motor = MotorStrToByte(motor_number);
            byte rhythm = VibStrToByte(rhythm_string);
            byte magnitude = VibStrToByte(magnitude_string);
            byte rhythm_length = (byte)(rhythm_cycles);

            String[] response = Vibrate_Motor(motor, rhythm, magnitude, rhythm_length);
            return response;
        }

        /*
         * This function is used to vibrate motors - send a bit string command FIXME 
         */
        //    string[] Vibrate_Motor(string motor_number, string rhythm, string magnitude, string rhythm_cycles)
        //        Returns: 
        //            string array of length 2
        //                where 
        //                    string[0] = error response
        //                    string[1] = command sent
        //        Parameters:
        //            string motor_number = motor to be vibrated (1 to 16)
        //            string rhythm = rhythm to vibrate motor in ("A" to "H")
        //            string magnitude = magnitude to vibrate motor in ("1" to "4")
        //            string rhythm_cycles = cycles to repeate the rhythm ("0" to "15")       
        public string[] Vibrate_Motor(byte motor, byte rhythm, byte magnitude, byte rhythm_cycles) {
            byte[] command_byte = { 0x00, 0x00 };
            string[] return_values = new string[2];
            return_values[1] = "";

            if (!serialOut.IsOpen()) {
                return_values[0] = "Serial port not open";
            }
            else {
                //byte mode = 0;
                //byte motor = 0x0; // this is the first valid motor
                //byte rhythm = 0x7; //rhy H =7
                //byte magnitude = 0x0; //mag A = 0
                //byte rhythm_length = 0x6;
                // This equates to hex 01 36

                //active_command_t command = new active_command_t();
                //command.mode = (byte)(mode << 4);
                //command.motor = (byte)(motor & 0xf);
                //command.v.rhythm = (byte)((rhythm & 0x7) << 5);
                //command.v.magnitude = (byte)((magnitude & 0x3) << 3);
                //command.v.duration = (byte)(rhythm_cycles & 0x7);

                try {
                    change_acmd_mode(acmd_mode_t.ACM_VIB);
                    if (acmd_mode == acmd_mode_t.ACM_VIB) {
                        // Use the Headers from Firmware to compile TODO
                        byte mode = (byte)acmd_mode_t.ACM_VIB;
                        command_byte[0] = (byte)((mode << 4) | (motor & 0xf));
                        command_byte[1] = (byte)(((rhythm & 0x7) << 5)
                            | ((magnitude & 0x3) << 3)
                            | (rhythm_cycles & 0x7));
                        
                        serialOut.WriteData(command_byte); // send debug menu activation

                        checkBeltStatus(50); //FIXME not sure of req'd time delay
                        if (belt_error == error_t.ESUCCESS)
                            return_values[0] = "";//Successful if this point is reached w/o error
                    }
                    else {
                        return_values[0] = "Haptic Belt Error Code: "
                            + (int)belt_error + ": " + Constants.error_t_names[(int)belt_error];
                    }
                }
                catch {
                    return_values[0] = "Error sending command over wireless";
                }
            }
            return return_values;
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

        //This function queries the belt using the private Query_All() function and returns a specific magnitude value
        public byte getMotors() {
            byte return_values = 0;

            try { //Convert.ToInt16 can cause exception
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("MTR")) {
                            char motor_count = split[2][0]; // gets char at string[0]
                            return_values = HexToByte(motor_count); // count of motors
                            break; // exit loop
                        }
                    }
                }
            } //[System.NullReferenceException] = {"Object reference not set to an instance of an object."}
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return return_values;
        }

        /*
         * This function queries the belt such that the function returns
         * a 2-dimensional string array of results 
         */
        //Sends belt commands in the following formats:
        //Belt Command: 
        //    QUERY
        //Belt Returns: 
        //    up to 8 responses in the following fashion, seperated by a newline character
        //        RESPONSE RHYTHM <id> <rhythm> <length>
        //            where 
        //                id is between "A" and "H"
        //                rhythm is a 16 character hex string
        //    followed by up to 4 responses in the following fashion, seperated by a newline character
        //        RESPONSE MAGNITUDE <period> <duty cycle>
        //    followed by up to 1 response 
        //        RESPONSE MOTORS ??? ???
        //    followed by a response in the form
        //            STATUS <error num> [<info>]
        //
        //Private functions in this program
        //    private string[,] Query_All_()
        //        Returns:
        //            string[,] array of length [13,4]
        //                where
        //                    index 0 = Version number
        //                    index 1 = rhythm number (A to H) or magnitude number (1 to 4)
        //                    index 2 = individual components of rhythm (up to 8) or magnitude (up to 4) or motors (up to 1)
        //                        where if rhythm
        //                            0 = "RHY"
        //                            1 = rhythm number (A to H)
        //                            2 = rhythm pattern (16 character hex string)
        //                            3 = rhythm length (0 to 64)
        //                        where if magnitude
        //                            0 = "MAG"
        //                            1 = magnitude number (A to D)
        //                            2 = period (in microseconds)
        //                            3 = duty cycle (in microseconds)
        //                        where if motors
        //                            0 = "MTR"
        //                            1 = list of motors seperated by commas
        //        Parameters:
        //            NONE
        public string[] Query_All() {

            //Send the QRY command if ports are setup
            if (!serialOut.IsOpen()) {
                qry_resp[0] = "Serial port not open";
            }
            else {
                try {
                    change_acmd_mode(acmd_mode_t.ACM_LRN);
                    if (acmd_mode == acmd_mode_t.ACM_LRN)
                        serialOut.WriteData("QRY ALL\r");
                }
                catch {
                    qry_resp[0] = "Error sending command over wireless";
                    return qry_resp;
                }
            }
            //Wait for system to respond.  Sometimes there is delay between Belt's echo back and data stream
            checkBeltStatus(250);

            //read responses from Serial_Port and place RSP lines into string array
            if (belt_error == error_t.ESUCCESS) {
                string[] buffer;
                
                //Removes the '\r\n' characters        
                char[] delimiters = new char[] { '\r', '\n' };
                //buffer = Regex.Split(serialIn.MsgInBuffer, "\r\n"); //CANNOT USE REGEX in CF 2.0
                buffer = ByteToAscii(serialIn.DataRecvBuffer()).Split(delimiters);
                int rsp_index = 1;

                // Formats check into string array so each RSP line is an element and removes "\r"
                for (int i = 0; i < buffer.Length; i++) {
                    //Ignore non RSP lines
                    if (buffer[i].Length >= 3) {
                        if (buffer[i].Substring(0, 3).Equals("RSP")) {
                            qry_resp[rsp_index] = buffer[i];
                            rsp_index++;
                        }
                        else if (buffer[i].Substring(0, 3).Equals("STS")) {
                            byte[] errors = IntStrToByte(buffer[i].Split(' ')[1]); // gets first error code
                            belt_error = (error_t)errors[0]; // gets 1st error code byte
                        }
                        else if (buffer[i].Substring(0, 3).Equals("DBG")) {
                            //string[] split = buffer[i].Split(' ');
                            //sys_error = (error_t)split[1][0]; // get char at index 0
                        }
                    }
                }
                rsp_index--; // decrement from last increment to give count of elements
                qry_resp[0] = rsp_index.ToString(); //count of elements
            }
            //Check error status
            if (belt_error != error_t.ESUCCESS) {
                qry_resp[0] = "Haptic Belt Error Code: "
                            + (int)belt_error + ": " + Constants.error_t_names[(int)belt_error];
            }
            return qry_resp;
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
                            serialOut.WriteData(returnState);
                        }

                        serialOut.WriteData(instruction);
                        //check for STATUS <error number> [<info>]
                        checkBeltStatus(50);
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
                            serialOut.WriteData(returnState);
                        }

                        serialOut.WriteData(instruction);
                        //check for STATUS <error number> [<info>]
                        checkBeltStatus(50); 
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

        /*
         * Method switches the global mode
         */
        private void change_glbl_mode(mode_t mode) {
            error_t error = error_t.EMAX;

            if (glbl_mode == mode) {
                error = error_t.ESUCCESS; // already in requested mode
            }
            else if (glbl_mode == mode_t.M_LEARN) {
                //switch to activate command mode
                serialIn.CurrentDataType = SerialPortManager.DataType.Text;
                serialOut.WriteData("BGN\n");
                checkBeltStatus(50);

                if (belt_error == error_t.ESUCCESS)
                    glbl_mode = mode_t.M_ACTIVE;
            }
            else { //if (glbl_mode == mode_t.M_ACTIVE)
                // back to learning mode
                serialIn.CurrentDataType = SerialPortManager.DataType.Text;
                byte[] returnState = { 0x30, 0x30 }; //should be Hex 0x30 0x00
                serialOut.WriteData(returnState);
                checkBeltStatus(50);

                if (belt_error == error_t.ESUCCESS)
                    glbl_mode = mode_t.M_LEARN;
            }
        }

        /*
         * This method is used to change the mode of the application
         * as the firmware mode is changed.  The application and the 
         * firmware modes must stay synchronized.
         */
        private void change_acmd_mode(acmd_mode_t mode) {
            if (acmd_mode == mode) {
                belt_error = error_t.ESUCCESS; // already in requested mode
            }
            else if (mode == acmd_mode_t.ACM_LRN) {
                //return to learning mode
                change_glbl_mode(mode_t.M_LEARN);
                acmd_mode = mode;
                serialIn.CurrentDataType = SerialPortManager.DataType.Text;
            }
            else { //(mode == acmd_mode_t.ACM_VIB || acmd_mode_t.ACM_SPT || acmd_mode_t.ACM_GCL)
                change_glbl_mode(mode_t.M_ACTIVE);
                acmd_mode = mode;
                serialIn.CurrentDataType = SerialPortManager.DataType.Hex;
            }
        }

        //public string[] Back() {
        //    string[] return_values = new string[2];
        //    return_values[1] = "";

        //    if (serialOut.IsOpen) {
        //        //send BACK command through Serial_Port
        //        try {
        //            serialOut.CurrentTransmissionType = SerialPortManager.TransmissionType.Hex;
        //            serialOut.WriteData("48"); //first_byte
        //            serialOut.WriteData("0"); //second_byte

        //            /*FIXME , do we ge a response here? check for STATUS <error number> [<info>]
        //            if (Check_Belt() != 0)
        //                return_values[0] = "No response from belt or belt error";
        //            else
        //                return_values[0] = "";
        //             */
        //            serialOut.CurrentTransmissionType = SerialPortManager.TransmissionType.Text;
        //        }
        //        catch {
        //            return_values[0] = "Error sending command over wireless";
        //        }
        //    }
        //    else
        //        return_values[0] = "Serial port not open";

        //    return return_values;
        //}

        /*
         * This function is used to issue a general STOP command to the belt
         */
        //    string[] Stop()
        //        Returns: 
        //            string array of length 2
        //                where 
        //                    string[0] = error response
        //                    string[1] = NULL
        //        Parameters: 
        //            NONE
        public string[] Stop(byte motor_number) {
            string[] return_values = new String[2];

            if (serialOut.IsOpen()) {
                // sending rhythm_cycles = 0 stops motor
                Vibrate_Motor(motor_number, 0x0, 0x0, 0x0);
                return_values[0] = "";
                return_values[1] = "";
            }
            else {
                return_values[0] = "Serial port not open";
                return_values[1] = "";
            }
            return return_values;
        }


        //this function is used to STOP all motors command to the belt
        public string[] StopAll() {
            string[] return_values = new String[2];

            if (serialOut.IsOpen() && this.getMotors() != 0) {
                for (byte i = 0; i < this.getMotors(); i++) {
                    // sending rhythm_cycles = 0 stops motor
                    Vibrate_Motor(i, 0x0, 0x0, 0x0);
                    return_values[0] = "";
                    return_values[1] = "";
                }
            }
            else {
                return_values[0] = "Serial port not open";
                return_values[1] = "";
            }
            return return_values;
        }

        /*
         * This is used to get status from belt after each send or receive 
         */
        //    private int Check_Belt()
        //        Returns:
        //            integer which is the error number (0 if success)
        //        Parameters:
        //            NONE
        private void checkBeltStatus(int timeout) {
            error_t error = error_t.EMAX;

            // wait for other processes with specified timeout
            // this should be reasonable wait for haptic belt to return status
            System.Threading.Thread.Sleep(timeout);

            // FIXME .NET SerialPort.Read() results in \r\n 
            // BUT I CAN ONLY SEE "\n" WHEN DIRECTLY LISTENING TO PORT???
            //if (serialIn.MsgInBuffer.Equals("STS 0\r\n") || serialIn.MsgInBuffer.Equals("53 54 53 20 30 0D 0A "))

            if (acmd_mode == acmd_mode_t.ACM_LRN) {
                char[] delimiters = new char[] { '\r', '\n', ' ' };
                string[] split = ByteToAscii(serialIn.DataRecvBuffer()).Split(delimiters);
                if (String.Equals(split[0], "STS")) {
                    byte[] errors = IntStrToByte(split[1]); // gets first error code
                    error = (error_t)(errors[0]);
                }
            }
            else if (acmd_mode == acmd_mode_t.ACM_VIB && serialIn.DataRecvBuffer()[0] == 0x00) {
                error = error_t.ESUCCESS;
            }
            else { ; }//do nothing
            belt_error = error;
        }

        /*
         * This function converts hex values to binary
         * By using a Switch function we can provide fault protection
         * against bit errors with serial communicaiton.  Invalid chars
         * will return an Error
         */
        //        Returns:
        //            binary string in form "xxxx"
        //        Parameters:
        //            hex character (0 to F)
        private string HexToBinary(string hexvalue) {
            string binaryval = "";
            char[] c = hexvalue.ToCharArray();

            for (int i = 0; i < hexvalue.Length; i++) {
                switch (c[i]) {
                    case '0':
                        binaryval += "0000";
                        continue;
                    case '1':
                        binaryval += "0001";
                        continue;
                    case '2':
                        binaryval += "0010";
                        continue;
                    case '3':
                        binaryval += "0011";
                        continue;
                    case '4':
                        binaryval += "0100";
                        continue;
                    case '5':
                        binaryval += "0101";
                        continue;
                    case '6':
                        binaryval += "0110";
                        continue;
                    case '7':
                        binaryval += "0111";
                        continue;
                    case '8':
                        binaryval += "1000";
                        continue;
                    case '9':
                        binaryval += "1001";
                        continue;
                    case 'A':
                        binaryval += "1010";
                        continue;
                    case 'B':
                        binaryval += "1011";
                        continue;
                    case 'C':
                        binaryval += "1100";
                        continue;
                    case 'D':
                        binaryval += "1101";
                        continue;
                    case 'E':
                        binaryval += "1110";
                        continue;
                    case 'F':
                        binaryval += "1111";
                        continue;
                    default:
                        // clears all previous values to record error            
                        binaryval = "Error";
                        break; // exits loop
                }
            }
            return binaryval;
        }
        /*
        * This function converts char representation of hex values to byte value
        */
        //        Returns:
        //            byte value
        //        Parameters:
        //            hex character (0 to F)
        private byte HexToByte(char hexValue) {
            byte byteValue = 0;
            switch (hexValue) {
                case '0':
                    byteValue = 0;
                    break;
                case '1':
                    byteValue = 1;
                    break;
                case '2':
                    byteValue = 2;
                    break;
                case '3':
                    byteValue = 3;
                    break;
                case '4':
                    byteValue = 4;
                    break;
                case '5':
                    byteValue = 5;
                    break;
                case '6':
                    byteValue = 6;
                    break;
                case '7':
                    byteValue = 7;
                    break;
                case '8':
                    byteValue = 8;
                    break;
                case '9':
                    byteValue = 9;
                    break;
                case 'A':
                    byteValue = 10;
                    break;
                case 'B':
                    byteValue = 11;
                    break;
                case 'C':
                    byteValue = 12;
                    break;
                case 'D':
                    byteValue = 13;
                    break;
                case 'E':
                    byteValue = 14;
                    break;
                case 'F':
                    byteValue = 15;
                    break;
                default:
                    byteValue = 0;
                    break;
            }
            return byteValue;
        }

        internal static char[] hexDigits = {
        '0', '1', '2', '3', '4', '5', '6', '7',
        '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        internal static string BytesToHexStr(byte[] bytes) {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++) {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }



        /*
         * This function converts an string representation of an int value to 
         * a byte value up to the max unsigned 8 bit value (255).  Given these
         * limitations, this function converts only the first 3 characters
         */
        private byte[] IntStrToByte(string intString) {
            byte[] byteValue = { 0, 0, 0, 0, 0, 0, 0, 0 }; // 4bytes = 32bits

            int i = 0;
            for (byte b = 0; b < byteValue.Length; b++) {
                if ((i + 1) < intString.Length) {
                    byteValue[b] = (byte)(HexToByte(intString[i]) << 4 | HexToByte(intString[i + 1]));
                    i += 2;
                }
                else if (i < intString.Length) {
                    byteValue[b] = (byte)(HexToByte(intString[i]));
                    i++;
                }
            }
            return byteValue;
        }

        /*
         * This function converts an string representation of an int value to 
         * the motor number offset
         */
        private byte MotorStrToByte(string strInteger) {
            byte byteValue = 0;
            switch (strInteger) {
                case "1":
                    byteValue = 0;
                    break;
                case "2":
                    byteValue = 1;
                    break;
                case "3":
                    byteValue = 2;
                    break;
                case "4":
                    byteValue = 3;
                    break;
                case "5":
                    byteValue = 4;
                    break;
                case "6":
                    byteValue = 5;
                    break;
                case "7":
                    byteValue = 6;
                    break;
                case "8":
                    byteValue = 7;
                    break;
                case "9":
                    byteValue = 8;
                    break;
                case "10":
                    byteValue = 9;
                    break;
                case "11":
                    byteValue = 10;
                    break;
                case "12":
                    byteValue = 11;
                    break;
                case "13":
                    byteValue = 12;
                    break;
                case "14":
                    byteValue = 13;
                    break;
                case "15":
                    byteValue = 14;
                    break;
                case "16":
                    byteValue = 15;
                    break;
                default:
                    byteValue = 0;
                    break;
            }
            return byteValue;
        }

        /*
         * This function converts an alpha character of the vibration pattern 
         * to an byte value used for the firmware starting with A=0
         */
        private byte VibStrToByte(string alphaStr) {
            byte byteValue = 8;
            switch (alphaStr) {
                case "A":
                    byteValue = 0;
                    break;
                case "B":
                    byteValue = 1;
                    break;
                case "C":
                    byteValue = 2;
                    break;
                case "D":
                    byteValue = 3;
                    break;
                case "E":
                    byteValue = 4;
                    break;
                case "F":
                    byteValue = 5;
                    break;
                case "G":
                    byteValue = 6;
                    break;
                case "H":
                    byteValue = 7;
                    break;
                default:
                    byteValue = 8;
                    break;
            }
            return byteValue;
        }

        /*
 * This function converts an alpha character to an integer starting with A=1
 */
        private string IntStrToAlpha(string intStr) {
            string strValue = "";
            switch (intStr) {
                case "1":
                    strValue = "A";
                    break;
                case "2":
                    strValue = "B";
                    break;
                case "3":
                    strValue = "C";
                    break;
                case "4":
                    strValue = "D";
                    break;
                case "5":
                    strValue = "E";
                    break;
                case "6":
                    strValue = "F";
                    break;
                case "7":
                    strValue = "G";
                    break;
                case "8":
                    strValue = "H";
                    break;
                default:
                    strValue = "A";
                    break;
            }
            return strValue;
        }

        #region HexToByte
        /// <summary>
        /// method to convert hex string into a byte array
        /// </summary>
        /// <param name="msg">string to convert</param>
        /// <returns>a byte array</returns>
        private byte[] HexToByte(string msg) {
            //remove any spaces from the string
            msg = msg.Replace(" ", "");

            //create a byte array the length divided by 2 (Hex is 2 characters in length)
            byte[] comBuffer = new byte[msg.Length / 2];
            //loop through the length of the provided string
            for (int i = 0; i < msg.Length; i += 2)
                //convert each set of 2 characters to a byte and add to the array
                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);

            //return the array
            return comBuffer;
        }
        #endregion

        #region ByteToHex
        /// <summary>
        /// method to convert a byte array into a hex string
        /// </summary>
        /// <param name="comByte">byte array to convert</param>
        /// <returns>a hex string</returns>
        private string ByteToHex(byte[] comByte) {
            //create a new StringBuilder object
            StringBuilder builder = new StringBuilder(comByte.Length * 3);

            //loop through each byte in the array
            foreach (byte data in comByte)
                //convert the byte to a string and add to the stringbuilder
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));

            //return the converted value
            return builder.ToString().ToUpper();
        }
        #endregion

        #region ByteToAscii
        /// <summary>
        /// method to convert a byte array into a ASCII string
        /// </summary>
        /// <param name="comByte">byte array to convert</param>
        /// <returns>a hex string</returns>
        private string ByteToAscii(byte[] comByte) {

            //return the converted value
            return Encoding.ASCII.GetString(comByte);
        }
        #endregion
    }
}