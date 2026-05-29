# Task Plan: Phase 3 - Non-Discrete Facet Parity

## Phase Summary

**Phase:** Phase 3 - Non-Discrete Facet Parity

**Status:** In progress

**Goal**

Extend the composed path to the remaining non-discrete facet families needed for legacy parity.

**Focus**

- complete range-target coverage beyond the currently validated targets
- implement and validate intersect-facet support under the same anchor contract
- implement and validate GIS polygon support under the same anchor contract
- keep facet-type-specific behavior inside resolver or composer boundaries

**Acceptance Criteria**

- [ ] Legacy range behavior needed by the active API is supported on the composed path.
- [ ] Intersect and GIS polygon requests have compiled contracts, runtime validation, and regression coverage.
- [ ] The composed runtime supports the legacy facet families required by the API, not just the first validated subset.

## Documentation Targets

- Maintain the working parity inventory in `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`.
- Record durable contract or runtime-boundary changes in `docs/DESIGN.md`.
- Update this task plan in place as range, intersect, and GIS polygon support widens or explicit exceptions are accepted.

## Work Breakdown

### Non-Discrete Inventory And Exception List

**Objective**

Turn the remaining non-discrete parity gap into one explicit working list of supported slices, widening candidates, and exceptions.

- [ ] Enumerate the in-scope legacy non-discrete facet families still outside the composed supported matrix.
- [ ] Separate the remaining surface into range, intersect, and GIS polygon lanes.
- [ ] Confirm that the routed discrete overcount blockers carried out of Phase 2 remain on the discrete exception list unless Phase 3 uncovers a shared non-discrete root cause.
- [ ] Classify each remaining lane as planned-for-phase support or explicit exception.
- [ ] Record one concrete blocker for every explicit exception.
- [ ] Keep `PARITY_INVENTORY.md` aligned with the current support or exception status.

**Completion Criteria**

- [ ] Every in-scope non-discrete facet family is either in the widening backlog or on an explicit exception list.
- [ ] The parity inventory distinguishes supported, in-progress, and excepted non-discrete behavior clearly enough to guide implementation.

### Range-Target Widening

**Objective**

Extend composed range support beyond the current validated subset without weakening the existing anchor and fallback contracts.

- [ ] Inventory the legacy range targets not yet promoted into `SupportedComposedRangeLiveUris`.
- [ ] Group remaining range targets by shape: direct, view-backed, measured-value, and routed range targets.
- [ ] Implement or widen target-side range support only where the current anchor and category contracts stay explicit.
- [ ] Keep unsupported range targets on the explicit exception list until a concrete contract exists.
- [ ] Add or update focused unit tests for each new range support rule.

**Completion Criteria**

- [ ] Newly supported range targets run through the composed path with explicit target-side contracts.
- [ ] Unsupported range cases still fail or fall back explicitly rather than producing misleading SQL.

### Intersect Facet Support

**Objective**

Replace the current intersect fallback with compiled composed-path support only when the intersect contract is explicit and testable.

- [ ] Define the composed contract for intersect facet requests under the current anchor model.
- [ ] Implement intersect-specific query behavior inside resolver or composer boundaries rather than in controllers or runtime wiring.
- [ ] Preserve explicit legacy fallback until the intersect contract is proven.
- [ ] Add focused unit coverage for intersect contract resolution and failure boundaries.
- [ ] Promote at least one live intersect comparison slice only after the composed path and legacy output agree.

**Completion Criteria**

- [ ] Intersect requests either run on the composed path with explicit contracts or remain on a documented exception list.
- [ ] Intersect support is covered by focused contract tests and live validation.

### GIS Polygon Support

**Objective**

Add a compiled and validated polygon-filter path without leaking GIS-specific behavior across unrelated composer boundaries.

- [ ] Define the composed contract for polygon-filter requests under the current anchor model.
- [ ] Implement GIS polygon filtering inside resolver or composer boundaries with explicit fallback rules.
- [ ] Preserve explicit legacy fallback until polygon filtering is proven for at least one live slice.
- [ ] Add focused unit coverage for polygon contract resolution and unsupported-boundary behavior.
- [ ] Promote live polygon validation only after the composed path and legacy output agree.

