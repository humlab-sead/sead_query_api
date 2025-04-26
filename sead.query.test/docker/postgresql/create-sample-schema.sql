/* Sample database for testing purpose. 
*/

drop schema if exists sample cascade;
create schema sample;


/* Physical sample determines subsetting */

create view sample.tbl_physical_samples as
    select *
    from public.tbl_physical_samples
    where TRUE
    limit 100;

/* Empty tables */
create view sample.tbl_aggregate_datasets as select * from public.tbl_aggregate_datasets where FALSE;
create view sample.tbl_aggregate_order_types as select * from public.tbl_aggregate_order_types where FALSE;
create view sample.tbl_aggregate_sample_ages as select * from public.tbl_aggregate_sample_ages where FALSE;
create view sample.tbl_aggregate_samples as select * from public.tbl_aggregate_samples where FALSE;
create view sample.tbl_ceramics_measurements as select * from public.tbl_ceramics_measurements where FALSE;
create view sample.tbl_chronologies as select * from public.tbl_chronologies where FALSE;
create view sample.tbl_colours as select * from public.tbl_colours where FALSE;
create view sample.tbl_dataset_methods as select * from public.tbl_dataset_methods where FALSE;
create view sample.tbl_ecocode_definitions as select * from public.tbl_ecocode_definitions where FALSE;
create view sample.tbl_ecocode_groups as select * from public.tbl_ecocode_groups where FALSE;
create view sample.tbl_ecocode_systems as select * from public.tbl_ecocode_systems where FALSE;
create view sample.tbl_ecocodes as select * from public.tbl_ecocodes where FALSE;
create view sample.tbl_geochron_refs as select * from public.tbl_geochron_refs where FALSE;
create view sample.tbl_imported_taxa_replacements as select * from public.tbl_imported_taxa_replacements where FALSE;
create view sample.tbl_isotope_measurements as select * from public.tbl_isotope_measurements where FALSE;
create view sample.tbl_isotope_standards as select * from public.tbl_isotope_standards where FALSE;
create view sample.tbl_isotope_types as select * from public.tbl_isotope_types where FALSE;
create view sample.tbl_isotope_value_specifiers as select * from public.tbl_isotope_value_specifiers where FALSE;
create view sample.tbl_isotopes as select * from public.tbl_isotopes where FALSE;
create view sample.tbl_languages as select * from public.tbl_languages where FALSE;
create view sample.tbl_lithology as select * from public.tbl_lithology where FALSE;
create view sample.tbl_mcr_names as select * from public.tbl_mcr_names where FALSE;
create view sample.tbl_mcr_summary_data as select * from public.tbl_mcr_summary_data where FALSE;
create view sample.tbl_mcrdata_birmbeetledat as select * from public.tbl_mcrdata_birmbeetledat where FALSE;
create view sample.tbl_measured_value_dimensions as select * from public.tbl_measured_value_dimensions where FALSE;
create view sample.tbl_sample_colours as select * from public.tbl_sample_colours where FALSE;
create view sample.tbl_sample_group_images as select * from public.tbl_sample_group_images where FALSE;
create view sample.tbl_sample_images as select * from public.tbl_sample_images where FALSE;
create view sample.tbl_site_images as select * from public.tbl_site_images where FALSE;
create view sample.tbl_site_natgridrefs as select * from public.tbl_site_natgridrefs where FALSE;
create view sample.tbl_site_preservation_status as select * from public.tbl_site_preservation_status where FALSE;
create view sample.tbl_taxa_images as select * from public.tbl_taxa_images where FALSE;
create view sample.tbl_taxa_reference_specimens as select * from public.tbl_taxa_reference_specimens where FALSE;
create view sample.tbl_taxa_synonyms as select * from public.tbl_taxa_synonyms where FALSE;
create view sample.tbl_taxonomic_order_biblio as select * from public.tbl_taxonomic_order_biblio where FALSE;
create view sample.tbl_temperatures as select * from public.tbl_temperatures where FALSE;
create view sample.tbl_tephra_dates as select * from public.tbl_tephra_dates where FALSE;
create view sample.tbl_tephra_refs as select * from public.tbl_tephra_refs where FALSE;
create view sample.tbl_tephras as select * from public.tbl_tephras where FALSE;
create view sample.tbl_updates_log as select * from public.tbl_updates_log where FALSE;

