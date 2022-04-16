

test:
	@export $(cat conf/.env | xargs) \
		&& dotnet test --settings conf/appsettings.Test.json sead.query.test/sead.query.test.csproj

win-test:
	@for /f "eol=# tokens=*" %%i in (conf\.env) do set %%i
	@dotnet test

kill-all:
	@pkill -f /home/roger/bin/dotnet3.1/dotnet

# @taskkill /IM "dotnet.exe" /F
# @taskkill /IM "testhost.exe" /F

.PHONY: kill-all test

clean:
	@dotnet clean
	@dotnet clean -c Release
	@dotnet nuget locals --clear all

build:
	dotnet build -c Release

publish:
	dotnet publish -c Release

tidy:
	dotnet format

.PHONY: test clean build publish
