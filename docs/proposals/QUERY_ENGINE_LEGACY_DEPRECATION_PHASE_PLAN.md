# Query Engine Legacy Deprecation Phase Plan

## Summary

This document is the implementation-sequencing plan for deprecating the live legacy query runtime.
It complements [QUERY_ENGIVE_LEGACY_DEPRECATION.md](QUERY_ENGIVE_LEGACY_DEPRECATION.md), which is the decision document.

The target end state is a composer-only runtime for supported facet-content and result requests.
To reach that state safely, the work needs an ordered path: inventory what is still live, close the remaining parity gaps, remove runtime fallbacks, remove retained implementation surfaces, and then remove or archive any still-authoritative legacy SQL assets.

## Problem

The current branch still runs a hybrid model.
Composer is the preferred runtime for the validated surface, but legacy execution still matters through explicit fallback seams and through hybrid helper services that still depend on legacy query-building.

That leaves two kinds of deprecation work.
One is behavioral: supported requests must no longer depend on legacy fallback.
The other is structural: legacy classes, registrations, and SQL assets must stop being authoritative.

## Scope

This plan covers the backend deprecation sequence needed to remove the live legacy query runtime from the supported API surface.

It includes:

- runtime fallback removal for facet-content and result loading
- migration or removal of hybrid helper services that still depend on legacy query-building
- removal of legacy DI registrations and retained runtime classes once cutover is proven
- inventory and disposition of still-authoritative legacy SQL assets

It does not include frontend changes, release scheduling, or unrelated cleanup of already archived historical material.

## Current Position

- `FacetContentService` still falls back to `CategoryCountService` when the composed facet-content path cannot handle a request.
- `ComposedResultProjectionHandoffBuilder` still falls back to `LegacyResultProjectionHandoffBuilder` for unsupported result requests.
- `QuerySetupBuilder` still underpins the legacy runtime and some hybrid supporting services.
- `BogusPickService` and `CategoryInfoService` still use legacy query-building logic.
- `scripts/prepare-phase5-facet-runtime-schema.sql` is still documented as an active operational schema-prep asset.
- imported `sql_override` data is still designed to land in `facet.facet_template`, so runtime SQL templates remain part of the inventory surface.
- the `deprecated/` SQL files are expected to be historical, but they should still be explicitly classified during inventory rather than assumed safe to ignore.

## Phase Plan

### Phase 1: Inventory Live Legacy Surfaces

**Goal**

Establish the exact set of legacy runtime and SQL assets that still matter to the supported system.

**Focus**

- inventory the live fallback and hybrid code paths centered on `FacetContentService`, `CategoryCountService`, `QuerySetupBuilder`, `LegacyResultProjectionHandoffBuilder`, `ComposedResultProjectionHandoffBuilder`, `BogusPickService`, and `CategoryInfoService`
- inventory legacy DI surfaces, including facet-type plugin registrations that still participate in authoritative execution
- inventory SQL assets that are still operationally relevant, including `scripts/prepare-phase5-facet-runtime-schema.sql`, imported `sql_override` content written to `facet.facet_template`, and the SQL files under `deprecated/`
- classify each inventoried item as live, transitional, archived, or dead

**Acceptance Criteria**

- the repository has a reviewed inventory of live legacy runtime classes, registrations, and helper services
- the repository has a reviewed inventory of still-authoritative or still-ambiguous SQL assets
- each inventoried item has a recorded disposition: migrate, remove, archive, or keep temporarily with an explicit reason
- the detailed execution tracker for this phase is maintained in `docs/proposals/TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1.md`

### Phase 2: Close Supported-Surface Parity Gaps

**Goal**

Remove the functional need for legacy fallback on the supported request surface.

**Focus**

- migrate or explicitly retire the remaining request families that still depend on legacy fallback
- replace hybrid helper behavior that still requires `QuerySetupBuilder` in request-time execution
- keep parity validation in place while both implementations still exist

**Acceptance Criteria**

- each supported request family either runs on the composed path or is explicitly removed from the supported surface
- no supported request relies on legacy fallback as a functional dependency
- focused validation proves composed-path behavior for the supported matrix

### Phase 3: Remove Runtime Fallback Branches

**Goal**

Make composer the only live request-execution path.

**Focus**

- remove facet-content fallback from `FacetContentService`
- remove legacy delegation from the result handoff boundary
- convert unsupported requests into explicit failures or clearly unsupported contracts rather than silent legacy routing

**Acceptance Criteria**

- facet-content requests no longer drop to `CategoryCountService` from the supported runtime path
- result requests no longer drop to `LegacyResultProjectionHandoffBuilder` from the supported runtime path
- diagnostics and tests confirm one authoritative execution model

### Phase 4: Remove Retained Legacy Implementation Surfaces

**Goal**

Delete legacy runtime code that is no longer needed after cutover.

**Focus**

- remove obsolete query-builder services and plugin registrations from authoritative DI
- remove transitional helpers whose only purpose was fallback compatibility
- simplify the codebase around the composer-only runtime model

**Acceptance Criteria**

- retained legacy runtime classes no longer participate in supported execution
- obsolete DI registrations are removed
- the authoritative backend code reflects one runtime model

### Phase 5: Remove Or Archive Legacy SQL Assets

**Goal**

Stop treating any legacy SQL asset as authoritative for runtime or operations.

**Focus**

- remove or archive SQL assets that remain live only because of the legacy runtime
- update testing and operational procedures to composer-era authoritative assets only
- keep historical SQL only where it is clearly marked as archival material

**Acceptance Criteria**

- no supported runtime or supported operational workflow depends on legacy SQL assets
- the status of imported SQL templates and schema-prep assets is explicit and documented
- remaining historical SQL is clearly archived and non-authoritative

## Cross-Phase Rules

- prefer incremental removal over large unvalidated rewrites
- keep unsupported behavior explicit during migration rather than masking it behind fallback
- do not describe planned removal as completed removal
- treat parity as the gate for fallback removal
- treat SQL assets as in scope only when runtime, tests, or supported operations still depend on them

## Validation Strategy

- keep focused tests for each request family moved from fallback to composer-only execution
- keep legacy comparison tests while both implementations still exist and parity must still be proven
- add or keep diagnostics that expose accidental use of fallback or hybrid legacy paths during migration
- verify that no supported path still depends on legacy SQL assets before those assets are removed or archived
- review docs, scripts, and operational guidance as part of each removal phase, not only at the end

## Final Recommendation

Use this plan to sequence the work after the deprecation proposal is accepted.
Start by fixing the inventory boundary first.
Once the live legacy surface is explicit, remove functional dependency on it, then remove the fallback branches, then delete the retained code and SQL assets that no longer have an authoritative role.