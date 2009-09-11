/*****************************************************************************
 * FILE:   globals.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Structure for global items on the ATtiny48, defined in tiny.c.
 * LOG:    20090418 - initial version
 ****************************************************************************/

#ifndef GLOBALS_H
#define GLOBALS_H

#include"rhythm.h"
#include"magnitude.h"

typedef struct {
	// tables of all known rhythms and magnitudes
	// written by learn_rhythm() and learn_magnitude() in learn_tiny.c
	// read by rhythm_step() in tiny.c
	rhythm_t rhythms[ MAX_RHYTHM ];
	magnitude_t magnitudes[ MAX_MAGNITUDE ];

	// indices for tracking the execution of the active rhythm
	// may be changed mid-execution when a new command arrives on I2C
	int ar;	// index into rhythms of currently active rhythm
	int ab;	// active bit position in rhythms[ar].pattern
	int ac;	// remaining number of cycles in active rhythm
} globals_t;

extern globals_t glbl;

#endif
