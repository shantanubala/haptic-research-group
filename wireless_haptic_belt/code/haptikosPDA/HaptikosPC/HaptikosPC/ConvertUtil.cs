﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HapticDriver
{
    public partial class HapticBelt
    {
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
                    // Does not work for error code string "16"
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