/* Full tables */

create view sample.tbl_abundance_elements as select * from public.tbl_abundance_elements;
create view sample.tbl_activity_types as select * from public.tbl_activity_types;
create view sample.tbl_age_types as select * from public.tbl_age_types;
create view sample.tbl_alt_ref_types as select * from public.tbl_alt_ref_types;
create view sample.tbl_ceramics_lookup as select * from public.tbl_ceramics_lookup;
create view sample.tbl_contact_types as select * from public.tbl_contact_types;
create view sample.tbl_coordinate_method_dimensions as select * from public.tbl_coordinate_method_dimensions;
create view sample.tbl_data_type_groups as select * from public.tbl_data_type_groups;
create view sample.tbl_data_types as select * from public.tbl_data_types;
create view sample.tbl_dataset_masters as select * from public.tbl_dataset_masters;
create view sample.tbl_dataset_submission_types as select * from public.tbl_dataset_submission_types;
create view sample.tbl_dating_labs as select * from public.tbl_dating_labs;
create view sample.tbl_dating_material as select * from public.tbl_dating_material;
create view sample.tbl_dating_uncertainty as select * from public.tbl_dating_uncertainty;
create view sample.tbl_dendro_lookup as select * from public.tbl_dendro_lookup;
create view sample.tbl_dimensions as select * from public.tbl_dimensions;
create view sample.tbl_feature_types as select * from public.tbl_feature_types;
create view sample.tbl_identification_levels as select * from public.tbl_identification_levels;
create view sample.tbl_image_types as select * from public.tbl_image_types;
create view sample.tbl_location_types as select * from public.tbl_location_types;
create view sample.tbl_method_groups as select * from public.tbl_method_groups;
create view sample.tbl_methods as select * from public.tbl_methods;
create view sample.tbl_modification_types as select * from public.tbl_modification_types;
create view sample.tbl_project_stages as select * from public.tbl_project_stages;
create view sample.tbl_project_types as select * from public.tbl_project_types;
create view sample.tbl_rdb_codes as select * from public.tbl_rdb_codes;
create view sample.tbl_rdb_systems as select * from public.tbl_rdb_systems;
create view sample.tbl_record_types as select * from public.tbl_record_types;
create view sample.tbl_relative_age_types as select * from public.tbl_relative_age_types;
create view sample.tbl_relative_ages as select * from public.tbl_relative_ages;
create view sample.tbl_sample_description_sample_group_contexts as select * from public.tbl_sample_description_sample_group_contexts;
create view sample.tbl_sample_description_types as select * from public.tbl_sample_description_types;
create view sample.tbl_sample_group_description_type_sampling_contexts as select * from public.tbl_sample_group_description_type_sampling_contexts;
create view sample.tbl_sample_group_description_types as select * from public.tbl_sample_group_description_types;
create view sample.tbl_sample_group_sampling_contexts as select * from public.tbl_sample_group_sampling_contexts;
create view sample.tbl_sample_location_type_sampling_contexts as select * from public.tbl_sample_location_type_sampling_contexts;
create view sample.tbl_sample_location_types as select * from public.tbl_sample_location_types;
create view sample.tbl_sample_types as select * from public.tbl_sample_types;
create view sample.tbl_season_types as select * from public.tbl_season_types;
create view sample.tbl_seasons as select * from public.tbl_seasons;
create view sample.tbl_species_association_types as select * from public.tbl_species_association_types;
create view sample.tbl_taxa_tree_orders as select * from public.tbl_taxa_tree_orders;
create view sample.tbl_taxonomic_order_systems as select * from public.tbl_taxonomic_order_systems;
create view sample.tbl_units as select * from public.tbl_units;
create view sample.tbl_value_classes as select * from public.tbl_value_classes;
create view sample.tbl_value_qualifier_symbols as select * from public.tbl_value_qualifier_symbols;
create view sample.tbl_value_qualifiers as select * from public.tbl_value_qualifiers;
create view sample.tbl_value_type_items as select * from public.tbl_value_type_items;
create view sample.tbl_value_types as select * from public.tbl_value_types;
create view sample.tbl_years_types as select * from public.tbl_years_types;

