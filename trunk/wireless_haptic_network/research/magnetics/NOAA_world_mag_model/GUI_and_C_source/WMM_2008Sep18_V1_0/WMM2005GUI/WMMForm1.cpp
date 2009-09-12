//---------------------------------------------------------------------------

#include <vcl.h>
#include <clipbrd.hpp>
#include <stdio.h>
#include <math.h>
#pragma hdrstop

#define NaN log(-1.0)
#include "WMMForm1.h"

//These lines need to be changed if compiled on a different machine
#include "C:\Documents and Settings\akimbrel\My Documents\Borland Studio Projects\WMMGUI4\ElemFun.c"
#include "C:\Documents and Settings\akimbrel\My Documents\Borland Studio Projects\WMMGUI4\TMinv.c"
#include "C:\Documents and Settings\akimbrel\My Documents\Borland Studio Projects\WMMGUI4\UTMinv.c"
//

#include "WMMHelp.h"
#include "WMMAbout.h"

//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma link "CStringGrid"
#pragma resource "*.dfm"
TForm1 *Form1;


static void E0000(int IENTRY, int *maxdeg, float alt,float glat,float glon, float time, float *dec, float *dip, float *ti, float *gv);
void geomag(int *maxdeg);
void geomg1(float alt, float glat, float glon, float time, float *dec, float *dip, float *ti, float *gv);
char geomag_introduction(float epochlowlim);

//Formats to convert a number to an AnsiString
AnsiString NormFormat1="###,##0.##";
AnsiString NormFormat2="###,##0.0";
AnsiString DateFormat="#####0.##";
AnsiString IntFormat="###,##0";

AnsiString NormFormat1l="#####0.##";
AnsiString NormFormat2l="#####0.0";
AnsiString IntFormatl="#####0";

float Min_Easting=-500000;
float Max_Easting=1500000;

float Min_Northing=-500000;
float Max_Northing=10500000;


