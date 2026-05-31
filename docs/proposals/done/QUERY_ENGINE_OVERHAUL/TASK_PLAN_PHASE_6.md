# Task Plan: Phase 6 - Cutover And Legacy Retirement

## Phase Summary

**Phase:** Phase 6 - Cutover And Legacy Retirement

**Status:** Done

**Goal**

Make the composed query engine the authoritative backend path and retire the legacy engine for feature-equivalent scenarios.

**Focus**

- choose and implement the final cutover path
- reduce legacy fallback coverage as parity closes
- retire obsolete query-building paths and outdated proposal-era assumptions
- keep one explicit list of remaining exceptions, if any

**Acceptance Criteria**

- [x] The composed runtime is the default path for feature-equivalent requests.
- [x] The legacy engine is no longer required for the supported API surface.
- [x] Any remaining legacy-only cases are documented as explicit exceptions rather than accidental gaps.
- [x] The system is feature-wise on par with the legacy query engine for the intended runtime scope.

## Scope

**In scope**

- default-path cutover for the validated composed runtime surface
- explicit classification and handling of any remaining legacy-only exceptions
- deployment-targeted validation, rollback verification, and runtime-readiness checks needed for cutover confidence
- retirement of obsolete legacy-only paths and outdated durable documentation for the supported API surface

**Out of scope**

- widening entirely new facet or result families that are not needed for the default cutover boundary
- frontend, client, staffing, scheduling, or release-management work
- undocumented legacy-only behavior that is intentionally deferred as an explicit exception

## Work Breakdown

### Cutover Boundary And Exception Inventory

**Objective**

Turn the current validated surface into one explicit cutover boundary with one maintained exception list.

- [x] Confirm the exact supported request surface that will move onto the default composed path at Phase 6 entry.
- [x] Classify all remaining legacy-only requests as either required-before-cutover work or explicit exceptions accepted at cutover.
- [x] Update the runtime-boundary documents so unsupported fallback remains intentional and reviewable rather than implicit.
- [x] Remove outdated proposal-era assumptions that still describe the composed runtime as pre-cutover where current validation already proves otherwise.

**Completion Criteria**

- [x] One maintained document set identifies the default cutover boundary and the remaining explicit exceptions.
- [x] No required-before-cutover gap remains hidden behind ambiguous fallback behavior or stale prose.

### Representative Cutover Validation Coverage

**Objective**

Expand validation from the current recorded slices to the remaining high-risk request families needed for default cutover.

- [x] Identify the remaining representative live query shapes required beyond the recorded `sites_polygon`, country-filter, and intersect baselines.
- [x] Seed the next representative shortlist with target-only `sites:sites`, target-only `geochronology:geochronology`, and prefixed `ceramic://sample_groups:sample_groups` slices, and confirm the current focused checks pass for those candidates.
- [x] Add focused validation for each newly promoted high-risk family at the narrowest useful layer before it joins broader regression coverage.
- [x] Keep grouped regression and deployment-targeted smoke checks aligned with the validated default-cutover boundary.
- [x] Record which families remain outside the default path and why.

**Completion Criteria**

- [x] Each request family included in the default path has repeatable validation coverage.
- [x] High-risk families outside the cutover boundary are documented as explicit exceptions with concrete blockers.

### Runtime Measurement And Deployment-Like Verification

**Objective**

Separate runtime behavior from local cold-start noise so cutover decisions use deployment-like measurements.

- [x] Add one repeatable warm-process or deployment-like runtime measurement path for representative composed requests.
- [x] Confirm acceptable interactive runtime behavior on the current default-cutover request matrix using that path on the staging branch probe.
- [x] Keep the measurement procedure documented alongside the existing smoke and rollback checks.
- [x] Record comparative interpretation rules and optional threshold handling for warmed timing runs.

**Completion Criteria**

- [x] Cutover readiness is evaluated with a repeatable runtime measurement path that is not dominated by PostgreSQL Testcontainers cold-start overhead.
- [x] Operators and contributors can run the same deployment-like verification procedure before and after cutover.

