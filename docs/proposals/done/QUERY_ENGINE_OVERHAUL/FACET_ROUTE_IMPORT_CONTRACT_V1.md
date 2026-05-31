# Facet Route Import Contract V1

## Status

- Proposed implementation contract
- Scope: import YAML-authored facet, anchor, and route configuration into the existing `facet` schema
- Goal: define the first machine-checkable contract for Phase 5 imports

## Summary

This contract defines the field-level v1 import mapping from facet-route authoring YAML into the current runtime schema.

It assumes the durable authoring, runtime, and workflow model already described in `docs/REQUIREMENTS.md`, `docs/DESIGN.md`, `docs/DEVELOPMENT.md`, and `docs/OPERATIONS.md`.
This document is only the narrow importer contract for the v1 shape.

## Scope

This contract covers:

- YAML key and field rules
- mapping into existing `facet` schema tables
- validation rules required before import
- the first end-to-end slice to implement

This contract does not restate the long-lived source-of-truth decision, contributor workflow, or operational import procedure.

## Existing Runtime Targets

The importer writes to these existing tables.

- `facet.anchor`
- `facet.route`
- `facet.route_step`
- `facet.facet`
- `facet.facet_table`
- `facet.facet_anchor`
- `facet.facet_clause` when clauses are defined
- `facet.facet_template` only when explicit SQL overrides are defined

The importer reads these existing lookup tables.

- `facet.table`
- `facet.facet_group`
- `facet.facet_type`

These tables are listed here because the importer maps into them directly.
Their broader runtime role belongs in durable architecture docs rather than in this contract.

## YAML To Schema Mapping

### Anchors

YAML:

- `anchors[].key`
- `anchors[].table`
- `anchors[].key_column`
- `anchors[].description`

Import rules:

- resolve `anchors[].table` against `facet.table.table_or_udf_name`
- insert or update `facet.anchor.name = anchors[].key`
- write the resolved `table_id` to `facet.anchor.table_id`
- write `description` to `facet.anchor.description`

Notes:

- `key_column` is validated against the resolved table metadata during import, but it is not stored in `facet.anchor`
- anchor keys are stable natural keys for import matching

### Route Macros And Route Families

YAML:

- `route_macros[].key`
- `route_macros[].path[]`
- `route_families[].key`
- `route_families[].source_table`
- `route_families[].source_key_column`
- `route_families[].generated_route_key_pattern`
- `route_families[].anchors.<anchor>.path[]`

Import rules:

- expand macros before writing routes
- resolve each path item table name against `facet.table`
- generate one concrete route per family-anchor pair
- write route key to `facet.route.route_name`
- write canonical expanded arrow specification to `facet.route.specification`
- resolve source and target table ids into `facet.route.source_table_id` and `facet.route.target_table_id`

Current compatibility note:

- the existing `facet.route_step` schema has a global unique constraint on `table_id`
- because of that, the v1 importer treats `facet.route.specification` as the runtime source of truth and does not persist route-step rows yet
- route-step persistence should be enabled only after the existing schema constraint is corrected

Notes:

- route aliases are optional in v1
- explicit route exceptions can be authored in a separate `routes` section later without changing this contract

### Facets

YAML:

- `facets[].key`
- `facets[].display_title`
- `facets[].description`
- `facets[].group_key`
- `facets[].type`
- `facets[].source_table`
- `facets[].category.*`
- `facets[].sort_expr`
- `facets[].flags.*`
- `facets[].aggregate.*`

Import rules:

- write `facets[].key` to `facet.facet.facet_code`
- write display and description fields directly
- resolve `group_key` using `facet.facet_group.facet_group_key`
- resolve `type` using `facet.facet_type.facet_type_name`
- write category fields directly to the existing facet columns
- write `sort_expr` directly
- write `flags.is_applicable` and `flags.is_default`
- write `aggregate.type` to `aggregate_type`
- write `aggregate.title` to `aggregate_title`
- if `aggregate.facet_key` is omitted, write `aggregate_facet_id = 0`
- if `aggregate.facet_key` is present, resolve it by imported facet code

### Facet Source Table

YAML:

- `facets[].source_table`

Import rules:

- resolve `source_table` against `facet.table`
- insert one row in `facet.facet_table`
- store it at `sequence_id = 1`

This preserves current runtime behavior where `Facet.TargetTable` resolves from the first facet table row.

### Facet Anchors

YAML:

- `facets[].anchors[].anchor`
- `facets[].anchors[].route`

Import rules:

- resolve the imported facet by `facet_code`
- resolve the imported anchor by `anchor.name`
- resolve the imported route by `route.route_name`
- insert one `facet.facet_anchor` row for each supported anchor

### Clauses And SQL Overrides

YAML:

- `facets[].clauses[]`
- `facets[].anchors[].sql_override`

Import rules:

- each clause writes one `facet.facet_clause` row
- SQL overrides are exceptions only and write to `facet.facet_template`
- SQL overrides are keyed by facet plus anchor key

## Required Validation Before Import

- schema validation passes against the JSON schema
- every top-level key is known
- every key field is unique within its section
- every table name resolves in `facet.table`
- every `group_key` resolves in `facet.facet_group`
- every `type` resolves in `facet.facet_type`
- every referenced anchor exists
- every referenced route exists after route-family expansion
- every expanded route path resolves to a valid table sequence
- every explicit SQL override is limited to declared facet-anchor pairs

## Import Semantics

Use deterministic upsert semantics by natural key.

- anchor natural key: `name`
- route natural key: `route_name`
- facet natural key: `facet_code`
- facet-anchor natural key: `facet_id + anchor_id`

The importer should fail the entire revision on any validation error. No partial import should become active.

## First End-To-End Slice

Implement this first slice:

- facet family: `feature_type`
- anchors: `sample`, `dataset`

Why this slice:

- it is a discrete facet
- it uses a shared path prefix that benefits from a macro
- it expands to two concrete routes
- it does not require an explicit SQL override
- it stays within the current runtime anchor vocabulary already present in `facet.anchor`
- it is small enough to validate against current runtime behavior without opening a broad migration front

## Acceptance Criteria

- one YAML file validates against the v1 schema
- the importer expands the `feature_type` family into concrete route rows
- imported rows exist in the existing `facet` schema for anchors, routes, route steps, facet, facet table, and facet-anchor links
- startup validation accepts the imported route definitions
- the imported slice can be compared against current runtime behavior for the same facet and anchors

## Final Recommendation

Use this contract as the Phase 5 v1 import boundary. Keep it narrow. Prove one small slice first. Widen only after the importer, runtime mapping, and validation path are stable.

If the durable configuration model changes, update the long-lived docs first and then adjust this contract to match the current v1 importer boundary.