/*  TMinv  */

#include "TMLib.h"

void TMinv(double Eps, double Epssq, double R4, double R4oa, double Bcoeff[],
         double Dcoeff[], double Lam0, double K0, double falseE,
         double falseN, double NtrmsB, double NtrmsD, int LLonly,
         double X, double Y, double *Lambda, double *Phi, double *pscale,
         double *CoM)
   {

/*
   Transverse Mercator inverse equations including point-scale and CoM
   =--------- =------- ===---- ---------

   Algorithm developed by: C. Rollins   August 3, 2006
   C software written by:  K. Robins
 
 
   Input items depending on the ellipsoid
   ----- -----
 
      Eps          Eccentricity (epsilon) of the ellipsoid
      Epssq        Eccentricity-squared
      R4           Meridional isoperimetric radius
      R4oa         Ratio of R4 over semi-major axis
      Bcoeff       Trig series coefficients, chi as a function of omega
      Dcoeff       Trig series coefficients, phi as a function of chi
 
 
   Input items that define the map projection and its accuracy
   ----- -----
 
      Lam0         Longitude of the central meridian in radians
      K0           Central scale factor, for example, 0.9996 for UTM
      falseE       False easting, for example, 500000 for UTM
      falseN       False northing
      NtrmsB       Number of trig terms in the mapping from the (x,y)
                   plane to the (u,v) plane.  For example, NtrmsB=6
                   implies the last term in the series for u is 
                   Bcoeff(6)*Sinh(12*X/R4)*Cos(12*Y/R4)
      NtrmsD       Number of terms in the trig series for Phi
                
 
   Processing option
   ---------- ------
 
      LLonly       If one (1), then only the position (Longitude and
                   Latitude) will be properly computed.  Values
                   returned for point-scale and CoM will merely be 
                   the trivial values on the central meridian.
 

   Input items that identify the point to be converted
   ----- -----
 
      X            x-coordinate (Easting) in meters
      Y            y-coordinate (Northing) in meters
 
 
   Output items
   ------ -----
 
      Lambda       Longitude (from Greenwich) in radians
                   The poles will be assigned the same longitude as the
                   central meridian.
      Phi          Latitude in radians
      pscale       point-scale (dimensionless)
      CoM          Convergence-of-meridians in radians 
*/

   double Xstar, Ystar, TwoX, TwoY;
   int K;
   double c2kx[8], s2kx[8], c2ky[8], s2ky[8];
   double U, V;
   double CU, SU, CV, SV;
   double Lam;
   double Schi, Chi, TwoChi;

/*   These are needed for point-scale and CoM  */

   double sig3, sig4;
   double Sphi;
   double W, P, denom;


/*  Undo offsets, scale change, and factor R4
    ---- -------  ----- ------  --- ------ --  */

   Xstar = (X - falseE) /  (K0 * R4);
   Ystar = (Y - falseN) / (K0 * R4);


/*  Trigonometric multiple angles
    ------------- -------- ------  */

   TwoX = 2 * Xstar;
   TwoY = 2 * Ystar;

   for (K = 0; K < NtrmsB; K++)
      {
      c2kx[K] = cosh((K+1) * TwoX);
      s2kx[K] = sinh((K+1) * TwoX);
      c2ky[K] = cos((K+1) * TwoY);
      s2ky[K] = sin((K+1) * TwoY);
      }

/*  Second plane (x*, y*) to first plane (u, v)
    ------ ----- -------- -- ----- ----- ------  */

   U = 0;
   V = 0;

   for (K = NtrmsB - 1; K >= 0; K--)
      {
      U += Bcoeff[K] * s2kx[K] * c2ky[K];
      V += Bcoeff[K] * c2kx[K] * s2ky[K];
      }

   U += Xstar;
   V += Ystar;


/*  First plane to sphere
    ----- ----- -- ------  */

   CU = cosh(U);
   SU = sinh(U);
   CV = cos(V);
   SV = sin(V);

/*   Longitude from central meridian  */

   if ((fabs(CV) < 10E-12) && (fabs(SU) < 10E-12))
      Lam = 0;
   else
      Lam = atan2(SU, CV);

/*   Conformal latitude  */

   Schi = SV / CU;

   if (fabs(Schi) < 0.984375000L)
      Chi = asin(Schi);
   else
      Chi = atan2(SV, sqrt((SU * SU) + (CV * CV)));

/*  Sphere to ellipsoid
    ------ -- ---------  */

   TwoChi = 2 * Chi;
   *Phi = 0;

   for (K = NtrmsD - 1; K >= 0; K--)
      *Phi += Dcoeff[K] * sin((K+1) * TwoChi);

   *Phi += Chi;


/*  Longitude from Greenwich
    --------  ---- ---------  */

   *Lambda = Lam0 + Lam;

   *Lambda = (*Lambda > PI) ? *Lambda - (2 * PI): *Lambda;
   *Lambda = (*Lambda <= -PI) ? *Lambda + (2 * PI): *Lambda;


/*  Point-scale and CoM
    ----------- --- ---  */

   if (LLonly == 1)
      {
      *pscale = K0;
      *CoM = 0;
      }
   else
      {
      sig3 = 0;
      sig4 = 0;

      for (K = NtrmsB - 1; K >= 0; K--)
         {
         sig3 += 2 * (K+1) * Bcoeff[K] * c2kx[K] * c2ky[K];
         sig4 += 2 * (K+1) * Bcoeff[K] * s2kx[K] * s2ky[K];
         }

      sig3++;

      Sphi = sin(*Phi);
      W = sqrt(1 - (Epssq * Sphi * Sphi));
      P = exp(Eps * ATanH(Eps * Sphi));
      denom = (1 + Sphi) / P + (1 - Sphi) * P;

      *pscale = K0 * R4oa * W * 2 / denom * CU /
                  sqrt((sig3 * sig3) + (sig4 * sig4));

      *CoM = atan2(Schi * SU, CV) - atan2(sig4, sig3);
      }
   }
