# SEAD Schema Summary

This file is the compact companion to whatever trusted schema snapshots the active SEAD workspace provides.

Use it when you need to place a table quickly, choose a likely join path, or explain the model at a high level. Use the workspace's trusted table and column schema snapshots for exact current schema facts.

## Collection Context

| Table family | Key tables | Purpose |
| --- | --- | --- |
| Site context | `tbl_sites`, `tbl_site_references`, `tbl_site_locations`, `tbl_site_preservation_status` | Defines excavation or sampling locations and their site-level metadata. |
| Sample grouping | `tbl_sample_groups`, `tbl_sample_group_dimensions`, `tbl_sample_group_references` | Organizes related samples within a site, such as houses, profiles, or cores. |
| Physical samples | `tbl_physical_samples`, `tbl_sample_locations`, `tbl_sample_dimensions`, `tbl_sample_notes`, `tbl_sample_alt_refs` | Describes the collected sample and its identifiers, dimensions, and context. |

## Analytical Packaging

| Table family | Key tables | Purpose |
| --- | --- | --- |
| Analysis bridge | `tbl_analysis_entities`, `tbl_analysis_entity_dimensions`, `tbl_analysis_entity_prep_methods` | Connects one physical sample to one dataset or proxy context. |
| Dataset packaging | `tbl_datasets`, `tbl_dataset_masters`, `tbl_projects`, `tbl_methods`, `tbl_data_types` | Defines the proxy-specific analytical container and its method, owner, and quantification type. |
| Provenance | `tbl_dataset_contacts`, `tbl_dataset_submissions`, `tbl_contacts`, `tbl_contact_types` | Tracks who provided, digitized, or submitted dataset content. |

## Proxy And Result Storage

| Table family | Key tables | Purpose |
| --- | --- | --- |
| Biological abundance | `tbl_abundances`, `tbl_abundance_elements`, `tbl_abundance_ident_levels`, `tbl_abundance_modifications` | Stores taxon-linked counts, parts, and identification or modification qualifiers. |
| Generic typed values | `tbl_analysis_values`, `tbl_analysis_boolean_values`, `tbl_analysis_categorical_values`, `tbl_analysis_integer_values`, `tbl_analysis_numerical_values`, `tbl_analysis_dating_ranges`, `tbl_analysis_taxon_counts` | Stores non-abundance analysis results in typed companion tables. Validate deployed availability before relying on them. |
| Domain-specific results | `tbl_ceramics`, `tbl_dendro`, `tbl_tephra_dates`, `tbl_measured_values` | Stores method-specific result structures attached to analysis entities. |

## Chronology And Dating

| Table family | Key tables | Purpose |
| --- | --- | --- |
| Interpreted chronology | `tbl_analysis_entity_ages`, `tbl_chronologies`, `tbl_chron_controls` | Captures interpreted ages, age ranges, and chronology models. |
| Absolute dating | `tbl_geochronology`, `tbl_dating_material`, `tbl_dating_labs`, `tbl_dating_uncertainty` | Stores radiometric dates, dated material, lab identity, and uncertainty metadata. |
| Auxiliary dating vocabularies | `tbl_age_types`, `tbl_years_types`, `tbl_seasons` | Provides year-system and season vocabulary used by date-related tables. |

## Taxonomy And Ecology

| Table family | Key tables | Purpose |
| --- | --- | --- |
| Taxonomic backbone | `tbl_taxa_tree_master`, `tbl_taxa_tree_genera`, `tbl_taxa_tree_families`, `tbl_taxa_tree_orders` | Defines taxonomic identifiers used by proxy result tables. |
| Taxon metadata | `tbl_taxa_common_names`, `tbl_taxonomy_notes`, `tbl_species_associations`, `tbl_text_biology`, `tbl_text_distribution` | Adds descriptive, ecological, and interpretive context to taxa. |
| Environmental coding | `tbl_ecocodes`, `tbl_ecocode_definitions`, `tbl_ecocode_groups`, `tbl_ecocode_systems` | Links taxa to ecocode systems and ecological classifications. |

## Bibliography And Controlled Vocabularies

| Table family | Key tables | Purpose |
| --- | --- | --- |
| Bibliography | `tbl_biblio`, `tbl_publication_types`, `tbl_publishers`, `tbl_collections_or_journals` | Centralizes citations and publication metadata used throughout the model. |
| Shared vocabularies | `tbl_dimensions`, `tbl_units`, `tbl_record_types`, `tbl_value_classes`, `tbl_value_types` | Defines reusable vocabulary and measurement semantics that many tables depend on. |

## High-Value Join Chains

- Site to taxa: `tbl_sites` -> `tbl_sample_groups` -> `tbl_physical_samples` -> `tbl_analysis_entities` -> `tbl_abundances` -> `tbl_taxa_tree_master`
- Site to datasets: `tbl_sites` -> `tbl_sample_groups` -> `tbl_physical_samples` -> `tbl_analysis_entities` -> `tbl_datasets`
- Analysis entity to chronology: `tbl_analysis_entities` -> `tbl_analysis_entity_ages` -> `tbl_chronologies`
- Analysis entity to absolute dates: `tbl_analysis_entities` -> `tbl_geochronology`
- Dataset to provenance: `tbl_datasets` -> `tbl_dataset_contacts` -> `tbl_contacts`

## Caveats

- Exact table and column facts should come from the current workspace's trusted schema snapshots or live exports.
- The repository contains later dated migrations under `sead_model/deploy/`, so deployment-specific SQL may need live-schema confirmation.