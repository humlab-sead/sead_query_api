# Inline SQL Facet Templates As Default Authoring

## Status

- Proposed feature / change request
- Scope: introduce inline SQL templates in YAML as the default facet authoring model, define the SQL contract expected by facet-type compilers, and clarify how anchor projection should work when a facet supports multiple anchors
- Goal: make facet authoring expressive enough to migrate the real staging catalog into the maintained YAML source of truth without recreating the legacy property explosion

## Summary

The current YAML v1 authoring model is too relationally constrained to represent many real facets cleanly. It still assumes that facet SQL should be reconstructed from properties such as `source_table`, category expressions, clauses, aliases, and route bindings.

That conflicts with two core goals of the query-engine overhaul:

- each facet should create its result set in isolation rather than through one large shared join template
- each facet should own its CTE expression rather than relying on the runtime to reconstruct the intended SQL from fragmented metadata

The recommendation is to make inline SQL templates the default facet authoring model in YAML. Each facet should define a type-specific CTE template that follows a strict output contract. The facet-type compilers should then inject only the user-input placeholders they own, validate the expected columns, and compose the resulting CTEs through the existing anchor-based filter pipeline.

Routes remain useful, but mainly for anchor projection and result-target joins. They should no longer be the primary way to express what a facet means.

The approved baseline for this proposal is now:

- one base-anchor CTE template per facet by default
- route-based projection to other supported anchors where that remains semantically correct
- explicit anchor-to-SQL mappings allowed for exceptions
- result-shape entries such as `result_facet`, `map_result`, and `result_datasets` remain facets rather than moving into a separate metadata model

## Problem

The current YAML authoring shape works for simple relational facets, but it breaks down for much of the actual staging catalog.

The gap is not just about migration volume. It is about representational fit.

- Facets that depend on related tables force extra authoring properties such as `tables` and aliases.
- Facets that depend on UDFs or denormalized sources do not fit the current `source_table` plus route-family shape cleanly.
- Facet-local clauses have to be modeled as separate properties even when they are naturally part of the SQL.
- The current importer rejects anchor-level SQL overrides even though the active route-compiler path already has an `ExplicitSql` concept.

This creates a false choice:

- either keep stretching the relational authoring model with more special-purpose properties
- or leave a large part of the real facet catalog outside the maintained YAML source of truth

Neither option is good enough if YAML is supposed to be the real maintained authoring surface.

## Scope

This proposal covers:

- YAML support for inline SQL templates on facets
- the facet-type-specific SQL contract expected by compilers
- how user input should be injected into facet-owned SQL templates
- how anchor projection should work when the facet’s natural grain differs from the requested composition anchor
- how the importer and runtime should store and use inline facet templates

## Non-Goals

This proposal does not:

- remove routes from the system
- replace the composed filter or result handoff model
- define every line of the importer implementation
- force every facet to be rewritten in one batch
- remove all legacy relational metadata in the same change that introduces inline SQL

## Current Behavior

Relevant current state:

- `route_v1.yaml` is the maintained YAML authoring source of truth for the current v1 configuration draft
- the active importer validates anchors, route families, and facet-anchor bindings, then materializes one active runtime copy in the `facet` schema
- the importer currently rejects anchor-level `sql_override` content as unsupported
- the active route compiler already has an `AnchorTemplate.ExplicitSql` field and the discrete predicate resolver already returns `ExplicitSql` directly when present
- the repository also contains an older YAML experiment in `sead.query.composer/Templates/templates.yml` that used `template_sql`, but that experiment was rejected as the default model because it required full per-anchor SQL templates everywhere

The current source-of-truth decision was right to reject full per-facet-per-anchor SQL as the default authoring shape. But it went too far in the opposite direction by leaving no first-class inline SQL path in the maintained YAML model.

## Proposed Design

### Recommendation

Make inline SQL the default facet authoring model.

Each facet should declare a facet-owned SQL template in YAML. The template should represent the facet’s local result-set logic directly. The runtime should stop treating properties such as `source_table`, `tables`, aliases, and facet-local clauses as the primary way to express facet SQL.

Routes should remain in the design, but mainly for these tasks:

- projecting one facet-owned CTE from its natural or base anchor to another supported anchor
- result-target routing in the existing result handoff model
- handling explicit exceptions where a reusable anchor projection is not sufficient

### Authoring Model

Add an inline SQL block to facet definitions.

Recommended shape:

```yaml
facets:
  - key: family
    display_title: Family
    type: discrete
    sql:
      mode: inline-template
      contract: discrete
      base_anchor: analysis_entity
      body: |
        select
          tf.family_id as category_id,
          tf.family_name as category_name,
          ae.analysis_entity_id as anchor_id
        from facet.abundance_taxon_shortcut ats
        join tbl_taxa_tree_genera tg on tg.genus_id = ats.genus_id
        join tbl_taxa_tree_families tf on tf.family_id = tg.family_id
        join tbl_analysis_entities ae on ae.analysis_entity_id = ats.analysis_entity_id
        where {pick_filter_sql}
    anchors:
      - anchor: analysis_entity
        mode: identity
      - anchor: dataset
        route: analysis_entity__dataset
```

This makes the facet SQL explicit while still allowing reusable anchor projection.

### SQL Contract

Inline SQL is not free-form runtime text. It must follow a type-specific contract.

Minimum required output contract for inline facet CTEs:

- `category_id`
- `anchor_id`

Common optional columns:

- `category_name`
- `sort_value`
- type-specific payload columns such as geometry or typed interval values

Type-specific expectations:

- `discrete`: returns category identity plus anchor identity, and normally also category name
- `range`: returns a range or scalar category value plus anchor identity in a shape the range compiler understands
- `intersect`: returns an interval or range-shaped category plus anchor identity
- `geopolygon`: returns category identity plus geometry-bearing category-info output in the shape expected by the geo compiler and category-info service

The compilers should own the contract for their type. YAML should name the contract explicitly, but not invent a new placeholder language per facet.

### Placeholder Model

Placeholders must be owned and validated by the facet-type compiler, not invented ad hoc in each facet.

Examples:

- discrete compilers may support placeholders such as `{pick_filter_sql}` or `{pick_values_sql}`
- range compilers may support `{low}`, `{high}`, and `{range_filter_sql}`
- intersect compilers may support `{range_filter_sql}` or a typed interval literal placeholder
- geo compilers may support placeholders such as `{polygon_filter_sql}`, `{polygon_wkt}`, and `{srid}`

Rules:

- placeholder names are fixed by compiler contract
- the importer validates that only supported placeholders are used for the declared facet type
- value injection remains compiler-owned and type-safe
- inline SQL must not concatenate unvalidated raw user input directly

### Anchor Strategy

The approved baseline is a hybrid model.

There are three possible models, but the default for implementation is now settled:

1. One full SQL template per supported anchor.
2. One base facet-owned SQL template plus route-based projection from the base anchor to other supported anchors.
3. A hybrid model with one base template by default and explicit per-anchor SQL only when necessary.

The approved recommendation is the hybrid model.

Default behavior:

- every facet declares one `base_anchor`
- the inline CTE returns rows grouped to that base anchor
- other anchors are supported by explicit projection routes from the base anchor to the requested anchor

Exception behavior:

- when projection through routes would be semantically wrong or operationally too expensive, a facet may provide an explicit anchor-to-SQL mapping
- the exception shape is `anchor` + `route` + `sql_override`
- `route` still names the supported anchor path, but `sql_override` tells the importer to persist anchor-specific SQL for that binding instead of relying on projection alone
- the importer should reject exception bindings that omit `route` or `sql_override`

This keeps most facets single-authored while still handling the genuinely exceptional cases.

### Compiler Integration

The facet-type compilers should treat inline SQL templates as the primary source of facet predicate and category-info logic.

That means:

- predicate compilers read imported facet templates instead of reconstructing SQL from relational properties whenever an inline template is present
- category-info and content compilers consume the template contract columns rather than relying on ad hoc joins or legacy property reconstruction
- composed filter generation still intersects anchor-key result sets as it does today
- result projection still joins through `composed_filter` and optional `target_route` as it does today

The active `ExplicitSql` support in the route compiler should be promoted from an exception-only escape hatch into a supported runtime path for imported inline templates.

### Import And Runtime Storage

Promote imported facet templates into the runtime schema as first-class configuration.

Recommended short-term path:

- reuse `facet.facet_template` as the persisted runtime storage for imported inline templates
- store the declared template contract and base-anchor metadata alongside the template body
- keep `facet.facet`, `facet.anchor`, `facet.route`, and `facet.facet_anchor` as supporting metadata for discovery, anchor projection, and runtime validation

This avoids inventing an entirely separate runtime storage surface when a template-oriented one already exists.

