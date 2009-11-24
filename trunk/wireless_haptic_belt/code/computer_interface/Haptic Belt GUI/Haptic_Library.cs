//Haptic Belt Accessor Functions for Sending belt commands from C# GUI interface
//Author: Daniel Moberly
//Date: April 20th, 2009
//
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

//Belt Command: 
//    LEARN RHYTHM <id> <rhythm> <lenth>
//Belt Returns:
//    a response in the form
//        STATUS <error num> [<info>]

//Belt Command: 
//    LEARN MAGNITUDE <period> <duty cycle>
//Belt Returns:
//    a response in the form
//        STATUS <error num> [<info>]

//Belt Command: 
//    START
//Belt Returns:
//    a response in the form
//        STATUS <error num> [<info>]

//Public functions in this program:

//    string[] Query_Rhythm(string id)
//        Returns: 
//            string array of length 2
//                where 
//                    string[0] = error response
//                    string[1] = rhythm (if no error)
//        Parameters:
//            string id = ID of rhythm to be queried ("A" through "H")

//    string[] Query_Magnitude(string id)
//        Returns: 
//            string array of length 3
//                where 
//                    string[0] = error response
//                    string[1] = period, duty cycle
//        Parameters:
//            string id = ID of rhythm to be queried ("A" through "H")

//    string[] Query_Motors()
//        Returns: 
//            string array of length 3
//                where 
//                    string[0] = error response
//                    string[1] = motor1, motor2, motor3, .... motorN
//        Parameters:
//            NONE

//    string[] Learn_Rhythm(string id, string pattern_string)
//        Returns:
//            string array of length 2
//                where 
//                    string[0] = error response
//                    string[1] = command sent
//        Parameters: 
//            string id = ID of rhythm to be learned ("A" through "H")
//            string pattern = rhythm to be learned (64 bit binary string)

//    string[] Learn_Magnitude(string period, string duty_cycle)
//        Returns:
//            string array of length 2
//                where 
//                    string[0] = error response
//                    string[1] = command sent
//        Parameters: 
//            string period = period of magnitude to be learned
//            string duty_cycle = duty_cycle of magnitude to be learned

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

//    string[] Start()
//        Returns: 
//            string array of length 2
//                where 
//                    string[0] = error response
//                    string[1] = NULL
//        Parameters: 
//            NONE

//    string[] Stop()
//        Returns: 
//            string array of length 2
//                where 
//                    string[0] = error response
//                    string[1] = NULL
//        Parameters: 
//            NONE

//    string[] Initialize_Serial_Port(string portno, string baud_string, string parity_string, string stopbits_string, string databits_string)
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
//    private int Check_Belt()
//        Returns:
//            integer which is the error number (0 if success)
//        Parameters:
//            NONE
//    private string Hex_To_Binary(char c)
//        Returns:
//            binary string in form "xxxx"
//        Parameters:
//            hex character (0 to F)

using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

//invalid or no belt response to query = -1, this is returned in the starting point of the array at [belt location,0]
//invalid rhythm ID = -3
//invalid pattern length = -4;
//pattern is something other than binary string = -5

namespace Haptic_Belt_Library
{
    public class Library
    {
        //Responses:
        //VER x 1, MAG x 4, RHY x 8, MTR x 1 = 14
        private static int TOTAL_RESPONSE_COUNT = 14;
        //serial port variable used for Serial_Port communications
        private SerialPort Serial_Port = new SerialPort();

