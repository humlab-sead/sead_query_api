# Task Plan: Phase 2 - Discrete Driver Slices

## Phase Summary

- Goal: prove the inline SQL path on discrete facets and retained result-shape facets using representative implementation-driving slices
- Focus: discrete compiler implementation, template_key path validation, routed discrete fan-out, clause-bearing joined discrete, and projected anchor route activation
- Acceptance Criteria:
  - [x] the discrete compiler can execute inline-template facets for one same-table result-shape slice, one routed discrete slice, and one clause-bearing joined discrete slice
  - [x] the `template_key` path is proven on `result_facet` and is reusable for `map_result` and `result_datasets`
  - [x] at least one known risky routed discrete slice is either parity-clean on the inline path or explicitly documented as blocked by a still-open semantic issue

## Work Breakdown

### 1. Route Template Definitions

Objective: define the missing route templates referenced by Phase 1 driver facets so projected anchor routes can be activated.

**Carried from Phase 1 deferred work**

- [x] define `sample__dataset` route template
- [x] define `sample__sites` route template
- [x] define `sample__analysis_entity` route template
- [x] define `analysis_entity__dataset` route template
- [x] define `analysis_entity__sample` route template
- [x] validate route definitions against existing graph relationships in the SEAD schema
- [x] document route join paths and anchor mappings

**Completion criteria:** driver facets can use projected anchor routes instead of identity-mode-only workarounds.

**Location:** `sead.query.composer/Templates/facet_configuration.yml` route_templates section

**Dependencies:** None; this is pure authoring work that unblocks route projection validation.

### 2. Discrete Compiler Implementation

Objective: implement compiler support for executing inline SQL templates on discrete facets.

- [x] implement template lookup in discrete compiler path
- [x] implement placeholder substitution for discrete contract (`{pick_filter_sql}`, `{pick_values_sql}`)
- [x] implement base-anchor template execution with identity anchor mode
- [x] implement projected anchor route resolution and SQL generation
- [x] implement explicit anchor-to-SQL exception handling
- [x] add compiler validation for unsupported contract types
- [x] add compiler validation for missing or unknown template keys
- [x] add compiler validation for placeholder mismatch

**Completion criteria:** the discrete compiler can execute inline-template facets with base anchors, projected anchors, and explicit anchor-to-SQL exceptions.

**Location:** `sead.query.composer/QueryComposer/` discrete compiler services

**Dependencies:** Phase 1 runtime loading and compiler entry contract are complete.

### 3. Template Key Path Implementation

Objective: prove the `template_key` path for retained result-shape facets using `result_facet` as the primary driver.

- [x] implement template key lookup in result projection handoff builder
- [x] implement `anchor_identity` template execution for result-shape facets
- [x] validate that `result_facet` produces correct SQL through the template_key path
- [x] add focused unit tests for template key lookup and execution
- [x] add focused integration test for `result_facet` with `anchor_identity`
- [x] document the template_key contract for future use by `map_result` and `result_datasets`

**Completion criteria:** `result_facet` works through the `template_key` path, proving the contract for other result-shape facets.

**Location:** `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs`

**Dependencies:** Phase 1 runtime loading and compiler entry contract are complete.

### 4. Routed Discrete Driver Validation

Objective: prove routed discrete execution with projected anchors using `family` as the known parity-risk driver.

- [x] uncomment projected anchor routes in `family` facet definition
- [x] implement route projection for `family` (analysis_entity → dataset, analysis_entity → sample)
- [x] validate `family` SQL output against expected structure
- [x] run focused parity check for `family` against legacy relational path
- [x] document any semantic discrepancies or blocked cases
- [x] add focused unit tests for routed discrete template execution
- [x] add focused integration test for `family` with projected anchors

**Completion criteria:** `family` produces correct or documented-as-blocked SQL through the routed discrete inline-template path.

**Location:** `sead.query.composer/Templates/facet_configuration.yml` (authoring), discrete compiler services (runtime)

**Dependencies:** Route template definitions (work item 1) and discrete compiler implementation (work item 2) are complete.

### 5. Joined Discrete Driver Validation

Objective: prove clause-bearing joined discrete execution using `tbl_biblio_sample_groups` as the driver.