/* Subsetted tables */

create view sample.tbl_analysis_entities as
    select ae.*
    from public.tbl_analysis_entities ae
    join sample.tbl_physical_samples ps using (physical_sample_id);
    

create view sample.tbl_sample_groups as
    select sg.*
    from public.tbl_sample_groups sg
    join sample.tbl_physical_samples ps using (sample_group_id);

create view sample.tbl_datasets as
    select ds.*
    from public.tbl_datasets ds
    join sample.tbl_analysis_entities ae using (dataset_id);

create view sample.tbl_sites as
    select s.*
    from public.tbl_sites s
    join sample.tbl_sample_groups sg using (site_id);

create view sample.tbl_abundances as
    select a.*
    from public.tbl_abundances a
    join sample.tbl_analysis_entities ae using (analysis_entity_id);

create view sample.tbl_ceramics as
    select c.*
    from public.tbl_ceramics c
    join sample.tbl_analysis_entities ae using (analysis_entity_id);

create view sample.tbl_analysis_values as
    select *
    from public.tbl_analysis_values av
    join sample.tbl_analysis_entities ae using (analysis_entity_id);

create view sample.tbl_analysis_boolean_values as
    select x.*
    from public.tbl_analysis_boolean_values x
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_categorical_values as
    select x.* from public.tbl_analysis_categorical_values x
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_dating_ranges as 
    select x.* 
    from public.tbl_analysis_dating_ranges x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_entity_ages as 
    select x.* 
    from public.tbl_analysis_entity_ages x 
    join sample.tbl_analysis_entities y using (analysis_entity_id);

create view sample.tbl_analysis_entity_dimensions as 
    select x.* 
    from public.tbl_analysis_entity_dimensions x 
    join sample.tbl_analysis_entities y using (analysis_entity_id);

create view sample.tbl_analysis_entity_prep_methods as 
    select x.* 
    from public.tbl_analysis_entity_prep_methods x 
    join sample.tbl_analysis_entities y using (analysis_entity_id);

create view sample.tbl_analysis_identifiers as 
    select x.* 
    from public.tbl_analysis_identifiers x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_integer_ranges as 
    select x.* 
    from public.tbl_analysis_integer_ranges x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_integer_values as 
    select x.* 
    from public.tbl_analysis_integer_values x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_notes as 
    select x.* 
    from public.tbl_analysis_notes x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_numerical_ranges as 
    select x.* 
    from public.tbl_analysis_numerical_ranges x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_numerical_values as 
    select x.* 
    from public.tbl_analysis_numerical_values x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_taxon_counts as 
    select x.* 
    from public.tbl_analysis_taxon_counts x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_analysis_value_dimensions as 
    select x.* 
    from public.tbl_analysis_value_dimensions x 
    join sample.tbl_analysis_values y using (analysis_value_id);

create view sample.tbl_dendro as
    select d.*
    from public.tbl_dendro d
    join sample.tbl_analysis_entities ae using (analysis_entity_id);

create view sample.tbl_dendro_dates as
    select dd.*
    from public.tbl_dendro_dates dd
    join sample.tbl_analysis_entities ae using (analysis_entity_id);

create view sample.tbl_geochronology as
    select g.*
    from public.tbl_geochronology g
    join sample.tbl_analysis_entities ae using (analysis_entity_id);

create view sample.tbl_measured_values as
    select x.*
    from public.tbl_measured_values x
    join sample.tbl_analysis_entities y using (analysis_entity_id);

create view sample.tbl_relative_dates as
    select rd.*
    from public.tbl_relative_dates rd
    join sample.tbl_analysis_entities ae using (analysis_entity_id);

create view sample.tbl_sample_group_coordinates as
    select x.*
    from public.tbl_sample_group_coordinates x
    join sample.tbl_sample_groups y using (sample_group_id);

create view sample.tbl_sample_group_descriptions as
    select x.*
    from public.tbl_sample_group_descriptions x
    join sample.tbl_sample_groups y using (sample_group_id);

