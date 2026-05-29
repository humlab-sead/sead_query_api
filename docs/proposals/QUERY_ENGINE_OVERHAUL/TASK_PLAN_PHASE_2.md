# Task Plan: Phase 2 - Discrete Facet Parity

## Phase Summary

**Phase:** Phase 2 - Discrete Facet Parity

**Status:** In progress

**Goal**

Reach practical parity for legacy discrete facet content generation.

**Focus**

- widen the composed path across the remaining discrete target families
- cover direct targets, routed targets, view-backed targets, clause-bearing targets, and multi-table targets
- keep validating through focused live probes first, then grouped promotion

**Acceptance Criteria**

- [ ] All in-scope legacy discrete target facets either run on the composed path or are listed on an explicit exception list with a concrete blocker.
- [ ] Discrete facet content produced by the composed path matches legacy output for the validated matrix.
- [ ] No currently validated discrete slice regresses during widening.

## Documentation Targets

- Maintain the working parity inventory in `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`.
- Record durable contract or runtime-boundary changes in `docs/DESIGN.md`.
- Update this task plan in place as discrete support widens or explicit exceptions are accepted.

## Work Breakdown

### Discrete Target Inventory And Exception List

**Objective**

Turn the remaining discrete parity gap into one explicit working list of targets and blockers.

- [ ] Enumerate the in-scope legacy discrete target facets not yet promoted into the supported composed matrix.
- [ ] Group remaining discrete targets by target shape: direct, routed, view-backed, clause-bearing, and multi-table.
- [ ] Classify each remaining target as planned-for-phase support or explicit exception.
- [ ] Record one concrete blocker for every explicit exception.
- [ ] Keep `PARITY_INVENTORY.md` aligned with the current support or exception status.

**Completion Criteria**

- [ ] Every in-scope discrete target is either in the widening backlog or on an explicit exception list.
- [ ] The parity inventory distinguishes supported, in-progress, and excepted discrete targets clearly enough to guide implementation.

### Target-Side Discrete Widening

**Objective**

Implement the remaining target-side support needed for discrete target parity without reopening the Phase 1 contract baseline.

- [ ] Widen direct-target support where the composed path still depends on legacy-only target handling.
- [ ] Widen routed-target support where target join derivation is still missing but can be implemented within the current anchor model.
- [ ] Widen view-backed and multi-table discrete targets where target-side category or join resolution is still incomplete.
- [ ] Preserve explicit fallback behavior for targets that still do not satisfy the current target-side contract.
- [ ] Add or update focused unit tests for every new target-side contract case promoted into support.

**Completion Criteria**

- [ ] Newly supported discrete targets run through the composed path with explicit target-side contracts.
- [ ] Unsupported target-side cases still fail or fall back explicitly rather than producing misleading SQL.

### Predicate-Side Discrete Widening

**Objective**

Extend predicate compatibility only where the source-key and clause contracts remain explicit and testable.

- [ ] Promote remaining discrete predicate families whose source-key expressions can be resolved cleanly on the predicate source table.
- [ ] Promote clause-bearing predicate slices only when clause normalization remains local to the predicate source table.
- [ ] Keep unsupported predicate-side patterns on the explicit exception list until a concrete contract exists.
- [ ] Add or update focused unit tests for each new predicate-side support rule.

**Completion Criteria**

- [ ] Predicate-side widening does not weaken the current explicit fallback boundaries.
- [ ] Every promoted predicate-side rule is covered by focused contract tests.

### Focused Live Promotion

**Objective**

Promote new discrete slices through the same live-validation path already used for the current supported matrix.

- [ ] Add one focused live comparison probe for each newly supported discrete slice in `sead.query.test/LiveTests/FacetLoadService.cs`.
- [ ] Confirm each new slice uses composed facet-content SQL before grouped promotion.
- [ ] Confirm each new slice matches legacy facet content before grouped promotion.
- [ ] Promote validated URIs into `SupportedComposedVisibleAndDiscreteLiveUris` in the same change.
- [ ] Re-run the grouped discrete live regression after each promotion batch.

**Completion Criteria**

- [ ] Each newly supported discrete slice is validated individually before it joins the grouped matrix.
- [ ] The grouped discrete live matrix stays green as support widens.

### Documentation And Phase Exit

**Objective**

Leave one explicit record of what Phase 2 delivered and what it intentionally deferred.

- [ ] Update `PARITY_INVENTORY.md` when a discrete target moves from unsupported or partial to supported.
- [ ] Update `docs/DESIGN.md` when discrete runtime or contract boundaries change.
- [ ] Record explicit exceptions that remain at phase exit, with blockers or deferral reasons.
- [ ] Update this task plan’s progress tracker, validation log, and deliverables as widening lands.

**Completion Criteria**

- [ ] The end-of-phase discrete support surface is visible without reading code diffs.
- [ ] Remaining exceptions are documented as intentional follow-up work rather than accidental gaps.

## Progress Tracker

