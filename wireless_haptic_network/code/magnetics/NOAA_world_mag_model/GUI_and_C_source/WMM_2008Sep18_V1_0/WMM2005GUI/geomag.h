
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/* The following include file must define a function 'isnan' */
/* This function, which returns '1' if the number is NaN and 0*/
/* otherwise, could be hand-written if not available. */
/* Comment out one of the two following lines, as applicable */
#include <math.h>               /* for gcc */
//#include <mathimf.h>            /* for Intel icc */

#define NaN log(-1.0)

static void E0000(int IENTRY, int *maxdeg, float alt,float glat,float glon, float time, float *dec, float *dip, float *ti, float *gv);
void geomag(int *maxdeg);
void geomg1(float alt, float glat, float glon, float time, float *dec, float *dip, float *ti, float *gv);
char geomag_introduction(float epochlowlim);