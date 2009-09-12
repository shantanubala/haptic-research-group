//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "WMMAbout.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TAboutForm *AboutForm;
//---------------------------------------------------------------------------
__fastcall TAboutForm::TAboutForm(TComponent* Owner)
	: TForm(Owner)
{
}
//---------------------------------------------------------------------------
void __fastcall TAboutForm::FormCreate(TObject *Sender)
{
	AnsiString date=__DATE__;
	AboutForm->Memo1->Lines->Add("Version: 1.0");
	AboutForm->Memo1->Lines->Add("Last updated on: "+date);
}
//---------------------------------------------------------------------------
void __fastcall TAboutForm::AboutCloseButtonClick(TObject *Sender)
{
	AboutForm->Close();	
}
//---------------------------------------------------------------------------
