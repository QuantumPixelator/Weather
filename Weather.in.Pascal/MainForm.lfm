
Unit WeatherLogic;

{$mode objfpc}{$H+}

Interface

Uses 
Classes, SysUtils, fphttpclient, fpjson, jsonparser, opensslsockets;

Type 
  TConfig = Record
    ApiKey: string;
    ZipCode: string;
    LastAlerts: string;
    WindowLeft: Integer;
    WindowTop: Integer;
  End;

  TWeatherData = Record
    LocationName: string;
    TempF: Double;
    ConditionText: string;
    ConditionIconUrl: string;
    WindMph: Double;
    MoonPhase: string;
    Alerts: string;
    HasAlerts: Boolean;
    Success: Boolean;
    ErrorMessage: string;
  End;

Function LoadConfig(Const FileName: String): TConfig;
Procedure SaveConfig(Const FileName: String; Const Config: TConfig);
Function GetWeatherData(Const Config: TConfig): TWeatherData;


Implementation

Function LoadConfig(Const FileName: String): TConfig;

Var 
  JSONString: string;
  JSONData: TJSONData;
  JSONObject: TJSONObject;
  F: TFileStream;
Begin
  Result.ApiKey := '';
  Result.ZipCode := '';
  Result.LastAlerts := '';
  Result.WindowLeft := 286;
  Result.WindowTop := 153;
  If Not FileExists(FileName) Then exit;

  F := TFileStream.Create(FileName, fmOpenRead Or fmShareDenyWrite);
  Try
    If F.Size > 0 Then
      Begin
        SetLength(JSONString, F.Size);
        F.Read(JSONString[1], F.Size);
      End;
  Finally
    F.Free;
End;

If JSONString = '' Then exit;

Try
  JSONData := GetJSON(JSONString);
  Try
    If JSONData.JSONType = jtObject Then
      Begin
        JSONObject := TJSONObject(JSONData);
        Result.ApiKey := JSONObject.Get('api_key', '');
        Result.ZipCode := JSONObject.Get('zip_code', '');
        Result.LastAlerts := JSONObject.Get('last_alerts', '');
        Result.WindowLeft := JSONObject.Get('window_left', 286);
        Result.WindowTop := JSONObject.Get('window_top', 153);
      End;
  Finally
    JSONData.Free;
End;
Except
End;
End;

Procedure SaveConfig(Const FileName: String; Const Config: TConfig);

Var 
  JSONObject: TJSONObject;
  JSONString: string;
  F: TFileStream;
Begin
  JSONObject := TJSONObject.Create;
  Try
    JSONObject.Add('api_key', Config.ApiKey);
    JSONObject.Add('zip_code', Config.ZipCode);
    JSONObject.Add('last_alerts', Config.LastAlerts);
    JSONObject.Add('window_left', Config.WindowLeft);
    JSONObject.Add('window_top', Config.WindowTop);
    JSONString := JSONObject.FormatJSON;

    F := TFileStream.Create(FileName, fmCreate);
    Try
      If Length(JSONString) > 0 Then
        F.Write(JSONString[1], Length(JSONString));
    Finally
      F.Free;
End;
Finally
  JSONObject.Free;
End;
End;

Function GetWeatherData(Const Config: TConfig): TWeatherData;

Var 
  Client: TFPHTTPClient;
  URL, Response: string;
  JSONData, CurrentObj, ConditionObj, AstroObj: TJSONData;
  AlertsArray: TJSONArray;
  i: Integer;
