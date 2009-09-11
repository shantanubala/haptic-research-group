/*****************************************************************************
 * FILE:   main.c
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Main code for the Funnel I/O.
 * LOG:    20090510 - initial version
 ****************************************************************************/

#include <Wire.h>
#include <EEPROM.h>
#include <stdlib.h>
#include <ctype.h>
//#include <avr/eeprom.h>	// arduino apparently breaks this

#include "error.h"
#include "parse.h"
#include "rhythm.h"
#include "magnitude.h"
#include "vibration.h"
#include "wire_err.h"
#include "globals_main.h"
#include "menu.h"

// because arduino is broken...
#include "error.c"
#include "parse.c"

// how long to wait for a status response from a motor, in ms
#define TWI_TIMEOUT 100

// how long to wait for vibrator modules to stabilize
#define TINY_WAIT 1000

// offsets into the EEPROM of the magnitudes and rhythms
#define EE_MAG ((magnitude_t*)0)
#define EE_RHY ((rhythm_t*)(EE_MAG + MAX_MAGNITUDE))

// if DEBUG is defined, compile in detailed runtime status messages
//#define DEBUG
#ifdef DEBUG
#	define DBGC( ... ) Serial.print( __VA_ARGS__ )
#	define DBGCN( ... ) Serial.println( __VA_ARGS__ )
#	define DBG( ... ) \
		do{ Serial.print("DBG "); Serial.print(__VA_ARGS__); }while(0)
#	define DBGN( ... ) \
		do{Serial.print("DBG ");Serial.println(__VA_ARGS__);}while(0)
#else
#	define DBG( ... )
#	define DBGN( ... )
#	define DBGC( ... )
#	define DBGCN( ... )
#endif

// Funnel globals, defined in globals_main.h
globals_t glbl;

void print_flash( const prog_char* );

// read a chunk of data from the EEPROM
// only because arduino is broken and the avr-libc eeprom_*() don't work
static inline void eeprom_read( void* into, void* from, size_t len )
{
	size_t i;
	for( i=0; i<len; ++i )
		*((uint8_t*)into + i) = EEPROM.read( (size_t)from + i );
}

// write a chunk of data to the EEPROM
static inline void eeprom_write( void* into, void* from, size_t len )
{
	size_t i;
	for( i=0; i<len; ++i )
		EEPROM.write( (size_t)into+i, *((uint8_t*)from+i) );
}

// zero a chunk the EEPROM
static inline void eeprom_zero( void* start, void* end )
{
	while( start < end ) {
		EEPROM.write( (size_t)start, 0 );
//		eeprom_write_byte( (uint8_t*)start, 0 );
		start = (uint8_t*)start + 1;
	}
}

// convert an unsigned byte to two hex digits plus null terminator
void itoh( char *into, uint8_t val )
{
	into[0] = val >> 4;
	into[0] += ( into[0]<10? '0' : 'A'-10 );
	into[1] = val & 0xf;
	into[1] += ( into[1]<10? '0' : 'A'-10 );
	into[2] = '\0';
}

// send the command in the global command buffer to the specified motor
// if motor is specified as -1, send command to all motors via a general call
error_t send_command( int8_t motor )
{
	unsigned long start;
	uint8_t status;

	// make sure the requested motor is present
	if( motor >= MAX_MOTORS ) return ENOMOTOR;

	if( motor < 0 )
		motor = 0;	// general call
	else {
		// find the actual TWI address of the given motor
		motor = glbl.mtrs[ motor ].addr;
		if( !motor ) return ENOMOTOR;
	}

	// send the command over TWI
	// choose which buffer to send based on the current belt mode
	Wire.beginTransmission( motor );
	if( glbl.mode == M_ACTIVE )
		Wire.send( *((uint8_t*)&glbl.acmd.v) );
	else	// learning mode
		Wire.send( glbl.cmd );
	status = Wire.endTransmission();

	// if the TWI transmission failed, return a specific bus error status
	if( status ) {
		if( status >= 4 )
			return EBUS;
		else	return (error_t)(EBUS + status);
	}

	// must not try to request data with a general call...
	// FIXME? anything else to do?
	if( motor == 0 ) return ESUCCESS;

	// without a delay the tiny misses the status request...
	// wait for at most TWI_TIMEOUT milliseconds
	for( start=millis(); millis()-start < TWI_TIMEOUT; )
		if( Wire.requestFrom((uint8_t)motor, (uint8_t)1) )
			return (error_t)Wire.receive();

	return EBUS;
}

