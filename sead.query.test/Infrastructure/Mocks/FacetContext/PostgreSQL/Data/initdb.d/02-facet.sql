--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1 (Debian 16.1-1.pgdg110+1)
-- Dumped by pg_dump version 16.8 (Debian 16.8-1.pgdg110+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: facet; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA facet;


--
-- Name: create_or_update_facet(jsonb); Type: FUNCTION; Schema: facet; Owner: -
--

CREATE FUNCTION facet.create_or_update_facet(j_facet jsonb) RETURNS json
    LANGUAGE plpgsql
    AS $$
            declare j_tables json;
            declare j_clauses json;
            declare i_facet_id int;
            declare s_facet_code text;
            declare s_aggregate_facet_code text;
            declare i_aggregate_facet_id int = 0;
        begin

            j_tables = j_facet -> 'tables';
            j_clauses = j_facet -> 'clauses';

            i_facet_id = (j_facet ->> 'facet_id')::int;
            s_facet_code = (j_facet ->> 'facet_code')::text;

            -- Save facet's association for domain facets before we delete the facet
            -- drop table if exists _facet_children_temp;
            if exists (
                select 1 from pg_catalog.pg_class 
                where relname = '_facet_children_temp' 
                  and relkind = 'r' 
                  and pg_catalog.pg_table_is_visible(oid) -- Ensures it's visible in the current session
            ) then
                drop table _facet_children_temp;
            end if;

            create temporary table _facet_children_temp as
                select facet_code, child_facet_code, position
                from facet.facet_children
                where s_facet_code in (facet_code, child_facet_code);

            if i_facet_id is null then
                i_facet_id = (select coalesce(max(facet_id),0)+1 from facet.facet);
            else

                delete from facet.facet_children
                    where  s_facet_code in (facet_code, child_facet_code);

                delete from facet.facet
                    where facet_id = i_facet_id;

            end if;

            s_aggregate_facet_code = (j_facet ->> 'aggregate_facet_code')::text;

            if  s_aggregate_facet_code is null then
                i_aggregate_facet_id = 0;
            else
                i_aggregate_facet_id = (select facet_id from facet.facet where facet_code = s_aggregate_facet_code);
                if i_aggregate_facet_id is null then
                    raise notice 'aggregate_facet_id not found for "%" - "%"', (j_facet ->> 'facet_code')::text, s_aggregate_facet_code;
                end if;
            end if;

            insert into facet.facet (facet_id, facet_code, display_title, description, facet_group_id, facet_type_id, category_id_expr, category_id_type, category_id_operator, category_name_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id)
                (values (
                    i_facet_id,
                    (j_facet ->> 'facet_code')::text,
                    (j_facet ->> 'display_title')::text,
                    (j_facet ->> 'description')::text,
                    (j_facet ->> 'facet_group_id')::int,
                    (j_facet ->> 'facet_type_id')::text::int,
                    (j_facet ->> 'category_id_expr')::text,
                    (j_facet ->> 'category_id_type')::text,
                    (j_facet ->> 'category_id_operator')::text,
                    (j_facet ->> 'category_name_expr')::text,
                    (j_facet ->> 'sort_expr')::text,
                    (j_facet ->> 'is_applicable')::boolean,
                    (j_facet ->> 'is_default')::boolean,
                    (j_facet ->> 'aggregate_type')::text,
                    (j_facet ->> 'aggregate_title')::text,
                    i_aggregate_facet_id
                ))
            on conflict (facet_id)
                do update set
                    facet_code = excluded.facet_code,
                    display_title = excluded.display_title,
                    description = excluded.description,
                    facet_group_id = excluded.facet_group_id,
                    facet_type_id = excluded.facet_type_id,
                    category_id_expr = excluded.category_id_expr,
                    category_id_type = excluded.category_id_type,
                    category_id_operator = coalesce(excluded.category_id_operator,'='),
                    category_name_expr = excluded.category_name_expr,
                    sort_expr = excluded.sort_expr,
                    is_applicable = excluded.is_applicable,
                    is_default = excluded.is_default,
                    aggregate_type = excluded.aggregate_type,
                    aggregate_title = excluded.aggregate_title,
                    aggregate_facet_id = excluded.aggregate_facet_id;

            insert into facet.facet_table (facet_id, sequence_id, table_id, udf_call_arguments, alias)
                select i_facet_id, sequence_id, table_id, udf_call_arguments, alias
                from (
                    select  (v ->> 'sequence_id')::int           as sequence_id,
                            (v ->> 'table_name')::text           as table_or_udf_name,
                            (v ->> 'udf_call_arguments')::text as udf_call_arguments,
                            (v ->> 'alias')                       as alias
                    from jsonb_array_elements(j_facet -> 'tables') as v
                ) as v(sequence_id, table_or_udf_name, udf_call_arguments, alias)
                left join facet.table t using (table_or_udf_name);

            insert into facet.facet_clause (facet_id, clause, enforce_constraint)
                select i_facet_id,
                        (v ->> 'clause')::text,
                        (v ->> 'enforce_constraint')::bool
                from jsonb_array_elements(j_facet -> 'clauses') as v;


            -- Restore domain facet associations
            insert into facet.facet_children (facet_code, child_facet_code, position)
                select facet_code, child_facet_code, position
                from _facet_children_temp;

            return j_facet;

        end $$;


--
-- Name: delete_facet(integer); Type: FUNCTION; Schema: facet; Owner: -
--

CREATE FUNCTION facet.delete_facet(i_facet_id integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
        declare j_tables json;
        declare j_clauses json;
        declare s_facet_code text;
        declare i_aggregate_facet_id int = 0;
    begin

        s_facet_code = (select max(facet_code) from facet.facet where facet_id = i_facet_id);
        raise notice 'facet code: %', i_facet_id;

        if s_facet_code is null then
            raise notice 'facet not found %', i_facet_id;
            return false;
        end if;

        delete from facet.facet_clause where facet_id = i_facet_id;
        delete from facet.facet_table where facet_id = i_facet_id;
        delete from facet.facet_children where s_facet_code in (facet_code, child_facet_code);
        delete from facet.facet_dependency where i_facet_id in (facet_id, dependency_facet_id);
        delete from facet.facet where facet_id = i_facet_id;

	return true;

end
$$;


--
-- Name: export_facets_to_json(); Type: FUNCTION; Schema: facet; Owner: -
--

CREATE FUNCTION facet.export_facets_to_json() RETURNS text
    LANGUAGE plpgsql
    AS $_X$

        declare json_template text;
        declare json_table_template text;
        declare json_clause_template text;
        declare json_facet text;
        declare json_facets text;
        declare json_table text;
        declare json_clause text;
        declare r_facet record;
        declare r_facet_table record;
        declare r_facet_clause record;

        begin

            json_template = $_$
            {
                "facet_id": %s,
                "facet_code": "%s",
                "display_title": "%s",
                "description": "%s",
                "facet_group_id":"%s",
                "facet_type_id": %s,
                "category_id_expr": "%s",
                "category_id_type": "%s",
                "category_id_operator": "%s",
                "category_name_expr": "%s",
                "sort_expr": "%s",
                "is_applicable": %s,
                "is_default": %s,
                "aggregate_type": "%s",
                "aggregate_title": "%s",
                "aggregate_facet_code": %s,
                "tables": [ %s ],
                "clauses": [ %s ]
            }$_$;

            json_table_template = $_$
                    {
                        "sequence_id": %s,
                        "table_name": "%s",
                        "udf_call_arguments": %s,
                        "alias":  %s
                    }$_$;

            json_clause_template = $_$
                    {
                        "clause": "%s",
                        "enforce_constraint": %s
                    }$_$;

            json_facets = null;

            FOR r_facet in
                SELECT f.*, af.facet_code as aggregate_facet_code
                FROM facet.facet f
                LEFT JOIN facet.facet af
                  ON af.facet_id = f.aggregate_facet_id
                ORDER BY f.facet_id
            LOOP

                SELECT string_agg(
                    format(json_table_template,
                        ft.sequence_id,
                        t.table_or_udf_name,
                        coalesce('"' || ft.udf_call_arguments || '"', 'null'),
                        coalesce('"' || ft.alias || '"', 'null')), ','
                        ORDER BY sequence_id
                )
                    INTO json_table
                FROM facet.facet_table ft
                JOIN facet.table t using (table_id)
                WHERE facet_id = r_facet.facet_id
                GROUP BY facet_id;

                SELECT string_agg(
                            format(json_clause_template,
                                clause,
                                case when enforce_constraint = TRUE then 'true' else 'false' end
                            ), ',')
                    INTO json_clause
                FROM facet.facet_clause
                WHERE facet_id = r_facet.facet_id
                GROUP BY facet_id;

                json_facet = format(json_template,
                    r_facet.facet_id,
                    r_facet.facet_code,
                    r_facet.display_title,
                    r_facet.description,
                    r_facet.facet_group_id,
                    r_facet.facet_type_id,
                    r_facet.category_id_expr,
                    r_facet.category_id_type,
                    r_facet.category_id_operator,
                    r_facet.category_name_expr,
                    r_facet.sort_expr,
                    case when r_facet.is_applicable = TRUE then 'true' else 'false' end,
                    case when r_facet.is_default = TRUE then 'true' else 'false' end,
                    r_facet.aggregate_type,
                    r_facet.aggregate_title,
                    coalesce('"' || r_facet.aggregate_facet_code || '"', 'null'),
                    json_table,
                    json_clause
                );

                json_facets = coalesce(json_facets || ', ', '') || json_facet;
            END LOOP;
            json_facets = '[' || json_facets || ']';
            raise notice '%', json_facets;
            return json_facets;
        end $_X$;


--
-- Name: method_abundance(integer); Type: FUNCTION; Schema: facet; Owner: -
--

CREATE FUNCTION facet.method_abundance(p_dataset_method_id integer) RETURNS TABLE(taxon_id integer, analysis_entity_id integer, abundance numeric)
    LANGUAGE plpgsql
    AS $$
begin

	 return query
		select a.taxon_id, a.analysis_entity_id, a.abundance
		from tbl_abundances a
		join tbl_analysis_entities ae
		  on ae.analysis_entity_id = a.analysis_entity_id
		join tbl_datasets ds
		  on ds.method_id = p_dataset_method_id;

end $$;


--
-- Name: method_measured_values(integer, integer); Type: FUNCTION; Schema: facet; Owner: -
--

CREATE FUNCTION facet.method_measured_values(p_dataset_method_id integer, p_prep_method_id integer) RETURNS TABLE(analysis_entity_id integer, measured_value numeric)
    LANGUAGE plpgsql
    AS $$
begin

    return query
        select mv.analysis_entity_id::int, mv.measured_value
        from tbl_measured_values mv
        join tbl_analysis_entities ae using (analysis_entity_id)
        join tbl_datasets ds using (dataset_id)
        left join tbl_analysis_entity_prep_methods pm using (analysis_entity_id)
        where ds.method_id = p_dataset_method_id
          and coalesce(pm.method_id, 0) = p_prep_method_id;

end $$;


--
-- Name: abundance_taxon_shortcut; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.abundance_taxon_shortcut AS
 WITH analysis AS (
         SELECT ae.analysis_entity_id,
            m.method_name
           FROM ((public.tbl_analysis_entities ae
             JOIN public.tbl_datasets d USING (dataset_id))
             JOIN public.tbl_methods m USING (method_id))
        ), modification AS (
         SELECT am.abundance_id,
            mt.modification_type_name
           FROM (public.tbl_abundance_modifications am
             JOIN public.tbl_modification_types mt USING (modification_type_id))
        ), taxon AS (
         SELECT t.taxon_id,
            concat_ws(' '::text, g.genus_name, t.species, a.author_name) AS taxon_name
           FROM ((public.tbl_taxa_tree_master t
             JOIN public.tbl_taxa_tree_genera g USING (genus_id))
             LEFT JOIN public.tbl_taxa_tree_authors a USING (author_id))
        )
 SELECT abundance.analysis_entity_id,
    abundance.abundance_id,
    abundance.taxon_id,
    taxon.taxon_name,
    concat_ws(' '::text, analysis.method_name, modification.modification_type_name) AS elements_part_mod,
    abundance.abundance
   FROM (((public.tbl_abundances abundance
     JOIN taxon USING (taxon_id))
     JOIN analysis USING (analysis_entity_id))
     LEFT JOIN modification USING (abundance_id));


SET default_table_access_method = heap;

--
-- Name: facet; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet (
    facet_id integer NOT NULL,
    facet_code character varying(80) NOT NULL,
    display_title character varying(80) NOT NULL,
    description character varying(256) DEFAULT ''::character varying NOT NULL,
    facet_group_id integer NOT NULL,
    facet_type_id integer NOT NULL,
    category_id_expr character varying(256) NOT NULL,
    category_id_type character varying(80) DEFAULT 'integer'::character varying NOT NULL,
    category_name_expr character varying(256) NOT NULL,
    sort_expr character varying(256) NOT NULL,
    is_applicable boolean NOT NULL,
    is_default boolean NOT NULL,
    aggregate_type character varying(256) NOT NULL,
    aggregate_title character varying(256) NOT NULL,
    aggregate_facet_id integer NOT NULL,
    category_id_operator character varying(40) DEFAULT '='::character varying NOT NULL
);


--
-- Name: facet_children; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_children (
    facet_code character varying NOT NULL,
    child_facet_code character varying NOT NULL,
    "position" integer DEFAULT 0 NOT NULL
);


--
-- Name: facet_clause; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_clause (
    facet_clause_id integer NOT NULL,
    facet_id integer NOT NULL,
    clause character varying(512),
    enforce_constraint boolean DEFAULT false NOT NULL
);


--
-- Name: facet_clause_facet_clause_id_seq; Type: SEQUENCE; Schema: facet; Owner: -
--

CREATE SEQUENCE facet.facet_clause_facet_clause_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: facet_clause_facet_clause_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.facet_clause_facet_clause_id_seq OWNED BY facet.facet_clause.facet_clause_id;


--
-- Name: facet_dependency; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_dependency (
    facet_dependency_id integer NOT NULL,
    facet_id integer NOT NULL,
    dependency_facet_id integer NOT NULL
);


--
-- Name: facet_dependency_facet_dependency_id_seq; Type: SEQUENCE; Schema: facet; Owner: -
--

CREATE SEQUENCE facet.facet_dependency_facet_dependency_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: facet_dependency_facet_dependency_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.facet_dependency_facet_dependency_id_seq OWNED BY facet.facet_dependency.facet_dependency_id;


--
-- Name: facet_group; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_group (
    facet_group_id integer NOT NULL,
    facet_group_key character varying(80) NOT NULL,
    display_title character varying(80) NOT NULL,
    description character varying(256) DEFAULT ''::character varying NOT NULL,
    is_applicable boolean NOT NULL,
    is_default boolean NOT NULL
);


--
-- Name: facet_table; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_table (
    facet_table_id integer NOT NULL,
    facet_id integer NOT NULL,
    sequence_id integer NOT NULL,
    table_id integer NOT NULL,
    udf_call_arguments character varying(80),
    alias character varying(80)
);


--
-- Name: facet_table_facet_table_id_seq; Type: SEQUENCE; Schema: facet; Owner: -
--

CREATE SEQUENCE facet.facet_table_facet_table_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: facet_table_facet_table_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.facet_table_facet_table_id_seq OWNED BY facet.facet_table.facet_table_id;


--
-- Name: facet_type; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_type (
    facet_type_id integer NOT NULL,
    facet_type_name character varying(80) NOT NULL,
    reload_as_target boolean DEFAULT false NOT NULL
);


--
-- Name: geochronology_taxa_shortcut; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.geochronology_taxa_shortcut AS
 WITH geochronology_taxa AS (
         SELECT g.geochron_id,
            t.taxon_id,
            aeg.analysis_entity_id
           FROM (((((public.tbl_physical_samples ps
             JOIN public.tbl_analysis_entities aea USING (physical_sample_id))
             JOIN public.tbl_abundances a ON ((a.analysis_entity_id = aea.analysis_entity_id)))
             JOIN public.tbl_taxa_tree_master t USING (taxon_id))
             JOIN public.tbl_analysis_entities aeg USING (physical_sample_id))
             JOIN public.tbl_geochronology g ON ((g.analysis_entity_id = aeg.analysis_entity_id)))
        )
 SELECT analysis_entity_id,
    taxon_id
   FROM geochronology_taxa;


--
-- Name: table; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet."table" (
    table_id integer NOT NULL,
    schema_name information_schema.sql_identifier DEFAULT ''::name NOT NULL,
    table_or_udf_name information_schema.sql_identifier NOT NULL,
    primary_key_name information_schema.sql_identifier DEFAULT ''::name NOT NULL,
    is_udf boolean DEFAULT false NOT NULL
);


--
-- Name: table_relation; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.table_relation (
    table_relation_id integer NOT NULL,
    source_table_id integer NOT NULL,
    target_table_id integer NOT NULL,
    weight integer DEFAULT 0 NOT NULL,
    source_column_name information_schema.sql_identifier NOT NULL,
    target_column_name information_schema.sql_identifier NOT NULL
);


--
-- Name: relation_weight; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.relation_weight AS
 SELECT r.table_relation_id,
    r.source_table_id,
    s.table_or_udf_name AS source_table,
    r.target_table_id,
    t.table_or_udf_name AS target_table,
    r.weight
   FROM ((facet.table_relation r
     JOIN facet."table" s ON ((s.table_id = r.source_table_id)))
     JOIN facet."table" t ON ((t.table_id = r.target_table_id)));


--
-- Name: report_site; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.report_site AS
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


--
-- Name: result_specification_field; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.result_specification_field (
    specification_field_id integer NOT NULL,
    specification_id integer NOT NULL,
    result_field_id integer NOT NULL,
    field_type_id character varying(40) DEFAULT 'single_item'::character varying NOT NULL,
    sequence_id integer DEFAULT 0 NOT NULL
);


--
-- Name: result_aggregate_field_aggregate_field_id_seq; Type: SEQUENCE; Schema: facet; Owner: -
--

CREATE SEQUENCE facet.result_aggregate_field_aggregate_field_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: result_aggregate_field_aggregate_field_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.result_aggregate_field_aggregate_field_id_seq OWNED BY facet.result_specification_field.specification_field_id;


--
-- Name: result_field; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.result_field (
    result_field_id integer NOT NULL,
    result_field_key character varying(40) NOT NULL,
    table_name character varying(80) NOT NULL,
    column_name character varying(80) NOT NULL,
    display_text character varying(80) NOT NULL,
    field_type_id character varying(20) NOT NULL,
    activated boolean NOT NULL,
    link_url character varying(256),
    link_label character varying(256),
    datatype character varying(40) DEFAULT 'text'::character varying NOT NULL
);


--
-- Name: result_field_result_field_id_seq; Type: SEQUENCE; Schema: facet; Owner: -
--

CREATE SEQUENCE facet.result_field_result_field_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: result_field_result_field_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.result_field_result_field_id_seq OWNED BY facet.result_field.result_field_id;


--
-- Name: result_field_type; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.result_field_type (
    field_type_id character varying(40) NOT NULL,
    is_result_value boolean DEFAULT true NOT NULL,
    sql_field_compiler character varying(40) DEFAULT ''::character varying NOT NULL,
    is_aggregate_field boolean DEFAULT false NOT NULL,
    is_sort_field boolean DEFAULT false NOT NULL,
    is_item_field boolean DEFAULT false NOT NULL,
    sql_template character varying(256) DEFAULT '{0}'::character varying NOT NULL
);


--
-- Name: result_specification; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.result_specification (
    specification_id integer NOT NULL,
    specification_key character varying(40) NOT NULL,
    display_text character varying(80) NOT NULL,
    is_applicable boolean DEFAULT false NOT NULL,
    is_activated boolean DEFAULT true NOT NULL
);


--
-- Name: result_view_type; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.result_view_type (
    view_type_id character varying(40) NOT NULL,
    view_name character varying(40) NOT NULL,
    is_cachable boolean DEFAULT true NOT NULL,
    result_facet_code character varying(40) DEFAULT ''::character varying NOT NULL,
    sql_compiler character varying(80) DEFAULT ''::character varying NOT NULL,
    specification_key character varying(40) DEFAULT ''::character varying NOT NULL
);


--
-- Name: sample_group_construction_purposes; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.sample_group_construction_purposes AS
 WITH purposes(purpose) AS (
         SELECT DISTINCT tbl_sample_group_descriptions.group_description AS purpose
           FROM public.tbl_sample_group_descriptions
          WHERE (tbl_sample_group_descriptions.sample_group_description_type_id = 62)
          ORDER BY tbl_sample_group_descriptions.group_description
        )
 SELECT row_number() OVER () AS purpose_id,
    purpose
   FROM purposes;


--
-- Name: site_location_shortcut; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.site_location_shortcut AS
 SELECT sl.site_id,
    l.location_id,
    l.location_name,
    l.location_type_id,
    l.default_lat_dd,
    l.default_long_dd,
    l.date_updated
   FROM (public.tbl_site_locations sl
     JOIN public.tbl_locations l USING (location_id));


--
-- Name: table_relation_table_relation_id_seq; Type: SEQUENCE; Schema: facet; Owner: -
--

CREATE SEQUENCE facet.table_relation_table_relation_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: table_relation_table_relation_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.table_relation_table_relation_id_seq OWNED BY facet.table_relation.table_relation_id;


--
-- Name: view_abundance; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.view_abundance AS
 WITH analysis AS (
         SELECT tbl_analysis_entities.analysis_entity_id,
            tbl_methods.method_name
           FROM ((public.tbl_analysis_entities
             JOIN public.tbl_datasets USING (dataset_id))
             JOIN public.tbl_methods USING (method_id))
        ), modification AS (
         SELECT tbl_abundance_modifications.abundance_id,
            tbl_modification_types.modification_type_name
           FROM (public.tbl_abundance_modifications
             JOIN public.tbl_modification_types USING (modification_type_id))
        )
 SELECT abundance.analysis_entity_id,
    abundance.taxon_id,
    format('%s %s'::text, analysis.method_name, COALESCE(modification.modification_type_name, ''::character varying)) AS elements_part_mod,
    abundance.abundance
   FROM ((public.tbl_abundances abundance
     JOIN analysis USING (analysis_entity_id))
     LEFT JOIN modification USING (abundance_id));


--
-- Name: view_abundances_by_taxon_analysis_entity; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.view_abundances_by_taxon_analysis_entity AS
 WITH method_abundance AS (
         SELECT ds.method_id,
            a_1.taxon_id,
            a_1.analysis_entity_id,
            a_1.abundance
           FROM ((public.tbl_abundances a_1
             LEFT JOIN public.tbl_analysis_entities ae ON ((ae.analysis_entity_id = a_1.analysis_entity_id)))
             JOIN public.tbl_datasets ds ON ((ae.dataset_id = ds.dataset_id)))
        )
 SELECT DISTINCT a.taxon_id,
    a.analysis_entity_id,
    m3.abundance AS abundance_m3,
    m8.abundance AS abundance_m8,
    m111.abundance AS abundance_m111
   FROM (((public.tbl_abundances a
     LEFT JOIN method_abundance m3 ON (((m3.method_id = 3) AND (m3.taxon_id = a.taxon_id) AND (m3.analysis_entity_id = a.analysis_entity_id))))
     LEFT JOIN method_abundance m8 ON (((m8.method_id = 8) AND (m8.taxon_id = a.taxon_id) AND (m8.analysis_entity_id = a.analysis_entity_id))))
     LEFT JOIN method_abundance m111 ON (((m111.method_id = 111) AND (m111.taxon_id = a.taxon_id) AND (m111.analysis_entity_id = a.analysis_entity_id))));


--
-- Name: view_sample_group_references; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.view_sample_group_references AS
 SELECT tbl_sample_group_references.sample_group_id,
    tbl_sample_group_references.biblio_id,
    tbl_sample_group_references.date_updated,
    'sample_group'::text AS biblio_link
   FROM public.tbl_sample_group_references
UNION
 SELECT tbl_sample_groups.sample_group_id,
    tbl_site_references.biblio_id,
    tbl_site_references.date_updated,
    'indirect_via_site'::text AS biblio_link
   FROM ((public.tbl_site_references
     JOIN public.tbl_sites ON ((tbl_site_references.site_id = tbl_sites.site_id)))
     JOIN public.tbl_sample_groups ON ((tbl_sample_groups.site_id = tbl_sites.site_id)))
  ORDER BY 1;


--
-- Name: view_site_references; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.view_site_references AS
 SELECT tbl_site_references.site_id,
    tbl_site_references.biblio_id,
    tbl_site_references.date_updated,
    'site_direct'::text AS biblio_link
   FROM public.tbl_site_references
UNION
 SELECT s.site_id,
    sgr.biblio_id,
    sgr.date_updated,
    'via_sample_group'::text AS biblio_link
   FROM ((public.tbl_sample_group_references sgr
     JOIN public.tbl_sample_groups sg ON ((sg.sample_group_id = sgr.sample_group_id)))
     JOIN public.tbl_sites s ON ((sg.site_id = s.site_id)))
  ORDER BY 1;


--
-- Name: view_state; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.view_state (
    view_state_key character varying(80) NOT NULL,
    view_state_data text NOT NULL,
    create_time timestamp with time zone DEFAULT clock_timestamp()
);


--
-- Name: view_taxa_biblio; Type: VIEW; Schema: facet; Owner: -
--

CREATE VIEW facet.view_taxa_biblio AS
 SELECT tbl_text_distribution.biblio_id,
    tbl_text_distribution.taxon_id
   FROM public.tbl_text_distribution
UNION
 SELECT tbl_text_biology.biblio_id,
    tbl_text_biology.taxon_id
   FROM public.tbl_text_biology
UNION
 SELECT tbl_taxonomy_notes.biblio_id,
    tbl_taxonomy_notes.taxon_id
   FROM public.tbl_taxonomy_notes;


--
-- Name: facet_clause facet_clause_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_clause ALTER COLUMN facet_clause_id SET DEFAULT nextval('facet.facet_clause_facet_clause_id_seq'::regclass);


--
-- Name: facet_dependency facet_dependency_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency ALTER COLUMN facet_dependency_id SET DEFAULT nextval('facet.facet_dependency_facet_dependency_id_seq'::regclass);


--
-- Name: facet_table facet_table_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table ALTER COLUMN facet_table_id SET DEFAULT nextval('facet.facet_table_facet_table_id_seq'::regclass);


--
-- Name: result_field result_field_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field ALTER COLUMN result_field_id SET DEFAULT nextval('facet.result_field_result_field_id_seq'::regclass);


--
-- Name: result_specification_field specification_field_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field ALTER COLUMN specification_field_id SET DEFAULT nextval('facet.result_aggregate_field_aggregate_field_id_seq'::regclass);


--
-- Name: table_relation table_relation_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation ALTER COLUMN table_relation_id SET DEFAULT nextval('facet.table_relation_table_relation_id_seq'::regclass);


--
-- Data for Name: facet; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.facet VALUES (1, 'result_facet', 'Analysis entities', 'Analysis entities', 99, 1, 'tbl_analysis_entities.analysis_entity_id', 'integer', 'tbl_physical_samples.sample_name||'' ''||tbl_datasets.dataset_name', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of samples', 0, '=');
INSERT INTO facet.facet VALUES (32, 'abundances_all_helper', 'Abundances', 'Abundances', 4, 2, 'facet.view_abundance.abundance', 'integer', 'facet.view_abundance.abundance', 'facet.view_abundance.abundance', false, false, '', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (40, 'result_datasets', 'Datasets', 'Datasets', 99, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 0, '=');
INSERT INTO facet.facet VALUES (19, 'sites_helper', 'Site', 'Report helper', 2, 1, 'tbl_sites.site_id', 'integer', 'tbl_sites.site_name', 'tbl_sites.site_name', false, true, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (3, 'tbl_denormalized_measured_values_33_0', 'Magnetic sus.', 'Magnetic sus.', 5, 2, 'method_values_33.measured_value', 'integer', 'method_values_33.measured_value', 'method_values_33.measured_value', true, false, '', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (4, 'tbl_denormalized_measured_values_33_82', 'MS Heating 550', 'MS Heating 550', 5, 2, 'method_values_33_82.measured_value', 'integer', 'method_values_33_82.measured_value', 'method_values_33_82.measured_value', true, false, '', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (5, 'tbl_denormalized_measured_values_32', 'Loss on Ignition', 'Loss of Ignition', 5, 2, 'method_values_32.measured_value', 'numeric(20,5)', 'method_values_32.measured_value', 'method_values_32.measured_value', true, false, '', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (6, 'tbl_denormalized_measured_values_37', 'Phosphates', 'Phosphates', 5, 2, 'method_values_37.measured_value', 'numeric(20,5)', 'method_values_37.measured_value', 'method_values_37.measured_value', true, false, '', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (9, 'map_result', 'Site', 'Site', 99, 1, 'tbl_sites.site_id', 'integer', 'tbl_sites.site_name', 'tbl_sites.site_name', false, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (11, 'relative_age_name', 'Time periods', 'Age of sample as defined by association with a (often regionally specific) cultural or geological period (in years before present)', 2, 1, 'tbl_relative_ages.relative_age_id', 'integer', 'tbl_relative_ages.relative_age_name', 'tbl_relative_ages.relative_age_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (12, 'record_types', 'Proxy types', 'Proxy types', 1, 1, 'tbl_record_types.record_type_id', 'integer', 'tbl_record_types.record_type_name', 'tbl_record_types.record_type_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (13, 'sample_groups', 'Sample groups', 'A collection of samples, usually defined by the excavator or collector', 2, 1, 'tbl_sample_groups.sample_group_id', 'integer', 'tbl_sites.site_name || '' '' || tbl_sample_groups.sample_group_name', 'tbl_sample_groups.sample_group_name', true, true, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (18, 'sites', 'Site', 'General name for the excavation or sampling location', 2, 1, 'tbl_sites.site_id', 'integer', 'tbl_sites.site_name', 'tbl_sites.site_name', true, true, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (22, 'ecocode', 'Eco code', 'Ecological category (trait) or cultural relevance of organisms based on a classification system', 4, 1, 'tbl_ecocode_definitions.ecocode_definition_id', 'integer', 'tbl_ecocode_definitions.name', 'tbl_ecocode_definitions.name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (23, 'family', 'Family', 'Taxonomic family', 6, 1, 'tbl_taxa_tree_families.family_id', 'integer', 'tbl_taxa_tree_families.family_name ', 'tbl_taxa_tree_families.family_name ', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (24, 'genus', 'Genus', 'Taxonomic genus (under family)', 6, 1, 'tbl_taxa_tree_genera.genus_id', 'integer', 'tbl_taxa_tree_genera.genus_name', 'tbl_taxa_tree_genera.genus_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (28, 'species_author', 'Author', 'Authority of the taxonomic name (not used for all species)', 6, 1, 'tbl_taxa_tree_authors.author_id ', 'integer', 'tbl_taxa_tree_authors.author_name ', 'tbl_taxa_tree_authors.author_name ', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (29, 'feature_type', 'Feature type', 'Feature type', 1, 1, 'tbl_feature_types.feature_type_id ', 'integer', 'tbl_feature_types.feature_type_name', 'tbl_feature_types.feature_type_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (30, 'ecocode_system', 'Eco code system', 'Ecological or cultural organism classification system (which groups items in the ecological/cultural category filter)', 4, 1, 'tbl_ecocode_systems.ecocode_system_id ', 'integer', 'tbl_ecocode_systems.name', 'tbl_ecocode_systems.definition', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (31, 'abundance_classification', 'abundance classification', 'abundance classification', 4, 1, 'facet.view_abundance.elements_part_mod ', 'text', 'facet.view_abundance.elements_part_mod ', 'facet.view_abundance.elements_part_mod ', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (33, 'abundances_all', 'Abundances', 'Abundances', 4, 2, 'facet.view_abundance.abundance', 'integer', 'facet.view_abundance.abundance', 'facet.view_abundance.abundance', true, false, '', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (34, 'activeseason', 'Insect activity seasons', 'Insect activity seasons', 2, 1, 'tbl_seasons.season_id', 'integer', 'tbl_seasons.season_name', 'tbl_seasons.season_type ', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (35, 'tbl_biblio_modern', 'Bibliography modern', 'Bibliography modern', 1, 1, 'facet.view_taxa_biblio.biblio_id', 'integer', 'tbl_biblio.title||''  ''||tbl_biblio.authors ', 'tbl_biblio.authors', true, false, 'count', 'count of species', 19, '=');
INSERT INTO facet.facet VALUES (36, 'tbl_biblio_sample_groups', 'Bibliography sites/Samplegroups', 'Bibliography sites/Samplegroups', 1, 1, 'tbl_biblio.biblio_id', 'integer', 'tbl_biblio.title||''  ''||tbl_biblio.authors', 'tbl_biblio.authors', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (37, 'tbl_biblio_sites', 'Bibliography sites', 'Bibliography sites', 1, 1, 'tbl_biblio.biblio_id', 'integer', 'tbl_biblio.title||''  ''||tbl_biblio.authors', 'tbl_biblio.authors', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (38, 'dataset_provider', 'Dataset provider', 'Dataset provider', 2, 1, 'tbl_dataset_masters.master_set_id ', 'integer', 'tbl_dataset_masters.master_name', 'tbl_dataset_masters.master_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (39, 'dataset_methods', 'Dataset methods', 'Dataset methods', 2, 1, 'tbl_methods.method_id ', 'integer', 'tbl_methods.method_name', 'tbl_methods.method_name', true, false, 'count', 'Number of datasets', 40, '=');
INSERT INTO facet.facet VALUES (42, 'data_types', 'Data types', 'Data types', 1, 1, 'tbl_data_types.data_type_id', 'integer', 'tbl_data_types.data_type_name', 'tbl_data_types.data_type_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (44, 'rdb_codes', 'RDB Code', 'RDB Code', 1, 1, 'tbl_rdb_codes.rdb_code_id', 'integer', 'tbl_rdb_codes.rdb_definition', 'tbl_rdb_codes.rdb_definition', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (45, 'modification_types', 'Modification Types', 'Modification Types', 1, 1, 'tbl_modification_types.modification_type_id', 'integer', 'tbl_modification_types.modification_type_name', 'tbl_modification_types.modification_type_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (46, 'abundance_elements', 'Abundance Elements', 'Abundance Elements', 1, 1, 'tbl_abundance_elements.abundance_element_id', 'integer', 'tbl_abundance_elements.element_name', 'tbl_abundance_elements.element_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (47, 'sample_group_sampling_contexts', 'Sampling Contexts', 'Sampling Contexts', 1, 1, 'tbl_sample_group_sampling_contexts.sampling_context_id', 'integer', 'tbl_sample_group_sampling_contexts.sampling_context', 'tbl_sample_group_sampling_contexts.sort_order', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (48, 'constructions', 'Constructions', 'Constructions', 1, 1, 'tbl_sample_group_descriptions.sample_group_description_id', 'integer', 'tbl_sample_group_descriptions.group_description ', 'tbl_sample_group_descriptions.group_description || '''' '''' || tbl_sample_groups.sample_group_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (1001, 'palaeoentomology', 'Palaeoentomology', 'Palaeoentomology domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');
INSERT INTO facet.facet VALUES (1002, 'archaeobotany', 'Archaeobotany', 'Archaeobotany domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');
INSERT INTO facet.facet VALUES (1003, 'pollen', 'Pollen', 'Pollen domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');
INSERT INTO facet.facet VALUES (1004, 'geoarchaeology', 'Geoarchaeology', 'Geoarchaeology domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');
INSERT INTO facet.facet VALUES (1005, 'dendrochronology', 'Dendrochronology', 'Dendrochronology domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');
INSERT INTO facet.facet VALUES (1006, 'ceramic', 'Ceramic', 'Ceramic domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');
INSERT INTO facet.facet VALUES (1007, 'isotope', 'Isotope', 'Isotope domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');
INSERT INTO facet.facet VALUES (21, 'country', 'Countries', 'The name of the country, at the time of collection, in which the samples were collected', 2, 1, 'countries.location_id', 'integer', 'countries.location_name ', 'countries.location_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (41, 'region', 'Region', 'Region', 2, 1, 'region.location_id ', 'integer', 'region.location_name', 'region.location_name', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (50, 'location_types', 'Location type', 'Type of location', 2, 1, 'tbl_location_types.location_type_id', 'integer', 'tbl_location_types.location_type', 'tbl_location_types.location_type', true, true, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (51, 'sites_polygon', 'Sites (map)', 'General name for the excavation or sampling location', 2, 3, 'tbl_sites.site_id', 'integer', 'tbl_sites.site_name', 'tbl_sites.site_name', true, true, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (25, 'species', 'Taxa', 'Taxonomic species (under genus)', 6, 1, 'facet.abundance_taxon_shortcut.taxon_id', 'integer', 'facet.abundance_taxon_shortcut.taxon_name', '1', true, false, 'count', 'Count of Anaylysis', 1, '=');
INSERT INTO facet.facet VALUES (54, 'construction_purpose', 'Construction purpose', 'Construction purpose', 1, 1, 'facet.sample_group_construction_purposes.purpose_id', 'text', 'facet.sample_group_construction_purposes.purpose', '1', true, false, 'count', 'Count of Analysis', 1, '=');
INSERT INTO facet.facet VALUES (52, 'analysis_entity_ages', 'Analysis entity ages', 'Analysis entity ages (intersects)', 2, 4, 'age_range', 'int4range', 'age_range', '1', true, true, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (53, 'dendro_age_contained_by', 'Dendrochronology ages', 'Generic age range filter for dendrchronology. Return both estimated felling year and outermost tree ring.', 2, 4, 'tbl_dendro_dates.age_range', 'int4range', 'tbl_dendro_dates.age_range', '1', true, true, 'count', 'Number of samples', 1, '@>');
INSERT INTO facet.facet VALUES (43, 'rdb_systems', 'RDB system', 'RDB system', 1, 1, 'tbl_rdb_systems.rdb_system_id', 'integer', 'concat_ws('' '', tbl_rdb_systems.rdb_system, tbl_locations.location_name)', 'tbl_rdb_systems.rdb_system', true, false, 'count', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (10, 'geochronology', 'Geochronology', 'Sample ages as retrieved through absolute methods such as radiocarbon dating or other radiometric methods (in method based years before present - e.g. 14C years)', 2, 2, 'tbl_geochronology.age', 'integer', 'tbl_geochronology.age', 'tbl_geochronology.age', true, false, '', 'Number of samples', 1, '=');
INSERT INTO facet.facet VALUES (1008, 'adna', 'aDNA', 'Ancient DNA domain facet', 999, 1, 'tbl_datasets.dataset_id', 'integer', 'tbl_datasets.dataset_name ', 'tbl_datasets.dataset_name', false, false, 'count', 'Number of datasets', 1, '=');


--
-- Data for Name: facet_children; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'country', 14);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'country', 14);
INSERT INTO facet.facet_children VALUES ('pollen', 'country', 14);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'country', 8);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'country', 10);
INSERT INTO facet.facet_children VALUES ('ceramic', 'country', 10);
INSERT INTO facet.facet_children VALUES ('isotope', 'country', 10);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'species', 9);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'species', 9);
INSERT INTO facet.facet_children VALUES ('pollen', 'species', 9);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'species', 5);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'rdb_systems', 17);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'location_types', 22);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'location_types', 24);
INSERT INTO facet.facet_children VALUES ('pollen', 'location_types', 24);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'location_types', 16);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'location_types', 22);
INSERT INTO facet.facet_children VALUES ('ceramic', 'location_types', 22);
INSERT INTO facet.facet_children VALUES ('isotope', 'location_types', 22);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'construction_purpose', 22);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'geochronology', 4);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'geochronology', 4);
INSERT INTO facet.facet_children VALUES ('pollen', 'geochronology', 4);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'geochronology', 3);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'geochronology', 1);
INSERT INTO facet.facet_children VALUES ('ceramic', 'geochronology', 1);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'relative_age_name', 5);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'sample_groups', 16);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'sites', 15);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'ecocode', 2);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'family', 7);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'genus', 8);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'species_author', 10);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'feature_type', 11);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'ecocode_system', 1);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'abundances_all', 3);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'activeseason', 6);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'tbl_biblio_modern', 12);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'data_types', 21);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'rdb_codes', 18);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'sample_group_sampling_contexts', 20);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'relative_age_name', 5);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'sample_groups', 16);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'sites', 15);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'ecocode', 2);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'family', 7);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'genus', 8);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'species_author', 10);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'feature_type', 11);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'ecocode_system', 1);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'abundances_all', 3);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'activeseason', 6);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'tbl_biblio_modern', 12);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'data_types', 21);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'modification_types', 22);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'abundance_elements', 23);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'sample_group_sampling_contexts', 20);
INSERT INTO facet.facet_children VALUES ('pollen', 'relative_age_name', 5);
INSERT INTO facet.facet_children VALUES ('pollen', 'sample_groups', 16);
INSERT INTO facet.facet_children VALUES ('pollen', 'sites', 15);
INSERT INTO facet.facet_children VALUES ('pollen', 'ecocode', 2);
INSERT INTO facet.facet_children VALUES ('pollen', 'family', 7);
INSERT INTO facet.facet_children VALUES ('pollen', 'genus', 8);
INSERT INTO facet.facet_children VALUES ('pollen', 'species_author', 10);
INSERT INTO facet.facet_children VALUES ('pollen', 'feature_type', 11);
INSERT INTO facet.facet_children VALUES ('pollen', 'ecocode_system', 1);
INSERT INTO facet.facet_children VALUES ('pollen', 'abundances_all', 3);
INSERT INTO facet.facet_children VALUES ('pollen', 'activeseason', 6);
INSERT INTO facet.facet_children VALUES ('pollen', 'tbl_biblio_modern', 12);
INSERT INTO facet.facet_children VALUES ('pollen', 'data_types', 21);
INSERT INTO facet.facet_children VALUES ('pollen', 'abundance_elements', 23);
INSERT INTO facet.facet_children VALUES ('pollen', 'sample_group_sampling_contexts', 20);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'tbl_denormalized_measured_values_33_0', 15);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'tbl_denormalized_measured_values_33_82', 1);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'tbl_denormalized_measured_values_32', 14);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'tbl_denormalized_measured_values_37', 2);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'relative_age_name', 4);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'sample_groups', 10);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'sites', 9);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'feature_type', 5);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'tbl_biblio_modern', 6);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'data_types', 13);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'sample_group_sampling_contexts', 12);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'relative_age_name', 2);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'sample_groups', 12);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'sites', 11);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'family', 3);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'genus', 4);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'species_author', 6);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'feature_type', 7);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'tbl_biblio_modern', 8);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'data_types', 21);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'sample_group_sampling_contexts', 20);
INSERT INTO facet.facet_children VALUES ('ceramic', 'relative_age_name', 2);
INSERT INTO facet.facet_children VALUES ('ceramic', 'sample_groups', 12);
INSERT INTO facet.facet_children VALUES ('ceramic', 'sites', 11);
INSERT INTO facet.facet_children VALUES ('ceramic', 'feature_type', 7);
INSERT INTO facet.facet_children VALUES ('ceramic', 'tbl_biblio_modern', 8);
INSERT INTO facet.facet_children VALUES ('ceramic', 'data_types', 21);
INSERT INTO facet.facet_children VALUES ('ceramic', 'sample_group_sampling_contexts', 20);
INSERT INTO facet.facet_children VALUES ('isotope', 'relative_age_name', 2);
INSERT INTO facet.facet_children VALUES ('isotope', 'sample_groups', 12);
INSERT INTO facet.facet_children VALUES ('isotope', 'sites', 11);
INSERT INTO facet.facet_children VALUES ('isotope', 'feature_type', 7);
INSERT INTO facet.facet_children VALUES ('isotope', 'tbl_biblio_modern', 8);
INSERT INTO facet.facet_children VALUES ('isotope', 'data_types', 21);
INSERT INTO facet.facet_children VALUES ('isotope', 'sample_group_sampling_contexts', 20);
INSERT INTO facet.facet_children VALUES ('geoarchaeology', 'sites_polygon', 99);
INSERT INTO facet.facet_children VALUES ('archaeobotany', 'sites_polygon', 99);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'sites_polygon', 99);
INSERT INTO facet.facet_children VALUES ('palaeoentomology', 'sites_polygon', 99);
INSERT INTO facet.facet_children VALUES ('isotope', 'sites_polygon', 99);
INSERT INTO facet.facet_children VALUES ('pollen', 'sites_polygon', 99);
INSERT INTO facet.facet_children VALUES ('ceramic', 'sites_polygon', 99);
INSERT INTO facet.facet_children VALUES ('dendrochronology', 'dendro_age_contained_by', 100);
INSERT INTO facet.facet_children VALUES ('adna', 'relative_age_name', 20);
INSERT INTO facet.facet_children VALUES ('adna', 'record_types', 21);
INSERT INTO facet.facet_children VALUES ('adna', 'sample_groups', 17);
INSERT INTO facet.facet_children VALUES ('adna', 'sites', 15);
INSERT INTO facet.facet_children VALUES ('adna', 'feature_type', 11);
INSERT INTO facet.facet_children VALUES ('adna', 'activeseason', 6);
INSERT INTO facet.facet_children VALUES ('adna', 'tbl_biblio_modern', 12);
INSERT INTO facet.facet_children VALUES ('adna', 'dataset_provider', 22);
INSERT INTO facet.facet_children VALUES ('adna', 'data_types', 19);
INSERT INTO facet.facet_children VALUES ('adna', 'sample_group_sampling_contexts', 18);
INSERT INTO facet.facet_children VALUES ('adna', 'constructions', 24);
INSERT INTO facet.facet_children VALUES ('adna', 'country', 14);
INSERT INTO facet.facet_children VALUES ('adna', 'region', 16);
INSERT INTO facet.facet_children VALUES ('adna', 'location_types', 23);
INSERT INTO facet.facet_children VALUES ('adna', 'sites_polygon', 25);
INSERT INTO facet.facet_children VALUES ('adna', 'species', 26);


