## Purpose

This document describes how to set up, build, run, validate, and extend the SEAD Query API during day-to-day development.

It is intended for contributors working in this repository. It does not cover deployment, release operations, or high-level architecture rationale beyond what is needed to navigate the codebase.

## Development Status

- Primary stack: .NET 9 solution with API, core, infrastructure, composer, and test projects.
- Current production-style runtime: the existing faceted query API.
- In-progress work: the query-engine overhaul in `sead.query.composer` on branch `query-engine-overhaul`.
- TBD: a final, stable contributor guide for operations and a dedicated testing guide document.

## Prerequisites

You need the following tools to work effectively in this repository:

- .NET 9 SDK
- Git
- PostgreSQL access for database-backed development tasks
- Docker, if you want to use the container workflow or Testcontainers-backed scenarios
- VS Code or another C#-capable editor

Optional but useful:

- `dotnet ef` for scaffold-related workflows
- `gh` for release and PR helper targets in the `Makefile`
- Go and Node.js only if you intend to use the changelog or release tooling

## Repository Layout

The main solution file is `sead_query_api.sln`.

The important projects and folders are:

- `sead.query.api`: ASP.NET Core API host and startup wiring
- `sead.query.core`: shared domain types, query model, facet model, and core contracts
- `sead.query.infra`: repositories and infrastructure services
- `sead.query.composer`: in-progress query-engine redesign and route-based composition work
- `sead.query.test`: unit and integration tests
- `conf/`: local appsettings variants and environment files used during development and tests
- `docker/`: container build and compose assets
- `docs/proposals/`: proposal and design-in-progress material, including the query-engine overhaul notes
- `deprecated/`: historical or non-authoritative code and experiments; do not treat as the current implementation guide

For architecture and system boundaries, use `docs/DESIGN.md` as the entry point.

## One-Time Setup

Start from the repository root.

Restore the solution:

```bash
dotnet restore sead_query_api.sln
```

Build the solution:

```bash
dotnet build sead_query_api.sln
```

If you prefer the repository helper targets, the `Makefile` also exposes:

```bash
make build
make debug
make release
```

## Local Configuration

Development and tests rely on files in `conf/`.

Relevant files include:

- `conf/appsettings.Development.json`
- `conf/appsettings.Test.json`
- `conf/appsettings.Production.json`
- `conf/.env`

Repository notes:

- The API project has a `UserSecretsId`, so local secrets can also be supplied through the .NET user-secrets mechanism when needed.
- The test project copies `conf/appsettings.Test.json` and `conf/.env` into test output.
- Several helper scripts and test utilities expect `.env` values to be present.

Do not commit local secrets. `.env` is gitignored at the repository level.

## Running the API Locally

The most reliable command is:

```bash
dotnet run --project sead.query.api/sead.query.api.csproj
```

The repository helper target is also available:

```bash
make serve
```

`make serve` builds the solution in Debug, copies `conf/appsettings.Development.json` and `conf/.env` into the API output directory, and runs the API project.

You can also use the VS Code launch and task configuration already present in `.vscode/` for local debugging.

## Common Development Commands

From the repository root, the most useful commands are:

Build everything:

```bash
dotnet build sead_query_api.sln
```

Build only tests:

```bash
dotnet build sead.query.test/sead.query.test.csproj
```

Run the test project:

```bash
dotnet test sead.query.test/sead.query.test.csproj
```

Run all tests through the helper target:

```bash
make test
```

Format code:

```bash
dotnet format
```

Or via the `Makefile`:

```bash
make tidy
```

Clean build outputs and NuGet caches:

```bash
make clean
```

## VS Code Workflow

The workspace already includes tasks for common .NET actions, including restore, build, and test.

Useful built-in tasks include:

- restore the full solution
- build the full solution
- restore and build the test project
- run the test project

Use those tasks when you want a repeatable editor-driven workflow instead of shell commands.

## Day-to-Day Development Workflow

For most code changes, the working loop should be:

1. Restore and build the solution if dependencies or project files changed.
2. Make a focused change in the relevant project.
3. Run targeted tests for the changed area.
4. Run a broader test pass when the change touches shared query infrastructure.
5. Run `dotnet format` if the change introduced style drift.
6. Review the resulting diff before commit.

For query-related work, prefer validating the smallest affected slice first. Shared query code can influence multiple facet types and result-compilation paths, so targeted validation matters.

## Project-Specific Development Conventions

The repository is C#- and .NET-first. Follow the coding guidance in `.github/instructions/coding.instructions.md` and the settings in `.editorconfig`.

Important local conventions visible in the current repo:

- use 4-space indentation in C# files
- prefer `dotnet format` as the normal formatting pass
- keep changes focused rather than mixing unrelated cleanup with feature work
- treat `sead.query.composer` as active redesign work, not yet the sole authoritative implementation path
- keep tests in `sead.query.test`, usually under `UnitTests/` or the existing integration-oriented folders already in the repo

The release workflow depends on conventional commits, so use commit messages that follow the repository’s conventional-commit instruction.

## Code Quality and Validation

The practical local quality checks for this repository are:

- `dotnet build sead_query_api.sln`
- `dotnet test sead.query.test/sead.query.test.csproj`
- `dotnet format`

Use narrower commands when the change is isolated, especially in the test project or one subsystem.

Examples:

```bash
dotnet build sead.query.composer/sead.query.composer.csproj
dotnet test sead.query.test/sead.query.test.csproj --filter "RouteGraphTests"
```

The repository does not currently present a separate lint-only workflow as the main local quality gate. Formatting, compile success, and test execution are the key developer checks.

## Database and Schema-Driven Work

The project is database-aware and includes scaffold and fixture helpers.

### Test data generation

The `Makefile` includes:

```bash
make test-data
```

This creates SQL DDL/DML for the PostgreSQL-backed test-container workflow and clears the cached PostgreSQL data directory used by test fixtures.

### Scaffold facet context

The `Makefile` also includes:

```bash
make scaffold-facet-context
```

This installs `dotnet-ef` if needed and scaffolds a context from PostgreSQL into `tmp/SeadQueryCore`.

This workflow depends on local vault-backed credentials referenced by the `Makefile`:

- `~/vault/.default.sead.server`
- `~/vault/.default.sead.username`
- `~/vault/.default.sead.password`

If those files are not present on your machine, scaffold-related commands will not work without local adaptation.

### Scripted facet-config import

The `Makefile` now also includes:

```bash
make import-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml
```

This runs the API host through the `--import-facet-config` command path instead of starting the web server.

The target sources `conf/.env`, resolves the current git commit into `SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT`, and defaults `SEAD_QUERY_FACET_CONFIG_IMPORTED_BY` to `make-import-facet-config`.

Use it only against a database you intend to update. It is a scripted import path, not a no-op validation mode.

For a non-mutating YAML check through the same host entry point, use:

```bash
make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml
```

This runs the `--validate-facet-config` command path and stops after deserialization and importer-contract validation.

The validation path is now semantic as well as structural. In addition to deserialization and importer-contract checks, it resolves anchor tables, generated route endpoints, facet source-table references, and facet-anchor route bindings against the current facet schema without mutating the database.

### Current Phase 5 branch assumptions

While Phase 5 remains in progress, contributors should work from these branch-local assumptions:

- YAML is the current authoring source for facet and route configuration work, but the running application still reads the imported database copy in the existing `facet` schema rather than loading YAML directly at request time.
- A YAML edit is not a runtime change until it has been validated and, when appropriate, imported through the current CLI path.
- The current runtime-readiness claim is intentionally narrow. It is backed by the recorded `sites_polygon`, country-filter, and `analysis_entity_ages` intersect baselines plus the broader green live result, controller, and composed facet-content reruns. Do not treat unmeasured composed families as already cleared for default cutover.
- Unsupported or unresolved composed requests are still expected to stay on the explicit legacy-fallback boundary rather than being repaired implicitly in request handlers.

