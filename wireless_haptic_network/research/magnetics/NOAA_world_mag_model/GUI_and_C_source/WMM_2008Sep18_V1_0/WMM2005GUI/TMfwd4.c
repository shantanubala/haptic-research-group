/*   TMfwd4  */

#include "TMLib.h"


void TMfwd4(double Eps, double Epssq, double K0R4, double K0R4oa,
         double Acoeff[], double Lam0, double K0, double falseE,
         double falseN, int XYonly, double Lambda, double Phi,
         double *X, double *Y, double *pscale, double *CoM)
   {

/*  Transverse Mercator forward equations including point-scale and CoM
    =--------- =------- =--=--= ---------
 
   Algorithm developed by: C. Rollins   August 7, 2006
   C software written by:  K. Robins
 
 
   Constants fixed by choice of ellipsoid, choice of projection param.s
   --------- -----
 
      Eps          Eccentricity (epsilon) of the ellipsoid
      Epssq        Eccentricity squared
    ( R4           Meridional isoperimetric radius   )
    ( K0           Central scale factor              )
      K0R4         K0 times R4
      K0R4oa       K0 times Ratio of R4 over semi-major axis
      Acoeff       Trig series coefficients, omega as a function of chi
      Lam0         Longitude of the central meridian in radians
      K0           Central scale factor, for example, 0.9996 for UTM
      falseE       False easting, for example, 500000 for UTM
      falseN       False northing
                
   Processing option
   ---------- ------
 
      XYonly       If one (1), then only X and Y will be properly
                   computed.  Values returned for point-scale
                   and CoM will merely be the trivial values for
                   points on the central meridian
 
   Input items that identify the point to be converted
   ----- -----
 
      Lambda       Longitude (from Greenwich) in radians
      Phi          Latitude in radians
 
   Output items
   ------ -----
 
      X            X coordinate (Easting) in meters
      Y            Y coordinate (Northing) in meters
      pscale       point-scale (dimensionless)
      CoM          Convergence-of-meridians in radians
*/

   double Lam, CLam, SLam, CPhi, SPhi;
   double P, part1, part2, denom, CChi, SChi;
   double U, V;
   double T, Tsq, denom2;
   double c2u, s2u, c4u, s4u, c6u, s6u, c8u, s8u;
   double c2v, s2v, c4v, s4v, c6v, s6v, c8v, s8v;
   double Xstar, Ystar;
   double sig1, sig2, comroo;

/* 
   Ellipsoid to sphere
   --------- -- ------
    
   Convert longitude (Greenwhich) to longitude from the central meridian
   It is unnecessary to find the (-Pi, Pi] equivalent of the result.
   Compute its cosine and sine.  
*/

   Lam = Lambda - Lam0;
   CLam = cos(Lam);
   SLam = sin(Lam);

/*   Latitude  */

   CPhi = cos(Phi);
   SPhi = sin(Phi);

/*   Convert geodetic latitude, Phi, to conformal latitude, Chi
     Only the cosine and sine of Chi are actually needed.        */

   P = exp(Eps * ATanH(Eps * SPhi));
   part1 = (1 + SPhi) / P;
   part2 = (1 - SPhi) * P;
   denom = 1 / (part1 + part2);
   CChi = 2 * CPhi * denom;
   SChi = (part1 - part2) * denom ;

/*
   Sphere to first plane
   ------ -- ----- -----
 
   Apply spherical theory of transverse Mercator to get (u,v) coord.s
   Note the order of the arguments in Fortran's version of ArcTan, i.e.
             atan2(y, x) = ATan(y/x)
   The two argument form of ArcTan is needed here.
*/

   T = CChi * SLam;
   U = ATanH(T);
   V = atan2(SChi, CChi * CLam);

/*
   Trigonometric multiple angles
   ------------- -------- ------
 
   Compute Cosh of even multiples of U
   Compute Sinh of even multiples of U
   Compute Cos  of even multiples of V
   Compute Sin  of even multiples of V
*/

   Tsq = T * T;
   denom2 = 1 / (1 - Tsq);
   c2u = (1 + Tsq) * denom2;
   s2u = 2 * T * denom2;
   c2v = (-1 + CChi * CChi * (1 + CLam * CLam)) * denom2;
   s2v = 2 * CLam * CChi * SChi * denom2;

   c4u = 1 + 2 * s2u * s2u;
   s4u = 2 * c2u * s2u;
   c4v = 1 - 2 * s2v * s2v;
   s4v = 2 * c2v * s2v;

   c6u = c4u * c2u + s4u * s2u;
   s6u = s4u * c2u + c4u * s2u;
   c6v = c4v * c2v - s4v * s2v;
   s6v = s4v * c2v + c4v * s2v;

   c8u = 1 + 2 * s4u * s4u;
   s8u = 2 * c4u * s4u;
   c8v = 1 - 2 * s4v * s4v;
   s8v = 2 * c4v * s4v;


/*   First plane to second plane
     ----- ----- -- ------ -----

     Accumulate terms for X and Y
*/

   Xstar =         Acoeff[3] * s8u * c8v;
   Xstar = Xstar + Acoeff[2] * s6u * c6v;
   Xstar = Xstar + Acoeff[1] * s4u * c4v;
   Xstar = Xstar + Acoeff[0] * s2u * c2v;
   Xstar = Xstar + U ;

   Ystar =         Acoeff[3] * c8u * s8v;
   Ystar = Ystar + Acoeff[2] * c6u * s6v;
   Ystar = Ystar + Acoeff[1] * c4u * s4v;
   Ystar = Ystar + Acoeff[0] * c2u * s2v;
   Ystar = Ystar + V ;

/*   Apply isoperimetric radius, scale adjustment, and offsets  */

   *X = K0R4 * Xstar + falseE;
   *Y = K0R4 * Ystar + falseN;


/*  Point-scale and CoM
    ----- ----- --- ---  */

   if (XYonly == 1)
      {
      *pscale = K0;
      *CoM = 0;
      }
   else
      {
      sig1 =        8 * Acoeff[3] * c8u * c8v;
      sig1 = sig1 + 6 * Acoeff[2] * c6u * c6v;
      sig1 = sig1 + 4 * Acoeff[1] * c4u * c4v;
      sig1 = sig1 + 2 * Acoeff[0] * c2u * c2v;
      sig1 = sig1 + 1;

      sig2 =        8 * Acoeff[3] * s8u * s8v;
      sig2 = sig2 + 6 * Acoeff[2] * s6u * s6v;
      sig2 = sig2 + 4 * Acoeff[1] * s4u * s4v;
      sig2 = sig2 + 2 * Acoeff[0] * s2u * s2v;

/*    Combined square roots  */
      comroo = sqrt((1 - Epssq * SPhi * SPhi) * denom2*
                  (sig1 * sig1 + sig2 * sig2));

      *pscale = K0R4oa * 2 * denom * comroo;
      *CoM = atan2(SChi * SLam, CLam) + atan2(sig2, sig1);
      }
   }
