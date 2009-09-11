/*****************************************************************************
 * FILE:   error.c
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Strings and functions for human-readable error messages.
 * LOG:    20090504 - initial version
 ****************************************************************************/

#include"error.h"

// this should check for something only defined on avr, but since it's not
// clear what that define would be, this is easy...                       
#ifndef PROGMEM                                                           
#	define PROGMEM                                                      
#	define pgm_read_word( _addr_ ) *(_addr_)                             
#endif                                                                    

// name the error strings individually                                    
// required to get the strings stored in program space                    
#define STR static const char                                             
STR esuccess[] PROGMEM = "Success";
STR ebadcmd[] PROGMEM = "Command not recognized";
STR etoobig[] PROGMEM = "Command too long";
STR earg[] PROGMEM = "Invalid argument";
STR enor[] PROGMEM = "Requested rhythm not defined";
STR enom[] PROGMEM = "Requested magnitude not defined";
STR enos[] PROGMEM = "Requested spatio-temporal pattern not defined";
STR enomotor[] PROGMEM = "Requested motor not present";
STR einvr[] PROGMEM = "Invalid rhythm definition";
STR einvm[] PROGMEM = "Invalid magnitude definition";
STR einvs[] PROGMEM = "Invalid spatio-temporal pattern definition";
STR ebadvc[] PROGMEM = "Vibrator command not recognized";
STR ebus[] PROGMEM = "Bus communication failed";
STR ebusof[] PROGMEM = "Bus transmit overflow";
STR ebusan[] PROGMEM = "Bus address not acknowledged";
STR ebusdn[] PROGMEM = "Bus data not acknowledged";
STR emissing[] PROGMEM = "Command not implemented";
STR emax[] PROGMEM = "Unknown error";

// error strings--must match the error_t enum in error.h
STR *error_names[] PROGMEM = {
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
	emax
};
#undef STR

// return a human-readable error string for the given error number
const char* errstr( error_t num )
{
	if( num<0 || num>EMAX )
		return (const char*)pgm_read_word( error_names + EMAX );

	return (const char*)pgm_read_word( error_names + num );
}