        //this function is used to vibrate motors - send a bit string command FIXME
        public string[] Vibrate_Motor(string motor_number, string rhythm_string, string magnitude_string, string rhythm_cycles)
        {
            string[] return_values = new string[2];
            byte[] send_values = new byte[2];
            return_values[1] = "";

            if (Serial_Port.IsOpen)
            {
                byte mode = 0;
                byte motor = 0;
                byte rhythm = 0;
                byte magnitude = 0;
                byte rhythm_length = 0;
                switch (motor_number)
                {
                    case "1":
                        motor = 0;
                        break;
                    case "2":
                        motor = 1;
                        break;
                    case "3":
                        motor = 2;
                        break;
                    case "4":
                        motor = 3;
                        break;
                    case "5":
                        motor = 4;
                        break;
                    case "6":
                        motor = 5;
                        break;
                    case "7":
                        motor = 6;
                        break;
                    case "8":
                        motor = 7;
                        break;
                    case "9":
                        motor = 8;
                        break;
                    case "10":
                        motor = 9;
                        break;
                    case "11":
                        motor = 10;
                        break;
                    case "12":
                        motor = 11;
                        break;
                    case "13":
                        motor = 12;
                        break;
                    case "14":
                        motor = 13;
                        break;
                    case "15":
                        motor = 14;
                        break;
                    case "16":
                        motor = 15;
                        break;
                    default:
                        motor = 0;
                        break;
                }
                switch (rhythm_string)
                {
                    case "A":
                        rhythm = 0;
                        break;
                    case "B":
                        rhythm = 1;
                        break;
                    case "C":
                        rhythm = 2;
                        break;
                    case "D":
                        rhythm = 3;
                        break;
                    case "E":
                        rhythm = 4;
                        break;
                    case "F":
                        rhythm = 5;
                        break;
                    case "G":
                        rhythm = 6;
                        break;
                    case "H":
                        rhythm = 7;
                        break;
                    default:
                        rhythm = 8;
                        break;
                }
                switch (magnitude_string)
                {
                    case "A":
                        magnitude = 0;
                        break;
                    case "B":
                        magnitude = 1;
                        break;
                    case "C":
                        magnitude = 2;
                        break;
                    case "D":
                        magnitude = 3;
                        break;
                    default:
                        magnitude = 0;
                        break;
                }
                switch (rhythm_cycles)
                {
                    case "0":
                        rhythm_length = 0;
                        break;
                    case "1":
                        rhythm_length = 1;
                        break;
                    case "2":
                        rhythm_length = 2;
                        break;
                    case "3":
                        rhythm_length = 3;
                        break;
                    case "4":
                        rhythm_length = 4;
                        break;
                    case "5":
                        rhythm_length = 5;
                        break;
                    case "6":
                        rhythm_length = 6;
                        break;
                    case "7":
                        rhythm_length = 7;
                        break;
                    case "8":
                        rhythm_length = 8;
                        break;
                    case "9":
                        rhythm_length = 9;
                        break;
                    case "10":
                        rhythm_length = 10;
                        break;
                    case "11":
                        rhythm_length = 11;
                        break;
                    case "12":
                        rhythm_length = 12;
                        break;
                    case "13":
                        rhythm_length = 13;
                        break;
                    case "14":
                        rhythm_length = 14;
                        break;
                    case "15":
                        rhythm_length = 15;
                        break;
                    default:
                        rhythm_length = 0;
                        break;
                }

                send_values[0] = (byte)((mode * 16) + motor);
                send_values[1] = (byte)((rhythm * 32) + (magnitude * 8) + rhythm_length);

                //Send in binary over Serial_Port FIXME Need to use writeline at the second_byte?
                try
                {
                    Serial_Port.Write(send_values, 0, 2);
                    //Successful if this point is reached w/o error
                    return_values[0] = "";
                }
                catch
                {
                    return_values[0] = "Error sending command over wireless";
                }  
            }
            else
                return_values[0] = "Serial port not open";   

            return return_values;
        }

