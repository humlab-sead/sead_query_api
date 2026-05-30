# Task Plan: Phase 5 - Configuration And Operational Hardening

## Phase Summary

**Phase:** Phase 5 - Configuration And Operational Hardening

**Status:** Done

**Goal**

Make the composed engine robust enough to be the default runtime path.

**Focus**

- settle route-definition governance and validation rules
- harden configuration errors and diagnostics
- confirm performance on representative live query shapes
- close the gap between branch-only behavior and maintainable long-term runtime behavior

**Acceptance Criteria**

- [x] Route definitions and anchor mappings have a maintained source of truth.
- [x] Configuration failures surface as clear diagnostics.
- [x] Representative composed queries meet acceptable runtime behavior for interactive use.
- [x] Durable docs describe the active architecture and the supported boundaries accurately.

## Documentation Targets

- Record route-governance and runtime-boundary decisions in `docs/DESIGN.md`.
- Record contributor-facing configuration and validation workflow changes in `docs/DEVELOPMENT.md`.
- Record operational diagnostics, performance expectations, and runtime-boundary guidance in `docs/OPERATIONS.md`.
- Keep `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md` aligned if Phase 5 narrows or clarifies supported boundaries.
- Update this task plan in place as hardening work lands or explicit Phase 6 prerequisites are identified.

## Work Breakdown

### Route Governance And Source Of Truth

**Objective**

Turn route-definition ownership from branch knowledge into one maintained and validated source of truth.

- [x] Inventory the active route-definition inputs, anchor mappings, and runtime lookup paths that still require manual branch knowledge.
- [x] Choose and document the maintained source of truth for route definitions and anchor mappings.
- [x] Define validation rules for route definitions, anchor mappings, and unsupported-boundary declarations.
- [x] Add focused validation at the narrowest useful boundary so broken route or anchor configuration fails explicitly.
- [x] Record any route families that still need manual exceptions before Phase 6 cutover.

**Completion Criteria**

- [x] Route-definition ownership is explicit and documented.
- [x] Broken route or anchor configuration is caught by repeatable validation instead of ad hoc runtime discovery.

### Configuration Diagnostics And Failure Boundaries

**Objective**

Make configuration and composition failures understandable enough to support the composed path as a default runtime candidate.

- [x] Inventory the current failure modes for route resolution, anchor mapping, unsupported request classification, and runtime configuration loading.
- [x] Tighten failure messages and logging so the runtime reports the failing contract, not only a low-level exception.
- [x] Add focused unit or integration coverage for startup-time and request-time configuration failures.
- [x] Confirm that unsupported requests still fail or fall back through explicit boundaries rather than ambiguous partial composition.
- [x] Capture any remaining diagnostics gaps as explicit Phase 6 prerequisites.

**Completion Criteria**

- [x] Configuration failures surface as clear, actionable diagnostics.
- [x] Unsupported-boundary behavior remains explicit under misconfiguration and partial rollout conditions.

### Performance Validation And Runtime Readiness

**Objective**

Prove that the composed runtime behaves acceptably on representative live query shapes before it becomes the default path.

- [x] Select an initial representative spatial query family for runtime readiness using the polygon-filtered `sites_polygon` slice across facet-content, result-load, and controller layers.
- [x] Record a repeatable way to measure those representative queries on the current branch runtime.
- [x] Run focused live validation for the initial representative query set and capture observed behavior, slow paths, and query-shape outliers.
- [x] Classify the recorded runtime-readiness probes: no unacceptable behavior is currently observed in the measured spatial, country-filter, or intersect baselines, so there is no immediate Phase 5 blocker from those covered slices.
- [x] Document the current acceptable runtime boundary and known outliers for the measured slices.

**Current measured runtime boundary**

- Accepted for current Phase 5 evidence: the covered `sites_polygon`, country-filter, and `analysis_entity_ages` intersect families, plus the broader live result, controller, and composed facet-content reruns recorded in this plan.
- Not yet part of the acceptance claim: unmeasured composed families outside those baselines, warm-process steady-state timing isolated from Testcontainers startup, and any default-cutover behavior that depends on route families not yet exercised by the recorded probes.