**Completion Criteria**

- [ ] GIS polygon requests either run on the composed path with explicit contracts or remain on a documented exception list.
- [ ] Polygon-specific behavior is validated without weakening current fallback boundaries for other facet families.

### Documentation And Phase Exit

**Objective**

Leave one explicit record of what Phase 3 delivered and what it intentionally deferred.

- [ ] Update `PARITY_INVENTORY.md` when a non-discrete lane moves from unsupported or partial to supported.
- [ ] Update `docs/DESIGN.md` when non-discrete runtime or contract boundaries change.
- [ ] Record explicit exceptions that remain at phase exit, with blockers or deferral reasons.
- [ ] Update this task plan’s progress tracker, validation log, and deliverables as widening lands.

**Completion Criteria**

- [ ] The end-of-phase non-discrete support surface is visible without reading code diffs.
- [ ] Remaining exceptions are documented as intentional follow-up work rather than accidental gaps.

## Progress Tracker

| Area | Status | Notes |
|---|---|---|
| Non-discrete inventory and exception list | In progress | `PARITY_INVENTORY.md` now shows supported range widening, supported intersect widening, and supported GIS polygon widening. The active catalog checks for Phase 3 found no additional real range, intersect, or geo-polygon candidates beyond the already-supported set and helper-only residue, while the routed discrete overcount set from Phase 2 (`abundance_classification`, `abundance_elements`, `construction_purpose`, `constructions`, `country`, `region`, `feature_type`, `family`, `sample_group_sampling_contexts`, and `species`) remains on the discrete exception track unless a shared non-discrete contract gap is proven. |
| Range-target widening | In progress | `SupportedComposedRangeLiveUris` now validates both `geochronology:geochronology` and `geochronology:country@1,2,5/geochronology` alongside `tbl_denormalized_measured_values_33_0`, `tbl_denormalized_measured_values_33_82`, `tbl_denormalized_measured_values_32`, `tbl_denormalized_measured_values_37`, and `abundances_all`. The active catalog inventory check for Phase 3 found no additional real range candidates beyond the already-supported set and helper-only residue. |
| Intersect facet support | In progress | `analysis_entity_ages:analysis_entity_ages` and `dendro_age_contained_by:dendro_age_contained_by` now run on the composed path with focused and grouped live coverage through `SupportedComposedIntersectLiveUris`. The current intersect catalog inventory is exhausted for Phase 3 unless additional intersect facets are later activated or reclassified as in-scope. |
| GIS polygon support | In progress | `sites_polygon:sites_polygon@63.872484,20.093291,63.947006,20.501316,63.878949,20.673213,63.748021,20.252953,63.793983,20.095738` now runs on the composed path with focused unit coverage, focused live parity, and grouped live coverage through `SupportedComposedGeoPolygonLiveUris`. The current geo-polygon catalog inventory is exhausted for Phase 3 unless additional geo facets are later activated or reclassified as in-scope. |
| Documentation and phase exit | Not started | Phase 3 needs its own execution tracker plus parity-inventory and design-document updates as non-discrete support changes. |

## Definition Of Done

- [ ] All Phase 3 acceptance criteria are satisfied.
- [ ] Every in-scope non-discrete facet family is either supported on the composed path or listed as an explicit exception with a concrete blocker.
- [ ] Supported composed non-discrete output matches legacy behavior for the validated matrix.
- [ ] Grouped range regression and any new non-discrete regression coverage stay green without regressions.
- [ ] Focused validation has been run and recorded for each promoted non-discrete widening batch.
- [ ] `PARITY_INVENTORY.md` and `docs/DESIGN.md` reflect the final Phase 3 support surface.
- [ ] Follow-up work is captured as explicit exceptions or later-phase tasks.

## Validation And Testing