        //This function queries the belt using the private Query_All() function and returns a specific rhythm value
        public string[] Query_Rhythm(string id)
        {
            string[] return_values = new string[2];
            return_values[1] = "";

            if (Serial_Port.IsOpen)
            {
                //verify that the rhythm ID is between A and H
                if (String.Compare(id, "H") > 0 || String.Compare(id, "A") < 0)
                {
                    //invalid rhythm ID
                    return_values[0] = "Invalid rhythm ID provided as argument to function";
                    return return_values;
                }

                //query all
                string[,] query_results = Query_All();

                if (query_results[0,0].Equals("Error sending command over wireless"))
                {
                    return_values[0] = "Error sending command over wireless";
                    return return_values;
                }

                //find the specified rhythm ID in the query_results array
                int rhythm_number = -1;
                for (int index = 0; index < TOTAL_RESPONSE_COUNT; index++)
                {
                    if (String.Equals(query_results[index, 0], "RSP") && String.Equals(query_results[index, 1], "RHY"))
                    {
                        if (String.Equals(query_results[index, 2], id))
                        {
                            rhythm_number = index;
                        }
                    }
                }

                //return the values for this ID
                if (rhythm_number == -1)
                    return_values[0] = "No rhythm returned for this rhythm id";
                else
                {
                    string binary_pattern = "";
                    string binary_temp;

                    //convert the pattern for this rhythm to a binary string from the hex
                    foreach (char hex in query_results[rhythm_number, 2])
                    {
                        binary_temp = Hex_To_Binary(hex);
                        if (String.Equals(binary_temp, "Error"))
                        {
                            return_values[0] = "Invalid rhythm return, rhythm from query did not contain hex values";
                            return return_values;
                        }
                        binary_pattern += binary_temp;
                    }

                    int length = Convert.ToInt32(query_results[rhythm_number, 3]);
                    if (length == 0)
                        return_values[0] = "This rhythm is currently empty";
                    else
                    {
                        return_values[0] = "";
                        return_values[1] = binary_pattern.Substring(0, length);
                    }
                }
            }
            else
                return_values[0] = "Serial port not open";
                
            return return_values;
        }

        //This function queries the belt using the private Query_All() function and returns a specific magnitude value
        public string[] Query_Magnitude(string id)
        {
            string[] return_values = new string[2];
            return_values[1] = "";

            if (Serial_Port.IsOpen)
            {
                //verify that the rhythm ID is between A and H
                if (String.Compare(id, "A") < 0 || String.Compare(id, "D") > 0)
                {
                    //invalid magnitude ID
                    return_values[0] = "Invalid magnitude ID provided as argument to function";
                    return return_values;
                }

                //query all
                string[,] query_results = Query_All();

                //find the specified magnitude ID in the query_results array
                int magnitude_number = -1;
                for (int index = 0; index < TOTAL_RESPONSE_COUNT; index++)
                {
                    if (String.Equals(query_results[index, 0], "MAG") && String.Equals(query_results[index, 1], id))
                    {
                        magnitude_number = index;
                        break;
                    }
                }

                //return the values for this ID
                if (magnitude_number == -1)
                {
                    return_values[0] = "No magnitude returned for this magnitude id";
                }
                else
                {
                    return_values[0] = "Magnitude successfully queried";
                    return_values[1] = query_results[magnitude_number, 2] + "," + query_results[magnitude_number, 3];
                }
            }
            else
                return_values[0] = "Serial port not open";

            return return_values;
        }

        //This function queries the belt using the private Query_All() function and returns a specific magnitude value
        public string[] Query_Motors()
        {
            string[] return_values = new string[2];
            return_values[0] = "";
            return_values[1] = "";

            //query all
            string[,] query_results = Query_All();

            for (int index = 0; index < TOTAL_RESPONSE_COUNT; index++)
            {
                if (String.Equals(query_results[index, 0], "MTR"))
                {
                    return_values[0] = "";
                    return_values[1] = query_results[index, 1];
                    break;
                }
            }
            return return_values;
        }

