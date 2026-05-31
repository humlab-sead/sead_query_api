# Glossary

## Site

A defined excavation or sampling location stored in `tbl_sites`.

## Sample group

A flexible grouping of related samples within a site, such as a house, profile, or core. Stored in `tbl_sample_groups`.

## Physical sample

The collected sample record stored in `tbl_physical_samples`.

## Analysis entity

A virtual bridge record that lets one physical sample participate in one dataset or proxy context. Stored in `tbl_analysis_entities`.

## Dataset

A structured collection of analysis entities for a specific proxy or method. Stored in `tbl_datasets`.

## Dataset master

A higher-level source or owner of datasets, such as a contributing database, project, or laboratory. Stored in `tbl_dataset_masters`.

## Method

The analysis method definition stored in `tbl_methods`.

## Abundance

A taxon-linked count, presence value, or scaled value for an analysis entity. Stored in `tbl_abundances`.

## Abundance element

The counted part or unit, such as seed, leaf, or MNI. Stored in `tbl_abundance_elements`.

## Taxon

The finest taxonomic unit used in result tables. Stored in `tbl_taxa_tree_master`.

## Chronology

An age-model or grouped dating context stored in `tbl_chronologies`.

## Geochronology

An absolute dating record, such as a radiocarbon measurement, stored in `tbl_geochronology`.

## Bibliography

Central citation storage in `tbl_biblio`.

## Provenance

The contacts, submission events, master dataset links, and references that describe where a dataset came from and who is associated with it.