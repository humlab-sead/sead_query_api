# Task Plan: Phase 1 - Authoring Contract And Runtime Plumbing

## Phase Summary

- Goal: make inline SQL a first-class authored, imported, and runtime-loadable facet path while keeping unchanged facets on the existing configuration path
- Focus: YAML contract, schema validation, importer persistence, runtime loading, and constrained `template_key` support for retained result-shape facets
- Acceptance Criteria:
  - [x] the maintained YAML contract supports inline SQL, base anchors, and explicit anchor-to-SQL exception mappings
  - [x] the importer validates and persists inline template metadata instead of rejecting it
  - [x] runtime services can load inline template metadata without requiring immediate migration of unchanged facets
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
- [x] persist base-template metadata and explicit anchor-to-SQL exception data in runtime storage
- [x] persist constrained `template_key` metadata for retained result-shape facets
- [x] load template metadata through the active configuration and runtime service path without regressing non-template facets

Completion criteria: imported inline-template data survives authoring validation, import, and runtime loading as first-class configuration.

### 4. Compiler Entry Contract

Objective: give the compiler path one stable way to discover and consume imported template metadata.

- [x] define the runtime contract that compiler services read for inline templates and template keys

  Implementation notes:
  - document the lookup sequence for each compiler type (discrete, range, intersect, geopolygon)
  - specify the precedence order: template_key → inline SQL (base_sql + anchor_sql) → relational fallback
  - define what each compiler must validate when template metadata is present vs. absent
  - capture this as a structured checklist in this section, not a separate document

  **Runtime Compiler Contract Checklist**:

  **For result projection (e.g., `ComposedResultProjectionHandoffBuilder`)**:
  - [x] if `template_key` is present, use `CreateTemplateKeySql(templateSnapshot, anchorTable, anchorKeyColumn)` to generate anchor-identity SQL
  - [x] if `template_key` is absent but `HasAnchorSql(facet, anchorTable)` is true, use `GetAnchorSql(facet, anchorTable)` as explicit anchor SQL
  - [x] if both are absent, fall back to relational reconstruction via `AnchorTemplate` with source key columns and criteria
  - [ ] validate that `template_key` is only used on `result_facet`, `map_result`, or `result_datasets` facets (validation exists in importer, runtime should enforce or document assumption)
  - [x] validate that the only supported `template_key` value is `"anchor_identity"` — throw `InvalidOperationException` for unknown keys

  **For facet content (e.g., `ComposedFacetContentFilterQueryFactory`)**:
  - [x] call `GetTemplateSnapshot(sourceFacet) ?? FacetTemplateRuntimeSnapshot.Empty` to retrieve base SQL and anchor SQL map
  - [x] if `AnchorSqlByTable.TryGetValue(anchorTable, out var explicitSql)` succeeds, populate `AnchorTemplate.ExplicitSql` with the returned SQL
  - [x] if no anchor SQL exists for the requested anchor, populate `AnchorTemplate.ExplicitSql` as empty string and rely on relational reconstruction
  - [x] the `AnchorTemplate` already has precedence logic: if `ExplicitSql` is non-empty, use it; otherwise reconstruct from source key and criteria
  - [x] do NOT use `template_key` in facet content paths — that's result-projection-only

  **For all compiler paths**:
  - [x] if a facet has no template metadata and no relational metadata, fail with a clear diagnostic (handled by `ResolvePredicateSourceKeyColumn` throwing `InvalidOperationException` when relational metadata is missing and template metadata is not present)
  - [x] log or throw when `template_contract` in the database doesn't match any known compiler type (`discrete`, `range`, `intersect`, `geopolygon`) — added `ValidateTemplateContract` in resolver
  - [x] ensure facets without template metadata continue to use the existing relational path with zero behavior change (compiler services now check for template metadata first, then fall back to relational)

- [x] route discrete compiler lookup through the new template contract when template metadata is present
- [x] keep unchanged facets on the existing relational path until later phases move them
- [x] define the minimum runtime checks for missing template metadata, unsupported contract types, and unknown template keys

  Implementation notes:
  - added `ValidateTemplateContract` to `FacetTemplateRuntimeResolver` — throws when `template_contract` is not in `SupportedTemplateContracts` (`discrete`, `range`, `intersect`, `geopolygon`)
  - refactored `CreateDiscretePredicateQueryPlan` in both `ComposedResultProjectionHandoffBuilder` and `ComposedFacetContentFilterQueryFactory` to check for explicit anchor SQL before resolving relational metadata
  - facets with template metadata no longer throw on missing relational metadata; facets without template metadata still throw clear diagnostics when relational metadata is missing
  - added test `Build_WithoutTemplateMetadata_UsesRelationalFallbackPath` validating that facets without template metadata use the relational path with empty `ExplicitSql` in the `AnchorTemplate`

Completion criteria: compiler services can distinguish inline-template facets, retained result-shape template-key facets, and unchanged legacy-mode facets without ambiguous fallback.

### 5. Driver-Slice Readiness

