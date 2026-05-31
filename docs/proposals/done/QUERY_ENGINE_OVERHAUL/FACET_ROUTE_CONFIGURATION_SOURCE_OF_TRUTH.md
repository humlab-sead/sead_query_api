# Facet and Route Configuration Source Of Truth

## Status

- Recommended and partially implemented
- Scope: facet, anchor, and route configuration for the query-engine overhaul
- Goal: choose a maintainable source of truth that avoids route explosion and still preserves a database-backed runtime copy

## Summary

The recommendation is to use YAML as the authoring format, keep it in git, validate it in CI, and import a normalized, versioned copy into the existing `facet` schema for runtime use.

YAML is a good option here, but not if it stores full `template_sql` per facet and per anchor as the primary model. That shape will recreate the current maintenance problem in a different place.

The right split is:

- YAML as the reviewed, diffable, human-edited source of truth
- database import into the existing `facet` schema as the runtime copy and audit trail
- route macros and route families to avoid hand-authoring every source-to-anchor path
- concrete expanded routes in the imported database copy so runtime stays simple

The durable governance and runtime-boundary parts of that decision now live in `docs/REQUIREMENTS.md`, `docs/DESIGN.md`, `docs/DEVELOPMENT.md`, and `docs/OPERATIONS.md`.
This proposal remains useful as the rationale record, migration-state note, and exception-inventory companion for the source-of-truth decision.

## Governance Decision

Phase 5 should treat the checked-in YAML authoring files as the maintained change surface and the imported database revision as the maintained runtime surface.

That durable split is now recorded in the stable docs.
Use this proposal for the reasoning behind the decision, the route-explosion guidance, and the remaining migration-state notes that should not live permanently in architecture or requirements docs.

## Problem

The current configuration lives in the `facet` schema. That makes runtime lookup easy, but it has three problems.

First, review and maintenance are harder than they need to be. Changes to facets, routes, and anchors are data changes, but they are also design changes. They benefit from git review, small diffs, and explicit validation.

Second, the current experiment in [sead.query.composer/Templates/templates.yml](../../../../sead.query.composer/Templates/templates.yml) shows the main risk of a naive YAML approach. If each facet stores full SQL per anchor, then adding a new anchor or a new routed family creates a multiplication problem.

Third, runtime still needs a database-backed copy. Even if YAML becomes the source of truth, the active configuration should still be imported into the database so the system has one resolved runtime version, one revision id, and one auditable active state.

## Scope

This proposal covers:

- how facets, anchors, routes, and reusable route parts should be authored
- how configuration should be versioned and imported into the database
- how to structure YAML to reduce route explosion
- what the initial YAML schema should look like

## Non-Goals

This proposal does not:

- replace the current runtime in one step
- redesign facet semantics or result semantics
- define every importer detail or migration script
- replace the route compiler with a new algorithm

## Current Behavior

Relevant current state:

- anchors, facets, and routes already exist as database entities and repositories
- the route compiler already supports named route definitions, macro expansion, route resolution, and SQL generation
- the YAML experiment in [sead.query.composer/Templates/templates.yml](../../../../sead.query.composer/Templates/templates.yml) mixes useful ideas with a duplication-heavy shape
- the active Phase 5 authoring draft now lives in `sead.query.composer/Templates/route_v1.yaml` and is validated before import
- the current host exposes `--validate-facet-config` and `--import-facet-config` so validation and import happen at one repeatable boundary instead of through ad hoc database editing
- `--validate-facet-config` now resolves anchor tables, generated route endpoints, and facet-anchor route bindings against the current facet schema before import, so route and anchor drift fails before any runtime rows are updated
- startup now validates the imported route graph and configured route names before request handling, so bad imported configuration fails before the first live request

The useful parts of that experiment are:

- explicit anchors
- explicit facet-to-anchor support
- explicit routed paths

The part to avoid as the long-term source of truth is:

- storing full handwritten SQL for every facet-anchor combination as the default configuration model

## Proposed Design

### Recommendation

Use a hybrid model.

1. Author configuration in YAML in the repository.
2. Validate it statically before import.
3. Expand macros and route families during import.
4. Import the expanded, normalized copy into the database.
5. Run the application against the imported database copy, not against raw YAML files.

