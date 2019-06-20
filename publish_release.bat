@Echo Off

SET PROJECT_FOLDER="%~dp0%"

SET FTP_COMMAND="C:\Program Files (x86)\WinSCP\WinSCP.com"
SET FTP_OPTIONS=/newinstance /console

SET TARGET_PLATFORM=debian.8-x64
SET TARGET_FRAMEWORK=netcoreapp2.0
SET TARGET_HOST=snares.humlab.umu.se
SET TARGET_FOLDER=/home/%QUERYSEAD_TARGET_USERNAME%/applications/pending
SET TARGET_FILENAME=publish.zip

SET RELEASE=Release
SET RELEASE_FOLDER=%PROJECT_FOLDER%\"query_sead_net\bin\%RELEASE%\%TARGET_FRAMEWORK%\%TARGET_PLATFORM%"\publish

SET FTP_SCRIPT=%PROJECT_FOLDER%\ftp_deploy_script.txt

cd %PROJECT_FOLDER%

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

@del %PROJECT_FOLDER%\%TARGET_FILENAME%

/usr/bin/7za a %PROJECT_FOLDER%\%TARGET_FILENAME%  %RELEASE_FOLDER%
if '%ERRORLEVEL%' neq '0' (
    echo "ZIP failed"
    goto failure
)

Call :Create_FTP_Script

%FTP_COMMAND% %FTP_OPTIONS% < %FTP_SCRIPT% 
REM if '%ERRORLEVEL%' neq '0' (
REM     echo "FTP failed"
REM     goto failure
REM )

GOTO :end

:Create_FTP_Script
echo open %TARGET_HOST%>%FTP_SCRIPT%
echo %QUERYSEAD_TARGET_USERNAME%>>%FTP_SCRIPT%
echo %QUERYSEAD_TARGET_PASSWORD%>>%FTP_SCRIPT%
echo cd %TARGET_FOLDER%>>%FTP_SCRIPT%
echo lcd %PROJECT_FOLDER%>>%FTP_SCRIPT%
echo binary>>%FTP_SCRIPT%
echo option echo off
echo rm %TARGET_FILENAME%>>%FTP_SCRIPT%
echo option echo on
echo put %TARGET_FILENAME%>>%FTP_SCRIPT%
echo exit>>%FTP_SCRIPT%
goto:EOF

:failure
pause

:end

REM del %FTP_SCRIPT%
del %TARGET_PATH%
pause