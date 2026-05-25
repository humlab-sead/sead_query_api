SHELL := /bin/bash

include docker/.env

DBHOST:=$(shell cat ~/vault/.default.sead.server)
DBUSER:=$(shell cat ~/vault/.default.sead.username)
DBNAME:=sead_staging
DBPORT:=5433
DBPASSWORD:=$(shell cat ~/vault/.default.sead.password)

SOLUTION=sead_query_api.sln
API_PROJECT=sead.query.api/sead.query.api.csproj
TEST_PROJECT=sead.query.test/sead.query.test.csproj
TARGET_FRAMEWORK=net9.0
SCAFFOLD_CONTEXT_FOLDER=tmp/SeadQueryCore

.PHONY: test clean build publish tidy

show-settings:
	@echo "info: postgres://$(DBUSER)@$(DBHOST):$(DBPORT)/$(DBNAME)"

.PHONY: test
test:
	@set -a \
		&& source conf/.env \
		&& set +a \
		&& dotnet test $(TEST_PROJECT) -l "console;verbosity=detailed"
		
#--settings conf/appsettings.Test.json sead.query.test/sead.query.test.csproj

# Creates SQL DDL/DML for a TestContainer PostgreSQL database
test-data:
	time ./sead.query.test/Infrastructure/Mocks/FacetContext/PostgreSQL/Data/create-sample $(DBNAME) --port $(DBPORT) --fixed-ids ./sead.query.test/Infrastructure/Mocks/FacetContext/PostgreSQL/Data/sample-fixture.csv
	@sudo rm -rf ./sead.query.test/tmp/sead-query-pgdata-cache
	@echo "info: pgdata cache of test database invalidated"
	@echo "info: test data generation completed!"


scaffold-facet-context: show-settings
	@echo "info: creating facet context..."
	@dotnet tool install -g dotnet-ef
	@rm -rf $(SCAFFOLD_CONTEXT_FOLDER) && mkdir -p $(SCAFFOLD_CONTEXT_FOLDER)/Models $(SCAFFOLD_CONTEXT_FOLDER)/Context
	@dotnet ef dbcontext scaffold \
		"Host=$(DBHOST);Port=$(DBPORT);Database=$(DBNAME);Username=$(DBUSER);Password=$(DBPASSWORD)" \
		Npgsql.EntityFrameworkCore.PostgreSQL \
		--context-dir $(SCAFFOLD_CONTEXT_FOLDER)/Context \
		--output-dir $(SCAFFOLD_CONTEXT_FOLDER)/Models --force \
		--no-onconfiguring --no-pluralize --schema facet \
		--project ./sead.query.core/sead.query.core.csproj

.PHONY: clean
clean:
	@dotnet clean
	@dotnet clean -c Release
	@dotnet nuget locals --clear all

.PHONY: serve
serve: debug
	@cp -f conf/appsettings.Development.json sead.query.api/bin/Debug/$(TARGET_FRAMEWORK)/appsettings.json
	@cp -f conf/.env sead.query.api/bin/Debug/$(TARGET_FRAMEWORK)/.env
	@dotnet run --project $(API_PROJECT)

.PHONY: build
build:
	dotnet build $(SOLUTION)

.PHONY: release
release:
	dotnet build $(SOLUTION) -c Release

.PHONY: debug
debug:
	dotnet build $(SOLUTION) -c Debug

.PHONY: publish
publish:
	dotnet publish $(API_PROJECT) -c Release

.PHONY: tidy
tidy:
	dotnet format

.PHONY: tag
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
	@cat 'add_folder_to_path "$${HOME}/.dotnet/tools"' >> ~/.bashrc'
	@echo "info: csharpier installed, see https://csharpier.com/docs/About for more information"

changelog-tools:
	@go install github.com/git-chglog/git-chglog/cmd/git-chglog@latest
	@echo "info: git-chglog installed, see https://github.com/git-chglog/git-chglog"