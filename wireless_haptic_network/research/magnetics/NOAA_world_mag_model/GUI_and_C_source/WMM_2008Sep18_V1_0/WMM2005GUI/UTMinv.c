#include "TMLib.h"

//Wrapper function for a inverse transverse mercator using WGS84
void UTMinv(int zoneLon, char NSHemisphere, double NtrmsB, double NtrmsD, int LLonly, double X, double Y, double *Lambda, double *Phi, double *pscale, double *CoM)
{
	double Lam0=(-183+zoneLon*6)*PI/180;
	double K0=0.9996;
    double falseE=500000;
    
	double falseN=0;
	
    //WGS84 ellipsoid
	double Eps=0.081819190842621494335;
    double Epssq=0.0066943799901413169961;
	double R4=6367449.1458234153093;
    double R4oa=0.99832429843125277950;
    double Bcoeff[10];
	double Dcoeff[6];


    Bcoeff[0]=-8.3773216405794867707E-04;
    Bcoeff[1]=-5.9058701522203651815E-08;    
    Bcoeff[2]=-1.6734826653438249274E-10;   
    Bcoeff[3]=-2.1647981104903861797E-13;   
    Bcoeff[4]=-3.7879309688396013649E-16;   
    Bcoeff[5]=-7.2367692879669040169E-19;   
    Bcoeff[6]=-1.4934544948474238627E-21;   
    Bcoeff[7]=-3.2538430893102429751E-24;   
    Bcoeff[8]=-7.3912479800652263027E-27;   
    Bcoeff[9]=-1.7344445906774504295E-29;  

    Dcoeff[0]=3.3565514691328321888E-03;
    Dcoeff[1]=6.5718731986276970786E-06;
    Dcoeff[2]=1.7646404113343270872E-08;
    Dcoeff[3]=5.3877540683992094349E-11;
    Dcoeff[4]=1.7639829049873024993E-13;
    Dcoeff[5]=6.0345363280209865351E-16;
    //WGS84 ellipsoid



	if(NSHemisphere=='n' || NSHemisphere=='N')
	{
		falseN=0;
	}
	if(NSHemisphere=='s' || NSHemisphere=='S')
	{
		falseN=10000000;
    }
    
    TMinv(Eps,Epssq,R4,R4oa,Bcoeff,Dcoeff,Lam0,K0,falseE,falseN,NtrmsB,NtrmsD,LLonly,X,Y,Lambda,Phi,pscale,CoM);
    //Convert angles from radians to degrees
	*Lambda=*Lambda * 180/PI;
	*Phi=*Phi*180/PI;
	*CoM=*CoM*180/PI;
}