SHELL := /bin/bash

include docker/.env

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

tag:
	@echo "info: adding annotated release tag $(SEAD_QUERY_API_TAG)..."
	@git tag -a $(SEAD_QUERY_API_TAG) -m "Release $(SEAD_QUERY_API_TAG)"
	@git push origin $(SEAD_QUERY_API_TAG)

.PHONY: release-pr
release-pr:
	@echo "info: creating pull request..."
	@gh pr create --base main  --title "Release $(SEAD_QUERY_API_TAG)" --body "Release $(SEAD_QUERY_API_TAG)" --assignee @me 
	