/*****************************************************************************
 * FILE:   learn_tiny.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Declaration for the learn parser in the tiny.
 * LOG:    20090423 - initial version
 ****************************************************************************/

#ifndef LEARN_TINY_H
#define LEARN_TINY_H

#include"parse.h"

// modifies the cmd argument in place
error_t handle_learn( char *cmd );

#endif