//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner)
	: TForm(Owner)
{
}
//---------------------------------------------------------------------------
void __fastcall TForm1::Exit1Click(TObject *Sender)
{
	Form1->Close();	
}
//---------------------------------------------------------------------------
void __fastcall TForm1::FormShow(TObject *Sender)
{
	FILE *wmmtemp;
	char d_str[81],modl[20];
	float epochlowlim;
	static int maxdeg;

	wmmtemp = fopen("WMM.COF","r");
	if(wmmtemp!=NULL)
	{
		//The file can be opened
		fgets(d_str, 80, wmmtemp);
		sscanf(d_str,"%f%s",&epochlowlim,modl);
		fclose(wmmtemp);
	}
	else
	{
		//The file can't be opened. Display an error and close.
		AnsiString text="The file WMM.COF couldn't be opened.\nMake sure that it is in the same directory \nas this program and is not being used.";
		AnsiString cap="WMM.COF File Error";
		MessageBox(Form1->ClientHandle,text.c_str(),cap.c_str(),(MB_OK | MB_ICONERROR));
		Form1->Close();
	}

	maxdeg = 12;
	geomag(&maxdeg);

	//Setup the lables for the results.
	Form1->SolutionStringGrid->Cells[1][0]="Total";
	Form1->SolutionStringGrid->Cells[2][0]="Horizontal";
	Form1->SolutionStringGrid->Cells[3][0]="North";
	Form1->SolutionStringGrid->Cells[4][0]="East";
	Form1->SolutionStringGrid->Cells[5][0]="Down";
	Form1->SolutionStringGrid->Cells[6][0]="Declination";
	Form1->SolutionStringGrid->Cells[7][0]="Inclination";

	Form1->SolutionStringGrid->Cells[0][1]="Values";
	Form1->SolutionStringGrid->Cells[0][2]="Change/year";

	Form1->UTMStringGrid->Cells[1][0]="Longitude";
	Form1->UTMStringGrid->Cells[2][0]="Latitude";
	Form1->UTMStringGrid->Cells[3][0]="True-Magnetic";
	Form1->UTMStringGrid->Cells[4][0]="True-Grid";
	Form1->UTMStringGrid->Cells[5][0]="Grid-Magnetic";
	Form1->UTMStringGrid->Cells[6][0]="Grid-True";

	Form1->UTMStringGrid->Cells[0][1]="UTM Related";

	//Default the date to the current date
	Form1->DatePicker->Date=Form1->DatePicker->Date.CurrentDate();
}
//---------------------------------------------------------------------------
void __fastcall TForm1::CalculateButtonClick(TObject *Sender)
{
	//Reset the warning and error labels to not be visible
	Form1->ErrorLabel->Visible=false;
	Form1->WarningLabel->Visible=false;
	Form1->GeoWarnLabel->Visible=false;
	Form1->MagWarnLabel->Visible=false;

	//int   warn_H, warn_H_strong, warn_P;
	static int maxdeg;
	static float altm, dlat, dlon;
	static float ati, adec, adip;
	static float alt, time, dec, dip, ti, gv;
	static float time1, dec1, dip1, ti1;
	static float time2, dec2, dip2, ti2;
	float x1,x2,y1,y2,z1,z2,h1,h2;
	float ax,ay,az,ah;
	float rTd=0.017453292;
	char decd[5], dipd[5];
	float COM;
	bool h1status=true;
	//float warn_H_val, warn_H_strong_val;


	//Get input from the fields
	if(!Form1->GetInput(alt,dlat,dlon,time,COM))
	{
    	//If there was a problem, clear the result and make the error lable visible and return
		Form1->SolutionStringGrid->ClearAll(false);
		Form1->UTMStringGrid->ClearAll(false);
		Form1->UTMStringGrid->Enabled=false;
		Form1->ErrorLabel->Visible=true;
		return;
	}
	Form1->ErrorLabel->Visible=false;

	if(Form1->LocationPageControl->ActivePageIndex==2) //UTM page active
	{
		//Enable the UTM results
		Form1->UTMStringGrid->Enabled=true;
		Form1->UTMStringGrid->Repaint();

		//Set the lat and lon results
		Form1->UTMStringGrid->Cells[1][1]=Form1->GetDMS(dlon);
		Form1->UTMStringGrid->Cells[2][1]=Form1->GetDMS(dlat);

		Form1->UTMStringGrid->Cells[4][1]=Form1->GetDM(COM,0);
		Form1->UTMStringGrid->Cells[6][1]=Form1->GetDM(-COM,0);

	}
	else
	{
		//UTM not active, so clear any results and disable
		Form1->UTMStringGrid->ClearAll(false);
		Form1->UTMStringGrid->Enabled=false;
		Form1->UTMStringGrid->Repaint();
		
    }

	geomg1(alt,dlat,dlon,time,&dec,&dip,&ti,&gv);
	time1 = time;
	dec1 = dec;
	dip1 = dip;
	ti1 = ti;
	time = time1 + 1.0;

	geomg1(alt,dlat,dlon,time,&dec,&dip,&ti,&gv);
	time2 = time;
	dec2 = dec;
	dip2 = dip;
	ti2 = ti;

/*COMPUTE X, Y, Z, AND H COMPONENTS OF THE MAGNETIC FIELD*/

	x1=ti1*(cos((dec1*rTd))*cos((dip1*rTd)));
	x2=ti2*(cos((dec2*rTd))*cos((dip2*rTd)));
	y1=ti1*(cos((dip1*rTd))*sin((dec1*rTd)));
	y2=ti2*(cos((dip2*rTd))*sin((dec2*rTd)));
	z1=ti1*(sin((dip1*rTd)));
	z2=ti2*(sin((dip2*rTd)));
	h1=ti1*(cos((dip1*rTd)));
	h2=ti2*(cos((dip2*rTd)));

/*  COMPUTE ANNUAL CHANGE FOR TOTAL INTENSITY  */
	ati = ti2 - ti1;

/*  COMPUTE ANNUAL CHANGE FOR DIP & DEC  */
	adip = (dip2 - dip1) * 60.;
	adec = (dec2 - dec1) * 60.;


/*  COMPUTE ANNUAL CHANGE FOR X, Y, Z, AND H */
	ax = x2-x1;
	ay = y2-y1;
	az = z2-z1;
	ah = h2-h1;


/* deal with geographic and magnetic poles */

	if (h1 < 100.0) /* at magnetic poles */
	{
		h1status=false;

		/* while rest is ok */
		Form1->MagWarnLabel->Visible=true;
	}
	else
	{
		Form1->MagWarnLabel->Visible=false;
	}

	if (h1 < 1000.0)
	{
		Form1->WarningLabel->Caption="There is a very high uncertainty in the horizontal field strength where it is less than 1000 nT. ";
		Form1->WarningLabel->Visible=true;
	}
	else if (h1 < 5000.0)
	{
		Form1->WarningLabel->Caption="There is a high uncertainty in the horizontal field strength where it is less than 5000 nT. ";
		Form1->WarningLabel->Visible=true;
	}
	else
	{
    	Form1->WarningLabel->Visible=false;
    }

	if (90.0-fabs(dlat) <= 0.001) /* at geographic poles */
	{

		Form1->SolutionStringGrid->Cells[3][1]="NaN";
		Form1->SolutionStringGrid->Cells[4][1]="NaN";
		Form1->SolutionStringGrid->Cells[6][1]="NaN";

		Form1->SolutionStringGrid->Cells[3][2]="NaN";
		Form1->SolutionStringGrid->Cells[4][2]="NaN";
		Form1->SolutionStringGrid->Cells[6][2]="NaN";

		if(Form1->LocationPageControl->ActivePageIndex==2) //UTM active
		{
			Form1->UTMStringGrid->Cells[3][1]="NaN";
			Form1->UTMStringGrid->Cells[5][1]="NaN";
		}

		/* while rest is ok */

		//Set warming label visible
		Form1->GeoWarnLabel->Visible=true;
	}
	else
	{
		//Set warning label invisible
		Form1->GeoWarnLabel->Visible=false;

		//Set results
		Form1->SolutionStringGrid->Cells[3][1]=Form1->Format(IntFormatl,IntFormat,x1,10000)+" nT";
		Form1->SolutionStringGrid->Cells[4][1]=Form1->Format(IntFormatl,IntFormat,y1,10000)+" nT";

		Form1->SolutionStringGrid->Cells[3][2]=Form1->Format(NormFormat2l,NormFormat2,ax,10000)+" nT";
		Form1->SolutionStringGrid->Cells[4][2]=Form1->Format(NormFormat2l,NormFormat2,ay,10000)+" nT";

		if(h1status)
		{
			Form1->SolutionStringGrid->Cells[6][2]=Form1->GetDM((float)adec/60,1);
			Form1->SolutionStringGrid->Cells[6][1]=Form1->GetDM(dec1,0);
		}
		else
		{
			Form1->SolutionStringGrid->Cells[6][2]="NaN";
			Form1->SolutionStringGrid->Cells[6][1]="NaN";
        }

		if(Form1->LocationPageControl->ActivePageIndex==2) //UTM active
		{
			if(h1status)
			{
				Form1->UTMStringGrid->Cells[3][1]=Form1->GetDM(dec1,0);
				Form1->UTMStringGrid->Cells[5][1]=Form1->GetDM(dec1-COM,0);
			}
			else
			{
				Form1->UTMStringGrid->Cells[3][1]="NaN";
				Form1->UTMStringGrid->Cells[5][1]="NaN";
            }
		}
	}

	//Set Results
	Form1->SolutionStringGrid->Cells[1][1]=Form1->Format(IntFormatl,IntFormat,ti1,10000)+" nT";
	Form1->SolutionStringGrid->Cells[2][1]=Form1->Format(IntFormatl,IntFormat,h1,10000)+" nT";
	Form1->SolutionStringGrid->Cells[5][1]=Form1->Format(IntFormatl,IntFormat,z1,10000)+" nT";
	Form1->SolutionStringGrid->Cells[7][1]=Form1->GetDM(dip1,0);

	Form1->SolutionStringGrid->Cells[1][2]=Form1->Format(NormFormat2l,NormFormat2,ati,10000)+" nT";
	Form1->SolutionStringGrid->Cells[2][2]=Form1->Format(NormFormat2l,NormFormat2,ah,10000)+" nT";
	Form1->SolutionStringGrid->Cells[5][2]=Form1->Format(NormFormat2l,NormFormat2,az,10000)+" nT";
	Form1->SolutionStringGrid->Cells[7][2]=Form1->GetDM((float)adip/60,1);

}
//---------------------------------------------------------------------------

