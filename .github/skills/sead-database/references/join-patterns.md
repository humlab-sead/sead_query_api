# Join Patterns

Use the shortest verified path first. Only add side tables after the backbone query is correct.

## 1. Site to taxa found at the site

Path:

`tbl_sites` -> `tbl_sample_groups` -> `tbl_physical_samples` -> `tbl_analysis_entities` -> `tbl_abundances` -> `tbl_taxa_tree_master`

Use this when the user wants species lists, counts, or presence data tied to a site.

Watch for:

- duplicate rows when the same taxon occurs in multiple analysis entities
- method-specific filtering, which usually enters through `tbl_datasets`

## 2. Site to datasets, methods, and projects

Path:

`tbl_sites` -> `tbl_sample_groups` -> `tbl_physical_samples` -> `tbl_analysis_entities` -> `tbl_datasets`

Then extend from `tbl_datasets` to:

- `tbl_methods`
- `tbl_projects`
- `tbl_dataset_masters`
- `tbl_biblio`

Use this for inventory, provenance, or method summaries.

## 3. Physical sample to all attached proxies

Path:

`tbl_physical_samples` -> `tbl_analysis_entities` -> `tbl_datasets`

This is the cleanest way to answer questions like "what analyses were performed on this sample?"

## 4. Analysis entity to interpreted chronology

Path:

`tbl_analysis_entities` -> `tbl_analysis_entity_ages` -> `tbl_chronologies`

Use this when the user wants interpreted ages, age ranges, chronology names, or age-model context.

## 5. Analysis entity to absolute dating details

Path:

`tbl_analysis_entities` -> `tbl_geochronology`

Then extend to:

- `tbl_dating_labs`
- `tbl_dating_uncertainty`
- `tbl_dating_material`
- `tbl_taxa_tree_master` through `tbl_dating_material.taxon_id` when the dated material is taxon-specific

Use this for radiocarbon and other radiometric date questions.

## 6. Dataset to provenance and people

Path:

`tbl_datasets` -> `tbl_dataset_contacts` -> `tbl_contacts`

Optional additions:

- `tbl_contact_types`
- `tbl_dataset_submissions`
- `tbl_dataset_submission_types`

Use this for provider, digitizer, submission, or ownership questions.

## 7. Dataset to bibliography

Path:

`tbl_datasets.biblio_id` -> `tbl_biblio`

Related higher-level context:

- `tbl_methods.biblio_id`
- `tbl_dataset_masters.biblio_id`
- `tbl_site_references` for site-level references

Use this when the user asks where publications or citations are stored.

## 8. Analysis entity to generic typed values

Path:

`tbl_analysis_entities` -> `tbl_analysis_values` -> typed value tables

Typed tables documented in comments include boolean, categorical, dating-range, integer, numerical, note, and taxon-count tables.

Use this when the data is not stored as abundance rows. Validate the target schema first, because these tables are documented in comments and may depend on later migrations.

## Join Strategy

- Start from the user's anchor entity.
- Walk toward the measure table, not away from it.
- Add method, project, contact, and bibliography joins only after the backbone path is correct.
- If the user asks for one row per site or one row per sample, aggregate before joining wide provenance tables.