# Task Plan: Query Engine Legacy Deprecation - Phase 5 Remove Or Archive Legacy SQL Assets

## Phase Summary

**Phase:** Phase 5 - Remove Or Archive Legacy SQL Assets

**Status:** Done

**Goal**

Stop treating any legacy SQL asset as authoritative for runtime or operations.

**Focus**

- archive historical SQL assets that remained at the top level of `deprecated/`
- remove the remaining importer and schema compatibility surface around `facet.facet_template` and imported `sql_override`
- replace `scripts/prepare-phase5-facet-runtime-schema.sql` with a current runtime schema baseline asset
- align operations, testing, and deprecation inventory documents with the post-legacy SQL baseline

**Acceptance Criteria**

- [x] No supported runtime or supported operational workflow depends on legacy SQL assets.
- [x] The status of imported SQL templates and schema-prep assets is explicit and documented.
- [x] Historical SQL retained under `deprecated/` is clearly archived and non-authoritative.
- [x] The maintained deprecation inventory and operations guidance match the shipped SQL and schema baseline.

## Work Breakdown

### Classify And Archive Deprecated SQL Files

**Objective**

Close the status of the named `deprecated/*.sql` assets so they no longer read as potentially live material.

- [x] Re-check the repository and maintained docs for active references to the named `deprecated/*.sql` files.
- [x] Move, archive, or otherwise mark the named legacy SQL files as explicitly historical and non-authoritative.
- [x] Update the maintained inventory to record the landed archival disposition.

**Completion Criteria**

- [x] The named `deprecated/*.sql` assets are either archived clearly or removed, and no maintained document implies that they are still authoritative.

### Resolve Imported SQL Template Compatibility

**Objective**

Decide whether imported `sql_override` content and `facet.facet_template` remain part of the supported model or can now be removed from active runtime and importer expectations.

- [x] Re-check the non-archived request path, importer path, and schema mapping for live `facet.facet_template` dependence.
- [x] Remove or narrow importer and runtime support for `sql_override` and `facet.facet_template` if no supported request path depends on them.
- [x] If removal is not yet safe, document the exact remaining compatibility boundary and move the unresolved work into a smaller follow-up scope.

**Completion Criteria**

- [x] The repository has an explicit post-Phase-5 disposition for `sql_override` and `facet.facet_template`: removed, retained with a narrow reason, or moved to a clearly bounded follow-up.

### Replace Or Retire Phase 5 Schema Prep Asset

**Objective**

Stop relying on `scripts/prepare-phase5-facet-runtime-schema.sql` as an ambiguous legacy-era operational asset.

- [x] Confirm whether supported target environments already carry the required `facet` runtime tables and provenance tables.
- [x] Replace the script with a durable baseline approach or archive it if it is no longer required by supported operations.
- [x] Update operations guidance so the authoritative deployment path no longer depends on ambiguous legacy SQL preparation.

**Completion Criteria**

- [x] The status of `scripts/prepare-phase5-facet-runtime-schema.sql` is explicit, and supported operations no longer treat a legacy SQL prep step as an open-ended authoritative dependency.

### Close The Phase With Documentation And Validation Alignment

**Objective**

Leave the SQL baseline, operational guidance, and deprecation inventory internally consistent after the cleanup lands.

- [x] Update `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` for the landed SQL dispositions.
- [x] Update `docs/OPERATIONS.md` and any other maintained runbook material affected by the SQL-asset cleanup.
- [x] Run focused validation for any touched importer, runtime-schema, or operational script changes and rerun `dotnet test sead.query.test/sead.query.test.csproj` before closing the phase.

**Completion Criteria**

- [x] The maintained inventory, operations guidance, and repository validation all reflect the post-Phase-5 SQL baseline.

## Progress Tracker

| Area | Status | Notes |
|---|---|---|
| Classify and archive deprecated SQL files | Done | The named top-level legacy SQL files now live under `deprecated/archive/` and no maintained doc treats them as authoritative. |
| Resolve imported SQL template compatibility | Done | `facet.facet_template` and imported `sql_override` were removed from the supported model, and import validation now rejects non-empty `sql_override`. |
| Replace or retire Phase 5 schema prep asset | Done | Supported operations now use `scripts/prepare-facet-runtime-schema.sql` via `make prepare-facet-runtime-schema`. |
| Close the phase with documentation and validation alignment | Done | Documentation is aligned, the focused importer slice passed with 13 tests, and the full test project passed with 1476 tests total, 1422 passed, 54 skipped, and 0 failed. |

## Definition Of Done

- [x] All Phase 5 acceptance criteria are satisfied.
- [x] Named legacy SQL files under `deprecated/` are clearly archived or removed.
- [x] The post-Phase-5 disposition of `facet.facet_template` and imported `sql_override` is explicit.
- [x] Supported operations no longer depend on an ambiguous legacy schema-prep asset.
- [x] The maintained inventory and operations documentation reflect the landed SQL baseline.
- [x] `dotnet test sead.query.test/sead.query.test.csproj` passes after the final Phase 5 slice.

## Validation And Testing

- [x] Run focused validation for each touched importer, schema, or operational-script slice.
- [x] Re-check active references for the named SQL assets after each removal or archival step.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj` before closing the phase.

## Deliverables

| Deliverable | Description | Status | Link |
|---|---|---|---|
| Phase 5 task plan | Execution tracker for legacy SQL asset cleanup | Done | `docs/proposals/TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_5.md` |
| SQL asset disposition update | Final archived, removed, or retained status for named SQL assets and importer compatibility surfaces | Done | `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` |
| Operations alignment update | Post-cleanup operational guidance for runtime schema and deployment prerequisites | Done | `docs/OPERATIONS.md` |

## Scope

**In scope**

- cleanup or archival of named legacy SQL assets and scripts
- importer and schema compatibility work tied to `facet.facet_template` and imported `sql_override`
- operations documentation and validation updates required by the SQL cleanup

**Out of scope**

- unrelated composer runtime refactors
- frontend or API contract changes
- deployment scheduling or release workflow changes

## Risks And Mitigations

| Risk | Mitigation |
|---|---|
| A SQL asset that appears archival may still be required by an undocumented operational workflow. | Re-check maintained docs, script references, and deployment-oriented material before archiving or deleting each asset. |
| `facet.facet_template` may still matter indirectly through importer or schema compatibility assumptions. | Verify request-path reachability and importer behavior separately before removing schema or importer support. |
| Operations guidance may drift from the actual supported schema baseline. | Update `docs/OPERATIONS.md` and the maintained inventory in the same slice as each operational SQL disposition change. |

## Assumptions

- Phase 4 has removed the retained legacy runtime implementation surfaces, so Phase 5 starts from a composer-only execution baseline.
- The maintained Phase 1 inventory is the current source of truth for which SQL assets remain live, transitional, archived, or dead.
- If an SQL surface cannot yet be removed safely, the phase should narrow and document the exact remaining compatibility reason rather than hiding it behind a broad legacy label.