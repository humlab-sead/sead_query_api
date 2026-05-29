# Task Plan: Phase 1 - Contract And Coverage Baseline

## Phase Summary

**Phase:** Phase 1 - Contract And Coverage Baseline

**Status:** In progress

**Goal**

Lock down the contracts that the rest of the migration depends on and establish a parity inventory against the legacy engine.

**Focus**

- stabilize anchor, route, and facet-resolver contracts
- keep the current composed slice reliable while widening continues
- create and maintain a parity inventory of legacy capabilities, grouped by facet family and result shape
- make unsupported requests explicit instead of ambiguous

**Acceptance Criteria**

- [x] Route, anchor, and composed-query contracts are documented and reflected in compiled code.
- [x] The branch has one maintained parity inventory for legacy facet families and result paths.
- [x] Grouped regression covers all currently supported composed slices.
- [x] Unsupported composed requests fail or fall back explicitly.

## Documentation Targets

- Store stable contract documentation and unsupported-request semantics in `docs/DESIGN.md`.
- Record long-lived requirement constraints that emerge from this phase in `docs/REQUIREMENTS.md`.
- Maintain the working parity inventory in `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`.

## Work Breakdown

### Contract Baseline

**Objective**

Define and stabilize the route, anchor, facet-resolver, and composed-query contracts that downstream migration work depends on.

- [x] Identify the current route, anchor, facet-resolver, and composed-query contract surfaces.
- [x] Document expected inputs, outputs, invariants, and failure modes in `docs/DESIGN.md`.
- [x] Mark ambiguous or unstable behavior explicitly.
- [x] Update compiled types or interfaces to match the documented contracts.
- [x] Add or update tests that protect the documented contract boundaries.

**Completion Criteria**

- [x] Contract documentation exists in the agreed project location.
- [x] Code-level contracts match the documentation.
- [x] Contract-focused tests compile and pass.
- [x] Known ambiguities are either resolved or recorded as explicit follow-up items.

### Supported Slice Regression

**Objective**

Keep the currently supported composed slice stable while contract work and widening continue.

- [x] List the currently supported composed slices.
- [x] Identify existing regression coverage for each supported slice.
- [x] Add missing grouped regression coverage for supported slices.
- [x] Organize coverage consistently by facet family, route shape, or result shape.
- [x] Confirm unsupported combinations do not silently produce misleading results.
- [x] Fix regressions uncovered during contract stabilization.

**Completion Criteria**

- [x] Every currently supported composed slice is represented in grouped regression coverage.
- [x] Regression tests are named and organized consistently.
- [x] Current composed behavior is preserved for the supported slice.
- [x] Test failures clearly identify the changed slice or contract.

### Legacy Parity Inventory

**Objective**

Create one maintained inventory of legacy capabilities for migration tracking.

- [x] Identify legacy facet families, result paths, and result shapes.
- [x] Group legacy capabilities by facet family and result shape.
- [x] Mark each capability as supported, partially supported, unsupported, ambiguous, or deprecated.
- [x] Link inventory entries to relevant tests, code paths, or legacy examples where available.
- [x] Add notes for known behavioral differences.
- [x] Maintain the canonical inventory in `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`.
- [x] Define how the inventory will be updated as support changes.

**Completion Criteria**

- [x] There is one canonical parity inventory.
- [x] Supported and unsupported capabilities are clearly distinguishable.
- [x] The inventory is linked from the relevant phase or project documentation.
- [x] The inventory has an agreed maintenance rule.

### Unsupported Request Handling

**Objective**

Ensure unsupported composed requests fail or fall back explicitly.

- [x] Identify unsupported composed request patterns.
- [x] Decide whether each pattern should fail, fall back, or be blocked earlier.
- [x] Define and implement explicit error or fallback semantics.
- [x] Ensure error messages are actionable.
- [x] Add tests for unsupported request behavior.
- [x] Document unsupported behavior in `docs/DESIGN.md` and `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`.

**Completion Criteria**

- [x] Unsupported requests no longer behave ambiguously.
- [x] Unsupported requests are covered by tests.
- [x] Error or fallback behavior is documented.
- [x] Unsupported items are visible in the parity inventory.

## Progress Tracker

| Area                         | Status      | Notes                                                                                                                                                                                                                                                                                                                                                                                                                       |
|------------------------------|-------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Contract baseline            | Completed   | Current contract surface is now documented in `docs/DESIGN.md`; focused tests now protect the route-parser return contract, the route-compiler null-input boundary, the discrete resolver missing-operator boundary, the composed-versus-legacy handoff, and the composed-query anchor-key alias contract, and the route parser, route compiler, anchor-template route, and discrete picked-filter input contracts now use read-only collection boundaries. Remaining widening work is tracked explicitly as follow-up items rather than as unresolved contract ambiguity. |
| Supported slice regression   | Completed   | The grouped live matrix and current supported slice list are now anchored explicitly to `sead.query.test/LiveTests/FacetLoadService.cs`; the supported visible/discrete and range families are grouped separately through `SupportedComposedVisibleAndDiscreteLiveUris` and `SupportedComposedRangeLiveUris`, the grouped live regression sweep passes across both supported families, and unsupported intersect plus GIS polygon slices are guarded with explicit live fallback assertions. |
| Legacy parity inventory      | Completed   | `PARITY_INVENTORY.md` is now the canonical inventory, linked from this phase plan, with clear supported versus unsupported status values and an explicit maintenance rule.                                                                                                                                                                                                                                                  |
| Unsupported request handling | Completed   | The current runtime fallback boundary is now documented; focused tests now show the rule consistently: unsupported requests are blocked earlier by `CanHandle(...)`, `FacetContentService.Load` falls back to the legacy path, and direct `ComposedFacetContentService.Load` calls fail with an actionable error. Predicate-side joined-table clauses and unresolved target-side join derivation are both pinned to that rule. |

