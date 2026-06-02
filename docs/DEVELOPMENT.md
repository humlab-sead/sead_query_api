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
- when behavior varies by facet type or another domain discriminator, prefer handler, strategy, resolver, or factory abstractions over adding new `FacetTypeId` branches in a central service
- keep central services orchestration-focused: select the implementation once near the boundary, compose shared state, and delegate type-specific behavior to the selected implementation

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

### Print SQL for a facet URL

The API host also exposes a developer CLI for inspecting the SQL generated for a facet-content request expressed as a facet URL.

```bash
dotnet run --project sead.query.api/sead.query.api.csproj -- --print-facet-sql "family:family"
```

The command prints the facet URL, the effective execution path (`composed` or `legacy`), the target facet metadata, the normalized facet configs and picks, the final SQL query, and any separate category-info SQL used by the request.

Use it when you need a quick local probe for a request shape without going through the HTTP controllers.

For result-query SQL, use the matching result probe:

```bash
dotnet run --project sead.query.api/sead.query.api.csproj -- --print-result-sql "family:family" --view-type tabular
```

You can also override the result facet explicitly when needed:

```bash
dotnet run --project sead.query.api/sead.query.api.csproj -- --print-result-sql "family:family" --view-type map --result-code map_result
```

The result probe prints the resolved view type, result facet, specification keys, result handoff SQL prologue and joins, and the final compiled result SQL.

### Managing facet configuration

Treat facet maintenance as a configuration change with one checked-in authoring source and one imported runtime copy.

- authoring source: `sead.query.composer/Templates/route_v1.yaml`
- authoring contract: `sead.query.composer/Templates/facet-route-config.schema.json`
- runtime copy: the active imported rows in schema `facet`

Do not edit the runtime tables by hand as a normal development workflow. Make the change in YAML, validate it, test the affected slice, and import it only when you intend to update the target database copy.

#### Add a facet

Use this flow when introducing a new visible facet, result facet, or route-backed target.

1. Add the new facet definition to `route_v1.yaml`, including the facet metadata, facet type, source table or expression, and any anchor or route bindings the facet needs.
2. If the facet depends on a new anchor mapping, route, or template, add that configuration in the same YAML change so the facet and its traversal contract stay in sync.
3. Run `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml`.
4. Run the narrowest focused tests for the touched path, especially composer, facet-content, or live-slice tests if the new facet extends the validated support surface.
5. Import with `make import-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml` only when you intend to update the target database copy.

#### Update a facet

Use this flow when changing facet labels, expressions, clauses, supported anchors, routes, or other query-shaping behavior.

1. Edit the existing facet entry in `route_v1.yaml`.
2. If the change affects traversal, anchor compatibility, or target joins, update the linked route or anchor configuration in the same change.
3. Run `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml`.
4. Run focused tests for the exact affected request family before widening to broader live or regression checks.
5. Import only when you need the changed configuration materialized in the runtime database.

#### Remove a facet

Use this flow when retiring a facet or removing an obsolete route-backed target.

1. Remove the facet definition from `route_v1.yaml`.
2. Remove or update any dependent route, anchor binding, or template entries that are no longer referenced by the remaining configuration.
3. Run `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml` to catch dangling bindings before import.
4. Run focused tests for nearby request families so the removal does not leave broken references in the composed or fallback path.
5. Import only when you want the removal reflected in the runtime database copy.

When a facet change alters the validated composed support surface, update the supporting tests and any durable docs that describe the supported matrix instead of leaving the new runtime boundary implicit.

### Managing the route inventory

Treat the route inventory as part of the maintained facet-configuration authoring surface.

- authoring source: `sead.query.composer/Templates/route_v1.yaml`
- validation contract: `sead.query.composer/Templates/facet-route-config.schema.json` plus `make validate-facet-config`
- runtime copy: imported rows in `facet.route`, `facet.facet_anchor`, and related `facet` tables

The current route inventory has two categories:

- generated route families, where one family expands to one concrete route per supported anchor
- explicit exception routes, which are reserved for hand-maintained special cases and should remain exceptional

Prefer generated route families whenever the source-to-anchor traversal follows one repeatable pattern. Use explicit `routes` only when the traversal cannot be expressed cleanly as a generated family or when a durable exception must stay explicit.

#### Add a route family

Use this flow when introducing a new repeatable source-to-anchor traversal.

1. Add the family to `route_v1.yaml` with one source table and one anchor entry per supported anchor.
2. Reuse existing macros where possible instead of duplicating path segments.
3. Keep the family declarative: route families should expand to table paths, not raw SQL.
4. Ensure the resulting concrete route keys remain deterministic, using the existing `<family>__<anchor>` naming rule.
5. Run `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml`.
6. Run focused tests for the affected route parsing, route compilation, facet-content, or result-handoff slice.
7. Import only when you intend to materialize the updated inventory in the target database copy.