--
-- Data for Name: facet_clause; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.facet_clause VALUES (15, 32, 'facet.view_abundance.abundance is not null', true);
INSERT INTO facet.facet_clause VALUES (18, 33, 'facet.view_abundance.abundance is not null', true);
INSERT INTO facet.facet_clause VALUES (19, 36, 'facet.view_sample_group_references.biblio_id is not null', true);
INSERT INTO facet.facet_clause VALUES (20, 37, 'facet.view_site_references.biblio_id is not null', true);
INSERT INTO facet.facet_clause VALUES (22, 48, 'tbl_sample_group_descriptions.sample_group_description_type_id=60', true);
INSERT INTO facet.facet_clause VALUES (23, 1001, 'tbl_datasets.method_id in (3, 6)', true);
INSERT INTO facet.facet_clause VALUES (24, 1002, 'tbl_datasets.method_id in (4, 8)', true);
INSERT INTO facet.facet_clause VALUES (25, 1003, 'tbl_datasets.method_id in (14, 15, 21)', true);
INSERT INTO facet.facet_clause VALUES (26, 1004, 'tbl_datasets.method_id in (32, 33, 35, 36, 37, 94, 106)', true);
INSERT INTO facet.facet_clause VALUES (27, 1005, 'tbl_datasets.method_id in (10)', true);
INSERT INTO facet.facet_clause VALUES (28, 1006, 'tbl_datasets.method_id in (172, 171)', true);
INSERT INTO facet.facet_clause VALUES (29, 1007, 'tbl_datasets.method_id in (175)', true);
INSERT INTO facet.facet_clause VALUES (30, 21, 'countries.location_type_id=1', true);
INSERT INTO facet.facet_clause VALUES (31, 41, 'region.location_type_id in (2, 7, 14, 16, 18)', true);
INSERT INTO facet.facet_clause VALUES (32, 25, 'tbl_sites.site_id is not null', true);
INSERT INTO facet.facet_clause VALUES (33, 1008, 'tbl_datasets.method_id in (     select m.method_id     from tbl_methods m     join tbl_record_types r using (record_type_id)     where r.record_type_name = ''DNA'' )', true);