Begin
  Result.Success := False;
  Result.ErrorMessage := '';
  Result.LocationName := '';
  Result.TempF := 0;
  Result.ConditionText := '';
  Result.ConditionIconUrl := '';
  Result.WindMph := 0;
  Result.MoonPhase := '';
  Result.Alerts := '';
  Result.HasAlerts := False;

  If Config.ApiKey = '' Then
    Begin
      Result.ErrorMessage := 'No API key';
      exit;
    End;

  Client := TFPHTTPClient.Create(Nil);
  Try
    Try
      URL := 'http://api.weatherapi.com/v1/current.json?key=' + Config.ApiKey +
             '&q=' + Config.ZipCode;
      Response := Client.Get(URL);
      JSONData := GetJSON(Response);
      Try
        CurrentObj := JSONData.FindPath('current');
        If Assigned(CurrentObj) Then
          Begin
            Result.TempF := CurrentObj.FindPath('temp_f').AsFloat;
            Result.WindMph := CurrentObj.FindPath('wind_mph').AsFloat;

            ConditionObj := CurrentObj.FindPath('condition');
            If Assigned(ConditionObj) Then
              Begin
                Result.ConditionText := ConditionObj.FindPath('text').AsString;
                Result.ConditionIconUrl := 'http:' + ConditionObj.FindPath(
                                           'icon'
                                           ).AsString;
              End;
          End;

        Result.LocationName := JSONData.FindPath('location.name').AsString;
      Finally
        JSONData.Free;
End;

URL := 'http://api.weatherapi.com/v1/astronomy.json?key=' + Config.ApiKey
       + '&q=' + Config.ZipCode + '&dt=' + FormatDateTime('yyyy-mm-dd',
       Now);
Response := Client.Get(URL);
JSONData := GetJSON(Response);
Try
  AstroObj := JSONData.FindPath('astronomy.astro');
  If Assigned(AstroObj) Then
    Begin
      Result.MoonPhase := AstroObj.FindPath('moon_phase').AsString;
    End;
Finally
  JSONData.Free;
End;

URL := 'http://api.weatherapi.com/v1/forecast.json?key=' + Config.ApiKey +
       '&q=' + Config.ZipCode + '&alerts=yes';
Response := Client.Get(URL);
JSONData := GetJSON(Response);
Try
  AlertsArray := TJSONArray(JSONData.FindPath('alerts.alert'));
  If Assigned(AlertsArray) And (AlertsArray.Count > 0) Then
    Begin
      Result.HasAlerts := True;
      Result.Alerts := '';
      For i := 0 To AlertsArray.Count - 1 Do
        Begin
          If i > 0 Then Result.Alerts := Result.Alerts + #13#10 +
                                         '---' + #13#10;
          Result.Alerts := Result.Alerts + 'Event: ' + AlertsArray.Items[i]
                           .FindPath('event').AsString + #13#10;
          Result.Alerts := Result.Alerts + 'Severity: ' + AlertsArray.Items
                           [i].FindPath('severity').AsString + #13#10;
          Result.Alerts := Result.Alerts + AlertsArray.Items[i].FindPath(
                           'desc').AsString;
        End;
    End;
Finally
  JSONData.Free;
End;

Result.Success := True;
Except
  On E: Exception Do
        Begin
          Result.ErrorMessage := 'Error: ' + E.Message;
          Result.Success := False;
        End;
End;
Finally
  Client.Free;
End;
End;

End.

Procedure TForm1.FormClose(Sender: TObject; Var CloseAction: TCloseAction);
Begin
  SaveWindowPosition;
End;

Procedure TForm1.fetchButtonClick(Sender: TObject);
Begin
  FConfig.ZipCode := zipTextBox.Text;
  UpdateUI;
End;

Procedure TForm1.fetchButtonMouseEnter(Sender: TObject);
Begin
  fetchButton.Color := clBlack;
  fetchButton.Font.Color := clSkyBlue;
End;

Procedure TForm1.fetchButtonMouseLeave(Sender: TObject);
Begin
  fetchButton.Color := clDefault;
  fetchButton.Font.Color := clDefault;
End;

Procedure TForm1.alertButtonClick(Sender: TObject);

Var 
  AlertForm: TForm;
  Memo: TMemo;
  CloseBtn: TButton;