        //This function queries the belt such that the function returns a 2-dimensional string array of results
        private string[,] Query_All()
        {
            //this is the array of results which will be returned to the calling function
            string[,] return_values = new string[TOTAL_RESPONSE_COUNT, 4];

            //this function needs to query all motors
            string instruction = "QRY ALL";

            //Send the QRY command
            try
            {
                Serial_Port.WriteLine(instruction);
            }
            catch
            {
                return_values[0, 0] = "Error sending command over wireless";
                return return_values;
            }

            //read back the responses
            string[] responses = new string[TOTAL_RESPONSE_COUNT];

            //read responses from Serial_Port
            try
            {
                for (int index = 0; index < TOTAL_RESPONSE_COUNT; index++)
                {
//FIXME??? Ignores STS and DBG lines, maybe we want a STS array and DBG array?
                    String check = Serial_Port.ReadLine();
                    //Ignore non RSP lines
                    if (!(check.Substring(0, 3).Equals("RSP")))
                        index--;
                    else //Remove the '\r' character at the end for RSP lines
                        responses[index] = check.Substring(0, check.Length - 1);
                }
            }
            catch(Exception e)
            {
                return_values[0, 0] = e.ToString();
                //return_values[0, 0] = "Error receiving query over wireless";
                return return_values;
            }
            //put the responses into a 2d array to return to calling function
            //do this for each of the 16 motors
            for (int index = 0; index < TOTAL_RESPONSE_COUNT; index++)
            {
                string[] split = responses[index].Split(' ');
                String check = split[1];
                //put the values from the response into the return array
                if (check.Equals("RHY") || check.Equals("MAG") || check.Equals("MTR") || check.Equals("VER"))
                {
                    //Populate Return Values
                    for (int i = 0; i < 4; i++)
                    {
                        try
                        {
                            return_values[index, i] = split[i + 1];
                        }
                        //catach split[i+1] OutOfRange Exception
                        catch (IndexOutOfRangeException)
                        {
                            return_values[index, i] = "";
                        }                               
                    }
                }
                else
                {
                    return_values[index, 0] = split[1];
                    //list of motors into second
                    for (int i = 2; i < 4; i++)
                    {
                        return_values[index, 1] = "Unknown";
                    }
                }
            }

            return return_values;
        }

        //This function sends a learn rhythm command to the belt in the form LEARN RHYTHM <id> <pattern> <length>
        public string[] Learn_Rhythm(string id, string pattern_string)
        {
            string[] return_values = new string[2];

            if (Serial_Port.IsOpen)
            {
                //convert the string into an array of integers
                int[] pattern = new int[pattern_string.Length];
                int i = 0;
                foreach (char c in pattern_string)
                {
                    pattern[i] = Convert.ToInt32(c.ToString());
                    i++;
                }

                //verify that the rhythm ID is between A and H
                if (String.Compare(id, "H") > 0 || String.Compare(id, "A") < 0)
                {
                    //invalid rhythm ID
                    return_values[0] = "Invalid rhythm ID provided as argument to function";
                    return_values[1] = "";
                }
                //make sure the length of the binary pattern is 64 bits or less
                else if (pattern.Length > 64)
                {
                    //invalid pattern length
                    return_values[0] = "Invalid Pattern Length provided as argument to function";
                    return_values[1] = "";
                }
                //copy the contents of the pattern array into a 64 bit array for conversion
                else
                {
                    int[] internal_pattern = new int[64];
                    for (int pattern_index = 0; pattern_index < pattern.Length; pattern_index++)
                    {
                        //error check for 0 or 1 in the pattern array and copy to 64 bit pattern
                        if (pattern[pattern_index] != 0 && pattern[pattern_index] != 1)
                        {
                            //pattern not a list of ones and zeros
                            return_values[0] = "Pattern not a list of zeros and ones, invalid pattern provided as argument to function";
                            return_values[1] = "";
                            return return_values;
                        }
                        internal_pattern[pattern_index] = pattern[pattern_index];
                    }
                    //put zeros in the remaining contents of the array
                    for (int pattern_index2 = pattern.Length; pattern_index2 < 64; pattern_index2++)
                    {
                        internal_pattern[pattern_index2] = 0;
                    }

                    //convert the array of values into a 16-character hex string
                    int temp_decimal_value = 0;
                    string hex_string = "";
                    for (int count = 0; count < 64; count = count + 4)
                    {
                        for (int index = 0; index < 4; index++)
                        {
                            temp_decimal_value = (temp_decimal_value * 2) + internal_pattern[index + count];
                        }
                        hex_string += temp_decimal_value.ToString("X");
                        temp_decimal_value = 0;
                    }

                    //at this point hex_string holds a string containing the 16-character hex code to be passed to belt

                    string instruction = String.Concat("LRN RHY ", id.ToString(), " ", hex_string, " ", pattern.Length.ToString());

                    //send this output to the belt
                    try
                    {
                        Serial_Port.WriteLine(instruction);
                        //check for STATUS <error number> [<info>]
                        if (Check_Belt() != 0)
                        {
                            return_values[0] = "No response from belt or belt error";
                            return_values[1] = "";
                        }
                        else
                        {
                            //if everything completes successfully
                            return_values[0] = "";
                            return_values[1] = "";
                        }
                    }
                    catch
                    {
                        return_values[0] = "Error sending command over wireless";
                        return_values[1] = "";
                    }
                }
            }
            else
            {
                return_values[0] = "Serial port not open";
                return_values[1] = "";
            }
            return return_values;
        }