**Completion Criteria**

- [x] Representative composed queries have recorded runtime behavior on the current branch state for the currently covered families.
- [x] Phase 5 leaves one explicit list of performance outliers and unmeasured risk instead of informal branch knowledge.

### Durable Runtime Hardening And Documentation

**Objective**

Close the gap between a validated branch runtime and a maintainable long-term default path.

- [x] Review branch-only setup steps, assumptions, and manual recovery paths that would block default composed runtime adoption.
- [x] Convert those assumptions into explicit docs, validation, or tracked follow-up work.
- [x] Update `docs/DESIGN.md`, `docs/DEVELOPMENT.md`, and `docs/OPERATIONS.md` so the active runtime, supported boundaries, and operational expectations are consistent.
- [x] Keep `PARITY_INVENTORY.md` and this task plan aligned with any clarified support or exception boundaries.
- [x] Record the concrete Phase 6 cutover prerequisites that remain after Phase 5 hardening.

**Completion Criteria**

- [x] Durable docs describe the active architecture, supported boundaries, and operating expectations without relying on branch-only context.
- [x] Remaining cutover blockers are documented as explicit Phase 6 prerequisites.

## Progress Tracker

| Area                                     | Status      | Notes                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
|------------------------------------------|-------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Route governance and source of truth     | Done        | Phase 5 now has an explicit governance decision for the maintained authoring surface: `sead.query.composer/Templates/route_v1.yaml` is the checked-in source of truth, `sead.query.composer/Templates/facet-route-config.schema.json` defines the authoring contract, and imports materialize one active runtime revision in the existing `facet` schema. `--validate-facet-config` now catches broken anchor-table mappings, generated-route endpoint drift, and unresolved facet-anchor route bindings before import, and the manual exception inventory is explicit for the current draft and its out-of-draft follow-up surface. |
| Configuration diagnostics and boundaries | Done        | Phase 5 now has one explicit diagnostics inventory and validation ladder: YAML schema validation plus `--validate-facet-config`, importer materialization into the active `facet.config_revision`, startup route-graph and configured-route validation, and the repository plus deployment smoke gates. Startup validation now reports route name or alias plus the failing specification, direct unsupported composed facet-content loads now report the first failing request-time contract instead of only a generic unsupported message, and composed result handoff now logs the first failing contract before falling back to the legacy projection path. Unsupported facet-content and result requests remain explicit fallback or direct-throw boundaries rather than ambiguous partial composition. |
| Performance validation and readiness     | Done        | Recorded runtime-readiness probing now covers three representative families across facet-content, result-load, and controller layers: heavy spatial `sites_polygon`, non-spatial country-filter, and intersect `analysis_entity_ages`. Those representative probes all passed on cold runs in about 9-17 seconds wall-clock, with startup cost dominated by PostgreSQL Testcontainers initialization. The broader reruns called out in this plan are also green: `SQT.LiveServices.ResultLoadServiceTests` passed 72 tests in about 53 seconds wall-clock, `IntegrationTests.Sead.ResultControllerTests` passed 260 tests in about 66 seconds wall-clock, and `FacetContentService_ComposedSupportedLiveSlices` passed 114 tests in about 56 seconds wall-clock during the latest `make default-cutover-smoke-check` rerun. The deployment-targeted staging HTTP smoke gate is now also green for the same representative country-filter, `sites_polygon`, and `analysis_entity_ages` slices after schema preparation plus import. |
| Durable runtime hardening and docs       | Done        | The importer now has runnable `--import-facet-config` and `--validate-facet-config` host entry points, scripted `make import-facet-config`, `make validate-facet-config`, `make prepare-phase5-facet-runtime-schema`, `make default-cutover-smoke-check`, and `make default-cutover-http-smoke-check` workflows, exact-commit deployment provenance through `docker/build.env`, and active revision tracking in `facet.config_revision`. `docs/DESIGN.md`, `docs/DEVELOPMENT.md`, `docs/OPERATIONS.md`, and `PARITY_INVENTORY.md` are aligned with the current Phase 5 runtime boundary and default-cutover prerequisites. The remaining target-environment blocker on `supersead` is now documented as explicit Phase 6 follow-up rather than Phase 5 ambiguity. |

