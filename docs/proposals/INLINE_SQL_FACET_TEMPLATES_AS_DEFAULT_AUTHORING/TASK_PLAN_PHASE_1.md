# Task Plan: Phase 1 - Authoring Contract And Runtime Plumbing

## Phase Summary

- Goal: make inline SQL a first-class authored, imported, and runtime-loadable facet path while keeping unchanged facets on the existing configuration path
- Focus: YAML contract, schema validation, importer persistence, runtime loading, and constrained `template_key` support for retained result-shape facets
- Acceptance Criteria:
  - [ ] the maintained YAML contract supports inline SQL, base anchors, and explicit anchor-to-SQL exception mappings
  - [x] the importer validates and persists inline template metadata instead of rejecting it
  - [ ] runtime services can load inline template metadata without requiring immediate migration of unchanged facets
  - [x] retained result-shape facets have a constrained `template_key` path for inner CTE composition SQL

## Work Breakdown

### 1. Authoring Contract

Objective: define the approved YAML surface for inline template authoring.

- [x] add the `sql` authoring block for inline templates, including mode, contract, base anchor, and template body
- [x] define the explicit anchor-to-SQL exception shape for facets that cannot rely on projected-anchor routes
- [x] define the constrained `template_key` property for retained result-shape facets
- [x] document the fixed placeholder sets owned by each supported compiler contract

Completion criteria: the YAML authoring shape is specific enough that the importer can validate it without relying on implicit relational reconstruction rules.

### 2. Schema And Import Validation

Objective: reject malformed inline-template authoring before any runtime rows are activated.

- [x] extend schema validation for inline SQL fields, base-anchor references, and explicit anchor-to-SQL mappings
- [x] validate allowed placeholders against the declared contract type
- [x] validate `template_key` usage for retained result-shape facets
- [x] define failure cases for missing anchors, unsupported placeholders, and inconsistent contract metadata

Completion criteria: invalid inline-template authoring fails in validation with explicit contract errors.

### 3. Runtime Persistence And Loading

Objective: persist template metadata into the existing runtime configuration model and make it loadable by the compiler path.

- [x] remove the current importer rejection of supported inline template content
- [ ] persist base-template metadata and explicit anchor-to-SQL exception data in runtime storage
- [ ] persist constrained `template_key` metadata for retained result-shape facets
- [ ] load template metadata through the active configuration and runtime service path without regressing non-template facets

Completion criteria: imported inline-template data survives authoring validation, import, and runtime loading as first-class configuration.

### 4. Compiler Entry Contract

Objective: give the compiler path one stable way to discover and consume imported template metadata.

- [ ] define the runtime contract that compiler services read for inline templates and template keys
- [ ] route discrete compiler lookup through the new template contract when template metadata is present
- [ ] keep unchanged facets on the existing relational path until later phases move them
- [ ] define the minimum runtime checks for missing template metadata, unsupported contract types, and unknown template keys

Completion criteria: compiler services can distinguish inline-template facets, retained result-shape template-key facets, and unchanged legacy-mode facets without ambiguous fallback.

### 5. Driver-Slice Readiness

Objective: prepare the first implementation-driving facets without turning Phase 1 into broad migration.

- [ ] prepare `result_facet` as the first retained result-shape driver for the `template_key` path
- [ ] prepare `family` as the first routed discrete driver for the later parity-risk proof slice
- [ ] prepare `tbl_biblio_sample_groups` as the clause-bearing joined discrete follow-on slice
- [ ] capture `geochronology` and `tbl_denormalized_measured_values_33_0` as queued non-discrete drivers for the next phase rather than pulling them into Phase 1
- [ ] capture `analysis_entity_ages` (intersect) and `sites_polygon` (geopolygon) as required restoration drivers for the next implementation phase

Completion criteria: the next implementation phase has named driver facets, including required intersect/geopolygon restoration drivers, and each facet's intended purpose is explicit.

## Progress Tracker

