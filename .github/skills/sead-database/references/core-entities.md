# Core Entities

## Location

`tbl_locations` stores named geographical locations, typically regions that may be current or historical. It provides reusable location context through a location type plus optional default latitude and longitude, and it is linked from site, contact, rarity, seasonality, and relative-age context tables.

## Site

`tbl_sites` stores excavation or sampling locations. It carries the site name, descriptive text, geographic coordinates, altitude, and site-level preservation metadata.

## Sample group

`tbl_sample_groups` groups related samples within a site. Comments describe these as flexible collections such as houses, stratigraphic profiles, or lake cores. Each group belongs to a site and records a retrieval method and sampling context.

## Physical sample

`tbl_physical_samples` records the collected sample itself. It belongs to a sample group and stores the primary sample name, sample type, and optional alternate-reference type.

## Analysis entity

`tbl_analysis_entities` is the key bridge table in SEAD. Comments describe it as a virtual construct that lets one physical sample participate in multiple proxy-specific datasets. In practice, it links a physical sample to one dataset.

## Dataset

`tbl_datasets` organizes collections of analysis entities for a specific proxy or method. The comments explicitly frame a dataset as a structured collection that is relevant to the proxy being studied.

## Dataset master

`tbl_dataset_masters` represents the larger source or owning collection for datasets, such as BugsCEP, MAL, or a laboratory.

## Method

`tbl_methods` defines analysis methods, including method group, record type, unit, bibliographic reference, and descriptive text.

## Project

`tbl_projects` stores project-level metadata used by datasets. It is a contextual owner, not the main measurement bridge.

## Abundance record

`tbl_abundances` stores taxon-linked counts, presence values, or scale values for one analysis entity. This is the main path for biological proxy result lists.

## Abundance element

`tbl_abundance_elements` defines what part or unit is being counted, such as a seed, a leaf, or Minimum Number of Individuals.

## Taxon

`tbl_taxa_tree_master` stores the finest taxonomic unit used by result tables. Comments note that this includes species-level taxa as well as forms such as `sp.`, `spp.`, groups, and split identifications.

## Analysis value

`tbl_analysis_values` is documented in comments as a generic result table storing untyped string values plus state flags. The comments also describe typed companion tables such as boolean, categorical, integer, numerical, dating-range, and taxon-count value tables. Validate deployed availability before relying on these tables in generated SQL.

## Interpreted age

`tbl_analysis_entity_ages` stores interpreted ages or age ranges for an analysis entity and can link that interpretation to a chronology.

## Chronology

`tbl_chronologies` stores the broader age-model context. Comments describe it as a grouping of dated samples used for unified age ranges or age-depth models.

## Absolute dating

`tbl_geochronology` stores radiometric or absolute dating measurements for an analysis entity. `tbl_dating_material` describes what was dated, and `tbl_dating_labs` identifies the laboratory.

## Bibliography and provenance

`tbl_biblio` is the central bibliography table. Dataset provenance is extended through `tbl_dataset_contacts`, `tbl_dataset_submissions`, and `tbl_contacts`.