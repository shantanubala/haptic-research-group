/*****************************************************************************
 * FILE:   HapticDriver.cs
 * AUTHOR: Daniel Moberly (Daniel.Moberly@gmail.com)
 *         Kris Blair (Kristopher.Blair@asu.edu)
 *         Nathan J. Edwards (nathan.edwards@asu.edu)
 *         
 * DESCR:  Haptic Belt DLL public interface that links C# or other high level
 *         application to Serial Communications with the haptic belt firmware.
 * LOG:    20090420 - initial version
 *         20091109 - refactored DLL to smaller code files, added features
 *                    from prototype GUI application. MODIFIED to work on 
 *                    Windows Mobile 5.0 or Microsoft CF 2.5 or 2.0.
 *                    Commented all methods using .NET XML document syntax
 *                    so that the helper pop-ups are available in Visual
 *                    Studio when using the DLL.
 *         20091120 - Fixed GetSerialPortNames() to handle all COM port No.
 *         20091201 - Added our own implementation of a blocking wait,
 *                    System.Threading.Thread.Sleep() is not desirable.
 ****************************************************************************/

using System;
using System.IO.Ports;

namespace HapticDriver
{
    /// <summary>
    /// Haptic Belt Driver class that is the public interface for all belt functions and control.
    /// </summary>
    public partial class HapticBelt
    {
        /// <summary>
        /// The maximum time to wait for a status response from
        /// the Haptic Belt before generating a timeout error.  The
        /// default is 400 milliseconds, which might need to be increased
        /// for long wireless transmission distances (up to 100m outdoors).
        /// </summary>
        public int MAX_RESPONSE_TIMEOUT = 500; // milliseconds

        //Responses:
        //VER x 1, MAG x 4, RHY x 8, MTR x 1, BAT x1 = 15
        private static int TOTAL_RESPONSE_COUNT = 14;
        /// <summary>
        /// Maximum number of vibrate motors supported
        /// </summary>
        public static int MTR_MAX_NO = 16;

        /// <summary>
        /// Maximum number of magnitude settings supported
        /// </summary>
        public static int MAG_MAX_NO = 4;

        /// <summary>
        /// Maximum number of rhythm patterns supported
        /// </summary>
        public static int RHY_MAX_NO = 5;

        // port variables used for SerialPort communications
        private SerialPortManager serialIn;
        private SerialPortManager serialOut;

        private bool port_setup = false;
        private string _portInName = "";
        private string _portOutName = "";

        /// <summary>
        /// Delegate handler used for a Data Recieved event
        /// </summary>
        public delegate void DataRecievedHandler();

        /// <summary>
        /// .NET Delegate which works as a function pointer
        /// that is called when data is recieved on the Serial COM port.
        /// </summary>
        public DataRecievedHandler DataReceivedFxn;

        //this is the array of results which will be returned to the calling function
        private string[] qry_resp;

        // enum for current firmware mode
        private acmd_mode_t _acmd_mode = acmd_mode_t.ACM_LRN;
        private error_t _belt_error = error_t.EMAX;
        private error_t _dll_error = error_t.EMAX;
        private mode_t _glbl_mode = mode_t.M_LEARN;

        #region Manager Constructors
        /// <summary>
        /// Generic Constructor of our Driver Class
        /// </summary>
        public HapticBelt() {

            qry_resp = new string[TOTAL_RESPONSE_COUNT + 1];
        }
        #endregion

        #region COM PORT data accessors
        /// <summary>
        /// Accessor method that queries the Operating System for available Serial Ports
        /// </summary>
        /// <returns>String array of available serial ports in system</returns>
        public string[] GetSerialPortNames() {
            string[] ports = SerialPort.GetPortNames();

            // Protection against clobbered names like "COM12c" or "COM6o".
            // Only "COM12" or "COM6" is valid in such case, so truncate the last char.
            for (int i = 0; i < ports.Length; i++) {

                // Replace all non-digit chars after "COM" with spaces and then trim.
                String decDigits = replaceNonDecDigits(ports[i].Substring(3, ports[i].Length - 3));
                ports[i] = "COM" + decDigits.Trim();
            }
            return ports;
        }

