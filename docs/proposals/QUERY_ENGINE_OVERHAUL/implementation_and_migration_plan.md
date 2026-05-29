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
- `done`: composed filter query contract and implementation
- `done`: facet content query path on top of the composed anchor set
- `done`: runtime integration for one discrete facet flow
- `done`: comparison-style validation against the legacy path

## Progress Summary

| Phase | Title                              | Status      | Exit Condition                                                                   |
|-------|------------------------------------|-------------|----------------------------------------------------------------------------------|
| 0     | Baseline consolidation             | done        | One active path kept, shelved path archived, minimal resolver contracts promoted |
| 1     | Contract stabilization             | done        | Core and composer contracts are explicit and wired for one discrete slice        |
| 2     | Composed filter query              | done        | Multiple discrete predicates can compose into one anchor-filter query            |
| 3     | Facet content query                | done        | Target facet content can run from the composed anchor set                        |
| 4     | Runtime integration and comparison | done        | One end-to-end request path works and is compared against legacy output          |
| 5     | Expansion gate                     | in progress | The discrete slice is proven and the first range-target expansion is validated   |

## Progress Status Checklist

- [x] Phase 0 is complete: one active implementation path remains and shelved code is archived.
- [x] Phase 1 is complete: the first discrete-slice contracts and DI wiring are stable.
- [x] Phase 2 is complete: multiple discrete predicates compose into one anchor-filter query.
- [x] Phase 3 is complete: target facet content can be generated from the composed anchor set.
- [x] Phase 4 is complete for the supported slice: the composed runtime is wired into `FacetContentService` and compared against legacy output.
- [x] Phase 4 live regression is in place: the grouped PostgreSQL-backed matrix in `FacetLoadService` covers the currently validated URIs.
- [x] Phase 4 widening is active: validated visible-target coverage now includes `sites`, `sample_groups`, `country`, `constructions`, `ecocode`, `feature_type`, `ecocode_system`, `genus`, `species`, `species_author`, `family`, `dataset_methods`, `record_types`, `dataset_provider`, and `relative_age_name`.
- [x] Phase 5 has started: `geochronology:country@1,2,5/geochronology` is now a validated composed-path range-target slice.
- [x] The next widening direction is chosen: treat `geochronology` as the first template for broader range-target support, not as a generic template for every non-discrete facet type.

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

`done`

### Tasks

- [x] Choose the first composition strategy: `INTERSECT`
- [x] Implement a composed filter query builder for multiple anchor-key predicate queries
- [x] Reject incompatible anchor-type mixes explicitly
- [x] Handle the single-facet case without extra composition overhead
- [x] Handle the zero-filter case explicitly and document the expected behavior
- [x] Add unit tests for single-facet, multi-facet, incompatible-anchor, and no-filter scenarios

### Exit Criteria

- more than one discrete predicate can compose into one query
- composition uses one anchor type only
- failure modes are explicit and tested

### Notes

- Keep the composition contract stable even if the SQL strategy changes later.
- Current implementation: `IntersectComposedFilterQueryComposer` in `sead.query.core/QueryComposer/Strategies/`.
- Current contract uses typed `PredicateQueryPlan` inputs instead of raw SQL strings.
- Current tests cover null input, empty filter set, single predicate, multiple predicates, incompatible anchor tables, incompatible anchor-key aliases, and whitespace-only predicate entries.

## Phase 3: Facet Content Query

### Objective

Generate target facet content from the composed anchor set rather than from the legacy global join path.

### Status

`done`

### Tasks

- [x] Define the first target facet content query contract
- [x] Implement a content query builder or service that accepts the composed anchor query as input
- [x] Support one discrete target facet only
- [x] Return the shape needed by the current facet-content consumer
- [x] Add unit or narrow integration tests for content query generation

### Exit Criteria

- one target facet content query runs from the composed anchor set
- the content path is separate from final result-set generation
- the query shape is testable without enabling a full runtime switch

### Notes

- Current implementation target: discrete facets whose target table either matches the composed anchor table or can be reached from it through one routed anchor-to-target path.
- Current implementation: `DiscreteFacetContentQueryComposer` in `sead.query.core/QueryComposer/Strategies/`.
- Current tests cover missing target facet, non-discrete targets, missing anchor-to-target routes, direct target joins, routed target joins, and target facets whose category expression depends on joined target-facet tables.
- Current runtime shape: `ComposedFacetContentService` returns `FacetContent` so the active `FacetContentService` consumer contract stays unchanged.

## Phase 4: Runtime Integration And Comparison

### Objective

Integrate the new discrete slice into one request path while keeping the legacy runtime authoritative.

### Status

`done`

### Tasks

