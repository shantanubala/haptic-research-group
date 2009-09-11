/*****************************************************************************
 * FILE:   menu.h
 * AUTHOR: Jon Lindsay (Jonathan.Lindsay@asu.edu)
 * DESCR:  Type definitions for the menu parser in menu.c.
 * LOG:    20090510 - initial version
 ****************************************************************************/

#ifndef MENU_H
#define MENU_H

// menu callback function type
typedef error_t (*menu_func_t)( void );

// menu table constituent type
typedef struct menu_step_s {
	const char *menu;
	const struct menu_step_s *choices;
	menu_func_t func;
} menu_step_t;

#endif