        /// <summary>
        /// Accessor method that returns data received type from the incoming
        /// serial COM port
        /// </summary>
        /// <returns>MessageType { INCOMING, OUTGOING, NORMAL, WARNING, ERROR }</returns>
        public byte getDataRecvType() {
            return serialIn.DataRecvBufferType();
        }

        /// <summary>
        /// Accessor method that returns data received buffer byte array 
        /// from the incoming serial COM port
        /// </summary>
        public string getDataRecvBuffer() {
            return ByteToAscii(serialIn.DataRecvBuffer());
        }
        #endregion

        #region Driver Status accessors
        /// <summary>
        /// Accessor method that returns status buffer type
        /// from the outgoing serial COM port.
        /// </summary>
        /// <returns>MessageType { INCOMING, OUTGOING, NORMAL, WARNING, ERROR }</returns>
        public byte getStatusType() {
            return serialOut.StatusBufferType();
        }

        /// <summary>
        /// Accessor method that returns status buffer string
        /// from the outgoing serial COM port
        /// </summary>
        public string getStatusBuffer() {
            return ByteToAscii(serialOut.StatusBuffer());
        }

        /// <summary>
        /// Accessor method that returns status buffer string message
        /// from the outgoing serial COM port
        /// </summary>
        public string getStatusBufferStr() {
            string byteString = "";
            byte[] status = serialOut.StatusBuffer();
            for (int i = 0; i < status.Length; i++) {
                byteString += "++" + Constants.error_t_names[status[i]];
            }
            return byteString;
        }
        #endregion

        /// <summary>
        /// Accessor method that returns the Haptic Belt Hardware's last error code
        /// </summary>
        public error_t getHardwareStatus() {
            return _belt_error;
        }
        /// <summary>
        /// Accessor method that returns the Haptic Belt DLL last error code.  This may
        /// be a hardware or software error
        /// </summary>
        public error_t getStatus() {
            return _dll_error;
        }
        /// <summary>
        /// Accessor method that returns the error code message for a particular error code.
        /// </summary>
        public string getErrorMsg(error_t error_code) {
            return "Code: " + (int)error_code + " " + Constants.error_t_names[(int)error_code];
        }

        /// <summary>
        /// Initializes the Serial COM ports
        /// </summary>
        /// <param name="portInName">COM port number used for recieving data, ie "COM8"</param>
        /// <param name="portOutName">COM port number used for sending data, ie "COM8"</param>
        /// <param name="baud">baud rate of connection</param>
        /// <param name="dBits">number of data bits to be sent at a time </param>
        /// <param name="sBits">stop bits to be used (0, 1, 1.5, 2)</param>
        /// <param name="par">parity bits to be used (None, Odd, Even, Mark, Space)</param>
        /// <param name="timeout">specifies the default timeout for sending/recieving data</param>
        /// <returns>error code resulting from COM port setup</returns>
        public error_t SetupPorts(string portInName, string portOutName, string baud, string dBits, string sBits, string par, int timeout) {
            error_t error = error_t.COMPRTSETUP;

            // validate parameters
            if (portInName == null || portOutName == null || baud == null || dBits == null
                || sBits == null || par == null || timeout == 0) {

                error = error_t.COMPRTINVALID;
            }
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

                port_setup = true;
                error = error_t.ESUCCESS;
            }
            catch {
                //error = error_t.COMPRTSETUP; // already default
            }
            _dll_error = error;
            return error;
        }

        /// <summary>
        /// Opens the COM ports if they have already been setup using SetupPorts().
        /// </summary>
        public error_t OpenPorts() {
            error_t error = error_t.COMPRTNOTOPEN;

            if (port_setup) {
                // this.DataRecievedFxn is already Method = {Void MethodName()} from parent class
                if (DataReceivedFxn != null) {
                    // serialIn.DataRecievedFxn becomes Method = {Void Invoke()}
                    serialIn.DataReceivedFxn += new SerialPortManager.DataRecievedHandler(DataReceivedFxn);
                }
                error = serialIn.OpenPort();
                if (_portInName != _portOutName)
                    error = serialOut.OpenPort();

                // Send  Use HEX 0x0A for newline to signal to belt "End of Transmission"
                // In case the belt is in an unknown byte count or state
                // expect COM port read timeout since nothing should be returned
                byte[] test = { 0x0A }; // == '\n'                
                SerialPortWriteData(test, 10);
            }
            _dll_error = error;
            return error;
        }

