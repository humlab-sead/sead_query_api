# Task Plan: Query Engine Legacy Deprecation - Phase 2 Close Supported-Surface Parity Gaps

## Phase Summary

**Phase:** Phase 2 - Close Supported-Surface Parity Gaps

**Status:** In progress

**Goal**

Remove the functional need for legacy fallback on the supported request surface.

**Focus**

- replace request-time helper behavior that still forces supported requests through `BogusPickService` or `QuerySetupBuilder`
- remove or isolate the remaining supported-path callers that keep `ComposedResultProjectionHandoffBuilder` hybrid
- keep parity validation in place while legacy fallback still exists for explicitly unsupported requests
- use the explicit legacy/composer/shared plugin registration split as the boundary for later removal phases

**Acceptance Criteria**

- [ ] Each supported request family either runs on the composed path or is explicitly removed from the supported surface.
- [ ] No supported request relies on legacy fallback as a functional dependency.
- [ ] Focused validation proves composed-path behavior for the supported matrix.
- [ ] The remaining legacy registrations required only for fallback are isolated behind the explicit DI boundary.

## Work Breakdown

### Remove Legacy Pick Normalization From Supported Requests

**Objective**

Stop treating `BogusPickService` as an authoritative preprocessing step for supported facet and result requests.

- [x] Trace the supported facet and result request families that still depended on `LoadFacetService.Load(...) -> BogusPickService.Update(...)` or `LoadResultService.Load(...) -> BogusPickService.Update(...)`.
- [x] Define the supported invalid-pick contract for those requests as a supported-request normalization path that preserves current valid-pick behavior without routing the controller load path through `BogusPickService`.
- [x] Replace the request-time dependency on `QuerySetupBuilder` in `BogusPickService` for the supported surface by introducing `SupportedRequestPickSanitizer` and rewiring the supported facet and result load services.
- [x] Keep the CLI probe commands aligned with the same supported invalid-pick contract instead of preserving separate legacy-only behavior.

**Completion Criteria**

- [x] Supported facet and result request paths no longer require `BogusPickService` as a hidden legacy normalization step.

### Remove Request-Time `QuerySetupBuilder` Dependency From Supported Paths

**Objective**

Turn the `QuerySetupBuilder` inventory into an ordered migration sequence for the supported request path.

- [x] Split the current `QuerySetupBuilder` callers into supported-path blockers, explicit fallback-only callers, and post-cutover cleanup candidates.
- [x] Prioritize the supported-path blockers in this order: `ComposedResultProjectionHandoffBuilder`, `BogusPickService`, then category-info generation that still affects supported facet-content behavior.
- [x] For `ComposedResultProjectionHandoffBuilder`, replace the current projection setup dependency on `QuerySetupBuilder` with a composer-era setup path or an explicit temporary bridge that no longer defines supported behavior.
- [x] For category-info generation, confirm which typed category-info services still rely on the abstract `CategoryInfoService` base and move the shared `CategoryInfoService` path to `SupportedRequestQuerySetupFactory` so discrete and geo-polygon category-info no longer depend on `QuerySetupBuilder`.
- [x] Move the remaining direct runtime callers `CategoryCountService`, `LegacyResultProjectionHandoffBuilder`, and `BogusPickService` to `SupportedRequestQuerySetupFactory` so fallback-era services no longer invoke `QuerySetupBuilder` directly.
- [x] Record the callers that remain fallback-only or compatibility-only after the supported surface cleanup so Phase 3 can delete them without reopening parity work.

**Completion Criteria**

- [x] The supported request path no longer depends on `QuerySetupBuilder` as a functional prerequisite.

### Hold The DI Boundary Explicit While Parity Work Proceeds

**Objective**

Keep the plugin and DI split explicit so fallback-only registrations can be removed surgically in later phases.

- [x] Split the facet plugin registrations into explicit legacy, composer, and shared registration groups.
- [x] Record which `Dependency.cs` registrations are still needed for the supported runtime versus fallback compatibility after the request-path work lands.
- [ ] Prevent new work from re-coupling composer-era services to the keyed legacy category-count registrations.
- [x] Capture the exact legacy registration groups that Phase 3 and Phase 4 should remove after parity is proven.

**Completion Criteria**

- [ ] The repository keeps a reviewable DI boundary between supported composer services and fallback-only legacy registrations.

### Close The SQL-Override Ambiguity Before Removal Planning

**Objective**

Resolve whether `facet.facet_template` is still a live runtime dependency or only a retained configuration/import surface.

- [x] Prove whether any non-archived request-path code still consumes `FacetTemplate` or `Facet.GetTemplate(...)`.
- [x] Record the proof result in the maintained Phase 1 inventory and Phase 2 planning material.
- [x] Decide whether Phase 5 should treat SQL override support as an importer/schema cleanup task or as a live runtime removal task.
- [x] Capture the remaining importer/schema and operational consumers explicitly before any removal work is scheduled.