        //This function sends a learn magnitude command to the belt in the form LEARN MAGNITUDE <period> <duty cycle>
        public string[] Learn_Magnitude(string id, string period, string duty_cycle)
        {
            string[] return_values = new string[2];

            if (Serial_Port.IsOpen)
            {
                if (String.Compare(id, "A") < 0 || String.Compare(id, "D") > 0)
                {
                    //invalid magnitude ID
                    return_values[0] = "Invalid magnitude ID provided as argument to function";
                    return_values[1] = "";
                }
                else
                {
                    string instruction = String.Concat("LRN MAG ", id, " ", period, " ", duty_cycle);

                    //send this output to the belt
                    try
                    {
                        Serial_Port.WriteLine(instruction);
                        //check for STATUS <error number> [<info>]
                        if (Check_Belt() != 0)
                        {
                            return_values[0] = "No response from belt or belt error";
                            return_values[1] = "";
                        }
                        else
                        {
                            //if everything completes successfully
                            return_values[0] = "";
                            return_values[1] = "";
                        }
                    }
                    catch
                    {
                        return_values[0] = "Error sending command over wireless";
                        return_values[1] = "";
                    }
                }
            }
            else
            {
                return_values[0] = "Serial port not open";
                return_values[1] = "";
            }
            return return_values;
        }
        
        public string[] Back()
        {
            string[] return_values = new string[2];
            byte[] send_values = new byte[2];
            return_values[1] = "";
   
            if (Serial_Port.IsOpen)
            {
                send_values[0] = 48;
                send_values[1] = 0;
                //send BACK command through Serial_Port
                try
                {
                    Serial_Port.Write(send_values, 0, 2);
                    /*FIXME , do we ge a response here? check for STATUS <error number> [<info>]
                    if (Check_Belt() != 0)
                        return_values[0] = "No response from belt or belt error";
                    else
                        return_values[0] = "";
                     */
                }
                catch
                {
                    return_values[0] = "Error sending command over wireless";  
                }
            }
            else
                return_values[0] = "Serial port not open";

            return return_values;
        }

        //this function is used to issue a general START command to the belt
        public string[] Start()
        {
            string[] return_values = new string[2];

            if (Serial_Port.IsOpen)
            {
                string instruction = "BGN";
                //send START command through Serial_Port
                try
                {
                    Serial_Port.WriteLine(instruction);
                    //check for STATUS <error number> [<info>]
                    if (Check_Belt() != 0)
                    {
                        return_values[0] = "No response from belt or belt error";
                        return_values[1] = "";
                    }
                    else
                    {
                        return_values[0] = "";
                        return_values[1] = "";
                    }
                }
                catch
                {
                    return_values[0] = "Error sending command over wireless";
                    return_values[1] = "";
                }
            }
            else
            {
                return_values[0] = "Serial port not open";
                return_values[1] = "";
            }            
            return return_values;
        }

