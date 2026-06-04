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
FACET_CONFIG_FILE?=sead.query.composer/Templates/route_v1.yaml
FACET_CONFIG_SOURCE_COMMIT?=$(shell git rev-parse HEAD 2>/dev/null || echo unknown)
FACET_CONFIG_IMPORTED_BY?=make-import-facet-config
FACET_RUNTIME_SCHEMA_FILE?=scripts/prepare-facet-runtime-schema.sql
SEAD_QUERY_API_BASE_URL?=http://localhost:8090

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

MD_FILES := $(wildcard docs/*.md)
PDF_FILES := $(patsubst docs/%.md,docs/output/%.pdf,$(MD_FILES))

LATEX_VARS ?= \
	-V lang=en \
	-V geometry:margin=2cm \
	-V header-includes='\emergencystretch=3em' \
	-V header-includes='\usepackage{fvextra}' \
	-V header-includes='\DefineVerbatimEnvironment{Highlighting}{Verbatim}{breaklines,breakanywhere,commandchars=\\\{\}}'

.PHONY: all-docs
all-docs: $(PDF_FILES)

.PHONY: clear-docs
clear-docs:
	@rm -f $(PDF_FILES)

docs/output/%.pdf: docs/%.md
	@mkdir -p "$(@D)"
	@pandoc "$<" -o "$@" $(LATEX_VARS) --pdf-engine=xelatex -F mermaid-filter

do-graphifyy:
	@pipx install graphifyy
	@dotnet tool install -g graphify-dotnet
	@${HOME}/.local/share/pipx/venvs/graphifyy/bin/graphify install --project --platform codex

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

.PHONY: import-facet-config
import-facet-config:
	@set -a \
		&& source conf/.env \
		&& set +a \
		&& SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT="$(FACET_CONFIG_SOURCE_COMMIT)" \
		SEAD_QUERY_FACET_CONFIG_IMPORTED_BY="$(FACET_CONFIG_IMPORTED_BY)" \
		dotnet run --project $(API_PROJECT) -- --import-facet-config "$(FACET_CONFIG_FILE)"

.PHONY: prepare-facet-runtime-schema
prepare-facet-runtime-schema:
	@PGPASSWORD="$(DBPASSWORD)" psql \
		-h "$(DBHOST)" \
		-p "$(DBPORT)" \
		-U "$(DBUSER)" \
		-d "$(DBNAME)" \
		-v ON_ERROR_STOP=1 \
		-f "$(FACET_RUNTIME_SCHEMA_FILE)"

.PHONY: validate-facet-config
validate-facet-config:
	@set -a \
		&& source conf/.env \
		&& set +a \
		&& SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT="$(FACET_CONFIG_SOURCE_COMMIT)" \
		SEAD_QUERY_FACET_CONFIG_IMPORTED_BY="$(FACET_CONFIG_IMPORTED_BY)" \
		dotnet run --project $(API_PROJECT) -- --validate-facet-config "$(FACET_CONFIG_FILE)"

.PHONY: default-cutover-smoke-check
default-cutover-smoke-check:
	@set -euo pipefail \
		&& set -a \
		&& source conf/.env \
		&& set +a \
		&& echo "info: running representative spatial cutover checks..." \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlySitesPolygonSlice" \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~Load_GeoPolygonFilteredMapResult_UsesComposedFilterSql|FullyQualifiedName~LoadMap_GeoPolygonFilteredRequest_UsesComposedFilterSql" \
		&& echo "info: running representative country-filter cutover checks..." \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~FacetContentService_ComposedCountryPredicateGeochronologySlice" \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~Load_CountryFilteredMapResult_UsesComposedFilterSql|FullyQualifiedName~Load_CountryFilteredMapResult_MatchesLegacyOutput|FullyQualifiedName~LoadMap_CountryFilteredRequest_UsesComposedFilterSql" \
		&& echo "info: running representative intersect cutover checks..." \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyIntersectSlice" \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~Load_IntersectFilteredMapResult_UsesComposedFilterSql|FullyQualifiedName~LoadMap_IntersectFilteredRequest_UsesComposedFilterSql" \
		&& echo "info: running broader cutover regression checks..." \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~SQT.LiveServices.ResultLoadServiceTests" \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~IntegrationTests.Sead.ResultControllerTests" \
		&& dotnet test $(TEST_PROJECT) --filter "FullyQualifiedName~FacetContentService_ComposedSupportedLiveSlices"

.PHONY: default-cutover-http-smoke-check
default-cutover-http-smoke-check:
	@./scripts/default-cutover-http-smoke-check.sh "$(SEAD_QUERY_API_BASE_URL)"

.PHONY: default-cutover-http-measure
default-cutover-http-measure:
	@./scripts/default-cutover-http-measure.sh "$(SEAD_QUERY_API_BASE_URL)"

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