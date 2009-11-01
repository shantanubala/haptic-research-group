/*****************************************************************************
 * FILE:   globals_main.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Structure for global items on the Funnel, defined in main.c.
 * LOG:    20090510 - initial version
 ****************************************************************************/

#ifndef GLOBALS_H
#define GLOBALS_H

#include"active_command.h"
#include"parse.h"
#include"menu.h"

// current version of the funnel firmware--must be an ASCII decimal number
#define FUNNEL_VER "0"

// expected version of the motor modules
#define TINY_VER 0

// maximum number of motors the firmware can support
#define MAX_MOTORS 16

// possible belt operation modes
typedef enum {
	M_LEARN,	// learning mode: ASCII commands
	M_ACTIVE	// active mode: raw byte stream
} mode_t;

typedef struct {
	// common buffer used to receive commands over serial, send commands
	// over TWI, and send responses over serial
	char cmd[ PARSE_MAX_LEN ];

	// used to receive active mode commands over serial and relay over TWI
	active_command_t acmd;

	// mapping of motor numbers to TWI addresses
	struct { uint8_t addr:7, err:1; } mtrs[ MAX_MOTORS+1 ];

	// current belt mode
	mode_t mode;

	// whether the belt is in menu mode, and which menu it is displaying
	uint8_t in_menu:1,
		echo:1;
	menu_step_t menustep;
} globals_t;

extern globals_t glbl;

#endif
