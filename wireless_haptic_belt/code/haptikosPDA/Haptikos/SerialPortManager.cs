using System;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PCCom.SerialCommunication Version 1.0.0.0
//   Class file for managing serial port communication
//
//   Copyright (C) 2007  
//   Richard L. McCutchen 
//   Email: richard@psychocoder.net
//   Created: 20OCT07
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************
/*
 * SerialCommManager.cs Version 2.0.0.0, Jul 15, 2009
 * Modified Class file from above to be more portable.
 * 
 * Nathan J. Edwards (nathan.edwards@asu.edu)
 */
//*****************************************************************************************
namespace HapticDriver
{
    // "internal" protection so that elements are accesible only through HapticDriver
    // class.  HapticDriver functions as interface for each Serial Port.  
    // XXXXXXXSerialPortManager class must be public to allow for some pass-through access.
    internal class SerialPortManager
    {
        #region Manager Enums
        /// <summary>
        /// enumeration to hold our transmission types
        /// </summary>
        internal enum TransmissionType { Text, Hex }

        /// <summary>
        /// enumeration to hold our message types
        /// </summary>
        internal enum MessageType { Incoming, Outgoing, Normal, Warning, Error };
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
        private bool _append_msg = false;
        internal bool _data_ready = false;
        private bool _echoBack = false;

        private TransmissionType _transType;

        // Message passing buffer. 
        // _stringBuffer[0] is MessageType
        // _stringBuffer[1] is the complete message
        private String[] _msgInBuffer = new String[2];
        private String[] _statusBuffer = new String[2];

        //global manager variables
        private SerialPort comPort = new SerialPort();
        #endregion

        #region Manager Constructors
        /// <summary>
        /// Constructor to set the properties of our Manager Class
        /// </summary>
        /// <param name="baud">Desired BaudRate</param>
        /// <param name="par">Desired Parity</param>
        /// <param name="sBits">Desired StopBits</param>
        /// <param name="dBits">Desired DataBits</param>
        /// <param name="name">Desired PortName</param>
        internal SerialPortManager(string portName, string baud, string dBits, string sBits, string par, string timeout, String[] strbuf) {
            _baudRate = baud;
            _parity = par;
            _stopBits = sBits;
            _dataBits = dBits;
            _portName = portName;
            _readTimeout = timeout;
            _msgInBuffer = strbuf;
            //now add an event handler
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }

        /// <summary>
        /// Constructor without string buffer specified
        /// </summary>
        internal SerialPortManager(string portName, string baud, string dBits, string sBits, string par, string timeout) {
            _baudRate = baud;
            _parity = par;
            _stopBits = sBits;
            _dataBits = dBits;
            _portName = portName;
            _readTimeout = timeout;
            _msgInBuffer[0] = string.Empty;
            _msgInBuffer[1] = string.Empty;
            //now add an event handler
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }

        /// <summary>
        /// Comstructor to set the properties of our
        /// serial port communicator to nothing
        /// </summary>
        internal SerialPortManager() //DataRecievedHandler handler)
        {
            _baudRate = "9600"; //string.Empty;
            _parity = "None";  //string.Empty;
            _stopBits = "1"; // string.Empty;
            _dataBits = "8"; // string.Empty;
            _portName = "COM1";
            _readTimeout = "1000";
            _msgInBuffer[0] = string.Empty;
            _msgInBuffer[1] = string.Empty;
            //this.DataRecievedFxn = handler;

            //add event handler
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }
        #endregion

        #region Manager Properties
        internal bool IsOpen {
            get { return _portOpened; }
        }

