/*****************************************************************************
 * FILE:   SerialPortManager.cs
 * AUTHOR: Nathan J. Edwards (nathan.edwards@asu.edu)
 *         
 * DESCR:  Contains serial port communication functions, buffers, and message
 *         retrieval.  Created to be more portable than regular .NET 
 *         applications where the managed Serial Port code is contained
 *         within this file and can easily be changed for a different
 *         programming language or target OS.
 * LOG:    20090708 - initial version
 *         20091110 - Changed from event driven design to functional
 *                    single thread design (more portable and less memory
 *                    intensive).
 *         20091201 - Added our own implementation of a blocking wait,
 *                    System.Threading.Thread.Sleep() is not desirable.
 ****************************************************************************/

using System;
using System.IO.Ports;


namespace HapticDriver
{
    // "internal" protection so that elements are accesible only through HapticDriver
    // class.  HapticDriver functions as interface for each Serial Port.  
    internal class SerialPortManager
    {
        #region Manager Enums
        /// <summary>
        /// enumeration to hold our transmission types
        /// </summary>
        internal enum DataType { Text, Hex }

        #endregion

        #region Manager Variables
        // parent class's data recieved function pointer
        internal delegate void DataRecievedHandler();
        internal event DataRecievedHandler DataReceivedFxn;

        //property variables
        private bool _portOpened = false;
        private string _baudRate = string.Empty;
        private string _parity = string.Empty;
        private string _stopBits = string.Empty;
        private string _dataBits = string.Empty;
        private string _portName = string.Empty;
        private int _readTimeout; //default

        internal bool EchoBack = false;

        // Message passing items. 
        private bool _append_msg = false;
        private MutexLock serialMutex;
        private MutexLock dataBufferMutex;
        private MutexLock statusBufferMutex;
        private Buffer _dataRecvBuffer;
        private Buffer _statusBuffer;

        //global manager variables
        private SerialPort comPort;
        #endregion

        #region Manager Constructors
        /// <summary>
        /// Constructor to set the properties of the SerialPortManager Class
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baud"></param>
        /// <param name="dBits"></param>
        /// <param name="sBits"></param>
        /// <param name="par"></param>
        /// <param name="timeout"></param>
        /// <param name="databuf"></param>
        internal SerialPortManager(string portName, string baud, string dBits, string sBits, string par, int timeout, Buffer databuf) {
            comPort = new SerialPort();
            serialMutex = new MutexLock();
            dataBufferMutex = new MutexLock();
            statusBufferMutex = new MutexLock();
            _baudRate = baud;
            _parity = par;
            _stopBits = sBits;
            _dataBits = dBits;
            _portName = portName;
            _readTimeout = timeout;
            _dataRecvBuffer = databuf;
            _statusBuffer = new Buffer(statusBufferMutex);

        }

        /// <summary>
        /// Constructor without string buffer specified
        /// </summary>
        internal SerialPortManager(string portName, string baud, string dBits, string sBits, string par, int timeout) {
            comPort = new SerialPort();
            serialMutex = new MutexLock();
            dataBufferMutex = new MutexLock();
            statusBufferMutex = new MutexLock();
            _baudRate = baud;
            _parity = par;
            _stopBits = sBits;
            _dataBits = dBits;
            _portName = portName;
            _readTimeout = timeout;
            _dataRecvBuffer = new Buffer(dataBufferMutex);
            _statusBuffer = new Buffer(statusBufferMutex);
        }

        /// <summary>
        /// Comstructor to set the properties of our
        /// serial port communicator to nothing
        /// </summary>
        internal SerialPortManager() {  //DataRecievedHandler handler)
            comPort = new SerialPort();
            serialMutex = new MutexLock();
            dataBufferMutex = new MutexLock();
            statusBufferMutex = new MutexLock();
            _baudRate = "9600"; //string.Empty;
            _parity = "None";  //string.Empty;
            _stopBits = "1"; // string.Empty;
            _dataBits = "8"; // string.Empty;
            _portName = "COM1";
            _readTimeout = 2056;
            _dataRecvBuffer = new Buffer(dataBufferMutex);
            _statusBuffer = new Buffer(statusBufferMutex);
            //this.DataRecievedFxn = handler;
        }
        #endregion

        #region Manager Property Accessors
        internal bool IsOpen() {
            return _portOpened;
        }

