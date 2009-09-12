/*   Demo3  */

#include "TMLib.h"
#include <stdio.h>

#define nptsLL 32

/*   Algorithm developed by: C. Rollins   April 18, 2006
     C software written by:  K. Robins                    */

int main ()
   {
   double Pi, deg;
   double Eps, Epssq, invfla, fla, n1, R4oa;
   double A, R4;
   double Acoeff[8], Bcoeff[8], Ccoeff[8], Dcoeff[8];
   double Lam0, K0, falseE, falseN;
   int NtrmsA, NtrmsB, NtrmsD;
   int TestId;
   int Lon, Lat;
   double Lambda, Phi;
   int XYonly;
   double X, Y, pscale, CoM;
   int LLonly;
   int TestXY[nptsLL*2];
   int Xkm, Ykm;

/*              xxxx,yyyy,  xxxx,yyyy,  xxxx,yyyy,  xxxx,yyyy,   */
   int TestLL[nptsLL*2] =
             {  40,  50,   -40,  50,   140,  50,  -140,  50,
                40, -50,   -40, -50,   140, -50,  -140, -50,
                 0,   0,    10,   0,    40,   0,    65,   0,
               -10,   0,   -40,   0,    23,  90,   -17, -90,
                 0,  89,    45,  89,   -45,  89,   135,  89,
              -135,  89,   180,  89,  -180,  89,    90,  30,
                 0, -89,    45, -89,   -45, -89,   135, -89,
              -135, -89,   180, -89,  -180, -89,    90, -30};

/*   Announcement  */

   printf(" Generate points to verify implementation of ** less-code ** version\n");
   printf("\n");

/*   Mathematical constants  */

   Pi = 4 * atan(1.0L);
   deg = Pi / 180;

/*   The ellipsoid  */

   invfla = 150.000L;
   A = 6400000L;

   printf(" Ellipsoid of extreme flattening defined by ISO 18026\n");
   printf(" Inverse flattening  = %20.12lf\n", invfla);
   printf(" Semi-major axis     = %20.6lf\n", A);
   printf("\n");

/*   Other ellipsoid constants  */

   GenCoe(invfla, &n1, Acoeff, Bcoeff, Ccoeff, Dcoeff, &R4oa);

   R4 = R4oa * A;
   fla = 1 / invfla;
   Epssq = fla * (2 - fla);
   Eps = sqrt(Epssq);

/*   The map projection  */

   Lam0 = 0;
   K0 = 1;
   falseE = 0;
   falseN = 0;
   NtrmsA = 8;
   NtrmsB = 8;
   NtrmsD = 8;

   printf(" Central meridian    = %20.8lf\n", (Lam0/deg));
   printf(" Central scale       = %20.12lf\n", K0);
   printf(" False Easting       = %20.6lf\n", falseE);
   printf(" False Northing      = %20.6lf\n", falseN);
   printf("\n");

/*   Forward algorithm test plan  */

   printf(" Results of (Lon, Lat) --> (X, Y) conversions: \n");
   printf("\n");
   printf("  Lon   Lat          X                   Y           pt. scale         CoM\n");
   printf("  deg   deg        meter               meter         (no unit)         deg\n");
   printf("  ---   ---        -----               -----         --  -----         ---\n");
   printf("\n");

/*   Prepare input for forward tests  */

   for (TestId = 0; TestId < nptsLL; TestId++)
      {
      Lon = TestLL[2*TestId];
      Lat = TestLL[2*TestId + 1];
      Lambda = Lon * deg;
      Phi = Lat * deg;

/*   Execution of the forward T.M. algorithm  */

      XYonly = 0;

      TMfwd(Eps, Epssq, R4, R4oa, Acoeff,
            Lam0, K0, falseE, falseN, NtrmsA,
            XYonly,
            Lambda, Phi,
            &X, &Y, &pscale, &CoM);

/*   Report results of forward tests  */

      printf("%5d %4d %19.8lf %19.8lf %12.9lf %14.8lf\n",
             Lon, Lat, X, Y, pscale, (CoM/deg));

/*   Save truncated output for later inverse tests  */

      TestXY[2*TestId] = X / 1000;
      TestXY[2*TestId + 1]     = Y / 1000;
      }

   printf("\n");

/*   Prepare input for the inverse tests  */

   printf(" Results of (X, Y) --> (Lon, Lat) conversions: \n");
   printf("\n");
   printf("    X      Y           Lon               Lat         pt. scale         CoM\n");
   printf("    km     km          deg               deg         (no unit)         deg\n");
   printf("    --     --          ---               ---         --  -----         ---\n");
   printf("\n");

   for (TestId = 0; TestId < nptsLL; TestId++)
      {
      Xkm = TestXY[2*TestId];
      Ykm = TestXY[2*TestId + 1];

      X = Xkm * 1000.0;
      Y = Ykm * 1000.0;

/*  Execution of the inverse T.M. algorithm  */

      LLonly = 0;

      TMinv(Eps, Epssq, R4, R4oa, Bcoeff, Dcoeff,
            Lam0, K0, falseE, falseN, NtrmsB, NtrmsD,
            LLonly,
            X, Y,
            &Lambda, &Phi, &pscale, &CoM);

/*  Report results of inverse tests  */

      printf("%7d %6d %17.12lf %17.12lf %12.9lf %14.8lf\n",
             Xkm, Ykm, (Lambda/deg), (Phi/deg), pscale, (CoM/deg));
      }
   printf("\n");
   printf(" Press ENTER after last look at screen\n");
   getc(stdin);
   return 0;
   }