create view sample.tbl_sample_group_dimensions as
    select x.*
    from public.tbl_sample_group_dimensions x
    join sample.tbl_sample_groups y using (sample_group_id);

create view sample.tbl_sample_group_notes as
    select x.*
    from public.tbl_sample_group_notes x
    join sample.tbl_sample_groups y using (sample_group_id);

create view sample.tbl_sample_group_references as
    select x.*
    from public.tbl_sample_group_references x
    join sample.tbl_sample_groups y using (sample_group_id);

create view sample.tbl_sample_horizons as
    select x.*
    from public.tbl_sample_horizons x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_sample_locations as
    select x.*
    from public.tbl_sample_locations x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_sample_alt_refs as
    select x.*
    from public.tbl_sample_alt_refs x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_sample_coordinates as
    select x.*
    from public.tbl_sample_coordinates x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_sample_descriptions as
    select x.*
    from public.tbl_sample_descriptions x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_sample_dimensions as
    select x.*
    from public.tbl_sample_dimensions x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_sample_notes as
    select x.*
    from public.tbl_sample_notes x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_physical_sample_features as
    select x.*
    from public.tbl_physical_sample_features x
    join sample.tbl_physical_samples y using (physical_sample_id);

create view sample.tbl_features as
    select x.*
    from public.tbl_features x
    join sample.tbl_physical_sample_features y using (feature_id);

create view sample.tbl_site_locations as
    select x.*
    from public.tbl_site_locations x
    join sample.tbl_sites s using (site_id);

create view sample.tbl_site_references as
    select x.*
    from public.tbl_site_references x
    join sample.tbl_sites s using (site_id);

create view sample.tbl_projects as
    select x.*
    from public.tbl_projects x
    join sample.tbl_datasets y using (project_id);


create view sample.tbl_dataset_contacts as  
    select x.*
    from public.tbl_dataset_contacts x
    join sample.tbl_datasets y using (dataset_id);

create view sample.tbl_dataset_submissions as
    select x.*
    from public.tbl_dataset_submissions x
    join sample.tbl_datasets y using (dataset_id);

create view sample.tbl_contacts as
    select *
    from public.tbl_contacts
    where  contact_id in (
        select contact_id from sample.tbl_chronologies union all
        select contact_id from sample.tbl_dataset_contacts union all
        select contact_id from sample.tbl_dataset_masters union all
        select contact_id from sample.tbl_dataset_submissions union all
        select contact_id from sample.tbl_dating_labs union all
        select contact_id from sample.tbl_site_images union all
        select contact_id from sample.tbl_site_preservation_status union all
        select contact_id from sample.tbl_taxa_reference_specimens
    );

create view sample.tbl_locations (location_id, location_name, location_type_id, default_lat_dd, default_long_dd, date_updated) as
	select *
	from public.tbl_locations
	where location_id in (
		select location_id from sample.tbl_sample_locations
		union all
		select location_id from sample.tbl_sample_group_coordinates
		union all
		select location_id from sample.tbl_site_locations
		union all
		select location_id from sample.tbl_contacts
		union all
		select location_id from sample.tbl_rdb_systems
		union all
		select location_id from sample.tbl_relative_ages
 );

create view sample.tbl_rdb as
    select x.*
    from public.tbl_rdb x
    join sample.tbl_locations y using (location_id);

create view sample.tbl_taxa_tree_master as
    select *
    from public.tbl_taxa_tree_master
    where taxon_id in (
        select taxon_id from sample.tbl_abundances
        union all
        select taxon_id from sample.tbl_analysis_taxon_counts
        union all
        select taxon_id from sample.tbl_dating_material
        union all
        select taxon_id from sample.tbl_ecocodes
        union all
        select taxon_id from sample.tbl_rdb
    );

create view sample.tbl_taxa_seasonality as
    select x.*
    from public.tbl_taxa_seasonality x
    join sample.tbl_taxa_tree_master t using (taxon_id)
    join sample.tbl_locations s using (location_id);

create view sample.tbl_species_associations as
    select x.*
    from public.tbl_species_associations x
    join sample.tbl_taxa_tree_master t1 on x.associated_taxon_id = t1.taxon_id
    join sample.tbl_taxa_tree_master t2 on x.taxon_id = t1.taxon_id;
	
