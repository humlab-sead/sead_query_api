# Task Plan: Phase 6 - Cutover And Legacy Retirement

## Phase Summary

**Phase:** Phase 6 - Cutover And Legacy Retirement

**Status:** Not started

**Goal**

Make the composed query engine the authoritative backend path and retire the legacy engine for feature-equivalent scenarios.

**Focus**

- choose and implement the final cutover path
- reduce legacy fallback coverage as parity closes
- retire obsolete query-building paths and outdated proposal-era assumptions
- keep one explicit list of remaining exceptions, if any

**Acceptance Criteria**

- [ ] The composed runtime is the default path for feature-equivalent requests.
- [ ] The legacy engine is no longer required for the supported API surface.
- [ ] Any remaining legacy-only cases are documented as explicit exceptions rather than accidental gaps.
- [ ] The system is feature-wise on par with the legacy query engine for the intended runtime scope.

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

- [ ] Confirm the exact supported request surface that will move onto the default composed path at Phase 6 entry.
- [ ] Classify all remaining legacy-only requests as either required-before-cutover work or explicit exceptions accepted at cutover.
- [ ] Update the runtime-boundary documents so unsupported fallback remains intentional and reviewable rather than implicit.
- [ ] Remove outdated proposal-era assumptions that still describe the composed runtime as pre-cutover where current validation already proves otherwise.

**Completion Criteria**

- [ ] One maintained document set identifies the default cutover boundary and the remaining explicit exceptions.
- [ ] No required-before-cutover gap remains hidden behind ambiguous fallback behavior or stale prose.

### Representative Cutover Validation Coverage

**Objective**

Expand validation from the current recorded slices to the remaining high-risk request families needed for default cutover.

- [ ] Identify the remaining representative live query shapes required beyond the recorded `sites_polygon`, country-filter, and intersect baselines.
- [ ] Add focused validation for each newly promoted high-risk family at the narrowest useful layer before it joins broader regression coverage.
- [ ] Keep grouped regression and deployment-targeted smoke checks aligned with the validated default-cutover boundary.
- [ ] Record which families remain outside the default path and why.

**Completion Criteria**

- [ ] Each request family included in the default path has repeatable validation coverage.
- [ ] High-risk families outside the cutover boundary are documented as explicit exceptions with concrete blockers.

### Runtime Measurement And Deployment-Like Verification

**Objective**

Separate runtime behavior from local cold-start noise so cutover decisions use deployment-like measurements.

- [ ] Add one repeatable warm-process or deployment-like runtime measurement path for representative composed requests.
- [ ] Confirm acceptable interactive runtime behavior on the default-cutover request matrix using that path.
- [ ] Keep the measurement procedure documented alongside the existing smoke and rollback checks.
- [ ] Record any threshold or interpretation rules that operators need during cutover validation.

**Completion Criteria**

- [ ] Cutover readiness is evaluated with a repeatable runtime measurement path that is not dominated by PostgreSQL Testcontainers cold-start overhead.
- [ ] Operators and contributors can run the same deployment-like verification procedure before and after cutover.

### Deployment Schema Readiness And Runtime Promotion

**Objective**

Prepare the deployment environment and runtime configuration so the composed path can become authoritative without startup-time schema failures.

- [ ] Prepare the target deployment database with the imported route, anchor, facet-template, and config-revision runtime schema expected by startup validation.
- [ ] Confirm the validation and import workflow remains the authoritative path for promoting YAML-authored configuration into runtime tables.
- [ ] Implement the final runtime switch so feature-equivalent requests use the composed path by default.
- [ ] Keep rollback verification current so the deployment path can safely retreat if a cutover check fails.

**Completion Criteria**

- [ ] Branch-built containers no longer fail at startup because required runtime facet-schema objects are missing on the target deployment database.
- [ ] The composed runtime is the default path for the supported API surface, with rollback steps documented and tested.

### Legacy Retirement And Durable Documentation

**Objective**

Retire obsolete legacy-only runtime paths and leave durable documentation aligned with the post-cutover system.

- [ ] Remove or quarantine legacy query-building paths that are no longer needed for the supported API surface after cutover.
- [ ] Update durable architecture, development, and operations documentation to describe the composed runtime as the authoritative backend path for the supported scope.
- [ ] Keep the exception inventory, route-governance notes, and diagnostics inventory aligned with the post-cutover runtime boundary.
- [ ] Record any intentionally retained legacy-only behavior as explicit follow-up work rather than leaving mixed-runtime assumptions in code or docs.

**Completion Criteria**

