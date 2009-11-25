/*****************************************************************************
 * FILE:   Constants.cs
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 *         Nathan J. Edwards (nathan.edwards@asu.edu)
 * DESCR:  Firmware and DLL Enumeration and standard messaging.
 * LOG:    20090510 - initial version
 *         20091109 - ported to DLL, added additional messaging
 ****************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace HapticDriver
{
    //Enums to handle bit fields in C# which are not allowed:

    /*****************************************************************************
     * FILE:   active_command.h
     * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
     * DESCR:  Active mode command definition.
     * LOG:    20090510 - initial version
     ****************************************************************************/

    // active mode command
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    internal struct active_command_t
    {
        // FieldOffsetAttribute to indicate the position of that field within the type
        // The offset, in bytes, from the beginning of the structure to the beginning of the field
        //
        // This is really a union but you can use it as a bitfield--you just have to 
        // be conscious of where in the byte the bits for each field are supposed to be.

        [FieldOffset(0)]
        internal byte motor; //lower 4 bits of byte
        [FieldOffset(0)]
        internal byte mode;//lower 4 bits of byte
        [FieldOffset(1)]
        internal vibration_t v; //next byte
    };

    // values for the mode field of an active mode command
    internal enum acmd_mode_t
    {
        ACM_VIB,	// activate a motor
        ACM_SPT,	// play back a spatio-temporal pattern (NOT YET IMPLEMENTED)
        ACM_GCL,	// send a command to all motors (general call)
        ACM_LRN		// return to learning mode
    } ;

    // From firmware globals_main.h
    // possible belt operation modes
    internal enum mode_t
    {
        M_LEARN,	// learning mode: ASCII commands
        M_ACTIVE	// active mode: raw byte stream
    } ;

    /*****************************************************************************
     * FILE:   vibration.h
     * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
     * DESCR:  Definition of the Funnel-to-tiny operational command.
     * LOG:    20090430 - initial version
     ****************************************************************************/

    //#define MAX_DURATION 7	// max cycle count for rhythm playback
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    internal struct vibration_t
    {
        // FieldOffsetAttribute to indicate the position of that field within the type
        // The offset, in bytes, from the beginning of the structure to the beginning of the field
        //
        // This is really a union but you can use it as a bitfield--you just have to 
        // be conscious of where in the byte the bits for each field are supposed to be.

        [FieldOffset(0)]
        internal byte duration; // lower 3 bits
        [FieldOffset(0)]
        internal byte magnitude; // middle 2 bits
        [FieldOffset(0)]
        internal byte rhythm; // upper 3 bits
    };

    /*****************************************************************************
     * FILE:   error.h
     * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
     * DESCR:  Type definitions and function declarations for error handling.
     * LOG:    20090501 - initial version
     ****************************************************************************/

    // error number definitions--must match the string table in error.c
    // in error descriptions, L = learning mode, O = operational mode,
    // P = controlling PC, M = main belt controller, V = vibrator controller

    /// <summary>
    /// Error number definitions--must match the string table in class Constants
    /// </summary>
    public enum error_t
    {
        // symbol	error type (belt mode, command source->destination)
        /// <summary>No error</summary> 
        ESUCCESS,
        /// <summary>Command not recognized</summary> 
        EBADCMD,
        /// <summary>Command too long</summary> 
        ETOOBIG,
        /// <summary>Invalid argument</summary> 
        EARG,
        /// <summary>Requested rhythm not defined</summary> 
        ENOR,
        /// <summary>Requested magnitude not defined</summary> 
        ENOM,
        /// <summary>Spatio-temporal pattern not defined</summary> 
        ENOS,
        /// <summary>Requested motor not present on belt</summary> 
        ENOMOTOR,
        /// <summary>Invalid rhythm definition</summary> 
        EINVR,
        /// <summary>Invalid magnitude definition</summary> 
        EINVM,
        /// <summary>Invalid spatio-temporal definition</summary> 
        EINVS,
        /// <summary>Vibrator command not recognized</summary> 
        EBADVC,
        /// <summary>I2C communication failed</summary> 
        EBUS,
        /// <summary>I2C transmit overflow</summary> 
        EBUSOF,
        /// <summary>I2C address not acknowledged</summary> 
        EBUSAN,
        /// <summary>I2C data not acknowledged</summary> 
        EBUSDN,
        /// <summary>Command not implemented yet</summary> 
        EMISSING,
        /// <summary>Unknown error</summary> 
        EMAX,

        /*
         * Status msg definitions generated from DLL project
         */
        /// <summary>Invalid comm port parameters</summary> 
        COMPRTINVALID,
        /// <summary>Com port opened</summary> 
        COMPRTOPEN,
        /// <summary>Com port not open</summary> 
        COMPRTNOTOPEN,
        /// <summary>Com port closed</summary> 
        COMPRTCLS,
        /// <summary>Com port previously closed</summary> 
        COMPRTCLSPREV,
        /// <summary>Com port setup error</summary> 
        COMPRTSETUP,
        /// <summary>Com port write data error</summary> 
        COMPRTWRITE,
        /// <summary>Com port read data error</summary> 
        COMPRTREAD,
        /// <summary>Com port read data timeout</summary> 
        COMPRTREADTIME,
        /// <summary>Invalid rhythm ID</summary> 
        INVRHYID,
        /// <summary>Rhythm pattern not a list of hex values</summary> 
        INVRHYPATHEX,
        /// <summary>Rhythm pattern not a list of binary values</summary> 
        INVRHYPATBIN,
        /// <summary>Invalid pattern length - too long</summary> 
        INVRHYPATLEN,
        /// <summary>Invalid rhythm time</summary> 
        INVRHYTIME,
        /// <summary>Invalid magnitude ID</summary> 
        INVMAGID,
        /// <summary>Invalid magnitude - exceeds maximum of 100%</summary> 
        INVMAGHIGH,
        /// <summary>Exception occured</summary> 
        EXCEPTION,
        /// <summary>Exception occured - vibrate command</summary> 
        EXCVIBCMD,
        /// <summary>Exception occured - query command</summary> 
        EXCQRYCMD,
        /// <summary>Exception occured - erase all command</summary> 
        EXCZAPCMD,
        /// <summary>Exception occured - learn rhythm command</summary> 
        EXCLRNRHY,
        /// <summary>Exception occured - learn magnitude command</summary> 
        EXCLRNMAG,
        /// <summary>Exception occured - serial com port write data</summary> 
        EXCCOMPRTWRITE,
        /// <summary>Exception occured - opening serial com port</summary> 
        EXCCOMPRTOPEN,
        /// <summary>Exception occured - closing serial com port</summary> 
        EXCCOMPRTCLS,
        /// <summary>Exception occured - error sending over wireless</summary> 
        EXCWIRELESS,
        /// <summary>Exception occured - searching for requested data</summary> 
        EXCDATASEARCH,
        /// <summary>Requested data not found</summary> 
        NOTFOUND
    };

    /// <summary>
    /// Enumeration to hold SerialPortManager message types
    /// </summary>
    public enum MessageType { 
        // The XML tags are not populated since this enumeration is self-explanatory.
        /// <summary>
        /// 
        /// </summary>
        INCOMING, 
        /// <summary>
        /// 
        /// </summary>
        OUTGOING, 
        /// <summary>
        /// 
        /// </summary>
        NORMAL, 
        /// <summary>
        /// 
        /// </summary>
        WARNING, 
        /// <summary>
        /// 
        /// </summary>
        ERROR 
    };

    /// <summary>
    /// Enumeration that specifies which type of query to execute 
    /// </summary>
    public enum QueryType { 
        /// <summary>
        /// Does not execute query.  Used in methods where you want 
        /// the previous local data returned without executing a query
        /// of the haptic belt's configuration. NOTE: may be old data.
        /// </summary>
        PREVIOUS, 
        /// <summary>
        /// Execute query of a single item (such as Rhythms).  Used
        /// in methods where you want current belt configuration
        /// returned but do not want to wait for a Query All
        /// </summary>
        SINGLE, 
        /// <summary>
        /// Execute query of all haptic belt configurations.
        /// </summary>
        ALL 
    };

    internal static class Constants
    {
        // name the error strings individually                                    
        // required to get the strings stored in program space                                     
        private const string esuccess = "Success";
        private const string ebadcmd = "Command not recognized";
        private const string etoobig = "Command too long";
        private const string earg = "Invalid argument";
        private const string enor = "Requested rhythm not defined";
        private const string enom = "Requested magnitude not defined";
        private const string enos = "Requested spatio-temporal pattern not defined";
        private const string enomotor = "Requested motor not present";
        private const string einvr = "Invalid rhythm definition";
        private const string einvm = "Invalid magnitude definition";
        private const string einvs = "Invalid spatio-temporal pattern definition";
        private const string ebadvc = "Vibrator command not recognized";
        private const string ebus = "Bus communication failed";
        private const string ebusof = "Bus transmit overflow";
        private const string ebusan = "Bus address not acknowledged";
        private const string ebusdn = "Bus data not acknowledged";
        private const string emissing = "Command not implemented";
        private const string emax = "Unknown error";

        // name the dll status strings individually                                    
        // required to get the strings stored in program space   
        private const string comprtinvalid = "Invalid comm port parameters";
        private const string comprtopen = "Com port opened";
        private const string comprtnotopen = "Com port not opened";
        private const string comprtcls = "Com port closed";
        private const string comprtclsprev = "Com port previously closed";
        private const string comprtsetup = "Com port setup error";
        private const string comprtwrite = "Com port write data error";
        private const string comprtread = "Com port read data error";
        private const string comprtreadtime = "Com port read data timeout";
        private const string invrhyid = "Invalid rhythm ID";
        private const string invrhypathex = "Rhythm pattern not a list of hex values";
        private const string invrhypatbin = "Rhythm pattern not a list of binary values";
        private const string invrhypatlen = "Invalid pattern length - too long";
        private const string invrhytime = "Invalid rhythm time";
        private const string invmagid = "Invalid magnitude ID";
        private const string invmaghigh = "Invalid magnitude - exceeds maximum of 100%";
        private const string exception = "Exception occured";
        private const string excvibcmd = "Exception occured - vibrate command";
        private const string excqrycmd = "Exception occured - query command";
        private const string exczapcmd = "Exception occured - erase all command";
        private const string exclrnrhy = "Exception occured - learn rhythm command";
        private const string exclrnmag = "Exception occured - learn magnitude command";
        private const string execomprtwrite = "Exception occured - serial com port write data";
        private const string execomprtopen = "Exception occured - opening serial com port";
        private const string execomprtcls = "Exception occured - closing serial com port";
        private const string excwireless = "Error sending command over wireless";
        private const string excdatasearch = "Exception occured - searching for requested data";
        private const string notfound = "Requested data not found";

        internal static string[] error_t_names = {
	        // Firmware error strings--must match the error_t enum in error.h
            esuccess,
	        ebadcmd,
	        etoobig,
	        earg,
	        enor,
	        enom,
	        enos,
	        enomotor,
	        einvr,
	        einvm,
	        einvs,
	        ebadvc,
	        ebus,
	        ebusof,
	        ebusan,
	        ebusdn,
	        emissing,
	        emax,

            // Driver (DLL) status_msg strings--must match the order of status_msg enum
            comprtinvalid,
            comprtopen,
            comprtnotopen,
            comprtcls,
            comprtclsprev,
            comprtsetup,
            comprtwrite,
            comprtread,
            comprtreadtime,
            invrhyid,
            invrhypathex,
            invrhypatbin,
            invrhypatlen,
            invrhytime,
            invmagid,
            invmaghigh,
            exception,
            excvibcmd,
            excqrycmd,
            exczapcmd,
            exclrnrhy,
            exclrnmag,
            execomprtwrite,
            execomprtopen,
            execomprtcls,
            excwireless,
            excdatasearch,
            notfound
        };

        public const UInt16 uint16_t_max = 65535;
        public const UInt16 PERIOD_MAX = 2000; //2000 microseconds (2 millisec)
        public const UInt16 DUTY_CYCLE_MIN = 2; //2 microseconds
    }
}