- [x] uncomment projected anchor routes in `tbl_biblio_sample_groups` facet definition
- [x] implement route projection for `tbl_biblio_sample_groups` (sample → dataset, sample → sites, sample → analysis_entity)
- [x] validate multi-table join structure in generated SQL
- [x] validate clause-bearing filter placement in generated SQL
- [x] run focused parity check for `tbl_biblio_sample_groups` against legacy relational path
- [x] document any semantic discrepancies or blocked cases
- [x] add focused unit tests for joined discrete template execution
- [x] document the projected-anchor integration slice for `tbl_biblio_sample_groups` as blocked and guard it with focused tests

**Completion criteria:** `tbl_biblio_sample_groups` produces correct or documented-as-blocked SQL through the joined discrete inline-template path.

**Location:** `sead.query.composer/Templates/facet_configuration.yml` (authoring), discrete compiler services (runtime)

**Dependencies:** Route template definitions (work item 1) and discrete compiler implementation (work item 2) are complete.

### 6. Anchor-to-SQL Exception Demonstration

Objective: add at least one facet demonstrating explicit anchor-to-SQL override to validate the exception contract.

**Carried from Phase 1 deferred work**

- [x] identify a candidate facet that requires explicit anchor-to-SQL mapping (cannot rely on projected-anchor routes)
- [x] author the explicit anchor-to-SQL exception in `facet_configuration.yml`
- [x] implement exception lookup and SQL override in discrete compiler
- [x] validate that the exception facet produces correct SQL
- [x] add focused unit test for anchor-to-SQL exception handling
- [x] add focused integration test for the exception facet
- [x] document the exception pattern for future use

**Completion criteria:** at least one facet uses explicit anchor-to-SQL override and produces correct SQL through the discrete inline-template path.

**Location:** `sead.query.composer/Templates/facet_configuration.yml` (authoring), discrete compiler services (runtime)

**Dependencies:** Discrete compiler implementation (work item 2) is complete.

### 7. Relational Fallback Validation

Objective: ensure facets not yet migrated to inline SQL continue to work through the existing relational path.

- [x] run regression tests for unchanged facets
- [x] validate that facets without template metadata still use relational fallback
- [x] document the relational fallback precedence order
- [x] add focused tests for relational fallback path
- [x] ensure no accidental migration of facets not intended for Phase 2

**Completion criteria:** unchanged facets continue to work with zero behavior change; relational fallback is explicit and testable.

**Location:** discrete compiler services, integration tests

**Dependencies:** Discrete compiler implementation (work item 2) is complete.

## Progress Tracker

| Area                             | Status   | Notes                                                                                                                                                                                                                     |
|----------------------------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Route template definitions       | Complete | Added `sample` and `analysis_entity` route templates with deferred Phase 1 generated routes; named path and route-template path definitions have been manually verified.                                                  |
| Discrete compiler implementation | Complete | Implemented discrete template lookup, placeholder substitution, base-anchor template execution, projected-anchor SQL generation, explicit anchor overrides, and focused validation for contract/key/placeholder failures. |
| Template key path                | Complete | `anchor_identity` runtime path is active with unit + focused integration coverage, including unsupported template-key failure guard and contract notes for retained result-shape migration.                               |
| Routed discrete driver           | Complete | Unit + focused integration coverage and parity checks passed for `family`; no additional semantic discrepancy observed in current environment.                                                                            |
| Joined discrete driver           | Complete | Unit + focused integration coverage validated joined clause-bearing SQL for country-predicate slice; unsupported projected cross-target and result-path behaviors are explicitly documented and guarded.                  |
| Anchor-to-SQL exception          | Complete | Added explicit `sql_override` for `family` dataset anchor with unit + focused live integration evidence, and documented the reusable exception authoring pattern.                                                         |
| Relational fallback validation   | Complete | Regression + focused unit coverage confirms no-template requests continue through relational predicate inputs in composed result and facet-content paths.                                                                 |

## Validation Evidence Summary

### Section 1: Route Template Definitions
- Added route template `sample` with generated routes: `sample__dataset`, `sample__sites`, `sample__analysis_entity`.
- Added route template `analysis_entity` with generated routes: `analysis_entity__dataset`, `analysis_entity__sample`.
- Named path definitions and route-template path definitions were manually reviewed and verified as semantically correct for the intended table traversals.
- The route inventory itself now documents join paths and anchor mappings directly in `facet_configuration.yml` through:
  - top-level `paths` entries for shared traversal fragments
  - per-template anchor `path` definitions under `route_templates`
