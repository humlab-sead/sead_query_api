# Query Engine Legacy Deprecation

## Status

- Proposed change request
- Scope: runtime deprecation and removal of the legacy query-builder, plugin-driven facet execution path, retained legacy SQL generation surfaces, and any still-authoritative legacy SQL DML or operational SQL assets
- Goal: make the composer runtime the only supported execution model for facet-content and result generation

## Summary

This proposal recommends full deprecation of the live legacy query runtime.
The current `dev` branch still runs a hybrid model.
The composer path is preferred for the validated surface, but legacy query-building and plugin-based execution remain active for unsupported requests and for some supporting or compatibility services.

Phase 2 has now removed two supported-path dependencies from that hybrid boundary.
Supported facet and result controller loads no longer route through `BogusPickService`, and supported composed result projection setup no longer routes through `QuerySetupBuilder`.
Phase 2 has also moved the facet/result probe commands onto the supported invalid-pick contract and moved the shared category-info path onto `SupportedRequestQuerySetupFactory`.
Phase 2 has now also moved `CategoryCountService`, `LegacyResultProjectionHandoffBuilder`, and `BogusPickService` off direct `QuerySetupBuilder` usage.
Legacy fallback and retained compatibility infrastructure still keep the deprecation work incomplete.

That hybrid model increases maintenance cost, keeps migration boundaries alive in production code, and makes it harder to reason about what is actually authoritative.
The right next step is to move from composer-first with legacy fallback to composer-only, then remove the retained legacy code and any still-used legacy SQL assets.
Execution sequencing for that work is defined in [QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md](QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_PLAN.md).

## Problem

The current runtime still contains two execution models.
The composer runtime handles the promoted slice, but the legacy runtime still matters in three ways.

First, unsupported facet-content and result requests still fall back to legacy execution.
Second, some supporting and compatibility services still rely on legacy query-building even when the top-level request remains on the composed path.
Third, the repository still contains retained legacy code and possibly retained SQL assets whose operational status is not yet fully closed.

The current Phase 2 execution tracker is maintained in [TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_2.md](TASK_PLAN_QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_2.md).

This causes several problems:

- the supported runtime boundary is harder to explain and enforce
- new work must still account for fallback behavior and old extension points
- validation must prove both parity and routing decisions instead of one authoritative runtime
- dead or semi-live SQL assets can remain ambiguous and therefore risky

## Scope

This proposal covers:

- deprecating the legacy facet-content runtime
- deprecating the legacy result-projection handoff
- removing fallback branches from the live request path once parity is proven
- removing retained legacy query-builder and plugin surfaces that are no longer authoritative
- identifying and deprecating any legacy SQL DML or related operational SQL assets that remain executable, required, or authoritative outside archival material
- updating documentation, diagnostics, and validation so the composer runtime is the only supported model

## Non-Goals

This proposal does not cover:

- a redesign of the composer architecture itself
- unrelated API or DTO changes
- cleanup of archived or historical material that is already clearly non-authoritative
- release scheduling, staffing, or deployment ownership

## Current Behavior

The current branch still has explicit live legacy boundaries.

- facet-content requests fall back through `FacetContentService` when `ComposedFacetContentService.CanHandle(...)` rejects the request
- result requests fall back through `ComposedResultProjectionHandoffBuilder` when `TryCreateRequest(...)` cannot build a composed request
- `QuerySetupBuilder` no longer underpins supported controller preprocessing, probe normalization, composed result projection setup, legacy result setup assembly, category-count setup assembly, bogus-pick normalization, or the shared category-info path, but it still underpins retained base-class and compatibility infrastructure
- the facet-type plugin registrations for discrete, range, intersect, and geo-polygon remain active in DI
- `BogusPickService` no longer sits on the supported controller or probe paths, but retained compatibility flows still keep it reachable
- imported SQL override data written to `facet.facet_template` now appears to be importer/schema compatibility only; Phase 2 has not found a non-archived request-path consumer

The result is not a dormant compatibility layer.
The legacy runtime is still part of the live execution model on `dev`.

The starting Phase 1 runtime inventory should explicitly review these code surfaces:

