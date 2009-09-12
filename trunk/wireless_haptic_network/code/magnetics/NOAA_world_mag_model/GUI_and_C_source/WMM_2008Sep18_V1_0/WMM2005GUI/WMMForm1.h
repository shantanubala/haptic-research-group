//---------------------------------------------------------------------------

#ifndef WMMForm1H
#define WMMForm1H
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
#include <ComCtrls.hpp>
#include <ExtCtrls.hpp>
#include <Menus.hpp>
#include <Grids.hpp>
#include "CStringGrid.h"

//---------------------------------------------------------------------------

class TForm1 : public TForm
{
__published:	// IDE-managed Components
	TPageControl *LocationPageControl;
	TTabSheet *TabSheet1;
	TTabSheet *TabSheet2;
	TGroupBox *LatDegGroupBox;
	TLabel *Label4;
	TEdit *LatMinEdit;
	TEdit *LatDegEdit;
	TLabel *Label3;
	TLabel *Label5;
	TEdit *LatSecEdit;
	TGroupBox *LonDegGroupBox;
	TLabel *Label2;
	TLabel *Label6;
	TLabel *Label7;
	TEdit *LonMinEdit;
	TEdit *LonDegEdit;
	TEdit *LonSecEdit;
	TGroupBox *LonDecGroupBox;
	TGroupBox *LatDecGroupBox;
	TEdit *LatDecEdit;
	TRadioButton *NorthRadio;
	TRadioButton *SouthRadio;
	TRadioButton *EastRadio;
	TRadioButton *WestRadio;
	TLabel *Label9;
	TEdit *LonDecEdit;
	TLabel *Label10;
	TGroupBox *AltitudeGroupBox;
	TEdit *AltitudeEdit;
	TRadioButton *MeterRadio;
	TRadioButton *FeetRadio;
	TPageControl *DatePageControl;
	TTabSheet *TabSheet3;
	TTabSheet *TabSheet4;
	TGroupBox *DateGroupBox;
	TDateTimePicker *DatePicker;
	TGroupBox *DecDateGroupBox;
	TEdit *DateEdit;
	TMainMenu *MainMenu1;
	TMenuItem *F1;
	TMenuItem *About1;
	TMenuItem *Exit1;
	TMenuItem *About2;
	TButton *CalculateButton;
	TLabel *ErrorLabel;
	TPopupMenu *PopupMenu1;
	TMenuItem *CopyPopupMenu;
	TMenuItem *SelectAllPopupMenu;
	TMenuItem *N1;
	TLabel *WarningLabel;
	TLabel *GeoWarnLabel;
	TLabel *MagWarnLabel;
	TCustomStringGrid *SolutionStringGrid;
	TTabSheet *TabSheet5;
	TGroupBox *GroupBox1;
	TGroupBox *GroupBox2;
	TComboBox *LonZoneCombo;
	TLabel *Label1;
	TLabel *Label8;
	TEdit *NorthingEdit;
	TEdit *EastingEdit;
	TLabel *Label11;
	TLabel *Label12;
	TMenuItem *Help1;
	TRadioButton *UTMNorthRadioButton;
	TRadioButton *UTMSouthRadioButton;
	TCustomStringGrid *UTMStringGrid;
	TPopupMenu *UTMPopupMenu;
	TMenuItem *Copy2MenuItem;
	TMenuItem *N2;
	TMenuItem *SelectAll2MenuItem;
	TLabel *Label13;
	TLabel *Label14;
	void __fastcall Exit1Click(TObject *Sender);
	void __fastcall FormShow(TObject *Sender);
	void __fastcall CalculateButtonClick(TObject *Sender);
	void __fastcall LatDegEditChange(TObject *Sender);
	void __fastcall LatMinEditChange(TObject *Sender);
	void __fastcall LatSecEditChange(TObject *Sender);
	void __fastcall LonDegEditChange(TObject *Sender);
	void __fastcall LonMinEditChange(TObject *Sender);
	void __fastcall LonSecEditChange(TObject *Sender);
	void __fastcall LatDecEditChange(TObject *Sender);
	void __fastcall LonDecEditChange(TObject *Sender);
	void __fastcall AltitudeEditChange(TObject *Sender);
	void __fastcall DateEditChange(TObject *Sender);
	void __fastcall SolutionStringGridKeyDown(TObject *Sender, WORD &Key,
          TShiftState Shift);
	void __fastcall CopyPopupMenuClick(TObject *Sender);
	void __fastcall SelectAllPopupMenuClick(TObject *Sender);
	void __fastcall About2Click(TObject *Sender);
	void __fastcall NorthingEditChange(TObject *Sender);
	void __fastcall EastingEditChange(TObject *Sender);
	void __fastcall Help1Click(TObject *Sender);
	void __fastcall Copy2MenuItemClick(TObject *Sender);
	void __fastcall SelectAll2MenuItemClick(TObject *Sender);
	void __fastcall UTMStringGridKeyDown(TObject *Sender, WORD &Key,
          TShiftState Shift);
private:	// User declarations
public:		// User declarations
	bool GetInput(float& alt, float& dlat, float& dlon, float& time, float& COM);
	float YearFraction(int month, int day);
	bool CheckText(TEdit* EditBox,bool integer,AnsiString Formatl,AnsiString Formath,float min,float max);
	float GetValue(TEdit* EditBox);
	bool CopyToCB(AnsiString astring);
	float ABS(float value);
	AnsiString GetDMS(float value);
	AnsiString GetDM(float value,int places);
	AnsiString Format(AnsiString formatlow,AnsiString formathigh,float value,float cutoff);
	__fastcall TForm1(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TForm1 *Form1;
//---------------------------------------------------------------------------
#endif
