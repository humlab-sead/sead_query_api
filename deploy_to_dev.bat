@Echo Off
SET WINSCP="C:\Program Files (x86)\WinSCP\WinSCP.exe"
SET RELEASE=Release
SET TARGET_PLATFORM=debian.8-x64
SET FRAMEWORK=netcoreapp2.0
SET PASSWORD=schi11er
SET WINSCP="C:\Program Files (x86)\WinSCP\WinSCP.com"
SET SOURCE_DIR="C:\Users\roma0050\Documents\Projects\SEAD\query_sead_api_core"
SET BUILD_DIR=%SOURCE_DIR%\"query_sead_net\bin\%RELEASE%\%FRAMEWORK%\%TARGET_PLATFORM%"\publish
SET ZIP_FILENAME=publish.zip
SET ZIP_PATH=%SOURCE_DIR%\%ZIP_FILENAME%
SET FTP_SCRIPT=%SOURCE_DIR%\ftp_deploy_script.txt

cd %SOURCE_DIR%

@dotnet clean
@if '%ERRORLEVEL%' neq '0' (
    echo "dotnet clean failed"
    goto failure
)

@dotnet restore
@if '%ERRORLEVEL%' neq '0' (
    echo "dotnet restore failed"
    goto failure
)
rem dotnet build --configuration %RELEASE% --runtime %TARGET_PLATFORM%
@if '%ERRORLEVEL%' neq '0' (
    echo "dotnet build failed"
    goto failure
)
@dotnet publish --configuration %RELEASE% --runtime %TARGET_PLATFORM%
@if '%ERRORLEVEL%' neq '0' (
    echo "dotnet publish failed"
    goto failure
)

:zip_file

@del %ZIP_PATH%

/usr/bin/7za a %ZIP_PATH% %BUILD_DIR%
if '%ERRORLEVEL%' neq '0' (
    echo "ZIP failed"
    goto failure
)

Call :Create_FTP_Script

%WINSCP% < %FTP_SCRIPT% 
REM if '%ERRORLEVEL%' neq '0' (
REM     echo "FTP failed"
REM     goto failure
REM )

GOTO :end

:Create_FTP_Script
echo open dev.humlab.umu.se>%FTP_SCRIPT%
echo roger>>%FTP_SCRIPT%
echo schi11er>>%FTP_SCRIPT%
rem echo lcd %PUBLISH_DIR%>>%FTP_SCRIPT%
echo cd applications/pending>>%FTP_SCRIPT%
rem echo rm ./publish>>%FTP_SCRIPT%
echo binary>>%FTP_SCRIPT%
rem echo prompt>>%FTP_SCRIPT%
echo rm %ZIP_FILENAME%>>%FTP_SCRIPT%
echo put %ZIP_PATH%>>%FTP_SCRIPT%
echo exit>>%FTP_SCRIPT%
goto:EOF

:failure
pause

:end

REM del %FTP_SCRIPT%
del %ZIP_PATH%
pause