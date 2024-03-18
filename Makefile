SHELL := /bin/bash

.PHONY: test clean build publish tidy

test:
	@export $(cat conf/.env | xargs) \
		&& dotnet test 
		
#--settings conf/appsettings.Test.json sead.query.test/sead.query.test.csproj

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