- Build verification passed via `make build`.
- `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/facet_configuration.yml` exits with code 1 in the current environment.
- Focused importer test run (`dotnet test --filter FullyQualifiedName~FacetRouteConfigurationImporterTests`) shows a schema precondition failure: `facet 'result_facet' group 'result' could not be resolved in the current facet schema`.
- Full route-template semantic validation is therefore blocked on local facet-schema baseline alignment.
- Runtime schema preparation now gets past authentication, but still stops on an existing facet-schema shape mismatch (`facet_template_id` column missing on `facet.facet_template` in the current database copy). Direct runtime import validation remains blocked until that runtime schema baseline is aligned.

### Section 2: Discrete Compiler Implementation
- Implemented discrete inline-template lookup and SQL composition in:
  - `sead.query.composer/QueryComposer/Services/ComposedFacetContentFilterQueryFactory.cs`
  - `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs`
- Added shared template SQL composition support in:
  - `sead.query.composer/QueryComposer/Services/ComposedFacetContentSupport.cs`
- Added placeholder rendering for discrete templates:
  - `{pick_filter_sql}` now resolves to category-based pick predicates (or `1=1` when no picks)
  - `{pick_values_sql}` now resolves to formatted pick literal lists
- Added compiler validation for unresolved template placeholders in shared discrete rendering path (`ComposedFacetContentSupport.RenderDiscreteTemplateSql`) with explicit failure for unsupported placeholder tokens.
- Added base-template normalization (`category_id` → `source_id`, `anchor_id` → `target_id`) and projected-anchor SQL join composition.
- Updated composed-result preflight to accept discrete predicates via template base+projection (not only anchor-specific overrides) in `ComposedResultProjectionHandoffBuilder`.
- Added focused unit validation coverage:
  - `ComposedFacetContentServiceTests.Create_WithUnsupportedTemplateContract_ThrowsInvalidOperationException`
  - `ComposedFacetContentServiceTests.Load_WithSnapshotBackedExplicitAnchorSqlAndUnknownPlaceholder_ThrowsInvalidOperationException`
  - `ComposedResultProjectionHandoffBuilderDiagnosticsTests.Build_WithUnsupportedTemplateKey_ThrowsInvalidOperationException`
  - `ComposedResultProjectionHandoffBuilderDiagnosticsTests.Build_WithDiscreteAnchorOverrideAndUnknownPlaceholder_ThrowsInvalidOperationException`
- Focused tests pass:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedFacetContentServiceTests|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests|FullyQualifiedName~DiscreteFacetPredicateResolverTests"`
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests|FullyQualifiedName~Load_BiblioSampleGroupsFilteredTabularResult_ThrowsUnsupportedClauseError"` (7 passed)
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedFacetContentServiceTests|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"` (40 passed)

### Section 3: Template Key Path Implementation
- `result_facet` template key lookup and `anchor_identity` execution are active in `ComposedResultProjectionHandoffBuilder`.
- Template-key contract documented for future `map_result` and `result_datasets` migration:
  - result-shape facets provide `template_key` that resolves to a runtime template snapshot
  - `anchor_identity` key binds result-shape predicates to `analysis_entity` anchor semantics
  - template snapshot is preferred over legacy key/anchor lookups; unknown template keys fail fast with `InvalidOperationException`
  - when no template SQL can be composed, composed runtime uses relational predicate metadata as explicit fallback
- Unit evidence:
  - `ComposedResultProjectionHandoffBuilderDiagnosticsTests.Build_WithTemplateSnapshot_UsesSnapshotInsteadOfLegacyTemplateLookups`
  - `ComposedResultProjectionHandoffBuilderDiagnosticsTests.Build_WithUnsupportedTemplateKey_ThrowsInvalidOperationException`
- Focused integration evidence:
  - `ResultLoadServiceTests.Load_ResultFacetAnchorIdentityTemplatePath_UsesAnalysisEntityAnchorWithoutTargetRoute`
