## Purpose

This document describes the current architecture of the SEAD Query API: how the system is structured, how requests move through the runtime, and which design constraints shape the codebase.

This is an architecture document. It focuses on component boundaries, runtime flows, persistence assumptions, and major design decisions. It is not a setup guide, test guide, or operations runbook.

## Design Status

- Current authoritative runtime: the checked-in .NET API, its supporting libraries, and the database-backed facet configuration it loads at runtime.
- Current query baseline: the composer-based, anchor-centered query path is the baseline for the validated composed surface for both facet-content and result requests.
- Retained compatibility boundary: legacy query services still exist as an explicit fallback path for request families that remain outside the validated composed contract.
- Planned follow-up work: widen composed coverage, reduce explicit fallback cases, and continue tightening configuration and route-governance workflows.

## System Overview

The repository provides a .NET REST API for faceted browsing and result retrieval over the SEAD PostgreSQL database.

At runtime, the system accepts a client request that carries facet state and, for result endpoints, result-view configuration. The runtime reconstructs that request into internal query models, normalizes the active selections, resolves the target facet or result projection, compiles SQL against the SEAD database, and returns either facet content or result payloads.

The system is best understood as five cooperating layers:

- API hosting and HTTP transport
- shared domain and query contracts
- infrastructure and repository access
- query composition and SQL generation
- database execution and result materialization

The most important architectural fact is that the system is not primarily an HTTP problem. Its core responsibility is translating configurable facet definitions and user selections into correct, composable SQL over a large relational schema.

## Main Runtime Components

The solution is organized around a small set of runtime responsibilities.

- `sead.query.api`: ASP.NET Core host, dependency wiring, controllers, request entry points, serializers, and HTTP-facing service composition
- `sead.query.core`: domain entities, facet and result models, service contracts, query abstractions, and shared query-composition strategies used across the runtime
- `sead.query.infra`: repositories, cache and configuration helpers, metadata access, and other infrastructure concerns that support runtime execution
- `sead.query.composer`: route parsing, route resolution, predicate planning, composed SQL generation, and the composer-owned services that drive the current composed query baseline
- `sead.query.test`: unit, integration, and live tests that validate controllers, services, compilers, repositories, and the current composed support matrix

These boundaries matter because the system has to keep HTTP concerns, domain/query semantics, infrastructure access, and SQL-generation logic separate. Query behavior belongs in query services and compilers, not in controllers or transport models.

## Runtime Baseline

The current runtime is a hybrid system with a clear primary path.

- The baseline query model is composition-first: supported requests are handled through the composer-owned route, predicate, and anchor contracts.
- The runtime still preserves explicit legacy fallback for unsupported request shapes so that unsupported cases fail over in a controlled way instead of mixing partial composed and legacy logic inside one request.
- Facet-content and result generation are separate runtime flows, but both now depend on the same general idea: establish the active query context first, then project that context into facet categories or result payloads.

This means the architecture should be read as one current system, not as a stable legacy system plus a separate speculative redesign. The composer is already part of the runtime baseline; the remaining legacy path is a bounded compatibility surface.

## Core Domain Concepts

Several domain concepts shape the whole system.

- Facet: a configurable filter definition that can be rendered in the UI and translated into SQL predicates
- Facet type: the behavior family for a facet, such as discrete, range, intersect, or geo-polygon
- Facet source: the table, view, or function that provides category values or filter inputs for a facet
- Target facet: the facet currently being populated for UI content
- Result facet: the entity or aggregate that defines the final result shape
- Anchor: the common entity identity used to combine multiple facet predicates into one query context
- Route: the configured traversal path that connects a facet source or result target to the active anchor

The anchor concept is the main unifying contract in the current query architecture. Both facet filtering and result filtering are easier to reason about when active predicates are reduced to one compatible anchor-key set before final projection.

## Request and Runtime Flows

### Startup and Composition

At application startup, the API host wires controllers, domain services, repositories, serializers, and query services through dependency injection. Runtime behavior depends on a checked-in application host plus a database-backed configuration model. The application does not execute directly from raw YAML authoring files; it executes against the imported normalized configuration stored in the database.

