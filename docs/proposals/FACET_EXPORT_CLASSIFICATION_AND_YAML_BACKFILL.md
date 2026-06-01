# Facet Export Classification And YAML Backfill

## Status

- Proposed feature / change request
- Scope: classify the staging `facet` export in `docs/proposals/facets.json` against the maintained YAML authoring surface in `sead.query.composer/Templates/route_v1.yaml`
- Goal: define which exported facets should move into YAML now, which need additional modeling first, and which should stay explicit exceptions or policy decisions

## Summary

`sead.query.composer/Templates/route_v1.yaml` is now the maintained authoring source of truth, but the migration baseline should now assume inline SQL facet templates as the default authoring model rather than the current purely relational YAML-v1 shape.

Under that baseline, the migration gap is real but materially smaller than the earlier relational-only classification suggested. The export contains 52 facet rows. The current YAML draft authors 8 of them directly. Of the remaining 44, 39 become plausible inline-SQL backfill candidates once facet-owned CTE SQL is first-class, 3 should stay as facet-authored result-shape entries, and only 2 remain in a helper-only bucket.

The recommendation is to treat `facets.json` as an inventory input, not as a direct serialization source. Backfill should proceed bucket by bucket, with explicit support, exception, and contract decisions recorded in the same change.

See `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING.md` for the new baseline design this classification now assumes.

## Problem

The repository now treats `route_v1.yaml` as the checked-in authoring source of truth for maintained facet, anchor, and route configuration, while the runtime still executes the imported database copy.

That leaves one migration question open: what should happen to the historical or staging facet rows exported in `docs/proposals/facets.json`?

A blind one-to-one conversion would create several problems.

- It would copy helper, result-shape, and non-applicable facets into the maintained YAML surface without first deciding whether they belong there.
- It would blur the supported composed surface with explicit exceptions and deferred legacy-only behavior.
- It would still need a clear facet-owned SQL contract and anchor-projection model before the migrated YAML could be used consistently by the compilers.
- It would hide real migration work behind a misleading “everything is in YAML now” claim.

## Scope

This change request covers:

- classification of the 52 exported staging facets
- a recommended migration policy for bringing facet rows into `route_v1.yaml`
- an updated bucketed list using inline SQL facet templates as the migration baseline

## Non-Goals

This change request does not:

- author the missing facets into YAML yet
- widen the supported composed surface by itself
- redefine facet semantics or runtime support guarantees
- replace the current parity inventory or exception inventory
- propose a bulk import from JSON to YAML

## Current Behavior

The current durable split is:

- authoring source: `sead.query.composer/Templates/route_v1.yaml`
- authoring contract: `sead.query.composer/Templates/facet-route-config.schema.json`
- runtime copy: imported active rows in the `facet` schema

The repository docs already treat YAML as the maintained authoring surface and the imported database revision as the maintained runtime surface.

At the same time, the broader migration policy for the query-engine overhaul remains support-surface based rather than “copy every legacy or staging artifact forward”. Supported requests should be modeled and validated deliberately, while unsupported or deferred behavior should remain explicit.

The staging export in `docs/proposals/facets.json` therefore acts as a catalog of existing runtime rows, not an instruction to preserve every row in the same form.

What changes with the new baseline is the representational fit. Many facets that looked like route-modeling or UDF-modeling gaps under the old relational YAML shape become directly expressible once a facet can own its inline SQL CTE and the compiler can inject only the type-specific placeholders it owns.

## Proposed Design

### Recommendation

Use the staging export as an inventory input for controlled YAML backfill.

Use `docs/proposals/INLINE_SQL_FACET_TEMPLATES_AS_DEFAULT_AUTHORING.md` as the authoring baseline for that backfill.

Apply this rule:

- every exported facet should end in one explicit state: authored in YAML, queued for YAML backfill, kept on an explicit exception list, or rejected as out of scope or legacy residue

Do not apply this rule:

- every exported facet row must be copied mechanically into YAML now

### Classification Method

This first-pass classification used the checked-in staging export and compared it with the current YAML draft using these signals:

- whether the facet code already exists in `route_v1.yaml`
- whether the facet is marked non-applicable in the export
- whether the exported facet is really a helper or result-shape artifact rather than a visible maintained facet
- whether the new inline-SQL baseline makes the old relational blockers no longer decisive

This is a migration triage under the new inline-SQL baseline, not a proof that every listed backfill candidate is ready for import without anchor, compiler, and parity validation.

### Classification List

Total exported facets: 52

Already authored in `route_v1.yaml` and should stay there unless intentionally removed: 8

- `feature_type`, `dataset_methods`, `data_types`, `dataset_provider`, `record_types`, `country`, `sites_polygon`, `analysis_entity_ages`

Inline-SQL backfill candidates under the new baseline: 39

