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

.PHONY: clean release debug publish tidy test tag serve

serve: debug
	@cp -f conf/appsettings.Development.json sead.query.api/bin/Debug/net8.0/appsettings.json
	@cp -f conf/.env sead.query.api/bin/Debug/net8.0/.env
	@dotnet run --project sead.query.api/sead.query.api.csproj

release:
	dotnet build -c Release

debug:
	dotnet build -c Debug

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
	
.PHONY: tools
tools:
	@dotnet tool install csharpier --global
	@cat 'add_folder_to_path "$$HOME/.dotnet/tools"' >> ~/.bashrc'
	@echo "info: csharpier installed, see https://csharpier.com/docs/About for more information"