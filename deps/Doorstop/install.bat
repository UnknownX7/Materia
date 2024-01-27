@echo off
for /f "skip=2 tokens=2*" %%a in ('reg query "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 2484110" /v InstallLocation') do set INSTALLDIR=%%~b
if %INSTALLDIR:~-5% neq FF7EC (
    set /p INSTALLDIR=Unable to find FF7EC install directory, please input it: 
)

if exist "%INSTALLDIR%/FF7EC.exe" (
    echo Installing to "%INSTALLDIR%"
    (
        echo [General]
        echo enabled=true
        echo target_assembly="%~dp0Materia.dll"
        echo redirect_output_log=false
        echo ignore_disable_switch=false
        echo [Il2Cpp]
        echo coreclr_path="%~dp0lib\coreclr.dll"
        echo corlib_dir="%~dp0lib"
    ) > doorstop_config.ini
    move /y doorstop_config.ini "%INSTALLDIR%" > nul
    copy /y winhttp.dll "%INSTALLDIR%" > nul
    echo Done!
    echo !!!IF YOU MOVE THE FOLDER THIS SCRIPT IS CONTAINED IN, YOU WILL NEED TO RUN THIS INSTALLER AGAIN!!!
) else (
    echo Invalid directory
)

pause