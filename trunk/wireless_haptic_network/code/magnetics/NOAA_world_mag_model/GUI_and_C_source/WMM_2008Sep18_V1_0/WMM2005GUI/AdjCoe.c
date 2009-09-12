/*   AdjCoe  */

#include "TMLib.h"


void AdjCoe(double Dcoeff[], double *Dnest, double *Dderiv)
   {

/*   Adjust Coefficients
     ===--- ===---------

     Algorithm developed by: C. Rollins   April 18, 2006
     C software written by:  K. Robins

     Input
     -----
     Dcoeff    Trig series coefficients for Phi as a function of Chi
 
 
     Output
     ------
     Dnest     Nested trig series coefficients for Chi to Phi conversion
     Dderiv    Nested trig series coefficients for the derivative of Phi
               with repsect to Chi, as a function of Chi
*/

   Dnest[0] = Dcoeff[0] - Dcoeff[2] + Dcoeff[4];
   Dnest[1] = ( 2 * Dcoeff[1]) - ( 4 * Dcoeff[3]) + ( 6 * Dcoeff[5]);
   Dnest[2] = ( 4 * Dcoeff[2]) - (12 * Dcoeff[4]);
   Dnest[3] = ( 8 * Dcoeff[3]) - (32 * Dcoeff[5]);
   Dnest[4] = (16 * Dcoeff[4]);
   Dnest[5] = (32 * Dcoeff[5]);

   Dderiv[0] = 1 - (4 * Dcoeff[1]) + (8 * Dcoeff[3]) - ( 12 * Dcoeff[5]);
   Dderiv[1] = (  2 * Dcoeff[0]) - ( 18 * Dcoeff[2]) + ( 50 * Dcoeff[4]);
   Dderiv[2] = (  8 * Dcoeff[1]) - ( 64 * Dcoeff[3]) + (216 * Dcoeff[5]);
   Dderiv[3] = ( 24 * Dcoeff[2]) - (200 * Dcoeff[4]);
   Dderiv[4] = ( 64 * Dcoeff[3]) - (576 * Dcoeff[5]);
   Dderiv[5] = (160 * Dcoeff[4]);
   Dderiv[6] = (384 * Dcoeff[5]);
   }
