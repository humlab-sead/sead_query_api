# Implementation Plan

## Summary

This document is the high-level implementation plan for the query-engine overhaul.

It complements `TASK_PLAN_PHASE_0.md`. That document tracks the detailed phase-0 vertical-slice work and the widening already underway. This document defines the broader phase plan needed to reach the end state: a query engine that is feature-wise on par with the legacy runtime.

## Problem

The branch now proves the architecture direction, but it does not yet deliver full runtime parity.

The remaining gap is no longer “can the composed model work at all?” The remaining gap is breadth and completion:

- all legacy facet families are not yet supported on the composed path
- all legacy target and result shapes are not yet produced from the new composer
- the legacy runtime is still authoritative outside the supported slice

The implementation plan therefore needs to move from one validated vertical slice to complete runtime replacement.

## Scope

This plan covers the backend implementation phases needed to move from the current validated slice to practical feature parity with the legacy query engine.

It includes:

- facet-family completion
- target-content parity
- result-set parity
- route and configuration hardening
- runtime cutover preparation

It does not include frontend rollout, staffing, or release scheduling.

## Current Position

The current branch state can be summarized as follows:

- the composed runtime is integrated for one proven vertical slice and already widened across multiple discrete target families and initial range-target support
- composed facet content is validated for an expanding set of discrete and range targets
- widening is active through focused live probes and grouped regression coverage
- result-set generation is not yet migrated to the new composer
- full feature parity with the legacy runtime is not yet reached

## Phase Plan

### Phase 0: Prove One Vertical Slice

This phase is the initial composed-runtime slice tracked in `TASK_PLAN_PHASE_0.md`. It establishes the first integrated path that the later parity phases widen.

**Goal**

Deliver one end-to-end discrete-facet path that proves the route-based, anchor-centered composer in compiled runtime code.

**Focus**

- stabilize the minimal route, anchor, resolver, and composed-query contracts needed for one discrete facet flow
- compose anchor-key predicates into one filtered anchor set
- generate one target facet's content from that composed anchor set
- integrate the slice at one narrow runtime boundary while keeping the legacy runtime authoritative elsewhere
- validate the slice through unit coverage, focused live comparison, and grouped regression promotion

**Acceptance Criteria**

- one compiled discrete-facet path runs end to end from route resolution to facet content generation on the composed path
- composed output for the supported slice matches legacy behavior for the validated scenarios
- grouped regression exists for the promoted slice and serves as the base for further widening
- the detailed execution tracker for this phase is maintained in `TASK_PLAN_PHASE_0.md`

### Phase 1: Contract And Coverage Baseline

**Goal**

Lock down the contracts that the rest of the migration depends on and establish a parity inventory against the legacy engine.

**Focus**

- stabilize anchor, route, and facet-resolver contracts
- keep the current composed slice reliable while widening continues
- create and maintain a parity inventory of legacy capabilities, grouped by facet family and result shape
- make unsupported requests explicit instead of ambiguous

**Acceptance Criteria**

- route, anchor, and composed-query contracts are documented and reflected in compiled code
- the branch has one maintained parity inventory for legacy facet families and result paths
- grouped regression covers all currently supported composed slices
- unsupported composed requests fail or fall back explicitly

### Phase 2: Discrete Facet Parity

**Goal**

Reach practical parity for legacy discrete facet content generation.

**Focus**

- widen the composed path across the remaining discrete target families
- cover direct targets, routed targets, view-backed targets, clause-bearing targets, and multi-table targets
- keep validating through focused live probes first, then grouped promotion

**Acceptance Criteria**

- all in-scope legacy discrete target facets either run on the composed path or are listed on an explicit exception list with a concrete blocker
- discrete facet content produced by the composed path matches legacy output for the validated matrix
- no currently validated discrete slice regresses during widening

### Phase 3: Non-Discrete Facet Parity

**Goal**

Extend the composed path to the remaining non-discrete facet families needed for legacy parity.

**Focus**

- complete range-target coverage beyond the currently validated targets
- implement and validate intersect-facet support under the same anchor contract
- implement and validate GIS polygon support under the same anchor contract
- keep facet-type-specific behavior inside resolver or composer boundaries

**Acceptance Criteria**

- legacy range behavior needed by the active API is supported on the composed path
- intersect and GIS polygon requests have compiled contracts, runtime validation, and regression coverage
- the composed runtime supports the legacy facet families required by the API, not just the first validated subset

### Phase 4: Result-Set Parity

**Goal**

Move final result generation onto the same composed anchor model so filtering and result projection are feature-wise on par with the legacy engine.

**Focus**

- define the handoff from composed anchor sets to result projections
- migrate the key result-set formats used by the API
- validate result payloads against legacy behavior where equivalence is required
- preserve the separation between filtering, facet content generation, and final result projection

**Acceptance Criteria**

- the main result-set formats used by the API can be generated from composed anchor sets
- result generation on the composed path matches legacy behavior for validated scenarios
- the runtime no longer depends on legacy filtering internals to generate equivalent results for supported requests

### Phase 5: Configuration And Operational Hardening

**Goal**

Make the composed engine robust enough to be the default runtime path.

**Focus**

- settle route-definition governance and validation rules
- harden configuration errors and diagnostics
- confirm performance on representative live query shapes
- close the gap between branch-only behavior and maintainable long-term runtime behavior

**Acceptance Criteria**

- route definitions and anchor mappings have a maintained source of truth
- configuration failures surface as clear diagnostics
- representative composed queries meet acceptable runtime behavior for interactive use
- durable docs describe the active architecture and the supported boundaries accurately

### Phase 6: Cutover And Legacy Retirement

**Goal**

Make the composed query engine the authoritative backend path and retire the legacy engine for feature-equivalent scenarios.

**Focus**

- choose and implement the final cutover path
- reduce legacy fallback coverage as parity closes
- retire obsolete query-building paths and outdated proposal-era assumptions
- keep one explicit list of remaining exceptions, if any

**Acceptance Criteria**

- the composed runtime is the default path for feature-equivalent requests
- the legacy engine is no longer required for the supported API surface
- any remaining legacy-only cases are documented as explicit exceptions rather than accidental gaps
- the system is feature-wise on par with the legacy query engine for the intended runtime scope

## Cross-Phase Rules

These rules apply across all phases.

- prefer incremental widening over large unvalidated rewrites
- validate one focused slice before grouped promotion
- keep the legacy runtime authoritative only where parity is not yet proven
- keep branch status explicit in docs; do not describe planned support as shipped support
- use parity with the legacy engine as the delivery measure, not only internal architectural elegance

## Validation Strategy

Validation should remain layered.

- unit tests for route parsing, route resolution, and resolver behavior
- focused live tests for each new discriminator slice
- grouped regression for promoted composed slices
- legacy comparison tests for both facet content and result generation

## Final Recommendation

Treat the current branch as the midpoint between phase-0 proof and runtime replacement.

The right implementation plan is to build from the proven phase-0 vertical slice and then close parity gaps explicitly: discrete parity first, then non-discrete parity, then result-set parity, then cutover. The end state is not merely a cleaner design. It is a composed query engine that can replace the legacy engine without feature loss for the intended API scope.