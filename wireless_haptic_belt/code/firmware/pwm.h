/*****************************************************************************
 * FILE:   pwm.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Low-level pwm function declarations for the ATtiny48.
 * LOG:    20090419 - initial version
 ****************************************************************************/

#ifndef PWM_H
#define PWM_H

void pwm_init( void );		// set up the tiny's pwm control registers
void pwm_set( int period, int duty );	// configure pwm period and duty cycle
void pwm_on( void );		// reset pwm counter and enable pwm
void pwm_off( void );		// turn off pwm

#endif
