# Schema Overview

This skill is about the SEAD data model, not one repository layout.

Typical source inputs for this skill are:

- live schema exports from PostgreSQL catalogs
- checked-in DDL and foreign-key definitions
- checked-in column and table comments
- application-specific table or column inventories

Prefer the most current trusted source available in the active workspace.

## Reference Levels

- trusted table-level schema summaries for current tables, descriptions, PK columns, and FK summaries
- trusted column-level schema listings for current table and column metadata
- `sead-schema-summary.md`: quick human-readable orientation by table family and usage
- foreign-key definitions or catalog extracts as the authoritative source when join correctness matters

Use the current workspace's trusted schema snapshots as the primary current schema references.

## Core Layers

### Context and collection

- `tbl_sites`: excavation or sampling location
- `tbl_sample_groups`: flexible groupings inside a site, such as structures, profiles, or cores
- `tbl_physical_samples`: collected samples within a sample group

### Analytical packaging

- `tbl_analysis_entities`: the bridge between physical samples and datasets
- `tbl_datasets`: proxy- or method-oriented dataset container
- `tbl_dataset_masters`: higher-level source or contributing collection
- `tbl_methods`: analysis method definition
- `tbl_projects`: project metadata attached to datasets

### Proxy and result storage

- `tbl_abundances`: taxon-linked counts, presence, or scaled values for an analysis entity
- `tbl_abundance_elements`: the counted part or unit, such as seed, leaf, or MNI
- `tbl_analysis_values` and typed analysis-value tables: generic result model documented in comments; validate availability in the deployed target before relying on it
- Domain tables such as `tbl_ceramics`, `tbl_dendro`, and `tbl_geochronology` attach method-specific results to analysis entities

### Chronology and dating

- `tbl_analysis_entity_ages`: interpreted age or age range for an analysis entity
- `tbl_chronologies`: grouped age model context
- `tbl_chron_controls`: dated control points inside a chronology
- `tbl_geochronology`: radiometric or absolute dating records tied to analysis entities
- `tbl_dating_material`: the material dated, optionally linked to taxa and abundance elements
- `tbl_dating_labs`: laboratory metadata for dating records

### Taxonomy and ecological context

- `tbl_taxa_tree_master`: finest taxonomic unit used in result tables
- Supporting taxonomy tables link genera, families, orders, common names, images, ecocodes, and notes back to `tbl_taxa_tree_master`

### Provenance and bibliography

- `tbl_biblio`: central bibliography store
- `tbl_dataset_contacts`: who is connected to a dataset and in what role
- `tbl_dataset_submissions`: ingestion or submission events for a dataset
- `tbl_contacts`: reusable people and organization records

## Backbone Chains

Use these chains before looking for narrower extensions:

- Site to proxy counts: `tbl_sites` -> `tbl_sample_groups` -> `tbl_physical_samples` -> `tbl_analysis_entities` -> `tbl_abundances` -> `tbl_taxa_tree_master`
- Site to datasets: `tbl_sites` -> `tbl_sample_groups` -> `tbl_physical_samples` -> `tbl_analysis_entities` -> `tbl_datasets`
- Dataset to method and project: `tbl_datasets` -> `tbl_methods`, `tbl_projects`, `tbl_dataset_masters`, `tbl_biblio`
- Analysis entity to interpreted age: `tbl_analysis_entities` -> `tbl_analysis_entity_ages` -> `tbl_chronologies`
- Analysis entity to absolute dating: `tbl_analysis_entities` -> `tbl_geochronology` -> `tbl_dating_labs`

## Practical Framing

When the user asks a SEAD question, first decide which layer owns the answer:

- context of collection
- analytical packaging
- proxy values
- chronology or dating
- taxonomy
- provenance or bibliography

That usually reveals the correct join path and the safest project for a schema change.
That usually reveals the correct join path and which layer of the model actually owns the answer.