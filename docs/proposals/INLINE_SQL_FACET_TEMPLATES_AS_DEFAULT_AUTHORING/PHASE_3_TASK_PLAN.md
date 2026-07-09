# Task Plan: Phase 3 - Non-Discrete Driver Slices

## Phase Summary

- Goal: extend the inline SQL contract to non-discrete families and restore composed facet-content support for intersect and geopolygon in this change request
- Focus: routed range support, denormalized or function-backed range support, intersect restoration, geopolygon restoration, and non-discrete validation guards
- Acceptance Criteria:
  - [ ] the non-discrete compiler path supports at least one routed range slice and one denormalized or function-backed range slice through the inline-template contract
  - [ ] composed facet-content handling for intersect targets is restored and validated
  - [ ] composed facet-content handling for geopolygon targets is restored and validated
  - [ ] placeholder validation and output-shape validation exist for the implemented non-discrete driver slices
  - [ ] any remaining follow-up is scoped to optimization or migration breadth, not restoration of these facet types

## Execution Rules

- [ ] treat each unchecked checkbox as one task and complete only that task plus minimal supporting edits
- [ ] do not continue to the next checkbox after completing the current one
- [ ] do not widen scope into adjacent work unless required by the current checkbox
- [ ] record discovered related work as follow-up items, not silent scope expansion
- [ ] prefer focused tests proving the current checkbox over broad refactors
- [ ] after each completed task, update checkbox state, progress notes, validation evidence, and deferred follow-up

## Work Breakdown

### 1. Range Compiler Extension

Objective: implement non-discrete range template execution in composed facet-content and result-handoff paths for routed and denormalized drivers.

- [x] implement range template lookup and rendering path parity with existing discrete precedence
- [x] implement supported range placeholders for inline-template rendering and fail-fast handling for unsupported placeholders
- [x] implement routed projection execution for geochronology base-anchor plus target projection
- [x] implement denormalized or function-backed range execution for tbl_denormalized_measured_values_33_0
- [x] add compiler validation for invalid range output-shape combinations in implemented slices

Completion criteria: geochronology and tbl_denormalized_measured_values_33_0 execute through template-driven range SQL with explicit validation on unsupported placeholder and output-shape combinations.

Location: composer services and template metadata handling in `sead.query.composer`

Dependencies: Phase 2 discrete template infrastructure is complete; runtime facet schema baseline alignment may be required for full live integration validation.

### 2. Intersect Restoration

Objective: restore composed facet-content handling for intersect facets through inline-template contract.

- [ ] implement intersect template lookup and placeholder rendering path
- [ ] restore intersect composed facet-content query generation for analysis_entity_ages
- [ ] validate intersect SQL structure against expected route and filter semantics
- [ ] add focused unit tests for intersect template rendering and diagnostics
- [ ] add focused integration test for analysis_entity_ages intersect slice

Completion criteria: analysis_entity_ages composed intersect flow executes via inline-template contract with focused unit and integration evidence.

Location: intersect handler and shared composed facet-content support in `sead.query.composer`, tests in `sead.query.test`

Dependencies: range placeholder and rendering primitives from Work Area 1.

### 3. Geopolygon Restoration

Objective: restore composed facet-content geopolygon handling through inline-template contract.

- [ ] implement geopolygon template lookup and placeholder rendering path
- [ ] restore geopolygon composed facet-content query generation for sites_polygon
- [ ] validate polygon filter placement and anchor and route semantics in generated SQL
- [ ] add focused unit tests for geopolygon template rendering and diagnostics
- [ ] add focused integration test for sites_polygon geopolygon slice

Completion criteria: sites_polygon geopolygon flow executes via inline-template contract with focused unit and integration evidence.

Location: geopolygon handler and shared composed facet-content support in `sead.query.composer`, tests in `sead.query.test`

Dependencies: shared placeholder diagnostics from Work Area 1.

### 4. Driver Validation, Parity, and Guarded Follow-up

Objective: validate all four Phase 3 drivers and ensure remaining gaps are explicitly scoped forward.

- [ ] run focused parity-style checks for geochronology and tbl_denormalized_measured_values_33_0 against legacy relational behavior
- [ ] run focused validation for analysis_entity_ages and sites_polygon composed facet-content restoration
- [ ] document semantic discrepancies or blocked slices with explicit guards
- [ ] confirm relational fallback remains unchanged for non-migrated non-discrete facets
- [ ] classify unresolved items as optimization or migration-breadth follow-up for Phase 4, not restoration debt

Completion criteria: all Phase 3 driver slices are validated as parity-clean or explicitly documented and guarded, and remaining work is clearly scoped to Phase 4 hardening and backfill.

Location: focused integration and diagnostics tests in `sead.query.test`, phase evidence in `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/PHASE_3_TASK_PLAN.md`