// send a command to each attached motor in turn
// mark any motor that returns an error
// FIXME: do something when an error occurs
error_t send_command_all( void )
{
	uint8_t i, errors = 0;

	for( i=0; glbl.mtrs[i].addr; ++i ) {
		error_t ret = send_command(i);
		glbl.mtrs[i].err = ret != ESUCCESS;
		errors |= glbl.mtrs[i].err;
		DBGC(" ");
		DBGC( ret, DEC );
	}
	DBGCN("");

	if( errors ) return EBUS;

	return ESUCCESS;
}

// detect which motors are present on the bus
void detect_motors( void )
{
	uint8_t i, j = 0;

	// send a command on every address
	// if the command is ACKed, then a motor is present
	for( i=1; i<128 && j<MAX_MOTORS; ++i ) {
		wire_err_t ret;

		Wire.beginTransmission(i);
		Wire.send(0);
		ret = (wire_err_t)Wire.endTransmission();

		if( ret != WE_ANACK ) {
			glbl.mtrs[j].addr = i;
			if( ret != WE_SUCCESS )
				glbl.mtrs[j].err = 1;
			++j;
		}
	}

	// print a debug message that shows addresses of all detected motors
	DBG( j, DEC ); DBGC( " motors:" );
	for( int i=0; i<j; ++i ) {
		DBGC( " " );
		DBGC( glbl.mtrs[i].addr, HEX );
	}
	DBGCN("");
}

// generate an ASCII representation of a rhythm
error_t rtos( char *into, uint8_t which )
{
		rhythm_t rhy;
		uint8_t i;

		// retrieve the specified rhythm from EEPROM
		eeprom_read( &rhy, EE_RHY+which, sizeof(rhy) );
		if( !rhy.bits || rhy.bits>MAX_RBITS )
			return ENOR;

		strcpy( into, "RHY " );
		into += 4;
		*into++ = itol( which );
		*into++ = ' ';
		for( i=0; i<sizeof(rhy.pattern); ++i, into+=2 )
			itoh( into, rhy.pattern[i] );
		*into++ = ' ';
		utoa( rhy.bits, into, 10 );

		return ESUCCESS;
}

// generate an ASCII representation of a magnitude
error_t mtos( char *into, uint8_t which )
{
		magnitude_t mag;

		// retrieve the specified magnitude from EEPROM
		eeprom_read( &mag, EE_MAG+which, sizeof(mag) );
		if( !mag.period ) return ENOM;

		strcpy( into, "MAG " );
		into += 4;
		*into++ = itol( which );
		*into++ = ' ';
		utoa( mag.period, into, 10 );
		into += strlen( into );
		*into++ = ' ';
		utoa( mag.duty, into, 10 );

		return ESUCCESS;
}

// load all rhythms and magnitudes from EEPROM and relay them to all motors
void teach_motors( void )
{
	uint8_t i;

	DBGN( "teaching rhythms" );
	strcpy( glbl.cmd, "LRN " );
	for( i=0; i<MAX_RHYTHM; ++i ) {
		if( rtos(glbl.cmd+4, i) != ESUCCESS )
			continue;
		DBG( glbl.cmd );
		send_command_all();
	}

	DBGN( "teaching magnitudes" );
	for( i=0; i<MAX_MAGNITUDE; ++i ) {
		if( mtos(glbl.cmd+4, i) != ESUCCESS )
			continue;
		DBG( glbl.cmd );
		send_command_all();
	}
}

// read a single character from the serial link
// busy wait until a character arrives, and convert '\r' to '\n'
// in menu mode, echo the character back
static inline char read_char( uint8_t echo )
{
	char ch;

	while( !Serial.available() );

	ch = toupper( Serial.read() );
	if( echo ) {
		Serial.print(ch);
		if( ch == '\r' ) Serial.print( '\n' );
	}

	return ch=='\r'? '\n' : ch;
}

static inline void print_status( error_t status )
{
	Serial.print( "STS " );
	Serial.println( status, DEC );
}