- Focused test pass:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"`
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_ResultFacetAnchorIdentityTemplatePath_UsesAnalysisEntityAnchorWithoutTargetRoute|FullyQualifiedName~Load_TargetOnlyGenusTabularResult_UsesComposedFilterSql|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"` (9 passed)

### Section 4: Routed Discrete Driver Validation
- Enabled projected anchor routes in `family` facet for `dataset` and `sample`.
- Added unit test coverage proving projected-anchor SQL generation from base template path in `ComposedResultProjectionHandoffBuilderDiagnosticsTests`.
- Focused live parity validation passed:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedCountryPredicateFamilySlice_"`
- Added focused result-path composed SQL assertion:
  - `ResultLoadServiceTests.Load_FamilyFilteredTabularResult_UsesTemplateDrivenComposedFilterSql`
- Strengthened focused integration assertion for `family` tabular result path:
  - confirms composed filter CTE and analysis-entity join are present
  - confirms map-style `target_route` CTE is not present for this tabular result path
- Focused family + diagnostics verification passed:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_FamilyFilteredTabularResult_UsesTemplateDrivenComposedFilterSql|FullyQualifiedName~FacetContentService_ComposedCountryPredicateFamilySlice_|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"` (9 passed)
- No additional semantic discrepancies were observed for `family` in the current environment; no blocker is recorded for this driver.

### Section 5: Joined Discrete Driver Validation
- Enabled projected anchor routes in `tbl_biblio_sample_groups` for `dataset`, `sites`, and `analysis_entity`.
- Added unit test coverage proving projected-anchor SQL generation from base template path in `ComposedFacetContentServiceTests`.
- Added focused joined-discrete route-chain coverage (`sample -> analysis_entity -> dataset`) for `tbl_biblio_sample_groups` in `ComposedFacetContentServiceTests.Create_WithJoinedDiscreteBaseTemplate_UsesSampleToDatasetProjectionRouteChain`.
- Validated multi-table join structure and clause-bearing placement in live facet-content SQL for country-predicate slice:
  - `FacetLoadServiceTests.FacetContentService_ComposedCountryPredicateBiblioSampleGroupsSlice_UsesComposedFacetContentQuery`
  - asserted fragments include `tbl_biblio.biblio_id`, `facet.view_sample_group_references.biblio_id is not null`, and `X_0.location_type_id=1`.
- Focused live parity validation passed:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedCountryPredicateBiblioSampleGroupsSlice_"`
- Documented discrepancy/blocker in result projection path:
  - `ResultLoadServiceTests.Load_BiblioSampleGroupsFilteredTabularResult_ThrowsUnsupportedClauseError` verifies current behavior that composed result projection rejects `tbl_biblio_sample_groups` due unsupported clause normalization.
- Added explicit live guard for currently unsupported projected-anchor cross-target slice:
  - `FacetLoadServiceTests.FacetContentService_ComposedBiblioSampleGroupsPredicateDatasetMethodsSlice_ThrowsUnsupportedClauseError`
  - URI `dataset_methods:tbl_biblio_sample_groups@1,2/dataset_methods` currently rejects with composed predicate-clause unsupported error.
- Added unit proof that composed-result preflight now allows unnormalizable discrete clauses when template base+projection SQL is available:
  - `ComposedResultProjectionHandoffBuilderDiagnosticsTests.Build_WithDiscreteTemplateAndUnnormalizableClause_ComposesFromTemplatePath`
- Remaining blocker to flip the live guard test: runtime template metadata import still cannot complete in this environment because the runtime facet schema copy is not yet aligned (`facet_template_id` missing on `facet.facet_template`).
- Focused joined-discrete verification passed:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedBiblioSampleGroupsPredicateDatasetMethodsSlice_ThrowsUnsupportedClauseError|FullyQualifiedName~FacetContentService_ComposedCountryPredicateBiblioSampleGroupsSlice_|FullyQualifiedName~ComposedFacetContentServiceTests"` (33 passed)

### Section 7: Relational Fallback Validation
- Documented fallback precedence order from current composed runtime implementation:
  - 1) try template-driven SQL composition (`TryCreateDiscreteTemplateSql`) using anchor override or base+projection route
  - 2) if template SQL is unavailable, resolve relational metadata (`sourceKeyColumn`, criteria, and route)
  - 3) execute relational predicate resolution via `IDiscreteFacetPredicateResolver.ResolveSql`
