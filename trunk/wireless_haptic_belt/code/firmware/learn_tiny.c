/*****************************************************************************
 * FILE:   learn_tiny.c
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Command handlers for learning mode in the vibration modules.
 * LOG:    20090426 - initial version
 ****************************************************************************/

#include<stdlib.h>
#include<string.h>

#include"globals.h"
#include"rhythm.h"
#include"magnitude.h"
#include"learn_tiny.h"

//#include"debug.h"

// command handlers
// LRN RHY <ID> <PATTERN> <BITS>
static error_t learn_rhythm( int argc, const char *const *argv )
{
	return parse_rhythm( argc, argv, glbl.rhythms+ltoi(0) );
}

// LRN MAG <ID> <PERIOD> <DUTY>
static error_t learn_magnitude( int argc, const char *const *argv )
{
	return parse_magnitude( argc, argv, glbl.magnitudes+ltoi(0) );
}

static error_t learn_address( int argc, const char *const *argv )
{
	return EMISSING;
}

static error_t query_version( int argc, const char *const *argv )
{
	return EMISSING;
}

static error_t query_address( int argc, const char *const *argv )
{
	return EMISSING;
}

static error_t test( int argc, const char *const *argv )
{
	return EMISSING;
}

// parse table definitions
static const parse_step_t pt_learn[] PROGMEM = {
	{ "RHY", NULL, learn_rhythm },
	{ "MAG", NULL, learn_magnitude },
	{ "ADD", NULL, learn_address },
	{ "", NULL, NULL }
};

static const parse_step_t pt_query[] PROGMEM = {
	{ "VER", NULL, query_version },
	{ "ADD", NULL, query_address },
	{ "", NULL, NULL }
};

static const parse_step_t pt_top[] PROGMEM = {
	{ "LRN", pt_learn, NULL },
	{ "QRY", pt_query, NULL },
	{ "TST", NULL, test },
	{ "", NULL, NULL }
};

error_t handle_learn( char *cmd )
{
	return parse( pt_top, cmd );
}
