## Purpose

This document describes how the SEAD Query API is structured, how its major components interact, and which design constraints govern the current system and the in-progress query-engine overhaul.

This is an architecture document, not a developer setup guide, testing guide, or operations runbook.

## Design Status

- Current authoritative runtime: the existing faceted query API implemented in the solution projects and described by the current request-flow notes.
- In progress: the query-engine overhaul on branch `query-engine-overhaul`, centered on the new composer and route-based query model.
- Current validated overhaul state: composed facet-content slices and the Phase 4 result-projection support surface are integrated into the branch runtime, while unsupported result routes remain explicit legacy-fallback exceptions.
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
- The composed facet-content path is active for the validated facet-content matrix and still falls back outside that supported surface.
- `ResultService` now enters result SQL compilation through an explicit result-projection handoff instead of calling `QuerySetupBuilder.Build(...)` directly.
- The legacy runtime still remains authoritative outside the supported composed slice.
- Phase 4 closes the current result-projection widening work on the validated support surface while keeping unsupported routes explicit.

This means `docs/DESIGN.md` should describe both the current authoritative runtime and the intended architectural destination, while keeping the delivery state explicit.

## Current Overhaul Contract Surface

The current composed path depends on a small contract surface that is already active in the branch runtime.

### Route Contract

- Route definitions are explicit traversal inputs, not inferred global join searches.
- The active route contract is owned by the route parser, route graph, route resolver, and route SQL compiler in `sead.query.composer/QueryComposer/RouteCompiler/`.
- The arrow-route parser resolves route expressions to a read-only table-chain contract before graph resolution and SQL compilation continue.
- Route compilation expects an explicit table chain before SQL generation and is responsible for emitting the table-traversal SQL used by composed filtering and target joins.
- The current route compiler interface accepts that table chain as a read-only input contract rather than requiring callers to provide a mutable collection.
- Missing route table chains are explicit input-validation failures at the route compiler boundary, not incidental null dereferences.
- The anchor-template route segments used to construct that table chain are also exposed as a read-only contract.

### Anchor Contract

- One composed query context uses one anchor type.
- Active facet predicates must resolve to the same anchor identity before they can be composed.
- Anchor mismatches are validation failures, not cases for silent repair.

### Facet-Resolver Contract

- Facet-type-specific resolver logic is responsible for turning facet configuration into anchor-key-producing predicate plans.
- The currently integrated picked-filter resolver path is the discrete-facet path used by the composed runtime slice.
- The discrete picked-filter input now exposes selected values as a read-only contract rather than a mutable collection.
- The discrete picked-filter operator must be non-empty; missing operators are explicit argument-validation failures rather than incidental null references or malformed SQL.
- Predicate planning now also supports same-table source-key overrides and enforced same-table facet clauses for the validated discrete predicate scenarios.

### Composed-Query Contract

- Predicate plans are composed through one anchor-key contract rather than through one global join shape.
- The current implementation combines compatible predicate plans through `INTERSECT`-style anchor-set composition.
- The composed filter must preserve the active anchor-key alias through the final composed SQL, including non-default aliases carried by validated predicate plans.
- Zero-filter, single-filter, and incompatible-anchor cases are treated as explicit contract cases rather than incidental SQL side effects.

### Facet-Content Contract

- Target facet content is generated from the composed anchor set, not from a re-expanded global join template.
- The current composed content path supports direct aggregate/result targets and routed visible targets whose category expression can be resolved either on the routed target table or on joined target-facet tables.
- The current composed content path also supports target-only discrete requests by using an explicit unfiltered anchor-set query instead of requiring prior picked predicates.
- The current composed content path also supports target-only range requests when the existing anchor route and interval category-info contract can be resolved without predicate-driven narrowing.
- The current composed content path also supports the active target-only intersect requests when the existing anchor route, target-table key, and interval category-info contract can be resolved without predicate-driven narrowing.
- The current composed content path also supports the active target-only geo-polygon requests when the existing anchor key already matches the geo target and the plugin-owned category-info query carries the polygon filter.
- Routed target-only discrete requests now overlay legacy-style discrete category-info rows onto composed counts so the composed result can retain zero-count categories where the legacy discrete path exposes them.
- The currently validated target set includes the baseline visible-target slices, multiple adjacent discrete targets, and the first validated range-target families recorded in the phase-0 tracker.

