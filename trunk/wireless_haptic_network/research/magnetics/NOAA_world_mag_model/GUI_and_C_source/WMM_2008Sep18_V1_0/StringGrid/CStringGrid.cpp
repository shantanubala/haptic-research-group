//---------------------------------------------------------------------------

#include <vcl.h>

#pragma hdrstop

#include "CStringGrid.h"
#include <clipbrd.hpp>
#pragma package(smart_init)
//---------------------------------------------------------------------------
// ValidCtrCheck is used to assure that the components created do not have
// any pure virtual functions.
//

static inline void ValidCtrCheck(TCustomStringGrid *)
{
	new TCustomStringGrid(NULL);
}
//---------------------------------------------------------------------------
__fastcall TCustomStringGrid::TCustomStringGrid(TComponent* Owner)
	: TStringGrid(Owner)
{
	this->DefaultDrawing=true;
	this->TextDrawOptions=(DT_CENTER | DT_SINGLELINE | DT_VCENTER);
}
//---------------------------------------------------------------------------
namespace Cstringgrid
{
	void __fastcall PACKAGE Register()
	{
		TComponentClass classes[1] = {__classid(TCustomStringGrid)};
		RegisterComponents("Samples", classes, 0);
	}
}
//---------------------------------------------------------------------------
void TCustomStringGrid::SelectAll()
{
	//this->SetFocus();
	if(this->FixedCols<this->ColCount && this->FixedRows<this->RowCount)
	{
		this->SetSelection(this->FixedRows,this->FixedCols,this->RowCount-1,this->ColCount-1);
		//this->FocusCell(this->FixedCols,this->FixedRows,true);
		//this->FocusCell(this->ColCount-1,this->RowCount-1,false);
	}
	//this->SetSelection()
}
void TCustomStringGrid::SetSelection(int top,int left, int bottom, int right)
{
	if(top<this->FixedRows)
	{
		top=this->FixedRows;
	}
	if(left<this->FixedCols)
	{
		left=this->FixedCols;
	}
	if(right>=this->ColCount)
	{
		right=this->ColCount;
	}
	if(bottom>=this->RowCount)
	{
		bottom=this->RowCount;
	}

	if(right<left || bottom<top)
	{
		return;
	}

	TGridRect sel;
	sel.TopLeft.X=left;
	sel.TopLeft.Y=top;
	sel.BottomRight.X=right;
	sel.BottomRight.Y=bottom;
	this->SetFocus();
	this->Selection=sel;
	this->Repaint();
}
void TCustomStringGrid::CopySelectionToCB(AnsiString ColSep,AnsiString RowSep)
{
	AnsiString astring="";
	for(int i=this->Selection.Top;i<=this->Selection.Bottom;i++)
	{
		for(int j=this->Selection.Left;j<=this->Selection.Right;j++)
		{
			astring+=this->Cells[j][i];
			if(j!=this->Selection.Right)
			{
				//astring+="\t";
				astring+=ColSep;
			}
		}
		if(i!=this->Selection.Bottom)
		{
			//astring+="\n";
			astring+=RowSep;
		}
	}
	this->CopyStringToCB(astring);
}

void TCustomStringGrid::CopyStringToCB(AnsiString astring)
{
	TClipboard * CB = Clipboard ();
	CB->SetTextBuf(astring.c_str());
}

void __fastcall TCustomStringGrid::DrawCell(int ACol, int ARow, const TRect & ARect, TGridDrawState AState)
{
	if(this->Enabled)
	{
		this->Canvas->Font->Color=this->Font->Color;
		//this->Canvas->Font->Color=clGrayText;
		if(AState.Contains(gdSelected) && !AState.Contains(gdFocused))
		{
			this->Canvas->Font->Color=clHighlightText;
		}
		RECT temp = Rect(ARect.left+1,ARect.Top+1,ARect.Right-1,ARect.Bottom-1);
		//DrawText(this->Canvas->Handle,this->Cells[ACol][ARow].c_str(),-1,&temp,DT_CENTER|DT_VCENTER|DT_SINGLELINE);
		DrawText(this->Canvas->Handle,this->Cells[ACol][ARow].c_str(),-1,&temp,this->TextDrawOptions);
	}
	else
	{
		this->Canvas->Font->Color=clGrayText;
		if(AState.Contains(gdSelected) && !AState.Contains(gdFocused))
		{
			this->Canvas->Font->Color=clInactiveCaptionText;
		}
		RECT temp = Rect(ARect.left+1,ARect.Top+1,ARect.Right-1,ARect.Bottom-1);
		DrawText(this->Canvas->Handle,this->Cells[ACol][ARow].c_str(),-1,&temp,this->TextDrawOptions);
    }
}

void TCustomStringGrid::Clear(int top, int left, int bottom, int right)
{
	if(top<0 || left<0 || bottom>=this->RowCount || right>=ColCount || top>bottom || left>right)
	{
		return;
    }
	for(int i=left; i<=right;i++)
	{
		for(int j=top; j<=bottom;j++)
		{
			this->Cells[i][j]="";
		}
	}
}
void TCustomStringGrid::ClearAll(bool fixed)
{
	if(fixed) //Clear the fixed cells too
	{
		this->Clear(0,0,this->RowCount-1,this->ColCount-1);
	}
	else
	{
		this->Clear(this->FixedRows,this->FixedCols,this->RowCount-1,this->ColCount-1);
    }
}
void TCustomStringGrid::SetTextOptions(int options)
{
	this->TextDrawOptions=options;
	this->Repaint();
}