Objective: prepare the first implementation-driving facets without turning Phase 1 into broad migration.

- [x] prepare `result_facet` as the first retained result-shape driver for the `template_key` path
- [x] prepare `family` as the first routed discrete driver for the later parity-risk proof slice
- [x] prepare `tbl_biblio_sample_groups` as the clause-bearing joined discrete follow-on slice
- [x] capture `geochronology` and `tbl_denormalized_measured_values_33_0` as queued non-discrete drivers for the next phase rather than pulling them into Phase 1
- [x] capture `analysis_entity_ages` (intersect) and `sites_polygon` (geopolygon) as required restoration drivers for the next implementation phase

Completion criteria: the next implementation phase has named driver facets, including required intersect/geopolygon restoration drivers, and each facet's intended purpose is explicit.

Implementation notes:
- added `result_facet` to `sead.query.composer/Templates/route_v1.yaml` with `template_key: anchor_identity` demonstrating the retained result-shape path
- added `family` as routed discrete driver with inline SQL template using `base_anchor: analysis_entity` and projected anchors for `dataset` and `sample`
- added `tbl_biblio_sample_groups` as clause-bearing joined discrete driver with inline SQL template using `base_anchor: sample` and multi-table join structure
- updated `analysis_entity_ages` and `sites_polygon` facet descriptions to document their roles as Phase 3 restoration drivers
- added YAML comment block at end of facets section documenting queued Phase 3 non-discrete drivers (`geochronology`, `tbl_denormalized_measured_values_33_0`) and restoration drivers (`analysis_entity_ages`, `sites_polygon`) with explicit purpose statements
- commented out anchor projection routes in driver facets pending Phase 2 route template definitions (sample__dataset, sample__sites, sample__analysis_entity, analysis_entity__dataset, analysis_entity__sample)

## Progress Tracker

| Area                            | Status   | Notes                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
|---------------------------------|----------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Authoring contract              | Complete | `sead.query.composer/Templates/facet-route-config.schema.json` now carries the inline `sql` block with canonical `body` support plus explicit projected-anchor, anchor-to-SQL exception, and constrained `template_key` shapes. Fixed placeholder-set documentation is now complete: `discrete` (`{pick_filter_sql}`, `{pick_values_sql}`), `range` (`{low}`, `{high}`, `{range_filter_sql}`), `intersect` (`{range_filter_sql}`), `geopolygon` (`{polygon_filter_sql}`, `{polygon_wkt}`, `{srid}`). |
| Schema and import validation    | Complete | `sead.query.infra/Configuration/FacetRouteConfigurationImporter.cs` now validates inline-template metadata and `sead.query.test/IntegrationTests/Infrastructure/FacetRouteConfigurationImporterTests.cs` covers persisted rows, placeholder validation, template_key validation, unsupported contract types, type/contract mismatch, missing base anchor references, missing sql body, and unsupported sql mode failure cases.                                                                       |
| Runtime persistence and loading | Complete | `scripts/prepare-facet-runtime-schema.sql` adds `facet.facet_template`, `sead.query.infra/Configuration/FacetTemplateRuntimeResolver.cs` loads a runtime snapshot, and both `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs` and `sead.query.composer/QueryComposer/Services/ComposedFacetContentFilterQueryFactory.cs` now consume it.                                                                                                                        |
| Compiler entry contract         | Complete | `sead.query.core/Interfaces/IFacetTemplateRuntimeResolver.cs` and `sead.query.api/Dependency.cs` add the runtime resolver contract, and the composer paths now read `anchor_identity` from the runtime snapshot rather than separate lookups.                                                                                                                                                                                                                                                        |
| Driver-slice readiness          | Complete | `result_facet`, `family`, and `tbl_biblio_sample_groups` added to `route_v1.yaml` as Phase 1 drivers. `analysis_entity_ages` and `sites_polygon` updated as Phase 3 restoration drivers. `geochronology` and `tbl_denormalized_measured_values_33_0` documented as queued Phase 3 non-discrete drivers.                                                                                                                                                                                              |

## Validation Evidence Summary

### Section 1: Authoring Contract
- ✅ Schema validation implemented in `facet-route-config.schema.json`
- ✅ `sql` block structure: mode, contract, base_anchor, body
- ✅ `template_key` property with enum constraint to result facets
- ✅ Anchor-to-SQL exception shape defined (anchor + route + sql_override)
- ✅ Placeholder documentation complete for all contract types

### Section 2: Schema and Import Validation
- ✅ 9 test methods in `FacetRouteConfigurationImporterTests.cs`
- ✅ Covers: placeholder validation, template_key validation, unsupported contract types, type/contract mismatch, missing base anchor references, missing SQL body, unsupported SQL mode
- ✅ Importer rejects malformed inline-template authoring with explicit errors

### Section 3: Runtime Persistence and Loading
- ✅ Runtime resolver implemented: `FacetTemplateRuntimeResolver.cs`
- ✅ Integration tests: `FacetTemplateRuntimeResolverTests.cs`
- ✅ Template metadata persisted to `facet.facet_template` table
- ✅ Fallback path preserved for non-template facets