- `sead.query.core/Services/FacetContent/FacetContentService.cs`
- `sead.query.core/Services/CategoryCount/CategoryCountService.cs`
- `sead.query.core/QueryBuilder/QuerySetupBuilder.cs`
- `sead.query.core/Services/Result/Services/LegacyResultProjectionHandoffBuilder.cs`
- `sead.query.composer/QueryComposer/Services/ComposedResultProjectionHandoffBuilder.cs`
- `sead.query.composer/QueryComposer/Services/ComposedFacetContentService.cs`
- `sead.query.core/Plugins/DiscreteFacet/BogusPickService.cs`
- `sead.query.core/Plugins/Common/CategoryInfoService.cs`
- `sead.query.api/Dependency.cs`
- `sead.query.core/Plugins/DiscreteFacet/Plugin.cs`
- `sead.query.core/Plugins/RangeFacet/Plugin.cs`
- `sead.query.core/Plugins/IntersectFacet/Plugin.cs`
- `sead.query.core/Plugins/GeoPolygonFacet/Plugin.cs`

## Proposed Design

### Deprecation target

The target end state is a single runtime model:

- composer is authoritative for facet-content
- composer is authoritative for result generation
- no live request path falls back to legacy execution
- no supported service depends on legacy query-building primitives
- no still-authoritative legacy SQL DML or runtime SQL asset remains outside the composer-era model

### Deprecation strategy

Deprecation should proceed in two layers.

The first layer is behavioral.
All currently supported request families must either run on the composed path with proven parity or be explicitly removed from the supported surface.

The second layer is structural.
Once no supported request depends on legacy execution, the fallback seams, old extension points, and any still-live legacy SQL assets should be removed.

### Legacy SQL scope

The repository contains legacy SQL material in multiple places, including clearly deprecated areas.
This proposal only treats SQL as in scope when it is one of the following:

- required by the promoted runtime
- required by supported operational workflows
- executed by tests or live diagnostics as part of current behavior
- still documented as authoritative setup or migration behavior

Legacy SQL that is already archived, deprecated, or clearly non-authoritative may be left as historical material or removed separately.
The first delivery phase should confirm which SQL assets are still live before removal work begins.

The starting Phase 1 SQL inventory should explicitly review these assets and asset families:

- `scripts/prepare-phase5-facet-runtime-schema.sql`, because `docs/OPERATIONS.md` currently treats it as an active deployment schema-prep asset
- imported `facets[].sql_override` and anchor-level SQL overrides written into `facet.facet_template`, because they remain part of the runtime SQL surface when present
- `deprecated/000_create_staging_db.sql`
- `deprecated/cr_01_change_set_results.sql`
- `deprecated/cr_02_change_set_bugfix.sql`
- `deprecated/measured_value.sql`
- `deprecated/UDF_bugg_29.sql`
- `deprecated/value_diffs.sql`
- `deprecated/bugg_tests.sql`

The expectation is that many `deprecated/` SQL files will classify as historical or dead.
They should still be named in the initial inventory so that removal or archival decisions are explicit rather than assumed.

## Alternatives Considered

### Keep the hybrid model indefinitely

This was rejected because it preserves dual-runtime complexity and keeps fallback logic in the main execution path.
It also keeps future feature work tied to both the composer model and the old plugin/query-builder model.

### Remove legacy code immediately without a phased cutover

This was rejected because the current branch still contains explicit fallback behavior and hybrid dependencies.
A direct removal would create a high regression risk and would not provide a clean way to resolve unsupported request families.

## Risks And Tradeoffs

- parity work may expose request families that still depend on legacy behavior in subtle ways
- some hybrid helper services may need temporary adapter logic before full removal is safe
- SQL asset discovery may reveal operational dependencies that are currently undocumented
- deprecating legacy extension points will raise the implementation bar for any still-incomplete facet or result surfaces

The main tradeoff is clear.
This deprecation adds short-term migration work in exchange for a simpler and more defensible long-term runtime.

## Testing And Validation

Validation should prove both behavior and removal boundaries.

- add or keep focused tests for each request family moved from fallback to composer-only execution
- keep parity-oriented tests where legacy and composed outputs can still be compared during the migration window
- add diagnostics that show whether a request uses any removed fallback surface before the cutover phase lands
- verify that no supported runtime path still calls the legacy handoff, legacy category-count engine, or legacy query-builder after final removal
- audit tests, scripts, and docs for any still-authoritative SQL DML or runtime SQL dependencies before deleting those assets

## Acceptance Criteria

- all supported facet-content requests execute on the composed path only
- all supported result requests execute on the composed path only
- no live request path contains a legacy fallback branch
- no supported service depends on `QuerySetupBuilder` for request execution
- legacy facet plugin registrations are removed from the authoritative runtime path
- any still-live legacy SQL DML or operational SQL assets have been either removed, replaced, or explicitly archived as non-authoritative
- documentation describes a single authoritative runtime model

