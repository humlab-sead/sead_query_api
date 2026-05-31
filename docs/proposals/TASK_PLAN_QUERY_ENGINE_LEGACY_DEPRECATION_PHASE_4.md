# Task Plan: Query Engine Legacy Deprecation - Phase 4 Remove Retained Legacy Implementation Surfaces

## Phase Summary

**Phase:** Phase 4 - Remove Retained Legacy Implementation Surfaces

**Status:** Done

**Goal**

Delete retained legacy implementation code that is no longer needed after the Phase 3 cutover.

**Focus**

- remove dead legacy query-builder scaffolding and compatibility-only services
- remove retained legacy DI registrations and plugin runtime groups from authoritative containers
- retire direct legacy comparison helpers that still keep deleted surfaces alive
- keep the maintained inventory and validation aligned with each deletion slice

**Acceptance Criteria**

- [x] Retained legacy runtime classes no longer participate in supported execution.
- [x] Obsolete DI registrations are removed from authoritative runtime and shared test containers.
- [x] Legacy plugin `RegisterLegacyRuntime(...)` groups are removed once no retained compatibility service depends on them.
- [x] Direct comparison tests and helpers no longer keep deleted legacy surfaces alive implicitly.
- [x] The authoritative backend code reflects one runtime model and the maintained deprecation docs match it.

## Work Breakdown

### Remove Dead Legacy Base-Class Scaffolding

**Objective**

Delete isolated legacy base classes or helper types that no longer have any runtime or test callers.

- [x] Re-check the self-only caller set for `QueryServiceBase` and `ReportService`.
- [x] Remove dead legacy base-class scaffolding that has no remaining callers.
- [x] Re-run focused validation on the touched core slice.

**Completion Criteria**

- [x] Self-only legacy base-class scaffolding is removed without changing supported runtime behavior.

### Remove Retained Legacy Result And Facet Compatibility Services

**Objective**

Delete compatibility-only services that remain after the fallback cutover and make any retained comparison coverage explicit.

- [x] Remove `LegacyResultProjectionHandoffBuilder` and the remaining test wiring that keeps it alive.
- [x] Remove `CategoryCountService` and `BogusPickService` from authoritative DI once their remaining callers are retired or replaced.
- [x] Rework retained live comparison tests so they no longer keep deleted legacy surfaces alive implicitly.
- [x] Delete the remaining implementation-only `CategoryCountService`, `BogusPickService`, and dead support types once the direct unit-test callers are retired or rewritten.

**Completion Criteria**

- [x] The main runtime and shared test containers no longer carry legacy result or facet compatibility services that the supported runtime does not use.

### Remove Legacy Plugin Runtime Groups

**Objective**

Delete `RegisterLegacyRuntime(...)` plugin groups once no retained compatibility service depends on them.

- [x] Verify which keyed services from the discrete, range, intersect, and geo-polygon plugins are still required after compatibility-service removal.
- [x] Remove legacy plugin group calls from authoritative DI and shared test DI.
- [x] Delete any plugin-side legacy registrations that become unreachable.

**Completion Criteria**

- [x] The authoritative runtime depends only on composer and shared plugin registrations.

### Close The Phase With Inventory And Validation Alignment

**Objective**

Leave the remaining runtime model and deletion boundary reviewable after each removal slice.

- [x] Update the maintained Phase 1 inventory as retained surfaces change classification or disappear.
- [x] Keep the Phase 4 task plan progress tracker current as deletion slices land.
- [x] Run focused validation for each removal slice and a full `sead.query.test` regression before closing the phase.
- [x] Repeat the inventory and full-regression alignment after the remaining implementation-only legacy classes are deleted.

**Completion Criteria**

- [x] The maintained inventory, the Phase 4 task plan, and the test suite all reflect the post-deletion runtime truth.

## Progress Tracker

