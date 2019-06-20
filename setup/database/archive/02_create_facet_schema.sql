

CREATE USER querysead_worker WITH LOGIN NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;
ALTER USER querysead_worker WITH ENCRYPTED PASSWORD 'XXX';

GRANT CONNECT ON DATABASE sead_bugs_import_20180503 TO querysead_worker;
GRANT USAGE ON SCHEMA public, metainformation TO querysead_worker;
GRANT sead_read TO querysead_worker;

GRANT SELECT ON ALL TABLES IN SCHEMA  public, metainformation TO querysead_worker;
GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA  public, metainformation to querysead_worker;
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public, metainformation TO querysead_worker;

ALTER DEFAULT PRIVILEGES IN SCHEMA public, metainformation GRANT SELECT, TRIGGER ON TABLES TO querysead_worker;

DO $$
BEGIN

	IF current_role != 'querysead_worker' THEN
		RAISE NOTICE 'This script must be run as querysead_worker!';
	END IF;

	DROP SCHEMA IF EXISTS facet CASCADE;

	CREATE SCHEMA IF NOT EXISTS facet AUTHORIZATION querysead_worker;

	GRANT USAGE ON SCHEMA facet TO public;

END $$ language plpgsql;

SET search_path = facet, pg_catalog;
SET default_with_oids = false;

/* Create Tables */

CREATE TABLE IF NOT EXISTS facet.facet (
    facet_id integer NOT NULL PRIMARY KEY,
    facet_key character varying(80) NOT NULL,
    display_title character varying(80) NOT NULL,
    facet_group_id integer NOT NULL,
    facet_type_id integer NOT NULL,
    category_id_expr character varying(256) NOT NULL,
    category_name_expr character varying(256) NOT NULL,
    icon_id_expr character varying(256) NOT NULL,
    sort_expr character varying(256) NOT NULL,
    is_applicable boolean NOT NULL,
    is_default boolean NOT NULL,
    aggregate_type character varying(256) NOT NULL,
    aggregate_title character varying(256) NOT NULL,
    aggregate_facet_id integer NOT NULL
);

CREATE TABLE IF NOT EXISTS facet.facet_condition_clause (
    facet_source_table_id serial PRIMARY KEY,
    facet_id integer not NULL,
    clause character varying(512)
);


CREATE TABLE IF NOT EXISTS facet.facet_group (
    facet_group_id integer NOT NULL PRIMARY KEY,
    facet_group_key character varying(80) NOT NULL,
    display_title character varying(80) NOT NULL,
    is_applicable boolean NOT NULL,
    is_default boolean NOT NULL
);

-- After Create script example for table facet.facet_group
-- CREATE TABLE facet.facet_column
--   (
--     facet_column_id SERIAL NOT NULL ,
--     field_type_id         INTEGER NOT NULL ,
--     facet_id              INTEGER NOT NULL ,
--     field_expr            VARCHAR (256) NOT NULL
--   );

-- ALTER TABLE facet.facet_column ADD CONSTRAINT facet_column_PK PRIMARY KEY ( facet_column_id ) ;

CREATE TABLE IF NOT EXISTS facet.facet_table (
    facet_table_id serial PRIMARY KEY,
    facet_id integer NOT NULL,
    sequence_id integer NOT NULL,
    schema_name character varying(80),
    table_name character varying(80),
    alias character varying(80)
);

CREATE TABLE IF NOT EXISTS facet.facet_type (
    facet_type_id integer NOT NULL PRIMARY KEY,
    facet_type_name character varying(80) NOT NULL,
    reload_as_target boolean DEFAULT false NOT NULL
);

CREATE TABLE IF NOT EXISTS facet.graph_table
(
	table_id serial PRIMARY KEY,
	table_name information_schema.sql_identifier NOT NULL
);

CREATE TABLE IF NOT EXISTS facet.graph_table_relation
(
	relation_id serial PRIMARY KEY,
	source_table_id integer not null,
	target_table_id integer not null,
	weight integer not null default(0),

	source_column_name information_schema.sql_identifier NOT NULL,
	target_column_name information_schema.sql_identifier NOT NULL
);

CREATE TABLE IF NOT EXISTS facet.result_aggregate (
    aggregate_id integer NOT NULL PRIMARY KEY,
    aggregate_key character varying(40) NOT NULL,
    display_text character varying(80) NOT NULL,
    is_applicable boolean DEFAULT false NOT NULL,
    is_activated boolean DEFAULT true NOT NULL,
    input_type character varying(40) DEFAULT 'checkboxes'::character varying NOT NULL,
    has_selector boolean DEFAULT true NOT NULL
);

CREATE TABLE IF NOT EXISTS facet.result_aggregate_field (
    aggregate_field_id serial PRIMARY KEY,
    aggregate_id integer NOT NULL,
    result_field_id integer NOT NULL,
    field_type_id character varying(40) DEFAULT 'single_item'::character varying NOT NULL,
    sequence_id integer DEFAULT 0 NOT NULL
);

CREATE TABLE IF NOT EXISTS facet.result_field (
    result_field_id serial PRIMARY KEY,
    result_field_key character varying(40) NOT NULL,
    table_name character varying(80),
    column_name character varying(80) NOT NULL,
    display_text character varying(80) NOT NULL,
    field_type_id character varying(20) NOT NULL,
    activated boolean NOT NULL,
    link_url character varying(256),
    link_label character varying(256)
);

CREATE TABLE IF NOT EXISTS facet.result_field_type (
    field_type_id character varying(40) PRIMARY KEY,
    is_result_value boolean DEFAULT true NOT NULL,
    sql_field_compiler character varying(40) DEFAULT ''::character varying NOT NULL,
    is_aggregate_field boolean DEFAULT false NOT NULL,
    is_sort_field boolean DEFAULT false NOT NULL,
    is_item_field boolean DEFAULT false NOT NULL,
    sql_template character varying(256) DEFAULT '{0}'::character varying NOT NULL
);

CREATE TABLE IF NOT EXISTS facet.result_view_type (
    view_type_id character varying(40) PRIMARY KEY,
    view_name character varying(40) NOT NULL,
    is_cachable boolean DEFAULT true NOT NULL
);

/* CREATE TABLE facet.tbl_result_fields (
    result_field_key character varying(256) PRIMARY KEY,
    table_name character varying(256),
    column_name character varying(256),
    display_text character varying(256),
    result_type character varying(256),
    activated character varying(256),
    parents character varying(256),
    link_url character varying(256),
    link_label character varying(256)
); */