Dependencies: Work Areas 1-3.

## Progress Tracker

| Area                                    | Status      | Notes                                                                                                                                                                                                       |
|-----------------------------------------|-------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Range compiler extension                | Complete    | Added range template precedence in composed filter factory, routed geochronology + function-backed measured-values templates, and fail-fast placeholder/output-shape validation with focused unit coverage. |
| Intersect restoration                   | Not started | Driver slice: analysis_entity_ages                                                                                                                                                                          |
| Geopolygon restoration                  | Not started | Driver slice: sites_polygon                                                                                                                                                                                 |
| Driver validation and guarded follow-up | Not started | Includes parity checks, blockers, and Phase 4 scoping                                                                                                                                                       |

## Definition Of Done

- [ ] all Phase 3 acceptance criteria are covered by completed work areas and recorded evidence
- [x] routed range inline-template path is validated on geochronology
- [x] denormalized or function-backed range inline-template path is validated on tbl_denormalized_measured_values_33_0
- [ ] intersect composed facet-content is restored and validated on analysis_entity_ages
- [ ] geopolygon composed facet-content is restored and validated on sites_polygon
- [x] placeholder and output-shape validation is implemented and tested for Phase 3 non-discrete slices
- [ ] unchanged facets continue to use explicit fallback behavior where template metadata is absent
- [ ] blocked or deferred behavior is explicitly documented and guarded by focused tests
- [ ] unresolved items are categorized as optimization or migration breadth and queued to Phase 4
- [ ] task-level evidence and progress tracker are updated after each completed checkbox

## Validation And Testing

- [ ] run configuration validation after non-discrete template authoring updates:
  - `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/facet_configuration.yml`
- [x] run focused unit coverage for composed template rendering and diagnostics in non-discrete handlers
- [ ] run focused integration slices for geochronology, tbl_denormalized_measured_values_33_0, analysis_entity_ages, and sites_polygon
- [ ] run focused parity checks against legacy relational path for routed and denormalized range drivers
- [ ] run targeted regression for unaffected facet families to confirm fallback behavior stability
- [ ] record any environment blockers (for example runtime facet schema alignment) directly in validation evidence with explicit blocked status

## Deliverables

| Deliverable                     | Description                                                            | Status      | Link                                                                                  |
|---------------------------------|------------------------------------------------------------------------|-------------|---------------------------------------------------------------------------------------|
| Phase 3 task plan               | Active execution tracker for non-discrete driver slices                | Complete    | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/PHASE_3_TASK_PLAN.md` |
| Range compiler support          | Inline SQL template execution for routed and denormalized range slices | Complete    | `sead.query.composer`                                                                 |
| Intersect restoration evidence  | Unit and focused integration proof for analysis_entity_ages            | Not started | `sead.query.test`                                                                     |
| Geopolygon restoration evidence | Unit and focused integration proof for sites_polygon                   | Not started | `sead.query.test`                                                                     |
| Validation evidence summary     | Focused parity and blocker and deferred documentation                  | Not started | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/PHASE_3_TASK_PLAN.md` |

## Scope

In scope:
- non-discrete compiler support for range drivers in this phase
- intersect composed facet-content restoration in this phase
- geopolygon composed facet-content restoration in this phase
- focused validation and parity evidence for the four approved Phase 3 drivers
- explicit documentation of blocked and deferred behavior when needed

Out of scope:
- migration of map_result and result_datasets template-key path (Phase 4)
- broad catalog-wide facet migration
- release scheduling, rollout governance, and broad performance optimization
- frontend changes

## Risks And Mitigations

- Risk: runtime facet-schema baseline mismatch blocks live import and integration validation.
  - Mitigation: treat schema alignment as early validation prerequisite; if blocked, keep focused unit coverage and record explicit blocked integration status.
- Risk: non-discrete placeholder drift causes silent SQL-shape differences.
  - Mitigation: fail-fast placeholder validation and focused assertions on rendered SQL fragments per driver slice.
- Risk: restoration work expands into broad migration.
  - Mitigation: enforce driver-slice-only execution and queue adjacent facet migration to Phase 4.

## Open Questions

- [ ] should runtime facet-schema alignment be executed as a first explicit checkbox in this phase or tracked as an external prerequisite when not owned by this stream?
- [ ] do any additional route-template definitions need to be promoted for non-discrete slices beyond current driver routes, or should such additions be deferred unless a driver slice directly requires them?

## Assumptions

- [ ] Phase 1 and Phase 2 contracts remain authoritative for lookup precedence and relational fallback behavior
- [ ] Phase 3 completes restoration for the four listed drivers only
- [ ] Phase 4 owns retained result-shape migration breadth and controlled backfill
