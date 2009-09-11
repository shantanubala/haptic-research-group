/*****************************************************************************
 * FILE:   vibration.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Definition of the Funnel-to-tiny operational command.
 * LOG:    20090430 - initial version
 ****************************************************************************/

#ifndef VIBRATION_H
#define VIBRATION_H

#define MAX_DURATION 7	// max cycle count for rhythm playback

// FIXME: The Wrong Thing (TM)
// (but it's so easy!)
typedef struct {
	uint8_t duration:3,
		magnitude:2,
		rhythm:3;
} vibration_t;

#endif
