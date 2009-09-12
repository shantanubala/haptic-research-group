/*   ElemFun  */

#include "TMLib.h"

/*   Algorithm developed by: C. Rollins   April 18, 2006
     C software written by:  K. Robins                     */

double ATanH(double x)
   {
   return(0.5 * log((1 + x) / (1 - x)));
   }

double Cot(double x)
   {
   return(tan((PI / 2) - x));
   }