### Deployment Schema Readiness And Runtime Promotion

**Objective**

Prepare the deployment environment and runtime configuration so the composed path can become authoritative without startup-time schema failures.

- [x] Prove that the staging deployment-like database carries the runtime facet schema and one active imported revision expected by startup validation.
- [x] Prepare the target deployment database with the imported route, anchor, facet-template, and config-revision runtime schema expected by startup validation.
- [x] Confirm the validation and import workflow remains the authoritative path for promoting YAML-authored configuration into runtime tables.
- [x] Implement the final runtime switch so feature-equivalent requests use the composed path by default.
- [x] Keep rollback verification current so the deployment path can safely retreat if a cutover check fails.

**Completion Criteria**

- [x] Branch-built containers no longer fail at startup because required runtime facet-schema objects are missing on the target deployment database.
- [x] The composed runtime is the default path for the supported API surface, with rollback steps documented and tested.

### Legacy Retirement And Durable Documentation

**Objective**

Retire obsolete legacy-only runtime paths and leave durable documentation aligned with the post-cutover system.

- [x] Remove the dead `ResultService` dependency on `QueryServiceBase` and `IQuerySetupBuilder` so supported result execution no longer carries the old query-setup service base.
- [x] Remove or quarantine the remaining legacy fallback seams that are still retained only for explicit unsupported facet-content and result exceptions.
- [x] Update durable architecture, development, and operations documentation to describe the composed runtime as the authoritative backend path for the supported scope.
- [x] Keep the exception inventory, route-governance notes, and diagnostics inventory aligned with the post-cutover runtime boundary.
- [x] Record any intentionally retained legacy-only behavior as explicit follow-up work rather than leaving mixed-runtime assumptions in code or docs.

**Completion Criteria**

- [x] The supported API surface no longer depends on legacy runtime paths beyond the explicit unsupported-request fallbacks still pinned at `FacetContentService` and `ComposedResultProjectionHandoffBuilder`.
- [x] Durable docs describe current cutover behavior, exceptions, and rollback expectations without proposal-era drift.

## Progress Tracker

| Area                                                 | Status | Notes                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
|------------------------------------------------------|--------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Cutover boundary and exception inventory             | Done   | The branch runtime default boundary, accepted current-cutover exceptions, and non-request cutover blockers are now explicit in the parity, diagnostics, and route-governance docs.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| Representative cutover validation coverage           | Done   | Target-only `sites`, target-only `geochronology`, target-only `feature_type`, and prefixed `ceramic://sample_groups:sample_groups` all have focused checks, the promoted `palaeoentomology://rdb_systems:rdb_systems` routed map slice plus the promoted `palaeoentomology://sample_group_sampling_contexts:sample_group_sampling_contexts` and `palaeoentomology://tbl_biblio_modern:tbl_biblio_modern` tabular slices now also have focused proof against the composed-versus-legacy boundary, the deployment-targeted HTTP smoke gate covers the expanded public matrix on the published `supersead` route, and the broader grouped suites were refreshed green at 75/260/116 after the new promotions. |
| Runtime measurement and deployment-like verification | Done   | `make default-cutover-http-measure` now warms and times the baseline-plus-expansion matrix over HTTP, and both staging and `supersead` live-network branch probes measured sub-half-second warmed averages across the current matrix.                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| Deployment schema readiness and runtime promotion    | Done   | Staging and `supersead` now both carry the required runtime tables and active revision, the published `https://supersead.humlab.umu.se/query` service now runs the branch cutover image with green public smoke and timing checks, and the previous image has been verified as a rollback target on localhost.                                                                                                                                                                                                                                                                                                                                                                                             |
| Legacy retirement and durable documentation          | Done   | `ResultService` no longer inherits the old query-setup service base or takes `IQuerySetupBuilder`; the durable docs now describe the promoted composed runtime as authoritative for the supported surface, and the retained fallback seams are quarantined as explicit out-of-scope follow-up work under GitHub issues #175 and #176 rather than Phase 6 blockers.                                                                                                                                                                                                                                                                                                                                         |