create view sample.tbl_relative_age_refs as
    select x.*
    from public.tbl_relative_age_refs x
    join sample.tbl_relative_ages y using (relative_age_id);

create view sample.tbl_taxa_common_names as
    select x.*
    from public.tbl_taxa_common_names x
    join sample.tbl_taxa_tree_master t using (taxon_id);

create view sample.tbl_taxa_measured_attributes as
    select x.*
    from public.tbl_taxa_measured_attributes x
    join sample.tbl_taxa_tree_master t using (taxon_id);

create view sample.tbl_taxonomic_order as
    select x.*
    from public.tbl_taxonomic_order x
    join sample.tbl_taxa_tree_master t using (taxon_id);

create view sample.tbl_taxa_tree_families as
    select x.*
    from public.tbl_taxa_tree_families x
    join sample.tbl_taxonomic_order o on o.taxonomic_order_id = x.order_id;

create view sample.tbl_taxa_tree_genera as
    select x.*
    from public.tbl_taxa_tree_genera x
    join sample.tbl_taxa_tree_families f using (family_id);
    
create view sample.tbl_taxa_tree_authors as
    select x.*
    from public.tbl_taxa_tree_authors x
    join sample.tbl_taxa_tree_master a using (author_id);

create view sample.tbl_taxonomy_notes as
    select x.*
    from public.tbl_taxonomy_notes x
    join sample.tbl_taxa_tree_master t using (taxon_id);

create view sample.tbl_text_biology as
    select x.*
    from public.tbl_text_biology x
    join sample.tbl_taxa_tree_master t using (taxon_id);
	
create view sample.tbl_site_other_records as
    select x.*
    from public.tbl_site_other_records x
    join sample.tbl_sites s using (site_id);
;
create view sample.tbl_text_distribution as
    select x.*
    from public.tbl_text_distribution x
    join sample.tbl_taxa_tree_master t using (taxon_id);

create view sample.tbl_text_identification_keys as
    select x.*
    from public.tbl_text_identification_keys x
    join sample.tbl_taxa_tree_master t using (taxon_id);

create view sample.tbl_abundance_ident_levels as
    select x.*
    from public.tbl_abundance_ident_levels x
    join sample.tbl_abundances b using (abundance_id);

create view sample.tbl_abundance_modifications as
    select x.*
    from public.tbl_abundance_modifications x
    join sample.tbl_abundances b using (abundance_id);

create view sample.tbl_dendro_date_notes as
    select x.*
    from public.tbl_dendro_date_notes x
    join sample.tbl_dendro_dates y using (dendro_date_id);

create view sample.tbl_horizons as
    select x.*
    from public.tbl_horizons x
    join sample.tbl_sample_horizons y using (horizon_id)
    join sample.tbl_physical_samples z using (physical_sample_id);

create view sample.tbl_biblio as
    select *
    from public.tbl_biblio
    where biblio_id in (
        select biblio_id from sample.tbl_aggregate_datasets union all
        select biblio_id from sample.tbl_dataset_masters union all
        select biblio_id from sample.tbl_datasets union all
        select biblio_id from sample.tbl_ecocode_systems union all
        select biblio_id from sample.tbl_geochron_refs union all
        select biblio_id from sample.tbl_methods union all
        select biblio_id from sample.tbl_rdb_systems union all
        select biblio_id from sample.tbl_relative_age_refs union all
        select biblio_id from sample.tbl_sample_group_references union all
        select biblio_id from sample.tbl_site_other_records union all
        select biblio_id from sample.tbl_site_references union all
        select biblio_id from sample.tbl_species_associations union all
        select biblio_id from sample.tbl_taxa_synonyms union all
        select biblio_id from sample.tbl_taxonomic_order_biblio union all
        select biblio_id from sample.tbl_taxonomy_notes union all
        select biblio_id from sample.tbl_tephra_refs union all
        select biblio_id from sample.tbl_text_biology union all
        select biblio_id from sample.tbl_text_distribution union all
        select biblio_id from sample.tbl_text_identification_keys
    )   ;

