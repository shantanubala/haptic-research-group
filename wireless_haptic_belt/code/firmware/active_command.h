/*****************************************************************************
 * FILE:   active_command.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Active mode command definition.
 * LOG:    20090510 - initial version
 ****************************************************************************/

#ifndef ACTIVE_COMMAND_H
#define ACTIVE_COMMAND_H

#include "vibration.h"

// active mode command
typedef struct {
	uint8_t motor:4,
		mode:4;
	vibration_t v;
} active_command_t;

// values for the mode field of an active mode command
typedef enum {
	ACM_VIB,	// activate a motor
	ACM_SPT,	// play back a spatio-temporal pattern
	ACM_GCL,	// send a command to all motors (general call)
	ACM_LRN		// return to learning mode
} acmd_mode_t;

#endif