        /// <summary>
        /// Closes all COM ports in use for the wireless haptic belt.
        /// </summary>
        public error_t ClosePorts() {
            error_t error = error_t.COMPRTOPEN;

            if (serialIn != null && serialOut != null) {
                error = serialIn.ClosePort();
                if (_portInName != _portOutName)
                    error = serialOut.ClosePort();
            }
            // Reset mode variables & state machine
            _glbl_mode = mode_t.M_LEARN;
            _acmd_mode = acmd_mode_t.ACM_LRN;

            _dll_error = error;
            return error;
        }

        /*
        * Wrapper used to get status from belt after each send or receive
        * using the global variable for acmd mode
        */
        private void checkBeltStatus() {
            checkBeltStatus(_acmd_mode);
        }

        /*
         * Function is used to get status from belt after each send or receive 
         */
        private void checkBeltStatus(acmd_mode_t acmd_mode) {
            error_t error = error_t.EMAX;

            // .NET SerialPort.ReadLine() results in \r\n 
            // BUT YOU CAN ONLY SEE "\n" WHEN DIRECTLY LISTENING TO PORT
            //if (serialIn.MsgInBuffer.Equals("STS 0\r\n") || serialIn.MsgInBuffer.Equals("53 54 53 20 30 0D 0A "))

            if (acmd_mode == acmd_mode_t.ACM_LRN) {
                char[] delimiters = new char[] { '\r', '\n', ' ' };
                string[] split = ByteToAscii(serialIn.DataRecvBuffer()).Split(delimiters);
                for (int i = 0; i < split.Length; i++) {
                    if (String.Equals(split[i], "STS")) {
                        byte[] errors = IntegerStrToByte(split[i + 1]); // gets first error code
                        error = (error_t)(errors[0]);
                    }
                }
            }
            // Otherwise the error code is returned as a hex digit without '\n'
            else if (acmd_mode == acmd_mode_t.ACM_VIB) {
                error = (error_t)(serialIn.DataRecvBuffer()[0]);
            }
            else { ; }//do nothing
            _belt_error = error;
        }

        private void clearBuffer(string[] buffer) {
            // Clear garbage
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = null;
        }

        /// <summary>
        /// Send a string to the outgoing serial COM port
        /// </summary>
        /// <param name="dataString"></param>
        /// /// <param name="responseTimeout">Time allowed to recieve data on incoming 
        /// serial COM port before processing buffer</param>
        /// <returns>error code resulting from sending the string data</returns>
        public error_t SerialPortWriteData(string dataString, int responseTimeout) {
            error_t error = error_t.COMPRTWRITE;

            if (!port_setup)
                error = error_t.COMPRTSETUP;
            else if (!serialOut.IsOpen())
                error = error_t.COMPRTNOTOPEN;
            else {
                error = serialOut.WriteData(dataString);
                if (error == error_t.ESUCCESS) {
                    //get response from belt
                    error = serialIn.ReceiveData(SerialPortManager.DataType.Text, responseTimeout);
                    if (error == error_t.ESUCCESS) {
                        checkBeltStatus();
                        error = _belt_error;
                    }
                }
            }
            _dll_error = error;
            return error;
        }

