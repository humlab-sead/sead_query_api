# Requirements

## Purpose

This document records the durable system requirements for the SEAD Query API query engine.

It is not a proposal and not an implementation log. It describes the requirements the runtime and the in-progress overhaul must satisfy.

## Current Status

- Current authoritative runtime: the legacy faceted query pipeline, plus the supported composed slice on the `query-engine-overhaul` branch.
- Required direction: route-based, anchor-centered query composition.
- Current branch status: one discrete vertical slice is integrated and validated; widening across adjacent targets is in progress.

## Core Terms

- Anchor: the entity identity used as the common composition key in one query context
- Anchor key: the identifier returned by a facet predicate and consumed by composed filtering
- Facet: a configurable filter definition with one facet type and one or more supported anchors
- Target facet: the facet currently being populated for UI content
- Result set: the final data projection produced from a filtered anchor set
- Route: the configured traversal from a facet source to an anchor

## Functional Requirements

### Query Composition

- The system must support a chain of facets composed dynamically from user input.
- Each active filtering facet must resolve to a query that returns distinct anchor keys.
- All active facets in one composed query must return keys for the same anchor type.
- The runtime must reject incompatible anchor mixes explicitly.
- A facet may support more than one anchor type, but only through explicit configuration.
- If a facet has no active user input, it must not change the composed filtering result.

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

## Quality Requirements

### Correctness

- Equivalent supported requests must produce results consistent with the legacy runtime during migration.
- Unsupported composed requests must fall back explicitly or fail explicitly, rather than silently produce ambiguous results.

### Testability

- Route parsing and route resolution must be unit-testable.
- Facet predicate resolution must be testable independently of the final runtime path.
- Runtime integration must be validated through focused and grouped regression coverage.

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
- one compiled discrete vertical slice is integrated and validated
- widening across adjacent discrete and range targets is active

The following remain required but not yet fully delivered:

- broad completion across all facet families
- fully settled result-set generation on the new composer
- final route governance and migration cutover rules

## Non-Goals

- This document does not define rollout scheduling.
- This document does not define staffing or delivery ownership.
- This document does not define frontend behavior beyond backend query and content requirements.

## Related Documents

- `docs/DESIGN.md`
- `docs/proposals/QUERY_ENGINE_OVERHAUL/QUERY_ENGINE_OVERHAL.md`
- `docs/proposals/QUERY_ENGINE_OVERHAUL/implementation_and_migration_plan.md`