CREATE TABLE IF NOT EXISTS facet.view_state (
    view_state_key character varying(80) NOT NULL PRIMARY KEY,
    view_state_data text NOT NULL,
    create_time timestamp with time zone DEFAULT clock_timestamp()
);

CREATE OR REPLACE VIEW facet.report_site AS
 SELECT tbl_sites.site_id AS id,
    tbl_sites.site_id,
    tbl_sites.site_name AS "Site name",
    tbl_sites.site_description AS "Site description",
    tbl_site_natgridrefs.natgridref AS "National grid ref",
    array_to_string(array_agg(tbl_locations.location_name ORDER BY tbl_locations.location_type_id DESC), ','::text) AS places,
    tbl_site_preservation_status.preservation_status_or_threat AS "Preservation status or threat",
    tbl_sites.latitude_dd AS site_lat,
    tbl_sites.longitude_dd AS site_lng
   FROM ((((public.tbl_sites
   LEFT JOIN public.tbl_site_locations ON ((tbl_site_locations.site_id = tbl_sites.site_id)))
   LEFT JOIN public.tbl_site_natgridrefs ON ((tbl_site_natgridrefs.site_id = tbl_sites.site_id)))
   LEFT JOIN public.tbl_site_preservation_status ON ((tbl_site_preservation_status.site_preservation_status_id = tbl_sites.site_preservation_status_id)))
   LEFT JOIN public.tbl_locations ON ((tbl_locations.location_id = tbl_site_locations.location_id)))
  GROUP BY tbl_sites.site_id, tbl_sites.site_name, tbl_sites.site_description, tbl_site_natgridrefs.natgridref, tbl_sites.latitude_dd, tbl_sites.longitude_dd, tbl_site_preservation_status.preservation_status_or_threat;

DO $$
BEGIN
	IF current_role != 'querysead_worker' THEN
		ALTER TABLE facet.facet OWNER TO querysead_worker;
		ALTER TABLE facet.facet_condition_clause OWNER TO querysead_worker;
		ALTER TABLE facet.facet_group OWNER TO querysead_worker;
		ALTER TABLE facet.facet_table OWNER TO querysead_worker;
		ALTER TABLE facet.facet_type OWNER TO querysead_worker;
		ALTER TABLE facet.graph_table OWNER TO querysead_worker;
		ALTER TABLE facet.graph_table_relation OWNER TO querysead_worker;
		ALTER TABLE facet.result_aggregate OWNER TO querysead_worker;
		ALTER TABLE facet.result_aggregate_field OWNER TO querysead_worker;
		ALTER TABLE facet.result_field OWNER TO querysead_worker;
		ALTER TABLE facet.result_field_type OWNER TO querysead_worker;
		ALTER TABLE facet.result_view_type OWNER TO querysead_worker;
		ALTER TABLE facet.report_site OWNER TO querysead_worker;
		ALTER TABLE facet.view_state OWNER TO querysead_worker;
	END IF;
END $$ language plpgsql;


INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (0, 'undefined', false);
INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (1, 'discrete', false);
INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (2, 'range', true);
INSERT INTO facet.facet_type (facet_type_id, facet_type_name, reload_as_target) VALUES (3, 'geo', true);

INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES (0, 'ROOT', 'ROOT', false, false);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES (1, 'others', 'Others', true, false);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES (2, 'space_time', 'Space/Time', true, false);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES (3, 'time', 'Time', true, false);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES (4, 'ecology', 'Ecology', true, false);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES (5, 'measured_values', 'Measured values', true, false);
INSERT INTO facet.facet_group (facet_group_id, facet_group_key, display_title, is_applicable, is_default) VALUES (6, 'taxonomy', 'Taxonomy', true, false);

INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (1, 'result_facet', 'Analysis entities', 0, 1, 'tbl_analysis_entities.analysis_entity_id', 'tbl_physical_samples.sample_name||'' ''||tbl_datasets.dataset_name', 'tbl_analysis_entities.analysis_entity_id', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of samples', 0);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (2, 'dataset_helper', 'dataset_helper', 0, 1, 'tbl_datasets.dataset_id', 'tbl_datasets.dataset_id', 'tbl_dataset.dataset_id', 'tbl_dataset.dataset_id', false, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (25, 'species', 'Taxa', 6, 1, 'tbl_taxa_tree_master.taxon_id', 'concat_ws('' '', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)', 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_genera.genus_name||'' ''||tbl_taxa_tree_master.species', true, false, 'sum', 'sum of Abundance', 32);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (35, 'tbl_biblio_modern', 'Bibligraphy modern', 1, 1, 'metainformation.view_taxa_biblio.biblio_id', 'tbl_biblio.title||''  ''||tbl_biblio.author ', 'tbl_biblio.biblio_id', 'tbl_biblio.author', true, false, 'count', 'count of species', 19);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (3, 'tbl_denormalized_measured_values_33_0', 'MS ', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_33_0', 'metainformation.tbl_denormalized_measured_values.value_33_0', 'metainformation.tbl_denormalized_measured_values.value_33_0', 'metainformation.tbl_denormalized_measured_values.value_33_0', true, false, '', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (4, 'tbl_denormalized_measured_values_33_82', 'MS Heating 550', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_33_82', 'metainformation.tbl_denormalized_measured_values.value_33_82', 'metainformation.tbl_denormalized_measured_values.value_33_82', 'metainformation.tbl_denormalized_measured_values.value_33_82', true, false, '', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (5, 'tbl_denormalized_measured_values_32', 'LOI', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_32_0', 'metainformation.tbl_denormalized_measured_values.value_32_0', 'metainformation.tbl_denormalized_measured_values.value_32', 'metainformation.tbl_denormalized_measured_values.value_32', true, false, '', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (6, 'tbl_denormalized_measured_values_37', ' P┬░', 5, 2, 'metainformation.tbl_denormalized_measured_values.value_37_0', 'metainformation.tbl_denormalized_measured_values.value_37_0', 'metainformation.tbl_denormalized_measured_values.value_37', 'metainformation.tbl_denormalized_measured_values.value_37', true, false, '', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (7, 'measured_values_helper', 'values', 0, 1, 'tbl_measured_values.measured_value', 'tbl_measured_values.measured_value', 'tbl_measured_values.measured_value', 'tbl_measured_values.measured_value', false, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (8, 'taxon_result', 'taxon_id', 0, 1, 'tbl_abundances.taxon_id', 'tbl_abundances.taxon_id', 'tbl_abundances.taxon_id', 'tbl_abundances.taxon_id', false, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (9, 'map_result', 'Site', 0, 1, 'tbl_sites.site_id', 'tbl_sites.site_name', 'tbl_sites.site_id', 'tbl_sites.site_name', false, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (12, 'record_types', 'Proxy types', 1, 1, 'tbl_record_types.record_type_id', 'tbl_record_types.record_type_name', 'tbl_record_types.record_type_id', 'tbl_record_types.record_type_name', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (13, 'sample_groups', 'Sample group', 2, 1, 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', true, true, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (14, 'places', 'Places', 2, 1, 'tbl_locations.location_id', 'tbl_locations.location_name', 'tbl_locations.location_id', 'tbl_locations.location_name', false, true, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (15, 'places_all2', 'view_places_relations', 2, 1, 'tbl_locations.location_id', 'tbl_locations.location_name', 'view_places_relations.rel_id', 'tbl_locations.location_name', false, true, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (16, 'sample_groups_helper', 'Sample group', 2, 1, 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', 'tbl_sample_groups.sample_group_id', 'tbl_sample_groups.sample_group_name', false, true, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (17, 'physical_samples', 'physical samples', 2, 1, 'tbl_physical_samples.physical_sample_id', 'tbl_physical_samples.sample_name', 'tbl_physical_samples.physical_sample_id', 'tbl_physical_samples.sample_name', false, true, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (18, 'sites', 'Site', 2, 1, 'tbl_sites.site_id', 'tbl_sites.site_name', 'tbl_sites.site_id', 'tbl_sites.site_name', true, true, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (19, 'sites_helper', 'Site', 2, 1, 'tbl_sites.site_id', 'tbl_sites.site_name', 'tbl_sites.site_id', 'tbl_sites.site_name', false, true, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (21, 'country', 'Country', 2, 1, 'countries.location_id', 'countries.location_name ', 'countries.location_id', 'countries.location_name', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (22, 'ecocode', 'Eco code', 4, 1, 'tbl_ecocode_definitions.ecocode_definition_id', 'tbl_ecocode_definitions.label', 'tbl_ecocode_definitions.ecocode_definition_id', 'tbl_ecocode_definitions.label', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (23, 'family', 'Family', 6, 1, 'tbl_taxa_tree_families.family_id', 'tbl_taxa_tree_families.family_name ', 'tbl_taxa_tree_families.family_id', 'tbl_taxa_tree_families.family_name ', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (24, 'genus', 'Genus', 6, 1, 'tbl_taxa_tree_genera.genus_id', 'tbl_taxa_tree_genera.genus_name', 'tbl_taxa_tree_genera.genus_id', 'tbl_taxa_tree_genera.genus_name', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (26, 'species_helper', 'Species', 6, 1, 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_master.taxon_id', 'tbl_taxa_tree_master.species', false, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (27, 'abundance_helper', 'abundance_id', 6, 1, 'tbl_abundances.abundance_id', 'tbl_abundances.abundance_id', 'tbl_abundances.abundance_id', 'tbl_abundances.abundance_id', false, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (28, 'species_author', 'Author', 6, 1, 'tbl_taxa_tree_authors.author_id ', 'tbl_taxa_tree_authors.author_name ', 'tbl_taxa_tree_authors.author_id ', 'tbl_taxa_tree_authors.author_name ', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (29, 'feature_type', 'Feature type', 1, 1, 'tbl_feature_types.feature_type_id ', 'tbl_feature_types.feature_type_name', 'tbl_feature_types.feature_id ', 'tbl_feature_types.feature_type_name', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (30, 'ecocode_system', 'Eco code system', 4, 1, 'tbl_ecocode_systems.ecocode_system_id ', 'tbl_ecocode_systems.name', 'tbl_ecocode_systems.ecocode_system_id ', 'tbl_ecocode_systems.definition', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (31, 'abundance_classification', 'abundance classification', 4, 1, 'metainformation.view_abundance.elements_part_mod ', 'metainformation.view_abundance.elements_part_mod ', 'metainformation.view_abundance.elements_part_mod ', 'metainformation.view_abundance.elements_part_mod ', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (10, 'geochronology', 'Geochronology', 2, 2, 'tbl_geochronology.age', 'tbl_geochronology.age', 'tbl_geochronology.age', 'tbl_geochronology.age', true, false, '', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (11, 'relative_age_name', 'Time periods', 2, 1, 'tbl_relative_ages.relative_age_id', 'tbl_relative_ages.relative_age_name', 'tbl_relative_ages.relative_age_id', 'tbl_relative_ages.relative_age_name', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (20, 'tbl_relative_dates_helper', 'tbl_relative_dates', 2, 1, 'tbl_relative_dates.relative_age_id', 'tbl_relative_dates.relative_age_name ', 'tbl_relative_dates.relative_age_name', 'tbl_relative_dates.relative_age_name ', false, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (34, 'activeseason', 'Seasons', 2, 1, 'tbl_seasons.season_id', 'tbl_seasons.season_name ', 'tbl_seasons.season_id', 'tbl_seasons.season_type ', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (32, 'abundances_all_helper', 'Abundances', 4, 2, 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance', false, false, '', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (33, 'abundances_all', 'Abundances', 4, 2, 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance ', 'metainformation.view_abundance.abundance', true, false, '', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (36, 'tbl_biblio_sample_groups', 'Bibligraphy sites/Samplegroups', 1, 1, 'tbl_biblio.biblio_id', 'tbl_biblio.title||''  ''||tbl_biblio.author', 'tbl_biblio.biblio_id', 'tbl_biblio.author', true, false, 'count', 'Number of samples', 1);
INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (37, 'tbl_biblio_sites', 'Bibligraphy sites', 1, 1, 'tbl_biblio.biblio_id', 'tbl_biblio.title||''  ''||tbl_biblio.author', 'tbl_biblio.biblio_id', 'tbl_biblio.author', false, false, 'count', 'Number of samples', 1);

/*INSERT INTO facet.facet (facet_id, facet_key, display_title, facet_group_id, facet_type_id, category_id_expr, category_name_expr, icon_id_expr, sort_expr, is_default, is_applicable, aggregate_type, aggregate_title, aggregate_facet_id) VALUES (25,'species', 'Taxa', 6, 1, 'tbl_taxa_tree_master.taxon_id',
	'concat_ws('' '', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)',
	'tbl_taxa_tree_master.taxon_id',
	'concat_ws('' '', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species)'
	0, 1, 'sum', 'sum of Abundance', 32);

update facet.facet set category_name_expr = 'concat_ws('' '', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species, tbl_taxa_tree_authors.author_name)' where facet_key = 'species';
update facet.facet set aggregate_facet_id = 32 where facet_key = 'species';
update facet.facet set sort_expr = 'concat_ws('' '', tbl_taxa_tree_genera.genus_name, tbl_taxa_tree_master.species)' where facet_key = 'species';
UPDATE facet.facet SET aggregate_facet_id = 1 WHERE aggregate_facet_id = 0 AND facet_id <> 1;
update facet.facet set facet_group_id = 2 where facet_group_id = 3;
*/

INSERT INTO facet.facet_condition_clause (facet_source_table_id, facet_id, clause) VALUES (1, 21, 'countries.location_type_id=1');
INSERT INTO facet.facet_condition_clause (facet_source_table_id, facet_id, clause) VALUES (2, 25, 'tbl_sites.site_id is not null');
INSERT INTO facet.facet_condition_clause (facet_source_table_id, facet_id, clause) VALUES (3, 32, 'metainformation.view_abundance.abundance is not null');
INSERT INTO facet.facet_condition_clause (facet_source_table_id, facet_id, clause) VALUES (4, 33, 'metainformation.view_abundance.abundance is not null');
INSERT INTO facet.facet_condition_clause (facet_source_table_id, facet_id, clause) VALUES (5, 36, 'metainformation.view_sample_group_references.biblio_id is not null');
INSERT INTO facet.facet_condition_clause (facet_source_table_id, facet_id, clause) VALUES (6, 37, 'metainformation.view_site_references.biblio_id is not null');

INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (1, 1, 1, '', 'tbl_analysis_entities', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (2, 2, 1, '', 'tbl_datasets', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (3, 3, 1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (4, 4, 1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (5, 5, 1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (6, 6, 1, '', 'metainformation.tbl_denormalized_measured_values', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (7, 7, 1, '', 'tbl_measured_values', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (8, 8, 1, '', 'tbl_abundances', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (9, 9, 1, '', 'tbl_sites', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (10, 10, 1, '', 'tbl_geochronology', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (11, 11, 1, '', 'tbl_relative_ages', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (12, 12, 1, '', 'tbl_record_types', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (13, 13, 1, '', 'tbl_sample_groups', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (14, 14, 1, '', 'tbl_locations', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (15, 15, 1, '', 'view_places_relations', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (16, 16, 1, '', 'tbl_sample_groups', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (17, 17, 1, '', 'tbl_physical_samples', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (18, 18, 1, '', 'tbl_sites', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (19, 19, 1, '', 'tbl_sites', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (20, 20, 1, '', 'tbl_relative_dates', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (21, 21, 1, '', 'tbl_locations', 'countries');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (22, 22, 1, '', 'tbl_ecocode_definitions', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (23, 23, 1, '', 'tbl_taxa_tree_families', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (24, 24, 1, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (25, 25, 1, '', 'tbl_taxa_tree_master', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (26, 26, 1, '', 'tbl_taxa_tree_master', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (27, 27, 1, '', 'tbl_abundances', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (28, 28, 1, '', 'tbl_taxa_tree_authors', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (29, 29, 1, '', 'tbl_feature_types', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (30, 30, 1, '', 'tbl_ecocode_systems', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (31, 31, 1, '', 'metainformation.view_abundance', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (32, 32, 1, '', 'metainformation.view_abundance', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (33, 33, 1, '', 'metainformation.view_abundance', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (34, 34, 1, '', 'tbl_seasons', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (35, 35, 1, '', 'metainformation.view_taxa_biblio', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (36, 36, 1, '', 'tbl_biblio', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (37, 37, 1, '', 'tbl_biblio', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (38, 1, 2, '', 'tbl_physical_samples', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (39, 14, 2, '', 'tbl_site_locations', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (40, 15, 2, '', 'tbl_locations', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (41, 21, 2, '', 'tbl_site_locations', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (42, 22, 2, '', 'tbl_ecocode_definitions', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (43, 23, 2, '', 'tbl_taxa_tree_families', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (44, 24, 2, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (45, 25, 2, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (46, 26, 2, '', 'tbl_taxa_tree_genera', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (47, 28, 2, '', 'tbl_taxa_tree_authors', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (48, 29, 2, '', 'tbl_physical_sample_features', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (49, 30, 2, '', 'tbl_ecocode_systems', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (50, 35, 2, '', 'tbl_biblio', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (51, 36, 2, '', 'metainformation.view_sample_group_references', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (52, 37, 2, '', 'metainformation.view_site_references', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (57, 25, 4, '', 'tbl_sites', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (53, 1, 3, '', 'tbl_datasets', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (54, 15, 3, '', 'tbl_site_locations', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (55, 25, 3, '', 'tbl_taxa_tree_authors', '');
INSERT INTO facet.facet_table (facet_table_id, facet_id, sequence_id, schema_name, table_name, alias) VALUES (56, 26, 3, '', 'tbl_taxa_tree_authors', '');

INSERT INTO facet.graph_table (table_id, table_name) VALUES (1, 'tbl_horizons');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (2, 'tbl_aggregate_samples');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (3, 'tbl_analysis_entity_prep_methods');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (4, 'tbl_analysis_entities');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (5, 'tbl_dating_uncertainty');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (6, 'tbl_feature_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (7, 'view_places_relations');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (8, 'tbl_sample_coordinates');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (9, 'tbl_project_stages');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (10, 'tbl_sample_alt_refs');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (11, 'tbl_activity_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (12, 'tbl_measured_value_dimensions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (13, 'tbl_taxa_synonyms');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (14, 'tbl_colours');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (15, 'tbl_site_references');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (16, 'tbl_image_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (17, 'tbl_project_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (18, 'tbl_mcr_names');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (19, 'tbl_mcr_summary_data');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (20, 'tbl_taxa_images');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (21, 'tbl_taxonomic_order');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (22, 'tbl_imported_taxa_replacements');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (23, 'tbl_dendro_measurement_lookup');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (24, 'tbl_rdb_codes');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (25, 'metainformation.view_abundances_by_taxon_analysis_entity');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (26, 'metainformation.view_site_references');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (27, 'tbl_taxa_seasonality');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (28, 'tbl_sample_group_description_type_sampling_contexts');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (29, 'tbl_contacts');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (30, 'tbl_sample_group_coordinates');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (31, 'tbl_tephras');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (32, 'tbl_sample_colours');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (33, 'tbl_method_groups');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (34, 'tbl_keywords');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (35, 'tbl_locations');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (36, 'tbl_taxa_tree_families');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (37, 'tbl_relative_age_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (38, 'tbl_abundance_elements');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (39, 'tbl_taxa_tree_authors');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (40, 'tbl_sample_group_sampling_contexts');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (41, 'tbl_sample_horizons');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (42, 'tbl_sample_group_references');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (43, 'tbl_taxonomic_order_systems');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (44, 'tbl_abundance_modifications');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (45, 'tbl_taxa_tree_orders');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (46, 'countries');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (47, 'tbl_aggregate_order_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (48, 'tbl_rdb');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (49, 'tbl_sample_locations');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (50, 'tbl_abundance_ident_levels');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (51, 'tbl_ecocode_groups');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (52, 'tbl_sample_group_notes');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (53, 'tbl_taxonomy_notes');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (54, 'tbl_sample_group_description_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (55, 'tbl_relative_dates');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (56, 'tbl_sample_description_sample_group_contexts');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (57, 'metainformation.view_abundance');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (58, 'tbl_projects');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (59, 'tbl_chron_controls');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (60, 'tbl_geochronology');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (61, 'tbl_sample_images');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (62, 'tbl_ecocode_definitions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (63, 'tbl_taxonomic_order_biblio');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (64, 'tbl_site_images');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (65, 'tbl_sample_location_type_sampling_contexts');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (66, 'tbl_publishers');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (67, 'tbl_text_identification_keys');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (68, 'tbl_relative_age_refs');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (69, 'tbl_dendro');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (70, 'tbl_mcrdata_birmbeetledat');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (71, 'tbl_relative_ages');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (72, 'tbl_units');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (73, 'tbl_sample_description_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (74, 'tbl_years_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (75, 'tbl_features');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (76, 'tbl_dataset_masters');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (77, 'tbl_site_other_records');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (78, 'tbl_site_natgridrefs');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (79, 'tbl_languages');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (80, 'tbl_lithology');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (81, 'tbl_ceramics_measurement_lookup');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (82, 'tbl_seasons');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (83, 'tbl_dataset_contacts');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (84, 'tbl_biblio');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (85, 'tbl_dating_labs');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (86, 'tbl_datasets');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (87, 'tbl_geochron_refs');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (88, 'tbl_abundances');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (89, 'tbl_methods');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (90, 'tbl_text_distribution');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (91, 'tbl_sample_groups');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (92, 'tbl_location_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (93, 'tbl_analysis_entity_dimensions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (94, 'tbl_sample_notes');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (95, 'tbl_site_preservation_status');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (96, 'tbl_analysis_entity_ages');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (97, 'tbl_ceramics_measurements');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (98, 'tbl_dimensions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (99, 'metainformation.view_taxa_biblio');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (100, 'tbl_dendro_measurements');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (101, 'tbl_ecocodes');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (102, 'tbl_physical_samples');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (103, 'tbl_taxa_common_names');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (104, 'tbl_data_type_groups');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (105, 'tbl_sample_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (106, 'tbl_coordinate_method_dimensions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (107, 'tbl_tephra_dates');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (108, 'tbl_season_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (109, 'tbl_taxa_tree_master');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (110, 'tbl_record_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (111, 'tbl_sample_descriptions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (112, 'tbl_sample_group_dimensions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (113, 'tbl_site_locations');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (114, 'metainformation.view_sample_group_references');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (115, 'tbl_aggregate_sample_ages');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (116, 'tbl_sample_dimensions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (117, 'tbl_chronologies');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (118, 'tbl_species_association_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (119, 'tbl_sites');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (120, 'tbl_collections_or_journals');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (121, 'tbl_measured_values');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (122, 'tbl_text_biology');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (123, 'tbl_contact_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (124, 'tbl_sample_group_images');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (125, 'metainformation.tbl_denormalized_measured_values');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (126, 'tbl_taxa_measured_attributes');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (127, 'tbl_dendro_date_notes');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (128, 'tbl_ecocode_systems');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (129, 'tbl_tephra_refs');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (130, 'tbl_sample_group_descriptions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (131, 'tbl_species_associations');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (132, 'tbl_physical_sample_features');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (133, 'tbl_dataset_submissions');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (134, 'tbl_dating_material');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (135, 'tbl_data_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (136, 'tbl_alt_ref_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (137, 'tbl_biblio_keywords');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (138, 'tbl_dendro_dates');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (139, 'tbl_ceramics');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (140, 'tbl_dataset_submission_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (141, 'tbl_taxa_reference_specimens');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (142, 'tbl_modification_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (143, 'tbl_rdb_systems');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (144, 'tbl_sample_location_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (145, 'tbl_aggregate_datasets');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (146, 'tbl_identification_levels');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (147, 'tbl_chron_control_types');
INSERT INTO facet.graph_table (table_id, table_name) VALUES (148, 'tbl_taxa_tree_genera');

/*
INSERT INTO facet.graph_table (table_name)
	select distinct table_name
	from (
		select source_table as table_name
		from metainformation.tbl_foreign_relations
		union
		select target_table as table_name
		from metainformation.tbl_foreign_relations
	) as X;
*/

INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (1, 38, 110, 20, 'record_type_id', 'record_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (2, 50, 88, 20, 'abundance_id', 'abundance_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (3, 50, 146, 20, 'identification_level_id', 'identification_level_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (4, 44, 88, 20, 'abundance_id', 'abundance_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (5, 44, 142, 20, 'modification_type_id', 'modification_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (6, 88, 38, 20, 'abundance_element_id', 'abundance_element_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (7, 88, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (8, 145, 47, 20, 'aggregate_order_type_id', 'aggregate_order_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (9, 115, 145, 20, 'aggregate_dataset_id', 'aggregate_dataset_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (10, 115, 96, 20, 'analysis_entity_age_id', 'analysis_entity_age_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (11, 2, 145, 20, 'aggregate_dataset_id', 'aggregate_dataset_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (12, 2, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (13, 96, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (14, 96, 117, 20, 'chronology_id', 'chronology_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (15, 93, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (16, 93, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (17, 3, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (18, 3, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (19, 137, 84, 20, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (20, 137, 34, 20, 'keyword_id', 'keyword_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (21, 139, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (22, 139, 97, 20, 'ceramics_measurement_id', 'ceramics_measurement_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (23, 81, 97, 20, 'ceramics_measurement_id', 'ceramics_measurement_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (24, 97, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (25, 59, 147, 20, 'chron_control_type_id', 'chron_control_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (26, 59, 117, 20, 'chronology_id', 'chronology_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (27, 117, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (28, 117, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (29, 120, 66, 20, 'publisher_id', 'publisher_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (30, 14, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (31, 106, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (32, 106, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (33, 83, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (34, 83, 123, 20, 'contact_type_id', 'contact_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (35, 83, 86, 20, 'dataset_id', 'dataset_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (36, 76, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (37, 86, 135, 20, 'data_type_id', 'data_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (38, 86, 76, 20, 'master_set_id', 'master_set_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (39, 86, 58, 20, 'project_id', 'project_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (40, 86, 86, 20, 'updated_dataset_id', 'dataset_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (41, 133, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (42, 133, 86, 20, 'dataset_id', 'dataset_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (43, 133, 140, 20, 'submission_type_id', 'submission_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (44, 135, 104, 20, 'data_type_group_id', 'data_type_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (45, 85, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (46, 134, 38, 20, 'abundance_element_id', 'abundance_element_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (47, 134, 60, 20, 'geochron_id', 'geochron_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (48, 134, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (49, 69, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (50, 69, 100, 20, 'dendro_measurement_id', 'dendro_measurement_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (51, 127, 138, 20, 'dendro_date_id', 'dendro_date_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (52, 138, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (53, 138, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (54, 138, 74, 20, 'years_type_id', 'years_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (55, 23, 100, 20, 'dendro_measurement_id', 'dendro_measurement_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (56, 100, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (57, 98, 33, 20, 'method_group_id', 'method_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (58, 98, 72, 20, 'unit_id', 'unit_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (59, 62, 51, 20, 'ecocode_group_id', 'ecocode_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (60, 51, 128, 20, 'ecocode_system_id', 'ecocode_system_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (61, 101, 62, 20, 'ecocode_definition_id', 'ecocode_definition_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (62, 101, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (63, 75, 6, 20, 'feature_type_id', 'feature_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (64, 60, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (65, 60, 85, 20, 'dating_lab_id', 'dating_lab_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (66, 60, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (67, 87, 60, 20, 'geochron_id', 'geochron_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (68, 22, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (69, 80, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (70, 145, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (71, 4, 86, 1, 'dataset_id', 'dataset_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (72, 4, 102, 1, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (73, 86, 89, 1, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (74, 86, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (75, 128, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (76, 87, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (77, 1, 89, 70, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (78, 76, 84, 200, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (79, 35, 92, 20, 'location_type_id', 'location_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (80, 70, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (81, 18, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (82, 19, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (83, 12, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (84, 12, 121, 20, 'measured_value_id', 'measured_value_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (85, 121, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (86, 89, 33, 20, 'method_group_id', 'method_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (87, 89, 110, 20, 'record_type_id', 'record_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (88, 132, 75, 20, 'feature_id', 'feature_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (89, 132, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (90, 102, 136, 20, 'alt_ref_type_id', 'alt_ref_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (91, 102, 105, 20, 'sample_type_id', 'sample_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (92, 58, 9, 20, 'project_stage_id', 'project_stage_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (93, 58, 17, 20, 'project_type_id', 'project_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (94, 48, 24, 20, 'rdb_code_id', 'rdb_code_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (95, 48, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (96, 24, 143, 20, 'rdb_system_id', 'rdb_system_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (97, 68, 71, 20, 'relative_age_id', 'relative_age_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (98, 71, 37, 20, 'relative_age_type_id', 'relative_age_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (99, 55, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (100, 55, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (101, 55, 71, 20, 'relative_age_id', 'relative_age_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (102, 10, 136, 20, 'alt_ref_type_id', 'alt_ref_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (103, 10, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (104, 32, 14, 20, 'colour_id', 'colour_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (105, 32, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (106, 8, 106, 20, 'coordinate_method_dimension_id', 'coordinate_method_dimension_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (107, 8, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (108, 111, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (109, 111, 73, 20, 'sample_description_type_id', 'sample_description_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (110, 56, 73, 20, 'sample_description_type_id', 'sample_description_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (111, 56, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (112, 116, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (113, 116, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (114, 30, 106, 20, 'coordinate_method_dimension_id', 'coordinate_method_dimension_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (115, 30, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (116, 130, 54, 20, 'sample_group_description_type_id', 'sample_group_description_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (117, 130, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (118, 28, 54, 20, 'sample_group_description_type_id', 'sample_group_description_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (119, 28, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (120, 112, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (121, 112, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (122, 124, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (123, 124, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (124, 52, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (125, 42, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (126, 91, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (127, 41, 1, 20, 'horizon_id', 'horizon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (128, 41, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (129, 61, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (130, 61, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (131, 49, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (132, 49, 144, 20, 'sample_location_type_id', 'sample_location_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (133, 65, 144, 20, 'sample_location_type_id', 'sample_location_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (134, 65, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (135, 94, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (136, 82, 108, 20, 'season_type_id', 'season_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (137, 64, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (138, 64, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (139, 64, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (140, 113, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (141, 89, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (142, 102, 91, 1, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (143, 48, 35, 150, 'location_id', 'location_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (144, 143, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (145, 143, 35, 150, 'location_id', 'location_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (146, 68, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (147, 55, 89, 70, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (148, 116, 89, 150, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (149, 91, 89, 150, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (150, 91, 119, 1, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (151, 113, 35, 5, 'location_id', 'location_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (152, 42, 84, 90, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (153, 78, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (154, 78, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (155, 95, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (156, 15, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (157, 131, 118, 20, 'association_type_id', 'association_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (158, 131, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (159, 103, 79, 20, 'language_id', 'language_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (160, 103, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (161, 20, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (162, 20, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (163, 126, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (164, 141, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (165, 141, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (166, 27, 11, 20, 'activity_type_id', 'activity_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (167, 27, 82, 20, 'season_id', 'season_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (168, 27, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (169, 109, 39, 20, 'author_id', 'author_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (170, 21, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (171, 21, 43, 20, 'taxonomic_order_system_id', 'taxonomic_order_system_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (172, 63, 43, 20, 'taxonomic_order_system_id', 'taxonomic_order_system_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (173, 53, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (174, 107, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (175, 107, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (176, 107, 31, 20, 'tephra_id', 'tephra_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (177, 129, 31, 20, 'tephra_id', 'tephra_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (178, 122, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (179, 90, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (180, 67, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (181, 88, 4, 1, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (182, 89, 72, 150, 'unit_id', 'unit_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (183, 71, 35, 70, 'location_id', 'location_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (184, 77, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (185, 77, 119, 150, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (186, 77, 110, 150, 'record_type_id', 'record_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (187, 131, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (188, 27, 35, 60, 'location_id', 'location_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (189, 13, 39, 150, 'author_id', 'author_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (190, 13, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (191, 122, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (192, 13, 36, 150, 'family_id', 'family_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (193, 13, 148, 150, 'genus_id', 'genus_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (194, 13, 109, 150, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (195, 36, 45, 1, 'order_id', 'order_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (196, 148, 36, 1, 'family_id', 'family_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (197, 109, 148, 1, 'genus_id', 'genus_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (198, 45, 110, 1, 'record_type_id', 'record_type_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (199, 63, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (200, 53, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (201, 90, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (202, 67, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (203, 129, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (204, 46, 7, 20, 'location_id', 'rel_to_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (205, 35, 7, 1, 'location_id', 'rel_to_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (206, 15, 84, 90, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (207, 102, 125, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (208, 25, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (209, 25, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (211, 57, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (212, 114, 84, 80, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (213, 114, 91, 15, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (214, 26, 84, 80, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (215, 26, 119, 15, 'site_id', 'site_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (216, 99, 84, 10, 'biblio_id', 'biblio_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (217, 99, 109, 2, 'taxon_id', 'taxon_id');
INSERT INTO facet.graph_table_relation (relation_id, source_table_id, target_table_id, weight, source_column_name, target_column_name) VALUES (210, 57, 4, 2, 'analysis_entity_id', 'analysis_entity_id');

INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (1, 'sitename', 'tbl_sites', 'tbl_sites.site_name', 'Site name', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (2, 'record_type', 'tbl_record_types', 'tbl_record_types.record_type_name', 'Record type(s)', 'text_agg_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (3, 'analysis_entities', 'tbl_analysis_entities', 'tbl_analysis_entities.analysis_entity_id', 'Filtered records', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (4, 'site_link', 'tbl_sites', 'tbl_sites.site_id', 'Full report', 'link_item', true, 'api/report/show_site_details.php?site_id', 'Show site report');
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (5, 'site_link_filtered', 'tbl_sites', 'tbl_sites.site_id', 'Filtered report', 'link_item', true, 'api/report/show_site_details.php?site_id', 'Show filtered report');
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (6, 'aggregate_all_filtered', 'tbl_aggregate_samples', '''Aggregated''::text', 'Filtered report', 'link_item_filtered', true, 'api/report/show_details_all_levels.php?level', NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (7, 'sample_group_link', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Full report', 'link_item', true, 'api/report/show_sample_group_details.php?sample_group_id', NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (8, 'sample_group_link_filtered', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Filtered report', 'link_item', true, 'api/report/show_sample_group_details.php?sample_group_id', NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (9, 'abundance', 'tbl_abundances', 'tbl_abundances.abundance', 'number of taxon_id', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (10, 'taxon_id', 'tbl_abundances', 'tbl_abundances.taxon_id', 'Taxon id  (specie)', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (11, 'dataset', 'tbl_datasets', 'tbl_datasets.dataset_name', 'Dataset', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (12, 'dataset_link', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Dataset details', 'single_item', true, 'client/show_dataset_details.php?dataset_id', NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (13, 'dataset_link_filtered', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Filtered report', 'single_item', true, 'client/show_dataset_details.php?dataset_id', NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (14, 'sample_group', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_name', 'Sample group', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (15, 'methods', 'tbl_methods', 'tbl_methods.method_name', 'Method', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (18, 'category_id', NULL, 'category_id', 'Site ID', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (19, 'category_name', NULL, 'category_name', 'Site Name', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (20, 'latitude_dd', NULL, 'latitude_dd', 'Latitude (dd)', 'single_item', true, NULL, NULL);
INSERT INTO facet.result_field (result_field_id, result_field_key, table_name, column_name, display_text, field_type_id, activated, link_url, link_label) VALUES (21, 'longitude_dd', NULL, 'longitude_dd', 'Longitude (dd)', 'single_item', true, NULL, NULL);

-- DEPRECATED???
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('sitename', 'tbl_sites', 'tbl_sites.site_name', 'Site name', 'single_item', '1', 'Array', '', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('record_type', 'tbl_record_types', 'tbl_record_types.record_type_name', 'Record type(s)', 'text_agg_item', '1', 'Array', '', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('analysis_entities', 'tbl_analysis_entities', 'tbl_analysis_entities.analysis_entity_id', 'Filtered records', 'single_item', '1', 'Array', '', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('site_link', 'tbl_sites', 'tbl_sites.site_id', 'Full report', 'link_item', '1', 'Array', 'api/report/show_site_details.php?site_id', 'Show site report');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('site_link_filtered', 'tbl_sites', 'tbl_sites.site_id', 'Filtered report', 'link_item', '1', 'Array', 'api/report/show_site_details.php?site_id', 'Show filtered report');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('aggregate_all_filtered', 'tbl_aggregate_samples', '''Aggregated''::text', 'Filtered report', 'link_item_filtered', '1', 'Array', 'api/report/show_details_all_levels.php?level', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('sample_group_link', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Full report', 'link_item', '1', 'Array', 'api/report/show_sample_group_details.php?sample_group_id', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('sample_group_link_filtered', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Filtered report', 'link_item', '1', 'Array', 'api/report/show_sample_group_details.php?sample_group_id', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('abundance', 'tbl_abundances', ' tbl_abundances.abundance', 'number of taxon_id', 'single_item', '1', 'Array', '', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('taxon_id', 'tbl_abundances', ' tbl_abundances.taxon_id', 'Taxon id  (specie)', 'single_item', '1', 'Array', '', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('dataset', 'tbl_datasets', 'tbl_datasets.dataset_name', 'Dataset', 'single_item', '1', 'Array', '', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('dataset_link', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Dataset details', 'single_item', '1', 'Array', 'client/show_dataset_details.php?dataset_id', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('dataset_link_filtered', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Filtered report', 'single_item', '1', 'Array', 'client/show_dataset_details.php?dataset_id', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('sample_group', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_name', 'Sample group', 'single_item', '1', 'Array', '', '');
--INSERT INTO facet.tbl_result_fields (result_field_key, table_name, column_name, display_text, result_type, activated, parents, link_url, link_label) VALUES ('methods', 'tbl_methods', 'tbl_methods.method_name', 'Method', 'single_item', '1', 'Array', '', '');

INSERT INTO facet.result_aggregate (aggregate_id, aggregate_key, display_text, is_applicable, is_activated, input_type, has_selector) VALUES (1, 'site_level', 'Site level', false, true, 'checkboxes', true);
INSERT INTO facet.result_aggregate (aggregate_id, aggregate_key, display_text, is_applicable, is_activated, input_type, has_selector) VALUES (2, 'aggregate_all', 'Aggregate all', false, true, 'checkboxes', true);
INSERT INTO facet.result_aggregate (aggregate_id, aggregate_key, display_text, is_applicable, is_activated, input_type, has_selector) VALUES (3, 'sample_group_level', 'Sample group level', false, true, 'checkboxes', true);
INSERT INTO facet.result_aggregate (aggregate_id, aggregate_key, display_text, is_applicable, is_activated, input_type, has_selector) VALUES (4, 'map_result', 'Map result', false, false, 'checkboxes', false);

INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('sum_item', true, 'TemplateFieldCompiler', true, false, false, 'SUM({0}::double precision) AS sum_of_{0}');
INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('count_item', true, 'TemplateFieldCompiler', true, false, false, 'COUNT({0}) AS count_of_{0}');
INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('avg_item', true, 'TemplateFieldCompiler', true, false, false, 'AVG({0}) AS avg_of_{0}');
INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('text_agg_item', true, 'TemplateFieldCompiler', true, false, false, 'ARRAY_TO_STRING(ARRAY_AGG(DISTINCT {0}),'','') AS text_agg_of_{0}');
INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('single_item', true, 'TemplateFieldCompiler', false, false, true, '{0}');
INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('link_item', true, 'TemplateFieldCompiler', false, false, true, '{0}');
INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('link_item_filtered', true, 'TemplateFieldCompiler', false, false, true, '{0}');
INSERT INTO facet.result_field_type (field_type_id, is_result_value, sql_field_compiler, is_aggregate_field, is_sort_field, is_item_field, sql_template) VALUES ('sort_item', false, 'TemplateFieldCompiler', false, true, false, '{0}');

INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (4, 1, 1, 'single_item', 1);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (5, 1, 2, 'text_agg_item', 2);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (8, 1, 3, 'count_item', 3);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (10, 1, 4, 'link_item', 4);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (16, 1, 5, 'link_item_filtered', 5);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (13, 1, 1, 'sort_item', 99);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (15, 2, 6, 'link_item_filtered', 1);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (7, 2, 3, 'count_item', 2);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (1, 3, 1, 'single_item', 1);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (2, 3, 14, 'single_item', 2);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (3, 3, 2, 'single_item', 3);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (6, 3, 3, 'count_item', 4);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (9, 3, 7, 'link_item', 5);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (14, 3, 8, 'link_item_filtered', 6);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (11, 3, 1, 'sort_item', 99);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (12, 3, 14, 'sort_item', 99);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (23, 4, 18, 'single_item', 1);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (24, 4, 19, 'single_item', 2);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (21, 4, 20, 'single_item', 3);
INSERT INTO facet.result_aggregate_field (aggregate_field_id, aggregate_id, result_field_id, field_type_id, sequence_id) VALUES (22, 4, 21, 'single_item', 4);

/*
INSERT INTO facet.result_aggregate_field (aggregate_id, result_type_id, result_field_id)
	select x.aggregate_id, y.result_type_id, z.result_field_id
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
	) as d(result_aggregate, result_type, result_field)
	join facet.result_aggregate x
	  on x.result_aggregate_key = d.result_aggregate
	join facet.result_type y
	  on y.result_type = d.result_type
	join facet.result_field z
	  on z.result_field_key = d.result_field
*/

INSERT INTO facet.result_view_type (view_type_id, view_name, is_cachable) VALUES ('tabular', 'Tabular', true);
INSERT INTO facet.result_view_type (view_type_id, view_name, is_cachable) VALUES ('map', 'Map', false);

/* Create foreign key constraints */

ALTER TABLE ONLY facet_condition_clause ADD CONSTRAINT facet_condition_clause_facet_fk FOREIGN KEY (facet_id) REFERENCES facet(facet_id);
ALTER TABLE ONLY facet ADD CONSTRAINT facet_facet_group_fk FOREIGN KEY (facet_group_id) REFERENCES facet_group(facet_group_id);
ALTER TABLE ONLY facet ADD CONSTRAINT facet_facet_type_fk FOREIGN KEY (facet_type_id) REFERENCES facet_type(facet_type_id);
ALTER TABLE ONLY facet_table ADD CONSTRAINT facet_table_facet_fk FOREIGN KEY (facet_id) REFERENCES facet(facet_id);

ALTER TABLE ONLY graph_table_relation ADD CONSTRAINT graph_table_relation_fk1 FOREIGN KEY (source_table_id) REFERENCES graph_table(table_id);
ALTER TABLE ONLY graph_table_relation ADD CONSTRAINT graph_table_relation_fk2 FOREIGN KEY (target_table_id) REFERENCES graph_table(table_id);

ALTER TABLE ONLY result_aggregate_field ADD CONSTRAINT result_definition_fieldfk1 FOREIGN KEY (aggregate_id) REFERENCES result_aggregate(aggregate_id);
ALTER TABLE ONLY result_aggregate_field ADD CONSTRAINT result_definition_fieldfk2 FOREIGN KEY (result_field_id) REFERENCES result_field(result_field_id);
ALTER TABLE ONLY result_aggregate_field ADD CONSTRAINT result_definition_fieldfk3 FOREIGN KEY (field_type_id) REFERENCES result_field_type(field_type_id);
ALTER TABLE ONLY result_field ADD CONSTRAINT result_fieldfk3 FOREIGN KEY (field_type_id) REFERENCES result_field_type(field_type_id);

/* Create Indexes */
DROP INDEX IF EXISTS idx_facet_condition_clause_fk1;
DROP INDEX IF EXISTS idx_facet_table_fk1;
DROP INDEX IF EXISTS idx_graph_table_relation_fk1;
DROP INDEX IF EXISTS idx_graph_table_relation_fk2;
DROP INDEX IF EXISTS idx_result_aggregate_field_fk1;
DROP INDEX IF EXISTS idx_result_aggregate_field_fk2;

CREATE INDEX idx_facet_condition_clause_fk1 ON facet_condition_clause (facet_id);
CREATE INDEX idx_facet_table_fk1 ON facet_table (facet_id);

CREATE INDEX idx_graph_table_relation_fk1 ON graph_table_relation (source_table_id);
CREATE INDEX idx_graph_table_relation_fk2 ON graph_table_relation (target_table_id);

CREATE INDEX idx_result_aggregate_field_fk1 on facet.result_aggregate_field (aggregate_id);
CREATE INDEX idx_result_aggregate_field_fk2 on facet.result_aggregate_field (result_field_id);

/* Set privileges */

GRANT USAGE ON SCHEMA facet TO public, sead_read, sead_write;

GRANT SELECT ON ALL TABLES IN SCHEMA facet TO public, sead_read, sead_write;
GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA facet to public, sead_read, sead_write;
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA facet TO public, sead_read, sead_write;

ALTER DEFAULT PRIVILEGES IN SCHEMA facet GRANT SELECT, TRIGGER ON TABLES TO public, sead_read, sead_write;

