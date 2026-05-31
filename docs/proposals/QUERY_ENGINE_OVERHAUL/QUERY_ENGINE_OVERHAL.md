# Query Engine Overhaul

## Summary

This change request covers the full query-engine overhaul, not only the first discrete-facet slice.

The problem is structural: the current query runtime derives one global join shape from the active facet chain. That model is hard to extend, hard to debug, and brittle when the same tables must be reused through different semantic routes.

The recommended direction is to move the query engine to anchor-based composition with explicit routes, facet-type-specific predicate resolvers, and facet content queries that reuse a composed anchor set.

The branch now proves that this direction is viable. One compiled, tested vertical slice is integrated into the runtime, compared against legacy behavior, and widened across an initial set of range and adjacent discrete targets. The overhaul is therefore past the proposal-only stage, but it is not complete.

## Problem

The current query pipeline has four practical limits.

- It flattens active facets into one join-heavy query shape.
- It relies on implicit graph traversal to discover joins.
- It couples filtering, facet content generation, and final result generation too tightly.
- It pushes facet-specific behavior into shared query-builder logic and workaround views.

These limits raise the cost of adding or changing facet behavior.

They are most visible when:

- the same table must be joined through different semantic paths
- target facet content depends on more than one routed or joined table
- a facet needs its own operator semantics, such as range or GIS behavior
- the system must explain or debug generated SQL

## Why This Still Matters

The overhaul is still justified because the current runtime still carries the same core design pressure:

- global join discovery remains the dominant composition model
- route semantics are harder to express than they should be
- facet filtering and result projection are still more coupled than they need to be

What changed is branch maturity.

- Route parsing, route resolution, and route SQL generation are no longer only ideas.
- The first compiled vertical slice is implemented and live-validated.
- The remaining work is not to prove the direction from scratch. It is to carry the same contract through the rest of the facet families and result paths.

## Scope

This change request covers the full backend query-engine overhaul for faceted filtering and facet content generation.

It includes:

- explicit anchor-based composition contracts
- explicit routes from facet sources to anchors
- facet-type-specific predicate resolution
- composed filter-query generation
- facet content generation from a composed anchor set
- incremental runtime integration and widening
- durable architecture and requirements documentation for the resulting system

## Non-Goals

This change request does not cover:

- a frontend redesign
- a full operational rollout plan
- a feature-flag or release-management design in this document
- staffing or scheduling
- a claim that the overhaul is already complete across all facet types

## Branch Status

The current branch should be described as an in-progress architectural migration with a proven first slice.

What is already proven:

- route parsing and route compilation are active implementation paths
- anchor-aware contracts exist in compiled code
- composed filter query generation is implemented for the first slice
- composed facet content generation is implemented for the first slice
- runtime integration into `FacetContentService` is in place for the supported slice
- comparison-style validation against the legacy runtime is in place
- widening beyond the original discrete slice is underway and live-validated

What is not complete:

- full coverage across all discrete targets
- full range, intersect, and GIS completion under one settled contract
- complete result-set generation on top of the new composer
- final route-definition governance and cutover plan

The right current reading is:

- direction proved
- first vertical slice shipped inside the branch runtime
- expansion in progress
- full migration still pending

## Proposed Design

### Composition Contract

Every active filtering facet compiles to a query that returns distinct anchor keys for one anchor type.

The composed filter query combines those anchor-key queries into one filtered anchor set.

`INTERSECT` is the first proven strategy, but the architectural requirement is the contract, not one exact SQL operator.

### Anchor Model

An anchor is the entity identity shared across the composed query.

Examples include:

- site
- physical sample
- analysis entity

One composed request uses exactly one anchor type.

Facets may support more than one anchor type, but only through explicit configuration and explicit route or predicate logic.

### Route Model

Routes define how a facet source reaches an anchor.

The overhaul keeps route handling explicit:

- arrow-based route syntax for readable route definitions
- route parsing and expansion
- route resolution against known table relationships
- route SQL generation from explicit table trails

This replaces repeated, implicit join discovery with named traversal rules.

### Facet-Type Resolver Model

Facet behavior stays split by facet type.

The long-term model is resolver-based:

- discrete facet predicate resolution
- range facet predicate resolution
- intersect facet predicate resolution
- GIS polygon facet predicate resolution

The important design rule is that facet-specific logic stays inside facet-type-specific resolvers or composers, not in controllers and not in one global query-builder class.

### Facet Content Queries

Facet content generation uses the composed anchor set from all other active facets, then aggregates values for the target facet.

This keeps filtering and content generation separate while still sharing the same anchor contract.

For the first implemented slice, this has already been proven for discrete targets and then widened into adjacent range and routed discrete targets.

### Result Generation

The composed anchor set should become the handoff point for final result generation as well.

That work is not yet complete, but the design direction is clear: filtering should produce a stable anchor set first, and result projection should happen afterward.

## Implementation Strategy

The recommended implementation strategy remains incremental.

1. Stabilize the route and anchor contracts already in compiled code.
2. Keep the legacy runtime authoritative outside the supported composed slice.
3. Continue widening through focused live probes first, then grouped regression coverage.
4. Extend the same contract across remaining discrete families, then deeper range, intersect, and GIS work.
5. Move result-set generation onto the same composed anchor model after the facet-content path is broad enough.

This branch has already validated the first three steps.

## Current Validation Position

The branch no longer needs a proposal that assumes runtime integration is still hypothetical.

The current validated position is:

- the original discrete-facet vertical slice is complete
- the expansion gate is active
- range support is already validated for multiple targets
- adjacent discrete widening is already validated for several target families
- widening is being managed through focused live probes and grouped regression promotion

That means the master change request should now act as the top-level document, while the implementation plan remains the execution tracker for the widening work.

## Risks And Tradeoffs

- Explicit routes reduce ambiguity, but route quality becomes a hard dependency.
- Anchor-based composition is easier to reason about, but mixed-anchor requests must fail explicitly instead of being guessed through fallback joins.
- Resolver-based behavior is more maintainable, but it creates more architectural pieces that must stay coherent.
- Incremental integration reduces migration risk, but it leaves the system in a hybrid state for longer.

## Validation And Acceptance

The overhaul should be accepted in layers, not as a one-shot rewrite.

Acceptance criteria for the overall change request are:

- route, anchor, and resolver contracts are stable and documented
- the composed runtime supports the intended facet families without depending on archived `BackBurner` code
- facet content generation consistently runs from composed anchor sets for supported facets
- result generation can reuse the same composed anchor contract
- focused and grouped regression coverage exist for supported runtime slices
- durable architecture and requirements documents describe the current design truth and the intended destination without overstating delivery status

## Document Map

Use the documents in this order.

- `QUERY_ENGINE_OVERHAL.md`: top-level change request and bird's-eye view
- `TASK_PLAN_PHASE_0.md`: phase-0 execution tracker and widening log
- `docs/REQUIREMENTS.md`: durable system requirements for the active architecture direction
- `archive/system_requirements_specification.md`: archived proposal-era technical requirements source material
- `docs/DESIGN.md`: durable architecture and runtime-structure documentation

The older problem statement and rationale material has been consolidated into this master CR.

## Final Recommendation

Treat the query-engine overhaul as an active architectural migration with one proven runtime slice and a widening program already in motion.

Keep the master CR broad, but keep execution incremental.

The right next work is not to reopen the architecture debate from first principles. It is to keep widening and documenting the route-based, anchor-centered composer until it can replace the legacy runtime path with confidence.