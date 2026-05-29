## Purpose

This document describes how the SEAD Query API is structured, how its major components interact, and which design constraints govern the current system and the in-progress query-engine overhaul.

This is an architecture document, not a developer setup guide, testing guide, or operations runbook.

## Design Status

- Current authoritative runtime: the existing faceted query API implemented in the solution projects and described by the current request-flow notes.
- In progress: the query-engine overhaul on branch `query-engine-overhaul`, centered on the new composer and route-based query model.
- Current validated overhaul state: one compiled, tested discrete vertical slice is integrated into `FacetContentService`, and widening across adjacent discrete and range targets is in progress.
- TBD: the exact cutover plan, final route configuration format, and any companion diagrams or ADRs.

## System Overview

The repository provides a .NET-based REST API for faceted browsing over the SEAD database.

At a high level, the system accepts a client facet-selection request, reconstructs query state, resolves the active facet and result configuration, compiles SQL against the SEAD PostgreSQL database, and returns either facet content or result sets.

The design is currently split between:

- a stable, existing query pipeline that powers the current API behavior
- a new query-composition architecture intended to replace the monolithic join-heavy approach with a route-based, anchor-centered model

## Main Runtime Components

The solution is organized around a small set of runtime responsibilities.

- `sead.query.api`: API entry point, request handling, and application startup/wiring
- `sead.query.core`: domain entities, query model, facet model, and shared contracts used by the runtime
- `sead.query.infra`: infrastructure and repository concerns, including access to persisted configuration and database-backed metadata
- `sead.query.composer`: in-progress query-engine redesign for route compilation and new composition rules
- `sead.query.test`: unit and integration tests for existing and new query behavior

These boundaries matter because the system’s main design problem is not HTTP transport. It is the translation from facet configuration into correct, composable SQL over a large relational schema.

## Core Domain Concepts

The following concepts shape both the current system and the redesign.

- Facet: a configurable filter definition that can be rendered in the UI and translated into SQL predicates
- Facet type: the behavior family for a facet, such as discrete, range, intersect, or geo-polygon
- Facet source: the table, view, or function where a facet’s category values originate
- Target facet: the facet currently being populated for UI content
- Result facet: the entity or aggregate used when producing final result sets
- Anchor: the common entity key used to compose multiple facet predicates into one query context
- Route: the path that connects a facet source to an anchor through schema relationships

The redesign treats anchor resolution as the primary composition contract. That is the central design decision behind the new query engine.

## Current Runtime Flow

The current request flow is centered on reconstructing a facet request, selecting the correct facet-content service by facet type, and compiling SQL for the active target facet.

In the current model, the runtime flow is:

1. Reconstitute the request payload into a `FacetsConfig` model.
2. Resolve the target facet and any trigger facet from the request context.
3. Remove invalid or stale picks before compiling SQL.
4. Select the facet-content service based on facet type.
5. Compile interval or category SQL for the target facet.
6. Compute category counts and outer counts.
7. Reattach user selections and build the final facet-content response.

This design keeps facet-type-specific behavior behind dedicated services and compilers, but the overall query model is still tightly coupled to explicit SQL templates and large join assemblies.

## Current Design Constraints

The existing query pipeline has a few defining characteristics.

- Facet loading is target-facet driven: the runtime compiles content for one active facet in the context of the other selected facets.
- Behavior is strongly split by facet type: discrete, range, intersect, and geo facets do not share one universal SQL compiler.
- SQL assembly is configuration-heavy: facet definitions, expressions, and templates drive much of the runtime behavior.
- Result generation is related to filtering but not identical to it: facet content and result payloads follow different compilation paths.

The main weakness of the current design is that the system must repeatedly encode relationship knowledge in SQL templates and joins, which becomes difficult to maintain as the number of supported anchors and facets grows.

## Query-Engine Overhaul

The in-progress query-engine overhaul introduces a different composition model.

Its architectural center is the anchor-based predicate contract:

- every active facet must resolve to the same anchor type within one composed query
- every facet predicate returns anchor keys, not arbitrary result shapes
- the final filtered set is produced by composing those anchor-key queries

The overhaul replaces template explosion with a route-based model and uses CTE-based composition to make the generated SQL more modular and easier to reason about.

## Overhaul Runtime Status

The overhaul is no longer only a design direction.

- The first compiled vertical slice is integrated into the branch runtime.
- The legacy runtime still remains authoritative outside the supported composed slice.
- The current widening work is extending the same contract across adjacent discrete targets and the first range-target families.

This means `docs/DESIGN.md` should describe both the current authoritative runtime and the intended architectural destination, while keeping the delivery state explicit.

## Planned Overhaul Components

The overhaul introduces or formalizes the following responsibilities.

- Query composer: orchestrates query assembly from facet configuration and route definitions
- Facet predicate resolvers: strategy-style implementations per facet type
- Route parser and route graph: resolve reusable route definitions into concrete table relationships
- Anchor-aware query contract: ensures every active facet returns compatible anchor keys
- Facet content query generation: produces grouped category output for UI population without breaking the anchor model

This is a change in architectural style, not only in SQL syntax. The goal is to move from large, facet-specific join templates to a composable system where relationship traversal is encoded once and reused.

## Planned Query Composition Model

