# Implementation Plan: First Deliverable For The Query Overhaul

## Status

- Proposed feature / change request
- Scope: implementation handoff for the first usable slice of the anchor-based query composer
- Goal: turn the current route and anchor experiments into compiled, tested, incremental product code

## Summary

The old implementation plan assumed a broad migration program that does not match the current codebase. This plan replaces it with a smaller, realistic handoff: stabilize the route work already explored, define the minimal anchor and resolver contracts, and land one compiled vertical slice before expanding scope.

## Problem

The current redesign work exists in three different states:

- useful route-compilation code in `sead.query.composer/QueryComposer/RouteCompiler/`
- broader but compiled-out redesign work in `sead.query.composer/BackBurner/`
- anchor and route entities plus repository scaffolding explored outside the integrated runtime path

Without a narrower plan, the overhaul will stay split between prototypes, empty stubs, and excluded code.

## Scope

This plan covers:

- selecting the implementation path to keep
- integrating the minimal contracts needed for one end-to-end slice
- defining the order of backend work
- defining the validation expected before expanding to more facet types

## Non-Goals

This plan does not cover:

- a production migration or rollback plan
- staffing, ownership, or estimates
- risk management
- a full replacement of the legacy runtime in one pass

## Current Behavior

Current implementation status is mixed:

- route parser, route graph, route resolver, and route SQL compiler work exist and have matching test areas
- `BackBurner` composer code is still excluded from compilation
- anchor and route entities and repositories exist, but the main `sead.query.core/QueryComposer/` contracts are still empty
- `NewFacetContentService.cs` is present but empty

This means the next step is not more broad design. It is integration and reduction.

## Proposed Design

### Keep One Composer Path

Do not continue maintaining both the active `RouteCompiler` track and the broader `BackBurner` design as peer solutions.

Use the active route compiler work as the implementation base. Pull only the ideas worth keeping from `BackBurner`, and reintroduce them into compiled code intentionally.

### Define Minimal Contracts First

Before adding more facet types, make the following contracts explicit and compiled:

- anchor model and anchor key naming
- route repository and route lookup contract
- facet predicate resolver contract
- composed filter query contract
- facet content query contract

These contracts should be small enough to support one discrete facet path without forcing the full final architecture up front.

### Land One Vertical Slice

The first deliverable should support one discrete facet path from configuration to:

- route resolution
- anchor-key predicate SQL
- composed filter query
- facet content query based on the composed anchor set

This slice is the gate for expanding to range, intersect, and GIS facets.

### Keep Legacy Runtime As The Fallback

The existing runtime remains authoritative while the first vertical slice is proven. The plan does not require feature flags yet. It requires a clear integration boundary and test coverage around the new path.

## Testing And Validation

Validation should be staged.

### Contract Validation

- unit tests for route parsing, graph lookup, route resolution, and route SQL generation
- tests that each resolver returns distinct anchor keys for the chosen anchor type

### Integration Validation

- one integration test path for a discrete facet using the new composer flow
- comparison checks against the legacy path for the same dataset and request shape

### Expansion Gate

Do not add additional facet types until the discrete-facet slice is compiled, integrated, and testable without `BackBurner` dependencies.

## Acceptance Criteria

- `BackBurner` is no longer required to understand the first deliverable
- anchor and route contracts are present in compiled code, not only in untracked scaffolding
- one discrete facet path produces anchor-key predicates and a composed filter query
- one facet content path uses the composed anchor set
- tests exist for the route compiler slice and the integrated discrete-facet slice
- the legacy runtime remains the default path until the new slice is proven

## Recommended Delivery Order

1. Decide which current files become authoritative and which remain archival.
2. Fill the empty core query-composer contracts with the minimal interfaces needed for one slice.
3. Wire anchor and route repositories into compiled code.
4. Integrate a discrete facet predicate resolver and composed filter query path.
5. Implement the matching facet content path.
6. Add comparison-style integration tests against the current runtime.
7. Only then extend to range, intersect, and GIS facet types.

## Open Questions

- Which pieces of `BackBurner` should be promoted into compiled code, if any?
- Should route definitions stay in fixtures and code first, or move to database-backed configuration immediately?
- Should the first composed query use `INTERSECT`, `INNER JOIN`, or a compiler abstraction that can emit either?

## Final Recommendation

Treat the next milestone as an implementation handoff, not a migration program.

There is already enough route and anchor exploration to justify one narrow end-to-end slice. The right next move is to compile and validate that slice before adding more architecture.