## Definition Of Done

- [x] All phase acceptance criteria are satisfied.
- [ ] Contract documentation is committed and matches compiled code.
- [x] One canonical parity inventory exists and is linked from the relevant documentation.
- [x] Grouped regression covers all currently supported composed slices.
- [x] Unsupported composed requests fail or fall back explicitly.
- [x] Required validation has been run and recorded.
- [x] Follow-up work is captured as explicit tasks or issues.

## Validation And Testing

- [x] Review contract documentation against the compiled contract surface.
- [x] Run contract-focused unit tests for the touched query-composer surfaces.
- [x] Run grouped regression for currently supported composed slices.
- [x] Run tests that cover unsupported composed request behavior.
- [x] Review the parity inventory for coverage of facet families and result paths.
- Recorded focused validation: `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~DiscreteFacetPredicateResolverTests|FullyQualifiedName~ComposedFacetContentServiceTests"`.
- Recorded focused validation: `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_UnsupportedIntersectSlice_FallsBackToLegacyFacetContent"`.
- Recorded focused validation: `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_UnsupportedSitesPolygonSlice_FallsBackToLegacyFacetContent"`.
- Recorded focused validation: `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ArrowRouteParserTests"`.
- Recorded focused validation: `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~DiscreteFacetPredicateResolverTests"`.
- Recorded focused validation: `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedFacetContentServiceTests"`.
- Recorded focused validation: `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedVisibleAndDiscreteLiveSlices|FullyQualifiedName~FacetContentService_ComposedSupportedRangeLiveSlices"`.
- [x] Use repository-specific commands only when they are explicitly known; otherwise record placeholders such as `<test-command>`.

## Deliverables

| Deliverable                  | Description                                                                                               | Status      | Link                                                                                                                                                                                                             |
|------------------------------|-----------------------------------------------------------------------------------------------------------|-------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Contract documentation       | Canonical description of route, anchor, facet-resolver, composed-query, and unsupported-request contracts | Completed   | `docs/DESIGN.md`                                                                                                                                                                                                 |
| Compiled contract updates    | Code-level types or interfaces that reflect the documented contracts                                      | Completed   | `sead.query.composer/QueryComposer/RouteCompiler/ArrowRouteParser.cs`, `sead.query.composer/QueryComposer/RouteCompiler/RouteSqlCompiler.cs`, `sead.query.composer/QueryComposer/RouteCompiler/AnchorTemplate.cs`, `sead.query.composer/QueryComposer/RouteCompiler/DiscreteFacetPredicateResolver.cs`, `sead.query.composer/QueryComposer/Inputs/DiscreteFacetUserInput.cs` |
| Parity inventory             | Maintained inventory of legacy facet families and result paths                                            | Completed   | `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md`                                                                                                                                                       |
| Grouped regression coverage  | Regression coverage for currently supported composed slices                                               | Completed   | `sead.query.test/LiveTests/FacetLoadService.cs`                                                                                                                                                                  |
| Unsupported-request behavior | Explicit current fallback and direct-throw boundary for unsupported composed requests                     | Completed   | `docs/DESIGN.md`, `sead.query.test/UnitTests/QueryComposer/Services/FacetContentServiceComposerTests.cs`, `sead.query.test/UnitTests/QueryComposer/Services/ComposedFacetContentServiceTests.cs`                 |

## Scope

**In scope**

- contract documentation for route, anchor, facet-resolver, and composed-query behavior
- type and interface updates needed to reflect documented contracts
- parity inventory for legacy facet families and result shapes
- grouped regression coverage for currently supported composed slices
- explicit failure or fallback behavior for unsupported composed requests

**Out of scope**

- full migration of unsupported legacy behavior
- performance optimization beyond preserving current reliability
- UI redesign or unrelated API changes
- new feature behavior not present in the legacy engine unless explicitly required for parity

## Risks And Mitigations

| Risk                                             | Mitigation                                                                                              |
|--------------------------------------------------|---------------------------------------------------------------------------------------------------------|
| Legacy behavior is undocumented or inconsistent. | Capture examples and classify ambiguous behavior explicitly in the parity inventory and contract notes. |
| Contracts drift while widening continues.        | Protect contracts with compiled types and grouped regression coverage.                                  |
| Unsupported requests produce misleading results. | Add explicit guards, errors, or fallback behavior and test them.                                        |
| The parity inventory becomes stale.              | Define an explicit maintenance rule and update it when support changes.                                 |

## Follow-Up Items

- [ ] Widen the composed contract for currently unsupported visible facets only when predicate-side source-key resolution or target-side join derivation can be implemented without reopening the current contract baseline.
- [ ] Extend grouped live regression and parity inventory rows in the same change whenever a new slice moves from fallback-only to supported composed behavior.
- [ ] Decide whether Phase 2 or a later close-out pass should add one broader repository-level validation command beyond the focused commands already recorded here.
