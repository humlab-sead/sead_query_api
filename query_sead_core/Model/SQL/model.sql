

DROP TABLE IF EXISTS facet.facet_condition_clause;
DROP TABLE IF EXISTS facet.facet_table;
DROP TABLE IF EXISTS facet.facet;
DROP TABLE IF EXISTS facet.facet_group;
DROP TABLE IF EXISTS facet.facet_type;

DROP SCHEMA facet;

CREATE SCHEMA facet
       AUTHORIZATION seadworker;

GRANT USAGE ON SCHEMA facet TO public;

CREATE TABLE facet.facet
  (
    facet_id             INTEGER NOT NULL ,
    facet_key            VARCHAR (80) NOT NULL ,
    display_title        VARCHAR (80) NOT NULL ,
    facet_group_id       INTEGER NOT NULL ,
    facet_type_id        INTEGER NOT NULL ,
    category_id_expr     VARCHAR (256) NOT NULL ,
    category_name_expr   VARCHAR (256) NOT NULL ,
    icon_id_expr         VARCHAR (256) NOT NULL ,
    sort_expr            VARCHAR (256) NOT NULL,
    is_applicable        SMALLINT NOT NULL,
    is_default           SMALLINT NOT NULL,
    aggregate_type       VARCHAR (256) NOT NULL,
    aggregate_title      VARCHAR (256) NOT NULL,
    aggregate_facet_id   INTEGER NOT NULL
  );

ALTER TABLE facet.facet ADD CONSTRAINT facet_PK PRIMARY KEY ( facet_id ) ;

-- CREATE TABLE facet.facet_aggregate
--   (
--     facet_aggregate_id SERIAL NOT NULL ,
--     facet_id           INTEGER NOT NULL ,
--     aggregate_type     VARCHAR (20) NOT NULL ,
--     aggregate_title    VARCHAR (80) NOT NULL
--   );
  
-- ALTER TABLE facet.facet_aggregate ADD CONSTRAINT facet_aggregate_PK PRIMARY KEY ( facet_aggregate_id ) ;

CREATE TABLE facet.facet_condition_clause
  (
    facet_source_table_id serial NOT NULL ,
    facet_id              INTEGER NOT NULL ,
    clause                VARCHAR (512)
  );

ALTER TABLE facet.facet_condition_clause ADD CONSTRAINT facet_condition_clause_PK PRIMARY KEY ( facet_source_table_id ) ;

CREATE TABLE facet.facet_group
  (
    facet_group_id   INTEGER NOT NULL ,
    facet_group_key  VARCHAR (80) NOT NULL ,
    display_title    VARCHAR (80) NOT NULL ,
    is_applicable    SMALLINT NOT NULL ,
    is_default       SMALLINT NOT NULL
  );

ALTER TABLE facet.facet_group ADD CONSTRAINT facet_group_PK PRIMARY KEY ( facet_group_id ) ;


-- After Create script example for table facet.facet_group
-- CREATE TABLE facet.facet_column
--   (
--     facet_column_id SERIAL NOT NULL ,
--     field_type_id         INTEGER NOT NULL ,
--     facet_id              INTEGER NOT NULL ,
--     field_expr            VARCHAR (256) NOT NULL
--   );

-- ALTER TABLE facet.facet_column ADD CONSTRAINT facet_column_PK PRIMARY KEY ( facet_column_id ) ;

CREATE TABLE facet.facet_table
  (
    facet_table_id SERIAL NOT NULL ,
    facet_id              INTEGER NOT NULL ,
    sequence_id           INTEGER NOT NULL ,
    schema_name           VARCHAR (80) ,
    table_name            VARCHAR (80) ,
    alias                 VARCHAR (80)
  );

ALTER TABLE facet.facet_table ADD CONSTRAINT facet_table_PK PRIMARY KEY ( facet_table_id ) ;


CREATE TABLE facet.facet_type
  (
    facet_type_id		INTEGER NOT NULL ,
    facet_type_name		VARCHAR (80) NOT NULL,
	reload_as_target	BOOLEAN NOT NULL default(FALSE)
  );

-- ALTER TABLE facet.facet_type ADD COLUMN reload_as_trigger BOOLEAN NOT NULL DEFAULT(FALSE)
-- UPDATE facet.facet_type SET reload_as_trigger = TRUE WHERE facet_type_id in (2, 3);

