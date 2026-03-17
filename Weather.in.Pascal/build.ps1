# Clean any existing compiled files
Write-Host "Compiling resources..." -ForegroundColor Cyan

# Compile resource file to .res format
& "C:\lazarus\fpc\3.2.2\bin\x86_64-win64\windres.exe" resource.rc Weather.res

Write-Host "Compiling Weather App..." -ForegroundColor Cyan

# Compile the project
& "C:\lazarus\fpc\3.2.2\bin\x86_64-win64\ppcx64.exe" `
    -MObjFPC -Scghi -CX -O2 -Xs -XX -vewnhibq -WG `
    -FiC:\lazarus\lcl\units\x86_64-win64\win32 `
    -FiC:\lazarus\lcl\units\x86_64-win64 `
    -FiC:\lazarus\packager\units\x86_64-win64 `
    -FuC:\lazarus\lcl\units\x86_64-win64\win32 `
    -FuC:\lazarus\lcl\units\x86_64-win64 `
    -FuC:\lazarus\packager\units\x86_64-win64 `
    -FuC:\lazarus\components\lazutils\lib\x86_64-win64 `
    -Fu"." `
    -FU"." `
    -FE"." `
    -oWeather.exe `
    -dLCL `
    -dLCLwin32 `
    Weather.lpr

if ($LASTEXITCODE -eq 0) {
    # Ensure the file has .exe extension
    if (Test-Path "Weather") {
        if (Test-Path "Weather.exe") {
            Remove-Item "Weather.exe" -Force
        }
        Rename-Item "Weather" "Weather.exe" -Force
    }
    Write-Host "Build Successful: Weather.exe" -ForegroundColor Green
}
else {
    Write-Host "Build Failed" -ForegroundColor Red
}
