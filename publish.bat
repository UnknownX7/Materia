set PROJECT=Materia
set OUTPUT=dist

rmdir /s /q %OUTPUT%
mkdir %OUTPUT%
dotnet publish %PROJECT% -c Release -r win-x64 --sc -o %OUTPUT%\lib
move /y %OUTPUT%\lib\runtimes\win-x64\native\* %OUTPUT%\lib
rmdir /s /q %OUTPUT%\lib\runtimes
move %OUTPUT%\lib\%PROJECT%* %OUTPUT%
copy deps\ECGen\symbols.bin %OUTPUT%
copy deps\Doorstop\winhttp.dll %OUTPUT%
copy deps\Doorstop\install.bat %OUTPUT%
copy deps\Doorstop\uninstall.bat %OUTPUT%
powershell -command "Compress-Archive %OUTPUT%\* -f %PROJECT%.zip"
rmdir /s /q %OUTPUT%
pause