ALTER TABLE facet.facet_type ADD CONSTRAINT facet_type_PK PRIMARY KEY ( facet_type_id ) ;


-- ALTER TABLE facet_aggregate
--     ADD CONSTRAINT facet_aggregate_facet_FK FOREIGN KEY ( facet_id ) REFERENCES facet ( facet_id )
--         ON DELETE NO ACTION ;

ALTER TABLE facet.facet_condition_clause
    ADD CONSTRAINT facet_condition_clause_facet_FK FOREIGN KEY ( facet_id ) REFERENCES facet.facet (facet_id )
        ON DELETE NO ACTION ;

ALTER TABLE facet.facet
    ADD CONSTRAINT facet_facet_group_FK FOREIGN KEY ( facet_group_id ) REFERENCES facet.facet_group ( facet_group_id )
        ON DELETE NO ACTION ;

ALTER TABLE facet.facet
    ADD CONSTRAINT facet_facet_type_FK FOREIGN KEY ( facet_type_id ) REFERENCES facet.facet_type ( facet_type_id )
        ON DELETE NO ACTION ;

-- ALTER TABLE facet.facet_column
--     ADD CONSTRAINT facet_column_facet_FK FOREIGN KEY ( facet_id ) REFERENCES facet.facet ( facet_id )
--         ON DELETE NO ACTION ;

ALTER TABLE facet.facet_table
    ADD CONSTRAINT facet_table_facet_FK FOREIGN KEY ( facet_id ) REFERENCES facet.facet ( facet_id )
        ON DELETE NO ACTION ;

INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (0, 'undefined', FALSE);
INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (1, 'discrete', FALSE);
INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (2, 'range', TRUE);
INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (3, 'geo', TRUE);

INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES(0, 'ROOT', 'ROOT', 0, 0);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES(1, 'others', 'Others', 1, 0);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES(2, 'space_time', 'Space/Time', 1, 0);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES(3, 'time', 'Time', 1, 0);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES(4, 'ecology', 'Ecology', 1, 0);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES(5, 'measured_values', 'Measured values', 1, 0);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES(6, 'taxonomy', 'Taxonomy', 1, 0);

INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (1,'result_facet', 'Analysis entities', 0, 1, 'tbl_analysis_entities.analysis_entity_id', 'tbl_physical_samples.sample_name||'' ''||tbl_datasets.dataset_name', 'tbl_analysis_entities.analysis_entity_id', 'tbl_datasets.dataset_name', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (2,'dataset_helper', 'dataset_helper', 0, 1, 'tbl_datasets.dataset_id', 'tbl_datasets.dataset_id', 'tbl_dataset.dataset_id', 'tbl_dataset.dataset_id', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (3,'tbl_denormalized_measured_values_33_0', 'MS ', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_33_0', 'metainformation.tbl_denormalized_measured_values.value_33_0', 'metainformation.tbl_denormalized_measured_values.value_33_0', 'metainformation.tbl_denormalized_measured_values.value_33_0', 0, 1, '', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (4,'tbl_denormalized_measured_values_33_82', 'MS Heating 550', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_33_82', 'metainformation.tbl_denormalized_measured_values.value_33_82', 'metainformation.tbl_denormalized_measured_values.value_33_82', 'metainformation.tbl_denormalized_measured_values.value_33_82', 0, 1, '', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (5,'tbl_denormalized_measured_values_32', 'LOI', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_32_0', 'metainformation.tbl_denormalized_measured_values.value_32_0', 'metainformation.tbl_denormalized_measured_values.value_32', 'metainformation.tbl_denormalized_measured_values.value_32', 0, 1, '', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (6,'tbl_denormalized_measured_values_37', ' P┬░', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_37_0', 'metainformation.tbl_denormalized_measured_values.value_37_0', 'metainformation.tbl_denormalized_measured_values.value_37', 'metainformation.tbl_denormalized_measured_values.value_37', 0, 1, '', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (7,'measured_values_helper', 'values', 0, 1, 'tbl_measured_values.measured_value', 'tbl_measured_values.measured_value', 'tbl_measured_values.measured_value', 'tbl_measured_values.measured_value', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (8,'taxon_result', 'taxon_id', 0, 1, 'tbl_abundances.taxon_id', 'tbl_abundances.taxon_id', 'tbl_abundances.taxon_id', 'tbl_abundances.taxon_id', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (9,'map_result', 'Site', 0, 1, 'tbl_sites.site_id', 'tbl_sites.site_name', 'tbl_sites.site_id', 'tbl_sites.site_name', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (10,'geochronology', 'Geochronology', 3, 2, 'tbl_geochronology.age', 'tbl_geochronology.age', 'tbl_geochronology.age', 'tbl_geochronology.age', 0, 1, '', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (11,'relative_age_name', 'Time periods', 3, 1, 'tbl_relative_ages.relative_age_id', 'tbl_relative_ages.relative_age_name', 'tbl_relative_ages.relative_age_id', 'tbl_relative_ages.relative_age_name', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (12,'record_types', 'Proxy types', 1, 1, 'tbl_record_types.record_type_id', 'tbl_record_types.record_type_name', 'tbl_record_types.record_type_id', 'tbl_record_types.record_type_name', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (13,'sample_groups', 'Sample group', 2, 1, 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', 1, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (14,'places', 'Places', 2, 1, 'tbl_locations.location_id', 'tbl_locations.location_name', 'tbl_locations.location_id', 'tbl_locations.location_name', 1, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (15,'places_all2', 'view_places_relations', 2, 1, 'tbl_locations.location_id', 'tbl_locations.location_name', 'view_places_relations.rel_id', 'tbl_locations.location_name', 1, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (16,'sample_groups_helper', 'Sample group', 2, 1, 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', 1, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (17,'physical_samples', 'physical samples', 2, 1, 'tbl_physical_samples.physical_sample_id', 'tbl_physical_samples.sample_name', 'tbl_physical_samples.physical_sample_id', 'tbl_physical_samples.sample_name', 1, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (18,'sites', 'Site', 2, 1, 'tbl_sites.site_id', 'tbl_sites.site_name', 'tbl_sites.site_id', 'tbl_sites.site_name', 1, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (19,'sites_helper', 'Site', 2, 1, 'tbl_sites.site_id', 'tbl_sites.site_name', 'tbl_sites.site_id', 'tbl_sites.site_name', 1, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (20,'tbl_relative_dates_helper', 'tbl_relative_dates', 3, 1, 'tbl_relative_dates.relative_age_id', 'tbl_relative_dates.relative_age_name ', 'tbl_relative_dates.relative_age_name', 'tbl_relative_dates.relative_age_name ', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (21,'country', 'Country', 2, 1, 'countries.location_id', 'countries.location_name ', 'countries.location_id', 'countries.location_name', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (22,'ecocode', 'Eco code', 4, 1, 'tbl_ecocode_definitions.ecocode_definition_id', 'tbl_ecocode_definitions.label', 'tbl_ecocode_definitions.ecocode_definition_id', 'tbl_ecocode_definitions.label', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (23,'family', 'Family', 6, 1, 'tbl_taxa_tree_families.family_id', 'tbl_taxa_tree_families.family_name ', 'tbl_taxa_tree_families.family_id', 'tbl_taxa_tree_families.family_name ', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (24,'genus', 'Genus', 6, 1, 'tbl_taxa_tree_genera.genus_id', 'tbl_taxa_tree_genera.genus_name', 'tbl_taxa_tree_genera.genus_id', 'tbl_taxa_tree_genera.genus_name', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (25,'species', 'Taxa', 6, 1, 'tbl_taxa_tree_master.taxon_id', 'COALESCE(tbl_taxa_tree_genera.genus_name,'''')||'' ''||COALESCE(tbl_taxa_tree_master.species,'''')||'' ''||COALESCE('' ''||tbl_taxa_tree_authors.author_name||'' '','''')', 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_genera.genus_name||'' ''||tbl_taxa_tree_master.species', 0, 1, 'sum', 'sum of Abundance', 27);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (26,'species_helper', 'Species', 6, 1, 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_master.species', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (27,'abundance_helper', 'abundance_id', 6, 1, 'tbl_abundances.abundance_id', 'tbl_abundances.abundance_id', 'tbl_abundances.abundance_id', 'tbl_abundances.abundance_id', 0, 0, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (28,'species_author', 'Author', 6, 1, 'tbl_taxa_tree_authors.author_id ', 'tbl_taxa_tree_authors.author_name ', 'tbl_taxa_tree_authors.author_id ', 'tbl_taxa_tree_authors.author_name ', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (29,'feature_type', 'Feature type', 1, 1, 'tbl_feature_types.feature_type_id ', 'tbl_feature_types.feature_type_name', 'tbl_feature_types.feature_id ', 'tbl_feature_types.feature_type_name', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (30,'ecocode_system', 'Eco code system', 4, 1, 'tbl_ecocode_systems.ecocode_system_id ', 'tbl_ecocode_systems.name', 'tbl_ecocode_systems.ecocode_system_id ', 'tbl_ecocode_systems.definition', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (31,'abundance_classification', 'abundance classification', 4, 1, 'metainformation.view_abundance.elements_part_mod ', 'metainformation.view_abundance.elements_part_mod ', 'metainformation.view_abundance.elements_part_mod ', 'metainformation.view_abundance.elements_part_mod ', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (32,'abundances_all_helper', 'Abundances', 4, 2, 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance', 0, 0, '', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (33,'abundances_all', 'Abundances', 4, 2, 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance', 0, 1, '', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (34,'activeseason', 'Seasons', 3, 1, 'tbl_seasons.season_id', 'tbl_seasons.season_name ', 'tbl_seasons.season_id', 'tbl_seasons.season_type ', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (35,'tbl_biblio_modern', 'Bibligraphy modern', 1, 1, 'metainformation.view_taxa_biblio.biblio_id', 'tbl_biblio.title||''  ''||tbl_biblio.author ', 'tbl_biblio.biblio_id', 'tbl_biblio.author', 0, 1, 'count', 'count of species', 19);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (36,'tbl_biblio_sample_groups', 'Bibligraphy sites/Samplegroups', 1, 1, 'tbl_biblio.biblio_id', 'tbl_biblio.title||''  ''||tbl_biblio.author', 'tbl_biblio.biblio_id', 'tbl_biblio.author', 0, 1, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (37,'tbl_biblio_sites', 'Bibligraphy sites', 1, 1, 'tbl_biblio.biblio_id', 'tbl_biblio.title||''  ''||tbl_biblio.author', 'tbl_biblio.biblio_id', 'tbl_biblio.author', 0, 0, 'count', 'Number of samples', 0);

UPDATE facet.facet SET aggregate_facet_id = 1 WHERE aggregate_facet_id = 0 AND facet_id <> 1;

INSERT INTO facet.facet_condition_clause (facet_id, clause) VALUES (21,'countries.location_type_id=1');
INSERT INTO facet.facet_condition_clause (facet_id, clause) VALUES (25,'tbl_sites.site_id is not null');
INSERT INTO facet.facet_condition_clause (facet_id, clause) VALUES (32,'metainformation.view_abundance.abundance is not null');
INSERT INTO facet.facet_condition_clause (facet_id, clause) VALUES (33,'metainformation.view_abundance.abundance is not null');
INSERT INTO facet.facet_condition_clause (facet_id, clause) VALUES (36,'metainformation.view_sample_group_references.biblio_id is not null');
INSERT INTO facet.facet_condition_clause (facet_id, clause) VALUES (37,'metainformation.view_site_references.biblio_id is not null');

INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (1,1, '', 'tbl_analysis_entities', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (2,1, '', 'tbl_datasets', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (3,1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (4,1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (5,1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (6,1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (7,1, '', 'tbl_measured_values', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (8,1, '', 'tbl_abundances', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (9,1, '', 'tbl_sites', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (10,1, '', 'tbl_geochronology', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (11,1, '', 'tbl_relative_ages', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (12,1, '', 'tbl_record_types', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (13,1, '', 'tbl_sample_groups', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (14,1, '', 'tbl_locations', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (15,1, '', 'view_places_relations', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (16,1, '', 'tbl_sample_groups', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (17,1, '', 'tbl_physical_samples', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (18,1, '', 'tbl_sites', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (19,1, '', 'tbl_sites', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (20,1, '', 'tbl_relative_dates', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (21,1, '', 'tbl_locations', 'countries');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (22,1, '', 'tbl_ecocode_definitions', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (23,1, '', 'tbl_taxa_tree_families', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (24,1, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (25,1, '', 'tbl_taxa_tree_master', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (26,1, '', 'tbl_taxa_tree_master', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (27,1, '', 'tbl_abundances', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (28,1, '', 'tbl_taxa_tree_authors', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (29,1, '', 'tbl_feature_types', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (30,1, '', 'tbl_ecocode_systems', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (31,1, '', 'metainformation.view_abundance', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (32,1, '', 'metainformation.view_abundance', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (33,1, '', 'metainformation.view_abundance', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (34,1, '', 'tbl_seasons', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (35,1, '', 'metainformation.view_taxa_biblio', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (36,1, '', 'tbl_biblio', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (37,1, '', 'tbl_biblio', '');


INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (1,2, '', 'tbl_physical_samples', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (14,2, '', 'tbl_site_locations', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (15,2, '', 'tbl_locations', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (21,2, '', 'tbl_site_locations', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (22,2, '', 'tbl_ecocode_definitions', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (23,2, '', 'tbl_taxa_tree_families', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (24,2, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (25,2, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (26,2, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (28,2, '', 'tbl_taxa_tree_authors', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (29,2, '', 'tbl_physical_sample_features', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (30,2, '', 'tbl_ecocode_systems', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (35,2, '', 'tbl_biblio', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (36,2, '', 'metainformation.view_sample_group_references', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (37,2, '', 'metainformation.view_site_references', '');

INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (1,3, '', 'tbl_datasets', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (15,3, '', 'tbl_site_locations', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (25,3, '', 'tbl_taxa_tree_authors', '');
INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (26,3, '', 'tbl_taxa_tree_authors', '');

INSERT INTO facet.facet_table (facet_id, sequence_id, schema_name, table_name, alias) VALUES (25,4, '', 'tbl_sites', '');


GRANT USAGE ON SCHEMA facet TO seadread;
GRANT SELECT ON TABLE facet.facet TO seadread;
GRANT SELECT ON TABLE facet.facet_condition_clause TO seadread;
GRANT SELECT ON TABLE facet.facet_table TO seadread;
GRANT SELECT ON TABLE facet.facet_group TO seadread;
GRANT SELECT ON TABLE facet.facet_type TO seadread;


Drop Table facet.graph_table;
Drop Table facet.graph_table_relation;

Create Table facet.graph_table
(
	table_id serial not null,
	table_name information_schema.sql_identifier NOT NULL
);

Create Table facet.graph_table_relation
(
	relation_id serial not null,
	source_table_id integer not null,
	target_table_id integer not null,
	weight integer not null default(0),
	
	source_column_name information_schema.sql_identifier NOT NULL,
	target_column_name information_schema.sql_identifier NOT NULL
);

Alter Table facet.graph_table ADD CONSTRAINT graph_table_PK PRIMARY KEY ( table_id ) ;
Alter Table facet.graph_table_relation ADD CONSTRAINT graph_table_relation_PK PRIMARY KEY ( relation_id ) ;

ALTER TABLE facet.graph_table_relation
    ADD CONSTRAINT graph_table_relation_FK1 FOREIGN KEY ( source_table_id ) REFERENCES facet.graph_table ( table_id )
        ON DELETE NO ACTION ;
        
ALTER TABLE facet.graph_table_relation
    ADD CONSTRAINT graph_table_relation_FK2 FOREIGN KEY ( target_table_id ) REFERENCES facet.graph_table ( table_id )
        ON DELETE NO ACTION ;

Insert Into facet.graph_table (table_name)
	select distinct table_name
	from (
		select source_table as table_name
		from metainformation.tbl_foreign_relations
		union
		select target_table as table_name
		from metainformation.tbl_foreign_relations
	) as X;

Insert Into facet.graph_table_relation (source_table_id, target_table_id, weight, source_column_name, target_column_name)
	Select s.table_id, t.table_id, r.weight, r.source_column, r.target_column
	From metainformation.tbl_foreign_relations r
	Join facet.graph_table s
	  On s.table_name = r.source_table
	Join facet.graph_table t
	  On t.table_name = r.target_table
 
ALTER TABLE facet.graph_table OWNER TO seadworker;
ALTER TABLE facet.graph_table_relation OWNER TO seadworker;

GRANT ALL ON TABLE facet.graph_table TO seadworker;
GRANT SELECT ON TABLE facet.graph_table TO readers;
GRANT SELECT ON TABLE facet.graph_table TO seadread;
GRANT ALL ON TABLE facet.graph_table_relation TO seadworker;
GRANT SELECT ON TABLE facet.graph_table_relation TO readers;
GRANT SELECT ON TABLE facet.graph_table_relation TO seadread;

Create Index idx_graph_table_relation_fk1 on facet.graph_table_relation (source_table_id);
Create Index idx_graph_table_relation_fk2 on facet.graph_table_relation (target_table_id);

GRANT ALL ON TABLE facet.graph_node TO seadworker;
GRANT SELECT ON TABLE facet.graph_node TO readers;
GRANT SELECT ON TABLE facet.graph_node TO seadread;
GRANT ALL ON TABLE facet.graph_edge TO seadworker;
GRANT SELECT ON TABLE facet.graph_edge TO readers;
GRANT SELECT ON TABLE facet.graph_edge TO seadread;

Create Index idx_graph_edge_fk1 on facet.graph_edge (source_node_id);
Create Index idx_graph_edge_fk2 on facet.graph_edge (target_node_id);



--drop table facet.tbl_result_fields;
create table facet.tbl_result_fields (
	result_field_key varchar(256),
	table_name varchar(256),
	column_name varchar(256),
	display_text varchar(256),
	result_type varchar(256),
	activated varchar(256),
	parents varchar(256),
	link_url varchar(256),
	link_label varchar(256)
);
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('sitename', 'tbl_sites', 'tbl_sites.site_name', 'Site name', 'single_item', '1', 'Array', '', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('record_type', 'tbl_record_types', 'tbl_record_types.record_type_name', 'Record type(s)', 'text_agg_item', '1', 'Array', '', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('analysis_entities', 'tbl_analysis_entities', 'tbl_analysis_entities.analysis_entity_id', 'Filtered records', 'single_item', '1', 'Array', '', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('site_link', 'tbl_sites', 'tbl_sites.site_id', 'Full report', 'link_item', '1', 'Array', 'api/report/show_site_details.php?site_id', 'Show site report');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('site_link_filtered', 'tbl_sites', 'tbl_sites.site_id', 'Filtered report', 'link_item', '1', 'Array', 'api/report/show_site_details.php?site_id', 'Show filtered report');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('aggregate_all_filtered', 'tbl_aggregate_samples', '''Aggregated''::text', 'Filtered report', 'link_item_filtered', '1', 'Array', 'api/report/show_details_all_levels.php?level', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('sample_group_link', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Full report', 'link_item', '1', 'Array', 'api/report/show_sample_group_details.php?sample_group_id', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('sample_group_link_filtered', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Filtered report', 'link_item', '1', 'Array', 'api/report/show_sample_group_details.php?sample_group_id', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('abundance', 'tbl_abundances', ' tbl_abundances.abundance', 'number of taxon_id', 'single_item', '1', 'Array', '', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('taxon_id', 'tbl_abundances', ' tbl_abundances.taxon_id', 'Taxon id  (specie)', 'single_item', '1', 'Array', '', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('dataset', 'tbl_datasets', 'tbl_datasets.dataset_name', 'Dataset', 'single_item', '1', 'Array', '', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('dataset_link', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Dataset details', 'single_item', '1', 'Array', 'client/show_dataset_details.php?dataset_id', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('dataset_link_filtered', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Filtered report', 'single_item', '1', 'Array', 'client/show_dataset_details.php?dataset_id', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('sample_group', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_name', 'Sample group', 'single_item', '1', 'Array', '', '');
INSERT INTO facet.tbl_result_fields (result_field_key,table_name,column_name,display_text,result_type,activated,parents,link_url,link_label) VALUES ('methods', 'tbl_methods', 'tbl_methods.method_name', 'Method', 'single_item', '1', 'Array', '', '');
select * from facet.tbl_result_fields

create table facet.result_field (
	result_field_id serial not null,
	result_field_key varchar(40) not null,
	table_name varchar(80) not null,
	column_name varchar(80) not null,
	display_text varchar(80) not null,
	result_type varchar(20) not null,
	activated smallint not null,
	link_url varchar(256) null,
	link_label varchar(256) null
)

insert into facet.result_field (result_field_key,table_name,column_name,display_text,result_type,activated,link_url,link_label)
   select trim(result_field_key),trim(table_name),trim(column_name),trim(display_text),trim(result_type),1,
			case when trim(link_url) = '' then null else trim(link_url) end,
			case when trim(link_label) = '' then null else trim(link_label) end
   from facet.tbl_result_fields
select * from facet.result_field 

alter table facet.result_field add  CONSTRAINT result_field_pk PRIMARY KEY (result_field_id);

select trim('    **** ')
drop table facet.result_definition_field;
drop table facet.result_definition;
drop table facet.result_type;

create table facet.result_definition (
	result_definition_id int not null,
	result_definition_key varchar(40) not null,
	display_text varchar(80) not null,
	is_applicable boolean not null default(FALSE),
	is_activated boolean not null default(TRUE),
	aggregation_type varchar(40) not null,
	input_type varchar(40) not null  default('checkboxes'),
	has_aggregation_selector boolean not null default(TRUE),
	CONSTRAINT result_definition_pk PRIMARY KEY (result_definition_id)
);

insert into facet.result_definition (result_definition_id, result_definition_key, display_text, aggregation_type)
	values (1, 'site_level','Site level','site_level');
insert into facet.result_definition (result_definition_id, result_definition_key, display_text, aggregation_type)
	values (2, 'aggregate_all','Aggregate all','aggregate_all');
insert into facet.result_definition (result_definition_id, result_definition_key, display_text, aggregation_type)
	values (3, 'sample_group_level','Sample group level','sample_group_level');

create table facet.result_type (
	result_type_id int not null,
	result_type varchar(40) not null,
	CONSTRAINT result_type_pk PRIMARY KEY (result_type_id)
);

insert into facet.result_type values (1, 'single_item'), (2, 'text_agg_item'), (3, 'count_item'), (4, 'link_item'), (5, 'sort_item'), (6, 'link_item_filtered');


create table facet.result_definition_field (
	result_definition_field_id serial not null,
	result_definition_id int not null,
	result_field_id int not null,
	result_type_id int not null,
	CONSTRAINT result_definition_field_pk PRIMARY KEY (result_definition_field_id),
	CONSTRAINT result_definition_fieldfk1 FOREIGN KEY (result_definition_id)
	  REFERENCES facet.result_definition (result_definition_id) 
	  ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT result_definition_fieldfk2 FOREIGN KEY (result_field_id)
	  REFERENCES facet.result_field (result_field_id) 
	  ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT result_definition_fieldfk3 FOREIGN KEY (result_type_id)
	  REFERENCES facet.result_type (result_type_id)
	  ON UPDATE NO ACTION ON DELETE NO ACTION
);

insert into facet.result_definition_field (result_definition_id, result_type_id, result_field_id)
	select x.result_definition_id, y.result_type_id, z.result_field_id
	from (
		values
		('site_level','single_item','sitename'),
		('site_level','text_agg_item','record_type'),
		('site_level','count_item','analysis_entities'),
		('site_level','link_item','site_link'),
		('site_level','sort_item','sitename'),
		('site_level','link_item_filtered','site_link_filtered'),
		('aggregate_all','link_item_filtered','aggregate_all_filtered'),
		('aggregate_all','count_item','analysis_entities'),
		('sample_group_level','single_item','sitename'),
		('sample_group_level','single_item','sample_group'),
		('sample_group_level','single_item','record_type'),
		('sample_group_level','sort_item','sitename'),
		('sample_group_level','sort_item','sample_group'),
		('sample_group_level','count_item','analysis_entities'),
		('sample_group_level','link_item','sample_group_link'),
		('sample_group_level','link_item_filtered','sample_group_link_filtered')
	) as d(result_definition, result_type, result_field)
	join facet.result_definition x
	  on x.result_definition_key = d.result_definition
	join facet.result_type y
	  on y.result_type = d.result_type
	join facet.result_field z
	  on z.result_field_key = d.result_field