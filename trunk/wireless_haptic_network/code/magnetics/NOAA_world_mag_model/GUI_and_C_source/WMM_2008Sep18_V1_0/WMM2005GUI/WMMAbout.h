//---------------------------------------------------------------------------

#ifndef WMMAboutH
#define WMMAboutH
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
//---------------------------------------------------------------------------
class TAboutForm : public TForm
{
__published:	// IDE-managed Components
	TMemo *Memo1;
	TButton *AboutCloseButton;
	void __fastcall FormCreate(TObject *Sender);
	void __fastcall AboutCloseButtonClick(TObject *Sender);
private:	// User declarations
public:		// User declarations
	__fastcall TAboutForm(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TAboutForm *AboutForm;
//---------------------------------------------------------------------------
#endif
