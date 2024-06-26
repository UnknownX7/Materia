set PROJECT=Materia
set UPDATER=%PROJECT%.Updater
set OUTPUT=dist

rmdir /s /q %OUTPUT%
mkdir %OUTPUT%
dotnet publish %PROJECT% -c Release -r win-x64 --sc -o %OUTPUT%\lib
dotnet publish %UPDATER% -c Release -r win-x64 --no-self-contained -p:PublishSingleFile=true -o %OUTPUT%
move /y %OUTPUT%\lib\runtimes\win-x64\native\* %OUTPUT%\lib
rmdir /s /q %OUTPUT%\lib\runtimes
move %OUTPUT%\lib\%PROJECT%* %OUTPUT%
copy deps\ECGen\symbols.bin %OUTPUT%
copy deps\Doorstop\winhttp.dll %OUTPUT%
copy deps\Doorstop\install.bat %OUTPUT%
copy deps\Doorstop\uninstall.bat %OUTPUT%
powershell -command "Compress-Archive %OUTPUT%\* -f release.zip"
del %OUTPUT%\lib\ECGen.Generated.dll
powershell -command "Compress-Archive %OUTPUT%\lib\* -f deps\lib.zip"
rmdir /s /q %OUTPUT%\lib
del %OUTPUT%\symbols.bin
powershell -command "Compress-Archive %OUTPUT%\* -f update.zip"
rmdir /s /q %OUTPUT%
pause