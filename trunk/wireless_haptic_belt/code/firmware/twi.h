/*****************************************************************************
 * FILE:   twi.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Low-level TWI function declarations for the ATtiny48.
 * LOG:    20090419 - initial version
 *         20090421 - add TW_STATUS_MASK fix
 ****************************************************************************/

#ifndef TWI_H
#define TWI_H

// somebody blew it in avr-libc and defined TWS[3-7] wrong for ATtiny48 only,
// making TW_STATUS_MASK 0x7c instead of 0xf8 as it should be
// so include util/twi.h here and fix TW_STATUS_MASK
#include<util/twi.h>
#ifdef __AVR_ATtiny48__
#	undef TW_STATUS_MASK
#	define TW_STATUS_MASK 0xf8
#endif

#include"error.h"

// timer function callback type
typedef error_t (*twi_func_t)( char *data, int len );

void twi_init( void );		// set up the TWI module for slave operation
void twi_func( twi_func_t );	// set function to call when command arrives

#endif
