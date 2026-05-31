# Wrap Up: Phase 2 Session

## Resume Point

- Branch: `query-engine-overhaul`
- HEAD: `b02c30c`
- Checkpoint commit: `feat(composer): promote routed zero-predicate discrete slices`
- Current worktree state: one uncommitted planning change in `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_2.md`

## What Landed In This Session

- Added composed support for routed zero-predicate discrete facet-content loads in `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs`.
- Kept composed count SQL as the active path, but overlaid legacy-style discrete outer category rows so zero-count categories are preserved where legacy exposes them.
- Added focused unit coverage for zero-predicate same-table and routed discrete behavior in `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`.
- Promoted these target-only routed discrete slices into the grouped visible/discrete composed live matrix in `sead.query.test/LiveTests/FacetLoadService.cs`:
  - `genus:genus`
  - `sites:sites`
  - `sample_groups:sample_groups`
- Updated durable and tracking docs:
  - `docs/DESIGN.md`
  - `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`
  - `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_2.md`

## Validation That Passed

- `dotnet test sead.query.test/sead.query.test.csproj --filter FullyQualifiedName~SQT.UnitTests.QueryComposer.Services.ComposedFacetContentServiceTests`
- `dotnet test sead.query.test/sead.query.test.csproj --filter FullyQualifiedName~FacetContentService_ComposedTargetOnlyGenusSlice`
- `dotnet test sead.query.test/sead.query.test.csproj --filter FullyQualifiedName~FacetContentService_ComposedTargetOnlySitesSlice`
- `dotnet test sead.query.test/sead.query.test.csproj --filter FullyQualifiedName~FacetContentService_ComposedTargetOnlySampleGroupsSlice`
- `dotnet test sead.query.test/sead.query.test.csproj --filter FullyQualifiedName~FacetContentService_ComposedSupportedVisibleAndDiscreteLiveSlices`

## Current Known Warnings

- `NU1903` for AutoMapper in `sead.query.test.csproj`
- nullable warning in `sead.query.composer/Utility/TemplateLoader.cs`
- xUnit non-serializable-data warnings in unrelated tests

None of these warnings blocked the Phase 2 slice work above.

## Current Tracking State

- `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_2.md` is now the active execution tracker for Phase 2.
- The tracker reflects that there was no clean live same-table target-only promotion candidate in the current smoke set.
- The routed zero-predicate contract is now proven for at least three slices: `genus:genus`, `sites:sites`, and `sample_groups:sample_groups`.
- The next explicitly selected candidate is `data_types:data_types`.

## Recommended Next Action

Continue with `data_types:data_types` using the same narrow sequence:

1. Add focused live probes in `sead.query.test/LiveTests/FacetLoadService.cs`.
2. Run `dotnet test sead.query.test/sead.query.test.csproj --filter FullyQualifiedName~FacetContentService_ComposedTargetOnlyDataTypesSlice`.
3. If that passes, add `data_types:data_types` to `SupportedComposedVisibleAndDiscreteLiveUris`.
4. Re-run `dotnet test sead.query.test/sead.query.test.csproj --filter FullyQualifiedName~FacetContentService_ComposedSupportedVisibleAndDiscreteLiveSlices`.
5. Update `PARITY_INVENTORY.md` and `TASK_PLAN_PHASE_2.md` in the same change.

## Good Starting Files For Resume

- `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_2.md`
- `sead.query.test/LiveTests/FacetLoadService.cs`
- `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs`
- `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`

## Notes On The Current Dirty State

- The only post-commit change right now is the planning update in `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_2.md` that pins `data_types:data_types` as the next target-only routed probe.
- If you want a fully clean restart point later, either commit that one-file plan update or keep it as the first local context read when resuming.