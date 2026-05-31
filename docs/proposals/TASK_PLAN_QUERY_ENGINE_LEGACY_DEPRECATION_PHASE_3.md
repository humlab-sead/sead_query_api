# Task Plan: Query Engine Legacy Deprecation - Phase 3 Remove Runtime Fallback Branches

## Phase Summary

**Phase:** Phase 3 - Remove Runtime Fallback Branches

**Status:** Done

**Goal**

Make composer the only live request-execution path.

**Focus**

- remove facet-content fallback from `FacetContentService`
- remove legacy delegation from the result handoff boundary
- convert unsupported requests into explicit failures or clearly unsupported contracts rather than silent legacy routing
- shrink the retained base-class and compatibility wiring that still keeps `QuerySetupBuilder` registered on the authoritative runtime path

**Acceptance Criteria**

- [x] Facet-content requests no longer drop to `CategoryCountService` from the supported runtime path.
- [x] Result requests no longer drop to `LegacyResultProjectionHandoffBuilder` from the supported runtime path.
- [x] Unsupported requests fail explicitly with reviewable diagnostics instead of silently routing to legacy execution.
- [x] `QuerySetupBuilder` is no longer required by the authoritative runtime DI path after fallback removal.
- [x] Focused validation and documentation updates confirm one authoritative execution model.

## Work Breakdown

### Remove Facet-Content Fallback From The Runtime Path

**Objective**

Turn `FacetContentService` into a composer-only boundary for live facet-content execution.

- [x] Confirm the unsupported facet-content contract to use when `ComposedFacetContentService` cannot handle a request.
- [x] Replace the current fallback branch in `FacetContentService.Load(...)` with explicit failure handling and request-context diagnostics.
- [x] Remove the authoritative runtime dependency from `FacetContentService` to `CategoryCountService` and any retained `QueryServiceBase` or `IQuerySetupBuilder` wiring that only exists because of the fallback path.
- [x] Update or add focused tests for supported facet-content success and unsupported facet-content failure behavior.

**Completion Criteria**

- [x] `FacetContentService` no longer executes legacy category-count logic as a runtime fallback.

### Remove Legacy Result Handoff Delegation

**Objective**

Make the result handoff boundary fail explicitly instead of delegating unsupported requests to legacy setup and projection logic.

- [x] Replace the legacy delegation branch in `ComposedResultProjectionHandoffBuilder.Build(...)` with explicit failure behavior when `TryCreateRequest(...)` cannot build a composed request.
- [x] Keep failure messages and diagnostics specific enough to identify the unsupported result facet, view, and reason without reopening silent fallback.
- [x] Remove the authoritative runtime dependency on `LegacyResultProjectionHandoffBuilder` from the composed handoff path and adjust DI wiring accordingly.
- [x] Update or add focused tests for composed result success and unsupported-result failure behavior.

**Completion Criteria**

- [x] Result loading no longer delegates unsupported requests to `LegacyResultProjectionHandoffBuilder` from the live runtime path.

### Shrink Retained Query-Builder Infrastructure From The Authoritative Runtime Path

**Objective**

Remove the remaining base-class and compatibility wiring that still keeps `QuerySetupBuilder` registered for authoritative runtime execution.

- [x] Re-check the live `QuerySetupBuilder`, `IQuerySetupBuilder`, and `QueryServiceBase` caller set after the fallback branches are removed.
- [x] Remove runtime DI registrations that are no longer needed once `FacetContentService` and the composed result handoff no longer depend on fallback-era services.
- [x] Simplify `FacetContentService` and any other touched runtime classes so they no longer inherit or inject legacy query-builder infrastructure only for compatibility.
- [x] Record any compatibility-only services that still reference `QuerySetupBuilder` as explicit Phase 4 removal candidates rather than leaving them on the authoritative runtime path implicitly.

**Completion Criteria**

- [x] The authoritative runtime DI path no longer needs `QuerySetupBuilder` or `QueryServiceBase` to serve live facet-content or result requests.

### Close The Phase With Validation And Planning Alignment

**Objective**

Leave fallback removal reviewable and make the post-cutover cleanup boundary explicit for Phase 4.

- [x] Run focused validation for the touched facet-content, result handoff, and DI slices.
- [x] Run a broader regression check before closing the phase.
- [x] Update `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md` and `docs/proposals/QUERY_ENGIVE_LEGACY_DEPRECATION.md` to reflect the landed Phase 3 state.
- [x] Update the maintained Phase 1 inventory if any retained legacy class changes disposition because fallback removal has landed.
- [x] Capture the exact retained services and registrations that move from Phase 3 cleanup to Phase 4 deletion.

**Completion Criteria**

- [x] Phase 3 leaves one documented runtime model and a smaller, explicit Phase 4 deletion set.