## Definition Of Done

- [x] All Phase 6 acceptance criteria are satisfied.
- [x] The default composed runtime boundary is explicit, validated, and reflected in durable docs.
- [x] Every remaining legacy-only case is either retired or documented as an intentional exception with a concrete blocker.
- [x] Deployment-targeted validation, rollback verification, and runtime-readiness checks are current and repeatable.
- [x] The target deployment database contains the runtime facet-schema objects and active revision data required by startup validation.
- [x] Obsolete legacy-only runtime paths for the supported API surface are removed or isolated so they are not part of normal execution beyond the explicit unsupported-request fallbacks still retained at the facet-content and result handoff boundaries.
- [x] Follow-up work is captured as explicit exceptions or later-phase tasks rather than hidden in mixed-runtime behavior.

## Validation And Testing

- [x] Run a branch-image `--validate-facet-config` check on the `supersead` deployment network against the mounted live appsettings before importing the candidate revision.
- [x] Run `make default-cutover-smoke-check` after widening the deployment-targeted smoke matrix and confirm the broader regression gate still passes.
- [x] Run `make default-cutover-http-smoke-check SEAD_QUERY_API_BASE_URL=http://127.0.0.1:8098` against a live-network branch probe on the prepared `supersead` target.
- [x] Run `make default-cutover-http-smoke-check SEAD_QUERY_API_BASE_URL=https://supersead.humlab.umu.se/query` against the published `supersead` query API after promoting the branch runtime.
- [x] Expand `scripts/default-cutover-http-smoke-check.sh` so the deployment-targeted smoke gate covers the current baseline plus target-only `sites`, target-only `geochronology`, and prefixed `ceramic://sample_groups:sample_groups`, then rerun it successfully against the published `supersead` route.
- [x] Run `SEAD_QUERY_API_BASE_URL=http://127.0.0.1:8096 SEAD_QUERY_API_MEASURE_SAMPLES=2 make default-cutover-http-measure` against a warmed staging branch probe for the agreed representative matrix.
- [x] Run branch-image `--import-facet-config` on the `supersead` deployment network and confirm one active `facet.config_revision` row exists on the target deployment database.
- [x] Run focused unit, integration, live, or controller validation for each newly promoted high-risk request family using the narrowest useful repository test slice.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlySitesSlice|FullyQualifiedName~Load_TargetOnlySitesMapResult_UsesComposedFilterSql|FullyQualifiedName~LoadMap_TargetOnlySitesRequest_UsesComposedFilterSql"` to validate the target-only routed `sites` expansion candidate.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyGeochronologySlice"` to validate the target-only range `geochronology` expansion candidate.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_TargetOnlyCeramicSampleGroupsTabularResult_UsesComposedFilterSql|FullyQualifiedName~Load_TargetOnlyCeramicSampleGroupsMapResult_UsesComposedFilterSql|FullyQualifiedName~LoadMap_TargetOnlyCeramicSampleGroupsRequest_UsesComposedFilterSql"` to validate the deeper prefixed `ceramic://sample_groups:sample_groups` result-path expansion candidate.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_TargetOnlyPalaeoentomologyRdbSystemsMapResult_MatchesLegacyOutput"` to validate the promoted `palaeoentomology://rdb_systems:rdb_systems` routed map slice against the forced-legacy result path.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedTargetOnlyFeatureTypeSlice|FullyQualifiedName~FacetContentService_ComposedTargetOnlySitesSlice_MatchesLegacyFacetContent|FullyQualifiedName~Build_WithTargetOnlyPrefixedSampleGroupSamplingContextsTabularResult_UsesUnfilteredComposedFilterJoin|FullyQualifiedName~Load_TargetOnlyPalaeoentomologySampleGroupSamplingContextsTabularResult"` to validate the target-only routed `feature_type` facet-content slice and the prefixed `sample_group_sampling_contexts` result-path exception candidate.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Build_WithTargetOnlyPrefixedBiblioModernTabularResult_UsesUnfilteredComposedFilterJoin|FullyQualifiedName~Load_TargetOnlyPalaeoentomologyBiblioModernTabularResult"` to validate the prefixed `tbl_biblio_modern` result-path exception candidate.
- [x] Run `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~SQT.Services.ResultServiceTests|FullyQualifiedName~DependencyInjectionTests"` after removing the dead `ResultService` query-setup dependency and confirm the focused result-service and DI slice still passes.
- [x] Run `SEAD_QUERY_API_BASE_URL=http://127.0.0.1:8098 SEAD_QUERY_API_MEASURE_SAMPLES=2 make default-cutover-http-measure` against a warmed live-network branch probe on the prepared `supersead` target.
- [x] Run `SEAD_QUERY_API_BASE_URL=https://supersead.humlab.umu.se/query SEAD_QUERY_API_MEASURE_SAMPLES=2 make default-cutover-http-measure` against the published `supersead` query API after promoting the branch runtime.
- [x] Start `supersead-sead_query_api:latest` as a localhost rollback probe on `127.0.0.1:8099` and confirm `api/version` still responds against the prepared live database.
- [x] Re-run rollback verification after the final runtime switch by starting `supersead-sead_query_api:latest` on `127.0.0.1:8099` and confirming both `api/version` and the representative legacy `country` map request still succeed against the prepared live database.