// read a line of text from serial link into global ascii command buffer
void read_line( void )
{
	uint8_t i;
	char ch;

	// erghhh...
	do{
		// read characters until a newline
		i = 0;
		while( (ch=read_char(glbl.echo)) != '\n' )
			if( i < sizeof(glbl.cmd) )
				glbl.cmd[ i++ ] = ch;

		// if the line was too long, return an error
		if( i >= sizeof(glbl.cmd) ) {
			if( glbl.echo )
				print_flash( errstr(ETOOBIG) );
			else
				print_status( ETOOBIG );
		}
	}while( i >= sizeof(glbl.cmd) );

	glbl.cmd[i] = '\0';
}

// read two raw bytes from serial link into global activate command buffer
void read_active( void )
{
	uint8_t i = 0;

	while( i < sizeof(active_command_t) )
		if( Serial.available() )
			*((uint8_t*)&glbl.acmd + i++) = Serial.read();
}

// read four hex digits from serial link into global activate command buffer
void read_active_hex( void )
{
	uint8_t *into = (uint8_t*)&glbl.acmd, i = 0;

	while( i++ < sizeof(active_command_t) )
		*into++ = htoi(read_char(1))<<4 | htoi(read_char(1));
}

// command handlers
error_t learn_rhythm( int argc, const char *const *argv )
{
	rhythm_t rhy;
	error_t ret;

	// parse the rhythm and store it in EEPROM
	ret = parse_rhythm( argc, argv, &rhy );
	if( ret != ESUCCESS ) return ret;

	eeprom_write( EE_RHY+(ltoi(0)), &rhy, sizeof(rhy) );

	// relay the learn to all connected motors
	DBG( "relaying rhythm:" );
	send_command_all();

	return ret;
}

error_t learn_magnitude( int argc, const char *const *argv )
{
	magnitude_t mag;
	error_t ret;

	// parse the magnitude and store it in EEPROM
	ret = parse_magnitude( argc, argv, &mag );
	if( ret != ESUCCESS ) return ret;

	eeprom_write( EE_MAG+(ltoi(0)), &mag, sizeof(mag) );

	// relay the learn to all connected motors
	DBG( "relaying magnitude:" );
	send_command_all();

	return ret;
}

error_t learn_spatio( int argc, const char *const *argv )
{ return EMISSING; }

error_t learn_address( int argc, const char *const *argv )
{ return EMISSING; }

error_t query_generic( int argc, const char *const *argv,
	uint8_t max, error_t (*func)( char*, uint8_t )
) {
	uint8_t start, finish;

	if( argc > 1 ) return EARG;

	if( argc > 0 ) {
		start = ltoi(0);
		finish = start + 1;
		if( start >= max )
			return EARG;
	}else {
		start = 0;
		finish = max;
	}

	strcpy( glbl.cmd, "RSP " );
	while( start < finish ) {
		if( func(glbl.cmd+4, start) == ESUCCESS )
			Serial.println( glbl.cmd );
		++start;
	}

	return ESUCCESS;
}

error_t query_rhythm( int argc, const char *const *argv )
{ return query_generic( argc, argv, MAX_RHYTHM, rtos ); }

error_t query_magnitude( int argc, const char *const *argv )
{ return query_generic( argc, argv, MAX_MAGNITUDE, mtos ); }

error_t query_spatio( int argc, const char *const *argv )
{ return EMISSING; }

error_t query_motors( int argc, const char *const *argv )
{
	uint8_t i;

	if( argc ) return EARG;

	strcpy( glbl.cmd, "RSP MTR " );
	for( i=0; glbl.mtrs[i].addr; ++i );
	utoa( i, glbl.cmd+8, 10 );

	Serial.println( glbl.cmd );

	return ESUCCESS;
}

error_t query_version( int argc, const char *const *argv )
{
	if( argc ) return EARG;

	Serial.println( "RSP VER " FUNNEL_VER );

	return ESUCCESS;
}

error_t query_all( int argc, const char *const *argv )
{
	if( argc ) return EARG;

	query_version( 0, NULL );
	query_motors( 0, NULL );
	query_rhythm( 0, NULL );
	query_magnitude( 0, NULL );

	return ESUCCESS;
}

error_t test( int argc, const char *const *argv )
{ return EMISSING; }

error_t begin( int argc, const char *const *argv )
{
	if( argc ) return EARG;

	glbl.mode = M_ACTIVE;

	return ESUCCESS;
}

error_t erase_all_learned( int argc, const char *const *argv )
{
	if( argc != 3 ) return EARG;

	eeprom_zero( EE_MAG, EE_RHY+MAX_RHYTHM );

	return ESUCCESS;
}