- [x] Identify the narrowest runtime boundary where the new path can be called
- [x] Wire one discrete facet flow from request configuration to the new composer path
- [x] Keep the legacy path as the default or fallback behavior
- [x] Add one integration test using the new path end to end
- [x] Add one comparison test that runs the same scenario against both new and legacy behavior
- [x] Capture known differences explicitly if the outputs cannot match exactly at first

### Notes

- Current runtime boundary: `FacetContentService` delegates to `IComposedFacetContentService` only when the request fits the first discrete slice.
- Current supported composed runtime slice: discrete target facet, one or more prior picked discrete filters, no target-side facet clauses, and predicate facets whose category expression resolves to the source-table key the composed path can derive.

#### Supported Request Shapes

- [x] Discrete target facet with one or more prior picked discrete filters. Example: `result_facet:sites@4/result_facet`.
- [x] Routed visible target reached through explicit anchor-to-target SQL. Examples: `sites:sample_groups@1/sites`, `sample_groups:sites@4/sample_groups`, `country:sites@4/country`.
- [x] Routed visible target that also needs target-facet-local joins. Examples: `constructions:sites@4/constructions`, `ecocode:sites@4/ecocode`.
- [x] Predicate facet with a normal source-table key shape. Examples: `record_types:country@1,2,5/record_types`, `dataset_methods:country@1,2,5/dataset_methods`, `dataset_provider:country@1,2,5/dataset_provider`, `relative_age_name:country@1,2,5/relative_age_name`.
- [x] Predicate facet with same-table enforced clauses preserved in composed SQL. Current live example: `country` contributes `location_type_id=1`.
- [x] Target facet whose join column comes from a simple target-table category expression. Examples: `family`, `dataset_provider`, `relative_age_name`.
- [x] Target facet whose join column must be resolved from a schema-qualified expression on a shortcut or joined table. Current live example: `species:country@1,2,5/species`.

#### Validated Live Matrix Checklist

- [x] `result_facet:sites@4/result_facet`
- [x] `sites:sample_groups@1/sites`
- [x] `sample_groups:sites@4/sample_groups`
- [x] `country:sites@4/country`
- [x] `constructions:sites@4/constructions`
- [x] `ecocode:sites@4/ecocode`
- [x] `sites:country@1,2,5/sites`
- [x] `ecocode:country@1,2,5/ecocode`
- [x] `feature_type:country@1,2,5/feature_type`
- [x] `ecocode_system:country@1,2,5/ecocode_system`
- [x] `genus:country@1,2,5/genus`
- [x] `species:country@1,2,5/species`
- [x] `species_author:country@1,2,5/species_author`
- [x] `family:country@1,2,5/family`
- [x] `dataset_methods:country@1,2,5/dataset_methods`
- [x] `record_types:country@1,2,5/record_types`
- [x] `dataset_provider:country@1,2,5/dataset_provider`
- [x] `relative_age_name:country@1,2,5/relative_age_name`
- [x] `abundance_classification:country@1,2,5/abundance_classification`
- [x] `tbl_biblio_sample_groups:country@1,2,5/tbl_biblio_sample_groups`
- [x] `tbl_biblio_sites:country@1,2,5/tbl_biblio_sites`
- [x] `tbl_biblio_modern:country@1,2,5/tbl_biblio_modern`
- [x] `geochronology:country@1,2,5/geochronology`
- [x] `tbl_denormalized_measured_values_33_0:country@1,2,5/tbl_denormalized_measured_values_33_0`
- [x] `tbl_denormalized_measured_values_33_82:country@1,2,5/tbl_denormalized_measured_values_33_82`
- [x] `tbl_denormalized_measured_values_32:country@1,2,5/tbl_denormalized_measured_values_32`
- [x] `tbl_denormalized_measured_values_37:country@1,2,5/tbl_denormalized_measured_values_37`
- [x] `abundances_all:country@1,2,5/abundances_all`

#### Current Support And Boundaries

