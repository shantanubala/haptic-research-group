/*****************************************************************************
 * FILE:   timer.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Low-level timer function declarations for the ATtiny48.
 * LOG:    20090421 - initial version
 ****************************************************************************/

#ifndef TIMER_H
#define TIMER_H

#include<inttypes.h>

// timer function callback type
typedef void (*timer_func_t)( void );

void timer_init( void );	// set up the tiny's timer0 control registers
void timer_set( uint8_t interval );	// configure interrupt interval (ms)
void timer_on( void );		// reset and enable timer
void timer_off( void );		// disable timer
void timer_func( timer_func_t );// set function to call when interrupt occurs

#endif