| Area                            | Status      | Notes                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
|---------------------------------|-------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Authoring contract              | In Progress | `sead.query.composer/Templates/facet-route-config.schema.json` now carries the inline `sql` block with canonical `body` support plus explicit projected-anchor, anchor-to-SQL exception, and constrained `template_key` shapes. Fixed placeholder-set documentation is now complete: `discrete` (`{pick_filter_sql}`, `{pick_values_sql}`), `range` (`{low}`, `{high}`, `{range_filter_sql}`), `intersect` (`{range_filter_sql}`), `geopolygon` (`{polygon_filter_sql}`, `{polygon_wkt}`, `{srid}`). |
| Schema and import validation    | In Progress | `sead.query.infra/Configuration/FacetRouteConfigurationImporter.cs` now validates inline-template metadata and `sead.query.test/IntegrationTests/Infrastructure/FacetRouteConfigurationImporterTests.cs` covers persisted rows, placeholder validation, template_key validation, unsupported contract types, type/contract mismatch, missing base anchor references, missing sql body, and unsupported sql mode failure cases.                                                                          |
| Runtime persistence and loading | In Progress | `scripts/prepare-facet-runtime-schema.sql` adds `facet.facet_template`, `sead.query.infra/Configuration/FacetTemplateRuntimeResolver.cs` reads it, and `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs` consumes it, but runtime loading remains narrow.                                                                                                                                                                                                       |
| Compiler entry contract         | In Progress | `sead.query.core/Interfaces/IFacetTemplateRuntimeResolver.cs` and `sead.query.api/Dependency.cs` add the runtime resolver contract, but only `anchor_identity` is supported so far in `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs`.                                                                                                                                                                                                                        |
| Driver-slice readiness          | Not started | Start with `result_facet`, `family`, and `tbl_biblio_sample_groups`, and queue required non-discrete restoration drivers `analysis_entity_ages` (intersect) and `sites_polygon` (geopolygon).                                                                                                                                                                                                                                                                                                        |

## Definition Of Done

- [ ] Phase 1 acceptance criteria are still accurate and fully covered by the work breakdown
- [ ] the approved YAML inline-template contract is documented in the proposal and reflected in the active implementation work
- [ ] schema and importer validation rules exist for inline templates, base anchors, explicit anchor-to-SQL exceptions, and template keys
- [ ] runtime services can load imported template metadata without regressing unchanged facets
- [ ] the first driver facets for Phase 2 are named and recorded in this task plan, including required intersect/geopolygon restoration drivers
- [ ] focused validation evidence is recorded for the contract and runtime-loading path
- [ ] any deferred behavior stays on an explicit follow-up list rather than being implied as already supported

## Validation And Testing

- validate the proposal-owned authoring shape against the maintained schema and importer rules
- run focused tests for the importer and runtime-loading path once inline-template persistence lands
- run focused compiler-contract tests for template lookup, placeholder validation, and unknown template-key failure paths
- use `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml` when the first authored inline-template slice is added

## Deliverables

| Deliverable           | Description                                                           | Status      | Link                                                                                    |
|-----------------------|-----------------------------------------------------------------------|-------------|-----------------------------------------------------------------------------------------|
| Phase 1 task plan     | Active execution tracker for Phase 1                                  | Not started | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/TASK_PLAN_PHASE_1.md`   |
| Implementation plan   | Ordered phase sequence for the inline SQL feature                     | Not started | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/IMPLEMENTATION_PLAN.md` |
| Proposal update       | Approved baseline for hybrid anchors and retained result-shape facets | Not started | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING.md`                     |
| Classification update | Backfill classification aligned with retained result-shape facets     | Not started | `docs/proposals/FACET_EXPORT_CLASSIFICATION_AND_YAML_BACKFILL.md`                       |

## Scope

**In scope**

- contract, validation, persistence, and runtime-loading work for inline templates
- constrained `template_key` support for retained result-shape facets
- naming the first driver facets for later proof slices
- explicitly queuing `analysis_entity_ages` and `sites_polygon` as required restoration drivers for the next phase

**Out of scope**

- bulk migration of the staging facet catalog
- broad parity cleanup for already-known problematic discrete slices
- full non-discrete rollout beyond naming the first queued drivers and restoration targets

## Risks And Mitigations

- Contract sprawl risk: too many ad hoc placeholders or template shapes would recreate the current indirect metadata problem. Mitigation: keep placeholders compiler-owned and fixed by contract.
- Runtime ambiguity risk: mixed inline-template and legacy-mode facets could blur fallback behavior. Mitigation: make lookup and failure modes explicit in the compiler entry contract.
- Result-template drift risk: `template_key` could become an uncontrolled second templating language. Mitigation: keep it compiler-owned and limited to named retained result-shape facets.

## Assumptions

- existing runtime template-oriented storage can be reused for Phase 1 instead of introducing a second persistence model immediately
- Phase 1 stops at contract and plumbing; it does not need to finish parity work for `family` or any non-discrete driver slice