- Cross-layer composed result checks for country-filter path passed:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_CountryFilteredTabularResult_UsesComposedFilterSql|FullyQualifiedName~Load_CountryFilteredMapResult_UsesComposedFilterSql"`
- Focused no-template fallback unit coverage now includes both composed paths:
  - `ComposedResultProjectionHandoffBuilderDiagnosticsTests.Build_WithoutTemplateMetadata_UsesRelationalFallbackPath`
  - `ComposedFacetContentServiceTests.Create_WithoutTemplateMetadata_UsesRelationalFallbackPredicateInputs`
- Focused verification command passed:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedFacetContentServiceTests|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"` (37 passed)
- Unchanged-facet live regression command passed:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~Load_CountryFilteredTabularResult_UsesComposedFilterSql|FullyQualifiedName~Load_CountryFilteredMapResult_UsesComposedFilterSql"` (2 passed)

### Section 6: Anchor-to-SQL Exception Demonstration
- Added explicit anchor-to-SQL override on `family` facet (`dataset` anchor) in `sead.query.composer/Templates/facet_configuration.yml` using `sql_override` with `{pick_filter_sql}`.
- Exception lookup + override usage is implemented in composed runtime paths through shared helper logic in `ComposedFacetContentSupport.TryCreateDiscreteTemplateSql`.
- Added focused unit coverage:
  - `ComposedFacetContentServiceTests.Load_WithSnapshotBackedExplicitAnchorSql_RendersPlaceholderSql`
  - `ComposedResultProjectionHandoffBuilderDiagnosticsTests.Build_WithDiscreteAnchorOverride_RendersPlaceholderSql`
- Added focused live integration coverage:
  - `FacetLoadServiceTests.FacetContentService_ComposedFamilyPredicateDatasetMethodsSlice_UsesAnchorSqlOverride`
  - verifies composed facet-content query execution for a family predicate resolved through dataset anchor and checks override-style SQL markers (dataset join present, no projected-template alias marker)
- Focused tests pass:
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedFacetContentServiceTests|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"` (34 passed)
  - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetContentService_ComposedFamilyPredicateDatasetMethodsSlice_UsesAnchorSqlOverride|FullyQualifiedName~FacetContentService_ComposedCountryPredicateFamilySlice_UsesComposedFacetContentQuery|FullyQualifiedName~ComposedFacetContentServiceTests"` (32 passed)
- Exception pattern for future facets:
  - keep base template anchored to the canonical authoring anchor
  - add `sql_override` under a specific projected anchor only when route projection is insufficient
  - keep `{pick_filter_sql}` placeholder in override SQL so pick rendering stays centralized
  - cover override behavior with one unit test and one focused live integration slice before wider rollout

## Carried Forward From Phase 1

### Route Template Definitions
**Status:** Required for Phase 2 route projection validation

**Deferred routes:**
- `sample__dataset`
- `sample__sites`
- `sample__analysis_entity`
- `analysis_entity__dataset`
- `analysis_entity__sample`

**Current state:** Route templates added and projected anchor routes enabled in Phase 2 driver facets.

**Phase 2 action:** Define these route templates and uncomment projected anchor routes in driver facets

### Anchor-to-SQL Exception Mapping
**Status:** Schema defined, importer validates, not yet exercised by any facet

**Phase 2 action:** Add at least one facet demonstrating explicit anchor-to-SQL override

### Runtime Template Key Validation
**Status:** Optional defensive validation deferred

**Phase 2 decision:** Keep as optional; importer validation is sufficient for Phase 2

**Follow-up:** Consider adding runtime assertion in handoff builder if production issues emerge

## Deferred to Phase 3

### Non-Discrete Compiler Support
**Description:** Range, intersect, geopolygon inline SQL execution deferred to Phase 3

**Queued drivers:**
- `geochronology` (range, routed)
- `tbl_denormalized_measured_values_33_0` (range, denormalized)

**Restoration drivers:**
- `analysis_entity_ages` (intersect)
- `sites_polygon` (geopolygon)

**Impact:** Phase 2 proves discrete contract only; non-discrete restoration in Phase 3

