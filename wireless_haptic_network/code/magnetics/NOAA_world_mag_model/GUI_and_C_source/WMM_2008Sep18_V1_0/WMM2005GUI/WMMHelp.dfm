object HelpForm: THelpForm
  Left = 256
  Top = 154
  BorderStyle = bsDialog
  Caption = 'World Magnetic Model 2005 Help'
  ClientHeight = 353
  ClientWidth = 531
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poDesktopCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object HelpCloseButton: TButton
    Left = 228
    Top = 320
    Width = 75
    Height = 25
    Caption = 'Close'
    Default = True
    TabOrder = 0
    OnClick = HelpCloseButtonClick
  end
  object RichEdit1: TRichEdit
    Left = 7
    Top = 8
    Width = 517
    Height = 306
    TabStop = False
    BevelInner = bvNone
    BevelOuter = bvNone
    Color = clBtnFace
    ReadOnly = True
    ScrollBars = ssVertical
    TabOrder = 1
    WantTabs = True
    WantReturns = False
  end
end
