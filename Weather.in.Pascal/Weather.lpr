
Program Weather;

{$mode objfpc}{$H+}

Uses 
  {$IFDEF UNIX}
cthreads,
  {$ENDIF}
Interfaces,
Forms, MainForm, WeatherLogic;

{$R *.res}

Begin
  RequireDerivedFormResource := True;
  Application.Scaled := True;
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
End.
