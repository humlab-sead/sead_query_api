# Configuration Diagnostics And Validation Boundary

## Status

- In progress
- Scope: Phase 5 configuration diagnostics, unsupported-boundary behavior, and repeatable validation boundaries

## Summary

Phase 5 already has several real validation seams, but they are spread across YAML authoring, importer behavior, startup validation, runtime fallback boundaries, and smoke gates.

The immediate Phase 5 need is not a new validation concept. It is one explicit inventory of what currently fails where, which boundary catches it, and which gaps still remain before default cutover.

## Current Failure-Mode Inventory

### Authoring Structure Failures

- Invalid YAML structure, unknown top-level keys, and malformed field shapes should fail at authoring time through `sead.query.composer/Templates/facet-route-config.schema.json` and `--validate-facet-config`.
- This is the earliest validation boundary for malformed authoring input.
- The current validation path now also catches semantic route and anchor drift before import, including anchor-table mismatches in generated routes and unresolved facet-anchor route bindings.

### Route-Macro Resolution Failures

- Missing `{macro}` tokens, route-macro repository entries that load as `null`, and empty route specifications now fail fast in `ArrowRouteParser`.
- These no longer degrade into skipped macros or later table-resolution errors.
- Startup validation now wraps those failures with route-contract context that includes whether the broken key was a route name or alias plus the failing specification text.
- Focused evidence already exists in `ArrowRouteParserTests`.

### Import-Time Reference And Drift Failures

- YAML-to-runtime import can fail when route, anchor, table, or facet references do not resolve against the runtime lookup tables.
- Merge-style drift on existing facet rows was a real Phase 5 failure class. The importer now clears stale `facet_table.alias` and `udf_call_arguments` values and removes obsolete `facet_clause` rows during reimport.
- This boundary is currently exercised through `--import-facet-config`, importer tests, and staging import plus smoke reruns.

### Startup-Time Runtime-Configuration Failures

- Bad imported route configuration should fail before the first live request.
- Startup now validates the route graph plus every configured route name and alias through the current route-configuration startup validation path.
- Missing deployment database runtime tables are also a startup-time failure class. The live `supersead` target hit that failure mode before 2026-05-30, but the current prep-plus-import flow has now prepared and imported `facet.anchor`, `facet.facet_anchor`, `facet.facet_template`, `facet.route`, `facet.route_step`, and `facet.config_revision` there.

### Request-Time Unsupported-Boundary Failures

- Unsupported facet-content requests remain explicit: `FacetContentService.Load` falls back only when `ComposedFacetContentService.CanHandle(...)` is false, and direct unsupported `ComposedFacetContentService.Load` calls throw an actionable error.
- Direct composed facet-content failures now report the first failing contract instead of only a generic unsupported message, including request-time cases such as missing predicate picks, non-routable target join columns, and facet clauses that the composed predicate path cannot normalize.
- Unsupported result requests remain explicit at the handoff boundary: `IResultProjectionHandoffBuilder` returns composed `composed_filter` and optional `target_route` SQL only for supported requests, and otherwise returns the legacy query-setup handoff.
- Result-handoff fallback now logs the first failing contract before returning the legacy projection path, so request-time result misconfiguration no longer disappears behind silent fallback.
- This means current unsupported behavior is explicit, but the inventory is still incomplete for unmeasured families beyond the recorded Phase 5 draft.

## Current Repeatable Validation Boundary

Phase 5 currently has one practical validation ladder.

1. Validate YAML structure and field shapes with `sead.query.composer/Templates/facet-route-config.schema.json` and `--validate-facet-config`.
   - This boundary now includes semantic checks against the current facet schema for anchor tables, generated route endpoints, facet source tables, aggregate facet references, and facet-anchor route bindings.
2. Import the candidate revision with `--import-facet-config`, which records one active `facet.config_revision` row and materializes the runtime copy in the existing `facet` schema.
3. Fail bad imported configuration at startup through route-graph and route-name validation before the first request is served.
4. Run focused configuration and startup tests:
   - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~FacetRouteConfigurationImporterTests"`
   - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~RouteGraphTests|FullyQualifiedName~RouteGraphFactoryTests"`
   - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ArrowRouteParserTests|FullyQualifiedName~RouteConfigurationStartupValidationServiceTests"`
   - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ComposedFacetContentServiceTests"`
   - `dotnet test sead.query.test/sead.query.test.csproj --filter "FullyQualifiedName~ResultProjectionHandoffBuilderTests|FullyQualifiedName~ComposedResultProjectionHandoffBuilderDiagnosticsTests"`
5. Run candidate cutover gates:
   - `make default-cutover-smoke-check`
   - `make default-cutover-http-smoke-check SEAD_QUERY_API_BASE_URL=https://host/query`

This is the current repeatable boundary for Phase 5. It is sufficient to catch malformed authoring, broken imports, bad startup configuration, and regressions in the validated representative runtime slices.

## Current Diagnostics Gaps

The remaining gaps are narrower than earlier in Phase 5.

- There is still no single focused inventory for request-time misconfiguration outside the currently covered representative families.
- Unsupported-boundary behavior is explicit, but the repository still needs one maintained list of unmeasured families that are expected to fall back, fail fast, or require explicit exception routes. The current Phase 6 cutover-boundary handoff now lives in `PARITY_INVENTORY.md`, but the unmeasured-family list still needs to stay current as that boundary widens.
- Deployment validation still depends on each target database carrying the prepared Phase 5 runtime tables plus an imported active revision; the runtime cannot make that state self-healing.
- Validation is repeatable, but it is still layered across schema validation, semantic validation, import, startup checks, tests, and smoke gates rather than one single command for all configuration failure classes.

## Current Route-Exception Inventory

The checked-in Phase 5 authoring draft is narrower than the eventual cutover surface.

Current generated families in `sead.query.composer/Templates/route_v1.yaml`:

- `feature_type`
- `dataset_methods`
- `data_types`
- `dataset_provider`
- `record_types`
- `country`
- `sites_polygon`
- `analysis_entity_ages`

Current explicit exception state:

- the checked-in draft currently contains no explicit `routes` exception section and no recorded YAML-side SQL override inventory
- `facet.route_step` persistence remains an implementation exception because the checked-in schema still applies a global unique constraint on `route_step.table_id`; imported `facet.route.specification` remains authoritative instead
- unsupported result requests remain explicit legacy-fallback exceptions outside the validated composed-result surface, and the fallback boundary now logs the first failing contract for those requests
- out-of-draft species result families currently covered as explicit legacy-fallback exceptions are `palaeoentomology`, `archaeobotany`, `pollen`, and `dendrochronology`
- the currently known out-of-draft follow-up set is now explicit rather than unclassified: prefixed `species`, plus archaeobotany `modification_types` and archaeobotany and pollen `abundance_elements`, remain deferred until focused cutover probes confirm whether they belong in generated families or require explicit exceptions

## Phase 5 Implication

Phase 5 can now treat three things as documented:

- the current diagnostics inventory
- the current repeatable validation boundary
- the current explicit exception surface for the checked-in draft

Phase 5 should not yet treat diagnostics hardening as complete. The remaining work is to widen request-time validation and keep the exception inventory current as more families move into YAML.

## Recommended Follow-Up

- keep this inventory aligned with any new family added to `route_v1.yaml`
- add focused tests when a new family introduces a request-time exception or explicit fallback boundary
- keep the maintained exception list aligned with the currently covered out-of-draft legacy-fallback families as that inventory widens
- move any durable runtime-boundary statements that should outlive Phase 5 into `docs/DESIGN.md` and `docs/OPERATIONS.md`