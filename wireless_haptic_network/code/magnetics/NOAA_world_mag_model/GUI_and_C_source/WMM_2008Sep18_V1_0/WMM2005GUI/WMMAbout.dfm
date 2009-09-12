object AboutForm: TAboutForm
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  BorderStyle = bsDialog
  Caption = 'World Magnetic Model 2005 About'
  ClientHeight = 254
  ClientWidth = 439
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
  object Memo1: TMemo
    Left = 7
    Top = 8
    Width = 424
    Height = 201
    TabStop = False
    BevelInner = bvNone
    BevelOuter = bvNone
    Color = clBtnFace
    Lines.Strings = (
      
        'This is a product of the National Geospatial-Intelligence Agency' +
        ' (U.S.A.), developed '
      
        'by the National Geophysical Data Center (U.S.A.), in consultatio' +
        'n with the British '
      'Geological Survey (U.K.).'
      ''
      
        'When installed with the correct coefficients file, WMM.COF (date' +
        'd 10/18/2004 in the '
      
        'first line), this program computes the elements of the World Mag' +
        'netic Model 2005.'
      '')
    ReadOnly = True
    TabOrder = 0
  end
  object AboutCloseButton: TButton
    Left = 169
    Top = 221
    Width = 75
    Height = 25
    Caption = 'Close'
    Default = True
    TabOrder = 1
    OnClick = AboutCloseButtonClick
  end
end
