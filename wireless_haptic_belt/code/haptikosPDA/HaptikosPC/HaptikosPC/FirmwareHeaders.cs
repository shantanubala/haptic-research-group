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
    public enum error_t
    {
        // symbol	error type (belt mode, command source->destination)
        ESUCCESS,	// no error
        EBADCMD,	// command not recognized		(L/O, P->M)
        ETOOBIG,	// command too long			(L, M->V)
        EARG,		// invalid argument			(L, P->M/M->V)
        ENOR,		// requested rhythm not defined		(O, P->M/M->V)
        ENOM,		// requested magnitude not defined	(O, P->M/M->V)
        ENOS,		// spatio-temporal pattern not defined	(O, P->M/M->V)
        ENOMOTOR,	// requested motor not present on belt	(O, P->M)
        EINVR,		// invalid rhythm definition		(L, P->M/M->V)
        EINVM,		// invalid magnitude definition		(L, P->M/M->V)
        EINVS,		// invalid spatio-temporal definition	(L, P->M)
        EBADVC,		// vibrator command not recognized	(L/O, M->V)
        EBUS,		// I2C communication failed		(L/O, M->V)
        EBUSOF,		// I2C transmit overflow		(L/O, M->V)
        EBUSAN,		// I2C address not acknowledged		(L/O, M->V)
        EBUSDN,		// I2C data not acknowledged		(L/O, M->V)
        EMISSING,	// command not implemented yet		(L, P->M/M->V)
        EMAX,		// invalid error number

        // status_msg definitions generated from DLL project
        // symbol
        COMPRTINVALID, // invalid comm port parameters
        COMPRTOPEN,    // comm port opened
        COMPRTNOTOPEN, // comm port not open
        COMPRTCLS,     // comm port closed
        COMPRTCLSPREV, // comm port previously closed
        EXCEPTION,     // exception occured
        EXCWIRELESS    // exception occured - error sending over wireless


    };
    /// <summary>
    /// enumeration to hold SerialPortManager message types
    /// </summary>
    public enum MessageType { INCOMING, OUTGOING, NORMAL, WARNING, ERROR };

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
        private const string exception = "Exception occured";
        private const string excwireless = "Error sending command over wireless";

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
            exception,
            excwireless
        };

        public const UInt16 uint16_t_max = 65535;
        public const UInt16 PERIOD_MAX = 2000; //2000 microseconds (2 millisec)
        public const UInt16 DUTY_CYCLE_MIN = 2; //2 microseconds
    }
}