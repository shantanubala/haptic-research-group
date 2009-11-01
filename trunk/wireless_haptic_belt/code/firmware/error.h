/*****************************************************************************
 * FILE:   error.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Type definitions and function declarations for error handling.
 * LOG:    20090501 - initial version
 ****************************************************************************/

#ifndef ERROR_H
#define ERROR_H

/* doing this the correct way with extern "C" makes the Funnel not run
#ifdef __cplusplus
extern "C" {
#endif
*/

// error number definitions--must match the string table in error.c
// in error descriptions, L = learning mode, O = operational mode,
// P = controlling PC, M = main belt controller, V = vibrator controller
typedef enum {
	// symbol	error type (belt mode, command source->destination)
	ESUCCESS,	// no error
	EBADCMD,	// command not recognized		(L/O, P->M)
	ETOOBIG,	// command too long			(L, M->V)
	EARG,		// invalid argument			(L, P->M/M->V)
	ENOR,		// requested rhythm not defined		(O, P->M/M->V)
	ENOM,		// requested magnitude not defined	(O, P->M/M->V)
	ENOS,		// spatio-temporal pattern not defined	(O, P->M/M->V)
	ENOMOTOR,	// requested motor not present on belt	(O, P->M)
	EINVR,		// invalid rhythm definition		(L, P->M/M->V)
	EINVM,		// invalid magnitude definition		(L, P->M/M->V)
	EINVS,		// invalid spatio-temporal definition	(L, P->M)
	EBADVC,		// vibrator command not recognized	(L/O, M->V)
	EBUS,		// I2C communication failed		(L/O, M->V)
	EBUSOF,		// I2C transmit overflow		(L/O, M->V)
	EBUSAN,		// I2C address not acknowledged		(L/O, M->V)
	EBUSDN,		// I2C data not acknowledged		(L/O, M->V)
	EMISSING,	// command not implemented yet		(L, P->M/M->V)
	EMAX		// invalid error number
} error_t;

// return a human-readable error string for the given error number
const char* errstr( error_t num );

/*
#ifdef __cplusplus
} // extern "C"
#endif
*/

#endif