        //this function is used to issue a general STOP command to the belt
        public string[] Stop()
        {
            string[] return_values = new String[2];

            if (Serial_Port.IsOpen)
            {
                Vibrate_Motor("1", "A", "A", "0");
                return_values[0] = "";
                return_values[1] = "";
            }
            else
            {
                return_values[0] = "Serial port not open";
                return_values[1] = "";
            }
            return return_values;
        }

        //function used to initialize the Serial_Port communication as serial, taking parameters as follows
        //portno = "0" or "1" or "2" .... such that this number represents the integer value for com port, for example for COM9 portno = "9"
        //baud_string = the baud rate to initialize at
        //parity_string = the parity setting (None, Odd, Even, Mark, Space)
        //stopbits_string = the stop bits setting (0, 1, 1.5, 2)
        //data_bits string = the number of data bits to send at a time
        public string[] Initialize_Serial_Port(string portno, string baud_string, string parity_string, string stopbits_string, string databits_string)
        {
            string[] return_values = new string[2];

            if (!Serial_Port.IsOpen)
            {
                int baudrate = Convert.ToInt32(baud_string);
                int databits = Convert.ToInt32(databits_string);

                //possible initializations which are not used
                //Serial_Port.Handshake = Handshake.RequestToSend;
                //Serial_Port.Encoding = System.Text.Encoding.Default;
                //Serial_Port.NewLine = "\r";
                //Serial_Port.ReceivedBytesThreshold = 20;

                Parity parity;
                StopBits stopBits;
                //Defaults to current proper settings for pairity and stop bits
                switch (parity_string)
                {
                    case "None":
                        parity = Parity.None;
                        break;
                    case "Even":
                        parity = Parity.Even;
                        break;
                    case "Odd":
                        parity = Parity.Odd;
                        break;
                    case "Mark":
                        parity = Parity.Mark;
                        break;
                    case "Space":
                        parity = Parity.Space;
                        break;
                    default:
                        parity = Parity.None;
                        break;
                }
                switch (stopbits_string)
                {
                    case "0":
                        stopBits = StopBits.None;
                        break;
                    case "1":
                        stopBits = StopBits.One;
                        break;
                    case "1.5":
                        stopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        stopBits = StopBits.Two;
                        break;
                    default:
                        stopBits = StopBits.One;
                        break;
                }
                Serial_Port = new SerialPort(portno, baudrate, parity, databits, stopBits);
                Serial_Port.Open();

                return_values[0] = "";
                return_values[1] = "";
            }
            else
            {
                return_values[0] = "Serial port not closed";
                return_values[1] = "";
            }
            return return_values;
        }

        public string[] Close_Serial_Port()
        {
            string[] results = new string[2];

            if (Serial_Port.IsOpen)
            {
                Serial_Port.Close();

                results[0] = "";
                results[1] = "";
            }
            else
            {
                results[0] = "Serial port not open";
                results[1] = "";
            }
            return results;
        }

        //returns COM ports available to the system
        public string[] Get_Available_Ports()
        {
            return SerialPort.GetPortNames();
        }

        //this is used to get status from belt after each send or receive
        private int Check_Belt()
        {
            string temp;

            try
            {
                temp = Serial_Port.ReadLine();
            }
            catch
            {
                return -9;
            }

            string[] split = temp.Split(' ');

            if (!String.Equals(split[0], "STS"))
            {
                return -9;
            }
            else
            {
                return Convert.ToInt32(split[1]);
            }
        }

        //this function converts hex values to binary
        private string Hex_To_Binary(char c)
        {
            switch (c)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'A':
                    return "1010";
                case 'B':
                    return "1011";
                case 'C':
                    return "1100";
                case 'D':
                    return "1101";
                case 'E':
                    return "1110";
                case 'F':
                    return "1111";
                default:
                    return "Error";
            }
        }
    }
}