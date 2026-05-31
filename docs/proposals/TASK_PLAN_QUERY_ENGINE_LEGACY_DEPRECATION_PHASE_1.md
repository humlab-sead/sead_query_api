# Task Plan: Query Engine Legacy Deprecation - Phase 1 Inventory Live Legacy Surfaces

## Phase Summary

**Phase:** Phase 1 - Inventory Live Legacy Surfaces

**Status:** Done

**Goal**

Establish the exact set of legacy runtime and SQL assets that still matter to the supported system.

**Focus**

- inventory the live fallback and hybrid code paths centered on `FacetContentService`, `CategoryCountService`, `QuerySetupBuilder`, `LegacyResultProjectionHandoffBuilder`, `ComposedResultProjectionHandoffBuilder`, `BogusPickService`, and `CategoryInfoService`
- inventory legacy DI surfaces, including facet-type plugin registrations that still participate in authoritative execution
- inventory SQL assets that are still operationally relevant, including `scripts/prepare-phase5-facet-runtime-schema.sql`, imported `sql_override` content written to `facet.facet_template`, and the SQL files under `deprecated/`
- classify each inventoried item as live, transitional, archived, or dead

**Acceptance Criteria**

- [x] The repository has a reviewed inventory of live legacy runtime classes, registrations, and helper services.
- [x] The repository has a reviewed inventory of still-authoritative or still-ambiguous SQL assets.
- [x] Each inventoried item has a recorded disposition: migrate, remove, archive, or keep temporarily with an explicit reason.

## Work Breakdown

### Runtime Fallback And Hybrid Surface Inventory

**Objective**

Document the live runtime classes that still implement legacy execution or still keep the system hybrid.

- [x] Inventory `FacetContentService`, `CategoryCountService`, and `ComposedFacetContentService` with their current runtime role, fallback or guard condition, and expected deprecation outcome.
- [x] Inventory `LegacyResultProjectionHandoffBuilder`, `ComposedResultProjectionHandoffBuilder`, and `ResultService` with their current handoff role, fallback boundary, and expected deprecation outcome.
- [x] Inventory `QuerySetupBuilder`, `BogusPickService`, and `CategoryInfoService` as hybrid helper surfaces that still depend on legacy query-building.
- [x] Record the request entry points or supporting flows that still reach each inventoried class.
- [x] Capture one disposition candidate per class: migrate, remove, archive, or keep temporarily.

**Completion Criteria**

- [x] The Phase 1 inventory names each live runtime legacy or hybrid class, its role, its trigger or call path, and its disposition candidate.

### DI And Plugin Registration Inventory

**Objective**

Identify the registration surfaces that keep legacy execution authoritative at runtime.

- [x] Inventory the relevant legacy and hybrid registrations in `sead.query.api/Dependency.cs`.
- [x] Inventory the discrete, range, intersect, and geo-polygon facet plugin registrations that still participate in authoritative runtime behavior.
- [x] Record which registrations are required for current supported behavior, which exist only for fallback compatibility, and which look removable after cutover.
- [x] Record any registration dependencies that would block later removal phases.

**Completion Criteria**

- [x] The Phase 1 inventory includes a reviewed list of legacy or hybrid DI and plugin surfaces with a clear post-cutover disposition.

### SQL Asset Inventory And Classification

**Objective**

Classify the SQL assets that may still be authoritative for runtime, operational, or migration behavior.

- [x] Review `scripts/prepare-phase5-facet-runtime-schema.sql` and record why it is still active, transitional, or removable.
- [x] Review the documented `sql_override` and anchor-level override path that writes to `facet.facet_template`, and record whether that runtime SQL surface is still authoritative.
- [x] Inventory the top-level SQL files under `deprecated/` and classify each as archived, ambiguous, or still live.
- [x] Cross-check docs and operational guidance to confirm whether any inventoried SQL asset is still described as required for setup, runtime readiness, migration, or rollback.
- [x] Record one disposition candidate per SQL asset or SQL asset family: migrate, remove, archive, or keep temporarily.

**Completion Criteria**

- [x] The Phase 1 inventory includes a reviewed SQL asset classification with explicit evidence for each asset marked live or ambiguous.

### Inventory Publication And Review Closure

**Objective**

Publish the inventory in one maintained place and close the phase with an explicit disposition record.