#### Update existing route inventory

Use this flow when changing path segments, macro expansion, supported anchors, or the family-to-facet bindings.

1. Edit the route family, macro, or explicit route in `route_v1.yaml`.
2. Update any dependent facet-anchor bindings in the same change so facets do not point at stale route keys.
3. Revalidate with `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml`.
4. Run the narrowest relevant tests first, especially route parser, route SQL compiler, composed facet-content, or result-handoff coverage if the route change affects composed execution.
5. Import only when you want the changed route inventory reflected in the runtime database copy.

#### Remove a route family or explicit route

Use this flow when retiring traversal paths that are no longer needed.

1. Remove the family or explicit route from `route_v1.yaml`.
2. Remove or update any facet-anchor bindings, templates, or exceptions that referenced the removed route keys.
3. Run `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml` to catch unresolved bindings before import.
4. Run focused tests for nearby request families so the removal does not leave an accidental runtime fallback or broken composed contract.
5. Import only when you want the removal reflected in the runtime database copy.

#### Route-inventory rules

Keep these rules explicit when reviewing route changes:

- each route family should declare one source table
- each anchor entry should expand to one valid concrete route
- macros should expand only to path items and should not carry facet semantics
- explicit `routes` should stay reserved for exceptions, not become the default authoring style
- unsupported source-to-anchor traversals should be absent from generated families and called out explicitly as exceptions or follow-up work
- route-inventory changes should be accompanied by the facet-anchor or support-boundary updates they require

Do not edit `facet.route` or related runtime tables by hand as a normal maintenance path. Keep the checked-in YAML inventory authoritative, validate it, and then import it.

If a route-inventory change widens or narrows the supported runtime surface, update the relevant durable docs and focused tests instead of leaving the new route boundary implicit.

### Implementing a new facet type

Adding a new facet type is a code-and-configuration change, not only a YAML change. The current runtime still has two relevant execution paths: the legacy plugin-based category-count path and the composed path used for the validated composed surface. A new facet type is not complete until its intended runtime boundary is explicit.

Start from the closest existing facet family rather than from an abstract template.

- use `Discrete` when the input is category picks
- use `Range` when the input is interval or min/max driven
- use `Intersect` when the type reuses interval-style grouping but needs its own composed contract
- use `GeoPolygon` when the type depends on plugin-owned spatial filtering and category-info SQL

#### Implementation checklist

1. Add or update the facet-type identifier in `sead.query.core/Model/Entities/Facet.cs` so the runtime can address the new type explicitly.
2. Add the legacy plugin implementation under `sead.query.core/Plugins/`, following the existing plugin pattern for keyed registrations of:
	- `ICategoryCountHelper`
	- `ICategoryCountSqlCompiler`
	- `ICategoryInfoService`
	- `IPickFilterCompiler`
	- `IFacetPlugin`
3. Register the new plugin in `sead.query.api/Dependency.cs` and the mirrored test container wiring in `sead.query.test/Infrastructure/Dependency.cs`.
4. Decide whether the new type is supported only on the legacy path first or whether it also needs composed-path support.
5. If it needs composed support, add the composed-path implementations at the relevant extension points rather than editing a central `switch` or `if` chain. The normal shape is: keep `ComposedFacetContentService` orchestration-only, add an `IComposedFacetContentHandler` implementation for target-specific content loading, and extend request, filter, resolver, or result-handoff collaborators only where the new type changes those contracts.
6. Add or update facet authoring in `sead.query.composer/Templates/route_v1.yaml`, then run validation and import through the documented CLI path.
7. Add focused tests before widening to grouped live or controller coverage.

#### Legacy runtime extension points

The legacy path is still the fallback boundary for unsupported requests, so new facet types usually need a complete plugin surface even if composed support is planned later.

Use the existing plugin folders in `sead.query.core/Plugins/DiscreteFacet`, `RangeFacet`, `IntersectFacet`, and `GeoPolygonFacet` as concrete examples. The important contract is the keyed registration pattern: `CategoryCountService` and `PickFilterCompilerLocator` resolve behavior by `EFacetType`, so missing keyed registrations usually mean the new type will fail at runtime even if the concrete classes compile.

#### Composed runtime extension points

If the new facet type should run on the composed path, define that contract explicitly.