This separation matters because startup owns service composition, while query behavior depends on runtime configuration that may change independently of code deployment.

### Facet-Content Flow

Facet-content loading is target-facet driven.

1. Reconstitute the request payload into a `FacetsConfig` model.
2. Resolve the target facet and any trigger facet from the request context.
3. Remove invalid or stale selections before query compilation begins.
4. Decide whether the request is inside the composed support surface.
5. For composed requests, resolve routes and predicate plans, build the composed anchor set, and generate category or interval SQL for the target facet.
6. For unsupported requests, use the retained legacy category-count path.
7. Compute counts, outer counts, and selection state.
8. Assemble the final facet-content response.

The important architectural point is that the runtime still presents one facet-content service boundary even though two internal execution paths exist. Capability selection happens inside the service layer, not at the HTTP boundary.

### Result Flow

Result generation is related to filtering but is not identical to facet-content loading.

1. Reconstitute facet state and result-view configuration.
2. Normalize facet selections and resolve the result target.
3. Build a `ResultProjectionHandoff` that separates filtering from final projection.
4. For composed requests, emit the composed filter SQL prologue and, when required, a routed target join.
5. For unsupported requests, build the legacy query-setup handoff.
6. Compile final tabular or map SQL from the handoff.
7. Execute the query and attach any view-specific payload.
8. Return the result content set.

This handoff boundary is central to the whole system because result rendering changes at a different rate than filtering logic. The runtime therefore treats filtering as one concern and result-shape projection as another.

## Query Architecture

The current query baseline uses a composed, anchor-centered model for the validated support surface.

Its core contract is simple:

- one composed request uses one anchor type
- each active predicate resolves to anchor keys
- composed filtering combines compatible predicate plans into one anchor-key set
- target facet content or final result rows are projected from that anchor-key set

Within that model, the composer owns several responsibilities.

- Route parsing: interpret configured route expressions into an explicit traversal contract
- Route resolution: map route definitions onto concrete table relationships
- Predicate planning: translate facet configuration and selections into facet-type-specific SQL fragments
- Composed filtering: combine compatible predicate plans into a single anchor-set query
- Target projection support: expose the joins or target routes required for facet-content or result SQL compilation

This is a system-level design choice, not an implementation detail. It reduces repeated join-template logic, keeps relationship traversal explicit, and makes unsupported cases easier to reject or fall back cleanly.

## Component Boundaries

The most important component boundaries are stable.

- API boundary: controllers and DTOs handle request parsing, endpoint behavior, and response formatting
- Domain/query boundary: facet semantics, anchor rules, query contracts, and shared models live in core abstractions
- Infrastructure boundary: repository access, persisted configuration, metadata lookup, and supporting database access live in infra
- Composer boundary: route parsing, route graphs, predicate resolution, and composed SQL generation live in the composer layer
- Database boundary: PostgreSQL is the execution environment for filtering, grouping, aggregates, range logic, and spatial operations

These boundaries keep SQL-generation decisions close to query services and compilers instead of spreading them across controllers, configuration classes, or transport objects.

## Data and Persistence Design

The system depends on two kinds of persisted information.

- SEAD domain data in PostgreSQL
- query and facet configuration stored in the `facet` schema

Facet definitions carry expressions, type metadata, display behavior, grouping rules, and other query-shaping details. Route definitions describe how facet sources and result targets reach the active anchor.

Configuration authoring is moving toward reviewed YAML plus importer-managed normalization, but runtime execution remains database-backed. The application reads one active imported configuration copy rather than interpreting authoring files directly during requests.

This has several consequences.

- The database is not just a storage backend; it is an active query-execution environment.
- Configuration quality is architecture-critical because route and facet definitions influence query correctness.
- Runtime determinism depends on having one authoritative active configuration revision.

## Cross-Cutting Concerns

### Validation