        /// <summary>
        /// Property to hold the BaudRate
        /// of our manager class
        /// </summary>
        internal string BaudRate {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        /// <summary>
        /// property to hold the Parity
        /// of our manager class
        /// </summary>
        internal string Parity {
            get { return _parity; }
            set { _parity = value; }
        }

        /// <summary>
        /// property to hold the StopBits
        /// of our manager class
        /// </summary>
        internal string StopBits {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        /// <summary>
        /// property to hold the DataBits
        /// of our manager class
        /// </summary>
        internal string DataBits {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        /// <summary>
        /// property to hold the PortName
        /// of our manager class
        /// </summary>
        internal string PortName {
            get { return _portName; }
            set { _portName = value; }
        }

        /// <summary>
        /// property to hold the PortName
        /// of our manager class
        /// </summary>
        internal string ReadTimeout {
            get { return _readTimeout; }
            set { _readTimeout = value; }
        }

        /// <summary>
        /// property to hold our TransmissionType
        /// of our manager class
        /// </summary>
        internal TransmissionType CurrentTransmissionType {
            get { return _transType; }
            set { _transType = value; }
        }

        /// <summary>
        /// property to hold our Terminal Settings Echo Back
        /// of our manager class
        /// </summary>
        internal bool EchoBack {
            get { return _echoBack; }
            set { _echoBack = value; }
        }

        /// <summary>
        /// property to hold our string buffer
        /// value
        /// </summary>
        internal String MsgInBuffer {
            get { return _msgInBuffer[1]; }
            set { _msgInBuffer[1] = value; }
        }

        /// <summary>
        /// property to hold the type of string passed
        /// value
        /// </summary>
        internal String MsgInBufferType {
            get { return _msgInBuffer[0]; }
            set { _msgInBuffer[0] = value; }
        }
        /// <summary>
        /// property to hold our string buffer
        /// value
        /// </summary>
        internal String StatusBuffer {
            get { return _statusBuffer[1]; }
            set { _statusBuffer[1] = value; }
        }

        /// <summary>
        /// property to hold the type of string passed
        /// value
        /// </summary>
        internal String StatusBufferType {
            get { return _statusBuffer[0]; }
            set { _statusBuffer[0] = value; }
        }
        #endregion


        #region WriteData
        internal void WriteData(string msg) {
            switch (CurrentTransmissionType) {
                case TransmissionType.Text:
                    //first make sure the port is open
                    //if its not open then open it
                    if (!(comPort.IsOpen == true)) comPort.Open();
                    //send the message to the port
                    comPort.Write(msg);
                    //display the message
                    ReturnData(MessageType.Outgoing, msg);
                    break;
                case TransmissionType.Hex:
                    try {
                        //convert the message to byte array
                        byte[] newMsg = HexToByte(msg);
                        //first make sure the port is open
                        //if its not open then open it
                        if (!(comPort.IsOpen == true)) comPort.Open();
                        //send the message to the port
                        comPort.Write(newMsg, 0, newMsg.Length);
                        //convert back to hex and display
                        ReturnData(MessageType.Outgoing, ByteToHex(newMsg));
                    }
                    catch (FormatException ex) {
                        //display error message
                        ReturnData(MessageType.Error, ex.Message);
                    }
                    break;
                default:
                    //first make sure the port is open
                    //if its not open then open it
                    if (!(comPort.IsOpen == true)) comPort.Open();
                    //send the message to the port
                    comPort.Write(msg);
                    //display the message
                    ReturnData(MessageType.Outgoing, msg);
                    break;
            }
        }
        #endregion

        #region HexToByte
        /// <summary>
        /// method to convert hex string into a byte array
        /// </summary>
        /// <param name="msg">string to convert</param>
        /// <returns>a byte array</returns>
        private byte[] HexToByte(string msg) {
            //remove any spaces from the string
            msg = msg.Replace(" ", "");
            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            byte[] comBuffer = new byte[msg.Length / 2];
            //loop through the length of the provided string
            for (int i = 0; i < msg.Length; i += 2)
                //convert each set of 2 characters to a byte
                //and add to the array
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

        #region ReturnData
        /// <summary>
        /// method to display the data to & from the port
        /// on the screen
        /// </summary>
        /// <param name="type">MessageType of the message</param>
        /// <param name="msg">Message to display</param>
        //[STAThread]
        private void ReturnData(MessageType type, string msg) {
            if (type == MessageType.Incoming) {
                if (!_append_msg) {  // TODO empty garbage ReturnData
                    _msgInBuffer[0] = string.Empty;
                    _msgInBuffer[1] = string.Empty;
                }
                _msgInBuffer[0] = type.ToString();
                _msgInBuffer[1] += msg;
            }
            else {
                if (!_append_msg) {  // TODO empty garbage ReturnData
                    _statusBuffer[0] = string.Empty;
                    _statusBuffer[1] = string.Empty;
                }
                _statusBuffer[0] = type.ToString();
                _statusBuffer[1] += msg;
            }

            //}));

            //// Execute the parent class's data recieved function pointer
            //if (DataReceivedFxn != null)
            //{
            //    DataReceivedFxn();
            //}
        }
        #endregion

        #region OpenPort
        internal bool OpenPort() {
            try {
                //first check if the port is already open
                //if its open then close it
                if (comPort.IsOpen == true) comPort.Close();

                //set the properties of our SerialPort Object
                comPort.BaudRate = int.Parse(_baudRate);    //BaudRate
                comPort.DataBits = int.Parse(_dataBits);    //DataBits
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits, true);    //StopBits
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), _parity, true);    //Parity
                comPort.PortName = _portName;   //PortName
                //comPort.ReadTimeout = int.Parse(_readTimeout); ;
                //now open the port
                comPort.Open();
                _portOpened = true;

                //display message
                ReturnData(MessageType.Normal, "Port opened at " + DateTime.Now + "\n");
                //return true
                return true;
            }
            catch (Exception ex) {
                ReturnData(MessageType.Error, ex.Message);
                return false;
            }
        }
        #endregion

        #region ClosePort
        internal bool ClosePort() {
            try {
                //first check if the port is already open
                //if its open then close it
                if (comPort.IsOpen == true) {
                    comPort.Close(); //TODO cannot close if serial port thread is sleeping or waiting.
                    _portOpened = false;
                    //display message
                    ReturnData(MessageType.Normal, "Port closed at " + DateTime.Now + "\n");
                }
                else
                    ReturnData(MessageType.Warning, "Port already closed, " + DateTime.Now + "\n");

                // Help with garbage
                this.DataReceivedFxn = null;
                //return true
                return true;
            }
            catch (Exception ex) {
                ReturnData(MessageType.Error, ex.Message);
                return false;
            }
        }
        #endregion

        #region comPort_DataReceived
        /// <summary>
        /// method that will be called when theres data waiting in the buffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            char[] charBuf;
            string msg = "";
            int bytes = 0;
            byte[] comBuffer;

            //if (!_append_msg) {  // empty garbage TODO
            //    _stringBuffer[0] = string.Empty;
            //    _stringBuffer[1] = string.Empty;
            //}

            // Wait for other data to arrive in buffer before read
            // Default baud rate is 9600 = 1 bit per 0.105 miliseconds
            // or slightly less than 10 bits per milisecond.  If we allow
            // for some small delays and use approximation of 1 ms = 8 bits
            // or 1 ASCII character, then 250ms allows for slightly more
            // than 250 new ASCII characters to arrive in buffer.  Current
            // HapticBelt firmware has 238 ASCII characters for Max Data 
            // Transmission in QueryAll()
            System.Threading.Thread.Sleep(400);

            //determine the mode the user selected (binary/string)
            switch (CurrentTransmissionType) {
                //user chose string
                case TransmissionType.Text:
                    //retrieve number of bytes in the buffer
                    bytes = comPort.BytesToRead;
                    //create a char array to hold the awaiting data
                    charBuf = new char[bytes]; // set to 80 for std terminal size

                    //read data waiting in the buffer
                    comPort.Read(charBuf, 0, bytes); // reads avail data in buffer

                    // set flag to appends msg if there is still data in receive buffer
                    // keep it interrupt driven rather than a busy_wait loop.
                    if (comPort.BytesToRead > 0)
                        _append_msg = true;

                    //display the data to the user
                    msg = new string(charBuf); // Create new string passing charBuf into the constructor
                    ReturnData(MessageType.Incoming, msg);

                    //if (this._echoBack == true) this.WriteData(msg + "\r\n");
                    break;

                //user chose binary
                case TransmissionType.Hex:
                    //retrieve number of bytes in the buffer
                    bytes = comPort.BytesToRead;
                    //create a byte array to hold the awaiting data
                    comBuffer = new byte[bytes];
                    //read the data and store it
                    comPort.Read(comBuffer, 0, bytes);

                    // set flag to appends msg if there is still data in receive buffer
                    // keep it interrupt driven rather than a busy_wait loop.
                    if (comPort.BytesToRead > 0)
                        _append_msg = true;

                    //display the data to the user
                    ReturnData(MessageType.Incoming, ByteToHex(comBuffer));

                    //if (this._echoBack == true) this.WriteData(ByteToHex(comBuffer)+"\r\n");
                    break;

                default:
                    string str = comPort.ReadExisting(); // Reads avail data in output stream & input buffer

                    //display the data to the user
                    ReturnData(MessageType.Incoming, str);

                    //if (this._echoBack == true) this.WriteData(str + "\r\n");
                    break;
            }
            // Finish routine if there is no data remaining in the input buffer.
            if (comPort.BytesToRead == 0) {
                _append_msg = false;
                _data_ready = true;

                // Execute the parent class's data recieved function pointer
                if (DataReceivedFxn != null) {
                    DataReceivedFxn();
                }
            }
        }
        #endregion
    }
}
