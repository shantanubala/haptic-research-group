/*   TMinv4  */

#include "TMLib.h"


void TMinv4(double Eps, double Epssq, double K0R4i, double psfact,
         double Bcoeff[], double Dnest[], double Dderiv[],
         double Lam0, double K0, double falseE, double falseN,
         int LLonly, double X, double Y,
         double *Lambda, double *Phi, double *pscale, double *CoM)
   {

/*
   Transverse Mercator inverse equations including point-scale and CoM
   =--------- =------- ===---- ---------
 
   Algorithm developed by: C. Rollins   August 3, 2006
   C software written by:  K. Robins
 
 
   Constants fixed by choice of ellipsoid, choice of projection param.s
   --------- -----
 
      Eps          Eccentricity (epsilon) of the ellipsoid
      Epssq        Eccentricity-squared
    ( A            Semi-major axis of the ellipsoid  )
    ( R4           Meridional isoperimetric radius   )
    ( R4oa         Ratio R4 over semi-major axis     )
    ( K0           Central scale factor              )
      K0R4i        The precomputed number 1/(K0*R4)
      psfact       The precomputed number K0*R4oa/(1 - Epssq)
      Bcoeff       Trig series coefficients for chi as a funct. of omega
      Dnest        Nested trig series coefficients for
                      phi as a function of chi
      Dderiv       Nested trig series coefficients for
                      the derivative of phi with repsect to chi
      Lam0         Longitude of the central meridian in radians
      K0           Central scale factor, for example, 0.9996 for UTM
      falseE       False easting, for example, 500000 for UTM
      falseN       False northing
                
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

   double Xstar, Ystar;
   double c2x, s2x, c4x, s4x, c6x, s6x, c8x, s8x;
   double c2y, s2y, c4y, s4y, c6y, s6y, c8y, s8y;
   double U, V;
   double CU, SU, CV, SV;
   double Lam, AbsLam;
   double Cchi, Schi, Chi;
   double C2chi, S2chi;

/*   These are needed for point-scale and CoM  */

   double sig3, sig4;
   double Sphi, Wsq, comroo, dphoch;


/*  Undo offsets, scale change, and factor R4
    ---- -------  ----- ------  --- ------ --  */

   Xstar = (X - falseE) * K0R4i;
   Ystar = (Y - falseN) * K0R4i;


/*  Trigonometric multiple angles
    ------------- -------- ------  */

   c2x = cosh(2 * Xstar);
   s2x = sinh(2 * Xstar);
   c2y = cos(2 * Ystar);
   s2y = sin(2 * Ystar);

   c4x = 1 + 2 * s2x * s2x;
   s4x = 2 * c2x * s2x;
   c4y = 1 - 2 * s2y * s2y;
   s4y = 2 * c2y * s2y;

   c6x = c4x * c2x + s4x * s2x;
   s6x = s4x * c2x + c4x * s2x;
   c6y = c4y * c2y - s4y * s2y;
   s6y = s4y * c2y + c4y * s2y;

   c8x = 1 + 2 * s4x * s4x;
   s8x = 2 * c4x * s4x;
   c8y = 1 - 2 * s4y * s4y;
   s8y = 2 * c4y * s4y;


/*  Second plane (x*, y*) to first plane (u, v)
    ------ ----- -------- -- ----- ----- ------  */

   U =     Bcoeff[3] * s8x * c8y;
   U = U + Bcoeff[2] * s6x * c6y;
   U = U + Bcoeff[1] * s4x * c4y;
   U = U + Bcoeff[0] * s2x * c2y;
   U = U + Xstar;

   V =     Bcoeff[3] * c8x * s8y;
   V = V + Bcoeff[2] * c6x * s6y;
   V = V + Bcoeff[1] * c4x * s4y;
   V = V + Bcoeff[0] * c2x * s2y;
   V = V + Ystar;


/*  First plane to sphere
    ----- ----- -- ------  */

   CU = cosh(U);
   SU = sinh(U);
   CV = cos(V);
   SV = sin(V);

/*  Longitude from central meridian  */

   if (fabs(CV) < 10E-12   &&   fabs(SU) < 10E-12)
      Lam = 0;
   else
      Lam = atan2(SU, CV);

   AbsLam = fabs(Lam);

/*  Conformal latitude  */

   if ((0.75L < AbsLam) && (AbsLam < 2.25L))
      Cchi = SU / (CU * sin(Lam));
   else
      Cchi = CV / (CU * cos(Lam));

   Schi = SV / CU ;

   if (fabs(Schi) < 0.984375)
      Chi = asin(Schi);
   else
/*    Chi = Sign(acos(Cchi), V);    How Fortran does it
      and now in C:                                        */
      {
      if (V >= 0.0L)
         Chi = (acos(Cchi) >= 0.0L) ? acos(Cchi): -acos(Cchi);
      else
         Chi = (acos(Cchi) >= 0.0L) ? -acos(Cchi): acos(Cchi);
      }


/*  Sphere to ellipsoid
    ------ -- ---------  */

   C2chi = 1 - 2 * Schi * Schi;
   S2chi = 2 * Schi * Cchi;

   *Phi = S2chi * (Dnest[0] + C2chi * (Dnest[1] + C2chi * (Dnest[2] +
         C2chi * (Dnest[3] + C2chi * (Dnest[4] + C2chi * (Dnest[5]))))));

   *Phi += Chi;


/*  Longitude from Greenwich
    --------  ---- ---------  */

   *Lambda = Lam0 + Lam;

   if (*Lambda > PI) *Lambda -= TwoPI;
   if (*Lambda <= -PI) *Lambda += TwoPI;


/*  Point-scale and CoM
    ----------- --- ---  */

   if (LLonly == 1)
      {
      *pscale = K0;
      *CoM = 0;
      }
   else
      {
      sig3 =  8 * Bcoeff[3] * c8x * c8y;
      sig3 += 6 * Bcoeff[2] * c6x * c6y;
      sig3 += 4 * Bcoeff[1] * c4x * c4y;
      sig3 += 2 * Bcoeff[0] * c2x * c2y;
      sig3 ++;

      sig4 =  8 * Bcoeff[3] * s8x * s8y;
      sig4 += 6 * Bcoeff[2] * s6x * s6y;
      sig4 += 4 * Bcoeff[1] * s4x * s4y;
      sig4 += 2 * Bcoeff[0] * s2x * s2y;

      Sphi = sin(*Phi);
      Wsq = 1 - Epssq * Sphi * Sphi;

/*  combined square roots  */
      comroo = sqrt(Wsq / (sig3 * sig3 + sig4 * sig4));

/*  derivative of phi with respect to chi  */
      dphoch = Dderiv[0] +
             C2chi * (Dderiv[1] + C2chi * (Dderiv[2] +
             C2chi * (Dderiv[3] + C2chi * (Dderiv[4] +
             C2chi * (Dderiv[5] + C2chi * (Dderiv[6]))))));

      *pscale = psfact * Wsq * comroo * CU / dphoch;
      *CoM = atan2(Schi * SU, CV) - atan2(sig4, sig3);
      }
   }
