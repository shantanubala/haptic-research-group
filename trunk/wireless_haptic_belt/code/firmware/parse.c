/*****************************************************************************
 * FILE:   parse.c
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Command parser for learning mode (main controller and tinys).
 * LOG:    20090430 - initial version
 ****************************************************************************/

#include<stdlib.h>
#include<string.h>
#include<inttypes.h>

#include"parse.h"

// convert a hex digit to its integer value
// return -1 if not a hex digit
int8_t htoi( char digit )
{
	if( digit <= '9' )
		return digit>='0'? digit-'0' : -1;
	if( digit <= 'F' )
		return digit>='A'? digit-('A'-10) : -1;
	if( digit <= 'f' )
		return digit>='a'? digit-('a'-10) : -1;
	return -1;
}

// parse the given arguments as a rhythm specification: <ID> <PATTERN> <BITS>
// if successful, store the rhythm into the given location
error_t parse_rhythm( int argc, const char *const *argv, rhythm_t *into )
{
	uint8_t bits;
	const uint8_t len = sizeof( into->pattern ) * 2;
	uint8_t i;

	// sanity checks
	if( argc != 3 ) return EARG;

	// convert ID argument
	if( argv[0][1] != '\0' ) return EARG;
	if( ltoi(0) >= MAX_RHYTHM ) return EARG;

	// ensure PATTERN consists only of hex digits
	if( strlen(argv[1]) != len ) return EINVR;
	for( i=0; i<len; ++i )
		if( htoi(argv[1][i]) == -1 ) return EINVR;

	// convert BITS argument
	bits = atoi( argv[2] );
	if( bits<1 || bits>MAX_RBITS ) return EINVR;

	// convert the ascii pattern into native format and store it in the
	// given location
	for( i=0; i<sizeof(into->pattern); ++i ) {
		into->pattern[i] = htoi( argv[1][i*2] ) << 4;
		into->pattern[i] |= htoi( argv[1][i*2+1] );
	}
	into->bits = bits;

	return ESUCCESS;
}

// parse the given arguments as a magnitude specification: <ID> <PERIOD> <DUTY>
// if successful, store the magnitude into the given location
error_t parse_magnitude( int argc, const char *const *argv, magnitude_t *into )
{
	uint16_t period, duty;

	if( argc != 3 ) return EARG;

	// convert arguments to integers
	if( argv[0][1] != '\0' ) return EARG;
	if( ltoi(0) >= MAX_MAGNITUDE ) return EARG;
	period = atoi( argv[1] );
	duty = atoi( argv[2] );

	// ensure minimum duty because PWM TOP cannot be too small
	if( duty>period || duty<2 ) return EINVM;

	into->period = period;
	into->duty = duty;

	return ESUCCESS;
}

// main parser for learning mode commands
// modifies the line argument in place
error_t parse( const parse_step_t *table, char *line )
{
	static const char **argv, *words[ PARSE_MAX_WORDS+1 ];
	parse_step_t step = { "", NULL, NULL };
	int argc = 1;

	if( *line == '\0' ) return EBADCMD;

	*words = line;

	// split the line into words
	for( ; *line; ++line ) {
		if( *line != ' ' ) continue;

		*line = '\0';

		if( argc+1 > PARSE_MAX_WORDS )
			return ETOOBIG;

		words[ argc ] = line + 1;
		++argc;
	}
	words[ argc ] = NULL;

	// walk the table to determine which function to call
	argv = words;
	memcpy_P( &step, table, sizeof(step) );
	while( step.str[0] != '\0' ) {
		if( strcasecmp(step.str, *argv) )
			++table;
		else {
			// this word matched something in the table, so move
			// argv past it and decrement the argument count
			++argv;
			--argc;

			// call the handler if this is the end of the chain
			if( step.next )
				table = step.next;
			else
				return step.func( argc, argv );
		}
		memcpy_P( &step, table, sizeof(step) );
	}

	// no matching command found
	return EBADCMD;
}