- [x] Create or update one maintained inventory document under `docs/proposals/` for the Phase 1 runtime and SQL findings.
- [x] Record classification fields consistently for all inventoried items: current role, evidence, disposition, and removal prerequisite if any.
- [x] Update the deprecation proposal and phase plan references if the final inventory changes the initial Phase 1 inventory boundary.
- [x] Review the completed inventory against the Phase 1 acceptance criteria and capture any follow-up work needed for Phase 2.

**Completion Criteria**

- [x] The Phase 1 inventory is published in one maintained document and is sufficient to drive Phase 2 and later removal phases.

## Progress Tracker

| Area                                          | Status | Notes                                                                                                                                                             |
|-----------------------------------------------|--------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Runtime fallback and hybrid surface inventory | Done   | Published in `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` with current roles, reachability, classifications, and disposition candidates. |
| DI and plugin registration inventory          | Done   | Published in `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` with dependency and post-cutover split requirements.                           |
| SQL asset inventory and classification        | Done   | Published in `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` with explicit live, transitional, archived, and dead classifications.          |
| Inventory publication and review closure      | Done   | Proposal, phase plan, and task plan now link to the maintained Phase 1 inventory.                                                                                 |

## Definition Of Done

- [x] All Phase 1 acceptance criteria are satisfied.
- [x] The inventory covers the named runtime classes, helper services, DI surfaces, and SQL assets in scope for Phase 1.
- [x] Each inventoried item has a recorded disposition and an explicit reason.
- [x] Ambiguous SQL assets are either resolved or recorded as explicit follow-up work.
- [x] The maintained inventory document is linked from the active deprecation planning material.
- [x] Validation and review are recorded so the inventory can be used as the baseline for Phase 2.

## Validation And Testing

- [x] Review the completed inventory against the named runtime and DI surfaces in `QUERY_ENGIVE_LEGACY_DEPRECATION.md` and `QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md`.
- [x] Review `docs/OPERATIONS.md` and the route-configuration proposal material to confirm whether any inventoried SQL asset is still operationally authoritative.
- [x] Run workspace diagnostics on the new or updated Phase 1 inventory and planning documents.
- [x] If Phase 1 work changes code or runtime wiring rather than only documenting inventory, run the narrowest affected validation for that touched slice before closing the phase.

## Deliverables

| Deliverable                              | Description                                                                 | Status | Link                                                                                                                   |
|------------------------------------------|-----------------------------------------------------------------------------|--------|------------------------------------------------------------------------------------------------------------------------|
| Phase 1 task plan                        | Execution tracker for legacy runtime and SQL inventory work                 | Done   | `docs/proposals/TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1.md`                                                  |
| Phase 1 inventory document               | Maintained inventory of legacy runtime classes, DI surfaces, and SQL assets | Done   | `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md`                                                  |
| Proposal and phase-plan alignment update | Any boundary corrections needed after the inventory is reviewed             | Done   | `docs/proposals/QUERY_ENGIVE_LEGACY_DEPRECATION.md` and `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md` |

## Scope

**In scope**

- live legacy runtime classes and helper services that still affect supported behavior
- DI and plugin registration surfaces that still keep legacy execution reachable
- SQL assets that may still be authoritative for runtime, tests, setup, migration, or rollback
- one explicit disposition record for each inventoried item

**Out of scope**

- removing fallback behavior or deleting legacy code during Phase 1
- broad refactoring of composer runtime code unrelated to the inventory
- release sequencing or deployment ownership
- cleanup of clearly archived historical material that Phase 1 confirms is already non-authoritative

## Risks And Mitigations

| Risk                                                                                                                | Mitigation                                                                                            |
|---------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------|
| Legacy behavior may still be depended on indirectly through helper services or registrations that are easy to miss. | Inventory runtime classes and DI/plugin surfaces together rather than as separate undocumented lists. |
| SQL assets in `deprecated/` may look historical while still being referenced implicitly in docs or workflows.       | Cross-check SQL files against docs and operational guidance before classifying them as dead.          |
| Phase 1 may produce an inventory that lists items but does not make removal decisions actionable.                   | Require a disposition and a removal prerequisite field for every inventoried item.                    |

## Assumptions

- `QUERY_ENGIVE_LEGACY_DEPRECATION.md` and `QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md` are the current authoritative planning baseline for this work.
- Phase 1 is primarily an inventory and classification phase, not a code-removal phase.
- The final location of the maintained inventory document may be chosen during Phase 1, but it should remain under `docs/proposals/` alongside the active deprecation planning docs.