        /// <summary>
        /// Send binary data (byte array) to the outgoing serial COM port
        /// </summary>
        /// <param name="data"></param>
        /// /// <param name="responseTimeout">Time allowed to recieve data on incoming 
        /// serial COM port before processing buffer</param>
        /// <returns>error code resulting from sending the binary data</returns>
        public error_t SerialPortWriteData(byte[] data, int responseTimeout) {
            error_t error = error_t.COMPRTWRITE;

            if (!port_setup)
                error = error_t.COMPRTSETUP;
            else if (!serialOut.IsOpen())
                error = error_t.COMPRTNOTOPEN;
            else {
                error = serialOut.WriteData(data);
                if (error == error_t.ESUCCESS) {
                    //get response from belt
                    error = serialIn.ReceiveData(SerialPortManager.DataType.Hex, responseTimeout);
                    if (error == error_t.ESUCCESS) {
                        checkBeltStatus();
                        error = _belt_error;
                    }
                }
            }
            _dll_error = error;
            return error;
        }

        /*
         * Method switches the global mode
         */
        private error_t change_glbl_mode(mode_t mode) {
            error_t return_error = error_t.EMAX;

            if (_glbl_mode == mode) {
                // already in requested mode, do nothing and no loss of performance
                _belt_error = error_t.ESUCCESS; // already in requested mode
                return_error = _belt_error;
            }
            else if (mode == mode_t.M_ACTIVE) {
                //switch to activate command mode
                return_error = SerialPortWriteData("BGN\n", MAX_RESPONSE_TIMEOUT);

                if (return_error == error_t.ESUCCESS)
                    _glbl_mode = mode_t.M_ACTIVE;
            }
            else { //if (_glbl_mode == mode_t.M_ACTIVE)
                // back to learning mode
                byte[] returnState = { 0x30, 0x30 }; //should send Hex 0x30 0x30 = mode 0
                return_error = SerialPortWriteData(returnState, MAX_RESPONSE_TIMEOUT);

                // BROWN OUT HANDLING - Error from sending 0x30 0x30 can return odd 
                // error code if belt is currently in ACM.LRN 
                if (return_error > error_t.NOTFOUND) {
                    return_error = SerialPortWriteData("\n", MAX_RESPONSE_TIMEOUT);

                    checkBeltStatus(acmd_mode_t.ACM_LRN);
                    return_error = _belt_error;
                    if (return_error == error_t.EBADCMD)
                        // set to success for expected behavior
                        return_error = error_t.ESUCCESS;
                }

                if (return_error == error_t.ESUCCESS)
                    _glbl_mode = mode_t.M_LEARN;
            }
            _dll_error = return_error;
            return return_error;
        }

        /*
         * This method is used to change the mode of the application
         * as the firmware mode is changed.  The application and the 
         * firmware modes must stay synchronized.
         */
        private error_t change_acmd_mode(acmd_mode_t mode) {
            error_t return_error = error_t.EMAX;

            if (_acmd_mode == mode) {
                _belt_error = error_t.ESUCCESS; // already in requested mode
                return_error = _belt_error;
            }
            else if (mode == acmd_mode_t.ACM_LRN) {
                //return to learning mode
                return_error = change_glbl_mode(mode_t.M_LEARN);
                if (return_error == error_t.ESUCCESS) {
                    _acmd_mode = mode;
                }
            }
            else { //(mode == acmd_mode_t.ACM_VIB || acmd_mode_t.ACM_SPT || acmd_mode_t.ACM_GCL)
                return_error = change_glbl_mode(mode_t.M_ACTIVE);
                if (return_error == error_t.ESUCCESS) {
                    _acmd_mode = mode;
                }
            }
            _dll_error = return_error;
            return return_error;
        }

        /// <summary>
        /// Reset haptic belt and driver to the default states if any erroneous 
        /// states occur in DLL or belt.  This is an attempt to regain control
        /// to a known state.
        /// </summary>
        /// <returns>error code resulting from the reset</returns>
        public error_t ResetHapticBelt() {
            return ResetHapticBelt(false);
        }

