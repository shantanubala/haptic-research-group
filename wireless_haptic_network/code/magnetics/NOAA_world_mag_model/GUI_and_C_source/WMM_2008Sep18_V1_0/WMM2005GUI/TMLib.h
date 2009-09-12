#define PI        3.14159265358979323846L
#define TwoPI     6.28318530717958647692L

/*  Use one of these include statements
#include <math.h>
#include "math.h"
*/

#include <math.h>

double ATanH(double);
double Cot(double);

void GenCoe(double, double *, double *, double *, double *,
         double *, double *);

void AdjCoe(double[], double *, double *);

void TMfwd (double , double, double, double, double[], double, double,
         double, double, double, int, double, double,
          double *, double *, double *, double *);

void TMinv(double, double, double, double, double[], double[], double,
         double, double, double, double, double, int, double, double,
         double *, double *, double *, double *);

void TMfwd4(double, double, double, double, double[], double,
         double, double, double, int, double, double, double *,
         double *, double *, double *);

void TMinv4(double, double, double, double, double[], double[],
         double[], double, double, double, double, int, double,
		 double, double *, double *, double *, double *);

void UTMinv(int, char, double, double, int,
		 double, double, double *, double *, double *,
		 double *);
