/*****************************************************************************
 * FILE:   magnitude.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  The magnitude structure.
 * LOG:    20090413 - initial version
 ****************************************************************************/

#ifndef MAGNITUDE_H
#define MAGNITUDE_H

#define MAX_MAGNITUDE 4

typedef struct {
	uint16_t period;	// in microseconds
	uint16_t duty;		// duty cycle in us; must be < period
} magnitude_t;

#endif