For Phase 5 work on facet and route configuration, the practical contributor loop is:

1. edit the YAML authoring file or importer code
2. run `make validate-facet-config FACET_CONFIG_FILE=...`
3. run the narrowest focused tests for the touched slice
4. run `make import-facet-config FACET_CONFIG_FILE=...` only when you intend to update the target database copy

### Migrations

Entity Framework migrations are not the documented primary development workflow in this repository.

Treat the current development model as schema-driven and configuration-driven. If a formal migration workflow is introduced later, document it separately instead of inferring one from generic .NET practice.

## Testing During Development

The test project targets `.NET 9` and includes xUnit, Moq, FluentAssertions, AutoFixture, Testcontainers, and ASP.NET Core TestHost.

In practice:

- use focused `dotnet test` runs while iterating
- run the full test project before finishing shared or cross-cutting changes
- expect tests and helpers to use `conf/appsettings.Test.json` and `conf/.env`

Detailed testing policy belongs in `docs/TESTING.md` once that document is added.

## Debugging and Troubleshooting

Common local issues and the first thing to check:

- Build failures after dependency or framework changes: run `dotnet restore` and rebuild the solution.
- API starts but configuration is wrong: verify the expected `conf/appsettings.*.json` file and `.env` values are available.
- Tests fail because settings are missing: confirm `conf/.env` exists and the test project is copying configuration into output.
- Scaffold or fixture-generation commands fail immediately: verify local PostgreSQL access and the required files in `~/vault/`.
- A helper target behaves unexpectedly: compare it against the project’s current target framework before assuming it is up to date.

Because the repository contains long-lived historical material and redesign work in parallel, always verify whether you are modifying the active runtime, test infrastructure, or the in-progress composer before debugging deeper.

## Choosing The Right Document

Use this section when you are deciding where new documentation should live or which existing document to update.

Keep decision documents, phase plans, task plans, and durable system documents separate when the work is substantial. For small changes, a proposal may include a short delivery-order or implementation-handoff section instead of a separate phase plan.

### Document roles

- `README.md`: front door for project overview, quick start, and links to the main documents
- `docs/DEVELOPMENT.md`: contributor workflow, local setup, build and test commands, repository conventions, and documentation-placement guidance
- `docs/DESIGN.md`: active architecture, component boundaries, runtime flows, design constraints, and major technical decisions
- `docs/REQUIREMENTS.md`: durable system requirements that should outlive a specific proposal
- `docs/TESTING.md`: repository testing strategy, validation expectations, and testing guidance
- `docs/OPERATIONS.md`: environments, deployment, rollback, observability, and operational readiness
- `docs/proposals/<name>.md`: proposal or change request document for problem, recommendation, tradeoffs, risks, and open questions
- `docs/proposals/<name>/IMPLEMENTATION_PLAN.md`: ordered multi-phase path from current state to target state for major efforts
- `docs/proposals/<name>/TASK_PLAN_PHASE_N.md`: concrete implementation work for one phase, including work breakdown, validation, and definition of done
- `docs/archive/` and proposal archive folders: historical material that is no longer authoritative

### Practical rules of thumb

- If the reader needs to decide whether to do the work, update or create a proposal.
- If the decision is already made and the reader needs the ordered delivery path, update or create a phase plan.
- If one phase needs concrete implementation steps, update or create a task plan.
- If the content should remain true after the proposal is closed, move it into a durable document.
- If the content is only historical, archive it.

### Default recommendation for major proposals

For major proposals, prefer this document set:

- one proposal or change request document as the master decision record
- one separate phase plan as the execution-sequencing document
- one task plan per active phase when that phase needs tracked implementation work
- updates to durable docs as design, requirements, development guidance, or operations truth becomes stable

## Related Documents

- `README.md`: short overview and entry point
- `docs/DESIGN.md`: current architecture and overhaul boundaries
- `docs/REQUIREMENTS.md`: durable query-engine requirements
- `docs/TESTING.md`: TBD
- `docs/OPERATIONS.md`: TBD