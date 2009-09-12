//---------------------------------------------------------------------------

#ifndef CStringGridH
#define CStringGridH
//---------------------------------------------------------------------------
#include <SysUtils.hpp>
#include <Classes.hpp>
#include <Controls.hpp>
#include <Grids.hpp>
//---------------------------------------------------------------------------
//enum TextAlign {LeftH=DT_LEFT,CenterH=DT_CENTER,RightH=DT_RIGHT};

class PACKAGE TCustomStringGrid : public TStringGrid
{
private:
	void CopyStringToCB(AnsiString astring);
	int TextDrawOptions;
protected:
	void __fastcall DrawCell(int ACol, int ARow, const TRect & ARect, TGridDrawState AState);

public:
	__fastcall TCustomStringGrid(TComponent* Owner);
__published:
	void SelectAll();
	void SetSelection(int top,int left, int bottom, int right);
	void CopySelectionToCB(AnsiString ColSep,AnsiString RowSep);
	void Clear(int top, int left, int bottom, int right);
	void ClearAll(bool fixed);
	void SetTextOptions(int options);
	__property OnClick;

};
//---------------------------------------------------------------------------
#endif
