## Purpose

This document explains how the SEAD Query API is validated during development, what kinds of tests exist in the repository, how contributors should run them locally, and what each test level is expected to cover.

It is a testing guide, not a setup guide, deployment runbook, or architecture document.

## Testing Goals

The repository’s testing strategy is designed to protect a query-heavy, database-aware .NET application where small changes in SQL compilation, facet behavior, or configuration can affect multiple runtime paths.

The main goals are:

- verify query behavior at the smallest useful scope first
- keep unit tests fast and isolated where possible
- use integration-style tests to validate DI wiring, API behavior, SQL compilation, and database-backed behavior
- keep configuration-sensitive tests reproducible through repository-managed test settings
- make regressions in shared query infrastructure visible before changes are merged

## Test Stack

The active test project is `sead.query.test/sead.query.test.csproj`.

The current test stack includes:

- `xUnit` for test execution
- `Moq` for mocks
- `FluentAssertions` for expressive assertions
- `AutoFixture` for test data generation where useful
- `Microsoft.AspNetCore.TestHost` for API-hosted integration scenarios
- `Testcontainers` and `Testcontainers.PostgreSql` for container-backed database scenarios
- `Microsoft.EntityFrameworkCore.InMemory` and `Microsoft.EntityFrameworkCore.Sqlite` for lighter-weight test setups where appropriate

The repository also contains some older or partial test areas and historical patterns. The supported current test stack should be documented from the active project file, not inferred from old experimental code.

## Test Project Layout

The main test project contains multiple test layers and support folders.

Important directories include:

- `sead.query.test/UnitTests/` — the primary home for unit tests
- `sead.query.test/IntegrationTests/` — broader tests that validate behavior across components or through the API host
- `sead.query.test/LiveTests/` — specialized tests that assume a more realistic or externally configured environment
- `sead.query.test/Infrastructure/` — fixtures, helpers, DI setup, mocks, and PostgreSQL container support
- `sead.query.test/QueryComposer/` — tests related to the in-progress composer work

There are also directories that are explicitly excluded or not part of the normal build path, including `Purgatory/` and `QueryComposer/BackBurner/`. Treat those as non-authoritative when documenting normal validation workflow.

## Test Levels

### Unit tests

Unit tests live primarily under `sead.query.test/UnitTests/`.

They should:

- validate one behavior at a time
- keep arrange, act, and assert phases clear
- isolate collaborators with mocks only when those collaborators are real dependencies
- prefer real simple objects over mocks for passive data
- cover both success paths and meaningful failure paths

In this repository, unit tests are especially valuable for:

- query-builder helpers
- field, join, and SQL compiler behavior
- facet-related services and plugins
- route parsing and route-graph logic in the composer work
- utility functions and repository-independent domain behavior

Unit tests should be the default first choice for new logic that does not require a running host or real database behavior.

### Integration tests

Integration tests live under `sead.query.test/IntegrationTests/` and related feature-specific folders.

They should validate interactions across component boundaries rather than isolated methods.

In this repository, that commonly means:

- API host setup through test host infrastructure
- Autofac container wiring and application startup
- behavior that depends on multiple services collaborating correctly
- query or repository behavior that is difficult to trust from unit tests alone
- database-backed test scenarios where SQL or persistence behavior matters

Integration tests are appropriate when the real risk is not just a single method failing, but the assembly of services, configuration, and runtime behavior.

### Live tests

The `LiveTests/` folder contains tests that appear to assume a more realistic runtime environment and registry/configuration setup.

Treat these as specialized tests rather than the first-line local validation path.

Use them when:

- you need higher-fidelity confirmation against realistic configuration or service wiring
- the scenario depends on more than the normal isolated test harness

Document these as environment-sensitive tests, not as a mandatory step for every small code change.

### Container-backed and database-backed tests

The test infrastructure includes PostgreSQL fixture support under `sead.query.test/Infrastructure/Mocks/FacetContext/PostgreSQL/` and package references for `Testcontainers`.

Use container-backed tests when:

- the behavior depends on real PostgreSQL semantics
- range operators, SQL generation, or persistence behavior need realistic execution
- in-memory or SQLite substitutes would hide the real behavior under test