--
-- Data for Name: facet_dependency; Type: TABLE DATA; Schema: facet; Owner: -
--



--
-- Data for Name: facet_group; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.facet_group VALUES (99, 'ROOT', 'ROOT', 'ROOT', false, false);
INSERT INTO facet.facet_group VALUES (1, 'others', 'Others', 'Others', true, false);
INSERT INTO facet.facet_group VALUES (2, 'space_time', 'Space/Time', 'Space/Time', true, false);
INSERT INTO facet.facet_group VALUES (3, 'time', 'Time', 'Time', true, false);
INSERT INTO facet.facet_group VALUES (4, 'ecology', 'Ecology', 'Ecology', true, false);
INSERT INTO facet.facet_group VALUES (5, 'measured_values', 'Measured values', 'Measured values', true, false);
INSERT INTO facet.facet_group VALUES (6, 'taxonomy', 'Taxonomy', 'Taxonomy', true, false);
INSERT INTO facet.facet_group VALUES (999, 'DOMAIN', 'Domain facets', 'DOMAIN', false, false);


--
-- Data for Name: facet_table; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.facet_table VALUES (142, 21, 1, 171, NULL, 'countries');
INSERT INTO facet.facet_table VALUES (143, 41, 1, 171, NULL, 'region');
INSERT INTO facet.facet_table VALUES (144, 50, 1, 92, NULL, NULL);
INSERT INTO facet.facet_table VALUES (145, 51, 1, 119, NULL, NULL);
INSERT INTO facet.facet_table VALUES (146, 25, 1, 176, NULL, NULL);
INSERT INTO facet.facet_table VALUES (147, 25, 2, 119, NULL, NULL);
INSERT INTO facet.facet_table VALUES (148, 54, 1, 177, NULL, NULL);
INSERT INTO facet.facet_table VALUES (149, 54, 2, 130, NULL, NULL);
INSERT INTO facet.facet_table VALUES (150, 52, 1, 96, NULL, NULL);
INSERT INTO facet.facet_table VALUES (151, 53, 1, 138, NULL, NULL);
INSERT INTO facet.facet_table VALUES (152, 43, 1, 143, NULL, NULL);
INSERT INTO facet.facet_table VALUES (153, 43, 2, 35, NULL, NULL);
INSERT INTO facet.facet_table VALUES (154, 10, 1, 60, NULL, NULL);
INSERT INTO facet.facet_table VALUES (155, 1008, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (67, 1, 1, 4, NULL, NULL);
INSERT INTO facet.facet_table VALUES (68, 1, 3, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (69, 1, 2, 102, NULL, NULL);
INSERT INTO facet.facet_table VALUES (70, 32, 1, 57, NULL, NULL);
INSERT INTO facet.facet_table VALUES (71, 40, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (72, 19, 1, 119, NULL, NULL);
INSERT INTO facet.facet_table VALUES (73, 3, 1, 150, NULL, 'method_values_33');
INSERT INTO facet.facet_table VALUES (74, 4, 1, 151, NULL, 'method_values_33_82');
INSERT INTO facet.facet_table VALUES (75, 5, 1, 152, NULL, 'method_values_32');
INSERT INTO facet.facet_table VALUES (76, 6, 1, 153, NULL, 'method_values_37');
INSERT INTO facet.facet_table VALUES (77, 9, 1, 119, NULL, NULL);
INSERT INTO facet.facet_table VALUES (79, 11, 1, 71, NULL, NULL);
INSERT INTO facet.facet_table VALUES (80, 12, 1, 110, NULL, NULL);
INSERT INTO facet.facet_table VALUES (81, 13, 1, 91, NULL, NULL);
INSERT INTO facet.facet_table VALUES (82, 13, 2, 119, NULL, NULL);
INSERT INTO facet.facet_table VALUES (83, 18, 1, 119, NULL, NULL);
INSERT INTO facet.facet_table VALUES (86, 22, 2, 62, NULL, NULL);
INSERT INTO facet.facet_table VALUES (87, 22, 1, 62, NULL, NULL);
INSERT INTO facet.facet_table VALUES (88, 23, 2, 36, NULL, NULL);
INSERT INTO facet.facet_table VALUES (89, 23, 1, 36, NULL, NULL);
INSERT INTO facet.facet_table VALUES (90, 24, 2, 148, NULL, NULL);
INSERT INTO facet.facet_table VALUES (91, 24, 1, 148, NULL, NULL);
INSERT INTO facet.facet_table VALUES (96, 28, 2, 39, NULL, NULL);
INSERT INTO facet.facet_table VALUES (97, 28, 1, 39, NULL, NULL);
INSERT INTO facet.facet_table VALUES (98, 29, 1, 6, NULL, NULL);
INSERT INTO facet.facet_table VALUES (99, 29, 2, 132, NULL, NULL);
INSERT INTO facet.facet_table VALUES (100, 30, 2, 128, NULL, NULL);
INSERT INTO facet.facet_table VALUES (101, 30, 1, 128, NULL, NULL);
INSERT INTO facet.facet_table VALUES (102, 31, 1, 57, NULL, NULL);
INSERT INTO facet.facet_table VALUES (103, 33, 1, 57, NULL, NULL);
INSERT INTO facet.facet_table VALUES (104, 34, 1, 82, NULL, NULL);
INSERT INTO facet.facet_table VALUES (105, 35, 2, 84, NULL, NULL);
INSERT INTO facet.facet_table VALUES (106, 35, 1, 99, NULL, NULL);
INSERT INTO facet.facet_table VALUES (107, 36, 1, 84, NULL, NULL);
INSERT INTO facet.facet_table VALUES (108, 36, 2, 114, NULL, NULL);
INSERT INTO facet.facet_table VALUES (109, 37, 1, 84, NULL, NULL);
INSERT INTO facet.facet_table VALUES (110, 37, 2, 26, NULL, NULL);
INSERT INTO facet.facet_table VALUES (111, 38, 1, 76, NULL, NULL);
INSERT INTO facet.facet_table VALUES (112, 39, 1, 162, NULL, NULL);
INSERT INTO facet.facet_table VALUES (113, 39, 2, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (114, 39, 2, 89, NULL, NULL);
INSERT INTO facet.facet_table VALUES (118, 42, 1, 135, NULL, NULL);
INSERT INTO facet.facet_table VALUES (121, 44, 1, 24, NULL, NULL);
INSERT INTO facet.facet_table VALUES (122, 45, 1, 142, NULL, NULL);
INSERT INTO facet.facet_table VALUES (123, 46, 1, 38, NULL, NULL);
INSERT INTO facet.facet_table VALUES (124, 46, 2, 88, NULL, NULL);
INSERT INTO facet.facet_table VALUES (125, 47, 3, 102, NULL, NULL);
INSERT INTO facet.facet_table VALUES (126, 47, 2, 91, NULL, NULL);
INSERT INTO facet.facet_table VALUES (127, 47, 1, 40, NULL, NULL);
INSERT INTO facet.facet_table VALUES (128, 48, 5, 102, NULL, NULL);
INSERT INTO facet.facet_table VALUES (129, 48, 2, 130, NULL, NULL);
INSERT INTO facet.facet_table VALUES (130, 48, 3, 54, NULL, NULL);
INSERT INTO facet.facet_table VALUES (131, 48, 1, 91, NULL, NULL);
INSERT INTO facet.facet_table VALUES (132, 48, 4, 119, NULL, NULL);
INSERT INTO facet.facet_table VALUES (135, 1001, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (136, 1002, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (137, 1003, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (138, 1004, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (139, 1005, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (140, 1006, 1, 86, NULL, NULL);
INSERT INTO facet.facet_table VALUES (141, 1007, 1, 86, NULL, NULL);


--
-- Data for Name: facet_type; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.facet_type VALUES (9, 'undefined', false);
INSERT INTO facet.facet_type VALUES (1, 'discrete', false);
INSERT INTO facet.facet_type VALUES (2, 'range', true);
INSERT INTO facet.facet_type VALUES (3, 'geopolygon', true);
INSERT INTO facet.facet_type VALUES (4, 'rangesintersect', true);


--
-- Data for Name: result_field; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.result_field VALUES (1, 'sitename', 'tbl_sites', 'tbl_sites.site_name', 'Site name', 'single_item', true, NULL, NULL, 'text');
INSERT INTO facet.result_field VALUES (2, 'record_type', 'tbl_record_types', 'tbl_record_types.record_type_name', 'Record type(s)', 'text_agg_item', true, NULL, NULL, 'text');
INSERT INTO facet.result_field VALUES (3, 'analysis_entities', 'tbl_analysis_entities', 'tbl_analysis_entities.analysis_entity_id', 'Filtered records', 'single_item', true, NULL, NULL, 'int');
INSERT INTO facet.result_field VALUES (4, 'site_link', 'tbl_sites', 'tbl_sites.site_id', 'Full report', 'link_item', true, 'deprecated', 'Show site report', 'int');
INSERT INTO facet.result_field VALUES (5, 'site_link_filtered', 'tbl_sites', 'tbl_sites.site_id', 'Filtered report', 'link_item', true, 'deprecated', 'Show filtered report', 'int');
INSERT INTO facet.result_field VALUES (6, 'aggregate_all_filtered', 'tbl_aggregate_samples', '''Aggregated''::text', 'Filtered report', 'link_item_filtered', true, 'deprecated', NULL, 'text');
INSERT INTO facet.result_field VALUES (7, 'sample_group_link', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Full report', 'link_item', true, 'deprecated', NULL, 'int');
INSERT INTO facet.result_field VALUES (8, 'sample_group_link_filtered', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_id', 'Filtered report', 'link_item', true, 'deprecated', NULL, 'int');
INSERT INTO facet.result_field VALUES (9, 'abundance', 'tbl_abundances', 'tbl_abundances.abundance', 'number of taxon_id', 'single_item', true, NULL, NULL, 'text');
INSERT INTO facet.result_field VALUES (10, 'taxon_id', 'tbl_abundances', 'tbl_abundances.taxon_id', 'Taxon id  (specie)', 'single_item', true, NULL, NULL, 'int');
INSERT INTO facet.result_field VALUES (11, 'dataset', 'tbl_datasets', 'tbl_datasets.dataset_name', 'Dataset', 'single_item', true, NULL, NULL, 'text');
INSERT INTO facet.result_field VALUES (12, 'dataset_link', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Dataset details', 'single_item', true, 'deprecated', NULL, 'int');
INSERT INTO facet.result_field VALUES (13, 'dataset_link_filtered', 'tbl_datasets', 'tbl_datasets.dataset_id', 'Filtered report', 'single_item', true, 'deprecated', NULL, 'int');
INSERT INTO facet.result_field VALUES (14, 'sample_group', 'tbl_sample_groups', 'tbl_sample_groups.sample_group_name', 'Sample group', 'single_item', true, NULL, NULL, 'text');
INSERT INTO facet.result_field VALUES (15, 'methods', 'tbl_methods', 'tbl_methods.method_name', 'Method', 'single_item', true, NULL, NULL, 'text');
INSERT INTO facet.result_field VALUES (18, 'category_id', 'tbl_sites', 'category_id', 'Site ID', 'single_item', true, NULL, NULL, 'int');
INSERT INTO facet.result_field VALUES (19, 'category_name', 'tbl_sites', 'category_name', 'Site Name', 'single_item', true, NULL, NULL, 'text');
INSERT INTO facet.result_field VALUES (20, 'latitude_dd', 'tbl_sites', 'latitude_dd', 'Latitude (dd)', 'single_item', true, NULL, NULL, 'decimal');
INSERT INTO facet.result_field VALUES (21, 'longitude_dd', 'tbl_sites', 'longitude_dd', 'Longitude (dd)', 'single_item', true, NULL, NULL, 'decimal');


--
-- Data for Name: result_field_type; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.result_field_type VALUES ('sum_item', true, 'TemplateFieldCompiler', true, false, false, 'SUM({0}::double precision) AS sum_of_{0}');
INSERT INTO facet.result_field_type VALUES ('count_item', true, 'TemplateFieldCompiler', true, false, false, 'COUNT({0}) AS count_of_{0}');
INSERT INTO facet.result_field_type VALUES ('avg_item', true, 'TemplateFieldCompiler', true, false, false, 'AVG({0}) AS avg_of_{0}');
INSERT INTO facet.result_field_type VALUES ('text_agg_item', true, 'TemplateFieldCompiler', true, false, false, 'ARRAY_TO_STRING(ARRAY_AGG(DISTINCT {0}),'','') AS text_agg_of_{0}');
INSERT INTO facet.result_field_type VALUES ('single_item', true, 'TemplateFieldCompiler', false, false, true, '{0}');
INSERT INTO facet.result_field_type VALUES ('link_item', true, 'TemplateFieldCompiler', false, false, true, '{0}');
INSERT INTO facet.result_field_type VALUES ('link_item_filtered', true, 'TemplateFieldCompiler', false, false, true, '{0}');
INSERT INTO facet.result_field_type VALUES ('sort_item', false, 'TemplateFieldCompiler', false, true, false, '{0}');
INSERT INTO facet.result_field_type VALUES ('count_distinct_item', true, 'TemplateFieldCompiler', true, false, false, 'COUNT({0}) AS count_distinct_of_{0}');


--
-- Data for Name: result_specification; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.result_specification VALUES (1, 'site_level', 'Site level', false, true);
INSERT INTO facet.result_specification VALUES (2, 'aggregate_all', 'Aggregate all', false, true);
INSERT INTO facet.result_specification VALUES (3, 'sample_group_level', 'Sample group level', false, true);
INSERT INTO facet.result_specification VALUES (4, 'map_result', 'Map result', false, false);


--
-- Data for Name: result_specification_field; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.result_specification_field VALUES (4, 1, 1, 'single_item', 1);
INSERT INTO facet.result_specification_field VALUES (5, 1, 2, 'text_agg_item', 2);
INSERT INTO facet.result_specification_field VALUES (8, 1, 3, 'count_item', 3);
INSERT INTO facet.result_specification_field VALUES (10, 1, 4, 'link_item', 4);
INSERT INTO facet.result_specification_field VALUES (16, 1, 5, 'link_item_filtered', 5);
INSERT INTO facet.result_specification_field VALUES (13, 1, 1, 'sort_item', 99);
INSERT INTO facet.result_specification_field VALUES (15, 2, 6, 'link_item_filtered', 1);
INSERT INTO facet.result_specification_field VALUES (7, 2, 3, 'count_item', 2);
INSERT INTO facet.result_specification_field VALUES (1, 3, 1, 'single_item', 1);
INSERT INTO facet.result_specification_field VALUES (2, 3, 14, 'single_item', 2);
INSERT INTO facet.result_specification_field VALUES (3, 3, 2, 'single_item', 3);
INSERT INTO facet.result_specification_field VALUES (6, 3, 3, 'count_item', 4);
INSERT INTO facet.result_specification_field VALUES (9, 3, 7, 'link_item', 5);
INSERT INTO facet.result_specification_field VALUES (14, 3, 8, 'link_item_filtered', 6);
INSERT INTO facet.result_specification_field VALUES (11, 3, 1, 'sort_item', 99);
INSERT INTO facet.result_specification_field VALUES (12, 3, 14, 'sort_item', 99);
INSERT INTO facet.result_specification_field VALUES (23, 4, 18, 'single_item', 1);
INSERT INTO facet.result_specification_field VALUES (24, 4, 19, 'single_item', 2);
INSERT INTO facet.result_specification_field VALUES (21, 4, 20, 'single_item', 3);
INSERT INTO facet.result_specification_field VALUES (22, 4, 21, 'single_item', 4);


--
-- Data for Name: result_view_type; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.result_view_type VALUES ('tabular', 'Tabular', true, 'result_facet', 'TabularResultSqlCompiler', 'site_level');
INSERT INTO facet.result_view_type VALUES ('map', 'Map', false, 'map_result', 'MapResultSqlCompiler', 'map_result');


--
-- Data for Name: table; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet."table" VALUES (38, '', 'tbl_abundance_elements', 'abundance_element_id', false);
INSERT INTO facet."table" VALUES (50, '', 'tbl_abundance_ident_levels', 'abundance_ident_level_id', false);
INSERT INTO facet."table" VALUES (44, '', 'tbl_abundance_modifications', 'abundance_modification_id', false);
INSERT INTO facet."table" VALUES (88, '', 'tbl_abundances', 'abundance_id', false);
INSERT INTO facet."table" VALUES (11, '', 'tbl_activity_types', 'activity_type_id', false);
INSERT INTO facet."table" VALUES (160, '', 'tbl_age_types', 'age_type_id', false);
INSERT INTO facet."table" VALUES (145, '', 'tbl_aggregate_datasets', 'aggregate_dataset_id', false);
INSERT INTO facet."table" VALUES (47, '', 'tbl_aggregate_order_types', 'aggregate_order_type_id', false);
INSERT INTO facet."table" VALUES (115, '', 'tbl_aggregate_sample_ages', 'aggregate_sample_age_id', false);
INSERT INTO facet."table" VALUES (2, '', 'tbl_aggregate_samples', 'aggregate_sample_id', false);
INSERT INTO facet."table" VALUES (136, '', 'tbl_alt_ref_types', 'alt_ref_type_id', false);
INSERT INTO facet."table" VALUES (4, '', 'tbl_analysis_entities', 'analysis_entity_id', false);
INSERT INTO facet."table" VALUES (96, '', 'tbl_analysis_entity_ages', 'analysis_entity_age_id', false);
INSERT INTO facet."table" VALUES (93, '', 'tbl_analysis_entity_dimensions', 'analysis_entity_dimension_id', false);
INSERT INTO facet."table" VALUES (3, '', 'tbl_analysis_entity_prep_methods', 'analysis_entity_prep_method_id', false);
INSERT INTO facet."table" VALUES (84, '', 'tbl_biblio', 'biblio_id', false);
INSERT INTO facet."table" VALUES (139, '', 'tbl_ceramics', 'ceramics_id', false);
INSERT INTO facet."table" VALUES (161, '', 'tbl_ceramics_lookup', 'ceramics_lookup_id', false);
INSERT INTO facet."table" VALUES (97, '', 'tbl_ceramics_measurements', 'ceramics_measurement_id', false);
INSERT INTO facet."table" VALUES (117, '', 'tbl_chronologies', 'chronology_id', false);
INSERT INTO facet."table" VALUES (14, '', 'tbl_colours', 'colour_id', false);
INSERT INTO facet."table" VALUES (29, '', 'tbl_contacts', 'contact_id', false);
INSERT INTO facet."table" VALUES (123, '', 'tbl_contact_types', 'contact_type_id', false);
INSERT INTO facet."table" VALUES (106, '', 'tbl_coordinate_method_dimensions', 'coordinate_method_dimension_id', false);
INSERT INTO facet."table" VALUES (83, '', 'tbl_dataset_contacts', 'dataset_contact_id', false);
INSERT INTO facet."table" VALUES (76, '', 'tbl_dataset_masters', 'master_set_id', false);
INSERT INTO facet."table" VALUES (162, '', 'tbl_dataset_methods', 'dataset_method_id', false);
INSERT INTO facet."table" VALUES (86, '', 'tbl_datasets', 'dataset_id', false);
INSERT INTO facet."table" VALUES (133, '', 'tbl_dataset_submissions', 'dataset_submission_id', false);
INSERT INTO facet."table" VALUES (140, '', 'tbl_dataset_submission_types', 'submission_type_id', false);
INSERT INTO facet."table" VALUES (104, '', 'tbl_data_type_groups', 'data_type_group_id', false);
INSERT INTO facet."table" VALUES (135, '', 'tbl_data_types', 'data_type_id', false);
INSERT INTO facet."table" VALUES (85, '', 'tbl_dating_labs', 'dating_lab_id', false);
INSERT INTO facet."table" VALUES (134, '', 'tbl_dating_material', 'dating_material_id', false);
INSERT INTO facet."table" VALUES (5, '', 'tbl_dating_uncertainty', 'dating_uncertainty_id', false);
INSERT INTO facet."table" VALUES (69, '', 'tbl_dendro', 'dendro_id', false);
INSERT INTO facet."table" VALUES (127, '', 'tbl_dendro_date_notes', 'dendro_date_note_id', false);
INSERT INTO facet."table" VALUES (138, '', 'tbl_dendro_dates', 'dendro_date_id', false);
INSERT INTO facet."table" VALUES (163, '', 'tbl_dendro_lookup', 'dendro_lookup_id', false);
INSERT INTO facet."table" VALUES (100, '', 'tbl_dendro_measurements', 'dendro_measurement_id', false);
INSERT INTO facet."table" VALUES (98, '', 'tbl_dimensions', 'dimension_id', false);
INSERT INTO facet."table" VALUES (62, '', 'tbl_ecocode_definitions', 'ecocode_definition_id', false);
INSERT INTO facet."table" VALUES (51, '', 'tbl_ecocode_groups', 'ecocode_group_id', false);
INSERT INTO facet."table" VALUES (101, '', 'tbl_ecocodes', 'ecocode_id', false);
INSERT INTO facet."table" VALUES (128, '', 'tbl_ecocode_systems', 'ecocode_system_id', false);
INSERT INTO facet."table" VALUES (164, '', 'tbl_error_uncertainties', 'error_uncertainty_id', false);
INSERT INTO facet."table" VALUES (75, '', 'tbl_features', 'feature_id', false);
INSERT INTO facet."table" VALUES (6, '', 'tbl_feature_types', 'feature_type_id', false);
INSERT INTO facet."table" VALUES (60, '', 'tbl_geochronology', 'geochron_id', false);
INSERT INTO facet."table" VALUES (87, '', 'tbl_geochron_refs', 'geochron_ref_id', false);
INSERT INTO facet."table" VALUES (1, '', 'tbl_horizons', 'horizon_id', false);
INSERT INTO facet."table" VALUES (146, '', 'tbl_identification_levels', 'identification_level_id', false);
INSERT INTO facet."table" VALUES (16, '', 'tbl_image_types', 'image_type_id', false);
INSERT INTO facet."table" VALUES (22, '', 'tbl_imported_taxa_replacements', 'imported_taxa_replacement_id', false);
INSERT INTO facet."table" VALUES (165, '', 'tbl_isotope_measurements', 'isotope_measurement_id', false);
INSERT INTO facet."table" VALUES (166, '', 'tbl_isotopes', 'isotope_id', false);
INSERT INTO facet."table" VALUES (167, '', 'tbl_isotope_standards', 'isotope_standard_id', false);
INSERT INTO facet."table" VALUES (168, '', 'tbl_isotope_types', 'isotope_type_id', false);
INSERT INTO facet."table" VALUES (79, '', 'tbl_languages', 'language_id', false);
INSERT INTO facet."table" VALUES (80, '', 'tbl_lithology', 'lithology_id', false);
INSERT INTO facet."table" VALUES (35, '', 'tbl_locations', 'location_id', false);
INSERT INTO facet."table" VALUES (92, '', 'tbl_location_types', 'location_type_id', false);
INSERT INTO facet."table" VALUES (70, '', 'tbl_mcrdata_birmbeetledat', 'mcrdata_birmbeetledat_id', false);
INSERT INTO facet."table" VALUES (18, '', 'tbl_mcr_names', 'taxon_id', false);
INSERT INTO facet."table" VALUES (19, '', 'tbl_mcr_summary_data', 'mcr_summary_data_id', false);
INSERT INTO facet."table" VALUES (12, '', 'tbl_measured_value_dimensions', 'measured_value_dimension_id', false);
INSERT INTO facet."table" VALUES (121, '', 'tbl_measured_values', 'measured_value_id', false);
INSERT INTO facet."table" VALUES (33, '', 'tbl_method_groups', 'method_group_id', false);
INSERT INTO facet."table" VALUES (89, '', 'tbl_methods', 'method_id', false);
INSERT INTO facet."table" VALUES (142, '', 'tbl_modification_types', 'modification_type_id', false);
INSERT INTO facet."table" VALUES (132, '', 'tbl_physical_sample_features', 'physical_sample_feature_id', false);
INSERT INTO facet."table" VALUES (102, '', 'tbl_physical_samples', 'physical_sample_id', false);
INSERT INTO facet."table" VALUES (58, '', 'tbl_projects', 'project_id', false);
INSERT INTO facet."table" VALUES (9, '', 'tbl_project_stages', 'project_stage_id', false);
INSERT INTO facet."table" VALUES (17, '', 'tbl_project_types', 'project_type_id', false);
INSERT INTO facet."table" VALUES (48, '', 'tbl_rdb', 'rdb_id', false);
INSERT INTO facet."table" VALUES (24, '', 'tbl_rdb_codes', 'rdb_code_id', false);
INSERT INTO facet."table" VALUES (143, '', 'tbl_rdb_systems', 'rdb_system_id', false);
INSERT INTO facet."table" VALUES (110, '', 'tbl_record_types', 'record_type_id', false);
INSERT INTO facet."table" VALUES (68, '', 'tbl_relative_age_refs', 'relative_age_ref_id', false);
INSERT INTO facet."table" VALUES (71, '', 'tbl_relative_ages', 'relative_age_id', false);
INSERT INTO facet."table" VALUES (37, '', 'tbl_relative_age_types', 'relative_age_type_id', false);
INSERT INTO facet."table" VALUES (55, '', 'tbl_relative_dates', 'relative_date_id', false);
INSERT INTO facet."table" VALUES (10, '', 'tbl_sample_alt_refs', 'sample_alt_ref_id', false);
INSERT INTO facet."table" VALUES (32, '', 'tbl_sample_colours', 'sample_colour_id', false);
INSERT INTO facet."table" VALUES (8, '', 'tbl_sample_coordinates', 'sample_coordinate_id', false);
INSERT INTO facet."table" VALUES (111, '', 'tbl_sample_descriptions', 'sample_description_id', false);
INSERT INTO facet."table" VALUES (56, '', 'tbl_sample_description_sample_group_contexts', 'sample_description_sample_group_context_id', false);
INSERT INTO facet."table" VALUES (73, '', 'tbl_sample_description_types', 'sample_description_type_id', false);
INSERT INTO facet."table" VALUES (116, '', 'tbl_sample_dimensions', 'sample_dimension_id', false);
INSERT INTO facet."table" VALUES (30, '', 'tbl_sample_group_coordinates', 'sample_group_position_id', false);
INSERT INTO facet."table" VALUES (130, '', 'tbl_sample_group_descriptions', 'sample_group_description_id', false);
INSERT INTO facet."table" VALUES (54, '', 'tbl_sample_group_description_types', 'sample_group_description_type_id', false);
INSERT INTO facet."table" VALUES (28, '', 'tbl_sample_group_description_type_sampling_contexts', 'sample_group_description_type_sampling_context_id', false);
INSERT INTO facet."table" VALUES (112, '', 'tbl_sample_group_dimensions', 'sample_group_dimension_id', false);
INSERT INTO facet."table" VALUES (124, '', 'tbl_sample_group_images', 'sample_group_image_id', false);
INSERT INTO facet."table" VALUES (52, '', 'tbl_sample_group_notes', 'sample_group_note_id', false);
INSERT INTO facet."table" VALUES (42, '', 'tbl_sample_group_references', 'sample_group_reference_id', false);
INSERT INTO facet."table" VALUES (91, '', 'tbl_sample_groups', 'sample_group_id', false);
INSERT INTO facet."table" VALUES (40, '', 'tbl_sample_group_sampling_contexts', 'sampling_context_id', false);
INSERT INTO facet."table" VALUES (41, '', 'tbl_sample_horizons', 'sample_horizon_id', false);
INSERT INTO facet."table" VALUES (61, '', 'tbl_sample_images', 'sample_image_id', false);
INSERT INTO facet."table" VALUES (49, '', 'tbl_sample_locations', 'sample_location_id', false);
INSERT INTO facet."table" VALUES (144, '', 'tbl_sample_location_types', 'sample_location_type_id', false);
INSERT INTO facet."table" VALUES (65, '', 'tbl_sample_location_type_sampling_contexts', 'sample_location_type_sampling_context_id', false);
INSERT INTO facet."table" VALUES (94, '', 'tbl_sample_notes', 'sample_note_id', false);
INSERT INTO facet."table" VALUES (105, '', 'tbl_sample_types', 'sample_type_id', false);
INSERT INTO facet."table" VALUES (169, '', 'tbl_season_or_qualifier', 'season_or_qualifier_id', false);
INSERT INTO facet."table" VALUES (82, '', 'tbl_seasons', 'season_id', false);
INSERT INTO facet."table" VALUES (108, '', 'tbl_season_types', 'season_type_id', false);
INSERT INTO facet."table" VALUES (64, '', 'tbl_site_images', 'site_image_id', false);
INSERT INTO facet."table" VALUES (113, '', 'tbl_site_locations', 'site_location_id', false);
INSERT INTO facet."table" VALUES (78, '', 'tbl_site_natgridrefs', 'site_natgridref_id', false);
INSERT INTO facet."table" VALUES (77, '', 'tbl_site_other_records', 'site_other_records_id', false);
INSERT INTO facet."table" VALUES (95, '', 'tbl_site_preservation_status', 'site_preservation_status_id', false);
INSERT INTO facet."table" VALUES (15, '', 'tbl_site_references', 'site_reference_id', false);
INSERT INTO facet."table" VALUES (119, '', 'tbl_sites', 'site_id', false);
INSERT INTO facet."table" VALUES (131, '', 'tbl_species_associations', 'species_association_id', false);
INSERT INTO facet."table" VALUES (118, '', 'tbl_species_association_types', 'association_type_id', false);
INSERT INTO facet."table" VALUES (103, '', 'tbl_taxa_common_names', 'taxon_common_name_id', false);
INSERT INTO facet."table" VALUES (20, '', 'tbl_taxa_images', 'taxa_images_id', false);
INSERT INTO facet."table" VALUES (126, '', 'tbl_taxa_measured_attributes', 'measured_attribute_id', false);
INSERT INTO facet."table" VALUES (141, '', 'tbl_taxa_reference_specimens', 'taxa_reference_specimen_id', false);
INSERT INTO facet."table" VALUES (27, '', 'tbl_taxa_seasonality', 'seasonality_id', false);
INSERT INTO facet."table" VALUES (13, '', 'tbl_taxa_synonyms', 'synonym_id', false);
INSERT INTO facet."table" VALUES (39, '', 'tbl_taxa_tree_authors', 'author_id', false);
INSERT INTO facet."table" VALUES (36, '', 'tbl_taxa_tree_families', 'family_id', false);
INSERT INTO facet."table" VALUES (148, '', 'tbl_taxa_tree_genera', 'genus_id', false);
INSERT INTO facet."table" VALUES (109, '', 'tbl_taxa_tree_master', 'taxon_id', false);
INSERT INTO facet."table" VALUES (45, '', 'tbl_taxa_tree_orders', 'order_id', false);
INSERT INTO facet."table" VALUES (21, '', 'tbl_taxonomic_order', 'taxonomic_order_id', false);
INSERT INTO facet."table" VALUES (63, '', 'tbl_taxonomic_order_biblio', 'taxonomic_order_biblio_id', false);
INSERT INTO facet."table" VALUES (43, '', 'tbl_taxonomic_order_systems', 'taxonomic_order_system_id', false);
INSERT INTO facet."table" VALUES (53, '', 'tbl_taxonomy_notes', 'taxonomy_notes_id', false);
INSERT INTO facet."table" VALUES (107, '', 'tbl_tephra_dates', 'tephra_date_id', false);
INSERT INTO facet."table" VALUES (129, '', 'tbl_tephra_refs', 'tephra_ref_id', false);
INSERT INTO facet."table" VALUES (31, '', 'tbl_tephras', 'tephra_id', false);
INSERT INTO facet."table" VALUES (122, '', 'tbl_text_biology', 'biology_id', false);
INSERT INTO facet."table" VALUES (90, '', 'tbl_text_distribution', 'distribution_id', false);
INSERT INTO facet."table" VALUES (67, '', 'tbl_text_identification_keys', 'key_id', false);
INSERT INTO facet."table" VALUES (72, '', 'tbl_units', 'unit_id', false);
INSERT INTO facet."table" VALUES (170, '', 'tbl_updates_log', 'updates_log_id', false);
INSERT INTO facet."table" VALUES (74, '', 'tbl_years_types', 'years_type_id', false);
INSERT INTO facet."table" VALUES (150, '', 'facet.method_measured_values(33,0)', 'analysis_entity_id', true);
INSERT INTO facet."table" VALUES (151, '', 'facet.method_measured_values(33,82)', 'analysis_entity_id', true);
INSERT INTO facet."table" VALUES (152, '', 'facet.method_measured_values(32,0)', 'analysis_entity_id', true);
INSERT INTO facet."table" VALUES (153, '', 'facet.method_measured_values(37,0)', 'analysis_entity_id', true);
INSERT INTO facet."table" VALUES (25, '', 'facet.view_abundances_by_taxon_analysis_entity', 'xxxx', false);
INSERT INTO facet."table" VALUES (26, '', 'facet.view_site_references', 'xxxx', false);
INSERT INTO facet."table" VALUES (57, '', 'facet.view_abundance', 'xxxx', false);
INSERT INTO facet."table" VALUES (99, '', 'facet.view_taxa_biblio', 'xxxx', false);
INSERT INTO facet."table" VALUES (114, '', 'facet.view_sample_group_references', 'xxxx', false);
INSERT INTO facet."table" VALUES (154, '', 'facet.method_abundance(3)', 'xxxx', true);
INSERT INTO facet."table" VALUES (155, '', 'facet.method_abundance(8)', 'xxxx', true);
INSERT INTO facet."table" VALUES (156, '', 'facet.method_abundance(111)', 'xxxx', true);
INSERT INTO facet."table" VALUES (46, '', 'countries', 'location_id', false);
INSERT INTO facet."table" VALUES (171, '', 'facet.site_location_shortcut', 'xxxx', false);
INSERT INTO facet."table" VALUES (176, '', 'facet.abundance_taxon_shortcut', 'xxx', false);
INSERT INTO facet."table" VALUES (177, '', 'facet.sample_group_construction_purposes', 'purpose', false);
INSERT INTO facet."table" VALUES (178, '', 'facet.geochronology_taxa_shortcut', 'xxx', false);


--
-- Data for Name: table_relation; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.table_relation VALUES (1, 38, 110, 20, 'record_type_id', 'record_type_id');
INSERT INTO facet.table_relation VALUES (2, 50, 88, 20, 'abundance_id', 'abundance_id');
INSERT INTO facet.table_relation VALUES (3, 50, 146, 20, 'identification_level_id', 'identification_level_id');
INSERT INTO facet.table_relation VALUES (4, 44, 88, 20, 'abundance_id', 'abundance_id');
INSERT INTO facet.table_relation VALUES (5, 44, 142, 20, 'modification_type_id', 'modification_type_id');
INSERT INTO facet.table_relation VALUES (6, 88, 38, 20, 'abundance_element_id', 'abundance_element_id');
INSERT INTO facet.table_relation VALUES (7, 88, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (8, 145, 47, 20, 'aggregate_order_type_id', 'aggregate_order_type_id');
INSERT INTO facet.table_relation VALUES (9, 115, 145, 20, 'aggregate_dataset_id', 'aggregate_dataset_id');
INSERT INTO facet.table_relation VALUES (10, 115, 96, 20, 'analysis_entity_age_id', 'analysis_entity_age_id');
INSERT INTO facet.table_relation VALUES (11, 2, 145, 20, 'aggregate_dataset_id', 'aggregate_dataset_id');
INSERT INTO facet.table_relation VALUES (12, 2, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (13, 96, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (14, 96, 117, 20, 'chronology_id', 'chronology_id');
INSERT INTO facet.table_relation VALUES (15, 93, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (16, 93, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.table_relation VALUES (17, 3, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (18, 3, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (19, 139, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (20, 139, 97, 20, 'ceramics_measurement_id', 'ceramics_measurement_id');
INSERT INTO facet.table_relation VALUES (21, 97, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (24, 117, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.table_relation VALUES (25, 14, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (26, 106, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.table_relation VALUES (27, 106, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (28, 83, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.table_relation VALUES (29, 83, 123, 20, 'contact_type_id', 'contact_type_id');
INSERT INTO facet.table_relation VALUES (30, 83, 86, 20, 'dataset_id', 'dataset_id');
INSERT INTO facet.table_relation VALUES (31, 76, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.table_relation VALUES (32, 86, 135, 20, 'data_type_id', 'data_type_id');
INSERT INTO facet.table_relation VALUES (33, 86, 76, 20, 'master_set_id', 'master_set_id');
INSERT INTO facet.table_relation VALUES (34, 86, 58, 20, 'project_id', 'project_id');
INSERT INTO facet.table_relation VALUES (35, 86, 86, 20, 'updated_dataset_id', 'dataset_id');
INSERT INTO facet.table_relation VALUES (36, 133, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.table_relation VALUES (37, 133, 86, 20, 'dataset_id', 'dataset_id');
INSERT INTO facet.table_relation VALUES (38, 133, 140, 20, 'submission_type_id', 'submission_type_id');
INSERT INTO facet.table_relation VALUES (39, 135, 104, 20, 'data_type_group_id', 'data_type_group_id');
INSERT INTO facet.table_relation VALUES (40, 85, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.table_relation VALUES (41, 134, 38, 20, 'abundance_element_id', 'abundance_element_id');
INSERT INTO facet.table_relation VALUES (42, 134, 60, 20, 'geochron_id', 'geochron_id');
INSERT INTO facet.table_relation VALUES (43, 134, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (44, 69, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (45, 69, 100, 20, 'dendro_measurement_id', 'dendro_measurement_id');
INSERT INTO facet.table_relation VALUES (46, 127, 138, 20, 'dendro_date_id', 'dendro_date_id');
INSERT INTO facet.table_relation VALUES (47, 138, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (48, 138, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.table_relation VALUES (49, 138, 74, 20, 'years_type_id', 'years_type_id');
INSERT INTO facet.table_relation VALUES (50, 100, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (51, 98, 33, 20, 'method_group_id', 'method_group_id');
INSERT INTO facet.table_relation VALUES (52, 98, 72, 20, 'unit_id', 'unit_id');
INSERT INTO facet.table_relation VALUES (53, 62, 51, 20, 'ecocode_group_id', 'ecocode_group_id');
INSERT INTO facet.table_relation VALUES (54, 51, 128, 20, 'ecocode_system_id', 'ecocode_system_id');
INSERT INTO facet.table_relation VALUES (55, 101, 62, 20, 'ecocode_definition_id', 'ecocode_definition_id');
INSERT INTO facet.table_relation VALUES (56, 101, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (57, 75, 6, 20, 'feature_type_id', 'feature_type_id');
INSERT INTO facet.table_relation VALUES (58, 60, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (59, 60, 85, 20, 'dating_lab_id', 'dating_lab_id');
INSERT INTO facet.table_relation VALUES (60, 60, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.table_relation VALUES (61, 87, 60, 20, 'geochron_id', 'geochron_id');
INSERT INTO facet.table_relation VALUES (62, 22, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (63, 80, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (64, 145, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (65, 4, 86, 1, 'dataset_id', 'dataset_id');
INSERT INTO facet.table_relation VALUES (66, 4, 102, 1, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (67, 86, 89, 1, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (68, 86, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (69, 128, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (70, 87, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (71, 1, 89, 70, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (72, 76, 84, 200, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (73, 35, 92, 20, 'location_type_id', 'location_type_id');
INSERT INTO facet.table_relation VALUES (74, 70, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (75, 18, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (76, 19, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (77, 12, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.table_relation VALUES (78, 12, 121, 20, 'measured_value_id', 'measured_value_id');
INSERT INTO facet.table_relation VALUES (79, 121, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (80, 89, 33, 20, 'method_group_id', 'method_group_id');
INSERT INTO facet.table_relation VALUES (81, 89, 110, 20, 'record_type_id', 'record_type_id');
INSERT INTO facet.table_relation VALUES (82, 132, 75, 20, 'feature_id', 'feature_id');
INSERT INTO facet.table_relation VALUES (83, 132, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (84, 102, 136, 20, 'alt_ref_type_id', 'alt_ref_type_id');
INSERT INTO facet.table_relation VALUES (85, 102, 105, 20, 'sample_type_id', 'sample_type_id');
INSERT INTO facet.table_relation VALUES (86, 58, 9, 20, 'project_stage_id', 'project_stage_id');
INSERT INTO facet.table_relation VALUES (87, 58, 17, 20, 'project_type_id', 'project_type_id');
INSERT INTO facet.table_relation VALUES (88, 48, 24, 20, 'rdb_code_id', 'rdb_code_id');
INSERT INTO facet.table_relation VALUES (89, 48, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (90, 24, 143, 20, 'rdb_system_id', 'rdb_system_id');
INSERT INTO facet.table_relation VALUES (91, 68, 71, 20, 'relative_age_id', 'relative_age_id');
INSERT INTO facet.table_relation VALUES (92, 71, 37, 20, 'relative_age_type_id', 'relative_age_type_id');
INSERT INTO facet.table_relation VALUES (93, 55, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.table_relation VALUES (95, 55, 71, 20, 'relative_age_id', 'relative_age_id');
INSERT INTO facet.table_relation VALUES (96, 10, 136, 20, 'alt_ref_type_id', 'alt_ref_type_id');
INSERT INTO facet.table_relation VALUES (97, 10, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (98, 32, 14, 20, 'colour_id', 'colour_id');
INSERT INTO facet.table_relation VALUES (99, 32, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (100, 8, 106, 20, 'coordinate_method_dimension_id', 'coordinate_method_dimension_id');
INSERT INTO facet.table_relation VALUES (101, 8, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (102, 111, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (103, 111, 73, 20, 'sample_description_type_id', 'sample_description_type_id');
INSERT INTO facet.table_relation VALUES (104, 56, 73, 20, 'sample_description_type_id', 'sample_description_type_id');
INSERT INTO facet.table_relation VALUES (105, 56, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.table_relation VALUES (106, 116, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.table_relation VALUES (107, 116, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (108, 30, 106, 20, 'coordinate_method_dimension_id', 'coordinate_method_dimension_id');
INSERT INTO facet.table_relation VALUES (109, 30, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (110, 130, 54, 20, 'sample_group_description_type_id', 'sample_group_description_type_id');
INSERT INTO facet.table_relation VALUES (111, 130, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (112, 28, 54, 20, 'sample_group_description_type_id', 'sample_group_description_type_id');
INSERT INTO facet.table_relation VALUES (113, 28, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.table_relation VALUES (114, 112, 98, 20, 'dimension_id', 'dimension_id');
INSERT INTO facet.table_relation VALUES (115, 112, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (116, 124, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.table_relation VALUES (117, 124, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (118, 52, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (119, 42, 91, 20, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (120, 91, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.table_relation VALUES (121, 41, 1, 20, 'horizon_id', 'horizon_id');
INSERT INTO facet.table_relation VALUES (122, 41, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (123, 61, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.table_relation VALUES (124, 61, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (125, 49, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (126, 49, 144, 20, 'sample_location_type_id', 'sample_location_type_id');
INSERT INTO facet.table_relation VALUES (127, 65, 144, 20, 'sample_location_type_id', 'sample_location_type_id');
INSERT INTO facet.table_relation VALUES (128, 65, 40, 20, 'sampling_context_id', 'sampling_context_id');
INSERT INTO facet.table_relation VALUES (129, 94, 102, 20, 'physical_sample_id', 'physical_sample_id');
INSERT INTO facet.table_relation VALUES (130, 82, 108, 20, 'season_type_id', 'season_type_id');
INSERT INTO facet.table_relation VALUES (131, 64, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.table_relation VALUES (132, 64, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.table_relation VALUES (133, 64, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (134, 113, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (135, 89, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (136, 102, 91, 1, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (137, 48, 35, 150, 'location_id', 'location_id');
INSERT INTO facet.table_relation VALUES (138, 143, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (140, 68, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (141, 55, 89, 70, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (142, 116, 89, 150, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (143, 91, 89, 150, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (144, 91, 119, 1, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (145, 113, 35, 5, 'location_id', 'location_id');
INSERT INTO facet.table_relation VALUES (146, 42, 84, 90, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (147, 78, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (148, 78, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (149, 95, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (150, 15, 119, 20, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (151, 131, 118, 20, 'association_type_id', 'association_type_id');
INSERT INTO facet.table_relation VALUES (152, 131, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (153, 103, 79, 20, 'language_id', 'language_id');
INSERT INTO facet.table_relation VALUES (154, 103, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (155, 20, 16, 20, 'image_type_id', 'image_type_id');
INSERT INTO facet.table_relation VALUES (156, 20, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (157, 126, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (158, 141, 29, 20, 'contact_id', 'contact_id');
INSERT INTO facet.table_relation VALUES (159, 141, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (160, 27, 11, 20, 'activity_type_id', 'activity_type_id');
INSERT INTO facet.table_relation VALUES (161, 27, 82, 20, 'season_id', 'season_id');
INSERT INTO facet.table_relation VALUES (162, 27, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (163, 109, 39, 20, 'author_id', 'author_id');
INSERT INTO facet.table_relation VALUES (164, 21, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (165, 21, 43, 20, 'taxonomic_order_system_id', 'taxonomic_order_system_id');
INSERT INTO facet.table_relation VALUES (166, 63, 43, 20, 'taxonomic_order_system_id', 'taxonomic_order_system_id');
INSERT INTO facet.table_relation VALUES (167, 53, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (168, 107, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (169, 107, 5, 20, 'dating_uncertainty_id', 'dating_uncertainty_id');
INSERT INTO facet.table_relation VALUES (170, 107, 31, 20, 'tephra_id', 'tephra_id');
INSERT INTO facet.table_relation VALUES (171, 129, 31, 20, 'tephra_id', 'tephra_id');
INSERT INTO facet.table_relation VALUES (172, 122, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (173, 90, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (174, 67, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (175, 88, 4, 1, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (176, 89, 72, 150, 'unit_id', 'unit_id');
INSERT INTO facet.table_relation VALUES (177, 71, 35, 70, 'location_id', 'location_id');
INSERT INTO facet.table_relation VALUES (178, 77, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (179, 77, 119, 150, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (180, 77, 110, 150, 'record_type_id', 'record_type_id');
INSERT INTO facet.table_relation VALUES (181, 131, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (182, 27, 35, 60, 'location_id', 'location_id');
INSERT INTO facet.table_relation VALUES (183, 13, 39, 150, 'author_id', 'author_id');
INSERT INTO facet.table_relation VALUES (184, 13, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (185, 122, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (186, 13, 36, 150, 'family_id', 'family_id');
INSERT INTO facet.table_relation VALUES (187, 13, 148, 150, 'genus_id', 'genus_id');
INSERT INTO facet.table_relation VALUES (188, 13, 109, 150, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (189, 36, 45, 1, 'order_id', 'order_id');
INSERT INTO facet.table_relation VALUES (190, 148, 36, 1, 'family_id', 'family_id');
INSERT INTO facet.table_relation VALUES (191, 109, 148, 1, 'genus_id', 'genus_id');
INSERT INTO facet.table_relation VALUES (192, 45, 110, 1, 'record_type_id', 'record_type_id');
INSERT INTO facet.table_relation VALUES (193, 63, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (194, 53, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (195, 90, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (196, 67, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (197, 129, 84, 150, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (198, 15, 84, 90, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (199, 57, 4, 2, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (200, 57, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (201, 114, 84, 80, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (202, 114, 91, 15, 'sample_group_id', 'sample_group_id');
INSERT INTO facet.table_relation VALUES (203, 26, 84, 80, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (204, 26, 119, 15, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (205, 99, 84, 10, 'biblio_id', 'biblio_id');
INSERT INTO facet.table_relation VALUES (206, 99, 109, 2, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (207, 150, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (208, 151, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (209, 152, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (210, 153, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (211, 25, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (212, 25, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (213, 154, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (214, 154, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (215, 155, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (216, 155, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (217, 156, 109, 20, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (218, 156, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (94, 55, 4, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (219, 139, 161, 20, 'ceramics_lookup_id', 'ceramics_lookup_id');
INSERT INTO facet.table_relation VALUES (220, 69, 163, 20, 'dendro_lookup_id', 'dendro_lookup_id');
INSERT INTO facet.table_relation VALUES (221, 165, 166, 20, 'isotope_measurement_id', 'isotope_measurement_id');
INSERT INTO facet.table_relation VALUES (222, 4, 166, 20, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (223, 165, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (224, 163, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (225, 161, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (226, 162, 89, 20, 'method_id', 'method_id');
INSERT INTO facet.table_relation VALUES (227, 119, 171, 20, 'site_id', 'site_id');
INSERT INTO facet.table_relation VALUES (235, 176, 109, 25, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (236, 176, 4, 25, 'analysis_entity_id', 'analysis_entity_id');
INSERT INTO facet.table_relation VALUES (237, 130, 177, 25, 'group_description', 'purpose');
INSERT INTO facet.table_relation VALUES (139, 143, 35, 25, 'location_id', 'location_id');
INSERT INTO facet.table_relation VALUES (238, 178, 109, 200, 'taxon_id', 'taxon_id');
INSERT INTO facet.table_relation VALUES (239, 178, 4, 200, 'analysis_entity_id', 'analysis_entity_id');


--
-- Data for Name: view_state; Type: TABLE DATA; Schema: facet; Owner: -
--

INSERT INTO facet.view_state VALUES ('roger', 'mähler', '2018-04-24 14:08:45.620597+00');
INSERT INTO facet.view_state VALUES ('humlab', 'roger mähler', '2018-04-24 14:13:07.975984+00');


--
-- Name: facet_clause_facet_clause_id_seq; Type: SEQUENCE SET; Schema: facet; Owner: -
--

SELECT pg_catalog.setval('facet.facet_clause_facet_clause_id_seq', 33, true);


--
-- Name: facet_dependency_facet_dependency_id_seq; Type: SEQUENCE SET; Schema: facet; Owner: -
--

SELECT pg_catalog.setval('facet.facet_dependency_facet_dependency_id_seq', 1, false);


--
-- Name: facet_table_facet_table_id_seq; Type: SEQUENCE SET; Schema: facet; Owner: -
--

SELECT pg_catalog.setval('facet.facet_table_facet_table_id_seq', 155, true);


--
-- Name: result_aggregate_field_aggregate_field_id_seq; Type: SEQUENCE SET; Schema: facet; Owner: -
--

SELECT pg_catalog.setval('facet.result_aggregate_field_aggregate_field_id_seq', 1, false);


--
-- Name: result_field_result_field_id_seq; Type: SEQUENCE SET; Schema: facet; Owner: -
--

SELECT pg_catalog.setval('facet.result_field_result_field_id_seq', 1, false);


--
-- Name: table_relation_table_relation_id_seq; Type: SEQUENCE SET; Schema: facet; Owner: -
--

SELECT pg_catalog.setval('facet.table_relation_table_relation_id_seq', 227, true);


--
-- Name: facet_children child_facet_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_children
    ADD CONSTRAINT child_facet_pkey PRIMARY KEY (facet_code, child_facet_code);


--
-- Name: facet_clause facet_clause_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_clause
    ADD CONSTRAINT facet_clause_pkey PRIMARY KEY (facet_clause_id);


--
-- Name: facet_dependency facet_dependency_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_pkey PRIMARY KEY (facet_dependency_id);


--
-- Name: facet facet_facet_code_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_code_key UNIQUE (facet_code);


--
-- Name: facet_group facet_group_facet_group_key_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_group
    ADD CONSTRAINT facet_group_facet_group_key_key UNIQUE (facet_group_key);


--
-- Name: facet_group facet_group_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_group
    ADD CONSTRAINT facet_group_pkey PRIMARY KEY (facet_group_id);


--
-- Name: facet facet_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_pkey PRIMARY KEY (facet_id);


--
-- Name: facet_table facet_table_alias_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_alias_key UNIQUE (alias);


--
-- Name: facet_table facet_table_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_pkey PRIMARY KEY (facet_table_id);


--
-- Name: facet_type facet_type_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_type
    ADD CONSTRAINT facet_type_pkey PRIMARY KEY (facet_type_id);


--
-- Name: result_specification_field result_aggregate_field_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_pkey PRIMARY KEY (specification_field_id);


--
-- Name: result_specification result_aggregate_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification
    ADD CONSTRAINT result_aggregate_pkey PRIMARY KEY (specification_id);


--
-- Name: result_field result_field_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_pkey PRIMARY KEY (result_field_id);


--
-- Name: result_field_type result_field_type_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field_type
    ADD CONSTRAINT result_field_type_pkey PRIMARY KEY (field_type_id);


--
-- Name: result_view_type result_view_type_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_view_type
    ADD CONSTRAINT result_view_type_pkey PRIMARY KEY (view_type_id);


--
-- Name: table table_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet."table"
    ADD CONSTRAINT table_pkey PRIMARY KEY (table_id);


--
-- Name: table_relation table_relation_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_pkey PRIMARY KEY (table_relation_id);


--
-- Name: table table_table_or_udf_name_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet."table"
    ADD CONSTRAINT table_table_or_udf_name_key UNIQUE (table_or_udf_name);


--
-- Name: view_state view_state_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.view_state
    ADD CONSTRAINT view_state_pkey PRIMARY KEY (view_state_key);


--
-- Name: idx_table_relation_fk1; Type: INDEX; Schema: facet; Owner: -
--

CREATE INDEX idx_table_relation_fk1 ON facet.table_relation USING btree (source_table_id);


--
-- Name: idx_table_relation_fk2; Type: INDEX; Schema: facet; Owner: -
--

CREATE INDEX idx_table_relation_fk2 ON facet.table_relation USING btree (target_table_id);


--
-- Name: facet_clause facet_clause_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_clause
    ADD CONSTRAINT facet_clause_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE;


--
-- Name: facet_dependency facet_dependency_dependency_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_dependency_facet_id_fkey FOREIGN KEY (dependency_facet_id) REFERENCES facet.facet(facet_id);


--
-- Name: facet_dependency facet_dependency_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE;


--
-- Name: facet facet_facet_group_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_group_id_fkey FOREIGN KEY (facet_group_id) REFERENCES facet.facet_group(facet_group_id);


--
-- Name: facet facet_facet_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_type_id_fkey FOREIGN KEY (facet_type_id) REFERENCES facet.facet_type(facet_type_id);


--
-- Name: facet_table facet_table_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE;


--
-- Name: facet_table facet_table_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_table_id_fkey FOREIGN KEY (table_id) REFERENCES facet."table"(table_id) ON DELETE CASCADE;


--
-- Name: facet_children fk_facet_children_child_facet_code_facet_code; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_children
    ADD CONSTRAINT fk_facet_children_child_facet_code_facet_code FOREIGN KEY (child_facet_code) REFERENCES facet.facet(facet_code);


--
-- Name: facet_children fk_facet_children_facet_code_facet_code; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_children
    ADD CONSTRAINT fk_facet_children_facet_code_facet_code FOREIGN KEY (facet_code) REFERENCES facet.facet(facet_code);


--
-- Name: result_specification_field result_aggregate_field_aggregate_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_aggregate_id_fkey FOREIGN KEY (specification_id) REFERENCES facet.result_specification(specification_id);


--
-- Name: result_specification_field result_aggregate_field_field_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_field_type_id_fkey FOREIGN KEY (field_type_id) REFERENCES facet.result_field_type(field_type_id);


--
-- Name: result_specification_field result_aggregate_field_result_field_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_result_field_id_fkey FOREIGN KEY (result_field_id) REFERENCES facet.result_field(result_field_id);


--
-- Name: result_field result_field_field_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_field_type_id_fkey FOREIGN KEY (field_type_id) REFERENCES facet.result_field_type(field_type_id);


--
-- Name: result_field result_field_table_name_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_table_name_fkey FOREIGN KEY (table_name) REFERENCES facet."table"(table_or_udf_name);


--
-- Name: table_relation table_relation_source_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_source_table_id_fkey FOREIGN KEY (source_table_id) REFERENCES facet."table"(table_id) ON DELETE CASCADE;


--
-- Name: table_relation table_relation_target_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_target_table_id_fkey FOREIGN KEY (target_table_id) REFERENCES facet."table"(table_id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

