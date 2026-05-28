# Implementation Plan: Anchor-Based Query Composer Vertical Slice

## Status

- Document type: working implementation plan
- Scope: one compiled, tested, discrete-facet vertical slice for the query overhaul
- Tracking mode: update this file in place as work moves from not started to in progress to done

## How To Use This Plan

This document is for execution tracking, not for proposal review.

Update these items as work progresses:

- phase status
- checklist items
- notes on blockers or decisions
- evidence links to tests, commits, or merged code

Use these status labels consistently:

- `not started`
- `in progress`
- `blocked`
- `done`

## Goal

Deliver one end-to-end discrete facet path that uses the new route and anchor model for:

- route resolution
- anchor-key predicate SQL
- composed filter query generation
- facet content generation from the composed anchor set

The legacy runtime remains the default path until this slice is integrated and proven.

## Current Baseline

The branch already contains useful groundwork.

### Completed Baseline Work

- `done`: route parsing exists in `sead.query.composer/QueryComposer/RouteCompiler/ArrowRouteParser.cs`
- `done`: route resolution and route SQL compilation exist in `sead.query.composer/QueryComposer/RouteCompiler/`
- `done`: route compiler unit tests exist in `sead.query.test/UnitTests/QueryComposer/RouteCompiler/`
- `done`: anchor and route entity scaffolding exists in `sead.query.core/Model/Entities/`
- `done`: anchor and route repositories exist in `sead.query.infra/Repository/`
- `done`: `BackBurner` has been moved to archival locations and is no longer the active path
- `done`: minimal active-path contracts promoted so far include `AnchorTemplate`, `DiscreteFacetUserInput`, and `DiscreteFacetPredicateResolver`

### Remaining Gaps

- `done`: stable core query-composer interfaces for the new slice
- `not started`: composed filter query contract and implementation
- `not started`: facet content query path on top of the composed anchor set
- `not started`: runtime integration for one discrete facet flow
- `not started`: comparison-style validation against the legacy path

## Progress Summary

| Phase | Title                              | Status      | Exit Condition                                                                   |
|-------|------------------------------------|-------------|----------------------------------------------------------------------------------|
| 0     | Baseline consolidation             | done        | One active path kept, shelved path archived, minimal resolver contracts promoted |
| 1     | Contract stabilization             | done        | Core and composer contracts are explicit and wired for one discrete slice        |
| 2     | Composed filter query              | not started | Multiple discrete predicates can compose into one anchor-filter query            |
| 3     | Facet content query                | not started | Target facet content can run from the composed anchor set                        |
| 4     | Runtime integration and comparison | not started | One end-to-end request path works and is compared against legacy output          |
| 5     | Expansion gate                     | not started | The discrete slice is proven and ready for extension to more facet types         |

## Phase 0: Baseline Consolidation

### Objective

Reduce the branch to one active implementation path and preserve only the useful foundation for the first vertical slice.

### Status

`done`

### Completed Items

- [x] Keep `sead.query.composer/QueryComposer/RouteCompiler/` as the active implementation base
- [x] Move shelved `BackBurner` files into archival locations
- [x] Move shelved `BackBurner` tests out of the active test path
- [x] Promote `DiscreteFacetUserInput` into compiled code
- [x] Promote `AnchorTemplate` into the active route-compiler path
- [x] Promote a first active `DiscreteFacetPredicateResolver`
- [x] Add focused unit tests for the promoted discrete resolver

### Evidence

- active route compiler code under `sead.query.composer/QueryComposer/RouteCompiler/`
- archived redesign code under `sead.query.composer/Archived/BackBurner/`
- focused discrete resolver tests under `sead.query.test/UnitTests/QueryComposer/RouteCompiler/`

## Phase 1: Contract Stabilization

### Objective

Define the smallest compiled contracts needed to support one discrete-facet path without reviving the old plugin architecture.

### Status

`done`

### Tasks

- [x] Decide that shared query-composer contracts live in `sead.query.core/QueryComposer/`, while route compilation and discrete resolver implementations stay in `sead.query.composer`
- [x] Replace the empty `sead.query.core/QueryComposer/` surface with the first minimal interfaces and query-plan contracts for the discrete slice
- [x] Define the contract for a composed filter query result
- [x] Define the contract for a facet content query result that consumes the composed anchor set
- [x] Decide and document the single anchor-key naming convention for the first slice
- [x] Add DI registration for the active route compiler and discrete predicate resolver path

### Exit Criteria

- compiled interfaces exist for the first slice
- the route compiler and discrete predicate resolver can be resolved through DI
- there is no need to consult archived `BackBurner` files to understand the first slice

### Notes

