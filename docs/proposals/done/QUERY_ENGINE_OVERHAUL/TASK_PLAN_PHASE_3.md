# Task Plan: Phase 3 - Non-Discrete Facet Parity

## Phase Summary

**Phase:** Phase 3 - Non-Discrete Facet Parity

**Status:** Done

**Goal**

Extend the composed path to the remaining non-discrete facet families needed for legacy parity.

**Focus**

- complete range-target coverage beyond the initially validated targets
- implement and validate intersect-facet support under the same anchor contract
- implement and validate GIS polygon support under the same anchor contract
- keep facet-type-specific behavior inside resolver or composer boundaries

**Acceptance Criteria**

- [x] Legacy range behavior needed by the active API is supported on the composed path.
- [x] Intersect and GIS polygon requests have compiled contracts, runtime validation, and regression coverage.
- [x] The composed runtime supports the legacy facet families required by the API, not just the first validated subset.

## Documentation Targets

- Maintain the working parity inventory in `docs/proposals/done/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`.
- Record durable contract or runtime-boundary changes in `docs/DESIGN.md`.
- Capture deferred result projection work in `docs/proposals/done/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_4.md`.

## Work Breakdown

### Non-Discrete Inventory And Exception List

**Objective**

Turn the remaining non-discrete parity gap into one explicit working list of supported slices, widening candidates, and exceptions.

- [x] Enumerate the in-scope legacy non-discrete facet families still outside the composed supported matrix.
- [x] Separate the remaining surface into range, intersect, and GIS polygon lanes.
- [x] Confirm that the routed discrete overcount blockers carried out of Phase 2 remain on the discrete exception list unless Phase 3 uncovers a shared non-discrete root cause.
- [x] Classify each remaining lane as planned-for-phase support or explicit exception.
- [x] Record one concrete blocker for every explicit exception.
- [x] Keep `PARITY_INVENTORY.md` aligned with the current support or exception status.

**Completion Criteria**

- [x] Every in-scope non-discrete facet family is either in the widening backlog or on an explicit exception list.
- [x] The parity inventory distinguishes supported, in-progress, and excepted non-discrete behavior clearly enough to guide implementation.

### Range-Target Widening

**Objective**

Extend composed range support beyond the current validated subset without weakening the existing anchor and fallback contracts.

- [x] Inventory the legacy range targets not yet promoted into `SupportedComposedRangeLiveUris`.
- [x] Group remaining range targets by shape: direct, view-backed, measured-value, and routed range targets.
- [x] Implement or widen target-side range support only where the current anchor and category contracts stay explicit.
- [x] Keep unsupported range targets on the explicit exception list until a concrete contract exists.
- [x] Add or update focused unit tests for each new range support rule.

**Completion Criteria**

- [x] Newly supported range targets run through the composed path with explicit target-side contracts.
- [x] Unsupported range cases still fail or fall back explicitly rather than producing misleading SQL.

### Intersect Facet Support

**Objective**

Replace the intersect fallback with composed-path support only when the intersect contract is explicit and testable.

- [x] Define the composed contract for intersect facet requests under the current anchor model.
- [x] Implement intersect-specific query behavior inside resolver or composer boundaries rather than in controllers or runtime wiring.
- [x] Preserve explicit legacy fallback until the intersect contract is proven.
- [x] Add focused unit coverage for intersect contract resolution and failure boundaries.
- [x] Promote live intersect comparison slices only after the composed path and legacy output agree.

**Completion Criteria**

- [x] Intersect requests either run on the composed path with explicit contracts or remain on a documented exception list.
- [x] Intersect support is covered by focused contract tests and live validation.

### GIS Polygon Support

**Objective**

Add a compiled and validated polygon-filter path without leaking GIS-specific behavior across unrelated composer boundaries.

- [x] Define the composed contract for polygon-filter requests under the current anchor model.
- [x] Implement GIS polygon filtering inside resolver or composer boundaries with explicit fallback rules.
- [x] Preserve explicit legacy fallback until polygon filtering is proven for a live slice.
- [x] Add focused unit coverage for polygon contract resolution and unsupported-boundary behavior.
- [x] Promote live polygon validation only after the composed path and legacy output agree.

**Completion Criteria**

- [x] GIS polygon requests either run on the composed path with explicit contracts or remain on a documented exception list.
- [x] Polygon-specific behavior is validated without weakening fallback boundaries for other facet families.

### Documentation And Phase Exit

**Objective**

Leave one explicit record of what Phase 3 delivered and what it intentionally deferred.

- [x] Update `PARITY_INVENTORY.md` when a non-discrete lane moves from unsupported or partial to supported.
- [x] Update `docs/DESIGN.md` when non-discrete runtime or contract boundaries change.
- [x] Record explicit exceptions that remain at phase exit, with blockers or deferral reasons.
- [x] Update this task plan’s progress tracker, validation log, and deliverables as widening lands.
- [x] Hand off result projection work to `docs/proposals/done/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_4.md`.

**Completion Criteria**

- [x] The end-of-phase non-discrete support surface is visible without reading code diffs.
- [x] Remaining exceptions are documented as intentional follow-up work rather than accidental gaps.

