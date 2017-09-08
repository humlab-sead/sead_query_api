
SET WINSCP="C:\Program Files (x86)\WinSCP\WinSCP.exe"
SET RELEASE=Release
SET PLATFORM=debian.8-x64
SET FRAMEWORK=netcoreapp2.0
SET FTP_SCRIPT=ftp_deploy_script.txt
SET PASSWORD=schi11er
SET WINSCP="C:\Program Files (x86)\WinSCP\WinSCP.exe"
SET SOURCE_DIR="C:\Users\roma0050\Documents\Projects\SEAD\query_sead_api_core"
SET BUILD_DIR="%SOURCE_DIR%\query_sead_net\bin\%RELEASE%\%FRAMEWORK%\%PLATFORM%\publish"
SET ZIP_FILE="%BUILD_DIR%.gz"

cd %SOURCE_DIR%

dotnet clean
dotnet restore
dotnet build --configuration %RELEASE% --runtime %PLATFORM%
dotnet publish --configuration %RELEASE% --runtime %PLATFORM%

del %ZIP_FILE%

/usr/bin/gzip --force --recursive %BUILD_DIR%\publish

echo open dev.humlab.umu.se>%FTP_SCRIPT%
echo roger>>%FTP_SCRIPT%
echo schi11er>>%FTP_SCRIPT%
echo lcd /D %PUBLISH_DIR%>>%FTP_SCRIPT%
echo cd ./applications/pending>>%FTP_SCRIPT%
rem echo rm ./publish>>%FTP_SCRIPT%
echo binary>>%FTP_SCRIPT%
echo prompt>>%FTP_SCRIPT%
echo put %ZIP_FILE%>>%FTP_SCRIPT%
echo disconnect>>%FTP_SCRIPT%
echo quit>>%FTP_SCRIPT%

pause