        /// <summary>
        /// Reset haptic belt and driver to the default states if any erroneous 
        /// states occur in DLL or belt.  This is an attempt to regain control
        /// to a known state.
        /// </summary>
        /// <param name="resetComPorts">Set if you want to reset the connection</param>
        /// <returns>error code resulting from the reset</returns>
        public error_t ResetHapticBelt(bool resetComPorts) {

            //Reset Com Ports
            if (resetComPorts) {
                _dll_error = ClosePorts();
                System.Threading.Thread.Sleep(1000); //must use Sleep for Windows
                _dll_error = OpenPorts();
            }

            //Reset Belt's mode
            _dll_error = change_acmd_mode(acmd_mode_t.ACM_LRN);

            return _dll_error;
        }

        /// <summary>
        /// Primary function used to vibrate motors - send a bit string command
        /// </summary>
        /// <returns>error code resulting from Vibrat Motor command</returns>
        private error_t Vibrate_Motor(acmd_mode_t cmd_mode, byte motor, byte rhythm, byte magnitude, byte rhythm_cycles) {
            byte[] command_byte = { 0x00, 0x00 };
            error_t return_error = error_t.EMAX;

            // Validate parameters
            if (motor < 0 || rhythm < 0 || rhythm > 8
                || magnitude < 0 || magnitude > 8
                || rhythm_cycles < 0 || rhythm_cycles > 7) {

                return_error = error_t.EARG;
            }
            else {
                byte mode = 0x0;
                //byte motor = 0x0; // this is the first valid motor
                //byte rhythm = 0x7; //rhy H =7
                //byte magnitude = 0x0; //mag A = 0
                //byte rhythm_cycles = 0x6;
                // This equates to hex command < 01 36 >

                try {
                    return_error = change_acmd_mode(cmd_mode);

                    if (return_error == error_t.ESUCCESS) {
                        mode = (byte)cmd_mode;

                        // Send mode first, it is the LSB of the first byte in firmware struct active_command_t
                        command_byte[0] = (byte)((mode << 4) | (motor & 0xf));
                        // Send rhythm first, it is the LSB of the firmware struct vibration_t
                        command_byte[1] = (byte)(((rhythm & 0x7) << 5)
                            | ((magnitude & 0x3) << 3)
                            | (rhythm_cycles & 0x7));

                        return_error = SerialPortWriteData(command_byte, MAX_RESPONSE_TIMEOUT);

                        // BROWN OUT HANDLING - Error from sending msg can return odd 
                        // error code if belt is currently in ACM.LRN.  Reasonable risk that 
                        // this will not occur when main controller is running on battery.
                        // error = 0xFC = 252 is from brown out condition
                        if (return_error > error_t.NOTFOUND) {

                            return_error = ResetHapticBelt();//error_t.ELOWPOWER;

                            // proceed for expected behavior of readtimeout
                            if (return_error == error_t.ESUCCESS)
                                //Try again with recursive call
                                return_error = Vibrate_Motor(cmd_mode, motor, rhythm, magnitude, rhythm_cycles);
                        }
                    }
                }
                catch {
                    return_error = error_t.EXCVIBCMD;
                }
            }
            _dll_error = return_error;
            return return_error;
        }

        /// <summary>
        /// This function is used to vibrate motors - using binary data as inputs
        /// </summary>
        /// <param name="motor">motor to be vibrated (1 to 16)</param>
        /// <param name="rhythm">rhythm to vibrate motor in (0 to 7)</param>
        /// <param name="magnitude">magnitude to vibrate motor in (0 to 3)</param>
        /// <param name="rhythm_cycles">cycles to repeate the rhythm (0 to 7)</param>
        /// <returns>error code resulting from Vibrat Motor command</returns>
        public error_t Vibrate_Motor(byte motor, byte rhythm, byte magnitude, byte rhythm_cycles) {

            /* Wrapper method that uses most common case of ACM_VIB  */
            acmd_mode_t cmd_mode = acmd_mode_t.ACM_VIB;

            error_t response = Vibrate_Motor(cmd_mode, motor, rhythm, magnitude, rhythm_cycles);
            return response;
        }