- [ ] The supported API surface no longer depends on legacy runtime paths that Phase 6 is supposed to retire.
- [ ] Durable docs describe current cutover behavior, exceptions, and rollback expectations without proposal-era drift.

## Progress Tracker

| Area | Status | Notes |
|---|---|---|
| Cutover boundary and exception inventory | Not started | Establish the authoritative default-path boundary and the explicit exception list first. |
| Representative cutover validation coverage | Not started | Expand beyond the recorded spatial, country-filter, and intersect baselines only where needed for cutover. |
| Runtime measurement and deployment-like verification | Not started | Add one repeatable warm-process or deployment-like path before judging cutover performance. |
| Deployment schema readiness and runtime promotion | Not started | Target deployment still needs the imported runtime facet schema and final default-path promotion. |
| Legacy retirement and durable documentation | Not started | Remove obsolete legacy-only paths only after the cutover boundary is validated and explicit. |

## Definition Of Done

- [ ] All Phase 6 acceptance criteria are satisfied.
- [ ] The default composed runtime boundary is explicit, validated, and reflected in durable docs.
- [ ] Every remaining legacy-only case is either retired or documented as an intentional exception with a concrete blocker.
- [ ] Deployment-targeted validation, rollback verification, and runtime-readiness checks are current and repeatable.
- [ ] The target deployment database contains the runtime facet-schema objects and active revision data required by startup validation.
- [ ] Obsolete legacy-only runtime paths for the supported API surface are removed or isolated so they are not part of normal execution.
- [ ] Follow-up work is captured as explicit exceptions or later-phase tasks rather than hidden in mixed-runtime behavior.

## Validation And Testing

- [ ] Run `make validate-facet-config` to confirm the authoring configuration remains structurally and semantically valid before promotion.
- [ ] Run `make default-cutover-smoke-check` after each meaningful widening or runtime-switch change.
- [ ] Run `make default-cutover-http-smoke-check SEAD_QUERY_API_BASE_URL=<target-base-url>` against the deployment-like environment targeted for cutover.
- [ ] Run focused unit, integration, live, or controller validation for each newly promoted high-risk request family using the narrowest useful repository test slice.
- [ ] Run the repeatable warm-process or deployment-like runtime measurement procedure for the agreed representative cutover matrix.
- [ ] Re-run rollback verification after the final runtime switch and before Phase 6 closure.

## Deliverables

| Deliverable | Description | Status | Link |
|---|---|---|---|
| Default cutover boundary record | One maintained record of the supported default-path surface and the remaining explicit exceptions. | Not started | `docs/proposals/QUERY_ENGINE_OVERHAUL/PARITY_INVENTORY.md` and adjacent cutover docs |
| Cutover validation matrix | Focused and grouped validation coverage for the request families required at cutover. | Not started | `TBD` |
| Deployment-like runtime measurement procedure | One repeatable runtime-readiness path that can be used before and after cutover. | Not started | `docs/OPERATIONS.md` |
| Deployment schema and promotion procedure | Durable guidance for validation, import, deployment readiness, runtime promotion, and rollback. | Not started | `docs/OPERATIONS.md` and `docs/DEVELOPMENT.md` |
| Post-cutover architecture record | Durable architecture and diagnostics docs aligned with the authoritative composed runtime. | Not started | `docs/DESIGN.md` and Phase 6 follow-up docs |

## Risks And Mitigations

- Runtime parity risk: a request family may appear green in grouped checks while still hiding a legacy-only edge case. Mitigation: require one focused validation slice before grouped promotion and keep exceptions explicit.
- Deployment drift risk: the target database may lag behind the runtime schema expected by startup validation. Mitigation: treat schema preparation and imported revision data as cutover prerequisites, not post-cutover cleanup.
- Performance interpretation risk: local cold-start overhead may distort readiness conclusions. Mitigation: use one repeatable warm-process or deployment-like measurement path for the representative matrix.

## Open Questions

- Which additional representative live query shapes beyond the recorded `sites_polygon`, country-filter, and intersect baselines must be green before default cutover?
- Which remaining facet families, if any, still require explicit exception routes or SQL overrides as YAML coverage widens?
- Which residual diagnostics gaps are acceptable explicit Phase 6 exceptions, and which must be closed before the runtime switch?

## Assumptions

- Phase 5 closed the configuration-governance, diagnostics, and semantic-validation gaps needed to start cutover work.
- The current composed support surface and explicit exception list remain authoritative until Phase 6 changes them explicitly.
- Phase 6 is the first phase that performs the runtime default-path switch; earlier phases only prepared for it.