- predicate-side filtering belongs in composer resolver logic, not in controllers
- composed anchor-set combination belongs in query-composition strategies, not in plugin wiring
- target facet content orchestration belongs in `ComposedFacetContentService`, but target-type-specific content loading belongs in `IComposedFacetContentHandler` implementations
- target facet content SQL belongs in `IFacetContentQueryComposer` or closely related query services
- result handoff behavior belongs in the composed result-projection boundary, not in ad hoc result-controller branching

The current concrete seams are:

- `sead.query.composer/QueryComposer/RouteCompiler/DiscreteFacetPredicateResolver.cs` for the first active predicate resolver pattern
- `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs` for shared composed facet-content orchestration and handler selection
- `sead.query.composer/QueryComposer/Services/ComposedFacetContentRequestFactory.cs` for composed request creation, anchor resolution, predicate compatibility checks, and anchor-to-target route SQL generation
- `sead.query.composer/QueryComposer/Services/ComposedFacetContentFilterQueryFactory.cs` for composed anchor-filter SQL construction
- `sead.query.composer/QueryComposer/Services/IComposedFacetContentHandler.cs` and the concrete handler classes for target-facet-specific category-info lookup, row mapping, and post-processing
- `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs` for composed result filtering and target-route handoff
- `sead.query.core/QueryComposer/Strategies/IntersectComposedFilterQueryComposer.cs` for anchor-set composition
- `sead.query.core/QueryComposer/Strategies/DiscreteFacetContentQueryComposer.cs` for target facet content SQL generation across the currently supported facet families

Do not force a new facet type through the discrete resolver or discrete content assumptions if its input contract is materially different. Add a dedicated resolver, handler, category-info service, or composer support surface when the facet semantics require it.

For this codebase, treat growing `FacetTypeId` conditionals in shared services as a design smell unless the branch is a small, stable guard clause. Adding a new facet type should usually mean adding a new implementation class and registering it, not editing one large central service.

#### Validation workflow for a new facet type

At minimum, add focused coverage for:

- plugin resolution and keyed DI registration under `sead.query.test/UnitTests/Plugins/`
- SQL compiler or category-info behavior for the new type
- request-path behavior in the narrowest relevant service tests
- composed-path contract tests if the type is intended to run on the composed baseline
- focused live or controller tests only after the local unit contract is stable

The existing test layout is the template here:

- plugin-specific tests live under `sead.query.test/UnitTests/Plugins/<FacetType>/`
- composed facet-content and result-handoff behavior lives under `sead.query.test/UnitTests/QueryComposer/`
- container wiring checks live in `sead.query.test/UnitTests/Utility/DependencyInjectionTests.cs`

When a new facet type changes the supported runtime surface, update the durable docs that describe current support boundaries instead of leaving the new type as tribal knowledge.

### Facet-configuration metadata reference

The runtime does not read facet YAML directly during requests. It reads the active imported configuration in schema `facet`, together with revision provenance stored in `facet.config_revision`.

The key metadata fields on the active revision are:

- `revision_id`: the imported configuration revision identifier
- `source_commit`: the git commit or source revision recorded for the import
- `content_hash`: the content hash of the imported configuration
- `imported_at`: when the runtime copy was imported
- `imported_by`: who or what performed the import
- `is_active`: whether this revision is the active runtime copy

The surrounding runtime metadata for facet configuration currently includes the imported tables in schema `facet`, including `anchor`, `facet_anchor`, `route`, `route_step`, and `config_revision`.

Use these references when you need to confirm what configuration is active or when you need to reason about the current runtime copy versus the checked-in authoring source:

- `docs/DESIGN.md` for the architecture and runtime/configuration split
- `docs/OPERATIONS.md` for deployment-time import provenance and runtime verification
- `docs/proposals/done/QUERY_ENGINE_OVERHAUL/FACET_ROUTE_CONFIGURATION_SOURCE_OF_TRUTH.md` for the detailed authoring and imported-copy contract

### Current branch assumptions

Contributors should work from these current branch-local assumptions:

- YAML is the current authoring source for facet and route configuration work, but the running application still reads the imported database copy in the existing `facet` schema rather than loading YAML directly at request time.
- A YAML edit is not a runtime change until it has been validated and, when appropriate, imported through the current CLI path.
- The current runtime-readiness claim is intentionally narrow. It is backed by the recorded `sites_polygon`, country-filter, and `analysis_entity_ages` intersect baselines plus the broader green live result, controller, and composed facet-content reruns that now back the published `supersead` route. Do not treat unmeasured composed families as already cleared for default cutover.
- Unsupported or unresolved composed requests are still expected to stay on the explicit legacy-fallback boundary rather than being repaired implicitly in request handlers. Current deferred examples include the live `family` facet-content parity follow-up and the prefixed `species:species` result fallback follow-up tracked outside the current CR.

For current facet and route configuration work, the practical contributor loop is:

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
