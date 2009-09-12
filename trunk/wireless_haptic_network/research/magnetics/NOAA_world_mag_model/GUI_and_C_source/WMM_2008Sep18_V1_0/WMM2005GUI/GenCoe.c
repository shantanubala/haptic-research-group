/*  GenCoe  */

#include "TMLib.h"


void GenCoe(double invfla, double *n1, double *Acoeff, double *Bcoeff,
         double *Ccoeff, double *Dcoeff, double *R4oa)
   {

/*  Generate Coefficients for Transverse Mercator algorithms
    ===----- ===---------

   Algorithm developed by: C. Rollins   April 18, 2006
   C software written by:  K. Robins


   INPUT
   -----
      invfla    Inverse flattening (reciprocal flattening)
 

   OUTPUT
   ------
      n1        Helmert's "n"
      Acoeff    Coefficients for omega as a trig series in chi
      Bcoeff    Coefficients for chi as a trig series in omega
      Ccoeff    Coefficients for psi as a trig series in chi
      Dcoeff    Coefficients for phi as a trig series in chi
      R4oa      Ratio "R4 over a", i.e. R4/a


   EXPLANATIONS
   ------------
      omega is rectifying latitude
      chi is conformal latitude
      psi is geocentric latitude
      phi is geodetic latitude, commonly, "the latitude"
      R4 is the meridional isoperimetric radius
      "a" is the semi-major axis of the ellipsoid
      "b" is the semi-minor axis of the ellipsoid
      Helmert's n = (a - b)/(a + b)
 
      All inputs, outputs, and intermediate calculations in this
         subroutine depend only on the shape of the ellipsoid.  They are
         independent of the size of the ellipsoid.
 
      The array Acoeff(8) stores eight coefficients corresponding
         to k = 2, 4, 6, 8, 10, 12, 14, 16 in the notation "a sub k".
      Likewise Bcoeff(8) etc.

   In the wonderful world of C and zero-based arrays, it is common to
   access these arrays from 0 - 7.....   K. Robins
*/

double n2, n3, n4, n5, n6, n7, n8, n9, n10, coeff, n;

   n  = 1.0 / (2*invfla - 1.0);  /* store as "n" for convenience */
   *n1 = n;                      /* assign it to "n1" for return */

   n2  = n * n;
   n3  = n2 * n;
   n4  = n3 * n;
   n5  = n4 * n;
   n6  = n5 * n;
   n7  = n6 * n;
   n8  = n7 * n;
   n9  = n8 * n;
   n10 = n9 * n;

/*   Computation of coefficient a2  */
   coeff = 0.0;
   coeff += (-18975107.0) * n8 / 50803200.0;
   coeff += (72161.0) * n7 / 387072.0;
   coeff += (7891.0) * n6 / 37800.0;
   coeff += (-127.0) * n5 / 288.0;
   coeff += (41.0) * n4 / 180.0;
   coeff += (5.0) * n3 / 16.0;
   coeff += (-2.0) * n2 / 3.0;
   coeff += (1.0) * n / 2.0;

   Acoeff[0] = coeff;

/*   Computation of coefficient a4  */
   coeff = 0.0;
   coeff += (148003883.0) * n8 / 174182400.0;
   coeff += (13769.0) * n7 / 28800.0;
   coeff += (-1983433.0) * n6 / 1935360.0;
   coeff += (281.0) * n5 / 630.0;
   coeff += (557.0) * n4 / 1440.0;
   coeff += (-3.0) * n3 / 5.0;
   coeff += (13.0) * n2 / 48.0;

   Acoeff[1] = coeff;

/*   Computation of coefficient a6  */
   coeff = 0.0;
   coeff += (79682431.0) * n8 / 79833600.0;
   coeff += (-67102379.0) * n7 / 29030400.0;
   coeff += (167603.0) * n6 / 181440.0;
   coeff += (15061.0) * n5 / 26880.0;
   coeff += (-103.0) * n4 / 140.0;
   coeff += (61.0) * n3 / 240.0;

   Acoeff[2] = coeff;

/*   Computation of coefficient a8  */
   coeff = 0.0;
   coeff += (-40176129013.0) * n8 / 7664025600.0;
   coeff += (97445.0) * n7 / 49896.0;
   coeff += (6601661.0) * n6 / 7257600.0;
   coeff += (-179.0) * n5 / 168.0;
   coeff += (49561.0) * n4 / 161280.0;

   Acoeff[3] = coeff;

/*   Computation of coefficient a10  */
   coeff = 0.0;
   coeff += (2605413599.0) * n8 / 622702080.0;
   coeff += (14644087.0) * n7 / 9123840.0;
   coeff += (-3418889.0) * n6 / 1995840.0;
   coeff += (34729.0) * n5 / 80640.0;

   Acoeff[4] = coeff;

/*   Computation of coefficient a12  */
   coeff = 0.0;
   coeff += (175214326799.0) * n8 / 58118860800.0;
   coeff += (-30705481.0) * n7 / 10378368.0;
   coeff += (212378941.0) * n6 / 319334400.0;

   Acoeff[5] = coeff;

/*   Computation of coefficient a14  */
   coeff = 0.0;
   coeff += (-16759934899.0) * n8 / 3113510400.0;
   coeff += (1522256789.0) * n7 / 1383782400.0;

   Acoeff[6] = coeff;

/*   Computation of coefficient a16  */
   coeff = 0.0;
   coeff += (1424729850961.0) * n8 / 743921418240.0;

   Acoeff[7] = coeff;
      
/*   Computation of coefficient b2  */
   coeff = 0.0;
   coeff += (-7944359.0) * n8 / 67737600.0;
   coeff += (5406467.0) * n7 / 38707200.0;
   coeff += (-96199.0) * n6 / 604800.0;
   coeff += (81.0) * n5 / 512.0;
   coeff += (1.0) * n4 / 360.0;
   coeff += (-37.0) * n3 / 96.0;
   coeff += (2.0) * n2 / 3.0;
   coeff += (-1.0) * n / 2.0;

   Bcoeff[0] = coeff;

/*   Computation of coefficient b4  */
   coeff = 0.0;
   coeff += (-24749483.0) * n8 / 348364800.0;
   coeff += (-51841.0) * n7 / 1209600.0;
   coeff += (1118711.0) * n6 / 3870720.0;
   coeff += (-46.0) * n5 / 105.0;
   coeff += (437.0) * n4 / 1440.0;
   coeff += (-1.0) * n3 / 15.0;
   coeff += (-1.0) * n2 / 48.0;

   Bcoeff[1] = coeff;

/*   Computation of coefficient b6  */
   coeff = 0.0;
   coeff += (6457463.0) * n8 / 17740800.0;
   coeff += (-9261899.0) * n7 / 58060800.0;
   coeff += (-5569.0) * n6 / 90720.0;
   coeff += (209.0) * n5 / 4480.0;
   coeff += (37.0) * n4 / 840.0;
   coeff += (-17.0) * n3 / 480.0;

   Bcoeff[2] = coeff;

/*   Computation of coefficient b8  */
   coeff = 0.0;
   coeff += (-324154477.0) * n8 / 7664025600.0;
   coeff += (-466511.0) * n7 / 2494800.0;
   coeff += (830251.0) * n6 / 7257600.0;
   coeff += (11.0) * n5 / 504.0;
   coeff += (-4397.0) * n4 / 161280.0;

   Bcoeff[3] = coeff;

/*   Computation of coefficient b10  */
   coeff = 0.0;
   coeff += (-22894433.0) * n8 / 124540416.0;
   coeff += (8005831.0) * n7 / 63866880.0;
   coeff += (108847.0) * n6 / 3991680.0;
   coeff += (-4583.0) * n5 / 161280.0;

   Bcoeff[4] = coeff;

/*   Computation of coefficient b12  */
   coeff = 0.0;
   coeff += (2204645983.0) * n8 / 12915302400.0;
   coeff += (16363163.0) * n7 / 518918400.0;
   coeff += (-20648693.0) * n6 / 638668800.0;

   Bcoeff[5] = coeff;

/*   Computation of coefficient b14  */
   coeff = 0.0;
   coeff += (497323811.0) * n8 / 12454041600.0;
   coeff += (-219941297.0) * n7 / 5535129600.0;

   Bcoeff[6] = coeff;

/*   Computation of coefficient b16  */
   coeff = 0.0;
   coeff += (-191773887257.0) * n8 / 3719607091200.0;

   Bcoeff[7] = coeff;

/*   Computation of coefficient c2  */
   coeff = 0.0;
   coeff += (64424.0) * n8 / 99225.0;
   coeff += (76.0) * n7 / 225.0;
   coeff += (-3658.0) * n6 / 4725.0;
   coeff += (2.0) * n5 / 9.0;
   coeff += (4.0) * n4 / 9.0;
   coeff += (-2.0) * n3 / 3.0;
   coeff += (-2.0) * n2 / 3.0;

   Ccoeff[0] = coeff;

/*   Computation of coefficient c4  */
   coeff = 0.0;
   coeff += (2146.0) * n8 / 1215.0;
   coeff += (-2728.0) * n7 / 945.0;
   coeff += (61.0) * n6 / 135.0;
   coeff += (68.0) * n5 / 45.0;
   coeff += (-23.0) * n4 / 45.0;
   coeff += (-4.0) * n3 / 15.0;
   coeff += (1.0) * n2 / 3.0;

   Ccoeff[1] = coeff;

/*   Computation of coefficient c6  */
   coeff = 0.0;
   coeff += (-95948.0) * n8 / 10395.0;
   coeff += (428.0) * n7 / 945.0;
   coeff += (9446.0) * n6 / 2835.0;
   coeff += (-46.0) * n5 / 35.0;
   coeff += (-24.0) * n4 / 35.0;
   coeff += (2.0) * n3 / 5.0;

   Ccoeff[2] = coeff;

/*   Computation of coefficient c8  */
   coeff = 0.0;
   coeff += (29741.0) * n8 / 85050.0;
   coeff += (4472.0) * n7 / 525.0;
   coeff += (-34712.0) * n6 / 14175.0;
   coeff += (-80.0) * n5 / 63.0;
   coeff += (83.0) * n4 / 126.0;

   Ccoeff[3] = coeff;

/*   Computation of coefficient c10  */
   coeff = 0.0;
   coeff += (280108.0) * n8 / 13365.0;
   coeff += (-17432.0) * n7 / 3465.0;
   coeff += (-2362.0) * n6 / 891.0;
   coeff += (52.0) * n5 / 45.0;

   Ccoeff[4] = coeff;

/*   Computation of coefficient c12  */
   coeff = 0.0;
   coeff += (-48965632.0) * n8 / 4729725.0;
   coeff += (-548752.0) * n7 / 96525.0;
   coeff += (335882.0) * n6 / 155925.0;

   Ccoeff[5] = coeff;

/*   Computation of coefficient c14  */
   coeff = 0.0;
   coeff += (-197456.0) * n8 / 15795.0;
   coeff += (51368.0) * n7 / 12285.0;

   Ccoeff[6] = coeff;

/*   Computation of coefficient c16  */
   coeff = 0.0;
   coeff += (1461335.0) * n8 / 174636.0;

   Ccoeff[7] = coeff;

/*   Computation of coefficient d2  */
   coeff = 0.0;
   coeff += (189416.0) * n8 / 99225.0;
   coeff += (16822.0) * n7 / 4725.0;
   coeff += (-2854.0) * n6 / 675.0;
   coeff += (26.0) * n5 / 45.0;
   coeff += (116.0) * n4 / 45.0;
   coeff += (-2.0) * n3 / 1.0;
   coeff += (-2.0) * n2 / 3.0;
   coeff += (2.0) * n / 1.0;

   Dcoeff[0] = coeff;

/*   Computation of coefficient d4  */
   coeff = 0.0;
   coeff += (141514.0) * n8 / 8505.0;
   coeff += (-31256.0) * n7 / 1575.0;
   coeff += (2323.0) * n6 / 945.0;
   coeff += (2704.0) * n5 / 315.0;
   coeff += (-227.0) * n4 / 45.0;
   coeff += (-8.0) * n3 / 5.0;
   coeff += (7.0) * n2 / 3.0;

   Dcoeff[1] = coeff;

/*   Computation of coefficient d6  */
   coeff = 0.0;
   coeff += (-2363828.0) * n8 / 31185.0;
   coeff += (98738.0) * n7 / 14175.0;
   coeff += (73814.0) * n6 / 2835.0;
   coeff += (-1262.0) * n5 / 105.0;
   coeff += (-136.0) * n4 / 35.0;
   coeff += (56.0) * n3 / 15.0;

   Dcoeff[2] = coeff;

/*   Computation of coefficient d8  */
   coeff = 0.0;
   coeff += (14416399.0) * n8 / 935550.0;
   coeff += (11763988.0) * n7 / 155925.0;
   coeff += (-399572.0) * n6 / 14175.0;
   coeff += (-332.0) * n5 / 35.0;
   coeff += (4279.0) * n4 / 630.0;

   Dcoeff[3] = coeff;

/*   Computation of coefficient d10  */
   coeff = 0.0;
   coeff += (258316372.0) * n8 / 1216215.0;
   coeff += (-2046082.0) * n7 / 31185.0;
   coeff += (-144838.0) * n6 / 6237.0;
   coeff += (4174.0) * n5 / 315.0;

   Dcoeff[4] = coeff;

/*   Computation of coefficient d12  */
   coeff = 0.0;
   coeff += (-2155215124.0) * n8 / 14189175.0;
   coeff += (-115444544.0) * n7 / 2027025.0;
   coeff += (601676.0) * n6 / 22275.0;

   Dcoeff[5] = coeff;

/*   Computation of coefficient d14  */
   coeff = 0.0;
   coeff += (-170079376.0) * n8 / 1216215.0;
   coeff += (38341552.0) * n7 / 675675.0;

   Dcoeff[6] = coeff;

/*   Computation of coefficient d16  */
   coeff = 0.0;
   coeff += (1383243703.0) * n8 / 11351340.0;

   Dcoeff[7] = coeff;

/*   Computation of ratio R4/a  */
   coeff = 0.0;
   coeff += (83349.0) * n10 / 65536.0;
   coeff += (-20825.0) * n9 / 16384.0;
   coeff += (20825.0) * n8 / 16384.0;
   coeff += (-325.0) * n7 / 256.0;
   coeff += (325.0) * n6 / 256.0;
   coeff += (-81.0) * n5 / 64.0;
   coeff += (81.0) * n4 / 64.0;
   coeff += (-5.0) * n3 / 4.0;
   coeff += (5.0) * n2 / 4.0;
   coeff += (-1.0) * n / 1.0;
   coeff += 1.0;

   *R4oa  = coeff;
   }