This keeps authoring safe and reviewable, while keeping runtime deterministic.

### Storage Model

Use four authoring layers.

1. `anchors`
Defines the counting and return units such as `site`, `sample_group`, `physical_sample`, `dataset`, and `analysis`.

2. `route_macros`
Defines reusable path fragments such as `feature_to_sample_context` or `sample_to_dataset_context`.

3. `route_families`
Defines one source table and the anchor-specific route expansions that hang off that source. This is the main route-explosion control.

4. `facets`
Defines facet metadata, facet type, category expressions, optional clauses, and which anchors are supported by referencing generated routes.

The importer should expand `route_families` plus `route_macros` into concrete routes and store those concrete routes in the database.

### Why YAML Is A Good Fit

YAML is a good fit for authoring because:

- it is easy to diff and review in git
- it is easier to validate in CI than manual database edits
- it supports split files and domain grouping
- it makes generated versus handwritten configuration explicit

YAML is not a good fit as the only runtime source. Runtime should not depend on ad hoc file loading, local file drift, or partial parse state.

### Database Copy And Versioning

The database should keep an imported copy of the active configuration.

Recommended model:

- one `facet.config_revision` table with `revision_id`, `source_commit`, `content_hash`, `imported_at`, `imported_by`, and `is_active`
- normalized imported rows in the existing `facet` schema tables already used at runtime
- one stored raw YAML blob or manifest checksum per revision for audit and rollback

Current implementation status:

- `facet.config_revision` is now implemented in the current importer path
- each import deactivates the previous active revision and marks the imported YAML revision active
- the importer stores `config_revision`, content hash, import time, and provenance fields from environment-backed runtime metadata
- the current operational surfaces wire provenance through Docker Compose and the scripted local import command

For Phase 5, import directly into the existing `facet` schema and add revision tracking there. This is less clean than a separate imported schema, but it is the lower-risk path for the current runtime and the right short-term choice.

The importer contract should map YAML to these existing runtime tables:

- `facet.anchor`
- `facet.route`
- `facet.facet`
- `facet.facet_table`
- `facet.facet_anchor`
- `facet.facet_clause` when YAML defines facet clauses
- `facet.facet_template` only for explicit SQL overrides

Current compatibility note:

- keep `facet.route.specification` as the authoritative imported route representation for now
- defer `facet.route_step` persistence until the existing schema is corrected; the checked-in schema currently applies a global unique constraint on `route_step.table_id`, which is incompatible with storing steps for multiple routes

### Route Explosion Control

This is the core rule: do not author full routes or SQL for every facet-anchor pair unless the pair is genuinely exceptional.

Use these controls instead.

1. Route macros
Store shared path fragments once.

2. Route families
Define a source once and expand it to anchors by combining macros with short anchor-specific tails.

3. Concrete generated routes at import time
Generate the repetitive runtime route rows during import, not by hand.

4. Explicit exceptions
If one facet-anchor pair really needs custom SQL or a custom route, mark it as an override instead of making overrides the default model.

This means new anchors should usually require:

- one anchor definition
- a small number of new anchor tails or shared macros
- importer expansion into many concrete routes

not a full rewrite of every related facet.

### Initial YAML Schema

This is the recommended initial authoring shape.

```yaml
schema_version: 1
config_revision: 2026-05-phase5-draft
runtime_import:
  target_schema: facet
  mode: merge-into-existing

anchors:
  - key: site
    table: tbl_sites
    key_column: site_id
    description: Site-level anchor

  - key: sample_group
    table: tbl_sample_groups
    key_column: sample_group_id
    description: Sample-group anchor

route_macros:
  - key: feature_type_context
    path:
      - table: tbl_features
      - table: tbl_physical_samples
      - table: tbl_sample_groups

  - key: sample_to_dataset_context
    path:
      - table: tbl_physical_samples
      - table: tbl_analysis_entities
      - table: tbl_datasets

route_families:
  - key: feature_type
    source_table: tbl_feature_types
    source_key_column: feature_type_id
    anchors:
      site:
        path:
          - macro: feature_type_context
          - table: tbl_sites
      sample_group:
        path:
          - macro: feature_type_context
      physical_sample:
        path:
          - table: tbl_features
          - table: tbl_physical_samples

routes:
  - key: bibliography_modern__site
    source_table: tbl_biblio_modern
    anchor: site
    path:
      - table: tbl_biblio
      - table: tbl_sample_groups
      - table: tbl_sites
    mode: explicit
    description: Explicit exception route kept outside family generation

facets:
  - key: feature_type
    display_title: Feature type
    type: discrete
    source_table: tbl_feature_types
    category:
      id_expr: tbl_feature_types.feature_type_id
      name_expr: tbl_feature_types.feature_type_name
      data_type: integer
      operator: in
    aggregate:
      type: count
    clauses: []
    anchors:
      - anchor: site
        route: feature_type__site
      - anchor: sample_group
        route: feature_type__sample_group
      - anchor: physical_sample
        route: feature_type__physical_sample
```