| Area | Status | Notes |
|---|---|---|
| Discrete target inventory and exception list | In progress | Inventory work showed there was no clean live same-table target-only promotion candidate in the current smoke set; the first promotable target-only slice was the routed `genus:genus` request. |
| Target-side discrete widening | In progress | Same-table target-only discrete requests use an explicit unfiltered anchor query, and routed zero-predicate discrete requests now use legacy-style outer-category overlay on top of composed counts. |
| Predicate-side discrete widening | Not started | Predicate-side widening should stay constrained by the explicit source-key and clause contracts from Phase 1. |
| Focused live promotion | In progress | `genus:genus`, `sites:sites`, `sample_groups:sample_groups`, `data_types:data_types`, and `rdb_systems:rdb_systems` now pass focused composed-path validation and have been promoted into `SupportedComposedVisibleAndDiscreteLiveUris`; continue inventorying the remaining routed target-only slices one focused probe at a time. |
| Documentation and phase exit | In progress | `PARITY_INVENTORY.md` and this plan now reflect the promoted routed target-only batch through `rdb_systems:rdb_systems`; update `docs/DESIGN.md` only when the runtime boundary changes rather than when the validated matrix widens within the same contract. |

## Definition Of Done

- [ ] All Phase 2 acceptance criteria are satisfied.
- [ ] Every in-scope discrete target facet is either supported on the composed path or listed as an explicit exception with a concrete blocker.
- [ ] Discrete composed output matches legacy behavior for the validated matrix.
- [ ] Grouped discrete regression covers the supported discrete matrix without regressions.
- [ ] Focused validation has been run and recorded for each promoted widening batch.
- [ ] `PARITY_INVENTORY.md` and `docs/DESIGN.md` reflect the final Phase 2 support surface.
- [ ] Follow-up work is captured as explicit exceptions or later-phase tasks.

## Validation And Testing

- [x] Run focused unit tests for touched discrete target and predicate contracts.
- [x] Run focused composed-service tests for touched discrete widening slices.
- [x] Run focused live comparison coverage for the promoted `genus:genus` discrete slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyGenusSlice"`.
- [x] Run grouped discrete live regression after promoting `genus:genus` into `SupportedComposedVisibleAndDiscreteLiveUris` using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedVisibleAndDiscreteLiveSlices"`.
- [x] Run focused live comparison coverage for the promoted `data_types:data_types` discrete slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyDataTypesSlice"`.
- [x] Run focused live comparison coverage for the promoted `rdb_systems:rdb_systems` discrete slice using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyRdbSystemsSlice"`.
- [x] Review `PARITY_INVENTORY.md` after the `genus:genus`, `sites:sites`, `sample_groups:sample_groups`, `data_types:data_types`, and `rdb_systems:rdb_systems` promotion batches to confirm support and exception rows remain current.

## Deliverables

| Deliverable | Description | Status | Link |
|---|---|---|---|
| Discrete parity task plan | Execution tracker for Phase 2 widening work | In progress | `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_2.md` |
| Updated discrete parity inventory | Current support and exception status for discrete targets | In progress | `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md` |
| Discrete contract updates | Code-level widening for discrete target and predicate support | In progress | `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs` |
| Focused live promotion coverage | Focused and grouped live coverage for newly promoted discrete slices | In progress | `sead.query.test/LiveTests/FacetLoadService.cs` |
| Durable architecture updates | Discrete runtime and contract boundary updates | In progress | `docs/DESIGN.md` |

## Scope

**In scope**

- widening the composed runtime across the remaining in-scope discrete target families
- explicit exception handling for discrete targets that still cannot satisfy the current composed contracts
- focused unit, composed-service, and live comparison validation for promoted discrete slices
- parity-inventory and design-document updates required to keep the supported discrete surface explicit

**Out of scope**

- non-discrete facet-family parity beyond discrete-related regression protection
- result-set parity or final result projection work
- full retirement of legacy fallback outside the discrete parity surface
- broad route or anchor redesign that would reopen the Phase 1 contract baseline

## Risks And Mitigations

| Risk | Mitigation |
|---|---|
| Remaining discrete targets depend on undocumented legacy target-side joins. | Promote one focused live slice at a time and keep explicit exception rows for anything still unclear. |
| Predicate widening weakens the explicit fallback boundary. | Add unit tests before promotion and keep unsupported predicate-side cases on the exception list until the contract is explicit. |
| Grouped live regression becomes stale as widening lands. | Promote new URIs into the grouped matrix and parity inventory in the same change that adds support. |
| Discrete parity work expands into non-discrete or result-set work. | Keep Phase 2 limited to discrete facet content parity and defer other runtime surfaces to later phases. |

## Follow-Up Items

- [ ] Hand off remaining discrete exceptions to the next widening batch or record them as later-phase work when Phase 2 closes.
- [ ] Revisit whether any explicit Phase 2 exceptions should instead be handled in Phase 3 or later cutover work.
- [ ] Decide whether a broader repository-level regression command is needed once the discrete parity matrix is materially larger than today.
- [ ] Select and probe the next routed target-only discrete live slice after `rdb_systems:rdb_systems`, then continue inventorying the remaining target-only targets once that result is known.
