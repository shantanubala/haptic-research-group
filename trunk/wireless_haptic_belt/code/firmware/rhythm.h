/*****************************************************************************
 * FILE:   rhythm.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  The rhythm structure.
 * LOG:    20090413 - initial version
 ****************************************************************************/

#ifndef RHYTHM_H
#define RHYTHM_H

#include<inttypes.h>

#define MAX_RHYTHM 8	// maximum number of rhythms that can be learned
#define MAX_RBITS 64	// number of bits per pattern

typedef struct {
	uint8_t pattern[8];	// each bit represents 50ms; MSB first
	uint8_t bits;		// number of pattern bits actually used
} rhythm_t;

#endif