static void E0000(int IENTRY, int *maxdeg, float alt, float glat, float glon, float time, float *dec, float *dip, float *ti, float *gv)
{
  static int maxord,i,icomp,n,m,j,D1,D2,D3,D4;
  static float c[13][13],cd[13][13],tc[13][13],dp[13][13],snorm[169],
	sp[13],cp[13],fn[13],fm[13],pp[13],k[13][13],pi,dtr,a,b,re,
    a2,b2,c2,a4,b4,c4,epoch,gnm,hnm,dgnm,dhnm,flnmj,otime,oalt,
    olat,olon,dt,rlon,rlat,srlon,srlat,crlon,crlat,srlat2,
    crlat2,q,q1,q2,ct,st,r2,r,d,ca,sa,aor,ar,br,bt,bp,bpp,
    par,temp1,temp2,parp,bx,by,bz,bh;
  static char model[20], c_str[81], c_new[5];
  static float *p = snorm;
  char answer;

  FILE *wmmdat;

  switch(IENTRY){case 0: goto GEOMAG; case 1: goto GEOMG1;}
  
 GEOMAG:
  wmmdat = fopen("WMM.COF","r");

/* INITIALIZE CONSTANTS */
  maxord = *maxdeg;
  sp[0] = 0.0;
  cp[0] = *p = pp[0] = 1.0;
  dp[0][0] = 0.0;
  a = 6378.137;
  b = 6356.7523142;
  re = 6371.2;
  a2 = a*a;
  b2 = b*b;
  c2 = a2-b2;
  a4 = a2*a2;
  b4 = b2*b2;
  c4 = a4 - b4;

/* READ WORLD MAGNETIC MODEL SPHERICAL HARMONIC COEFFICIENTS */
  c[0][0] = 0.0;
  cd[0][0] = 0.0;

  fgets(c_str, 80, wmmdat);
  sscanf(c_str,"%f%s",&epoch,model);
 S3:
  fgets(c_str, 80, wmmdat);
/* CHECK FOR LAST LINE IN FILE */
  for (i=0; i<4 && (c_str[i] != '\0'); i++)
	{
      c_new[i] = c_str[i];
	  c_new[i+1] = '\0';
    }
  icomp = strcmp("9999", c_new);
  if (icomp == 0) goto S4;
/* END OF FILE NOT ENCOUNTERED, GET VALUES */
  sscanf(c_str,"%d%d%f%f%f%f",&n,&m,&gnm,&hnm,&dgnm,&dhnm);
  if (m <= n)
    {
	  c[m][n] = gnm;
      cd[m][n] = dgnm;
	  if (m != 0)
        {
		  c[n][m-1] = hnm;
		  cd[n][m-1] = dhnm;
		}
    }
  goto S3;

/* CONVERT SCHMIDT NORMALIZED GAUSS COEFFICIENTS TO UNNORMALIZED */
 S4:
  *snorm = 1.0;
  for (n=1; n<=maxord; n++)
	{
	  *(snorm+n) = *(snorm+n-1)*(float)(2*n-1)/(float)n;
	  j = 2;
      for (m=0,D1=1,D2=(n-m+D1)/D1; D2>0; D2--,m+=D1)
		{
          k[m][n] = (float)(((n-1)*(n-1))-(m*m))/(float)((2*n-1)*(2*n-3));
		  if (m > 0)
			{
			  flnmj = (float)((n-m+1)*j)/(float)(n+m);
			  *(snorm+n+m*13) = *(snorm+n+(m-1)*13)*sqrt(flnmj);
			  j = 1;
			  c[n][m-1] = *(snorm+n+m*13)*c[n][m-1];
			  cd[n][m-1] = *(snorm+n+m*13)*cd[n][m-1];
            }
          c[m][n] = *(snorm+n+m*13)*c[m][n];
		  cd[m][n] = *(snorm+n+m*13)*cd[m][n];
		}
	  fn[n] = (float)(n+1);
      fm[n] = (float)n;
    }
  k[1][1] = 0.0;

  otime = oalt = olat = olon = -1000.0;
  fclose(wmmdat);
  return;

/*************************************************************************/

 GEOMG1:

  dt = time - epoch;
  if (otime < 0.0 && (dt < 0.0 || dt > 5.0))
	{
	  /*printf("\n\n WARNING - TIME EXTENDS BEYOND MODEL 5-YEAR LIFE SPAN");
	  printf("\n CONTACT NGDC FOR PRODUCT UPDATES:");
	  printf("\n         National Geophysical Data Center");
	  printf("\n         NOAA EGC/2");
	  printf("\n         325 Broadway");
	  printf("\n         Boulder, CO 80303 USA");
	  printf("\n         Attn: Susan McLean or Stefan Maus");
	  printf("\n         Phone:  (303) 497-6478 or -6522");
	  printf("\n         Email:  Susan.McLean@noaa.gov");
	  printf("\n         or");
	  printf("\n         Stefan.Maus@noaa.gov");
	  printf("\n         Web: http://www.ngdc.noaa.gov/seg/WMM/");
	  printf("\n\n EPOCH  = %.3lf",epoch);
	  printf("\n TIME   = %.3lf",time);
	  printf("\n Do you wish to continue? (y or n) ");
	  scanf("%c%*[^\n]",&answer);
	  getchar();
	  if ((answer == 'n') || (answer == 'N'))
		{
		  printf("\n Do you wish to enter more point data? (y or n) ");
		  scanf("%c%*[^\n]",&answer);
		  getchar();
		  if ((answer == 'y')||(answer == 'Y')) goto GEOMG1;
		  else exit (0);
		}  */
	}

  pi = 3.14159265359;
  dtr = pi/180.0;
  rlon = glon*dtr;
  rlat = glat*dtr;
  srlon = sin(rlon);
  srlat = sin(rlat);
  crlon = cos(rlon);
  crlat = cos(rlat);
  srlat2 = srlat*srlat;
  crlat2 = crlat*crlat;
  sp[1] = srlon;
  cp[1] = crlon;

/* CONVERT FROM GEODETIC COORDS. TO SPHERICAL COORDS. */
  if (alt != oalt || glat != olat)
    {
	  q = sqrt(a2-c2*srlat2);
      q1 = alt*q;
      q2 = ((q1+a2)/(q1+b2))*((q1+a2)/(q1+b2));
      ct = srlat/sqrt(q2*crlat2+srlat2);
	  st = sqrt(1.0-(ct*ct));
	  r2 = (alt*alt)+2.0*q1+(a4-c4*srlat2)/(q*q);
      r = sqrt(r2);
      d = sqrt(a2*crlat2+b2*srlat2);
      ca = (alt+d)/r;
      sa = c2*crlat*srlat/(r*d);
	}
  if (glon != olon)
    {
      for (m=2; m<=maxord; m++)
        {
		  sp[m] = sp[1]*cp[m-1]+cp[1]*sp[m-1];
		  cp[m] = cp[1]*cp[m-1]-sp[1]*sp[m-1];
        }
    }
  aor = re/r;
  ar = aor*aor;
  br = bt = bp = bpp = 0.0;
  for (n=1; n<=maxord; n++)
    {
      ar = ar*aor;
	  for (m=0,D3=1,D4=(n+m+D3)/D3; D4>0; D4--,m+=D3)
        {
/*
   COMPUTE UNNORMALIZED ASSOCIATED LEGENDRE POLYNOMIALS
   AND DERIVATIVES VIA RECURSION RELATIONS
*/
          if (alt != oalt || glat != olat)
            {
              if (n == m)
                {
				  *(p+n+m*13) = st**(p+n-1+(m-1)*13);
                  dp[m][n] = st*dp[m-1][n-1]+ct**(p+n-1+(m-1)*13);
                  goto S50;
                }
              if (n == 1 && m == 0)
				{
				  *(p+n+m*13) = ct**(p+n-1+m*13);
                  dp[m][n] = ct*dp[m][n-1]-st**(p+n-1+m*13);
                  goto S50;
                }
			  if (n > 1 && n != m)
                {
                  if (m > n-2) *(p+n-2+m*13) = 0.0;
                  if (m > n-2) dp[m][n-2] = 0.0;
                  *(p+n+m*13) = ct**(p+n-1+m*13)-k[m][n]**(p+n-2+m*13);
				  dp[m][n] = ct*dp[m][n-1] - st**(p+n-1+m*13)-k[m][n]*dp[m][n-2];
                }
            }
        S50:
/*
	TIME ADJUST THE GAUSS COEFFICIENTS
*/
          if (time != otime)
            {
			  tc[m][n] = c[m][n]+dt*cd[m][n];
			  if (m != 0) tc[n][m-1] = c[n][m-1]+dt*cd[n][m-1];
            }
/*
    ACCUMULATE TERMS OF THE SPHERICAL HARMONIC EXPANSIONS
*/
		  par = ar**(p+n+m*13);
		  if (m == 0)
            {
              temp1 = tc[m][n]*cp[m];
              temp2 = tc[m][n]*sp[m];
			}
          else
            {
              temp1 = tc[m][n]*cp[m]+tc[n][m-1]*sp[m];
              temp2 = tc[m][n]*sp[m]-tc[n][m-1]*cp[m];
			}
          bt = bt-ar*temp1*dp[m][n];
          bp += (fm[m]*temp2*par);
          br += (fn[n]*temp1*par);
/*
	SPECIAL CASE:  NORTH/SOUTH GEOGRAPHIC POLES
*/
          if (st == 0.0 && m == 1)
			{
              if (n == 1) pp[n] = pp[n-1];
			  else pp[n] = ct*pp[n-1]-k[m][n]*pp[n-2];
              parp = ar*pp[n];
              bpp += (fm[m]*temp2*parp);
            }
        }
	}
  if (st == 0.0) bp = bpp;
  else bp /= st;
/*
    ROTATE MAGNETIC VECTOR COMPONENTS FROM SPHERICAL TO
	GEODETIC COORDINATES
*/
  bx = -bt*ca-br*sa;
  by = bp;
  bz = bt*sa-br*ca;
/*
    COMPUTE DECLINATION (DEC), INCLINATION (DIP) AND
    TOTAL INTENSITY (TI)
*/
  bh = sqrt((bx*bx)+(by*by));
  *ti = sqrt((bh*bh)+(bz*bz));
  *dec = atan2(by,bx)/dtr;
  *dip = atan2(bz,bh)/dtr;
/*
	COMPUTE MAGNETIC GRID VARIATION IF THE CURRENT
	GEODETIC POSITION IS IN THE ARCTIC OR ANTARCTIC
	(I.E. GLAT > +55 DEGREES OR GLAT < -55 DEGREES)

	OTHERWISE, SET MAGNETIC GRID VARIATION TO -999.0
*/
  *gv = -999.0;
  if (fabs(glat) >= 55.)
	{
	  if (glat > 0.0 && glon >= 0.0) *gv = *dec-glon;
	  if (glat > 0.0 && glon < 0.0) *gv = *dec+fabs(glon);
	  if (glat < 0.0 && glon >= 0.0) *gv = *dec+glon;
	  if (glat < 0.0 && glon < 0.0) *gv = *dec-fabs(glon);
	  if (*gv > +180.0) *gv -= 360.0;
	  if (*gv < -180.0) *gv += 360.0;
	}
  otime = time;
  oalt = alt;
  olat = glat;
  olon = glon;
  return;
}