// parse table definitions
static const parse_step_t pt_learn[] PROGMEM = {
	{ "RHY", NULL, learn_rhythm },
	{ "MAG", NULL, learn_magnitude },
	{ "SPT", NULL, learn_spatio },
	{ "ADD", NULL, learn_address },
	{ "", NULL, NULL }
};

static const parse_step_t pt_query[] PROGMEM = {
	{ "RHY", NULL, query_rhythm },
	{ "MAG", NULL, query_magnitude },
	{ "SPT", NULL, query_spatio },
	{ "MTR", NULL, query_motors },
	{ "VER", NULL, query_version },
	{ "ALL", NULL, query_all },
	{ "", NULL, NULL }
};

static const parse_step_t pt_top[] PROGMEM = {
	{ "LRN", pt_learn, NULL },
	{ "QRY", pt_query, NULL },
	{ "TST", NULL, test },
	{ "BGN", NULL, begin },
	{ "ZAP", NULL, erase_all_learned },
	{ "", NULL, NULL }
};

error_t parse_active( void )
{
	// FIXME
	switch( glbl.acmd.mode ) {
	case ACM_VIB:	return send_command( glbl.acmd.motor );
	case ACM_SPT:	return EMISSING;
	case ACM_GCL:	return send_command(-1);
	case ACM_LRN:	glbl.mode = M_LEARN;
			return ESUCCESS;
	default:	return EBADCMD;
	}
}

error_t handle_learn( void )
{
	char cpy[ sizeof(glbl.cmd) ];

	// make a copy of the command, because parse() modifies its argument
	// in place and the learn handlers expect an unmodified glbl.cmd
	strcpy( cpy, glbl.cmd );

	return parse( pt_top, cpy );
}

// menu tables
static const char menu_str_top[] PROGMEM =
	"0. Exit menu\n\r"
	"1. Query commands\n\r"
	"2. Learn commands\n\r"
	"3. Activate a motor\n\r"
	"4. Raw command entry\n\r"
;

static const char menu_str_qry[] PROGMEM =
	"0. Return to main menu\n\r"
	"1. Query belt version\n\r"
	"2. Query number of motors present\n\r"
	"3. Query defined rhythms\n\r"
	"4. Query defined magnitudes\n\r"
	"5. Query all belt configuration\n\r"
;

static const char menu_str_lrn[] PROGMEM =
	"0. Return to main menu\n\r"
	"1. Learn rhythm\n\r"
	"2. Learn magnitude\n\r"
	"3. Forget all rhythms and magnitudes\n\r"
;

static const char menu_str_lrn_rhy[] PROGMEM =
	"Enter rhythm ID, pattern, and number of bits, then press ENTER.\n\r"
	"Press ENTER on a blank line when finished.\n\r"
	"\n\r"
	"The rhythm pattern consists of 16 hexadecimal digits. Each bit\n\r"
	"of the pattern represents 50 ms of the rhythm. If a bit is set,\n\r"
	"the motor will vibrate for the corresponding 50 ms during rhythm\n\r"
	"playback; if a bit is cleared, those 50 ms will elapse without\n\r"
	"any vibration.\n\r"
	"\n\r"
	"The number of bits argument specifies how many of the 64 bits\n\r"
	"specified by the pattern are actually used in the rhythm.\n\r"
	"\n\r"
	"Example: A F0C1F00000000000 17<ENTER> defines rhythm A to be\n\r"
	"17 * 50 ms = 850 ms long: 200 ms ON, 200 ms OFF, 100 ms ON, 250\n\r"
	"ms OFF, and finally ON for the last 100 ms.\n\r"
;

static const char menu_str_lrn_mag[] PROGMEM =
	"Enter magnitude ID, period, and pulse width, in microseconds,\n\r"
	"then press ENTER. Press ENTER on a blank line when finished.\n\r"
	"\n\r"
	"To specify full ON (digital 1), enter the same number for both\n\r"
	"period and pulse width.\n\r"
	"\n\r"
	"Example: C 2000 500<ENTER> defines magnitude C to have a 2 ms\n\r"
	"period with 25% duty cycle.\n\r"
;

static const char menu_str_forget[] PROGMEM =
	"All defined rhythms and magnitudes will be erased from EEPROM.\n\r"
	"Continue?\n\r"
	"0. No\n\r"
	"1. Yes\n\r"