**Completion Criteria**

- [x] The status of `facet.facet_template` is explicit enough that later removal phases do not rely on assumption.

## Progress Tracker

| Area                                                                    | Status      | Notes                                                                                                                                                                                                                                                                                                                          |
|-------------------------------------------------------------------------|-------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Remove legacy pick normalization from supported requests                | Done        | `LoadFacetService`, `LoadResultService`, and the facet/result probe commands now use `SupportedRequestPickSanitizer`; `BogusPickService` is no longer part of the supported or probe request path.                                                                                                                             |
| Remove request-time `QuerySetupBuilder` dependency from supported paths | Done        | `ComposedResultProjectionHandoffBuilder`, `CategoryCountService`, `LegacyResultProjectionHandoffBuilder`, `BogusPickService`, and the shared category-info path now use `SupportedRequestQuerySetupFactory`; remaining legacy builder use is now limited to retained base classes and non-request-path compatibility surfaces. |
| Hold the DI boundary explicit while parity work proceeds                | In progress | Plugin registrations remain split into explicit legacy, composer, and shared groups, and Phase 2 now records the supported-request DI boundary explicitly.                                                                                                                                                                     |
| Close the SQL-override ambiguity before removal planning                | Done        | `facet.facet_template` is now classified as importer/schema compatibility and Phase 5 cleanup scope, not as a live request-path blocker.                                                                                                                                                                                       |

## Definition Of Done

- [ ] All Phase 2 acceptance criteria are satisfied.
- [x] Supported request paths no longer depend on legacy preprocessing or legacy request planning as a functional requirement.
- [ ] Remaining fallback-only registrations are isolated and explicitly documented.
- [x] The `facet.facet_template` status is explicit enough to drive Phase 5 removal or archival work.
- [ ] Validation and review are recorded so Phase 3 can remove fallback branches without reopening parity analysis.

## Validation And Testing

- [x] Run focused validation for each supported request family that is moved away from legacy preprocessing or request planning.
- [ ] Keep parity-oriented checks where legacy and composed outputs can still be compared safely during the migration window.
- [x] Run the narrowest build or test validation for each DI or request-path refactor before expanding scope.
- [ ] Run workspace diagnostics on the updated Phase 2 planning and inventory documents.

## Deliverables

| Deliverable                              | Description                                                                                                                                   | Status      | Link                                                                                                                   |
|------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|-------------|------------------------------------------------------------------------------------------------------------------------|
| Phase 2 task plan                        | Execution tracker for supported-surface parity and hybrid-helper removal work                                                                 | In progress | `docs/proposals/TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_2.md`                                                  |
| Updated Phase 1 inventory                | Revised runtime, DI, and SQL evidence after the plugin split, supported-request slice implementation, and `facet_template` reachability proof | Done        | `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md`                                                  |
| Proposal and phase-plan alignment update | Boundary updates reflecting the landed Phase 2 supported-request slice                                                                        | Done        | `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md` and `docs/proposals/QUERY_ENGIVE_LEGACY_DEPRECATION.md` |

## Scope

**In scope**

- supported facet and result request families that still depend on `BogusPickService` or `QuerySetupBuilder`
- DI registration boundaries needed to remove fallback safely in later phases
- explicit proof of whether SQL override support is still part of the live request path
- focused validation for the supported parity slice being migrated

**Out of scope**

- deleting fallback branches outright before supported-surface parity is proven
- removing clearly fallback-only services whose callers have not yet been isolated
- archival cleanup of already-classified historical SQL files under `deprecated/`
- release sequencing or deployment ownership

## Risks And Mitigations

| Risk                                                                                                       | Mitigation                                                                                                              |
|------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------|
| Removing hidden pick normalization may expose unsupported requests that were previously silently repaired. | Make the invalid-pick contract explicit and validate the supported route matrix before widening scope.                  |
| `QuerySetupBuilder` callers may look equivalent while actually serving different supported-path roles.     | Sequence callers explicitly and validate after each supported-path replacement instead of attempting one broad rewrite. |
| The importer/schema presence of `facet.facet_template` may be mistaken for live runtime consumption.       | Keep request-path proof separate from importer/schema evidence and record both in the maintained inventory.             |

## Assumptions

- The Phase 1 inventory is the current authoritative baseline for choosing the first Phase 2 implementation slices.
- The explicit legacy/composer/shared plugin registration split is preparatory infrastructure, not the completion of Phase 2 by itself.
- Phase 2 may leave fallback-only code in place temporarily, but it should remove the supported surface's functional dependency on that code.
- The Phase 2 supported-request slice is now implemented for controller load preprocessing, probe command normalization, composed result projection setup, the shared category-info path, and the remaining direct runtime callers that previously invoked `QuerySetupBuilder`; fallback-branch removal remains follow-up work.