/*************************************************************************/

void geomag(int *maxdeg)
{
  E0000(0,maxdeg,0.0,0.0,0.0,0.0,NULL,NULL,NULL,NULL);
}

/*************************************************************************/

void geomg1(float alt, float glat, float glon, float time, float *dec, float *dip, float *ti, float *gv)
{
  E0000(1,NULL,alt,glat,glon,time,dec,dip,ti,gv);
}

/*************************************************************************/




float TForm1::YearFraction(int month, int day) //Returns the fraction of a year given by the month and day
{
	switch(month)
	{
		case 1:
			break;
		case 2:
			day=31+day;
			break;
		case 3:
			day=59+day;
			break;
		case 4:
			day=90+day;
			break;
		case 5:
			day=120+day;
			break;
		case 6:
			day=151+day;
			break;
		case 7:
			day=181+day;
			break;
		case 8:
			day=212+day;
			break;
		case 9:
			day=243+day;
			break;
		case 10:
			day=273+day;
			break;
		case 11:
			day=304+day;
			break;
		case 12:
			day=334+day;
			break;
	}
	return (float)(day-1)/365;
}

float TForm1::GetValue(TEdit* EditBox) //Returns a float from a text box
{
	if(EditBox->Text.Trim()=="")
	{
		EditBox->ShowHint=false;
		EditBox->Font->Color=clWindowText;
		return 0;
	}
	return EditBox->Text.ToDouble();
}