;

static const char menu_str_act[] PROGMEM =
	"Enter motor, rhythm, magnitude, and duration, then press ENTER.\n\r"
	"Press ENTER on a blank line when finished.\n\r"
	"\n\r"
	"Example: CED6<ENTER> will activate the third motor for 6 cycles\n\r"
	"of rhythm E at magnitude D.\n\r"
;

// menu handlers
error_t menu_qry_ver( void ) { return query_version(0, NULL); }
error_t menu_qry_mtr( void ) { return query_motors(0, NULL); }
error_t menu_qry_rhy( void ) { return query_rhythm(0, NULL); }
error_t menu_qry_mag( void ) { return query_magnitude(0, NULL); }
error_t menu_qry_all( void ) { return query_all(0, NULL); }
void menu_lrn_generic( const char *prepend )
{
	while(1) {
		// read the rhythm/magnitude specification from the user
		Serial.print( "Specification: " );
		read_line();
		if( glbl.cmd[0] == '\0' ) break;

		// prepend the LRN RHY/MAG
		memmove( glbl.cmd+8, glbl.cmd, sizeof(glbl.cmd)-8 );
		glbl.cmd[ sizeof(glbl.cmd) - 1 ] = '\0';
		memcpy( glbl.cmd, "LRN ", 4 );
		memcpy( glbl.cmd+4, prepend, 4 );

		// handle the command...should always be in learning mode here
		// so not necessary to change glbl.mode
		print_flash( errstr(handle_learn()) );
	}
}
error_t menu_lrn_rhy( void )
{
	print_flash( menu_str_lrn_rhy );
	menu_lrn_generic( "RHY " );
	return ESUCCESS;
}
error_t menu_lrn_mag( void )
{
	print_flash( menu_str_lrn_mag );
	menu_lrn_generic( "MAG " );
	return ESUCCESS;
}
error_t menu_lrn_forget( void ) { return erase_all_learned(3, NULL); }
#define ARG( _dst_, _base_, _max_ ) \
	val = glbl.cmd[ i++ ] - _base_; \
	if( val >= _max_ ) { \
		Serial.println( "Invalid command" ); \
		continue; \
	} \
	_dst_ = val;
error_t menu_act( void )
{
	print_flash( menu_str_act );

	while(1) {
		uint8_t val, i = 0;	// used by the ARG macro
		mode_t save;

		// read the command from the user
		Serial.print( "Command: " );
		read_line();
		if( glbl.cmd[0] == '\0' ) break;

		// convert it into a native activate command
		ARG( glbl.acmd.motor, 'A', MAX_MOTORS );
		ARG( glbl.acmd.v.rhythm, 'A', MAX_RHYTHM );
		ARG( glbl.acmd.v.magnitude, 'A', MAX_MAGNITUDE );
		ARG( glbl.acmd.v.duration, '0', MAX_DURATION+1 );
		glbl.acmd.mode = 0;

		// send the command
		save = glbl.mode;
		glbl.mode = M_ACTIVE;
		print_flash( errstr(send_command(glbl.acmd.motor)) );
		glbl.mode = save;
	}

	return ESUCCESS;
}
#undef ARG
error_t menu_raw( void ) { glbl.echo = 1; glbl.in_menu = 0; return ESUCCESS; }
error_t menu_exit( void ) { glbl.in_menu = glbl.echo = 0; return ESUCCESS; }

// "forward declarations" of menu steps not yet defined
extern const menu_step_t menu_choices_top[] PROGMEM;
extern const menu_step_t menu_choices_lrn[] PROGMEM;

// special value to mark the end of a set of menu choices
error_t menu_end( void ) { return EMAX; }

static const menu_step_t menu_choices_qry[] PROGMEM = {
	{ menu_str_top, menu_choices_top, NULL },
	{ NULL, NULL, menu_qry_ver },
	{ NULL, NULL, menu_qry_mtr },
	{ NULL, NULL, menu_qry_rhy },
	{ NULL, NULL, menu_qry_mag },
	{ NULL, NULL, menu_qry_all },
	{ NULL, NULL, menu_end }
};

static const menu_step_t menu_choices_forget[] PROGMEM = {
	{ menu_str_lrn, menu_choices_lrn, NULL },
	{ menu_str_lrn, menu_choices_lrn, menu_lrn_forget },
	{ NULL, NULL, menu_end }
};