## Progress Tracker

| Area                                      | Status | Notes                                                                                                                                                                                                                                                                                 |
|-------------------------------------------|--------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Non-discrete inventory and exception list | Done   | `PARITY_INVENTORY.md` records supported range, intersect, and GIS polygon widening. The active catalog checks found no additional real range, intersect, or geo-polygon candidates beyond the already-supported set and helper-only residue.                                          |
| Range-target widening                     | Done   | `SupportedComposedRangeLiveUris` covers both target-only and country-predicate `geochronology` plus the active measured-value range targets, and the current range catalog is exhausted for Phase 3.                                                                                  |
| Intersect facet support                   | Done   | `analysis_entity_ages:analysis_entity_ages` and `dendro_age_contained_by:dendro_age_contained_by` run on the composed path with focused and grouped live coverage through `SupportedComposedIntersectLiveUris`.                                                                       |
| GIS polygon support                       | Done   | `sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738` runs on the composed path with focused unit coverage, focused live parity, and grouped live coverage through `SupportedComposedGeoPolygonLiveUris`. |
| Documentation and phase exit              | Done   | `PARITY_INVENTORY.md`, `docs/DESIGN.md`, and this plan reflect the final Phase 3 support surface. Deferred result projection work is now tracked in `TASK_PLAN_PHASE_4.md`.                                                                                                           |

## Definition Of Done

- [x] All Phase 3 acceptance criteria are satisfied.
- [x] Every in-scope non-discrete facet family is either supported on the composed path or listed as an explicit exception with a concrete blocker.
- [x] Supported composed non-discrete output matches legacy behavior for the validated matrix.
- [x] Grouped range regression and any new non-discrete regression coverage stay green without regressions.
- [x] Focused validation has been run and recorded for each promoted non-discrete widening batch.
- [x] `PARITY_INVENTORY.md` and `docs/DESIGN.md` reflect the final Phase 3 support surface.
- [x] Follow-up work is captured as explicit exceptions or later-phase tasks.

## Validation And Testing

- [x] Run focused unit validation for the target-only range, intersect, and geo-polygon contracts using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~SQT.UnitTests.QueryComposer.Services.ComposedFacetContentServiceTests"`.
- [x] Run focused live comparison coverage for the promoted target-only range slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyGeochronologySlice"`.
- [x] Run grouped live comparison coverage for the supported range subset using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedRangeLiveSlices"`.
- [x] Run focused live comparison coverage for the promoted intersect slices using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyIntersectSlice"` and `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyDendroAgeContainedBySlice|FullyQualifiedName~Load_Intersect_Facets"`.
- [x] Run grouped live comparison coverage for the supported intersect subset using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedIntersectLiveSlices"`.
- [x] Run focused live comparison coverage for the promoted `sites_polygon` slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlySitesPolygonSlice"`.
- [x] Run grouped live comparison coverage for the supported geo-polygon subset using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedGeoPolygonLiveSlices"`.
- [x] Run the combined supported composed live regression using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedLiveSlices"`.

## Deliverables

| Deliverable                   | Description                                                                         | Status | Link                                                                        |
|-------------------------------|-------------------------------------------------------------------------------------|--------|-----------------------------------------------------------------------------|
| Non-discrete parity task plan | Execution tracker for Phase 3 widening work                                         | Done   | `docs/proposals/done/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_3.md`                 |
| Updated parity inventory      | Current support and exception status for range, intersect, and GIS polygon behavior | Done   | `docs/proposals/done/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`                  |
| Non-discrete contract updates | Code-level widening for range, intersect, and GIS polygon support                   | Done   | `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs` |
| Non-discrete live coverage    | Focused and grouped live coverage for supported non-discrete slices                 | Done   | `sead.query.test/LiveTests/FacetLoadService.cs`                             |
| Durable architecture updates  | Non-discrete runtime and contract boundary updates                                  | Done   | `docs/DESIGN.md`                                                            |
| Phase 4 handoff               | Follow-on tracker for result projection parity                                      | Done   | `docs/proposals/done/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_4.md`                 |

## Scope

**In scope**

- widening the composed runtime across the remaining in-scope non-discrete facet families
- explicit exception handling for range, intersect, or GIS polygon cases that still cannot satisfy the current composed contracts
- focused unit, composed-service, and live comparison validation for promoted non-discrete slices
- parity-inventory and design-document updates required to keep the supported non-discrete surface explicit

**Out of scope**

- discrete facet-family widening beyond regression protection for already validated slices
- the routed discrete overcount blockers carried out of Phase 2, unless shared with later result-projection work
- result-set parity or final result projection work
- full retirement of legacy fallback outside the non-discrete parity surface
- broad route or anchor redesign that would reopen the Phase 1 contract baseline

## Assumptions

- Phase 3 covers facet-content parity for non-discrete families only; result projection moves to Phase 4.
- The active range, intersect, and geo-polygon catalogs required by the API are exhausted under the current repository data set unless additional facets are later activated or reclassified as in scope.
- The routed discrete overcount blockers identified in Phase 2 remain discrete exceptions by default unless later result-projection work proves a shared root cause.