bool TForm1::CheckText(TEdit* EditBox,bool integer,AnsiString Formatl,AnsiString Formath,float min,float max) //Checks to make sure that the text in an edit box is valid
{
	float value;
	if(EditBox->Text.Trim()=="")
	{
		//Assume a value of 0 if nothing is entered. Then continue checking.
		value=0;
	}
	else
	{
		try
		{
			//Convert the text in the edit box to a float
			value=EditBox->Text.ToDouble();
		}
		catch(EConvertError &e)
		{
			//Could not be recognized as a number. The check fails
			EditBox->Hint="Value is not a number";
			EditBox->ShowHint=true;
			EditBox->Font->Color=clRed;
			return false;
		}
	}
	if(value<min) //Lower than minimum
	{
		EditBox->Hint="Value is lower than the minimum ("+Form1->Format(Formatl,Formath,min,10000)+")";
		EditBox->ShowHint=true;
		EditBox->Font->Color=clRed;
		return false;
	}
	if(value>max) //Higher than maximum
	{
		EditBox->Hint="Value is higher than the maximum ("+Form1->Format(Formatl,Formath,max,10000)+")";
		EditBox->ShowHint=true;
		EditBox->Font->Color=clRed;
		return false;
	}
	if(integer) //If this value is suspose to be an integer
	{
		if(value-((int)value)!=0) //If the value is not an integer, the check fails
		{
			EditBox->Hint="Value is not an integer";
			EditBox->ShowHint=true;
			EditBox->Font->Color=clRed;
			return false;
		}
	}

	//Turn off the real time error for this edit box
	EditBox->ShowHint=false;
	EditBox->Font->Color=clWindowText;
	return true;
}

void __fastcall TForm1::LatDegEditChange(TObject *Sender)
{
	float value;
	if(Form1->CheckText(Form1->LatDegEdit,true,NormFormat1l,NormFormat1,0,90))
	{
		value=Form1->GetValue(Form1->LatDegEdit);
		if(value==90)
		{
			//If the deg field is 90, then both min and sec must be 0
			Form1->CheckText(Form1->LatMinEdit,true,NormFormat1l,NormFormat1,0,0);
			Form1->CheckText(Form1->LatSecEdit,true,NormFormat1l,NormFormat1,0,0);
		}
		else
		{
			Form1->CheckText(Form1->LatMinEdit,true,NormFormat1l,NormFormat1,0,59);
			Form1->CheckText(Form1->LatSecEdit,true,NormFormat1l,NormFormat1,0,59);
		}
	}
	else
	{
    	//Deg field is unknown, so allow the standard 0-59 for min and sec
		Form1->CheckText(Form1->LatMinEdit,true,NormFormat1l,NormFormat1,0,59);
		Form1->CheckText(Form1->LatSecEdit,true,NormFormat1l,NormFormat1,0,59);
    }
}
//---------------------------------------------------------------------------

