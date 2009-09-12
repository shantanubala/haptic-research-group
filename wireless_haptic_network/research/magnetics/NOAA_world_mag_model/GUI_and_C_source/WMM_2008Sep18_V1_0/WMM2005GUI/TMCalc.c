/*  TMCalc  */

#include "TMLib.h"
#include <stdio.h>

/*   Algorithm developed by: C. Rollins   April 18, 2006
     C software written by:  K. Robins                    */

int main ()
   {
   double Pi, deg;
   double Eps, Epssq, invfla, fla, n1, R4oa;
   double A, R4;
   double Acoeff[8], Bcoeff[8], Ccoeff[8], Dcoeff[8];
   int idx;
   double Lon0, Lam0, K0, falseE, falseN;
   int NtrmsA, NtrmsB, NtrmsD;
   int XYonly, LLonly;
   double Lon, Lat, Lambda, Phi;
   double LonDif;
   int GoodLL, GoodXY;
   double X, Y, pscale, CoM;
   double R4star, Xstar, Ystar;
   double CutAgl[4], CutX[4];
   double CMdist;
   char istring[32];

/*
   Some constants for the Clark 1880 and SRM max ellipsoids starting
   with the inverse flattening and meridional isoperimetric radius.
   The other numbers are taken from the forward and inverse error
   bounding curves at error bounds 10**-6, 10**-3, 1, and 10**3 meters
*/
   double Clark[10] = {293.465L, 6367386.643980L,
                  65.0L,  72.9L,  78.1L,  81.6L,
                  11.41E6, 13.95E6, 16.38E6, 18.40E6};

   double SRM[10] = {150.000L, 6378684.503914L,
                  55.1L,  66.1L,  73.5L,  78.3L,
                  9.19E6, 11.71E6, 14.15E6, 16.23E6};

/*   Mathematical constants  */

   Pi = 4 * atan(1.0L);
   deg = Pi / 180;

/*   Title  */

   printf("Transverse Mercator calculator\n");
   printf("---------- -------- ----------\n");

/*   The ellipsoid's inverse flattening  */

   for (;;)
      {
      printf("\nEnter inverse flattening or -999 to quit: ");
      scanf("%s", istring);
      invfla = atof(istring);
      if (invfla == -999) break;

/*   Cut off values for computational error warnings  */

      if (invfla < SRM[0])
         {
         printf("In this program, the inverse flattening " \
               "must be greater than 150\n");
         break;
         }
      else if (invfla < Clark[0])
         {
         printf("Warnings of inaccuracy will be based on "\
               "the SRM max ellipsoid\n");

         for (idx = 0; idx < 4; idx++)
            {
            CutAgl[idx] = SRM[2 + idx];
            CutX[idx] = SRM[6 + idx];
            }

         R4star = SRM[1];
         }
      else
         {
         printf("Warnings of inaccuracy will be based on "\
               "the Clark 1880 ellipsoid\n");

         for (idx = 0; idx < 4; idx++)
            {
            CutAgl[idx] = Clark[2 + idx];
            CutX[idx] = Clark[6 + idx];
            }

         R4star = Clark[1];
         }

/*   The ellipsoid's semi-major axis  */

      printf("\nEnter semi-major axis: ");
      scanf("%s", istring);
      A = atof(istring);

      if (A < 0.0L)
         {
         printf("Semi-major axis must be a positive number\n");
         break;
         }
      else if ((A < 6.3E6) || (A > 6.4E6))
         {
         printf("Accuracy goals 0.001 meters, 1.0 meters etc. " \
               "must be reinterpreted to mean\n");
         printf("0.001 meters etc. per 6.4 million meters of "\
               "semi-major axis\n");
         }

/*   Other ellipsoid constants  */

      GenCoe(invfla, &n1, Acoeff, Bcoeff, Ccoeff, Dcoeff, &R4oa);

      R4 = R4oa * A;
      fla = 1 / invfla;
      Epssq = fla * (2 - fla);
      Eps = sqrt(Epssq);

/*   The map projection  */

      for(;;)
         {
         printf("\nEnter central meridian (deg) or -999 for another "\
               "ellipsoid: ");

         scanf("%s", istring);
         Lon0 = atof(istring);

         if ((int)Lon0 == -999)
            {
            break;
            }

         Lam0 = Lon0 * deg ;
         printf("Enter central scale (scale factor): ");
         scanf("%s", istring);
         K0 = atof(istring);

         printf("Enter false Easting: ");
         scanf("%s", istring);
         falseE = atof(istring);

         printf("Enter false Northing: ");
         scanf("%s", istring);
         falseN = atof(istring);

         NtrmsA = 8;
         NtrmsB = 8;
         NtrmsD = 8;

         XYonly = 0;
         LLonly = 0;

/*   Validity Checking of map projection parameters  */

         if (abs(Lon0) > 180.0L)
            {
            printf("Central meridian longitude is out of range\n");
            break;
            }

         if (K0 < 0)
            {
            printf("Central scale factor K0 must be positive\n");
            break;
            }

/*  A point given by Longitude and Latitude  */

         for (;;)
            {
            printf("\nEnter longitude in degrees or -999 for "\
                  "an inverse calculation: ");

            scanf("%s", istring);
            Lon = atof(istring);

            if ((int)Lon == -999)
               {
               break;
               }

            printf("Enter latitude in degrees: ");
            scanf("%s", istring);
            Lat = atof(istring);

            Lambda = Lon * deg;
            Phi = Lat * deg;

/*   Validity checking of (Lon, Lat)  */

            GoodLL = 1;

            if (abs(Lon) > 180.0L  ||  abs(Lat) > 90.0L)
               {
               printf("Longitude or Latitude is out of range\n");
               GoodLL = 0;
               }
            else
               {
               LonDif = Lon - Lon0;

               if (LonDif > 180.0L) LonDif -= 360.0L;
               if (LonDif <= -180.0L) LonDif += 360.0L;

               CMdist = ((90.0L - abs(Lat)) < abs(LonDif)) ?
                       (90.0L - abs(Lat)): abs(LonDif);
               CMdist = ((180.0L - abs(LonDif)) < CMdist) ?
                       (180.0L - abs(LonDif)): CMdist;

               if (CMdist > CutAgl[3])
                  {
                  printf("Computational error may exceed 1000 m "\
                        "or series may not converge\n");

                  printf("The point is not converted\n");
                  GoodLL = 0;
                  }
               else if (CMdist > CutAgl[2])
                  printf("Computational error may exceed 1.0 m\n");
               else if (CMdist > CutAgl[1])
                  printf("Computaional error may exceed 0.001 m\n");
               else if (CMdist > CutAgl[0])
                  printf("Computaional error may exceed 0.000 001 m\n");
               }

/*   Execution of the forward T.M. algorithm  */

            if (GoodLL == 1)
               {
               TMfwd(Eps, Epssq, R4, R4oa, Acoeff,
                     Lam0, K0, falseE, falseN, NtrmsA,
                     XYonly,
                     Lambda, Phi,
                     &X, &Y, &pscale, &CoM);

               printf("\nEllipsoid   = %18.8lf\t%18.8lf\n", invfla, A);
               printf("C.Merid, K0 = %18.8lf\t%18.8lf\n", Lon0, K0);
               printf("FE, FN      = %18.8lf\t%18.8lf\n", falseE, falseN);
               printf("\nLon, Lat    = %18.8lf\t%18.8lf\n", Lon, Lat);
               printf("(X, Y)      = %18.8lf\t%18.8lf\n", X, Y);
               printf("point-scale = %18.8lf\n", pscale);
               printf("CoM         = %18.8lf\n\n", (CoM / deg));
               }
            }

/*   A point given by Easting and Northing  */

         for (;;)
            {
            printf("\nEnter Easting of point to convert "\
                  "or -999 for new projection parameters: ");

            scanf("%s", istring);
            X = atof(istring);

            if ((int)X == -999)
               {
               break;
               }

            printf("Enter Northing of point: ");
            scanf("%s", istring);
            Y = atof(istring);

/*   Validity checking of (X, Y)  */

            Xstar = (X - falseE) / K0 / R4 * R4star;
            Ystar = (Y - falseN) / K0 / R4 * R4star;

            GoodXY = 1;

            if (Xstar > CutX[3])
               {
               printf("Computational error may exceed "\
                     "equivalent of 1000 m or series "\
                     "may not converge\n");

               printf("The point is not converted\n");
               GoodXY = 0;
               }
            else if (Xstar > CutX[2])
               printf("Computational error may exceed "\
                     "equivalent of 1 m\n");
            else if (Xstar > CutX[1])
               printf("Computational error may exceed "\
                     "equivalent of 0.001 m\n");
            else if (Xstar > CutX[0])
               printf("Computational error may exceed "\
                     "equivalent of 0.000 001 m\n");

/*   Execution of the inverse T.M. algorithm  */

            if (GoodXY == 1)
               {
               TMinv(Eps, Epssq, R4, R4oa, Bcoeff, Dcoeff,
                     Lam0, K0, falseE, falseN, NtrmsB, NtrmsD,
                     LLonly,
                     X, Y,
                     &Lambda, &Phi, &pscale, &CoM);

               Lon = Lambda / deg;
               Lat = Phi / deg;

               printf("\nEllipsoid   = %18.8lf\t%18.8lf\n", invfla, A);
               printf("C.Merid, K0 = %18.8lf\t%18.8lf\n", Lon0, K0);
               printf("FE, FN      = %18.8lf\t%18.8lf\n", falseE, falseN);
               printf("\n(X, Y)      = %18.8lf\t%18.8lf\n", X, Y);
               printf("Lon, Lat    = %18.8lf\t%18.8lf\n", Lon, Lat);
               printf("point-scale = %18.8lf\n", pscale);
               printf("CoM         = %18.8lf\n\n", (CoM / deg));
               }
            }
         }
      }
   printf("Press ENTER after last look at screen\n");
   getc(stdin);
   getc(stdin);
   return 0;
   }
