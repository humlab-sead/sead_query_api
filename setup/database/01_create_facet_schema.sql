
DO $$
BEGIN

    SET search_path = facet, pg_catalog;
    SET default_with_oids = false;

	IF current_database() != 'sead_staging' THEN
		RAISE EXCEPTION 'This script must be run in sead_staging!';
	END IF;

	IF current_role != 'querysead_owner' THEN
		RAISE EXCEPTION 'This script must be run as querysead_owner!';
	END IF;

    /* Create Tables */

    CREATE TABLE IF NOT EXISTS facet.facet_group (
        facet_group_id integer NOT NULL PRIMARY KEY,
        facet_group_key character varying(80) NOT NULL,
        display_title character varying(80) NOT NULL,
        is_applicable boolean NOT NULL,
        is_default boolean NOT NULL
    );

    CREATE TABLE IF NOT EXISTS facet.facet_type (
        facet_type_id integer NOT NULL PRIMARY KEY,
        facet_type_name character varying(80) NOT NULL,
        reload_as_target boolean DEFAULT false NOT NULL
    );

    CREATE TABLE IF NOT EXISTS facet.facet (
        facet_id integer NOT NULL PRIMARY KEY,
        facet_key character varying(80) NOT NULL,
        display_title character varying(80) NOT NULL,
        facet_group_id integer NOT NULL REFERENCES facet_group(facet_group_id),
        facet_type_id integer NOT NULL REFERENCES facet_type(facet_type_id),
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
        facet_id integer NOT NULL REFERENCES facet(facet_id),
        clause character varying(512)
    );

    CREATE TABLE IF NOT EXISTS facet.facet_table (
        facet_table_id serial PRIMARY KEY,
        facet_id integer NOT NULL REFERENCES facet(facet_id),
        sequence_id integer NOT NULL,
        schema_name character varying(80),
        table_name character varying(80),
        alias character varying(80)
    );

    CREATE TABLE IF NOT EXISTS facet.graph_table (
        table_id serial PRIMARY KEY,
        table_name information_schema.sql_identifier NOT NULL
    );

    CREATE TABLE IF NOT EXISTS facet.graph_table_relation (
        relation_id serial PRIMARY KEY,
        source_table_id integer NOT NULL REFERENCES graph_table(table_id),
        target_table_id integer NOT NULL REFERENCES graph_table(table_id),
        weight integer DEFAULT 0 NOT NULL,
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

    CREATE TABLE IF NOT EXISTS facet.result_field_type (
        field_type_id character varying(40) PRIMARY KEY,
        is_result_value boolean DEFAULT true NOT NULL,
        sql_field_compiler character varying(40) DEFAULT ''::character varying NOT NULL,
        is_aggregate_field boolean DEFAULT false NOT NULL,
        is_sort_field boolean DEFAULT false NOT NULL,
        is_item_field boolean DEFAULT false NOT NULL,
        sql_template character varying(256) DEFAULT '{0}'::character varying NOT NULL
    );

    CREATE TABLE IF NOT EXISTS facet.result_field (
        result_field_id serial PRIMARY KEY,
        result_field_key character varying(40) NOT NULL,
        table_name character varying(80),
        column_name character varying(80) NOT NULL,
        display_text character varying(80) NOT NULL,
        field_type_id character varying(20) NOT NULL REFERENCES result_field_type(field_type_id),
        activated boolean NOT NULL,
        link_url character varying(256),
        link_label character varying(256)
    );

    CREATE TABLE IF NOT EXISTS facet.result_aggregate_field (
        aggregate_field_id serial PRIMARY KEY,
        aggregate_id integer NOT NULL REFERENCES result_aggregate(aggregate_id),
        result_field_id integer NOT NULL REFERENCES result_field(result_field_id),
        field_type_id character varying(40) DEFAULT 'single_item'::character varying NOT NULL REFERENCES result_field_type(field_type_id),
        sequence_id integer DEFAULT 0 NOT NULL
    );


    CREATE TABLE IF NOT EXISTS facet.result_view_type (
        view_type_id character varying(40) NOT NULL PRIMARY KEY,
        view_name character varying(40) NOT NULL,
        is_cachable boolean DEFAULT true NOT NULL
    );

    /*
    CREATE TABLE IF NOT EXISTS facet.tbl_result_fields (
        result_field_key character varying(256),
        table_name character varying(256),
        column_name character varying(256),
        display_text character varying(256),
        result_type character varying(256),
        activated character varying(256),
        parents character varying(256),
        link_url character varying(256),
        link_label character varying(256)
    );
    */

    CREATE TABLE IF NOT EXISTS facet.view_state (
        view_state_key character varying(80) NOT NULL PRIMARY KEY,
        view_state_data text NOT NULL,
        create_time timestamp with time zone DEFAULT clock_timestamp()
    );

    -- CREATE TABLE IF NOT EXISTS facet.viewstate (
    --     viewstate_key character varying(80) NOT NULL PRIMARY KEY,
    --     viewstate_data text NOT NULL
    -- );

    CREATE VIEW report_site AS
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

    CREATE INDEX idx_graph_table_relation_fk1 ON graph_table_relation USING btree (source_table_id);
    CREATE INDEX idx_graph_table_relation_fk2 ON graph_table_relation USING btree (target_table_id);

END $$ language plpgsql;

