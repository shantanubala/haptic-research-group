//Haptic Belt Accessor Functions for Sending belt commands from C# GUI interface
//Author: Daniel Moberly
//Date: April 20th, 2009

// *
// * MODIFIED to work on Windows Mobile 5.0 or Microsoft CF 2.5
// * Oct 22, 2009
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
    public partial class HapticBelt //: SerialPortManager // Derived  "extends" from SerialPortMananger
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
                byteString +=  "++"+ Constants.status_msg_names[status[i]];
            }
            return byteString;
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

                if (portInName != portOutName) {
                    serialOut = new SerialPortManager(portOutName, baud, dBits, sBits, par, timeout);
                }
                else serialOut = serialIn;

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
                serialOut.WriteData(dataString, 200);
        }

        public void WriteData(byte[] data) {
            if (port_setup && serialOut.IsOpen())
                serialOut.WriteData(data, 200);
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

                        serialOut.WriteData(command_byte, 50); // send debug menu activation

                        checkBeltStatus();
                        if (belt_error == error_t.ESUCCESS)
                            return_values[0] = "";//Successful if this point is reached w/o error
                    }
                    else {
                        return_values[0] = "Haptic Belt Error Code: "
                            + belt_error.ToString() + ": ";// +Constants.error_t_names[belt_error];
                    }
                }
                catch {
                    return_values[0] = "Error sending command over wireless";
                }
            }
            return return_values;
        }

        //This function returns the motor count from the last QRY operation
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
            } 
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
        private string[] Query(string typeMsg, int timeout) {

            clearBuffer(qry_resp);

            //Send the QRY command if ports are setup
            if (!serialOut.IsOpen()) {
                qry_resp[0] = "Serial port not open";
            }
            else {
                try {
                    change_acmd_mode(acmd_mode_t.ACM_LRN);
                    if (acmd_mode == acmd_mode_t.ACM_LRN)
                        // Send command with wait time for belt to respond back.
                        serialOut.WriteData(typeMsg, timeout); 
                }
                catch {
                    qry_resp[0] = "Error sending command over wireless";
                    return qry_resp;
                }
            }
            checkBeltStatus();

            //read responses from Serial_Port and place RSP lines into string array
            if (belt_error == error_t.ESUCCESS) {
                string[] buffer;

                //Removes the '\r\n' characters        
                char[] delimiters = new char[] { '\r', '\n' }; //, ' '
                //buffer = Regex.Split(serialIn.MsgInBuffer, "\r\n"); //CANNOT USE REGEX in CF 2.0
                buffer = ByteToAscii(serialIn.DataRecvBuffer()).Split(delimiters);
                int rsp_index = 1;

                // Formats buffer into string array so each RSP line is an element and removes "\r"
                for (int i = 0; i < buffer.Length; i++) {
                    //Ignore non RSP lines
                    if (buffer[i].Length >= 3) {
                        if (buffer[i].Substring(0, 3).Equals("RSP")) {
                            qry_resp[rsp_index] = buffer[i];
                            rsp_index++;
                        }
                        //else if (buffer[i].Substring(0, 3).Equals("STS")) {
                        //    byte[] errors = IntStrToByte(buffer[i].Split(' ')[1]); // gets first error code
                        //    belt_error = (error_t)errors[0]; // gets 1st error code byte
                        //}
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
                            + belt_error.ToString() + ": "; //+ Constants.error_t_names[(int)belt_error];
            }
            return qry_resp;
        }

        public string[] Query_All() {
            return Query("QRY ALL\r", 250);
        }

        public string[] Query_Motor() {
            return Query("QRY MTR\r", 50);
        }

        public string[] Query_SpatioTemporal() {
            return Query("QRY SPT\r", 250);
        }

        public string[] Query_Version() {
            return Query("QRY VER\r", 50);
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
                serialOut.WriteData("BGN\n", 50);
                checkBeltStatus();

                if (belt_error == error_t.ESUCCESS)
                    glbl_mode = mode_t.M_ACTIVE;
            }
            else { //if (glbl_mode == mode_t.M_ACTIVE)
                // back to learning mode
                serialIn.CurrentDataType = SerialPortManager.DataType.Text;
                byte[] returnState = { 0x30, 0x30 }; //should be Hex 0x30 0x00
                serialOut.WriteData(returnState, 50);
                checkBeltStatus();

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
        private void checkBeltStatus() {
            error_t error = error_t.EMAX;

            // FIXME .NET SerialPort.Read() results in \r\n 
            // BUT I CAN ONLY SEE "\n" WHEN DIRECTLY LISTENING TO PORT???
            //if (serialIn.MsgInBuffer.Equals("STS 0\r\n") || serialIn.MsgInBuffer.Equals("53 54 53 20 30 0D 0A "))

            if (acmd_mode == acmd_mode_t.ACM_LRN) {
                char[] delimiters = new char[] { '\r', '\n', ' ' };
                string[] split = ByteToAscii(serialIn.DataRecvBuffer()).Split(delimiters);
                for (int i = 0; i < split.Length; i++) {
                    if (String.Equals(split[i], "STS")) {
                        byte[] errors = IntStrToByte(split[i+1]); // gets first error code
                        error = (error_t)(errors[0]);
                    }
                }
            }
            else if (acmd_mode == acmd_mode_t.ACM_VIB && serialIn.DataRecvBuffer()[0] == 0x00) {
                error = error_t.ESUCCESS;
            }
            else { ; }//do nothing
            belt_error = error;
        }

        private void clearBuffer(string[] buffer) {
            // Clear garbage
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = null;
        }

    }
}