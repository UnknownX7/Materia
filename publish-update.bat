set PROJECT=Materia
set OUTPUT=dist

rmdir /s /q %OUTPUT%
mkdir %OUTPUT%
dotnet publish %PROJECT% -c Release -r win-x64 --no-self-contained -o %OUTPUT%\lib
move %OUTPUT%\lib\%PROJECT%* %OUTPUT%
move %OUTPUT%\lib\ECGen.Generated.dll %OUTPUT%
rmdir /s /q %OUTPUT%\lib
mkdir %OUTPUT%\lib
move %OUTPUT%\ECGen.Generated.dll %OUTPUT%\lib
copy deps\ECGen\symbols.bin %OUTPUT%
powershell -command "Compress-Archive %OUTPUT%\* -f %PROJECT%-update.zip"
rmdir /s /q %OUTPUT%
pause