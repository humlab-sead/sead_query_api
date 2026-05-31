# Requirements

## Purpose

This document records the durable system requirements for the SEAD Query API query engine.

It is not a proposal and not an implementation log. It describes the requirements the runtime and the in-progress overhaul must satisfy.

## Current Status

- Current authoritative runtime: the legacy faceted query pipeline, plus the supported composed slice on the `query-engine-overhaul` branch.
- Required direction: route-based, anchor-centered query composition.
- Current branch status: one proven vertical slice is integrated and validated, and the composed path is already widened across multiple discrete target families and initial range-target support.

## Core Terms

- Anchor: the entity identity used as the common composition key in one query context
- Anchor key: the identifier returned by a facet predicate and consumed by composed filtering
- Configuration revision: one imported, versioned copy of facet and route configuration that the runtime treats as authoritative
- Facet: a configurable filter definition with one facet type and one or more supported anchors
- Target facet: the facet currently being populated for UI content
- Result set: the final data projection produced from a filtered anchor set
- Route: the configured traversal from a facet source to an anchor

## Functional Requirements

### Query Composition

- The system must support a chain of facets composed dynamically from user input.
- Each active filtering facet must resolve to a query that returns distinct anchor keys.
- All active facets in one composed query must return keys for the same anchor type.
- Composed filtering must keep only anchor keys that survive all active facet predicates.
- The runtime must reject incompatible anchor mixes explicitly.
- A facet may support more than one anchor type, but only through explicit configuration.
- If a facet has no active user input, it must not change the composed filtering result.
- The runtime may implement composed filtering with `INTERSECT` or equivalent inner-join semantics, as long as the shared anchor contract is preserved.

### Route And Traversal

- The system must support explicit route definitions from facet sources to anchors.
- Route resolution must be reusable across facets and anchor types.
- The runtime must prefer explicit route errors over silent fallback behavior when route resolution fails.

### Facet Types

- The system must support discrete facets.
- The system must support range facets.
- The system must support intersect-style facets.
- The system must support GIS polygon facets.
- Each facet must use one defined operator family rather than mixing multiple operator semantics in one facet definition.

### Predicate Isolation

- Facet-specific filtering logic must stay isolated to facet-type-specific query logic.
- Filtering must operate on the facet's own source or target structures, while still returning anchor keys.
- Shared query-composition services must not need to know facet-specific business rules beyond the explicit facet contract.

### Facet Content

- The system must generate target facet content from the composed anchor set produced by the other active facets.
- A target facet content query must return grouped values and counts appropriate to the target facet type.
- Discrete facet content must return grouped categories and counts.
- Range facet content must support grouped range output rather than only raw values.
- The counted entity for facet content must be a supported anchor identity, not an arbitrary row count.

### Result Sets

- The system must support final result generation from the filtered anchor set.
- Different result-set formats may be projected from the same filtered anchor set.
- Result projection must remain separate from composed filtering and facet content generation.

### Configuration Governance

- The system must support checked-in, reviewed authoring for facet and route configuration.
- The runtime must execute against one imported, normalized configuration copy in the `facet` schema rather than interpreting raw authoring files during requests.
- Exactly one active configuration revision must be authoritative for runtime reads at a time.
- Import and validation must resolve authoring keys through runtime lookup tables rather than relying on hard-coded database ids in authoring files.
- Generated route families and macros must be the default authoring model for repeatable source-to-anchor traversal, while explicit routes and SQL overrides remain reserved for true exceptions.
- A configuration revision that fails validation or import must not become active partially.
- Explicit SQL overrides may exist for exception cases, but they must remain exceptional rather than becoming the primary authoring model.

## Quality Requirements

### Correctness

- Equivalent supported requests must produce results consistent with the legacy runtime during migration.
- Unsupported composed requests must fall back explicitly or fail explicitly, rather than silently produce ambiguous results.

### Testability

- Route parsing and route resolution must be unit-testable.
- Anchor-key-producing facet predicates must be testable through stable contract-level checks.
- Facet predicate resolution must be testable independently of the final runtime path.
- Runtime integration must be validated through focused and grouped regression coverage.
- Migration validation must compare legacy and composed behavior separately for facet content and final result generation.

### Debuggability

- Generated SQL must remain inspectable.
- Query composition should favor named intermediate query pieces, such as CTEs, where they improve diagnosis.

### Maintainability

- Relationship traversal must be modeled once and reused, rather than re-encoded repeatedly in facet-specific join templates.
- Facet-type behavior must remain localized.
- Durable architecture and requirements documents must distinguish current runtime truth from planned destination.

### Performance

- Facet content queries must remain responsive enough for interactive UI use.
- The system should rely on native PostgreSQL capabilities where that improves correctness or performance.

## Current Delivery Boundary

The following points are already required and partially delivered in the branch:

- explicit route-based composition is the target architecture
- one proven vertical slice is integrated and validated
- widening across adjacent discrete and range targets is active

The following remain required but not yet fully delivered:

- broad completion across all facet families
- fully settled result-set generation on the new composer
- final route governance and migration cutover rules

## Non-Goals

- This document does not define rollout scheduling.
- This document does not define staffing or delivery ownership.
- This document does not define feature-flag infrastructure.
- This document does not define operational risk management.
- This document does not define frontend behavior beyond backend query and content requirements.

## Related Documents

- `docs/DESIGN.md`
- `docs/proposals/done/QUERY_ENGINE_OVERHAUL/QUERY_ENGINE_OVERHAL.md`
- `docs/proposals/done/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_0.md`