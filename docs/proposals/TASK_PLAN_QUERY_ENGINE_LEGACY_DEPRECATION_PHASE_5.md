# Task Plan: Query Engine Legacy Deprecation - Phase 5 Remove Or Archive Legacy SQL Assets

## Phase Summary

**Phase:** Phase 5 - Remove Or Archive Legacy SQL Assets

**Status:** Not started

**Goal**

Stop treating any legacy SQL asset as authoritative for runtime or operations.

**Focus**

- remove or archive SQL assets that remain live only because of the legacy runtime
- resolve the remaining importer and schema compatibility surface around `facet.facet_template` and imported `sql_override`
- replace or retire `scripts/prepare-phase5-facet-runtime-schema.sql` as an authoritative operational prerequisite
- align operations, testing, and deprecation inventory documents with the post-legacy SQL baseline

**Acceptance Criteria**

- [ ] No supported runtime or supported operational workflow depends on legacy SQL assets.
- [ ] The status of imported SQL templates and schema-prep assets is explicit and documented.
- [ ] Historical SQL retained under `deprecated/` is clearly archived and non-authoritative.
- [ ] The maintained deprecation inventory and operations guidance match the shipped SQL and schema baseline.

## Work Breakdown

### Classify And Archive Deprecated SQL Files

**Objective**

Close the status of the named `deprecated/*.sql` assets so they no longer read as potentially live material.

- [ ] Re-check the repository and maintained docs for active references to the named `deprecated/*.sql` files.
- [ ] Move, archive, or otherwise mark the named legacy SQL files as explicitly historical and non-authoritative.
- [ ] Update the maintained inventory to record the landed archival disposition.

**Completion Criteria**

- [ ] The named `deprecated/*.sql` assets are either archived clearly or removed, and no maintained document implies that they are still authoritative.

### Resolve Imported SQL Template Compatibility

**Objective**

Decide whether imported `sql_override` content and `facet.facet_template` remain part of the supported model or can now be removed from active runtime and importer expectations.

- [ ] Re-check the non-archived request path, importer path, and schema mapping for live `facet.facet_template` dependence.
- [ ] Remove or narrow importer and runtime support for `sql_override` and `facet.facet_template` if no supported request path depends on them.
- [ ] If removal is not yet safe, document the exact remaining compatibility boundary and move the unresolved work into a smaller follow-up scope.

**Completion Criteria**

- [ ] The repository has an explicit post-Phase-5 disposition for `sql_override` and `facet.facet_template`: removed, retained with a narrow reason, or moved to a clearly bounded follow-up.

### Replace Or Retire Phase 5 Schema Prep Asset

**Objective**

Stop relying on `scripts/prepare-phase5-facet-runtime-schema.sql` as an ambiguous legacy-era operational asset.

- [ ] Confirm whether supported target environments already carry the required `facet` runtime tables and provenance tables.
- [ ] Replace the script with a durable baseline approach or archive it if it is no longer required by supported operations.
- [ ] Update operations guidance so the authoritative deployment path no longer depends on ambiguous legacy SQL preparation.

**Completion Criteria**

- [ ] The status of `scripts/prepare-phase5-facet-runtime-schema.sql` is explicit, and supported operations no longer treat a legacy SQL prep step as an open-ended authoritative dependency.

### Close The Phase With Documentation And Validation Alignment

**Objective**

Leave the SQL baseline, operational guidance, and deprecation inventory internally consistent after the cleanup lands.

- [ ] Update `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` for the landed SQL dispositions.
- [ ] Update `docs/OPERATIONS.md` and any other maintained runbook material affected by the SQL-asset cleanup.
- [ ] Run focused validation for any touched importer, runtime-schema, or operational script changes and rerun `dotnet test sead.query.test/sead.query.test.csproj` before closing the phase.

**Completion Criteria**

- [ ] The maintained inventory, operations guidance, and repository validation all reflect the post-Phase-5 SQL baseline.

## Progress Tracker

| Area | Status | Notes |
|---|---|---|
| Classify and archive deprecated SQL files | Not started | Phase 1 inventory already classifies the named `deprecated/*.sql` files as archived or dead, but the repository still needs the final archival action. |
| Resolve imported SQL template compatibility | Not started | `facet.facet_template` and imported `sql_override` are currently treated as importer or schema compatibility only. |
| Replace or retire Phase 5 schema prep asset | Not started | `scripts/prepare-phase5-facet-runtime-schema.sql` is still documented as an active operational schema-prep asset in `docs/OPERATIONS.md`. |
| Close the phase with documentation and validation alignment | Not started | Inventory, operations, and validation need to move together once the SQL cleanup lands. |

## Definition Of Done

- [ ] All Phase 5 acceptance criteria are satisfied.
- [ ] Named legacy SQL files under `deprecated/` are clearly archived or removed.
- [ ] The post-Phase-5 disposition of `facet.facet_template` and imported `sql_override` is explicit.
- [ ] Supported operations no longer depend on an ambiguous legacy schema-prep asset.
- [ ] The maintained inventory and operations documentation reflect the landed SQL baseline.
- [ ] `dotnet test sead.query.test/sead.query.test.csproj` passes after the final Phase 5 slice.

## Validation And Testing

- [ ] Run focused validation for each touched importer, schema, or operational-script slice.
- [ ] Re-check active references for the named SQL assets after each removal or archival step.
- [ ] Run `dotnet test sead.query.test/sead.query.test.csproj` before closing the phase.

## Deliverables

| Deliverable | Description | Status | Link |
|---|---|---|---|
| Phase 5 task plan | Execution tracker for legacy SQL asset cleanup | Not started | `docs/proposals/TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_5.md` |
| SQL asset disposition update | Final archived, removed, or retained status for named SQL assets and importer compatibility surfaces | Not started | `docs/proposals/QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md` |
| Operations alignment update | Post-cleanup operational guidance for runtime schema and deployment prerequisites | Not started | `docs/OPERATIONS.md` |

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