        /// <summary>
        /// This function is used to vibrate motors - using integers and strings as inputs
        /// </summary>
        /// <param name="motor_number">motor to be vibrated (0 to 15)</param>
        /// <param name="rhythm_string">rhythm to vibrate motor in ("A" to "H")</param>
        /// <param name="magnitude_string">magnitude to vibrate motor in ("A" to "D")</param>
        /// <param name="rhythm_cycles">cycles to repeate the rhythm (0 to 7)</param>
        /// <returns>error code resulting from Vibrat Motor command</returns>
        public error_t Vibrate_Motor(int motor_number, string rhythm_string, string magnitude_string, int rhythm_cycles) {

            byte motor = (byte)(motor_number);
            byte rhythm = VibStrToByte(rhythm_string);
            byte magnitude = VibStrToByte(magnitude_string);
            byte rhythm_length = (byte)(rhythm_cycles);

            error_t response = Vibrate_Motor(motor, rhythm, magnitude, rhythm_length);
            return response;
        }

        /// <summary>
        /// Function is used to STOP vibration of a specific motor on the haptic belt
        /// </summary>
        /// <param name="motor_number"></param>
        /// <returns>error code resulting from Stop Vibrate command</returns>
        public error_t Stop(byte motor_number) {
            error_t return_error = error_t.EMAX;

            if (motor_number < 0) {
                return_error = error_t.ENOMOTOR;
            }
            else {
                // sending rhythm_cycles = 0 stops motor
                return_error = Vibrate_Motor(motor_number, 0x0, 0x0, 0x0);
            }
            _dll_error = return_error;
            return return_error;
        }

        /// <summary>
        /// Function is used to STOP vibration of all motors on the haptic belt
        /// </summary>
        /// <returns>error code resulting from Stop Vibrate command</returns>
        public error_t StopAll() {
            error_t return_error = error_t.EMAX;
            int motorCount = this.getMotors(QueryType.PREVIOUS);

            // Assumes that QRY has already occured in order to activate motors
            if (motorCount != 0) {
                for (byte i = 0; i < motorCount; i++) {
                    // sending rhythm_cycles = 0 stops motor
                    return_error = Vibrate_Motor(i, 0x0, 0x0, 0x0);
                }
            }
            else {
                return_error = _belt_error;
            }
            _dll_error = return_error;
            return return_error;
        }