- Request payloads must be reconstructed into valid internal models before query compilation begins.
- Invalid or stale selections should be removed early.
- Anchor mismatches, missing route inputs, and unsupported request shapes are explicit contract failures.
- The composed path should reject invalid input deliberately rather than drifting into malformed SQL or implicit behavior repair.

### Error Handling

- Failures should be attributed to the right layer: request reconstruction, repository/configuration loading, route resolution, predicate planning, or SQL compilation.
- Unsupported composed requests should remain explicit so the runtime can either fall back intentionally or fail with an actionable error.

### Logging and Debuggability

- Generated SQL is a primary debugging surface.
- Route resolution, predicate planning, and composed SQL assembly should remain inspectable as separate steps.
- CTE-based composition improves debuggability because named query parts are easier to reason about than one large assembled join template.

### Performance

- Facet-content queries must remain responsive because they drive interactive UI updates.
- The system prefers database-native capabilities for grouping, range operations, aggregates, and spatial behavior when those improve correctness and performance.
- Query composition moves complexity away from repeated handwritten join templates and into reusable route and anchor modeling.

### Testing

- Unit tests validate route parsing, route compilation, predicate resolution, composed filtering, and service contracts.
- Integration and live tests validate controller behavior, database-backed execution, and the supported composed request matrix.
- The test project is part of the architecture because the runtime depends on explicit validation of both composed behavior and retained fallback boundaries.

## External Dependencies and Integration Points

The key external integration points are:

- PostgreSQL as the execution backend for SEAD data and runtime configuration
- ASP.NET Core as the API host and dependency-injection runtime
- environment-specific application configuration files for host behavior
- database capabilities such as range operators and spatial functions for advanced facet families

The system is intentionally database-aware. It does not aim for full database portability, because major parts of correctness and performance depend on PostgreSQL behavior.

## Major Design Decisions and Tradeoffs

### Decision: composition-first query baseline

- Why: supported requests are easier to reason about when filtering is expressed through one explicit anchor-centered contract
- Benefit: consistent query planning across facet-content and result flows
- Tradeoff: unsupported families must remain explicit until their routes and predicates are modeled correctly

### Decision: strategy-style facet behavior

- Why: discrete, range, intersect, and geo facets have materially different compilation rules
- Benefit: facet-type-specific behavior stays localized instead of forcing one over-general compiler
- Tradeoff: more service types and contracts to maintain

### Decision: explicit route modeling

- Why: repeated join-template logic does not scale across anchors, sources, and result targets
- Benefit: relationship traversal is encoded once and reused
- Tradeoff: route quality becomes a core design dependency and must be validated carefully

### Decision: separate filtering from final projection

- Why: result filtering and result rendering change independently and have different runtime constraints
- Benefit: one filtered anchor set can support multiple output formats
- Tradeoff: the handoff between filter construction and final SQL projection must stay clear and stable

### Decision: database-backed active configuration

- Why: runtime needs one deterministic, normalized configuration source even when authoring moves toward reviewed YAML
- Benefit: stable runtime reads, revision tracking, and explicit import validation
- Tradeoff: configuration governance spans both authoring and import workflows

## Known Constraints and Follow-Up Areas

- The current runtime baseline is composition-first for the validated surface, but fallback remains necessary for unsupported request families.
- Some request shapes still depend on legacy behavior because their route, join-key, or predicate contracts are not yet modeled cleanly enough for composed execution.
- Runtime configuration governance continues to depend on importer-managed normalization and revision tracking.
- The supported composed matrix is broader than the historical vertical slice, but validation still needs to expand deliberately rather than by assumption.
- Additional architecture notes or ADR-style records may still be useful for narrower subsystems. `TBD`.

## Related Documents

- `README.md`: project overview and entry points
- `docs/DIAGRAMS.md`: current runtime and query-flow diagrams
- `docs/REQUIREMENTS.md`: durable system requirements
- `docs/DEVELOPMENT.md`: local workflow and contributor guidance
- `docs/TESTING.md`: validation strategy and test workflow
- `docs/OPERATIONS.md`: runtime and deployment guidance