/*****************************************************************************
 * FILE:   wire_err.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Error codes for the arduino Wire.endTrasmission() function.
 * LOG:    20090425 - initial version
 ****************************************************************************/

#ifndef WIRE_ERR_H
#define WIRE_ERR_H

typedef enum {
	WE_SUCCESS,
	WE_OVERFLOW,
	WE_ANACK,
	WE_DNACK,
	WE_ERROR
} wire_err_t;

#endif
