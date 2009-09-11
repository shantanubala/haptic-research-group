/*****************************************************************************
 * FILE:   debug.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Function declarations to aid with debugging.
 * LOG:    20090421 - initial version
 ****************************************************************************/

#ifndef DEBUG_H
#define DEBUG_H

void dumpbyte( uint8_t byte );	// dump a byte onto the motor
void delay( uint16_t ms );	// busy wait with ms granularity (1MHz clock)

#endif
