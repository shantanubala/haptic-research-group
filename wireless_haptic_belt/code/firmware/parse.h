/*****************************************************************************
 * FILE:   parse.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Type definitions for the parser in parse.c.
 * LOG:    20090429 - initial version
 ****************************************************************************/

#ifndef PARSE_H
#define PARSE_H

/* doing this the correct way with extern "C" makes the Funnel not run
#ifdef __cplusplus
extern "C" {
#endif
*/

#include<avr/pgmspace.h>

#include"rhythm.h"
#include"magnitude.h"
#include"error.h"

#define PARSE_MAX_WORDS 10	// maximum number of words in a single command
#define PARSE_MAX_LEN 32	// maximum length of a single command

// convert an ASCII ID letter from the given argument number to an index
#define ltoi( _argnum_ ) ((uint8_t)(*argv[_argnum_] - 'A'))

// convert an index to an ASCII ID letter
#define itol( _id_ ) ('A' + _id_)

// function pointer type for individual command handlers
typedef error_t (*parse_func_t)( int argc, const char *const *argv );

// parse table constituent type
typedef struct parse_step_s {
	const char str[4];
	const struct parse_step_s *next;
	parse_func_t func;
} parse_step_t;

// convert an ASCII hex digit into an integer
int8_t htoi( char digit );

// convert a rhythm specification into native format at the given location
error_t parse_rhythm( int argc, const char *const *argv, rhythm_t *into );
error_t parse_magnitude( int argc, const char *const *argv, magnitude_t *into );

// main parser for learning mode commands; modifies line argument in place
error_t parse( const parse_step_t *table, char *line );

/*
#ifdef __cplusplus
} // extern "C"
#endif
*/

#endif