## Deliverables

| Deliverable                                   | Description                                                                                        | Status | Link                                                                                                                                                                        |
|-----------------------------------------------|----------------------------------------------------------------------------------------------------|--------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Default cutover boundary record               | One maintained record of the supported default-path surface and the remaining explicit exceptions. | Done   | `docs/proposals/done/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md` and adjacent cutover docs                                                                                        |
| Cutover validation matrix                     | Focused and grouped validation coverage for the request families required at cutover.              | Done   | `docs/OPERATIONS.md` and this task plan; the current public smoke matrix and the broader grouped regression gate are now aligned for the validated default-cutover surface. |
| Deployment-like runtime measurement procedure | One repeatable runtime-readiness path that can be used before and after cutover.                   | Done   | `docs/OPERATIONS.md`, `Makefile`, and `scripts/default-cutover-http-measure.sh`                                                                                             |
| Deployment schema and promotion procedure     | Durable guidance for validation, import, deployment readiness, runtime promotion, and rollback.    | Done   | `docs/OPERATIONS.md` and `docs/DEVELOPMENT.md`                                                                                                                              |
| Post-cutover architecture record              | Durable architecture and diagnostics docs aligned with the authoritative composed runtime.         | Done   | `docs/DESIGN.md`, `docs/DEVELOPMENT.md`, `docs/OPERATIONS.md`, `docs/DIAGRAMS.md`, and Phase 6 follow-up docs                                                               |

## Risks And Mitigations

- Runtime parity risk: a request family may appear green in grouped checks while still hiding a legacy-only edge case. Mitigation: require one focused validation slice before grouped promotion and keep exceptions explicit.
- Deployment drift risk: the target database may lag behind the runtime schema expected by startup validation. Mitigation: treat schema preparation and imported revision data as cutover prerequisites, not post-cutover cleanup.
- Performance interpretation risk: local cold-start overhead may distort readiness conclusions. Mitigation: use one repeatable warm-process or deployment-like measurement path for the representative matrix.

## Open Questions

- Which explicit unsupported facet-content family should be retired next so the `FacetContentService` fallback boundary can shrink without widening scope?
- Which explicit unsupported result family should be retired next so `ComposedResultProjectionHandoffBuilder` can stop delegating that request shape to the legacy handoff?

## Assumptions

- Phase 5 closed the configuration-governance, diagnostics, and semantic-validation gaps needed to start cutover work.
- The current composed support surface and explicit exception list remain authoritative until Phase 6 changes them explicitly.
- Phase 6 is the first phase that performs the runtime default-path switch; earlier phases only prepared for it.