/*****************************************************************************
 * FILE:   led.h
 * AUTHOR: Jacob Rosenthal (Jacob.Rosenthal@asu.edu)
 * DESCR:  Function declarations to aid with debugging.
 * LOG:    20090714 - initial version
 ****************************************************************************/

#ifndef LED_H
#define LED_H

#include<avr/io.h>
#define ELED PORTC1 
#define SLED PORTC0 

void setup_led( void );	//engage or disengage a light  
void set_led(uint8_t port, uint8_t boolean); //engage or disengage a light

#endif