These tests are more expensive than pure unit tests and should be used deliberately.

## Test Naming and Style

The repository already contains a mix of historical and newer test styles, but the recommended style is:

- use clear behavior-driven names such as `Method_Scenario_ExpectedOutcome`
- keep test setup minimal and readable
- avoid unnecessary branching or complicated logic inside tests
- prefer direct assertions on observable behavior over broad incidental assertions

The older `sead.query.test/README.md` still contains useful general principles such as AAA structure and keeping tests fast, isolated, and repeatable. Those principles still fit the current repo even though the file is not a complete testing guide.

## Local Test Configuration

The test project copies the following files into test output:

- `conf/appsettings.Test.json`
- `conf/.env`
- `sead.query.test/xunit.runner.json`

These files matter because tests and fixtures rely on repository-provided configuration rather than ad hoc local setup.

Repository-specific notes:

- `xunit.runner.json` enables diagnostic messages and marks tests running longer than 10 seconds as long-running.
- several test helpers and settings factories look for `.env` values
- configuration-sensitive failures are often caused by missing or stale local config, not only broken code

## Supported Local Commands

From the repository root, the main supported commands are:

Run the full test project:

```bash
dotnet test sead.query.test/sead.query.test.csproj
```

Run tests through the repository helper target:

```bash
make test
```

Build the test project only:

```bash
dotnet build sead.query.test/sead.query.test.csproj
```

Run focused tests while iterating:

```bash
dotnet test sead.query.test/sead.query.test.csproj --filter "RouteGraphTests"
```

The repository’s VS Code tasks also provide supported restore, build, and test commands for both the solution and the test project.

## Test Data and Fixtures

The test infrastructure includes helper code, collection fixtures, DI wiring, and PostgreSQL-related fixtures under `sead.query.test/Infrastructure/`.

For database-backed test data generation, the `Makefile` provides:

```bash
make test-data
```

This command generates SQL DDL/DML for the PostgreSQL-backed test workflow and clears the cached PostgreSQL data directory used by the test setup.

Use it when the database fixture data itself needs to be refreshed or regenerated.

## Local Validation Workflow

The expected local validation workflow is:

1. run focused unit or subsystem tests for the area you changed
2. run broader tests when the change touches shared query infrastructure, service wiring, or database-backed behavior
3. rebuild the solution or relevant project if the change touched project files, DI wiring, or startup paths
4. use formatting and compile success alongside tests before finishing the change

For small isolated logic changes, a focused `dotnet test --filter` run is usually the right first check.

For changes in shared compilers, route resolution, infrastructure, or service registration, prefer a wider test pass because those areas have broader blast radius.

## Validation Before Merge

Before merge or handoff, the practical default expectation is:

- relevant focused tests pass
- the main test project passes when the change is shared or cross-cutting
- the solution builds successfully if the change touched runtime code or project structure

At minimum, contributors should not rely only on reasoning or diff inspection for changes that have an available executable test path.

## CI and Automation Status

Current CI validation expectations are not fully defined in the repository’s active GitHub workflow configuration.

What is visible today:

- `.github/workflows/release.yml` is focused on release automation, not on a full test gate
- `.travis.yml` exists as historical CI configuration and still references an older `.NET 3.1` workflow

Document the current CI test-gating situation as `TBD` rather than claiming a fully active modern CI test pipeline that is not present in the repository.

Until that is formalized, local validation remains the primary documented contributor workflow.

## Common Pitfalls

Common causes of confusing test failures in this repository include:

- missing or incorrect values in `conf/.env`
- stale assumptions about framework version or output paths
- relying on historical or excluded test folders as if they were part of the normal build
- using in-memory substitutes for behavior that actually depends on PostgreSQL features
- validating only one narrow test when the change affects shared query infrastructure

When test behavior seems inconsistent, first confirm whether the scenario is intended to be a unit test, an integration test, a live test, or a container-backed test. Many failures come from running the right code in the wrong test mode.

## Related Documents

- `docs/DEVELOPMENT.md` — local setup, contributor workflow, and common commands
- `docs/DESIGN.md` — architecture and query-engine boundaries
- `README.md` — short overview and entry point
- `docs/OPERATIONS.md` — TBD