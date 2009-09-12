/*   Demo1  */

#include "TMLib.h"
#include <stdio.h>

/*   Algorithm developed by: C. Rollins   April 18, 2006
     C software written by:  K. Robins                   */

int main ()
   {
   double Pi, deg;
   double Eps, Epssq, invfla, fla, n1, R4oa;
   double A, R4;
   double Acoeff[8], Bcoeff[8], Ccoeff[8], Dcoeff[8];
   double Lam0, K0, falseE, falseN;
   int NtrmsA, NtrmsB, NtrmsD;
   double Lambda, Phi;
   int XYonly;
   double X, Y, pscale, CoM;
   int LLonly;

/*   Announcement  */

   printf(" Single point demo of ** less-code ** version\n");
   printf("\n");

/*   Mathematical constants  */

   Pi = 4 * atan(1.0L);
   deg = Pi / 180;

/*   The ellipsoid (e.g. WGS84)  */

   invfla = 298.257223563000L;
   A = 6378137.000L;

   printf(" WGS84 ellipsoid\n");
   printf(" Inverse flattening  = %19.12lf\n", invfla);
   printf(" Semi-major axis     =    %lf\n", A);
   printf("\n");

/*   Other ellipsoid constants  */

   GenCoe(invfla, &n1, Acoeff, Bcoeff, Ccoeff, Dcoeff, &R4oa);

   R4 = R4oa * A;
   fla = 1 / invfla;
   Epssq = fla * (2 - fla);
   Eps = sqrt(Epssq);

/*   The map projection (e.g. UTM Zone 31 South)  */

   Lam0 = 3 * deg;
   K0 = 0.9996000L;
   falseE = 500000;
   falseN = 10000000;
   NtrmsA = 8;
   NtrmsB = 8;
   NtrmsD = 8;

   printf(" Central meridian    =    %lf\n", (Lam0/deg));
   printf(" Central scale       =    %lf\n", K0);
   printf(" False Easting       =    %lf\n", falseE);
   printf(" False Northing      =    %lf\n", falseN);
   printf("\n");

/*   A point given by Longitude and Latitude  */

   Lambda = 39 * deg;
   Phi = -54 * deg;
   printf(" Forward test\n");
   printf(" Lon, Lat =    %lf \t %lf\n", (Lambda/deg), (Phi/deg));

/*   Execution of the forward T.M. algorithm  */

   XYonly = 0;

   TMfwd(Eps, Epssq, R4, R4oa, Acoeff,
        Lam0, K0, falseE, falseN, NtrmsA,
        XYonly, Lambda, Phi,
        &X, &Y, &pscale, &CoM);

   printf(" (X, Y)   =    %lf \t %lf\n", X, Y);
   printf(" pt-scale =    %lf\n", pscale);
   printf(" CoM      =    %lf\n", (CoM/deg));
   printf("\n");

/*   A point given by Easting and Northing  */

   X = 2800000;
   Y = 3395000;
   printf(" Inverse test\n");
   printf(" (X, Y)   =    %lf \t %lf\n", X, Y);

/*   Execution of the inverse T.M. algorithm  */

   LLonly = 0;

   TMinv(Eps, Epssq, R4, R4oa, Bcoeff, Dcoeff,
        Lam0, K0, falseE, falseN, NtrmsB, NtrmsD,
        LLonly, X, Y,
        &Lambda, &Phi, &pscale, &CoM);

   printf(" Lon, Lat =    %lf \t %lf\n", (Lambda/deg), (Phi/deg));
   printf(" pt-scale =    %lf\n", pscale);
   printf(" CoM      =    %lf\n", (CoM/deg));
   printf("\n");

   printf(" Press ENTER after last look at screen\n");
   getc(stdin);
   return 0;
   }