void __fastcall TForm1::LatMinEditChange(TObject *Sender)
{
	if(Form1->CheckText(Form1->LatDegEdit,true,NormFormat1l,NormFormat1,0,90) && Form1->GetValue(Form1->LatDegEdit)==90)
	{
		//The deg field checks out and is 90
		Form1->CheckText(Form1->LatMinEdit,true,NormFormat1l,NormFormat1,0,0);
	}
	else
	{
		//The deg field doesn't check out or is not 90
		Form1->CheckText(Form1->LatMinEdit,true,NormFormat1l,NormFormat1,0,59);
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::LatSecEditChange(TObject *Sender)
{
	if(Form1->CheckText(Form1->LatDegEdit,true,NormFormat1l,NormFormat1,0,90) && Form1->GetValue(Form1->LatDegEdit)==90)
	{
		//The deg field checks out and is 90
		Form1->CheckText(Form1->LatSecEdit,true,NormFormat1l,NormFormat1,0,0);
	}
	else
	{
		//The deg field doesn't check out or is not 90
		Form1->CheckText(Form1->LatSecEdit,true,NormFormat1l,NormFormat1,0,59);
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::LonDegEditChange(TObject *Sender)
{
	float value;
	if(Form1->CheckText(Form1->LonDegEdit,true,NormFormat1l,NormFormat1,0,180))
	{
		value=Form1->GetValue(Form1->LonDegEdit);
		if(value==180)
		{
			//if the deg field is 180, min and sec must be 0
			Form1->CheckText(Form1->LonMinEdit,true,NormFormat1l,NormFormat1,0,0);
			Form1->CheckText(Form1->LonSecEdit,true,NormFormat1l,NormFormat1,0,0);
		}
		else
		{
			Form1->CheckText(Form1->LonMinEdit,true,NormFormat1l,NormFormat1,0,59);
			Form1->CheckText(Form1->LonSecEdit,true,NormFormat1l,NormFormat1,0,59);
		}
	}
	else
	{
		//If the deg field doesn't check out, then allow a standard 0-59 for min and sec
		Form1->CheckText(Form1->LonMinEdit,true,NormFormat1l,NormFormat1,0,59);
		Form1->CheckText(Form1->LonSecEdit,true,NormFormat1l,NormFormat1,0,59);
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::LonMinEditChange(TObject *Sender)
{
	if(Form1->CheckText(Form1->LonDegEdit,true,NormFormat1l,NormFormat1,0,180)==true && Form1->GetValue(Form1->LatDegEdit)==180)
	{
		//The deg field checks out and is 180
		Form1->CheckText(Form1->LonMinEdit,true,NormFormat1l,NormFormat1,0,0);
	}
	else
	{
		//The deg field doesn't check out or is not 180
		Form1->CheckText(Form1->LonMinEdit,true,NormFormat1l,NormFormat1,0,59);
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::LonSecEditChange(TObject *Sender)
{
	if(Form1->CheckText(Form1->LonDegEdit,true,NormFormat1l,NormFormat1,0,180) && Form1->GetValue(Form1->LatDegEdit)==180)
	{
		//The deg field checks out and is 180
		Form1->CheckText(Form1->LonSecEdit,true,NormFormat1l,NormFormat1,0,0);
	}
	else
	{
		//The deg field doesn't check out or is not 180
		Form1->CheckText(Form1->LonSecEdit,true,NormFormat1l,NormFormat1,0,59);
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::LatDecEditChange(TObject *Sender)
{
	Form1->CheckText(Form1->LatDecEdit,false,NormFormat1l,NormFormat1,-90,90);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::LonDecEditChange(TObject *Sender)
{
	Form1->CheckText(Form1->LonDecEdit,false,NormFormat1l,NormFormat1,-180,180);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::AltitudeEditChange(TObject *Sender)
{
	Form1->CheckText(Form1->AltitudeEdit,false,NormFormat1l,NormFormat1,-MaxInt,MaxInt);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::DateEditChange(TObject *Sender)
{
	Form1->CheckText(Form1->DateEdit,false,DateFormat,DateFormat,2005,2010);
}
//---------------------------------------------------------------------------
bool TForm1::GetInput(float& alt, float& dlat, float& dlon, float& time, float& COM) //Gets input from fields
{
	if(Form1->LocationPageControl->ActivePageIndex==0) //Deg, min, and sec fields are active
	{
		//Insert 0s if the fields are left blank
		if(Form1->LatDegEdit->Text.Trim()=="")
		{
			Form1->LatDegEdit->Text="0";
		}
		if(Form1->LatMinEdit->Text.Trim()=="")
		{
			Form1->LatMinEdit->Text="0";
		}
		if(Form1->LatSecEdit->Text.Trim()=="")
		{
			Form1->LatSecEdit->Text="0";
		}
		if(Form1->LonDegEdit->Text.Trim()=="")
		{
			Form1->LonDegEdit->Text="0";
		}
		if(Form1->LonMinEdit->Text.Trim()=="")
		{
			Form1->LonMinEdit->Text="0";
		}
		if(Form1->LonSecEdit->Text.Trim()=="")
		{
			Form1->LonSecEdit->Text="0";
		}

		if(!Form1->CheckText(Form1->LatDegEdit,true,NormFormat1l,NormFormat1,0,90))
		{
			Form1->FocusControl(Form1->LatDegEdit);
			Form1->LatDegEdit->SelectAll();
			return false;
		}
		if(!Form1->CheckText(Form1->LonDegEdit,true,NormFormat1l,NormFormat1,0,180))
		{
			Form1->FocusControl(Form1->LonDegEdit);
			Form1->LonDegEdit->SelectAll();
			return false;
		}

		float value;
		value=Form1->GetValue(Form1->LatDegEdit);
		if(value==90)
		{
			//If the lat deg field is 90, the min and sec must be 0
			if(!Form1->CheckText(Form1->LatMinEdit,true,NormFormat1l,NormFormat1,0,0))
			{
				Form1->FocusControl(Form1->LatMinEdit);
				Form1->LatMinEdit->SelectAll();
				return false;
			}
			if(!Form1->CheckText(Form1->LatSecEdit,true,NormFormat1l,NormFormat1,0,0))
			{
				Form1->FocusControl(Form1->LatSecEdit);
				Form1->LatSecEdit->SelectAll();
				return false;
			}
		}
		else
		{
			//Otherwise min and sec can be 0-59
			if(!Form1->CheckText(Form1->LatMinEdit,true,NormFormat1l,NormFormat1,0,59))
			{
				Form1->FocusControl(Form1->LatMinEdit);
				Form1->LatMinEdit->SelectAll();
				return false;
			}
			if(!Form1->CheckText(Form1->LatSecEdit,true,NormFormat1l,NormFormat1,0,59))
			{
				Form1->FocusControl(Form1->LatSecEdit);
				Form1->LatSecEdit->SelectAll();
				return false;
			}
		}

		value=Form1->GetValue(Form1->LonDegEdit);
		if(value==180)
		{
			//If the lat deg field is 180, the min and sec must be 0
			if(!Form1->CheckText(Form1->LonMinEdit,true,NormFormat1l,NormFormat1,0,0))
			{
				Form1->FocusControl(Form1->LonMinEdit);
				Form1->LonMinEdit->SelectAll();
				return false;
			}
			if(!Form1->CheckText(Form1->LonSecEdit,true,NormFormat1l,NormFormat1,0,0))
			{
				Form1->FocusControl(Form1->LonSecEdit);
				Form1->LonSecEdit->SelectAll();
				return false;
			}
		}
		else
		{
			//Otherwise min and sec can be 0-59
			if(!Form1->CheckText(Form1->LonMinEdit,true,NormFormat1l,NormFormat1,0,59))
			{
				Form1->FocusControl(Form1->LonMinEdit);
				Form1->LonMinEdit->SelectAll();
				return false;
			}
			if(!Form1->CheckText(Form1->LonSecEdit,true,NormFormat1l,NormFormat1,0,59))
			{
				Form1->FocusControl(Form1->LonSecEdit);
				Form1->LonSecEdit->SelectAll();
				return false;
			}
		}
		//Get lat and lon
		dlat=(float)(Form1->GetValue(Form1->LatDegEdit)+Form1->GetValue(Form1->LatMinEdit)/60+Form1->GetValue(Form1->LatSecEdit)/3600);
		dlon=(float)(Form1->GetValue(Form1->LonDegEdit)+Form1->GetValue(Form1->LonMinEdit)/60+Form1->GetValue(Form1->LonSecEdit)/3600);
		if(Form1->SouthRadio->Checked)
		{
			dlat=-1*dlat;
		}
		if(Form1->WestRadio->Checked)
		{
			dlon=-1*dlon;
		}
	}
	if(Form1->LocationPageControl->ActivePageIndex==1) //Decimal deg fields active
	{
		//Insert 0s if the fields are left blank
		if(Form1->LatDecEdit->Text.Trim()=="")
		{
			Form1->LatDecEdit->Text="0";
		}
		if(Form1->LonDecEdit->Text.Trim()=="")
		{
			Form1->LonDecEdit->Text="0";
		}

		if(!Form1->CheckText(Form1->LatDecEdit,false,NormFormat1l,NormFormat1,-90,90))
		{
			Form1->FocusControl(Form1->LatDecEdit);
			Form1->LatDecEdit->SelectAll();
			return false;
		}
		if(!Form1->CheckText(Form1->LonDecEdit,false,NormFormat1l,NormFormat1,-180,180))
		{
			Form1->FocusControl(Form1->LonDecEdit);
			Form1->LonDecEdit->SelectAll();
			return false;
		}
		dlat=(float)Form1->GetValue(Form1->LatDecEdit);
		dlon=(float)Form1->GetValue(Form1->LonDecEdit);
	}
	if(Form1->LocationPageControl->ActivePageIndex==2) //UTM fields active
	{
		//Insert 0s if the fields are left blank
		if(Form1->NorthingEdit->Text.Trim()=="")
		{
			Form1->NorthingEdit->Text="0";
		}
		if(Form1->EastingEdit->Text.Trim()=="")
		{
			Form1->EastingEdit->Text="0";
		}

		//Northing limited from Min_Northing to Max_Northing
		if(!Form1->CheckText(Form1->NorthingEdit,false,NormFormat1l,NormFormat1,Min_Northing,Max_Northing))
		{
			Form1->FocusControl(Form1->NorthingEdit);
			Form1->NorthingEdit->SelectAll();
			return false;
		}
		//Easting limited from Min_Easting to Max_Easting
		if(!Form1->CheckText(Form1->EastingEdit,false,NormFormat1l,NormFormat1,Min_Easting,Max_Easting))
		{
			Form1->FocusControl(Form1->EastingEdit);
			Form1->EastingEdit->SelectAll();
			return false;
		}
		double X=(float)Form1->GetValue(Form1->EastingEdit);
		double Y=(float)Form1->GetValue(Form1->NorthingEdit);
		double pscale,CoM,Lambda,Phi;
		char hemi='N';
		if(Form1->UTMNorthRadioButton->Checked)
		{
			hemi='N';
		}
		if(Form1->UTMSouthRadioButton->Checked)
		{
			hemi='S';
		}

		//Get lat and lon
		UTMinv(Form1->LonZoneCombo->ItemIndex+1, hemi, 6, 6, 0, X, Y, &Lambda, &Phi, &pscale, &CoM);
		dlat=Phi;
		dlon=Lambda;
		COM=CoM;

	}

	if(Form1->DatePageControl->ActivePageIndex==0) //Traditional date page active
	{
		unsigned short year,month,day;
		Form1->DatePicker->Date.DecodeDate(&year,&month,&day);
		time=year+Form1->YearFraction(month,day);
	}
	else
	{
		//Decimal date page active
		if(Form1->DateEdit->Text.Trim()=="")
		{
			Form1->DateEdit->Text="0";
		}

		if(!Form1->CheckText(Form1->DateEdit,false,DateFormat,DateFormat,2005,2010))
		{
			Form1->FocusControl(Form1->DateEdit);
			Form1->DateEdit->SelectAll();
			return false;
		}
		time=(float)Form1->GetValue(Form1->DateEdit);
	}

	//Insert 0s if the fields are left blank
	if(Form1->AltitudeEdit->Text.Trim()=="")
	{
		Form1->AltitudeEdit->Text="0";
	}

	//Altitude not limited
	if(!Form1->CheckText(Form1->AltitudeEdit,false,NormFormat1l,NormFormat1,-MaxInt,MaxInt))
	{
		Form1->FocusControl(Form1->AltitudeEdit);
		Form1->AltitudeEdit->SelectAll();
		return false;
	}
	alt=(float)Form1->GetValue(Form1->AltitudeEdit)/1000;
	if(Form1->FeetRadio->Checked)
	{
		//Convert meters to feet
		alt=alt*0.3048;
	}

	return true;
}


bool TForm1::CopyToCB(AnsiString astring)
{
	//Coppies a string to the clipboard
	TClipboard * CB = Clipboard ();
	CB->SetTextBuf(astring.c_str());
	return true;
}
void __fastcall TForm1::SolutionStringGridKeyDown(TObject *Sender, WORD &Key,
	  TShiftState Shift)
{
	if(Shift.Contains(ssCtrl) && (Key==67 || Key==88))  //67=c 88=x
	{
		Form1->SolutionStringGrid->CopySelectionToCB("\t","\n");
	}
	if(Shift.Contains(ssCtrl) && Key==65) //65=a
	{
		Form1->SolutionStringGrid->SelectAll();
	}
}
//---------------------------------------------------------------------------

void __fastcall TForm1::CopyPopupMenuClick(TObject *Sender)
{
	Form1->SolutionStringGrid->CopySelectionToCB("\t","\n");
}
//---------------------------------------------------------------------------

void __fastcall TForm1::SelectAllPopupMenuClick(TObject *Sender)
{
	Form1->SolutionStringGrid->SelectAll();
}
//---------------------------------------------------------------------------

void __fastcall TForm1::About2Click(TObject *Sender)
{
	//MessageBox(Form1->Handle,"The World Magnetic Model is a product of the United States National Geospatial-Intelligence Agency (NGA). The U.S. National Geophysical Data Center (NGDC) and the British Geological Survey (BGS) produced the WMM with funding provided by NGA in the USA and by the Defence Geographic Imagery and Intelligence Agency (DGIA) in the UK.\n\nThe World Magnetic Model is the standard model of the US Department of Defense, the UK Ministry of Defence, the North Atlantic Treaty Organization (NATO), and the World Hydrographic Office (WHO) navigation and attitude/heading referencing systems. It is also used widely in civilian navigation systems. The model, associated software, and documentation are distributed by NGDC on behalf of NGA. The model is produced at 5-year intervals, with the current model expiring on December 31, 2009.\n\nVisit the WMM home page for more information: http://www.ngdc.noaa.gov/seg/WMM/DoDWMM.shtml\n","About WMM 2005",MB_OK|MB_ICONINFORMATION);
	AboutForm->ShowModal();
}
//---------------------------------------------------------------------------

void __fastcall TForm1::NorthingEditChange(TObject *Sender)
{
	Form1->CheckText(Form1->NorthingEdit,false,NormFormat1l,NormFormat1,Min_Northing,Max_Northing);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::EastingEditChange(TObject *Sender)
{
	Form1->CheckText(Form1->EastingEdit,false,NormFormat1l,NormFormat1,Min_Easting,Max_Easting);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::Help1Click(TObject *Sender)
{
	HelpForm->ShowModal();
}
//---------------------------------------------------------------------------

float TForm1::ABS(float value)
{
	//Returns the absolute value of the entered float
	if(value<0)
	{
		return -value;
	}
	return value;
}
void __fastcall TForm1::Copy2MenuItemClick(TObject *Sender)
{
	Form1->UTMStringGrid->CopySelectionToCB("\t","\n");
}
//---------------------------------------------------------------------------

void __fastcall TForm1::SelectAll2MenuItemClick(TObject *Sender)
{
	Form1->UTMStringGrid->SelectAll();
}
//---------------------------------------------------------------------------

void __fastcall TForm1::UTMStringGridKeyDown(TObject *Sender, WORD &Key,
	  TShiftState Shift)
{
	if(Shift.Contains(ssCtrl) && (Key==67 || Key==88))  //67=c 88=x
	{
		Form1->UTMStringGrid->CopySelectionToCB("\t","\n");
	}
	if(Shift.Contains(ssCtrl) && Key==65) //65=a
	{
		Form1->UTMStringGrid->SelectAll();
	}
}
//---------------------------------------------------------------------------

AnsiString TForm1::GetDMS(float value)
{
	//Returns a string representation of the passed value as an angle with degrees, minutes, and seconds
	AnsiString Temp="";
	int deg,min,sec;

	if(value<0)
	{
		Temp+="- ";
		value=fabs(value);
	}

	deg = (int)value;
	value = (value-deg)*60;
	min=value;
	value = (value-min)*60;
	sec=value;
	value=value-sec;
	if(value>=0.5) //Round instead of floor
	{
		sec++;
	}
	if(sec>=60) //Increase the min field if sec is 60
	{
		min++;
		sec-=60;
	}
	if(min>=60) //Increase the deg field if min is 60
	{
		deg++;
		min-=60;
	}

	Temp=Temp+AnsiString::FormatFloat("###0",(float)deg)+"\° ";
	Temp=Temp+AnsiString::FormatFloat("##00",(float)min)+"\' "; //Leading 0 for a single digit minute
	Temp=Temp+AnsiString::FormatFloat("##00",(float)sec)+"\'' "; //Leading 0 for a single digit second

	return Temp.Trim();
}

AnsiString TForm1::GetDM(float value,int places)
{
	//Returns a string representation of the passed value as an angle with degrees and minutes  with "places" decimal points on the minute
	AnsiString Temp="";
	AnsiString MinFormat="##00"; //Leading 0 for a single digit minute
	int deg;
	float min;

	if(value<0) //Add only one "-" per returned value
	{
		Temp+="- ";
		value=fabs(value);
	}

	deg = (int)value;

	if(places>0) //Change the minute format for the number of places
	{
		MinFormat+=".";
		for(int i=0; i<places; i++)
		{
			MinFormat+="0";
		}
	}


	value = fabs((value-deg)*60)*pow(10,places);
	min = (int)value;

	if(value-min>=0.5) //Round
	{
		min++;
	}
	min=min/pow(10,places);

	if(min>=60) //Increase the deg field if min is 60
	{
		deg++;
		min-=60;
	}


	Temp=Temp+AnsiString::FormatFloat("###0",(float)deg)+"\° ";
	Temp=Temp+AnsiString::FormatFloat(MinFormat,(float)min)+"\' ";

	return Temp.Trim();
}

//Used to make it so that 2546 will be reported as 2546 instead of 2,546 but 25467 will still be 25,467
AnsiString TForm1::Format(AnsiString formatlow,AnsiString formathigh,float value,float cutoff)
{
	if(value<cutoff && value>-cutoff)
	{
		return AnsiString::FormatFloat(formatlow,value);
	}
	return AnsiString::FormatFloat(formathigh,value);
}