const menu_step_t menu_choices_lrn[] PROGMEM = {
	{ menu_str_top, menu_choices_top, NULL },
	{ NULL, NULL, menu_lrn_rhy },
	{ NULL, NULL, menu_lrn_mag },
	{ menu_str_forget, menu_choices_forget, NULL },
	{ NULL, NULL, menu_end }
};

const menu_step_t menu_choices_top[] PROGMEM = {
	{ NULL, NULL, menu_exit },
	{ menu_str_qry, menu_choices_qry, NULL },
	{ menu_str_lrn, menu_choices_lrn, NULL },
	{ NULL, NULL, menu_act },
	{ NULL, NULL, menu_raw },
	{ NULL, NULL, menu_end }
};

static const menu_step_t menu_top[] PROGMEM = {
	{ menu_str_top, menu_choices_top, NULL },
	{ NULL, NULL, menu_end }
};

void handle_menu( void )
{
	menu_step_t step;
	int8_t max, sel;
	error_t status;

	glbl.in_menu = 1;
	glbl.echo = 1;

	while( glbl.in_menu ) {
		const menu_step_t *choice = glbl.menustep.choices;

		// display the current menu
		Serial.println();
		print_flash( glbl.menustep.menu );

		// figure out the number of choices
		// FIXME: (int) should be (uint16_t)
		max = 0;
		while( pgm_read_word(&choice[max].func) != (int)menu_end )
			++max;

		// wait for a valid selection
		Serial.print( "Choice: " );
		for( sel=-1; sel<0 || sel>=max; sel=read_char(0)-'0' );
		Serial.println( sel, DEC );

		// load the selected menu step into RAM
		memcpy_P( &step, choice+sel, sizeof(step) );

		// call the handler function if defined
		// do this even if a submenu is specified, so that e.g. a
		// confirmation menu can specify what menu to return to
		status = ESUCCESS;
		if( step.func != NULL ) {
			Serial.println();
			status = step.func();
		}

		// if a submenu is defined in the tables, move into it
		if( step.choices != NULL )
			glbl.menustep = step;
		else if( step.func == NULL ) {
			Serial.println();
			status = EMISSING;
		}

		if( status != ESUCCESS )
			print_flash( errstr(status) );
	}

	// empty the command/response buffer so that the main loop doesn't
	// try to execute whatever happens to be in there when we return
	glbl.cmd[0] = '\0';
}

void setup( void )
{
        unsigned long start;
        
	Wire.begin();
	Serial.begin( 9600 );

	memset( &glbl, 0, sizeof(glbl) );

        //need to wait for vibrator microprocessor to stabalize for detection function
        for( start=millis(); millis()-start < TINY_WAIT; )
        
	detect_motors();	// determine which motors are present on bus

	teach_motors();	// relay rhythms and magnitudes to attached motors

	// initialize the menu to the top level
	memcpy_P( &glbl.menustep, menu_top, sizeof(glbl.menustep) );
}

void loop( void )
{
	error_t status;

	if( glbl.mode == M_ACTIVE ) {
		if( glbl.echo ) {
			// raw command mode; read the command as ASCII hex
			// and print human-readable status response
			read_active_hex();
			Serial.print( ' ', BYTE );
			print_flash( errstr(parse_active()) );
		}else {
			// normal mode; read and reply machine-format bytes
			read_active();
			Serial.print( parse_active(), BYTE );
		}
	}else {
		uint8_t count = 0;

		do{
			read_line();
			if( count < 2 )
				++count;
			else {
				handle_menu();
				count = 0;
			}
		}while( glbl.cmd[0] == '\0' );
		DBGN( glbl.cmd );

		// handle the command and return status
		status = handle_learn();

		if( glbl.echo ) {
			// raw command mode; print a pretty status message
			print_flash( errstr(status) );
		}else {
			// normal mode; return parsable status
			print_status( status );
		}
	}
}

/*Author: Kristopher Blair (Haptic-Research-Group)
 *Date: April 22nd, 2009
 *
 *Description: Menu display for Arduino controller, for use with
 *haptic belt research project, Arizona State University.
 *
 */
void print_flash(const prog_char *str)
{ 
	char c;
	if(!str) return;
	while((c = pgm_read_byte(str++))) Serial.print(c,BYTE);
	Serial.println();
}