### Result-Projection Handoff Contract

- `ResultService` now treats result projection as a two-step contract: build a `ResultProjectionHandoff`, then compile the final tabular or map SQL from that handoff.
- `IResultProjectionHandoffBuilder` owns the boundary between composed result filtering and legacy fallback. `ResultService` no longer decides SQL shape by calling `QuerySetupBuilder.Build(...)` directly.
- The handoff currently carries the `QuerySetup` that the result compilers consume plus the sorted result fields used for projection, grouping, and ordering.
- The active composed handoff builder emits `composed_filter` CTE SQL and, when needed for routed map or site-level projections, a `target_route` CTE before the result compiler runs.
- `TabularResultSqlCompiler` and `MapResultSqlCompiler` preserve their format-specific projection rules, but now accept the handoff `QuerySetup.LeadingSql` as the composed SQL prologue instead of assuming a legacy-only query-setup path.
- Payload lookup remains a result-service concern. After SQL compilation and query execution, `ResultService` still resolves the view-type-specific payload service separately from the filtering handoff.
- The currently validated composed handoff shapes include discrete, clause-only discrete, range, geo-polygon, and normalized intersect requests plus the promoted target-only result families tracked in `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`.
- Unsupported result requests remain explicit. They still fall back through the legacy result-projection handoff path rather than silently mixing composed and legacy query-setup behavior inside one compiled result request.

### Unsupported-Request Boundary

- Unsupported composed requests must remain explicit.
- `FacetContentService.Load` uses the composed path only when `ComposedFacetContentService.CanHandle(...)` returns `true`; otherwise it falls back to the legacy category-count path.
- `ResultService.Load` now uses whatever `IResultProjectionHandoffBuilder` returns, so the unsupported-result boundary is the handoff builder itself: supported requests return composed `composed_filter` and optional `target_route` SQL, while unsupported requests return the legacy query-setup handoff.
- Predicate-side clauses that cannot be normalized onto the predicate source table, including joined-table clause references, remain outside the composed contract and continue to fall back before composed execution starts.
- Discrete targets whose join key cannot be derived from a simple target expression and that do not expose a real target primary key also remain outside the composed contract and fall back before composed execution starts.
- Target-only discrete requests remain outside the composed contract when routed zero-predicate execution still cannot derive the target-side join key, resolve the target route, or enumerate the legacy-compatible outer category set.
- `ComposedFacetContentService.Load` throws for direct unsupported use with an actionable error that tells callers to check `CanHandle(...)` first or to use `FacetContentService` for legacy fallback.
- The current boundary is still the legacy category-count path for requests outside the validated composed contract.
- Remaining unsupported visible facets are the ones whose predicate side still does not resolve cleanly to a source-table key or whose target-side join key cannot yet be derived from the routed target contract.

### Current Validation Anchors