### Schema Rules

The YAML schema should enforce these rules.

#### Top-level

- `schema_version` is required
- `config_revision` is required
- top-level keys are limited to known sections

#### Anchors

- each anchor key is unique
- each anchor declares exactly one table and one key column
- anchor keys are stable identifiers and should not be renamed casually

#### Route Macros

- macro keys are unique
- macros expand only to path items
- macros do not carry facet semantics
- macros should not contain raw SQL

#### Route Families

- each family declares one source table
- each anchor entry expands to one concrete route
- generated route keys follow a deterministic naming rule, for example `<family>__<anchor>`
- families can reference macros and table steps, but not arbitrary SQL

#### Routes

- explicit `routes` are reserved for exceptions and hand-maintained special cases
- each route targets exactly one anchor
- each route path expands to a valid table sequence
- unsupported source-to-anchor paths should be absent from generated families and called out explicitly as exceptions or follow-up work

#### Facets

- each facet key is unique
- each facet declares one facet type
- each facet declares category expressions and category type
- each supported anchor references a concrete route key
- facet-specific clauses are explicit and local to the facet
- raw per-anchor `template_sql` is allowed only through an explicit override field and should be exceptional
- facets must not rely on implicit legacy runtime fallbacks to define support; supported anchors and unsupported boundaries should be declared in YAML or rejected during validation/import

### Importer Contract

The importer should resolve YAML keys against existing runtime lookup tables rather than hard-coding ids in YAML.

- `anchors[].table` resolves to `facet.table.table_or_udf_name`, then writes `facet.anchor`
- `route_families` expand into concrete route rows written to `facet.route` and `facet.route_step`
- `facets[].group_key` resolves to `facet.facet_group.facet_group_key`
- `facets[].type` resolves to `facet.facet_type.facet_type_name`
- `facets[].source_table` writes one `facet.facet_table` row at `sequence_id = 1`
- `facets[].anchors[]` writes `facet.facet_anchor` rows by linking the imported facet, anchor, and route ids
- `facets[].sql_override` or anchor-level SQL overrides, if they exist, write `facet.facet_template`
- `facets[].clauses[]` writes `facet.facet_clause`

If `aggregate.facet_key` is omitted, the importer should store `aggregate_facet_id = 0`. Current runtime behavior already falls back to the target facet when that field is zero or unresolved.

See [docs/proposals/done/QUERY_ENGINE_OVERHAUL/FACET_ROUTE_IMPORT_CONTRACT_V1.md](FACET_ROUTE_IMPORT_CONTRACT_V1.md) for the concrete v1 mapping.

### First Import Slice

The first end-to-end import slice should be the `feature_type` facet family with two current-runtime anchors:

- `sample`
- `dataset`

This slice is small enough to validate quickly, but it still exercises the new model properly:

- one discrete facet
- one shared route macro
- one generated route family
- two concrete imported routes
- two `facet_anchor` links
- no explicit SQL override requirement
- current runtime anchor vocabulary is preserved instead of inventing new anchor keys during the first import

Use `site` and `dataset` later. They are useful follow-on slices, but they are not the smallest proof that the authoring model, importer, and current runtime schema can work together.

### File Layout

Prefer split YAML files, not one large monolith.

Recommended layout:

```text
config/facets/
  anchors.yml
  route_macros.yml
  route_families.yml
  routes.exceptions.yml
  facets.discrete.yml
  facets.range.yml
  facets.intersect.yml
  facets.geopolygon.yml
```

That layout keeps ownership narrow and avoids merge conflicts.

## Current Exception Inventory

The goal is to keep exceptions explicit and reviewable.

Current exception surface:

- the checked-in `route_v1.yaml` draft currently uses generated route families only; it does not yet carry a YAML-side `routes` exception section or a recorded SQL-override inventory
- explicit `routes` remain the place for source-to-anchor paths that do not fit a reusable family yet
- `facet.facet_template` remains the place for true SQL override cases and should stay rare
- `facet.route_step` persistence is still deferred because the current seeded schema applies a global unique constraint on `route_step.table_id`; the runtime currently relies on imported `facet.route.specification` instead

Current remaining governance gap:

- the authoring model and validation boundary are now explicit, but the repository still needs to keep the widening inventory current as YAML coverage expands toward default cutover
- the currently known out-of-draft families are now classified as deferred follow-up rather than left unclassified: prefixed `species`, plus archaeobotany `modification_types` and archaeobotany and pollen `abundance_elements`, remain outside the current draft until focused cutover probes prove that they belong in generated families rather than in a later explicit-route or SQL-override inventory
- the checked-in draft still has no active YAML-side `routes` exception section and no active SQL-override inventory; those remain reserved for genuine exceptions rather than for routine widening

## Alternatives Considered

### Keep The Database As The Only Source Of Truth

Rejected as the primary recommendation.

It keeps runtime simple, but it keeps review, validation, and change history weaker than they should be.

### Use YAML Only And Remove The Database Copy

Rejected.

It makes runtime drift, deployment drift, and rollback harder than necessary.

### Store Full SQL Templates Per Facet And Per Anchor In YAML

Rejected as the default design.

That would move the current maintenance problem from the database into files and would make route explosion worse when anchors grow.

## Risks And Tradeoffs

- YAML authoring plus database import is more moving parts than database-only editing.
- Import tooling becomes part of the critical path and must be validated.
- Route-family generation needs discipline. If it becomes too clever, it will become hard to debug.
- Some facets will still need explicit exceptions. The goal is to isolate them, not pretend they do not exist.

## Testing And Validation

This proposal should be validated by:

- authoring-time schema validation for YAML structure
- authoring-time semantic validation for cross-references, duplicate keys, missing anchors, and broken route expansions
- import validation that produces one revisioned database snapshot and one active revision row
- startup validation that fails on bad imported configuration before first request
- focused route parser, route graph, and startup diagnostics tests at the current runtime boundary
- request-shape and unsupported-boundary tests when a routed family adds an explicit exception or new fallback boundary

## Acceptance Criteria

- one authoring format exists for anchors, routes, and facets
- imported configuration in the existing `facet` schema has a revision id and an active version
- route families and macros are sufficient to avoid hand-authoring most source-to-anchor paths
- explicit exceptions are isolated instead of becoming the common case
- the initial YAML schema is documented and validated
- one small facet family with one or two anchors is imported end to end against the current runtime schema

## Recommended Delivery Order

1. Define the YAML schema and validator.
2. Define the imported revision model in the existing `facet` schema.
3. Build importer expansion from macros and route families to concrete routes.
4. Import the `feature_type` slice with `sample` and `dataset` anchors and compare it to the current `facet` schema output.
5. Widen incrementally by facet family.

## Open Questions

- Which existing facet families are likely to need explicit `sql_override` support from the start?
- Should generated concrete routes be persisted only in the database, or also written back out as a generated artifact for review?

## Final Recommendation

Use YAML as the authoring source of truth, but not as the runtime source.

Store anchors, route macros, route families, explicit exception routes, and facets in validated YAML. Expand them during import. Persist the expanded, versioned result in the existing `facet` schema. Run the application against one active imported revision.

Do not adopt full per-facet-per-anchor handwritten SQL as the main YAML model. That is the fastest way to recreate route explosion.

For Phase 5, keep the durable governance model in the stable docs and keep this proposal focused on rationale, tradeoffs, current exceptions, and remaining widening notes. Use the import contract for the concrete field-level mapping.