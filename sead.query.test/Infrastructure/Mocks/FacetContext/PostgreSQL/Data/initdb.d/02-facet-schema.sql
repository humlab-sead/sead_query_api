--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1 (Debian 16.1-1.pgdg110+1)
-- Dumped by pg_dump version 16.9 (Debian 16.9-1.pgdg110+1)

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
    category_id_operator character varying(80) DEFAULT '='::character varying NOT NULL,
    category_name_expr character varying(256) NOT NULL,
    sort_expr character varying(256) NOT NULL,
    is_applicable boolean NOT NULL,
    is_default boolean NOT NULL,
    aggregate_type character varying(256) NOT NULL,
    aggregate_title character varying(256) NOT NULL,
    aggregate_facet_id integer NOT NULL
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
    ADD CONSTRAINT facet_clause_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: facet_dependency facet_dependency_dependency_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_dependency_facet_id_fkey FOREIGN KEY (dependency_facet_id) REFERENCES facet.facet(facet_id) DEFERRABLE;


--
-- Name: facet_dependency facet_dependency_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: facet facet_facet_group_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_group_id_fkey FOREIGN KEY (facet_group_id) REFERENCES facet.facet_group(facet_group_id) DEFERRABLE;


--
-- Name: facet facet_facet_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_type_id_fkey FOREIGN KEY (facet_type_id) REFERENCES facet.facet_type(facet_type_id) DEFERRABLE;


--
-- Name: facet_table facet_table_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: facet_table facet_table_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_table_id_fkey FOREIGN KEY (table_id) REFERENCES facet."table"(table_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: facet_children fk_facet_children_child_facet_code_facet_code; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_children
    ADD CONSTRAINT fk_facet_children_child_facet_code_facet_code FOREIGN KEY (child_facet_code) REFERENCES facet.facet(facet_code) DEFERRABLE;


--
-- Name: facet_children fk_facet_children_facet_code_facet_code; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_children
    ADD CONSTRAINT fk_facet_children_facet_code_facet_code FOREIGN KEY (facet_code) REFERENCES facet.facet(facet_code) DEFERRABLE;


--
-- Name: result_specification_field result_aggregate_field_aggregate_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_aggregate_id_fkey FOREIGN KEY (specification_id) REFERENCES facet.result_specification(specification_id) DEFERRABLE;


--
-- Name: result_specification_field result_aggregate_field_field_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_field_type_id_fkey FOREIGN KEY (field_type_id) REFERENCES facet.result_field_type(field_type_id) DEFERRABLE;


--
-- Name: result_specification_field result_aggregate_field_result_field_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_result_field_id_fkey FOREIGN KEY (result_field_id) REFERENCES facet.result_field(result_field_id) DEFERRABLE;


--
-- Name: result_field result_field_field_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_field_type_id_fkey FOREIGN KEY (field_type_id) REFERENCES facet.result_field_type(field_type_id) DEFERRABLE;


--
-- Name: result_field result_field_table_name_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_table_name_fkey FOREIGN KEY (table_name) REFERENCES facet."table"(table_or_udf_name) DEFERRABLE;


--
-- Name: table_relation table_relation_source_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_source_table_id_fkey FOREIGN KEY (source_table_id) REFERENCES facet."table"(table_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: table_relation table_relation_target_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_target_table_id_fkey FOREIGN KEY (target_table_id) REFERENCES facet."table"(table_id) ON DELETE CASCADE DEFERRABLE;


--
-- PostgreSQL database dump complete
--

