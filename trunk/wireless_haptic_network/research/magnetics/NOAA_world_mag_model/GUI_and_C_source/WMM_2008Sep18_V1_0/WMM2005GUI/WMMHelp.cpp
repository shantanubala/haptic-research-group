//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "WMMHelp.h"
#include "WMMForm1.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
THelpForm *HelpForm;
//---------------------------------------------------------------------------
__fastcall THelpForm::THelpForm(TComponent* Owner)
	: TForm(Owner)
{
}
//---------------------------------------------------------------------------
void __fastcall THelpForm::HelpCloseButtonClick(TObject *Sender)
{
	HelpForm->Close();	
}
//---------------------------------------------------------------------------
void __fastcall THelpForm::FormCreate(TObject *Sender)
{
	try
	{
		HelpForm->RichEdit1->Lines->LoadFromFile("WMM2005_HELP.rtf");
	}
	catch(Exception &E)
	{
		AnsiString text="The file 'WMM2005_HELP.rtf' was not found. The help function will be disabled until this file is replaced.";
		AnsiString cap="WMM2005_HELP.rtf not found";
		MessageBox(Form1->ClientHandle,text.c_str(),cap.c_str(),(MB_OK | MB_ICONWARNING));
		Form1->Help1->Enabled=false;
    }
}
//---------------------------------------------------------------------------