### Retained Result-Shape Migration
**Description:** `map_result` and `result_datasets` migration to template_key path

**Status:** Contract proven on `result_facet`; migration deferred to Phase 4

**Impact:** Phase 2 proves the contract; Phase 4 executes the migration

## Definition Of Done

- [x] Phase 2 acceptance criteria are fully covered by the work breakdown
- [x] route template definitions exist for all deferred Phase 1 routes
- [x] discrete compiler can execute inline-template facets with base anchors, projected anchors, and explicit anchor-to-SQL exceptions
- [x] `result_facet` works through the `template_key` path
- [x] `family` produces correct or documented-as-blocked SQL through routed discrete inline-template path
- [x] `tbl_biblio_sample_groups` produces correct or documented-as-blocked SQL through joined discrete inline-template path
- [x] at least one facet demonstrates explicit anchor-to-SQL override
- [x] unchanged facets continue to work through relational fallback
- [x] focused validation evidence is recorded for each driver slice
- [x] any deferred behavior or blocked cases are explicitly documented

## Validation And Testing

- run `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/facet_configuration.yml` after route template definitions are complete
- run focused unit tests for discrete compiler, template key lookup, route projection, and anchor-to-SQL exceptions
- run focused integration tests for `result_facet`, `family`, and `tbl_biblio_sample_groups`
- run focused parity checks for driver facets against legacy relational path
- run regression tests for unchanged facets to validate relational fallback
- document any semantic discrepancies or blocked cases explicitly

## Deliverables

| Deliverable                 | Description                                                    | Status   | Link                                                                                                                |
|-----------------------------|----------------------------------------------------------------|----------|---------------------------------------------------------------------------------------------------------------------|
| Phase 2 task plan           | Active execution tracker for Phase 2                           | Complete | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/PHASE_2_TASK_PLAN.md`                               |
| Route template definitions  | Missing route templates for projected anchor validation        | Complete | `sead.query.composer/Templates/facet_configuration.yml`                                                                       |
| Discrete compiler           | Inline SQL template execution for discrete facets              | Complete | `sead.query.composer/QueryComposer/` discrete compiler services                                                     |
| Template key implementation | `anchor_identity` execution for result-shape facets            | Complete | `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs`                              |
| Driver facet validation     | Parity checks and SQL validation for Phase 2 driver facets     | Complete | `sead.query.test/` focused integration tests                                                                        |
| Implementation plan         | Ordered phase sequence for the inline SQL feature              | Exists   | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/IMPLEMENTATION_PLAN.md`                             |
| Proposal                    | Approved baseline for hybrid anchors and retained result-shape | Complete | `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING.md` |

## Implementation Notes

### Discrete Compiler Entry Points

The discrete compiler implementation likely needs changes in:
- `sead.query.composer/QueryComposer/Services/ComposedFacetContentFilterQueryFactory.cs` (facet content SQL generation)
- `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs` (result projection handoff)
- Route projection services (if not already separated)

### Template Lookup Precedence

The Phase 1 implementation established this lookup order:
1. Check for `template_key` (result-shape facets only)
2. Check for inline SQL template (base anchor + optional projected anchors)
3. Fall back to relational reconstruction (unchanged facets)

Phase 2 must honor this precedence in the discrete compiler path.

### Parity Risk

`family` is explicitly called out as a known parity-risk slice. Phase 2 validation should:
- compare generated SQL structure against legacy path
- document any semantic differences
- explicitly mark as blocked if parity cannot be achieved
- do not let parity issues block the broader Phase 2 completion if the contract itself is proven

### Route Projection Strategy

Route projection should use:
- base anchor template as the starting CTE or subquery
- projected anchor routes to fan out to related entities
- explicit anchor-to-SQL exceptions when projection is not sufficient

Phase 2 proves this pattern on `family` (2 projections) and `tbl_biblio_sample_groups` (3 projections).

### Anchor-to-SQL Exception Candidate

Good candidates for anchor-to-SQL exception demonstration:
- facets with complex joins that cannot be expressed through simple route projection
- facets with denormalized or function-backed anchor tables
- facets with multi-step indirect relationships

The candidate should be small enough to validate quickly but realistic enough to prove the exception contract.