### Section 4: Compiler Entry Contract
- ✅ Result projection handoff builder consumes template metadata
- ✅ Facet content filter factory consumes template metadata
- ✅ Template lookup precedence: template_key → inline SQL → relational fallback
- ✅ Unknown template key validation (throws `InvalidOperationException`)
- ✅ Unsupported template contract validation (`ValidateTemplateContract`)
- ⚠️ Runtime enforcement of template_key restriction to result facets (validated at import only, runtime assumes compliance)

### Section 5: Driver-Slice Readiness
- ✅ 3 driver facets added to `route_v1.yaml`: `result_facet`, `family`, `tbl_biblio_sample_groups`
- ✅ Restoration drivers identified: `analysis_entity_ages`, `sites_polygon`
- ✅ Queued Phase 3 drivers documented: `geochronology`, `tbl_denormalized_measured_values_33_0`

## Deferred to Phase 2

### Route Template Definitions
**Description:** Driver facets reference routes not yet defined in route_templates section

**Deferred routes:**
- `sample__dataset`, `sample__sites`, `sample__analysis_entity`
- `analysis_entity__dataset`, `analysis_entity__sample`

**Current workaround:** Driver facets use identity mode only; projected anchor routes commented out with TODO markers

**Impact:** Low; identity mode proves the base template contract. Route projection will be validated in Phase 2.

**Location:** `sead.query.composer/Templates/route_v1.yaml` lines ~408, ~454, ~498

### Anchor-to-SQL Exception Mapping
**Description:** Schema defines explicit anchor-to-SQL exception shape but no driver facet exercises it

**Status:** Defined in schema, validated by importer, not yet used

**Impact:** None for Phase 1; will be validated when first exception facet is added in Phase 2

**Follow-up:** Add at least one facet demonstrating explicit anchor SQL override in Phase 2 discrete implementation

### Runtime Template Key Validation
**Description:** Importer validates template_key is only used on result facets; runtime assumes compliance

**Current state:** Schema constraint + importer validation sufficient; runtime does not re-validate

**Impact:** Low risk; imported configuration is already validated

**Follow-up:** Consider adding runtime assertion in handoff builder if defensive validation is desired

### Non-Discrete Compiler Support
**Description:** Range, intersect, geopolygon inline SQL execution deferred to Phase 3

**Queued drivers:**
- `geochronology` (range, routed)
- `tbl_denormalized_measured_values_33_0` (range, denormalized)

**Restoration drivers:**
- `analysis_entity_ages` (intersect)
- `sites_polygon` (geopolygon)

**Impact:** Phase 1 proves discrete contract only; non-discrete restoration in Phase 3

## Definition Of Done

- [x] Phase 1 acceptance criteria are still accurate and fully covered by the work breakdown
- [x] the approved YAML inline-template contract is documented in the proposal and reflected in the active implementation work
- [x] schema and importer validation rules exist for inline templates, base anchors, explicit anchor-to-SQL exceptions, and template keys
- [x] runtime services can load imported template metadata without regressing unchanged facets
- [x] the first driver facets for Phase 2 are named and recorded in this task plan, including required intersect/geopolygon restoration drivers
- [x] focused validation evidence is recorded for the contract and runtime-loading path
- [x] any deferred behavior stays on an explicit follow-up list rather than being implied as already supported

## Validation And Testing

- validate the proposal-owned authoring shape against the maintained schema and importer rules
- run focused tests for the importer and runtime-loading path once inline-template persistence lands
- run focused compiler-contract tests for template lookup, placeholder validation, and unknown template-key failure paths
- validate the snapshot-backed composer path for both result projection and facet content
- use `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml` when the first authored inline-template slice is added

## Deliverables

| Deliverable           | Description                                                           | Status      | Link                                                                                                                |
|-----------------------|-----------------------------------------------------------------------|-------------|---------------------------------------------------------------------------------------------------------------------|
| Phase 1 task plan     | Active execution tracker for Phase 1                                  | Complete    | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/PHASE_1_TASK_PLAN.md`                               |
| Implementation plan   | Ordered phase sequence for the inline SQL feature                     | Exists      | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/IMPLEMENTATION_PLAN.md`                             |
| Proposal update       | Approved baseline for hybrid anchors and retained result-shape facets | Complete    | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING.md` |
| Classification update | Backfill classification aligned with retained result-shape facets     | Not started | `docs/proposals/FACET_EXPORT_CLASSIFICATION_AND_YAML_BACKFILL.md`                                                   |

## Scope

**In scope**

- contract, validation, persistence, and runtime-loading work for inline templates
- constrained `template_key` support for retained result-shape facets
- naming the first driver facets for later proof slices
- explicitly queuing `analysis_entity_ages` and `sites_polygon` as required restoration drivers for the next phase
- snapshot-backed composer consumption for result projection and facet content

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