### Property Model After The Change

Once inline SQL becomes the default, these legacy properties should become compatibility-only rather than primary facet authoring surfaces:

- `tables`
- alias-heavy multi-table source modeling
- facet-local `clauses`
- per-facet relational reconstruction properties that only exist to rebuild SQL indirectly

Some lightweight metadata still remains useful in YAML even with inline SQL:

- `key`
- `display_title`
- `description`
- `type`
- group metadata
- aggregate metadata
- supported anchors and projection routes

### Result-Shape Facets

`result_facet`, `map_result`, and `result_datasets` should remain facets.

They should not move into a separate result-metadata model as part of this change. Instead, result-oriented facets should be able to reference a compiler-known `template_key` for their inner CTE composition SQL.

That means:

- a result-shape facet can stay in the facet catalog and the maintained YAML surface
- the facet still declares normal facet metadata such as key, type, aggregate metadata, and anchor support
- the inner CTE composition SQL can be selected through a constrained `template_key` rather than repeated ad hoc per result facet
- the compiler owns what each `template_key` means and which facet families may use it
- the only currently accepted key is `anchor_identity`
- only `result_facet`, `map_result`, and `result_datasets` may declare `template_key`

This keeps result-shape behavior governed by the same catalog while avoiding a second authoring surface for inner result composition.

## Alternatives Considered

### Keep relational YAML as the default and use inline SQL only as an override

Rejected.

That would preserve the current mismatch between the real facet catalog and the maintained authoring surface. The hard cases would remain second-class exceptions even though they are common in practice.

### Use one full inline template per facet-anchor pair by default

Rejected as the default.

That recreates the route-explosion problem in YAML and duplicates too much SQL.

### Remove routes entirely and rely only on facet-owned SQL

Rejected.

Routes still solve real problems around anchor projection and result-target joins. The proposal is to narrow their role, not delete them.

## Risks And Tradeoffs

- Inline SQL increases authoring expressiveness, which means validation quality has to rise as well.
- Poorly governed templates could drift into incompatible output shapes if the contract is not enforced strictly.
- The hybrid anchor model adds one more concept: base-anchor templates versus projected anchors.
- Reusing `facet.facet_template` may preserve some legacy naming even though the runtime meaning will shift from “rare override” to “normal template storage”.

## Testing And Validation

Validate the proposal in layers:

- YAML schema validation for the new `sql` block
- importer validation for allowed placeholders, declared contract, base-anchor references, and projected-anchor references
- focused compiler tests per facet type proving placeholder substitution and contract validation
- focused composed-filter tests proving inline-template predicates still compose cleanly through the anchor-based filter path
- focused category-info and content tests proving inline-template facets can render target facet content without legacy relational reconstruction

## Acceptance Criteria

- YAML supports inline facet SQL as a first-class authoring surface
- inline SQL is the documented default authoring model for facets
- each facet type has a documented template contract and placeholder set
- the importer can store and validate inline templates without treating them as unsupported overrides
- the runtime can consume imported inline templates through the facet-type compilers
- routes remain available for anchor projection and explicit exceptions rather than as the primary facet-SQL expression model

## Recommended Delivery Order

1. Define the inline SQL YAML schema, explicit anchor-to-SQL exception shape, and compiler-owned placeholder contract.
2. Promote imported templates into runtime storage and remove the current importer rejection of inline SQL.
3. Implement the result-shape facet path, including `template_key` support for inner CTE composition SQL.
4. Implement one end-to-end driver slice per major supported facet shape using inline SQL.
5. Reclassify the staging export and begin YAML backfill using inline SQL as the default.

## Open Questions

- Should the runtime preserve legacy relational fields for all facets during migration, or only for facets not yet converted to inline SQL?
- Should explicit anchor-to-SQL mappings be stored in the same runtime table shape as base templates, or as explicit per-anchor template rows?
- For target facet content, should all compilers require `category_name` from inline SQL, or should some types still allow derived display metadata outside the template?

## Final Recommendation

Adopt inline SQL templates in YAML as the default facet authoring model.

Treat routes as supporting infrastructure for anchor projection and result-target composition, not as the main language for expressing facet semantics. Use the approved hybrid model with one base-anchor template by default and explicit anchor-to-SQL mappings only when route projection is not enough.

This keeps the query-engine overhaul aligned with its original goals: each facet owns its CTE expression, and each facet can produce its result set in isolation without relying on one large shared relational reconstruction model.
