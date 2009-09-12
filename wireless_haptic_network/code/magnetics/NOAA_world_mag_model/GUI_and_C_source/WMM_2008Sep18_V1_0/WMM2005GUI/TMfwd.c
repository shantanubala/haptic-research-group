/*   TMfwd  */

#include "TMLib.h"

void TMfwd(double Eps, double Epssq, double R4, double R4oa, double Acoeff[],
                  double Lam0, double K0, double falseE, double falseN,
                  double NtrmsA, int XYonly, double Lambda, double Phi,
                  double *X, double *Y, double *pscale, double *CoM)
   {

/*
   Transverse Mercator forward equations including point-scale and CoM
   =--------- =------- =--=--= ---------
 
   Algorithm developed by: C. Rollins   August 3, 2006
   C software written by:  K. Robins
 
 
   Input items depending on the ellipsoid
   ----- -----
 
      Eps          Eccentricity (epsilon) of the ellipsoid
      Epssq        Eccentricity squared
      R4           Meridional isoperimetric radius
      R4oa         Ratio of R4 over semi-major axis
      Acoeff       Trig series coefficients, omega as a function of chi
 
   Input items that define the map projection and its accuracy
   ----- -----
 
      Lam0         Longitude of the central meridian in radians
      K0           Central scale factor, for example, 0.9996 for UTM
      falseE       False easting, for example, 500000 for UTM
      falseN       False northing
      NtrmsA       Number of trig terms in the mapping from the (u,v)
                   plane to the (x,y) plane.  For example, NtrmsA=6
                   implies the last term in the series for X is 
                   Acoeff(6)*Sinh(12*U)*Cos(12*V)

   Processing option
   ---------- ------
 
      XYonly       If one (1), then only X and Y will be properly
                   computed.  Values returned for point-scale and CoM
                   will merely be the trivial values for points on the
                   central meridian.
 
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
   double U, V, TwoU, TwoV;
   int K;
   double c2ku[8], s2ku[8];
   double c2kv[8], s2kv[8];
   double Xstar, Ystar;

/*   These items are needed for point-scale and CoM  */

   double sig1, sig2, W;


/*  Ellipsoid to sphere
    --------- -- ------  */

/*  Convert longitude (Greenwhich) to longitude from the central meridian
    It is unnecessary to find the (-Pi, Pi] equivalent of the result.
    Compute its cosine and sine.                                           */

   Lam = Lambda - Lam0;
   CLam = cos(Lam);
   SLam = sin(Lam);

/*   Latitude  */

   CPhi = cos(Phi);
   SPhi = sin(Phi);

/*  Convert geodetic latitude, Phi, to conformal latitude, Chi
    Only the cosine and sine of Chi are actually needed.         */

   P = exp(Eps * ATanH(Eps * SPhi));
   part1 = (1 + SPhi) / P;
   part2 = (1 - SPhi) * P;
   denom = part1 + part2;
   CChi = 2 * CPhi / denom;
   SChi = (part1 - part2) / denom;


/*  Sphere to first plane
    ------ -- ----- -----  */

/*  Apply spherical theory of transverse Mercator to get (u,v) coord.s
    Note the order of the arguments in Fortran's version of ArcTan, i.e.
                   ATan2(y, x) = ATan(y/x)
    The two argument form of ArcTan is needed here.                       */

   U = ATanH(CChi * SLam);
   V = atan2(SChi, CChi * CLam);


/*  Trigonometric multiple angles
    ------------- -------- ------  */

/*  Compute Cosh of even multiples of U
    Compute Sinh of even multiples of U
    Compute Cos  of even multiples of V
    Compute Sin  of even multiples of V  */

   TwoU = 2 * U;
   TwoV = 2 * V;

   for (K = 0; K < NtrmsA; K++)
      {
      c2ku[K] = cosh((K+1) * TwoU);
      s2ku[K] = sinh((K+1) * TwoU);
      c2kv[K] = cos((K+1) * TwoV);
      s2kv[K] = sin((K+1) * TwoV);
      }


/*  First plane to second plane
    ----- ----- -- ------ -----  */

/*  Accumulate terms for X and Y  */

   Xstar = 0;
   Ystar = 0;

   for (K = NtrmsA - 1; K >= 0; K--)
      {
      Xstar += Acoeff[K] * s2ku[K] * c2kv[K];
      Ystar += Acoeff[K] * c2ku[K] * s2kv[K];
      }

   Xstar += U;
   Ystar += V;

/*  Apply isoperimetric radius, scale adjustment, and offsets  */

   *X = (K0 * R4 * Xstar) + falseE;
   *Y = (K0 * R4 * Ystar) + falseN;


/*  Point-scale and CoM
    ----- ----- --- ---  */

   if (XYonly == 1)
      {
      *pscale = K0;
      *CoM = 0;
      }
   else
      {
      sig1 = 0;
      sig2 = 0;

      for (K = NtrmsA - 1; K >= 0; K--)
         {
         sig1 += 2 * (K+1) * Acoeff[K] * c2ku[K] * c2kv[K];
         sig2 += 2 * (K+1) * Acoeff[K] * s2ku[K] * s2kv[K];
         }

      sig1++;

      W = sqrt(1 - (Epssq * SPhi * SPhi));
      
      *pscale = K0 * R4oa * W * 2 / denom * cosh(U) *
                  sqrt((sig1 * sig1) + (sig2 * sig2));

      *CoM = atan2(SChi * SLam, CLam) + atan2(sig2, sig1);
      }
   }
