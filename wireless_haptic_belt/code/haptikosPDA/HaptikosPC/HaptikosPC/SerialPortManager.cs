using System;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
//*****************************************************************************************
/*
 * SerialCommManager.cs Nov 10, 2009
 * Created to be more portable than regular .NET applications.
 * 
 * Nathan J. Edwards (nathan.edwards@asu.edu)
 */
//*****************************************************************************************
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
        private string _readTimeout = string.Empty;

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
        internal SerialPortManager(string portName, string baud, string dBits, string sBits, string par, string timeout, Buffer databuf) {
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
        internal SerialPortManager(string portName, string baud, string dBits, string sBits, string par, string timeout) {
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
            _readTimeout = "1000";
            _dataRecvBuffer = new Buffer(dataBufferMutex);
            _statusBuffer = new Buffer(statusBufferMutex);
            //this.DataRecievedFxn = handler;
        }
        #endregion

        #region Manager Properties
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
        internal void WriteData(string msg, int responseTimeout) {
            //*** _transType == TransmissionType.Text ***//
            try {
                //first make sure the port is open, if its not open then open it
                if (comPort.IsOpen == false) {
                    comPort.Open();
                    ReturnData(MessageType.WARNING, (byte)error_t.COMPRTNOTOPEN);
                }

                //send the message to the port
                comPort.Write(msg);
                ReturnData(MessageType.OUTGOING, (byte)error_t.ESUCCESS);

                //get response from belt
                comPort_DataReceived(responseTimeout);
            }
            // Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch {
                ReturnData(MessageType.ERROR, (byte)error_t.EXCCOMPRTWRITE);
            }
        }
        internal void WriteData(byte[] msg, int responseTimeout) {
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
                ReturnData(MessageType.OUTGOING, msg);

                //get response from belt
                comPort_DataReceived(responseTimeout);
            }
            // Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch {
                ReturnData(MessageType.ERROR, (byte)error_t.EXCCOMPRTWRITE);
            }
        }
        #endregion

        #region OpenPort
        internal bool OpenPort() {
            try {
                //first check if the port is already open, if its open then close it
                if (comPort.IsOpen == true) comPort.Close();

                //set the properties of our SerialPort Object
                comPort.BaudRate = int.Parse(_baudRate);
                comPort.DataBits = int.Parse(_dataBits);
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits, true);
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), _parity, true);
                comPort.PortName = _portName;

                //now open the port
                comPort.Open();
                _portOpened = true;

                //display message
                ReturnData(MessageType.NORMAL, (byte)error_t.COMPRTOPEN);

                // Add slight timing delay before other functions can use serial port.
                System.Threading.Thread.Sleep(50); 
            }
            // Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch {
                ReturnData(MessageType.ERROR, (byte)error_t.EXCCOMPRTOPEN);
            }
            return _portOpened;
        }
        #endregion

        #region ClosePort
        internal bool ClosePort() {
            try {
                //first check if the port is already open, if its open then close it
                if (comPort.IsOpen == true) {
                    comPort.Close();
                    _portOpened = false;

                    //display message
                    ReturnData(MessageType.NORMAL, (byte)error_t.COMPRTCLS);
                }
                else
                    ReturnData(MessageType.WARNING, (byte)error_t.COMPRTCLSPREV);

                // Help with garbage
                this.DataReceivedFxn = null;
            }
            // Saved for debug purposes
            //catch (FormatException ex) {
            //    ReturnData(MessageType.ERROR, Encoding.ASCII.GetBytes(ex.Message));
            //}
            catch {
                ReturnData(MessageType.ERROR, (byte)error_t.EXCCOMPRTCLS);
            }
            return !_portOpened; // portOpened == false means success in operation
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

        #region comPort_DataReceived

        /// <summary>
        /// method that will be called when there is data waiting in the buffer
        /// </summary>
        /// <param name="timeout">Time allowed to recieve data on incoming 
        /// serial COM port before processing buffer</param>
        internal void comPort_DataReceived(int timeout) {
            byte[] comBuffer;
            int byte_count;

            // Wait for other data to arrive in buffer before read
            // Default baud rate is 9600 = 1 bit per 0.105 miliseconds
            // or slightly less than 10 bits per milisecond.  If we allow
            // for some small delays and use approximation of 1 ms = 8 bits
            // or 1 ASCII character, then 250ms allows for slightly more
            // than 250 new ASCII characters to arrive in buffer.  Current
            // HapticBelt firmware has 238 ASCII characters for Max Data 
            // Transmission in QueryAll()
            //System.Threading.Thread.Sleep(400);
            System.Threading.Thread.Sleep(timeout);

            // Mutual exclusion on this method so that it does not get interrupted
            // by another thread or Event
            serialMutex.GetLock();

            //retrieve number of bytes in the buffer
            byte_count = comPort.BytesToRead;

            if (byte_count > 0) {
                //create a byte array to hold the awaiting data
                comBuffer = new byte[byte_count];
                //read the data and store it
                comPort.Read(comBuffer, 0, byte_count);

                //store the data in a buffer available to the Driver
                ReturnData(MessageType.INCOMING, comBuffer);//ByteToHex(comBuffer));
            }
            serialMutex.Unlock(); // release mutex

            // set flag to appends msg if there is still data in receive buffer
            // keep it interrupt driven rather than a busy_wait loop.
            if (comPort.BytesToRead > 0) {
                _append_msg = true;
                comPort_DataReceived(10); // Recursive call
            }

            //if (this._echoBack == true) this.WriteData(ByteToHex(comBuffer)+"\r\n");

            // Finish routine if there is no data remaining in the input buffer.
            if (comPort.BytesToRead == 0) {
                _append_msg = false;

                // Execute the parent class's data recieved function pointer
                if (DataReceivedFxn != null) {
                    DataReceivedFxn();
                }
            }
        }
        #endregion
    }
}