- The composed runtime now supports direct aggregate/result targets plus routed visible targets whose category expression is either on the routed target table itself or on joined target-facet tables, including `sites`, `sample_groups`, `country`, `constructions`, `ecocode`, `feature_type`, `ecocode_system`, `genus`, `species`, `species_author`, `family`, `dataset_methods`, `record_types`, `dataset_provider`, `relative_age_name`, `abundance_classification`, `tbl_biblio_sample_groups`, `tbl_biblio_sites`, and `tbl_biblio_modern`, plus the validated phase-5 range targets `geochronology`, `tbl_denormalized_measured_values_33_0`, `tbl_denormalized_measured_values_33_82`, `tbl_denormalized_measured_values_32`, `tbl_denormalized_measured_values_37`, and `abundances_all`.
- The grouped live matrix in `FacetLoadService` now covers the baseline visible-target slices, the supported `country`-predicate slices, four additional validated discrete bibliography/view-backed targets (`abundance_classification`, `tbl_biblio_sample_groups`, `tbl_biblio_sites`, and `tbl_biblio_modern`), and six validated phase-5 range-target slices: `geochronology`, `tbl_denormalized_measured_values_33_0`, `tbl_denormalized_measured_values_33_82`, `tbl_denormalized_measured_values_32`, `tbl_denormalized_measured_values_37`, and `abundances_all`.
- Predicate planning now has unit-validated support for routed source-key overrides when the picked facet's source table uses a placeholder primary key and exposes a simple same-table category column instead.
- Predicate planning now also supports same-table enforced facet clauses on picked discrete facets, including the live `country` clause `countries.location_type_id=1`.
- Target join resolution now also handles schema-qualified category expressions on target shortcut tables, which unblocks `species:country@1,2,5/species` where the target table metadata primary key is a placeholder.
- Discrete target SQL now also carries enforced target-side clauses, which unblocks clause-bearing joined targets such as `tbl_biblio_sample_groups:country@1,2,5/tbl_biblio_sample_groups`.
- The same discrete target-clause pattern also covers the sibling bibliography view target `tbl_biblio_sites:country@1,2,5/tbl_biblio_sites` without additional runtime changes.
- The same bibliography/view-backed target support also covers `tbl_biblio_modern:country@1,2,5/tbl_biblio_modern`, so the alternate aggregate facet and target-table ordering do not require new runtime branching.
- Range target planning now also handles placeholder target primary keys by falling back to the aggregate anchor key when the target rows are keyed on the anchor identity instead of metadata PK placeholders.
- Range target SQL now carries enforced target-side clauses, which unblocks `abundances_all:country@1,2,5/abundances_all` where the target view requires `facet.view_abundance.abundance is not null`.
- Clause-bearing `country` predicates now have live-validated coverage across multiple visible targets, including `sites`, `ecocode`, `feature_type`, `ecocode_system`, `genus`, `species`, `species_author`, `family`, `dataset_methods`, `record_types`, `dataset_provider`, and `relative_age_name`.
- Former phase-4 boundary crossed in phase 5: `geochronology:country@1,2,5/geochronology` now uses the composed path and matches legacy facet content in live validation.
- Remaining unsupported visible facets are those that still need more than a simple target-join column derived from the category-id expression or whose predicate side does not resolve cleanly to a source-table key.
- Phase-4 fixes included a route SQL target-alias off-by-one correction and predicate SQL wrapping so `source_id` filters apply in an outer query block.
- Expansion fixes also included deriving the composed anchor table from the aggregate facet, routing anchor-to-target joins through an explicit table trail before SQL compilation, and allowing routed target joins to override placeholder target primary keys.
- Unsupported requests still fall back to the legacy category-count path.

### Exit Criteria

- one request path uses the new discrete slice end to end
- the new output can be compared against the legacy path on the same dataset and request shape
- the legacy runtime still remains the default path outside the explicitly integrated slice

## Phase 5: Expansion Gate

### Objective

Decide whether the first slice is stable enough to extend to more facet types.

### Status

`in progress`

### Tasks

- [ ] Review unresolved issues from phases 1 through 4
- [ ] Confirm that the discrete slice no longer depends on archived design assumptions
- [x] Decide whether the next facet type is range, intersect, or GIS
- [x] Decide whether out-of-scope requests such as `geochronology:country@1,2,5/geochronology` define the first phase-5 expansion candidate
- [x] Record the reasons for the chosen next facet type
- [ ] Update this plan or split a new follow-up plan for the next slice

### Proposed First Phase-5 Slice

- [x] Keep the predicate side unchanged with `country@1,2,5` as the picked discrete filter.
- [x] Add target-content support for `geochronology` as the first out-of-scope phase-5 candidate.
- [x] Add one focused live comparison for `geochronology:country@1,2,5/geochronology` that passes through the composed path instead of the fallback path.
- [x] Decide whether a passing `geochronology` slice should be treated as the start of broader non-discrete target support or as a one-off target expansion.

### Current Notes