## Progress Tracker

| Area                                                                             | Status      | Notes                                                                                                                                                                                                                                                             |
|----------------------------------------------------------------------------------|-------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Remove facet-content fallback from the runtime path                              | Done        | `FacetContentService` now delegates directly to `ComposedFacetContentService`; unsupported facet-content requests fail explicitly instead of dropping to `CategoryCountService`.                                                                                  |
| Remove legacy result handoff delegation                                          | Done        | `ComposedResultProjectionHandoffBuilder` now logs and throws for unsupported requests instead of delegating to `LegacyResultProjectionHandoffBuilder`.                                                                                                            |
| Shrink retained query-builder infrastructure from the authoritative runtime path | Done        | The main API and test DI modules no longer register `IQuerySetupBuilder` or `LegacyResultProjectionHandoffBuilder`; remaining `QuerySetupBuilder` and `QueryServiceBase` references are retained direct-test or Phase 4 cleanup surfaces such as `ReportService`. |
| Close the phase with validation and planning alignment                           | Done        | The Phase 1 inventory now reflects composer-only live execution, the exact Phase 4 carry-over set is explicit, and the full `sead.query.test` project passed with `1633` tests passing and `1` skipped.                                                      |

## Definition Of Done

- [x] All Phase 3 acceptance criteria are satisfied.
- [x] Live facet-content and result requests execute through the composer-only runtime path.
- [x] Unsupported requests fail explicitly with diagnosable messages instead of silent legacy routing.
- [x] The authoritative runtime DI path no longer requires `QuerySetupBuilder` for fallback-era wiring.
- [x] Tests and diagnostics are updated to guard the composer-only execution model.
- [x] Proposal, phase-plan, and inventory documents reflect the landed fallback-removal boundary and the remaining Phase 4 cleanup set.

## Validation And Testing

- [x] Run targeted `dotnet test` coverage for the touched facet-content, result handoff, and DI suites in `sead.query.test`.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj` before closing the phase.
- [x] Run workspace diagnostics on the updated Phase 3 planning and deprecation documents.
- [x] Confirm there is no remaining supported-path diagnostic, test, or runtime assertion that expects legacy fallback success.

## Deliverables

| Deliverable                             | Description                                                                 | Status      | Link                                                                                                                          |
|-----------------------------------------|-----------------------------------------------------------------------------|-------------|-------------------------------------------------------------------------------------------------------------------------------|
| Phase 3 task plan                       | Execution tracker for runtime fallback removal and authoritative DI cleanup | Done        | `docs/proposals/TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_3.md`                                                         |
| Phase-plan alignment update             | Phase 3 execution link and post-cutover sequencing update                   | Done        | `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md`                                                                |
| Proposal and inventory alignment update | Updated deprecation boundary after fallback removal lands and Phase 4 carry-over set capture | Done        | `docs/proposals/QUERY_ENGIVE_LEGACY_DEPRECATION.md` and `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` |

## Scope

**In scope**

- removing the live fallback branches in `FacetContentService` and `ComposedResultProjectionHandoffBuilder`
- replacing silent legacy routing with explicit unsupported-request behavior
- removing authoritative runtime DI and base-class wiring that remains only because of fallback execution
- updating focused validation and maintained deprecation planning material for the composer-only cutover

**Out of scope**

- deleting every legacy compatibility service or plugin implementation regardless of remaining non-runtime references
- Phase 5 SQL asset removal or archival work
- unrelated API, DTO, or route-inventory redesign
- release sequencing or deployment ownership

## Risks And Mitigations

| Risk                                                                                                                   | Mitigation                                                                                                                                                   |
|------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Unsupported requests may have been relying on silent fallback as accidental behavior.                                  | Replace fallback with explicit failures and keep the failure reason in diagnostics and focused tests.                                                        |
| Removing fallback wiring may expose hidden DI or test fixtures that still assume `QuerySetupBuilder` is authoritative. | Re-check the live caller set immediately after fallback removal and treat any remaining caller as explicit Phase 3 or Phase 4 work, not as an implicit keep. |
| Phase 3 cleanup may blur into broad legacy deletion and make review harder.                                            | Limit this phase to fallback-branch removal plus the DI and base-class cleanup directly unlocked by that cutover; leave deeper deletions to Phase 4.         |

## Assumptions

- Phase 2 is the current supported-surface baseline and has already removed supported-path functional dependency on `BogusPickService` and direct `QuerySetupBuilder` request setup.
- Phase 3 can turn unsupported requests into explicit failures without reopening Phase 2 parity analysis for already-supported requests.
- Any remaining `QuerySetupBuilder` reference after the fallback cutover should be treated as compatibility-only unless Phase 3 proves it still affects live runtime execution.