        /// <summary>
        /// method that returns data received buffer byte array
        /// </summary>
        internal byte[] DataRecvBuffer() {
            return _dataRecvBuffer.GetBuffer();
        }

        /// <summary>
        /// method that returns data received buffer type
        /// </summary>
        internal byte DataRecvBufferType() {
            return _dataRecvBuffer.GetBufferType();
        }
        /// <summary>
        /// method that returns status buffer byte array
        /// </summary>
        internal byte[] StatusBuffer() {
            return _statusBuffer.GetBuffer();
        }

        /// <summary>
        /// method that returns status buffer type
        /// </summary>
        internal byte StatusBufferType() {
            return _statusBuffer.GetBufferType();
        }
        #endregion

        #region WriteData
        internal error_t WriteData(string msg) {
            error_t error = error_t.COMPRTWRITE;

            //*** _transType == TransmissionType.Text ***//
            try {
                //first make sure the port is open, if its not open then open it
                if (comPort.IsOpen == false) {
                    comPort.Open();
                    ReturnData(MessageType.WARNING, (byte)error_t.COMPRTNOTOPEN);
                }

                //send the message to the port
                comPort.Write(msg);

                error = error_t.ESUCCESS;
                ReturnData(MessageType.OUTGOING, (byte)error); // overhead to convert message

            }
            // Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch {
                error = error_t.EXCCOMPRTWRITE;
                ReturnData(MessageType.ERROR, (byte)error);
            }
            return error;
        }
        internal error_t WriteData(byte[] msg) {
            error_t error = error_t.COMPRTWRITE;

            //*** _transType == TransmissionType.Hex ***//

            try {
                //first make sure the port is open
                //if its not open then open it
                if (comPort.IsOpen == false) {
                    comPort.Open();
                    ReturnData(MessageType.WARNING, (byte)error_t.COMPRTNOTOPEN);
                }

                //send the message to the port  
                comPort.Write(msg, 0, msg.Length);

                //record message
                error = error_t.ESUCCESS;
                ReturnData(MessageType.OUTGOING, (byte)error);

            }
            //// Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch {
                error = error_t.EXCCOMPRTWRITE;
                ReturnData(MessageType.ERROR, (byte)error);
            }
            return error;
        }
        #endregion

        #region OpenPort
        internal error_t OpenPort() {
            error_t error = error_t.COMPRTNOTOPEN;
            try {
                //first check if the port is already open, if its open then close it
                if (comPort.IsOpen == true) {
                    comPort.Close();
                    comPort.Dispose();
                }
                // New instantiation of COM port
                comPort = new SerialPort();

                //set the properties of our SerialPort Object
                comPort.BaudRate = int.Parse(_baudRate);
                comPort.DataBits = int.Parse(_dataBits);
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits, true);
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), _parity, true);
                comPort.PortName = _portName;

                //now open the port
                comPort.Open();
                _portOpened = true;
                error = error_t.ESUCCESS;

                //display message
                ReturnData(MessageType.NORMAL, (byte)error);

                // Add slight timing delay before other functions can use serial port.
                //System.Threading.Thread.Sleep(50);
                HapticBelt.Wait(500);
            }
            // Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch (Exception e){
                error = error_t.EXCCOMPRTOPEN;
                ReturnData(MessageType.ERROR, (byte)error);
            }
            return error;
        }
        #endregion

        #region ClosePort
        internal error_t ClosePort() {
            error_t error = error_t.COMPRTOPEN;

            try {
                //first check if the port is already open, if its open then close it
                if (comPort.IsOpen == true) {
                    comPort.Close();
                    comPort.Dispose();
                    //comPort.DiscardInBuffer();
                    //comPort.DiscardOutBuffer();
                    
                    _portOpened = false; //false means success in operation
                    error = error_t.ESUCCESS;

                    //display message
                    ReturnData(MessageType.NORMAL, (byte)error);
                }
                else
                    error = error_t.COMPRTCLSPREV;
                ReturnData(MessageType.WARNING, (byte)error);

                // Help with garbage
                this.DataReceivedFxn = null;
                //System.Threading.Thread.Sleep(50);
                HapticBelt.Wait(500);
            }
            // Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch {
                error = error_t.EXCCOMPRTCLS;
                ReturnData(MessageType.ERROR, (byte)error);
            }
            return error;
        }
        #endregion

        #region ReturnData
        /// <summary>
        /// method to store data. Parent class must convert to ASCII if desired.
        /// 
        /// </summary>
        /// <param name="type">MessageType of the message</param>
        /// <param name="msg">Message to display</param>
        //[STAThread]
        private void ReturnData(MessageType type, byte[] msg) {
            if (type == MessageType.INCOMING) {
                if (_append_msg)
                    _dataRecvBuffer.AppendBuffer((byte)type, msg);
                else // empty garbage ReturnData 
                    _dataRecvBuffer.SetBuffer((byte)type, msg);
            }
            else {
                //store data and let parent class convert to ASCII if desired.
                if (_append_msg)
                    _statusBuffer.AppendBuffer((byte)type, msg);
                else // empty garbage ReturnData
                    _statusBuffer.SetBuffer((byte)type, msg);
            }
        }
        // Wrapper method to handle a single byte message
        private void ReturnData(MessageType type, byte msgByte) {
            byte[] msg = { msgByte };
            ReturnData(type, msg);
        }
        #endregion

        #region ReceiveData

        /// <summary>
        /// method that will be called when there is data waiting in the buffer
        /// </summary>
        /// <param name="type">The expected data type to be read from the serial port</param>
        /// <param name="timeout">Time allowed to recieve data on incoming 
        /// serial COM port before processing buffer</param>
        internal error_t ReceiveData(DataType type, int timeout) {
            byte[] dataBuffer;
            string strBuffer = "";
            int byte_count;
            error_t error = error_t.COMPRTREAD;

            // Mutual exclusion is used in this method so that it does not get 
            // interrupted by another thread or Event.
            // Uses serialMutex.GetLock() and serialMutex.Unlock();

            // Timer for read timeout
            DateTime startTime = DateTime.Now;
            DateTime currentTime = DateTime.Now;
            TimeSpan duration = startTime - startTime;

            //set current COM port timeout
            comPort.ReadTimeout = timeout;

            //Loop
            bool _continue = true;
            try {
                while (_continue) {
                    if (type == DataType.Text) {
                        // NOTE: ReadLine() does not read hex digits as input.
                        // Also while this method does not return the NewLine value, 
                        // the NewLine value is removed from the input buffer. By default, 
                        // the ReadLine method will block until a line is received.
                        // Method also results in "\r" inserted at the end.

                        //serialMutex.GetLock(); Buffer is not currently shared
                        string line = comPort.ReadLine();
                        //serialMutex.Unlock();

                        // add to buffer while replacing the stripped NewLine character
                        strBuffer += line + "\n";

                        // check for STS response line (means end of transmission)
                        if (line.Substring(0, 3) == "STS") {
                            _continue = false;
                            error = error_t.ESUCCESS;
                        }
                    }
                    else { //type = DataType.Hex
                        //serialMutex.GetLock();  Buffer is not currently shared
                        if (comPort.BytesToRead > 0) {
                            byte_count = comPort.BytesToRead;

                            //create a byte array to hold the awaiting data
                            dataBuffer = new byte[byte_count];

                            //read the data and store it
                            comPort.Read(dataBuffer, 0, byte_count);

                            // check for more data
                            if (comPort.BytesToRead > 0)
                                _append_msg = true;
                            else {
                                _append_msg = false;
                                _continue = false; // exit
                            }
                            //store the data in a buffer available to the Driver
                            ReturnData(MessageType.INCOMING, dataBuffer);
                        }
                        //serialMutex.Unlock(); // release mutex
                        error = error_t.ESUCCESS;
                    }
                    // Update timer and compare (need to disable during step debug)
                    currentTime = DateTime.Now;
                    duration = currentTime - startTime;
                    if (duration.Milliseconds > timeout) {
                        error = error_t.COMPRTREADTIME;
                        _continue = false;
                    }
                } // END OF LOOP
            }
            catch (TimeoutException) {
                error = error_t.EXCCOMPRTREAD;//COMPRTREADTIME;
                //serialMutex.Unlock(); Buffer is not currently shared
            }
            //store the text data in a buffer available to the Driver
            if (type == DataType.Text) {
                dataBuffer = HapticBelt.AsciiToByte(strBuffer);
                serialMutex.GetLock();
                ReturnData(MessageType.INCOMING, dataBuffer);
                serialMutex.Unlock();
            }

            // Execute the parent class's data recieved function pointer
            if (DataReceivedFxn != null) {
                DataReceivedFxn();
            }
            return error;
        }
        #endregion
    }
}