- Current validated phase-5 scenario: `geochronology:country@1,2,5/geochronology` through `IFacetContentService`.
- Current additional validated phase-5 scenario: `tbl_denormalized_measured_values_33_0:country@1,2,5/tbl_denormalized_measured_values_33_0` through `IFacetContentService`.
- Current additional validated phase-5 scenario: `tbl_denormalized_measured_values_33_82:country@1,2,5/tbl_denormalized_measured_values_33_82` through `IFacetContentService`.
- Current additional validated phase-5 scenario: `tbl_denormalized_measured_values_32:country@1,2,5/tbl_denormalized_measured_values_32` through `IFacetContentService`.
- Current additional validated phase-5 scenario: `tbl_denormalized_measured_values_37:country@1,2,5/tbl_denormalized_measured_values_37` through `IFacetContentService`.
- Current additional validated phase-5 scenario: `abundances_all:country@1,2,5/abundances_all` through `IFacetContentService`.
- Current adjacent validated discrete view-backed scenario: `abundance_classification:country@1,2,5/abundance_classification` through `IFacetContentService`.
- Current adjacent validated discrete target-clause scenario: `tbl_biblio_sample_groups:country@1,2,5/tbl_biblio_sample_groups` through `IFacetContentService`.
- Current adjacent validated discrete target-clause scenario: `tbl_biblio_sites:country@1,2,5/tbl_biblio_sites` through `IFacetContentService`.
- Current adjacent validated discrete bibliography scenario: `tbl_biblio_modern:country@1,2,5/tbl_biblio_modern` through `IFacetContentService`.
- The composed path now supports a routed range target (`geochronology`), multiple UDF-backed measured-value range targets (`tbl_denormalized_measured_values_33_0`, `tbl_denormalized_measured_values_33_82`, `tbl_denormalized_measured_values_32`, `tbl_denormalized_measured_values_37`), and a placeholder-PK view-backed range target with enforced target-side clauses (`abundances_all`) while keeping the predicate side on the proven discrete `country@1,2,5` slice.
- After exhausting the currently visible range-target variants in metadata, the next validated boundary is discrete view-backed targets with placeholder metadata or enforced target-side clauses.
- `geochronology` is no longer just a fallback boundary; it is now the first implemented phase-5 expansion candidate.

### Decision

- Treat `geochronology` as the template for the next range-target slices, not as a one-off exception.
- Do not treat it as a generic template for all non-discrete facet types. `Intersect` and `GeoPolygon` targets still need separate contracts and validation.
- The decisive reuse points are now explicit: the composed path can keep the discrete predicate side, derive interval metadata through `IRangeCategoryInfoService`, join routed targets on the target-table primary key, and count distinct composed anchor ids inside interval buckets.
- The second validated slice shows that the same range-target contract also works when the target facet is backed by a UDF keyed directly on `analysis_entity_id`, without requiring a routed `target_route` hop.
- The third validated slice shows that the same UDF-backed measured-value contract also works for `method_values_33_82`, which strengthens the case that the measured-value widening track is alias-reusable rather than fitted only to the first measured-value target.
- The third validated slice shows that this UDF-backed measured-value pattern is not specific to one facet alias; the same contract also works for `method_values_32` without new runtime changes.
- The fourth validated slice shows the same outcome for `method_values_37`, which strengthens the case that the measured-value widening track is a reusable subpattern, not a pair of isolated aliases.
- The fifth validated slice shows the same outcome for `method_values_37`, which strengthens the case that the measured-value widening track is a reusable subpattern, not a pair of isolated aliases.
- The sixth validated slice shows that the range-target contract also extends to a view-backed target whose metadata primary key is a placeholder and whose target-side clause must be preserved in the composed content SQL.
- The next validated post-range boundary shows that discrete view-backed targets can also use the composed path when the join key stays routable on the root target table (`abundance_classification`), when sibling joined target tables contribute enforced target-side clauses (`tbl_biblio_sample_groups`, `tbl_biblio_sites`), and when the same bibliography target family routes through a different aggregate helper (`tbl_biblio_modern`).
- This makes `Range` the intentional next facet-type track for phase 5, while keeping the support boundary narrower than “all non-discrete targets”.

### Exit Criteria

- the discrete slice is compiled, integrated, and testable
- the next scope is chosen intentionally rather than by leftover prototype code

## Immediate Next Actions

These are the next actions to take unless a blocker appears:

1. Pick the next range target adjacent to the same `country@1,2,5` predicate side and validate whether it behaves like the routed `geochronology` slice or the now-established UDF-backed measured-value subpattern.
2. If the next range target needs different interval semantics or target-join resolution, document that difference explicitly before widening further.
3. Keep `Intersect` and `GeoPolygon` targets outside this widening track until they get their own phase-5 entry criteria.

## Decision Log

Record implementation decisions here as they are made.

- `done`: keep the active route-compiler path and archive `BackBurner`
- `done`: promote only small, useful contracts from the shelved design
- `done`: use `source_id` as the facet-source key alias and `target_id` as the anchor-key alias for the first vertical slice
- `done`: use `INTERSECT` as the first composed-filter strategy
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