## Definition Of Done

- [x] All Phase 5 acceptance criteria are satisfied.
- [x] Route-definition ownership and validation rules are documented and enforced at a repeatable boundary.
- [x] Configuration failures surface as clear diagnostics with focused validation coverage.
- [x] Representative composed-query runtime behavior is measured and any outliers are classified explicitly.
- [x] `docs/DESIGN.md`, `docs/DEVELOPMENT.md`, and `docs/OPERATIONS.md` reflect the active runtime and supported boundaries accurately.
- [x] Phase 6 prerequisites are captured as explicit follow-up work rather than implied branch knowledge.

## Validation And Testing

- [x] Run focused route or configuration validation coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~RouteGraphTests|FullyQualifiedName~RouteGraphFactoryTests"`.
- [x] Run focused importer semantic validation coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetRouteConfigurationImporterTests"`.
- [x] Run focused startup or diagnostics coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ArrowRouteParserTests|FullyQualifiedName~RouteConfigurationStartupValidationServiceTests"`.
- [x] Run focused request-time diagnostics coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedFacetContentServiceTests"`.
- [x] Run focused result-handoff fallback diagnostics coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ResultProjectionHandoffBuilderTests|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"`.
- [x] Run representative live runtime-readiness checks using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlySitesPolygonSlice"`, `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_GeoPolygonFilteredMapResult_UsesComposedFilterSql"`, `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~LoadMap_GeoPolygonFilteredRequest_UsesComposedFilterSql"`, `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedCountryPredicateGeochronologySlice"`, `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_CountryFilteredMapResult_UsesComposedFilterSql|FullyQualifiedName~Load_CountryFilteredMapResult_MatchesLegacyOutput"`, `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~LoadMap_CountryFilteredRequest_UsesComposedFilterSql"`, `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyIntersectSlice"`, `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_IntersectFilteredMapResult_UsesComposedFilterSql"`, and `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~LoadMap_IntersectFilteredRequest_UsesComposedFilterSql"`.
- [x] Re-run broader composed live result coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~SQT.LiveServices.ResultLoadServiceTests"`.
- [x] Re-run broader controller coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~IntegrationTests.Sead.ResultControllerTests"`.
- [x] Re-run composed facet-content regression coverage using `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedSupportedLiveSlices"` if Phase 5 changes shared query composition or runtime wiring.
- [x] Run the repository cutover gate using `make default-cutover-smoke-check` when validating a candidate default-path rollout.
- [x] Run the deployment-targeted HTTP smoke gate using `make default-cutover-http-smoke-check SEAD_QUERY_API_BASE_URL=https://host/query` when validating a deployed candidate.
- [x] Build and start an isolated branch container on the `supersead` deployment network; current startup fails fast against the live database because the expected `facet.route` runtime table is missing there.
- [x] Apply `make prepare-phase5-facet-runtime-schema` plus the active facet-route import on a deployment-like staging target, then rerun the deployment-targeted HTTP smoke gate; the branch probe now passes `api/version` plus representative `country`, `sites_polygon`, and `analysis_entity_ages` map requests.

## Deliverables