The target composition flow is:

1. Identify the active anchor type for the request.
2. Resolve each active facet into a facet predicate query that returns anchor keys.
3. Materialize each predicate as a CTE or equivalent intermediate result.
4. Combine active facet predicates using `INTERSECT` or equivalent anchor-set composition.
5. Use the composed anchor set for either facet content queries or result generation.

This model separates three concerns that are more entangled in the current runtime:

- relationship traversal
- facet-specific filtering logic
- final result projection

That separation is the main design lever for maintainability and testability.

## Component Boundaries

The important component boundaries are:

- API layer boundary: request parsing and response formatting belong in the API layer, not in query-model classes
- Domain/query-model boundary: facet definitions, anchor semantics, and composition contracts belong in core abstractions
- Infrastructure boundary: repository access, persisted configuration, and database metadata lookup belong in infra
- Composer boundary: route resolution and new query composition logic belong in the composer, not in controllers or transport models

The system should continue to keep SQL-generation decisions close to query-specific services and compilers rather than spreading them across request handlers.

## Data and Persistence Design

The system depends on a PostgreSQL-backed SEAD schema and configuration-driven facet metadata.

From the current design and proposal, the important persistence assumptions are:

- facet definitions carry expressions, type metadata, and grouping/display behavior
- relationship data between tables can be modeled as graph edges or route fragments
- result generation is downstream from anchor filtering rather than the primary filtering mechanism itself
- range and geo facets rely on database capabilities such as range operators and spatial functions

This means the database is not just a passive store. It is an active execution environment whose supported operators materially shape the application design.

## Cross-Cutting Concerns

### Validation

- Request payloads must be reconstituted into valid facet configuration before query compilation begins.
- Invalid or stale picks should be removed early so downstream compilers work from normalized input.
- Under the new model, anchor-type mismatches are a first-class validation failure.

### Error Handling

- Failures should be attributed to the correct layer: request reconstruction, facet resolution, route resolution, or SQL compilation.
- The redesign should prefer explicit route and anchor validation errors over silent query fallbacks.

### Logging and Debuggability

- Generated SQL and the query-compilation path are important debugging surfaces.
- The move toward CTE-based composition is partly a debuggability decision because smaller named query pieces are easier to inspect than one large assembled statement.

### Performance

- Facet content queries must remain responsive because they drive interactive UI updates.
- The existing design pays complexity cost in template management; the new design pays some upfront modeling cost in routes and anchors to reduce long-term SQL complexity.
- Range, geo, and aggregate queries should continue to rely on database-native capabilities where that improves correctness and performance.

### Configuration

- The system is heavily configuration-driven, especially for facet behavior.
- The redesign increases the importance of configuration quality because route definitions and anchor mappings become architectural inputs rather than incidental SQL details.

## External Dependencies and Integration Points

The key external integration point is the SEAD PostgreSQL database.

Other notable dependencies, based on the current repo and project direction, include:

- ASP.NET Core hosting for the API runtime
- configuration files for environment-specific behavior
- database features such as range operators and spatial functions for advanced facet types

The system is intentionally database-aware. It is not designed around full database portability.

## Major Design Decisions and Tradeoffs

### Decision: Anchor-based composition

- Why: makes multi-facet composition predictable
- Benefit: every active facet participates through the same output contract
- Tradeoff: the system must reject mixed-anchor combinations instead of trying to infer or repair them implicitly

### Decision: Strategy-style facet behavior

- Why: discrete, range, intersect, and geo facets have materially different compilation rules
- Benefit: type-specific logic remains localized
- Tradeoff: more component types and interfaces to manage

### Decision: Route-based traversal instead of repeated join templates

- Why: repeated explicit join SQL does not scale across anchors and facet sources
- Benefit: lower template duplication and clearer relationship ownership
- Tradeoff: route modeling becomes a core design artifact and must be kept accurate

### Decision: Decouple filtering from result projection

- Why: facet filtering and result rendering change at different rates and have different performance needs
- Benefit: one filtered anchor set can support multiple result formats
- Tradeoff: the runtime must maintain a clear handoff between composed filters and final result queries

## Known Constraints and Open Items

- The composer architecture is in progress and should not be documented as fully authoritative runtime behavior yet.
- Final route-definition format and migration sequencing remain TBD.
- Final documentation split between `docs/DESIGN.md` and any future ADRs or subsystem notes is TBD.

## Related Documents

- `README.md`: short project overview
- `docs/DIAGRAMS.md`: visual overview of core interactions and workflows
- `docs/REQUIREMENTS.md`: durable system requirements for the active architecture direction
- `docs/proposals/QUERY_ENGINE_OVERHAUL/QUERY_ENGINE_OVERHAL.md`: top-level change request and bird's-eye overview of the overhaul
- `docs/proposals/QUERY_ENGINE_OVERHAUL/implementation_and_migration_plan.md`: execution tracker for the widening and migration work
- `docs/proposals/QUERY_ENGINE_OVERHAUL/system_requirements_specification.md`: proposal-era technical source material for the overhaul
- `docs/DEVELOPMENT.md`: contributor workflow and local development guidance
- `docs/TESTING.md`: test strategy and validation guidance
- `docs/OPERATIONS.md`: runtime and deployment guidance