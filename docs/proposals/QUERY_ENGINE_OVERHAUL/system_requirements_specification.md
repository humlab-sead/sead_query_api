# Proposal: Anchor-Based Query Composition For SEAD Facets

## Status

- Proposed feature / change request
- Scope: query composition, facet content generation, route resolution, and anchor-aware filtering in the SEAD Query API
- Goal: replace the current monolithic join-path compilation model with isolated facet predicates that compose through a shared anchor contract

## Summary

The current faceted query pipeline is hard to extend because every active facet is flattened into a single join-heavy SQL query. The proposal is to move query composition to an anchor-based model where each facet resolves its own filtering logic in isolation and contributes anchor keys to a composed query. Route definitions remain important, but they become explicit inputs to facet compilation instead of implicit global pathfinding.

This proposal is intentionally narrower than the earlier 2025 draft. Route-compilation work exists, but the full CTE and strategy-based composer is not integrated into the runtime. The recommendation is to continue from the existing route and anchor prototypes, not to restart from the broad migration plan.

## Problem

The current query builder has three structural limits.

- It derives one global join shape from all active facets, which makes local facet behavior hard to reason about.
- It depends on graph pathfinding to discover joins, which becomes brittle when the same table must be joined more than once or when the shortest path is not the right semantic path.
- It mixes filtering, join discovery, facet content generation, and final result generation into a tightly coupled flow.

This leads to expensive queries, workaround views, hard-to-debug SQL, and high change cost for new facet types or new anchor shapes.

## Scope

This proposal covers:

- anchor-based composition for filtering facets
- explicit route definitions between facet sources and anchors
- a facet predicate contract that yields anchor keys
- facet content generation that reuses the composed anchor set
- an incremental implementation path from the current codebase state

## Non-Goals

This proposal does not cover:

- a full runtime migration or rollout plan
- feature-flag infrastructure
- staffing, timeline, or resource planning
- operational risk management
- frontend or client redesign beyond what the backend contract requires

## Current Behavior

The current runtime still uses the legacy faceted query pipeline.

The codebase contains useful redesign work, but it is incomplete:

- route parsing and route-based SQL compilation have active prototypes in `sead.query.composer/QueryComposer/RouteCompiler/`
- the broader strategy-based composer work under `sead.query.composer/BackBurner/` is excluded from compilation
- anchor and route entities plus repository scaffolding have been explored, but the main query-composer contracts in `sead.query.core/QueryComposer/` are still stubs

That means the branch proves the direction, but not the end-to-end architecture yet.

## Proposed Design

### Composition Contract

Every active filtering facet compiles to a query fragment that returns distinct anchor keys for one anchor type.

The composed filter query then combines those anchor-key queries by anchor identity. `INTERSECT` and `INNER JOIN` are both acceptable implementation choices as long as the contract remains the same: only anchors present in all active facet predicates survive the composed filter.

### Anchor Model

An anchor is a configured entity that defines the unit being counted or returned, for example a sample, site, or analysis entity.

Each composed query must use exactly one anchor type. A facet may support multiple anchor types, but only through explicit per-anchor route or predicate configuration.

### Route Model

Routes define how a facet source reaches an anchor. They replace implicit graph discovery with named, explicit traversal rules.

The current route work in `RouteCompiler` should remain the foundation:

- arrow-based route specifications for readable route definitions
- route parsing and macro expansion
- route resolution and SQL generation from explicit table sequences

The proposal does not require every route to live in the database immediately. Route configuration can remain file- or fixture-backed while the contract is stabilized.

### Facet Predicate Isolation

Facet-specific behavior belongs in facet-type-specific resolver implementations.

At minimum, the design should support:

- discrete facets
- range facets
- intersect facets
- GIS polygon facets

Each resolver is responsible for translating facet configuration into anchor-key-producing SQL without depending on other facet implementations.

### Facet Content Queries

Facet content generation should use the composed anchor set from all other active facets, then aggregate values for the target facet.

This keeps filtering and content generation separate while still sharing the same anchor contract.

### Runtime Integration Strategy

The legacy runtime remains authoritative until the new composer can prove one complete vertical slice from facet configuration to composed filter query and facet content query.

The proposal therefore favors incremental integration over a full rewrite branch that stays permanently compiled out.

## Alternatives Considered

### Keep Patching The Legacy Graph Compiler

Rejected. It preserves the hardest part of the current system: implicit global join discovery.

### Revive The Existing BackBurner Design As-Is

Rejected. It contains useful ideas, but in its current state it is excluded from compilation and does not align cleanly with the newer route work.

### Big-Bang Rewrite

Rejected. The current branch history shows that broad redesign plans are easy to over-document and hard to land incrementally.

## Risks And Tradeoffs

- The design becomes more explicit and more maintainable, but it also requires stricter facet contracts.
- Route definitions reduce ambiguity, but they move more responsibility into configuration and validation.
- Supporting multiple anchor types per facet increases reuse, but it also increases configuration complexity.
- Keeping the legacy runtime authoritative for longer slows visible rollout, but it reduces the chance of landing another large compiled-out subsystem.

## Testing And Validation

Validation should follow the implementation path, not wait for a full rewrite.

- keep unit coverage for route parsing, route graph behavior, route resolution, and route SQL generation
- add contract tests for anchor-key-producing facet predicates
- add integration tests that compare legacy and new behavior for one supported vertical slice
- validate facet content generation separately from final result generation

## Acceptance Criteria

- route and anchor contracts are documented and reflected in compiled code
- one end-to-end discrete facet path uses the new composer without relying on compiled-out `BackBurner` code
- the new path produces distinct anchor keys for a single anchor type and rejects incompatible anchor mixes
- facet content generation for the same slice works from the composed anchor set
- targeted tests exist for route compilation, predicate resolution, and the integrated vertical slice

## Recommended Delivery Order

1. Stabilize the route and anchor contracts already explored in the branch.
2. Integrate a compiled resolver contract for one anchor-aware discrete facet path.
3. Add facet content generation on top of the same composed anchor set.
4. Extend the resolver model to range and intersect facets.
5. Reassess GIS and broader result-set generation after the first vertical slice is proven.

## Open Questions

- Should the composed filter query prefer `INTERSECT`, `INNER JOIN`, or support both behind the same contract?
- Which route and anchor definitions must be database-backed from the start, and which can remain in code or fixtures initially?
- Should the resolver interfaces live in `sead.query.core` or remain implementation-facing in `sead.query.composer` until the contracts settle?

## Final Recommendation

Continue the overhaul, but narrow it.

The next target should be a compiled, tested vertical slice built on explicit routes and anchor-key predicates. Treat the current branch as proof of direction, not as a completed migration plan.