## Recommended Delivery Order

### Phase 1: Inventory the live legacy surface

**Goal**

Establish the exact set of live legacy runtime and SQL assets that are still authoritative.

**Focus**

- inventory the legacy fallback seams centered on `FacetContentService`, `CategoryCountService`, `LegacyResultProjectionHandoffBuilder`, and `ComposedResultProjectionHandoffBuilder`
- inventory hybrid helper services centered on `QuerySetupBuilder`, `BogusPickService`, and `CategoryInfoService`
- inventory the active DI and plugin surfaces in `sead.query.api/Dependency.cs` and the facet plugin registrations
- inventory SQL assets that are still required by runtime, tests, or supported operational workflows, starting with `scripts/prepare-phase5-facet-runtime-schema.sql`, imported `sql_override` content in `facet.facet_template`, and the named SQL files under `deprecated/`
- classify retained material as live, transitional, archived, or dead

**Acceptance Criteria**

- the repository has a reviewed inventory of live legacy code paths
- the repository has a reviewed inventory of still-authoritative legacy SQL assets, if any
- each inventoried legacy surface has a planned disposition: migrate, archive, or remove

The published Phase 1 inventory is maintained in [QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md](QUERY_ENGINE_LEGACY_DEPRECATION_PHASE_1_INVENTORY.md).

### Phase 2: Reach composer parity for remaining supported requests

**Goal**

Remove the need for legacy fallback on the supported runtime surface.

**Focus**

- close the remaining unsupported request families or explicitly narrow support
- replace the remaining hybrid request-time dependencies that still require legacy query-building
- keep parity validation in place while both implementations still exist

Phase 2 has already completed the first supported-path slice by introducing `SupportedRequestPickSanitizer` and `SupportedRequestQuerySetupFactory` for the supported controller, probe, composed result, shared category-info, category-count, legacy result handoff, and bogus-pick setup paths.

**Acceptance Criteria**

- every supported request family has a composed-path implementation or is explicitly out of scope
- focused validation proves composed-path behavior for the supported surface
- no supported request still requires legacy fallback as a functional dependency

### Phase 3: Remove live fallback branches

**Goal**

Make composer the only live execution path.

**Focus**

- remove facet-content fallback from the runtime path
- remove result handoff fallback from the runtime path
- fail unsupported requests explicitly rather than silently routing to legacy code

**Acceptance Criteria**

- `FacetContentService` no longer routes unsupported requests to the legacy category-count engine
- result loading no longer delegates unsupported requests to the legacy result handoff builder
- runtime diagnostics and tests confirm one authoritative execution model

### Phase 4: Remove retained legacy implementation surfaces

**Goal**

Delete legacy code that is no longer required after cutover.

**Focus**

- remove unused legacy query-builder services from the authoritative runtime path
- remove legacy plugin registrations and implementations that no longer serve supported behavior
- remove or archive transitional helpers whose only purpose was compatibility during cutover

**Acceptance Criteria**

- retained legacy runtime classes no longer participate in supported execution
- obsolete registrations are removed from DI
- the remaining codebase reflects the composer-only runtime model

### Phase 5: Remove or archive live legacy SQL assets

**Goal**

Eliminate SQL assets that remain authoritative only because of the legacy runtime.

**Focus**

- remove or archive legacy SQL DML and related SQL assets that are no longer required
- update operational and testing workflows to composer-era authoritative assets only
- keep historical SQL only where it is clearly archived and non-authoritative

**Acceptance Criteria**

- no supported runtime or operational workflow depends on legacy SQL assets
- remaining historical SQL is clearly marked as archived or deprecated
- docs no longer describe legacy SQL assets as current setup or runtime requirements

## Open Questions

- Which currently deferred request families should be migrated to composer parity versus removed from the supported surface?
- Are there any still-executed SQL DML assets outside clearly deprecated or archival folders that must be preserved for cutover or rollback?
- Should any compatibility diagnostics remain temporarily after cutover, or should they be removed immediately with the fallback code?

## Final Recommendation

Adopt this deprecation proposal.
Treat the current hybrid runtime as a temporary migration state, not a durable architecture.
Use Phase 1 to close the exact live legacy inventory, then remove fallback behavior, retained runtime code, and any still-authoritative legacy SQL assets in ordered stages.