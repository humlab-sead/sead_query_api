# Implementation Plan

## Summary

This document is the execution-sequencing plan for the inline SQL facet template change request.

The decision is already made at the proposal level: inline SQL becomes the default facet authoring model, the approved anchor strategy is base-anchor template plus route projection by default with explicit anchor-to-SQL mappings for exceptions, and result-shape entries such as `result_facet`, `map_result`, and `result_datasets` stay in the facet catalog through a constrained `template_key` path.

The most important near-term goal is feature implementation, not bulk migration. The phase plan therefore uses a small set of driver facets with distinct peculiarities to prove the contracts before any broad YAML backfill begins.

## Problem

The repository already has the outline of runtime support for explicit SQL through the current `ExplicitSql` path, but the maintained YAML authoring model, importer contract, and compiler integration do not yet support inline SQL as a first-class path.

Without that work:

- the maintained source of truth still cannot represent many real facets cleanly
- result-shape facets still lack a governed inner-template mechanism
- manual backfill work would start before the feature contract is stable
- known problematic facets such as `family` would remain hard to reason about because the runtime still reconstructs intent from relational fragments instead of facet-owned SQL

## Scope

This plan covers the backend implementation phases needed to ship inline SQL facet templates as an authoring and runtime feature.

It includes:

- authoring contract changes in YAML and schema validation
- importer and runtime-storage support for inline templates
- compiler integration for inline template execution
- retained result-shape facet support through a constrained `template_key`
- proof slices using selected driver facets
- limited, controlled backfill only after the feature contract is stable

It does not include broad catalog migration, frontend changes, or release scheduling.

## Current Position

- `sead.query.composer/Templates/route_v1.yaml` is the maintained authoring surface and imports materialize one active runtime copy in the `facet` schema
- the current importer still rejects inline SQL overrides instead of treating them as supported authoring
- the route compiler already has an `ExplicitSql` precedent, so the runtime path is not starting from zero
- the approved baseline is one base-anchor template plus projected anchors by default, with explicit anchor-to-SQL mappings allowed for exceptions
- `result_facet`, `map_result`, and `result_datasets` should remain facets and drive a constrained `template_key` path for inner CTE composition SQL
- broad facet migration is secondary to enabling and validating the feature itself

## Phase Plan

### Phase 1: Authoring Contract And Runtime Plumbing

**Goal**

Make inline SQL a first-class authored and imported configuration path without breaking existing non-template facets.

**Focus**

- extend the YAML and schema contract for inline SQL, base anchors, and explicit anchor-to-SQL exception mappings
- add importer validation and runtime persistence for template metadata
- define the constrained `template_key` contract for retained result-shape facets
- wire runtime loading so facet compilers can consume imported template metadata while unchanged facets continue to use the existing path

**Acceptance Criteria**

- the maintained YAML contract supports inline SQL as a first-class facet authoring path
- the importer validates and persists inline template metadata instead of rejecting it
- runtime services can load imported template metadata without forcing immediate migration of all existing facets
- the detailed execution tracker for this phase is maintained in `TASK_PLAN_PHASE_1.md`

### Phase 2: Discrete Driver Slices

**Goal**

Prove the inline SQL path on discrete facets and retained result-shape facets using representative implementation-driving slices.

**Focus**

- use `result_facet` as the primary retained result-shape driver for the new `template_key` path
- use `family` as the routed discrete fan-out driver because it is already a known parity-risk slice
- use `tbl_biblio_sample_groups` as the clause-bearing joined discrete driver for facet-owned SQL replacing indirect clause reconstruction
- keep relational fallback explicit for facets not yet moved to inline SQL

**Acceptance Criteria**

- the discrete compiler can execute inline-template facets for one same-table result-shape slice, one routed discrete slice, and one clause-bearing joined discrete slice
- the `template_key` path is proven on `result_facet` and is reusable for `map_result` and `result_datasets`
- at least one known risky routed discrete slice is either parity-clean on the inline path or explicitly documented as blocked by a still-open semantic issue

### Phase 3: Non-Discrete Driver Slices

**Goal**

Extend the inline SQL contract to non-discrete families and restore composed facet-content support for `intersect` and `geopolygon` in this CR.

**Focus**

- use `geochronology` as the routed range driver for base-anchor template plus projection
- use `tbl_denormalized_measured_values_33_0` as the denormalized or function-backed range driver
- restore composed facet-content handling for `analysis_entity_ages` as the intersect driver
- restore composed facet-content handling for `sites_polygon` as the geopolygon driver
- keep unsupported placeholder or output-shape combinations explicit

**Acceptance Criteria**

- the non-discrete compiler path supports at least one routed range slice and one denormalized or function-backed range slice through the inline-template contract
- composed facet-content handling for `intersect` targets is restored and validated
- composed facet-content handling for `geopolygon` targets is restored and validated
- placeholder validation and output-shape validation exist for the implemented non-discrete driver slices
- any remaining follow-up is scoped to optimization or migration breadth, not restoration of these facet types

### Phase 4: Hardening And Controlled Backfill

**Goal**

Stabilize the feature contract and begin small, manual backfill batches without turning the feature rollout into a bulk migration effort.

**Focus**

- harden diagnostics, importer validation, and authoring guidance
- move `map_result` and `result_datasets` onto the retained result-shape template-key path after `result_facet` proves the contract
- use the classification CR to choose only small, representative manual backfill batches
- keep helper-only facets and unresolved exceptions on explicit lists

**Acceptance Criteria**

- the inline-template feature has focused validation across the approved driver shapes
- retained result-shape facets stay in the facet catalog through the constrained `template_key` path
- manual backfill can proceed in small batches without reopening the core feature design
- helper-only or deferred facets remain explicit rather than being silently absorbed into the first rollout

## Cross-Phase Rules

- implement the feature contract before attempting broad facet migration
- use driver facets for distinct behavior shapes, not for migration volume
- keep relational fallback explicit for facets not yet converted to inline SQL
- validate one focused slice before widening to adjacent facets of the same shape
- keep result-shape facets in the facet catalog; do not introduce a second authoring surface for them during this effort
- treat explicit anchor-to-SQL mappings as exceptions, not the default authoring pattern

## Validation Strategy

- authoring validation for the YAML schema, placeholder rules, anchor references, and explicit anchor-to-SQL exception shape
- importer validation for persistence and semantic failures before runtime activation
- focused compiler tests for each implemented driver shape
- focused live or parity-style validation for `result_facet`, `family`, `tbl_biblio_sample_groups`, `geochronology`, `tbl_denormalized_measured_values_33_0`, `analysis_entity_ages`, and `sites_polygon`
- grouped regression only after a focused driver slice is clean enough to promote

## Final Recommendation

Start with Phase 1 and keep it narrow: authoring contract, importer persistence, runtime loading, and the constrained `template_key` contract for retained result-shape facets.

Use these driver facets to shape the implementation sequence:

- `result_facet` for same-table result-shape behavior and `template_key`
- `family` for routed discrete fan-out risk
- `tbl_biblio_sample_groups` for clause-bearing joined discrete behavior
- `geochronology` for routed range behavior
- `tbl_denormalized_measured_values_33_0` for denormalized or function-backed range behavior
- `analysis_entity_ages` for intersect restoration
- `sites_polygon` for geopolygon restoration

That gives the feature a credible implementation path without letting manual migration overtake the core runtime contract work.