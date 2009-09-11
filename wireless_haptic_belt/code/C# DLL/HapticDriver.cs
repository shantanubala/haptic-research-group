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

        // Boolean if any motors are activated (helps indicate current location in the debug menu)
        private bool activated = false; //TODO


        #region Manager Constructors
        /// <summary>
        /// Constructor to set the properties of our Driver Class
        /// </summary>
        public HapticBelt() {
            // PLACEHOLDERS
            serialIn = new SerialPortManager(); // allows access to GetPortNames();
            //serialOut = new SerialPortManager();

            //// Function pointer
            //if (DataReceivedFxn != null) // Always seems to be null
            //{
            //    serialIn.DataReceivedFxn += new SerialPortManager.DataRecievedHandler(DataReceivedFxn);
            //}

            qry_resp = new string[TOTAL_RESPONSE_COUNT + 1];
        }
        #endregion

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
                else
                    serialOut = serialIn;
            }
        }

        public void ClosePorts() {
            if (serialIn != null && serialOut != null) {
                serialIn.ClosePort();
                serialOut.ClosePort();
            }
        }

        public string[] getPortNames() {
            return serialIn.getPortNames;
        }

        public void WriteData(string dataString) {
            if (port_setup && serialOut.IsOpen)
                serialOut.WriteData(dataString);
        }

        public string getMsgBufferType() {
            return serialIn.MsgInBufferType;
        }

        public string getStatusBufferType() {
            return serialOut.StatusBufferType;
        }

        public string getMsgBuffer() {
            return serialIn.MsgInBuffer;
        }

        public string getStatusBuffer() {
            return serialOut.StatusBuffer;
        }


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
                serialIn.CurrentTransmissionType = SerialPortManager.TransmissionType.Text;

                if (portInName != portOutName) {
                    serialOut = new SerialPortManager(portOutName, baud, dBits, sBits, par, timeout);
                    serialOut.CurrentTransmissionType = SerialPortManager.TransmissionType.Text;
                }

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
        public string[] Vibrate_Motor(string motor_number, string rhythm_string, string magnitude_string, int rhythm_cycles) {
            string[] return_values = new string[2];
            return_values[1] = "";

            if (serialOut.IsOpen) {
                byte mode = 0;
                byte motor = NumberStrToByte(motor_number);
                byte rhythm = AlphaStrToByte(rhythm_string);
                byte magnitude = AlphaStrToByte(magnitude_string);
                byte rhythm_length = (byte)(rhythm_cycles);

                //byte[] buffer = { mode, motor, rhythm, magnitude, rhythm_length };
                string first_byte = "" + (mode * 16) + motor;
                string second_byte = "" + (rhythm * 32) + (magnitude * 8) + rhythm_length;

                //Send in binary over Serial_Port FIXME Need to use writeline at the second_byte?
                try {
                    //TODO Temporary fix to activate motors through debug menu
                    //if (!activated) {
                        first_byte = "\r\r\r3" + IntStrToAlpha(motor_number) + rhythm_string 
                            + magnitude_string + rhythm_cycles.ToString() + "\r\r\n0";
                            // back to direct command line entry
                        activated = true;
                    //}
                    //else {
                    //    first_byte = IntStrToAlpha(motor_number) + rhythm_string + magnitude_string + rhythm_cycles.ToString() + "\r";
                    //}
                    serialOut.WriteData(first_byte); // send debug menu activation

                    if (rhythm_cycles == 0) {
                        //serialOut.WriteData("\r\n0"); // back to direct command line entry
                        activated = false;
                    }
                    //************** END OF TEMPORARY FIX *********************//
                    //serialOut.CurrentTransmissionType = SerialPortManager.TransmissionType.Hex;
                    //serialOut.WriteData(first_byte); // send first_byte
                    //serialOut.WriteData(second_byte); // send second_byte

                    ////Successful if this point is reached w/o error
                    //serialOut.CurrentTransmissionType = SerialPortManager.TransmissionType.Text;
                    //return_values[0] = "";
                }
                catch {
                    return_values[0] = "Error sending command over wireless";
                }
            }
            else
                return_values[0] = "Serial port not open";

            return return_values;
        }

        //This function queries the belt using the private Query_All() function and returns a specific rhythm value
        public string[] getRhythm(Boolean binary) {
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
                    string[] split = qry_resp[index].Split(' ');

                    //put the values from the response into the return array
                    if (split[1].Equals("MAG")) {
                        //Populate Return Values --> Equals "Rhy,period,dutyCycle"
                        return_values[magCount + 1] = split[2] + "," + split[3] + "," + split[4];
                        magCount++; // count of defined magnitudes
                    }
                }
            } //[System.NullReferenceException] = {"Object reference not set to an instance of an object."}
            catch (Exception ex) {

            }
            return_values[0] = magCount.ToString(); // count of defined magnitudes
            return return_values;
        }

        //This function queries the belt using the private Query_All() function and returns a specific magnitude value
        public string[] getMotors() {
            string[] return_values = new string[MTR_MAX_NO + 1];
            return_values[0] = "NONE DEFINED";

            try { //Convert.ToInt16 can cause exception
                for (int index = 1; index < qry_resp.Length; index++) {
                    if (qry_resp[index] != null) {
                        string[] split = qry_resp[index].Split(' ');

                        //put the values from the response into the return array
                        if (split[1].Equals("MTR")) {
                            //Populate Return Values up to max of split[2]'s value
                            return_values[0] = split[2]; // count of motors
                            for (int i = 1; i <= Convert.ToInt16(split[2]); i++) {
                                return_values[i] = i.ToString();
                            }

                        }
                    }
                }
            } //[System.NullReferenceException] = {"Object reference not set to an instance of an object."}
            catch (Exception ex) {

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

            //this function needs to query all motors
            string instruction = "QRY ALL\r";

            // Reset data ready flag
            serialIn._data_ready = false;

            //Send the QRY command if ports are setup
            if (serialOut.IsOpen) {
                try {
                    serialOut.WriteData(instruction);
                }
                catch {
                    qry_resp[0] = "Error sending command over wireless";
                    return qry_resp;
                }
            }
            //read responses from Serial_Port and place RSP lines into string array
            if (serialIn.IsOpen) {
                try {
                    string[] buffer;

                    //Wait for system to respond.  Sometimes there is delay between Belt's echo back and data stream
                    while (serialIn._data_ready == false) {
                        System.Threading.Thread.Sleep(0); //1500 // Wait for other processes
                    }

                    //Removes the '\r\n' characters        
                    char[] delimiters = new char[] { '\r', '\n' };
                    //buffer = Regex.Split(serialIn.MsgInBuffer, "\r\n"); //CANNOT USE REGEX in CF 2.0
                    buffer = serialIn.MsgInBuffer.Split(delimiters);
                    int rsp_index = 1;

                    // Formats check into string array so each RSP line is an element and removes "\r"
                    for (int i = 0; i < buffer.Length; i++) {
                        //Ignore non RSP lines
                        //TODO Ignores STS and DBG lines, maybe we want a STS array and DBG array?
                        if (buffer[i].Length >= 3) {
                            if (buffer[i].Substring(0, 3).Equals("RSP")) {
                                qry_resp[rsp_index] = buffer[i];
                                rsp_index++;
                            }
                        }
                    }
                    rsp_index--; // decrement from last increment to give count of elements
                    qry_resp[0] = rsp_index.ToString(); //count of elements
                }
                catch (Exception e) {
                    qry_resp[0] = e.ToString();
                    //return_values[0, 0] = "Error receiving query over wireless";
                    return qry_resp;
                }
            }
            //else
            //    ; // TODO pass some error about ports not open

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

            if (serialOut.IsOpen) {
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
                        serialOut.WriteData(instruction);
                        //check for STATUS <error number> [<info>]
                        if (Check_Belt() != 0) {
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

            if (serialOut.IsOpen) {
                if (String.Compare(id, "A") < 0 || String.Compare(id, "D") > 0) {
                    //invalid magnitude ID
                    return_values[0] = "Invalid magnitude ID provided as argument to function";
                    return_values[1] = "";
                }
                else {
                    string instruction = String.Concat("LRN MAG ", id, " ", period, " ", duty_cycle);

                    //send this output to the belt
                    try {
                        serialOut.WriteData(instruction);
                        //check for STATUS <error number> [<info>]
                        if (Check_Belt() != 0) {
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

        public string[] Back() {
            string[] return_values = new string[2];
            return_values[1] = "";

            if (serialOut.IsOpen) {
                //send BACK command through Serial_Port
                try {
                    serialOut.CurrentTransmissionType = SerialPortManager.TransmissionType.Hex;
                    serialOut.WriteData("48"); //first_byte
                    serialOut.WriteData("0"); //second_byte

                    /*FIXME , do we ge a response here? check for STATUS <error number> [<info>]
                    if (Check_Belt() != 0)
                        return_values[0] = "No response from belt or belt error";
                    else
                        return_values[0] = "";
                     */
                    serialOut.CurrentTransmissionType = SerialPortManager.TransmissionType.Text;
                }
                catch {
                    return_values[0] = "Error sending command over wireless";
                }
            }
            else
                return_values[0] = "Serial port not open";

            return return_values;
        }

        /* 
         * this function is used to issue a general START command to the belt
         */
        //Belt Command: 
        //    START
        //Belt Returns:
        //    a response in the form
        //        STATUS <error num> [<info>]
        //
        //    string[] Start()
        //        Returns: 
        //            string array of length 2
        //                where 
        //                    string[0] = error response
        //                    string[1] = NULL
        //        Parameters: 
        //            NONE
        public string[] Start() {
            string[] return_values = new string[2];

            if (serialOut.IsOpen) {
                string instruction = "BGN\r\n";
                //send START command through Serial_Port
                try {
                    serialOut.WriteData(instruction);
                    //check for STATUS <error number> [<info>]
                    if (Check_Belt() != 0) {
                        return_values[0] = "No response from belt or belt error";
                        return_values[1] = "";
                    }
                    else {
                        return_values[0] = "";
                        return_values[1] = "";
                    }
                }
                catch {
                    return_values[0] = "Error sending command over wireless";
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
        public string[] Stop(string motor_number) {
            string[] return_values = new String[2];

            if (serialOut.IsOpen) {
                Vibrate_Motor(motor_number, "A", "A", 0);
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

            if (serialOut.IsOpen) {
                string[] motors = this.getMotors();
                for (int i = 1; i < motors.Length; i++) {
                    if (motors[i] != null) {
                        Vibrate_Motor(motors[i], "A", "A", 0);
                        return_values[0] = "";
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
         * This is used to get status from belt after each send or receive 
         */
        //    private int Check_Belt()
        //        Returns:
        //            integer which is the error number (0 if success)
        //        Parameters:
        //            NONE
        private int Check_Belt() {
            string temp;

            try {
                temp = serialIn.MsgInBuffer;
            }
            catch {
                return -9;
            }

            string[] split = temp.Split(' ');

            if (!String.Equals(split[0], "STS")) {
                return -9;
            }
            else {
                return Convert.ToInt32(split[1]);
            }
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
            // NOT USED DUE TO .NET FUNCTION -> trying to make code portable
            //if (hexvalue.Length <= 16)
            //    binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
            //else
            //    binaryval = Convert.ToString(Convert.ToInt64(hexvalue, 16), 2);

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
         * This function converts an int value to the alpha character starting with 1=A
         */
        private byte NumberStrToByte(string strInteger) {
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
         * This function converts an alpha character to an integer starting with A=1
         */
        private byte AlphaStrToByte(string alphaStr) {
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
    }
}