- [x] Run focused unit validation for the target-only range contract using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~SQT.UnitTests.QueryComposer.Services.ComposedFacetContentServiceTests"`.
- [ ] Run focused composed-service tests for touched non-discrete widening slices.
- [x] Run grouped live comparison coverage for the current supported range subset using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedRangeLiveSlices"`.
- [x] Run focused live comparison coverage for the promoted target-only geochronology range slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyGeochronologySlice"` before grouped promotion.
- [x] Run focused unit validation for the promoted target-only intersect contract using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~SQT.UnitTests.QueryComposer.Services.ComposedFacetContentServiceTests"`.
- [x] Run focused live comparison coverage for the promoted target-only intersect slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyIntersectSlice"` before grouped promotion.
- [x] Run grouped live comparison coverage for the promoted intersect subset using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedIntersectLiveSlices"`.
- [x] Run focused live comparison coverage for the promoted `dendro_age_contained_by` intersect slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyDendroAgeContainedBySlice|FullyQualifiedName~Load_Intersect_Facets"` before grouped promotion.
- [x] Run focused unit validation for the promoted target-only geo-polygon contract using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~SQT.UnitTests.QueryComposer.Services.ComposedFacetContentServiceTests"`.
- [x] Run focused live comparison coverage for the promoted `sites_polygon` slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlySitesPolygonSlice"` before grouped promotion.
- [x] Run grouped live comparison coverage for the promoted geo-polygon subset using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedGeoPolygonLiveSlices"`.
- [ ] Run focused live comparison coverage for each intersect or GIS polygon slice before changing fallback boundaries.
- [ ] Review `PARITY_INVENTORY.md` after each non-discrete promotion or exception decision to confirm range, intersect, and GIS rows remain current.

## Deliverables

| Deliverable | Description | Status | Link |
|---|---|---|---|
| Non-discrete parity task plan | Execution tracker for Phase 3 widening work | In progress | `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_3.md` |
| Updated parity inventory | Current support and exception status for range, intersect, and GIS polygon behavior | In progress | `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md` |
| Non-discrete contract updates | Code-level widening for range, intersect, and GIS polygon support | In progress | `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs` |
| Non-discrete live coverage | Focused and grouped live coverage for supported non-discrete slices and explicit fallback boundaries | In progress | `sead.query.test/LiveTests/FacetLoadService.cs` |
| Durable architecture updates | Non-discrete runtime and contract boundary updates | Not started | `docs/DESIGN.md` |

## Scope

**In scope**

- widening the composed runtime across the remaining in-scope non-discrete facet families
- explicit exception handling for range, intersect, or GIS polygon cases that still cannot satisfy the current composed contracts
- focused unit, composed-service, and live comparison validation for promoted non-discrete slices
- parity-inventory and design-document updates required to keep the supported non-discrete surface explicit

**Out of scope**

- discrete facet-family widening beyond regression protection for already validated slices
- the routed discrete overcount blockers carried out of Phase 2, unless Phase 3 work proves they share a non-discrete root cause
- result-set parity or final result projection work
- full retirement of legacy fallback outside the non-discrete parity surface
- broad route or anchor redesign that would reopen the Phase 1 contract baseline

## Risks And Mitigations

| Risk | Mitigation |
|---|---|
| Remaining range targets depend on undocumented legacy category or view behavior. | Promote one focused live slice at a time and keep explicit exception rows for anything still unclear. |
| Intersect support needs a broader anchor contract than the current composed path exposes. | Define the intersect contract explicitly and keep fallback authoritative until focused unit and live comparisons pass. |
| GIS polygon support leaks spatial behavior into unrelated composer boundaries. | Keep polygon-specific behavior local to resolver or composer boundaries and validate fallback behavior before any promotion. |
| Non-discrete widening regresses the already-supported range matrix. | Re-run the grouped range live regression after each promotion batch and keep parity inventory updates in the same change. |

## Assumptions

- Phase 3 covers facet-content parity for non-discrete families only; result projection remains Phase 4 work.
- The current supported range subset and the explicit intersect and GIS fallback tests are the authoritative starting baseline for this phase.
- The routed discrete overcount blockers identified in Phase 2 remain discrete exceptions by default and should move into Phase 3 only if range, intersect, or GIS implementation work exposes the same underlying contract defect.