- `tbl_denormalized_measured_values_33_0`, `tbl_denormalized_measured_values_33_82`, `tbl_denormalized_measured_values_32`, `tbl_denormalized_measured_values_37`, `geochronology`, `relative_age_name`, `sample_groups`, `sites`, `ecocode`, `family`, `genus`, `species`, `species_author`, `ecocode_system`, `abundance_classification`, `abundances_all`, `activeseason`, `tbl_biblio_modern`, `tbl_biblio_sample_groups`, `tbl_biblio_sites`, `region`, `rdb_systems`, `rdb_codes`, `modification_types`, `abundance_elements`, `sample_group_sampling_contexts`, `constructions`, `location_types`, `dendro_age_contained_by`, `construction_purpose`, `datasets`, `palaeoentomology`, `archaeobotany`, `pollen`, `geoarchaeology`, `dendrochronology`, `ceramic`, `isotope`, `adna`

Result-shape facets to keep as facets, using a constrained template key for inner CTE composition SQL: 3

- `result_facet`, `map_result`, `result_datasets`

Helper or non-applicable facets that still need an explicit keep or remove decision before backfill: 2

- `sites_helper`, `abundances_all_helper`

The old route-modeling, denormalized-source, clause, alias, and UDF buckets no longer stand as separate blockers once inline SQL is the default. Their remaining implementation dependency is now mostly anchor projection strategy and focused runtime validation rather than lack of authoring expressiveness.

### Migration Rules By Bucket

For the inline-SQL backfill candidates:

- add the facet definitions to `route_v1.yaml` using inline SQL as the default facet-authoring model
- add or confirm required base-anchor and projected-anchor bindings in the same change
- validate and import only after focused tests cover the affected support slice

For the retained result-shape facet bucket:

- keep the entries in the maintained facet catalog
- add constrained `template_key` support for their inner CTE composition SQL rather than moving them to a separate metadata surface
- validate them through focused result-oriented slices before treating them as a migration template for other facets

For the helper or non-applicable bucket:

- decide whether the facet is still part of the maintained runtime surface
- if yes, model it explicitly in YAML with a documented reason
- if no, record it as deprecated, helper-only, or excluded rather than silently dropping it

For the already-authored YAML facets:

- keep them working under the current draft
- migrate them toward inline SQL when the compiler and importer support lands, instead of doubling down on the old relational-only authoring shape

## Alternatives Considered

### Convert all 52 exported facets into YAML immediately

Rejected for now.

This would still conflate supported behavior with helper rows and unresolved policy decisions, even though the new inline-SQL baseline removes many of the old authoring blockers.

### Leave `facets.json` as the practical source of truth for the missing rows

Rejected.

That would undermine the Phase 5 governance decision that checked-in YAML is the maintained authoring surface.

## Risks And Tradeoffs

- The new first-pass classification is more optimistic because inline SQL removes many representational blockers, but that does not prove parity or support readiness.
- Anchor projection remains the main unresolved implementation boundary for many migrated facets.
- The retained result-shape facets need a constrained `template_key` contract so they do not become an ungoverned second templating mechanism.
- The helper bucket still needs explicit policy decisions; it should not be smuggled into YAML just because SQL expressiveness now allows it.

## Testing And Validation

Validate this work in three layers:

- keep the classification aligned with `docs/proposals/facets.json` and the current `route_v1.yaml` draft
- validate each backfill batch against the inline-SQL authoring contract once that proposal lands
- use `make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/route_v1.yaml` for each backfill batch
- run focused composer, facet-content, or result-path tests for the exact bucket being promoted before importing a new runtime revision

## Acceptance Criteria

- the staging export is classified into explicit migration buckets rather than treated as one bulk-copy backlog
- the classification names the facets already authored in YAML
- the classification uses inline SQL facet templates as the migration baseline
- the classification identifies which exported facets become backfill candidates under that baseline, which result-shape entries stay as facets, and which still remain helper decisions
- follow-up implementation can add backfill batches without reopening the source-of-truth decision each time

## Recommended Delivery Order

1. Approve the inline-SQL authoring proposal and anchor-projection baseline.
2. Implement retained result-shape facet support through a constrained `template_key` path.
3. Confirm the helper bucket for non-applicable helper-only facets.
4. Backfill the inline-SQL candidates in small batches with focused validation.
5. Add projected-anchor routes or explicit anchor-to-SQL mappings only where the base-template path is not enough.

## Open Questions

- Which migrated facets can rely on projected-anchor routes from one base anchor, and which need explicit per-anchor inline SQL?

## Final Recommendation

Create and maintain one explicit YAML-backfill classification for the staging facet export, and use that classification to drive controlled migration work.

Do not convert all exported facets into `route_v1.yaml` in one step. Treat the export as an inventory, keep YAML as the maintained authoring source of truth, use inline SQL facet templates as the default migration baseline, keep the result-shape entries as facets through a constrained `template_key` path, and move the rest of the catalog in controlled batches with explicit policy decisions for the remaining helper-only facets.