| Area                                                           | Status      | Notes                                                                                                                                                                                                                                                                                                                 |
|----------------------------------------------------------------|-------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Remove dead legacy base-class scaffolding                      | Done        | `QueryServiceBase` and `ReportService` were removed, and `dotnet build sead.query.test/sead.query.test.csproj` passed afterward.                                                                                                                                                                                      |
| Remove retained legacy result and facet compatibility services | Done | `LegacyResultProjectionHandoffBuilder`, `CategoryCountService`, `BogusPickService`, their dead support types, and their direct legacy-only test callers are removed from active code. |
| Remove legacy plugin runtime groups                            | Done        | The discrete, range, intersect, and geo-polygon plugins now expose only composer and shared registrations, with keyed `IPickFilterCompiler` services moved into shared registration where needed.                                                                                                                     |
| Close the phase with inventory and validation alignment        | Done | Focused deletion-slice regression passed (`373` passed, `0` failed), and full `dotnet test sead.query.test/sead.query.test.csproj --logger "console;verbosity=minimal"` passed (`1422` passed, `54` skipped). |

## Definition Of Done

- [x] All Phase 4 acceptance criteria are satisfied.
- [x] Dead legacy scaffolding and compatibility-only runtime services are removed.
- [x] Authoritative DI no longer registers legacy runtime services or plugin groups.
- [x] Remaining tests no longer depend on deleted legacy surfaces implicitly.
- [x] The Phase 1 inventory and Phase 4 task plan reflect the landed deletion set.
- [x] `dotnet test sead.query.test/sead.query.test.csproj` passes after the final Phase 4 slice.

## Validation And Testing

- [x] Run focused build or test validation for each touched legacy-deletion slice.
- [x] Run focused unit or live-comparison coverage for any rewritten comparison helper or DI boundary.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj` after the current Phase 4 DI/plugin-removal slice.
- [x] Re-run the full project regression after the remaining implementation-only legacy deletions land.

## Deliverables

| Deliverable                | Description                                                                              | Status      | Link                                                                  |
|----------------------------|------------------------------------------------------------------------------------------|-------------|-----------------------------------------------------------------------|
| Phase 4 task plan          | Execution tracker for retained legacy runtime deletion                                   | Done        | `docs/proposals/TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_4.md` |
| Runtime deletion slices    | Removal of retained legacy code, direct legacy-only tests, and dead support surfaces     | Done        | `TBD`                                                                 |
| Inventory alignment update | Maintained legacy-surface inventory for the post-Phase-4 runtime state                   | Done        | `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` |

## Scope

**In scope**

- deleting retained legacy runtime classes and dead scaffolding left after Phase 3
- removing compatibility-only DI registrations from authoritative runtime and shared test containers
- deleting legacy plugin runtime groups once their callers are gone
- updating maintained deprecation documents and validation coverage for each deletion slice

**Out of scope**

- Phase 5 SQL asset archival or operational migration work
- unrelated composer architecture changes
- release scheduling or deployment workflow changes

## Risks And Mitigations

| Risk                                                                                                               | Mitigation                                                                                                           |
|--------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------|
| A retained comparison test may still rely on a legacy service that looks unused from the runtime path alone.       | Re-check callers before deletion and validate the touched comparison or live-test slice immediately after each edit. |
| Legacy plugin groups may still supply keyed services to compatibility helpers even after runtime fallback removal. | Remove compatibility services before removing plugin legacy groups, and validate DI boundaries after each step.      |
| Inventory drift may reopen ambiguity about what still belongs to Phase 4 versus Phase 5.                           | Update the maintained Phase 1 inventory as each Phase 4 deletion slice lands.                                        |

## Assumptions

- Phase 3 has already established the composer-only live request path and removed authoritative `IQuerySetupBuilder` and `LegacyResultProjectionHandoffBuilder` DI registrations.
- The explicit Phase 4 carry-over set in `QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` is the current deletion boundary.
- It is acceptable to remove comparison-only legacy surfaces once they no longer protect an active migration boundary.