## Purpose

This guide explains how to author `sead.query.composer/Templates/facet_configuration.yml`.

It focuses on:

* the structure of the YAML file
* the meaning of each top-level section
* the semantic role of each important property
* the rules that prevent common validation and import failures

Use this guide when adding, changing, or reviewing facet configuration before validation and import.

## Canonical Files

| Purpose                 | File or location                                                    |
| ----------------------- | ------------------------------------------------------------------- |
| Authoring source        | `sead.query.composer/Templates/facet_configuration.yml`             |
| Authoring contract      | `sead.query.composer/Templates/facet-configuration-schema.json`     |
| Importer implementation | `sead.query.infra/Configuration/FacetRouteConfigurationImporter.cs` |
| Runtime copy            | Imported rows in schema `facet`, including `facet.config_revision`  |

## File Anatomy

The configuration has these top-level sections:

1. `schema_version`
2. `config_revision`
3. `runtime_import`
4. `anchors`
5. `paths`
6. `route_templates`
7. `facets`

Minimal shape:

```yaml
schema_version: 1
config_revision: phaseX-some-revision

runtime_import:
  target_schema: facet
  mode: merge-into-existing

anchors: []
paths: []
route_templates: []
facets: []
```

## Top-Level Sections

### `schema_version`

Required value:

```yaml
schema_version: 1
```

Semantic meaning:

`schema_version` identifies the configuration contract version expected by the importer and validator.

Authoring rule:

Do not change this value unless the schema and importer both support the new version.

### `config_revision`

Example:

```yaml
config_revision: phaseX-some-revision
```

Semantic meaning:

`config_revision` is the logical revision label recorded in runtime metadata, specifically in `facet.config_revision.config_revision`.

Practical use:

Use it to trace which authored configuration was imported into the runtime database.

Authoring rule:

The value must be a non-empty string.

### `runtime_import`

Required shape:

```yaml
runtime_import:
  target_schema: facet
  mode: merge-into-existing
```

Semantic meaning:

`runtime_import` declares where and how the configuration is imported.

Required values:

| Property        | Required value        | Meaning                                                 |
| --------------- | --------------------- | ------------------------------------------------------- |
| `target_schema` | `facet`               | Import into the runtime `facet` schema                  |
| `mode`          | `merge-into-existing` | Merge authored configuration into existing runtime data |

Authoring rule:

Use only the supported values above unless the importer has been extended.

## Anchors

`anchors` define stable target identity surfaces for routes and facet bindings.

Example:

```yaml
anchors:
  - key: sample
    table: tbl_physical_samples
    key_column: physical_sample_id
    description: Physical sample identity
```

Required properties per anchor:

| Property      | Meaning                                                                           |
| ------------- | --------------------------------------------------------------------------------- |
| `key`         | Anchor name used by route templates and facets, for example `sample` or `dataset` |
| `table`       | Terminal table that routes to this anchor must end on                             |
| `key_column`  | Identity column for the anchor table                                              |
| `description` | Human-readable intent                                                             |

Semantic meaning:

An anchor represents an endpoint type. Routes that bind to an anchor must end on the table declared by that anchor.

Important rule:

A facet anchor binding is valid only when the bound route ends on the same table as the declared anchor.

## Paths

`paths` are reusable route macros.

Example:

```yaml
paths:
  - key: sample_to_dataset
    path:
      - table: tbl_analysis_entities
      - table: tbl_datasets
```

Required properties per path:

| Property | Meaning                     |
| -------- | --------------------------- |
| `key`    | Macro name                  |
| `path`   | Ordered list of route steps |

Each path step must be one of:

```yaml
- table: <table_name>
```

or:

```yaml
- include: <path_key>
```

Semantic meaning:

Paths allow common traversal segments to be reused by route templates.

Authoring rules:

* Use `table` steps for concrete route traversal.
* Use `include` steps to reuse another path.
* Do not create include cycles.

## Route Templates

A route template defines a route family from one source table to one or more anchors.

Example shape:

```yaml
route_templates:
  - key: country_routes
    source_table: tbl_countries
    source_key_column: country_id
    generated_route_key_pattern: country__{anchor}
    anchors:
      sample:
        path:
          - include: country_to_sample
      dataset:
        path:
          - include: country_to_dataset
```

Required properties per route template:

| Property                      | Meaning                                           |
| ----------------------------- | ------------------------------------------------- |
| `key`                         | Route template identifier                         |
| `source_table`                | Source table for the generated routes             |
| `source_key_column`           | Identity column on the source table               |
| `generated_route_key_pattern` | Pattern used to generate concrete route keys      |
| `anchors`                     | Map of anchor bindings generated from this source |

Important constraint:

`generated_route_key_pattern` must end with:

```text
__{anchor}
```

Each `anchors.<anchor_key>` binding requires:

| Property | Meaning                                              |
| -------- | ---------------------------------------------------- |
| `path`   | Route path from the source table to the anchor table |

Semantic meaning:

Route templates generate concrete route names such as:

```text
country__analysis_entity
country__sample
country__dataset
```

These generated route names are then referenced by facet anchor bindings.

Authoring rules:

* Every route template anchor key must reference a declared anchor.
* The expanded path for an anchor binding must end on the table declared by that anchor.
* Generated route names must be stable because facets depend on them.

## Facets

A facet is a queryable or filterable unit with metadata, category semantics, aggregation behavior, and anchor projection bindings.

Example shape:

```yaml
facets:
  - key: country
    display_title: Country
    description: Country facet
    group_key: geography
    type: discrete
    source_table: tbl_countries
    category:
      id_expr: country_id
      name_expr: country_name
      data_type: integer
      operator: equals
    sort_expr: country_name
    flags:
      is_applicable: true
      is_default: false
    aggregate:
      type: count
      title: Count
    anchors:
      - anchor: sample
        route: country__sample
```

Required properties per facet:

| Property        | Meaning                                                |
| --------------- | ------------------------------------------------------ |
| `key`           | Stable facet identifier used by runtime wiring         |
| `display_title` | UI-facing title                                        |
| `description`   | Maintainer and user intent text                        |
| `group_key`     | Runtime facet-group assignment                         |
| `type`          | Facet type                                             |
| `source_table`  | Source relation for the facet                          |
| `category`      | Category identity, label, type, and operator semantics |
| `sort_expr`     | SQL expression used to sort category rows              |
| `flags`         | Applicability and default behavior                     |
| `aggregate`     | Count or aggregation metadata                          |
| `anchors`       | Anchor bindings for this facet                         |

Supported `type` values:

```text
discrete
range
intersect
geopolygon
```

Authoring rule:

`group_key` must refer to an existing runtime facet group.

## Facet Property Semantics

### Metadata and identity

| Property        | Meaning                                            |
| --------------- | -------------------------------------------------- |
| `key`           | Stable identifier used in route/facet wiring       |
| `display_title` | Title shown to users                               |
| `description`   | Human-readable intent                              |
| `group_key`     | Group assignment                                   |
| `type`          | Facet behavior model                               |
| `source_table`  | Source relation for category and anchor derivation |

`source_table` may be a table name or another supported qualified reference, such as a supported UDF call.

### `category`

`category` defines how category identity and display values are produced.

Required fields:

| Property    | Meaning                                           |
| ----------- | ------------------------------------------------- |
| `id_expr`   | SQL expression for category identity              |
| `name_expr` | SQL expression for category label                 |
| `data_type` | Value type used by category and filter processing |
| `operator`  | Filter operator semantics                         |

Semantic meaning:

The category expressions define the value model used by pick lists, range filters, and other category-driven filtering behavior.

Authoring rule:

Keep `category`, `type`, `sql.contract`, and operator semantics aligned.

### `sort_expr`

`sort_expr` is the SQL expression used to order facet category rows.

Example:

```yaml
sort_expr: country_name
```

Authoring rule:

Use an expression that is valid in the category query context.

### `flags`

Required fields:

| Property        | Meaning                                                    |
| --------------- | ---------------------------------------------------------- |
| `is_applicable` | Whether the facet is applicable in runtime/client behavior |
| `is_default`    | Whether the facet is treated as a default facet            |

Semantic meaning:

These flags control applicability and default behavior as consumed by runtime code and clients.

### `aggregate`

Required fields:

| Property | Meaning           |
| -------- | ----------------- |
| `type`   | Aggregation type  |
| `title`  | Aggregation title |

Optional field:

| Property    | Meaning                     |
| ----------- | --------------------------- |
| `facet_key` | Dependency on another facet |

Semantic meaning:

`aggregate` declares count or aggregation metadata. If `facet_key` is set, it must reference an existing facet.

### `template_key`

Current supported value:

```yaml
template_key: anchor_identity
```

Semantic meaning:

`template_key` marks facets that use retained template behavior instead of normal inline SQL-template authoring.

Important constraint:

Only retained result-shape facets may declare `template_key`.

Currently supported retained result-shape facet keys:

```text
result_facet
map_result
result_datasets
```

Authoring rule:

Do not add `template_key` to ordinary authored facets.

### `sql`

`sql` defines inline SQL-template behavior for facets that derive categories and anchors from a base SQL template.

Example shape:

```yaml
sql:
  mode: inline-template
  contract: discrete
  base_anchor: sample
  body: |
    SELECT ...
```

Required fields when `sql` is present:

| Property            | Required value or meaning                    |
| ------------------- | -------------------------------------------- |
| `mode`              | Must be `inline-template`                    |
| `contract`          | Currently `discrete` or `range`              |
| `base_anchor`       | Anchor key emitted by the SQL as `anchor_id` |
| `body` or `cte_sql` | SQL template content                         |

Semantic meaning:

The SQL block defines the base template used for category and anchor derivation.

Authoring rules:

* `base_anchor` must also appear in the facet `anchors` list.
* The facet `type` and `sql.contract` must match.
* The SQL must emit the expected shape for the selected contract.
* Use only placeholders supported by the selected contract.