- The current route-parser contract is anchored in `sead.query.composer/QueryComposer/RouteCompiler/ArrowRouteParser.cs` and `sead.query.test/UnitTests/QueryComposer/RouteCompiler/ArrowRouteParserTests.cs`.
- The current discrete resolver input-validation contract is anchored in `sead.query.composer/QueryComposer/RouteCompiler/DiscreteFacetPredicateResolver.cs` and `sead.query.test/UnitTests/QueryComposer/RouteCompiler/DiscreteFacetPredicateResolverTests.cs`.
- The grouped live support matrix is maintained in `sead.query.test/LiveTests/FacetLoadService.cs` through `SupportedComposedLiveUris`.
- The supported visible and discrete subset is also grouped explicitly in that file through `SupportedComposedVisibleAndDiscreteLiveUris` and the `FacetContentService_ComposedSupportedVisibleAndDiscreteLiveSlices_*` tests.
- The supported range subset is also grouped explicitly in that file through `SupportedComposedRangeLiveUris` and the `FacetContentService_ComposedSupportedRangeLiveSlices_*` tests.
- The supported intersect subset is also grouped explicitly in that file through `SupportedComposedIntersectLiveUris` and the `FacetContentService_ComposedSupportedIntersectLiveSlices_*` tests, currently covering `analysis_entity_ages:analysis_entity_ages` and `dendro_age_contained_by:dendro_age_contained_by`.
- The supported geo-polygon subset is also grouped explicitly in that file through `SupportedComposedGeoPolygonLiveUris` and the `FacetContentService_ComposedSupportedGeoPolygonLiveSlices_*` tests, currently covering the active `sites_polygon` request shape.
- The active composed-path assertions in that file are `FacetContentService_ComposedSupportedLiveSlices_UseComposedFacetContentQuery` and `FacetContentService_ComposedSupportedLiveSlices_MatchLegacyFacetContent`.
- The runtime handoff between composed and legacy behavior is anchored in `sead.query.core/Services/FacetContent/FacetContentService.cs` and `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs`.
- The current direct unsupported-load boundary is anchored in `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs` and `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`.
- The current same-table target-only discrete contract is also anchored in `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs` and `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`.
- The current target-only range contract is also anchored in `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs`, `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`, and `sead.query.test/LiveTests/FacetLoadService.cs` through the `geochronology:geochronology` slice.
- The current target-only intersect contract is also anchored in `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs`, `sead.query.core/QueryComposer/Strategies/DiscreteFacetContentQueryComposer.cs`, `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`, and `sead.query.test/LiveTests/FacetLoadService.cs` through the `analysis_entity_ages:analysis_entity_ages` and `dendro_age_contained_by:dendro_age_contained_by` slices.
- The current target-only geo-polygon contract is also anchored in `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs`, `sead.query.core/QueryComposer/Strategies/DiscreteFacetContentQueryComposer.cs`, `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`, and `sead.query.test/LiveTests/FacetLoadService.cs` through the active `sites_polygon` slice.
- The current routed target-only discrete outer-category overlay contract is also anchored in `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs` and `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`.
- The current composed-query alias contract is anchored in `sead.query.core/QueryComposer/Strategies/IntersectComposedFilterQueryComposer.cs` and `sead.query.test/UnitTests/QueryComposer/Strategies/IntersectComposedFilterQueryComposerTests.cs`.
- The current route compiler input-validation contract is anchored in `sead.query.composer/QueryComposer/RouteCompiler/RouteSqlCompiler.cs` and `sead.query.test/UnitTests/QueryComposer/RouteCompiler/RouteSqlCompilerTests.cs`.
- The current routed zero-predicate genus validation is anchored in `sead.query.test/LiveTests/FacetLoadService.cs` through `FacetContentService_ComposedTargetOnlyGenusSlice_*` and the grouped `SupportedComposedVisibleAndDiscreteLiveUris` matrix.

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
- Current state: the composed facet-content path already uses the explicit anchor-set contract, and `ResultService` now depends on an explicit result-projection handoff builder before SQL compilation
- Current limitation: the active Phase 4 handoff builder composes validated discrete, range, geo-polygon, and normalized intersect result requests through `composed_filter` and `target_route` joins, but still falls back to the legacy query-setup path for unsupported result requests
- Phase 4 outcome: supported result formats now use the widened handoff on the validated tabular and map matrix, while remaining unproven routes stay on the explicit fallback boundary as follow-up work

## Known Constraints and Open Items

- The composer architecture is in progress and should not be documented as fully authoritative runtime behavior yet.
- Final result projection is on the composed path for the validated Phase 4 support matrix; unsupported result requests still fall back to the legacy query-setup path.
- Final route-definition format and migration sequencing remain TBD.
- Final documentation split between `docs/DESIGN.md` and any future ADRs or subsystem notes is TBD.

## Related Documents

- `README.md`: short project overview
- `docs/DIAGRAMS.md`: visual overview of core interactions and workflows
- `docs/REQUIREMENTS.md`: durable system requirements for the active architecture direction
- `docs/proposals/QUERY_ENGINE_OVERHAUL/QUERY_ENGINE_OVERHAL.md`: top-level change request and bird's-eye overview of the overhaul
- `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_0.md`: phase-0 execution tracker for the vertical slice and widening work
- `docs/proposals/QUERY_ENGINE_OVERHAUL/archive/system_requirements_specification.md`: archived proposal-era technical source material for the overhaul
- `docs/DEVELOPMENT.md`: contributor workflow and local development guidance
- `docs/TESTING.md`: test strategy and validation guidance
- `docs/OPERATIONS.md`: runtime and deployment guidance