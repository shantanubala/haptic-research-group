/*****************************************************************************
 * FILE:   debug.c
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Functions to aid with debugging.
 * LOG:    20090421 - initial version
 ****************************************************************************/

#include<util/delay_basic.h>

#include"pwm.h"

#define DEBUG_SCALE 1
#define DELAY( _ms_ ) delay( _ms_ * DEBUG_SCALE )

// busy-wait with millisecond granularity
// assumes CPU is clocked at 1MHz
void delay( uint16_t ms )
{
	while( ms > 256 ) {
		_delay_loop_2( (uint16_t)256 * 250 );
		ms -= 256;
	}
	if( ms ) _delay_loop_2( ms * 250 );
}

// dump a byte onto the motor
// essentially turns the byte into a reduced-frequency rhythm and plays one
// cycle at full magnitude, using busy-waiting to avoid any interrupt issues
void dumpbyte( uint8_t byte )
{
	int i;
	pwm_off();
	pwm_set( 2000, 2000 );

	for( i=0; i<8; ++i ) {
		pwm_on();
		if( byte & ((uint8_t)0x80>>i) ) {
			DELAY(300);
			pwm_off();
			DELAY(200);
		}else {
			DELAY(50);
			pwm_off();
			DELAY(450);
		}
	}

	pwm_off();
	DELAY(1000);
}