        /// <summary>
        /// Primary function used to query the belt configuration
        /// </summary>
        /// <param name="typeMsg">Type of query: 
        /// "QRY ALL\r" | "QRY RHY\r" | "QRY MAG\r" | "QRY MTR\r" | "QRY SPT\r" 
        /// | "QRY VER\r"</param>
        /// <param name="timeout">Time allowed to recieve data on incoming 
        /// serial COM port before processing buffer</param>
        /// <returns>error code resulting from Query command</returns>
        private error_t Query(string typeMsg, int timeout) {

            error_t return_error = error_t.EMAX;

            clearBuffer(qry_resp);

            //Send the QRY command if ports are setup
            try {
                return_error = change_acmd_mode(acmd_mode_t.ACM_LRN);

                if (return_error == error_t.ESUCCESS) {
                    // Send command with wait time for belt to respond back.
                    return_error = SerialPortWriteData(typeMsg, timeout);

                    //read responses from Serial_Port and place RSP lines into string array
                    if (return_error == error_t.ESUCCESS) {
                        string[] buffer;

                        //Removes the '\r\n' characters        
                        char[] delimiters = new char[] { '\r', '\n' }; //, ' '
                        //CANNOT USE REGEX in CF 2.0
                        //buffer = Regex.Split(serialIn.MsgInBuffer, "\r\n");
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
                                else if (buffer[i].Substring(0, 3).Equals("DBG")) {
                                    // Not yet implemented
                                    //string[] split = buffer[i].Split(' ');
                                    //sys_error = (error_t)split[1][0]; // get char at index 0
                                }
                            }
                        }
                        rsp_index--; // decrement from last increment to give count of elements
                        qry_resp[0] = rsp_index.ToString(); //count of elements
                    }
                }
            }
            catch (Exception e) {
                qry_resp[0] = "0";
                return_error = error_t.EXCQRYCMD;
            }
            _dll_error = return_error;
            return return_error;
        }

        private error_t QuerySelect(string msg, QueryType query_type) {
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            if (query_type == QueryType.SINGLE)
                return_error = Query(msg, MAX_RESPONSE_TIMEOUT);
            else if (query_type == QueryType.ALL)
                return_error = Query("QRY ALL\r", MAX_RESPONSE_TIMEOUT);
            else // No query - DLL data structure already populated from recent QRY
                return_error = error_t.ESUCCESS;

            _dll_error = return_error;
            return return_error;
        }

        /// <summary>
        /// Function queries all haptic belt configurations
        /// </summary>
        /// <returns>error code resulting from Query command</returns>
        public error_t Query_All() {
            return QuerySelect("QRY ALL\r", QueryType.ALL);
        }

        /// <summary>
        /// Function queries the haptic belt vibrator motor configuration
        /// </summary>
        /// <returns>error code resulting from Query command</returns>
        private error_t Query_Motor() {
            return Query("QRY MTR\r", 50);
        }

        /// <summary>
        /// Function queries the haptic belt temporal-spatial pattern configurations
        /// </summary>
        /// <returns>error code resulting from Query command</returns>
        public error_t Query_SpatioTemporal() {
            return QuerySelect("QRY SPT\r", QueryType.SINGLE);
        }

        /// <summary>
        /// Function queries the haptic belt firmware version
        /// </summary>
        /// <returns>error code resulting from Query command</returns>
        private error_t Query_Version() {
            return Query("QRY VER\r", 50);
        }

        /// <summary>
        /// This function returns the firmware version from the specified QRY operation
        /// </summary>
        public string getVersion(QueryType query_type) {

            string return_value = "";
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            return_error = QuerySelect("QRY VER\r", query_type);

            // Search and process data
            if (return_error == error_t.ESUCCESS) {

                return_error = error_t.NOTFOUND;
                try {
                    for (int index = 1; index < qry_resp.Length; index++) {
                        if (qry_resp[index] != null) {
                            string[] split = qry_resp[index].Split(' ');

                            //put the values from the response into the return string
                            if (split[1].Equals("VER")) {
                                return_value = split[2];
                                return_error = error_t.ESUCCESS;
                                break; // exit loop
                            }
                        }
                    }
                }
                catch {
                    return_error = error_t.EXCDATASEARCH;
                }
            }
            _dll_error = return_error;
            return return_value;
        }

        /// <summary>
        /// This function returns the motor count from the specified QRY operation
        /// </summary>
        public byte getMotors(QueryType query_type) {
            byte return_values = 0;
            error_t return_error = error_t.NOTFOUND;

            // Query configuration data from belt
            return_error = QuerySelect("QRY MTR\r", query_type);

            // Search and process data
            if (return_error == error_t.ESUCCESS) {

                return_error = error_t.NOTFOUND;
                try { //Convert.ToInt16 can cause exception
                    for (int index = 1; index < qry_resp.Length; index++) {
                        if (qry_resp[index] != null) {
                            string[] split = qry_resp[index].Split(' ');

                            //put the values from the response into the return array
                            if (split[1].Equals("MTR")) {
                                char motor_count = split[2][0]; // gets char at string[0]
                                return_values = HexToByte(motor_count); // count of motors
                                return_error = error_t.ESUCCESS;
                                break; // exit loop
                            }
                        }
                    }
                }
                catch {
                    return_error = error_t.EXCDATASEARCH;
                }
            }
            _dll_error = return_error;
            return return_values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>error code resulting from Erase All command</returns>
        public error_t Erase_All() {
            error_t return_error = error_t.EMAX;

            try {
                return_error = change_acmd_mode(acmd_mode_t.ACM_LRN);

                if (return_error == error_t.ESUCCESS) {
                    // Send command with wait time for belt to respond back.
                    return_error = SerialPortWriteData("ZAP 1 2 3\n", MAX_RESPONSE_TIMEOUT); // requires 3 arguments
                }
            }
            catch {
                qry_resp[0] = "0";
                return_error = error_t.EXCZAPCMD;
            }
            _dll_error = return_error;
            return return_error;
        }
    }
}