Allowed placeholders for `discrete`:

```text
{pick_filter_sql}
{pick_values_sql}
```

Allowed placeholders for `range`:

```text
{low}
{high}
{range_filter_sql}
```

### `clauses`

Example:

```yaml
clauses:
  - "some_column IS NOT NULL"
```

Semantic meaning:

`clauses` is an optional list of facet-owned fixed SQL constraints associated with the facet definition.

Authoring rule:

Use clauses for constraints that belong to the facet itself, not for route-specific anchor projection logic.

### Facet `anchors`

Facet anchor bindings link the facet to one or more target anchors through concrete routes.

Example:

```yaml
anchors:
  - anchor: sample
    route: country__sample
  - anchor: dataset
    route: country__dataset
```

Required fields per binding:

| Property | Meaning                                          |
| -------- | ------------------------------------------------ |
| `anchor` | Declared anchor key                              |
| `route`  | Concrete route key generated by a route template |

Optional field:

| Property       | Meaning                      |
| -------------- | ---------------------------- |
| `sql_override` | Explicit anchor-specific SQL |

Semantic meaning:

The route projects facet source records to the selected anchor surface.

Authoring rules:

* `anchor` must reference a declared anchor.
* `route` must reference a generated route.
* The route target table must match the declared anchor table.
* Use `sql_override` only when normal projection from base template SQL is not sufficient.
* If any anchor binding uses `sql_override`, the facet must also define `sql`.
* `sql_override` cannot target the same anchor as `sql.base_anchor`.

## Common Failure Prevention Rules

Follow these rules to avoid common authoring, validation, and import failures.

1. Keep semantically coupled route, anchor, and facet changes in the same commit.
2. Do not rely on undeclared properties.
3. Treat YAML property names as strict, even if deserialization ignores unknown fields.
4. Ensure every facet anchor binding references a valid concrete route.
5. Ensure every route template anchor path ends on that anchor's declared table.
6. Keep `type`, `sql.contract`, category expressions, and operator semantics aligned.
7. Use `template_key` only for supported retained result-shape facets.
8. Use only placeholders supported by the selected SQL contract.
9. Keep generated route names stable when existing facets depend on them.
10. Validate before importing into runtime data.

Important note:

The importer deserializer may ignore unmatched YAML properties. A typo can therefore be silently ignored unless caught by schema or semantic validation.

## Worked Authoring Checklist

Use this checklist after any YAML edit.

### Top-level contract

* `schema_version` is still valid.
* `config_revision` is present and meaningful.
* `runtime_import.target_schema` is `facet`.
* `runtime_import.mode` is `merge-into-existing`.
* Required top-level sections are present.

### Anchors

* Every anchor has `key`, `table`, `key_column`, and `description`.
* Anchor keys are stable.
* Anchor tables are the correct route terminal tables.

### Paths

* Every path has a unique `key`.
* Every path step is either `table` or `include`.
* Included paths exist.
* Includes do not form cycles.

### Route templates

* Every route template has the required metadata.
* `generated_route_key_pattern` ends in `__{anchor}`.
* Every anchor binding references a declared anchor.
* Every expanded route path ends on the expected anchor table.
* Generated route keys match the names used by facets.

### Facets

* Every facet has the required metadata.
* `group_key` exists in runtime facet-group data.
* `type` is one of the supported values.
* `category` expressions match the facet behavior.
* `sort_expr` is valid in the category context.
* `aggregate.facet_key`, when present, references an existing facet.
* `template_key`, when present, is allowed for that facet.
* Inline SQL placeholders match the selected SQL contract.
* Anchor bindings reference valid anchors and valid generated routes.

## Validation and Import Workflow

After authoring, run semantic validation:

```bash
make validate-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/facet_configuration.yml
```

If PostgreSQL is intentionally unavailable, run offline validation:

```bash
make validate-facet-config-offline FACET_CONFIG_FILE=sead.query.composer/Templates/facet_configuration.yml
```

Then run focused tests for the affected request and facet slices.

When the configuration is ready to mutate runtime data, import it:

```bash
make import-facet-config FACET_CONFIG_FILE=sead.query.composer/Templates/facet_configuration.yml
```

After import, verify active revision metadata:

```sql
SELECT
  revision_id,
  source_commit,
  content_hash,
  imported_at,
  imported_by,
  is_active
FROM facet.config_revision
WHERE is_active = true;
```

## Related Documents

| Document              | Purpose                                                             |
| --------------------- | ------------------------------------------------------------------- |
| `docs/DEVELOPMENT.md` | Contributor workflow and route/facet maintenance patterns           |
| `docs/TESTING.md`     | Validation-mode guidance for local, CI, and bootstrap contexts      |
| `docs/OPERATIONS.md`  | Deployment-time import provenance and promotion checks              |
| `docs/DESIGN.md`      | Architecture boundaries and composed versus legacy runtime behavior |