| Deliverable                        | Description                                                                                | Status      | Link                                                                                        |
|------------------------------------|--------------------------------------------------------------------------------------------|-------------|---------------------------------------------------------------------------------------------|
| Phase 5 task plan                  | Execution tracker for configuration and operational hardening                              | Done        | `docs/proposals/QUERY_ENGINE_OVERHAUL/TASK_PLAN_PHASE_5.md`                                 |
| Route-governance decisions         | Maintained source-of-truth and validation rules for route and anchor setup                 | Done        | `docs/proposals/QUERY_ENGINE_OVERHAUL/FACET_ROUTE_CONFIGURATION_SOURCE_OF_TRUTH.md`         |
| Configuration diagnostics coverage | Focused validation for startup-time and request-time configuration failure                 | Done        | `docs/proposals/QUERY_ENGINE_OVERHAUL/CONFIGURATION_DIAGNOSTICS_AND_VALIDATION_BOUNDARY.md` |
| Runtime-readiness notes            | Representative performance observations, limits, and known outliers                        | Done        | `docs/OPERATIONS.md`                                                                        |
| Durable hardening documentation    | Architecture, development, operations, and parity-inventory updates for Phase 5 boundaries | Done        | `docs/DEVELOPMENT.md`                                                                       |
| Deployment schema prep             | Idempotent SQL for the missing Phase 5 route/provenance tables on target databases         | Done        | `scripts/prepare-phase5-facet-runtime-schema.sql`                                           |

## Scope

**In scope**

- route-definition governance and validation needed to support a default composed runtime path
- clearer configuration diagnostics and explicit unsupported-boundary behavior
- representative performance confirmation for composed facet-content and result-query shapes
- durable documentation updates required to explain the active runtime and cutover prerequisites

**Out of scope**

- making the composed runtime the default path for all supported requests before Phase 6
- retiring the legacy runtime or removing fallback coverage that still protects supported behavior
- widening unrelated feature families unless Phase 5 exposes a shared governance or runtime-readiness defect
- speculative performance tuning without a representative measured query shape

## Risks And Mitigations

| Risk                                                                                                              | Mitigation                                                                                                                           |
|-------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------|
| Route definitions may still depend on scattered branch knowledge.                                                 | Choose one maintained source of truth and validate it at a repeatable boundary.                                                      |
| Stricter validation may surface latent configuration drift late.                                                  | Add focused diagnostics coverage before tightening runtime defaults or startup behavior.                                             |
| Covered probes may pass while unmeasured composed families still hide default-cutover regressions.                | Keep the current acceptance claim limited to the recorded baselines and widen the representative catalog before default cutover.     |
| Cold-run timings are dominated by PostgreSQL Testcontainers startup, which can obscure steady-state runtime cost. | Treat current timings as branch-readiness evidence only and add one repeatable warm-process measurement path before Phase 6 cutover. |
| Durable docs may drift from runtime behavior during hardening.                                                    | Update architecture, development, and operations docs alongside each hardening milestone.                                            |

## Phase 6 Prerequisites

- [ ] Expand representative runtime-readiness coverage beyond the recorded `sites_polygon`, country-filter, and intersect baselines to any remaining high-risk composed families required for default cutover.
- [ ] Add one repeatable warm-process or deployment-like runtime measurement path so query execution can be evaluated separately from PostgreSQL Testcontainers cold-start overhead.
- [ ] Prepare the target deployment database with the imported Phase 5 route/provenance runtime schema and active revision data expected by startup validation so branch-built containers do not fail on missing `facet.anchor`, `facet.facet_anchor`, `facet.facet_template`, `facet.route`, `facet.route_step`, and `facet.config_revision`.
- [ ] Keep the route-governance exception inventory current as YAML coverage widens, and remove any remaining manual-route knowledge from families still outside the current draft.
- [ ] Finish unsupported-boundary and diagnostics inventory so default cutover does not rely on ambiguous fallback or partial-composition behavior.
- [ ] Keep the deployment-targeted HTTP smoke procedure and rollback verification aligned with any future widening of the accepted default-cutover boundary.

## Open Questions

- Which remaining facet families are most likely to need explicit exception routes or SQL overrides as YAML coverage widens beyond the current draft?
- Which additional representative live query shapes beyond the recorded `sites_polygon` spatial slice, country-filter baseline, and intersect baseline should define acceptable interactive runtime behavior before default cutover?
- Which remaining diagnostics gaps are acceptable Phase 6 follow-up work versus Phase 5 blockers?

## Assumptions

- Phase 4 result-set parity is the baseline runtime state for Phase 5 hardening.
- The current composed support surface and explicit exception list remain authoritative until Phase 5 changes them explicitly.
- Phase 5 should prepare the runtime for default-path cutover without performing that cutover.