- Keep contracts small.
- Do not define range, intersect, or GIS abstractions yet unless the discrete slice requires them.
- Initial core contracts added: `ComposedFilterQuery`, `IComposedFilterQueryComposer`, `FacetContentQueryPlan`, and `IFacetContentQueryComposer`.
- First-slice naming convention: predicate queries expose `source_id` for the facet-source key and `target_id` for the anchor key.
- Active DI wiring now resolves `IRouteRepository`, `IRouteGraphFactory`, `IRouteResolver`, `IArrowRouteParser`, `IRouteSqlCompiler`, and `IDiscreteFacetPredicateResolver`.

## Phase 2: Composed Filter Query

### Objective

Compose multiple discrete facet predicates into one anchor-filter query for a single anchor type.

### Status

`in progress`

### Tasks

- [x] Choose the first composition strategy: `INTERSECT`
- [x] Implement a composed filter query builder for multiple anchor-key predicate queries
- [ ] Reject incompatible anchor-type mixes explicitly
- [x] Handle the single-facet case without extra composition overhead
- [x] Handle the zero-filter case explicitly and document the expected behavior
- [ ] Add unit tests for single-facet, multi-facet, incompatible-anchor, and no-filter scenarios

### Exit Criteria

- more than one discrete predicate can compose into one query
- composition uses one anchor type only
- failure modes are explicit and tested

### Notes

- Keep the composition contract stable even if the SQL strategy changes later.
- Current implementation: `IntersectComposedFilterQueryComposer` in `sead.query.core/QueryComposer/Strategies/`.
- Current tests cover null input, empty filter set, single predicate, multiple predicates, and whitespace-only predicate entries.
- Explicit incompatible-anchor validation is still pending.

## Phase 3: Facet Content Query

### Objective

Generate target facet content from the composed anchor set rather than from the legacy global join path.

### Status

`not started`

### Tasks

- [ ] Define the first target facet content query contract
- [ ] Implement a content query builder or service that accepts the composed anchor query as input
- [ ] Support one discrete target facet only
- [ ] Return the shape needed by the current facet-content consumer
- [ ] Add unit or narrow integration tests for content query generation

### Exit Criteria

- one target facet content query runs from the composed anchor set
- the content path is separate from final result-set generation
- the query shape is testable without enabling a full runtime switch

## Phase 4: Runtime Integration And Comparison

### Objective

Integrate the new discrete slice into one request path while keeping the legacy runtime authoritative.

### Status

`not started`

### Tasks

- [ ] Identify the narrowest runtime boundary where the new path can be called
- [ ] Wire one discrete facet flow from request configuration to the new composer path
- [ ] Keep the legacy path as the default or fallback behavior
- [ ] Add one integration test using the new path end to end
- [ ] Add one comparison test that runs the same scenario against both new and legacy behavior
- [ ] Capture known differences explicitly if the outputs cannot match exactly at first

### Exit Criteria

- one request path uses the new discrete slice end to end
- the new output can be compared against the legacy path on the same dataset and request shape
- the legacy runtime still remains the default path outside the explicitly integrated slice

## Phase 5: Expansion Gate

### Objective

Decide whether the first slice is stable enough to extend to more facet types.

### Status

`not started`

### Tasks

- [ ] Review unresolved issues from phases 1 through 4
- [ ] Confirm that the discrete slice no longer depends on archived design assumptions
- [ ] Decide whether the next facet type is range, intersect, or GIS
- [ ] Record the reasons for the chosen next facet type
- [ ] Update this plan or split a new follow-up plan for the next slice

### Exit Criteria

- the discrete slice is compiled, integrated, and testable
- the next scope is chosen intentionally rather than by leftover prototype code

## Immediate Next Actions

These are the next actions to take unless a blocker appears:

1. Implement the composed filter query for one anchor type.
2. Add tests for single-facet, multi-facet, incompatible-anchor, and no-filter cases.
3. Choose the first composition strategy: `INTERSECT` or `INNER JOIN`.
4. Implement one target facet content path from the composed anchor set.
5. Identify the narrowest runtime boundary for the first end-to-end integration.

## Decision Log

Record implementation decisions here as they are made.

- `done`: keep the active route-compiler path and archive `BackBurner`
- `done`: promote only small, useful contracts from the shelved design
- `done`: use `source_id` as the facet-source key alias and `target_id` as the anchor-key alias for the first vertical slice
- `open`: decide whether the first composed filter query uses `INTERSECT` or `INNER JOIN`
- `open`: decide when route definitions move from code or fixtures to database-backed configuration
- `open`: decide which stable contracts move to `sead.query.core` after the first slice settles

## Completion Definition

This plan is complete when all of the following are true:

- one discrete facet path can resolve routes and produce anchor-key predicate SQL
- multiple discrete predicates can compose into one anchor-filter query
- one target facet content query can run from the composed anchor set
- one runtime path can execute the new slice end to end
- targeted unit and integration tests prove the slice without relying on archived `BackBurner` code
- the legacy runtime remains the default path outside the explicitly integrated slice