Begin
  AlertForm := TForm.Create(Nil);
  Try
    AlertForm.Caption := 'Weather Alerts';
    AlertForm.SetBounds(0, 0, 400, 300);
    AlertForm.Position := poMainFormCenter;
    AlertForm.Color := $1E1E1E;

    Memo := TMemo.Create(AlertForm);
    Memo.Parent := AlertForm;
    Memo.Align := alClient;
    Memo.ReadOnly := True;
    Memo.Color := $323232;
    Memo.Font.Color := clWhite;
    Memo.ScrollBars := ssVertical;
    Memo.Lines.Text := FAlertText;

    CloseBtn := TButton.Create(AlertForm);
    CloseBtn.Parent := AlertForm;
    CloseBtn.Align := alBottom;
    CloseBtn.Caption := 'Close';
    CloseBtn.ModalResult := mrOk;

    AlertForm.ShowModal;
  Finally
    AlertForm.Free;
End;
// Mark alerts as viewed: persist the last alerts text so we can
// detect new alerts on subsequent updates.
FConfig.LastAlerts := FAlertText;
SaveConfig('config.json', FConfig);
alertButton.Font.Color := clDefault;
End;

Procedure TForm1.updateTimerTimer(Sender: TObject);
Begin
  UpdateUI;
End;

Procedure TForm1.UpdateUI;

Var 
  Data: TWeatherData;
Begin
  Data := GetWeatherData(FConfig);
  If Data.Success Then
    Begin
      nameLabel.Caption := Data.LocationName;
      tempLabel.Caption := FormatFloat('0.0', Data.TempF) + ' °F';
      weatherLabel.Caption := Data.ConditionText;
      windLabel.Caption := FormatFloat('0.0', Data.WindMph) + ' mph';
      moonLabel.Caption := 'Moon Phase: ' + Data.MoonPhase;
      lastUpdateLabel.Caption := 'Updated: ' + FormatDateTime('hh:nn AM/PM', Now
                                 );

      FAlertText := Data.Alerts;
      FHasAlerts := Data.HasAlerts;
      alertButton.Enabled := FHasAlerts;
      // If there are alerts and they differ from the last viewed alerts,
      // make the alert button text red to show there's an unviewed alert.
      If FHasAlerts Then
        Begin
          If FConfig.LastAlerts <> FAlertText Then
            alertButton.Font.Color := clRed
          Else
            alertButton.Font.Color := clDefault;
        End
      Else
        alertButton.Font.Color := clDefault;

      If Data.ConditionIconUrl <> '' Then
        DownloadAndSetImage(Data.ConditionIconUrl, weatherPictureBox);
    End
  Else
    Begin
      tempLabel.Caption := Data.ErrorMessage;
      nameLabel.Caption := '';
      weatherLabel.Caption := '';
      windLabel.Caption := '';
      lastUpdateLabel.Caption := '';
      weatherPictureBox.Picture := Nil;
    End;
End;

Procedure TForm1.DownloadAndSetImage(Const URL: String; TargetImage: TImage);

Var 
  Client: TFPHTTPClient;
  MS: TMemoryStream;
Begin
  MS := TMemoryStream.Create;
  Client := TFPHTTPClient.Create(Nil);
  Try
    Try
      Client.AllowRedirect := True;
      Client.Get(URL, MS);
      MS.Position := 0;
      If MS.Size > 0 Then
        TargetImage.Picture.LoadFromStream(MS);
    Except
      TargetImage.Picture := Nil;
End;
Finally
  Client.Free;
  MS.Free;
End;
End;

Procedure TForm1.SaveWindowPosition;
Begin
  FConfig.WindowLeft := Left;
  FConfig.WindowTop := Top;
  SaveConfig('config.json', FConfig);
End;

Procedure TForm1.LoadWindowPosition;
Begin
  Left := FConfig.WindowLeft;
  Top := FConfig.WindowTop;
End;

End.
