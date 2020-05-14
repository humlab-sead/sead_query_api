--
-- PostgreSQL database dump
--

-- Dumped from database version 12.2
-- Dumped by pg_dump version 12.2

-- Started on 2020-05-13 13:01:43

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
-- TOC entry 10 (class 2615 OID 44195)
-- Name: facet; Type: SCHEMA; Schema: -; Owner: -
--

DROP SCHEMA IF EXISTS facet CASCADE;
CREATE SCHEMA facet;


--
-- TOC entry 590 (class 1255 OID 45148)
-- Name: create_or_update_facet(jsonb); Type: FUNCTION; Schema: facet; Owner: -
--

CREATE FUNCTION facet.create_or_update_facet(j_facet jsonb) RETURNS json
    LANGUAGE plpgsql
    AS $$
        declare j_tables json;
        declare j_clauses json;
        declare i_facet_id int;
        declare s_aggregate_facet_code text;
        declare i_aggregate_facet_id int = 0;
    begin

        j_tables = j_facet -> 'tables';
        j_clauses = j_facet -> 'clauses';
    --	j_facet = j_facet - 'tables';
    --	j_facet = j_facet - 'clauses';

        i_facet_id = (j_facet ->> 'facet_id')::int;
        if i_facet_id is null then
            i_facet_id = (select coalesce(max(facet_id),0)+1 from facet.facet);
        else
            /* leave facet table be (onconflict update), just clear projectren's data */
            delete from facet.facet_table
                where facet_id = i_facet_id;
            delete from facet.facet_clause
                where facet_id = i_facet_id;
        end if;

        s_aggregate_facet_code = (j_facet ->> 'aggregate_facet_code')::text;

        if  s_aggregate_facet_code is null then
            i_aggregate_facet_id = 0;
        else
            i_aggregate_facet_id = (select facet_id from facet.facet where facet_code = s_aggregate_facet_code);
            if i_aggregate_facet_id is null then
                raise notice 'aggregate_facet_id not found for % - %', (j_facet ->> 'facet_code')::text, s_aggregate_facet_code;
            end if;
        end if;


        insert into facet.facet (facet_id, facet_code, display_title, description, facet_group_id, facet_type_id, category_id_expr, category_name_expr, sort_expr, is_applicable, is_default, aggregate_type, aggregate_title, aggregate_facet_id)
            (values (
                i_facet_id,
                (j_facet ->> 'facet_code')::text,
                (j_facet ->> 'display_title')::text,
                (j_facet ->> 'description')::text,
                (j_facet ->> 'facet_group_id')::int,
                (j_facet ->> 'facet_type_id')::text::int,
                (j_facet ->> 'category_id_expr')::text,
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
                select  (v ->> 'sequence_id')::int		   as sequence_id,
                        (v ->> 'table_name')::text		   as table_or_udf_name,
                        (v ->> 'udf_call_arguments')::text as udf_call_arguments,
                        (v ->> 'alias')					   as alias
                from jsonb_array_elements(j_facet -> 'tables') as v
            ) as v(sequence_id, table_or_udf_name, udf_call_arguments, alias)
            left join facet.table t using (table_or_udf_name);

        insert into facet.facet_clause (facet_id, clause, enforce_constraint)
            select i_facet_id,
                    (v ->> 'clause')::text,
                    (v ->> 'enforce_constraint')::bool
            from jsonb_array_elements(j_facet -> 'clauses') as v;

        return j_facet;

    end $$;


--
-- TOC entry 582 (class 1255 OID 45149)
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


SET default_table_access_method = heap;

--
-- TOC entry 361 (class 1259 OID 45174)
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
    category_name_expr character varying(256) NOT NULL,
    sort_expr character varying(256) NOT NULL,
    is_applicable boolean NOT NULL,
    is_default boolean NOT NULL,
    aggregate_type character varying(256) NOT NULL,
    aggregate_title character varying(256) NOT NULL,
    aggregate_facet_id integer NOT NULL
);


--
-- TOC entry 362 (class 1259 OID 45181)
-- Name: facet_projectren; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_projectren (
    facet_code character varying NOT NULL,
    project_facet_code character varying NOT NULL,
    "position" integer DEFAULT 0 NOT NULL
);


--
-- TOC entry 363 (class 1259 OID 45188)
-- Name: facet_clause; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_clause (
    facet_clause_id integer NOT NULL,
    facet_id integer NOT NULL,
    clause character varying(512),
    enforce_constraint boolean DEFAULT false NOT NULL
);


--
-- TOC entry 364 (class 1259 OID 45194)
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
-- TOC entry 4043 (class 0 OID 0)
-- Dependencies: 364
-- Name: facet_clause_facet_clause_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.facet_clause_facet_clause_id_seq OWNED BY facet.facet_clause.facet_clause_id;


--
-- TOC entry 365 (class 1259 OID 45196)
-- Name: facet_dependency; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_dependency (
    facet_dependency_id integer NOT NULL,
    facet_id integer NOT NULL,
    dependency_facet_id integer NOT NULL
);


--
-- TOC entry 366 (class 1259 OID 45199)
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
-- TOC entry 4044 (class 0 OID 0)
-- Dependencies: 366
-- Name: facet_dependency_facet_dependency_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.facet_dependency_facet_dependency_id_seq OWNED BY facet.facet_dependency.facet_dependency_id;


--
-- TOC entry 367 (class 1259 OID 45201)
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
-- TOC entry 368 (class 1259 OID 45205)
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
-- TOC entry 369 (class 1259 OID 45208)
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
-- TOC entry 4045 (class 0 OID 0)
-- Dependencies: 369
-- Name: facet_table_facet_table_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.facet_table_facet_table_id_seq OWNED BY facet.facet_table.facet_table_id;


--
-- TOC entry 370 (class 1259 OID 45210)
-- Name: facet_type; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.facet_type (
    facet_type_id integer NOT NULL,
    facet_type_name character varying(80) NOT NULL,
    reload_as_target boolean DEFAULT false NOT NULL
);


--
-- TOC entry 373 (class 1259 OID 45226)
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
-- TOC entry 374 (class 1259 OID 45231)
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
-- TOC entry 4046 (class 0 OID 0)
-- Dependencies: 374
-- Name: result_aggregate_field_aggregate_field_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.result_aggregate_field_aggregate_field_id_seq OWNED BY facet.result_specification_field.specification_field_id;


--
-- TOC entry 375 (class 1259 OID 45233)
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
-- TOC entry 376 (class 1259 OID 45239)
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
-- TOC entry 4047 (class 0 OID 0)
-- Dependencies: 376
-- Name: result_field_result_field_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.result_field_result_field_id_seq OWNED BY facet.result_field.result_field_id;


--
-- TOC entry 377 (class 1259 OID 45241)
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
-- TOC entry 372 (class 1259 OID 45219)
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
-- TOC entry 378 (class 1259 OID 45250)
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
-- TOC entry 379 (class 1259 OID 45254)
-- Name: table; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.table (
    table_id integer NOT NULL,
    schema_name information_schema.sql_identifier DEFAULT ''::character varying NOT NULL,
    table_or_udf_name information_schema.sql_identifier NOT NULL,
    primary_key_name information_schema.sql_identifier DEFAULT ''::character varying NOT NULL,
    is_udf boolean DEFAULT false NOT NULL
);


--
-- TOC entry 380 (class 1259 OID 45260)
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
-- TOC entry 381 (class 1259 OID 45264)
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
-- TOC entry 4048 (class 0 OID 0)
-- Dependencies: 381
-- Name: table_relation_table_relation_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: -
--

ALTER SEQUENCE facet.table_relation_table_relation_id_seq OWNED BY facet.table_relation.table_relation_id;


--
-- TOC entry 386 (class 1259 OID 45286)
-- Name: view_state; Type: TABLE; Schema: facet; Owner: -
--

CREATE TABLE facet.view_state (
    view_state_key character varying(80) NOT NULL,
    view_state_data text NOT NULL,
    create_time timestamp with time zone DEFAULT clock_timestamp()
);


--
-- TOC entry 387 (class 1259 OID 45293)
-- Name: view_taxa_biblio; Type: VIEW; Schema: facet; Owner: -
--

--
-- TOC entry 3813 (class 2604 OID 45635)
-- Name: facet_clause facet_clause_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_clause ALTER COLUMN facet_clause_id SET DEFAULT nextval('facet.facet_clause_facet_clause_id_seq'::regclass);


--
-- TOC entry 3815 (class 2604 OID 45636)
-- Name: facet_dependency facet_dependency_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency ALTER COLUMN facet_dependency_id SET DEFAULT nextval('facet.facet_dependency_facet_dependency_id_seq'::regclass);


--
-- TOC entry 3817 (class 2604 OID 45637)
-- Name: facet_table facet_table_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table ALTER COLUMN facet_table_id SET DEFAULT nextval('facet.facet_table_facet_table_id_seq'::regclass);


--
-- TOC entry 3824 (class 2604 OID 45639)
-- Name: result_field result_field_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field ALTER COLUMN result_field_id SET DEFAULT nextval('facet.result_field_result_field_id_seq'::regclass);


--
-- TOC entry 3823 (class 2604 OID 45638)
-- Name: result_specification_field specification_field_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field ALTER COLUMN specification_field_id SET DEFAULT nextval('facet.result_aggregate_field_aggregate_field_id_seq'::regclass);


--
-- TOC entry 3840 (class 2604 OID 45640)
-- Name: table_relation table_relation_id; Type: DEFAULT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation ALTER COLUMN table_relation_id SET DEFAULT nextval('facet.table_relation_table_relation_id_seq'::regclass);


--
-- TOC entry 3847 (class 2606 OID 45867)
-- Name: facet_projectren project_facet_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_projectren
    ADD CONSTRAINT project_facet_pkey PRIMARY KEY (facet_code, project_facet_code);


--
-- TOC entry 3849 (class 2606 OID 45869)
-- Name: facet_clause facet_clause_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_clause
    ADD CONSTRAINT facet_clause_pkey PRIMARY KEY (facet_clause_id);


--
-- TOC entry 3851 (class 2606 OID 45871)
-- Name: facet_dependency facet_dependency_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_pkey PRIMARY KEY (facet_dependency_id);


--
-- TOC entry 3843 (class 2606 OID 45873)
-- Name: facet facet_facet_code_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_code_key UNIQUE (facet_code);


--
-- TOC entry 3853 (class 2606 OID 45875)
-- Name: facet_group facet_group_facet_group_key_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_group
    ADD CONSTRAINT facet_group_facet_group_key_key UNIQUE (facet_group_key);


--
-- TOC entry 3855 (class 2606 OID 45877)
-- Name: facet_group facet_group_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_group
    ADD CONSTRAINT facet_group_pkey PRIMARY KEY (facet_group_id);


--
-- TOC entry 3845 (class 2606 OID 45879)
-- Name: facet facet_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_pkey PRIMARY KEY (facet_id);


--
-- TOC entry 3857 (class 2606 OID 45881)
-- Name: facet_table facet_table_alias_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_alias_key UNIQUE (alias);


--
-- TOC entry 3859 (class 2606 OID 45883)
-- Name: facet_table facet_table_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_pkey PRIMARY KEY (facet_table_id);


--
-- TOC entry 3861 (class 2606 OID 45885)
-- Name: facet_type facet_type_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_type
    ADD CONSTRAINT facet_type_pkey PRIMARY KEY (facet_type_id);


--
-- TOC entry 3865 (class 2606 OID 45887)
-- Name: result_specification_field result_aggregate_field_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_pkey PRIMARY KEY (specification_field_id);


--
-- TOC entry 3863 (class 2606 OID 45889)
-- Name: result_specification result_aggregate_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification
    ADD CONSTRAINT result_aggregate_pkey PRIMARY KEY (specification_id);


--
-- TOC entry 3867 (class 2606 OID 45891)
-- Name: result_field result_field_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_pkey PRIMARY KEY (result_field_id);


--
-- TOC entry 3869 (class 2606 OID 45893)
-- Name: result_field_type result_field_type_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field_type
    ADD CONSTRAINT result_field_type_pkey PRIMARY KEY (field_type_id);


--
-- TOC entry 3871 (class 2606 OID 45895)
-- Name: result_view_type result_view_type_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_view_type
    ADD CONSTRAINT result_view_type_pkey PRIMARY KEY (view_type_id);


--
-- TOC entry 3873 (class 2606 OID 45897)
-- Name: table table_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet."table"
    ADD CONSTRAINT table_pkey PRIMARY KEY (table_id);


--
-- TOC entry 3879 (class 2606 OID 45899)
-- Name: table_relation table_relation_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_pkey PRIMARY KEY (table_relation_id);


--
-- TOC entry 3875 (class 2606 OID 45901)
-- Name: table table_table_or_udf_name_key; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet."table"
    ADD CONSTRAINT table_table_or_udf_name_key UNIQUE (table_or_udf_name);


--
-- TOC entry 3881 (class 2606 OID 45903)
-- Name: view_state view_state_pkey; Type: CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.view_state
    ADD CONSTRAINT view_state_pkey PRIMARY KEY (view_state_key);


--
-- TOC entry 3876 (class 1259 OID 46218)
-- Name: idx_table_relation_fk1; Type: INDEX; Schema: facet; Owner: -
--

CREATE INDEX idx_table_relation_fk1 ON facet.table_relation USING btree (source_table_id);


--
-- TOC entry 3877 (class 1259 OID 46219)
-- Name: idx_table_relation_fk2; Type: INDEX; Schema: facet; Owner: -
--

CREATE INDEX idx_table_relation_fk2 ON facet.table_relation USING btree (target_table_id);


--
-- TOC entry 3886 (class 2606 OID 46246)
-- Name: facet_clause facet_clause_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_clause
    ADD CONSTRAINT facet_clause_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE;


--
-- TOC entry 3887 (class 2606 OID 46251)
-- Name: facet_dependency facet_dependency_dependency_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_dependency_facet_id_fkey FOREIGN KEY (dependency_facet_id) REFERENCES facet.facet(facet_id);


--
-- TOC entry 3888 (class 2606 OID 46256)
-- Name: facet_dependency facet_dependency_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_dependency
    ADD CONSTRAINT facet_dependency_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE;


--
-- TOC entry 3882 (class 2606 OID 46261)
-- Name: facet facet_facet_group_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_group_id_fkey FOREIGN KEY (facet_group_id) REFERENCES facet.facet_group(facet_group_id);


--
-- TOC entry 3883 (class 2606 OID 46267)
-- Name: facet facet_facet_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet
    ADD CONSTRAINT facet_facet_type_id_fkey FOREIGN KEY (facet_type_id) REFERENCES facet.facet_type(facet_type_id);


--
-- TOC entry 3889 (class 2606 OID 46273)
-- Name: facet_table facet_table_facet_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) ON DELETE CASCADE;


--
-- TOC entry 3890 (class 2606 OID 46278)
-- Name: facet_table facet_table_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_table
    ADD CONSTRAINT facet_table_table_id_fkey FOREIGN KEY (table_id) REFERENCES facet."table"(table_id);


--
-- TOC entry 3884 (class 2606 OID 46283)
-- Name: facet_projectren fk_facet_projectren_project_facet_code_facet_code; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_projectren
    ADD CONSTRAINT fk_facet_projectren_project_facet_code_facet_code FOREIGN KEY (project_facet_code) REFERENCES facet.facet(facet_code);


--
-- TOC entry 3885 (class 2606 OID 46288)
-- Name: facet_projectren fk_facet_projectren_facet_code_facet_code; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.facet_projectren
    ADD CONSTRAINT fk_facet_projectren_facet_code_facet_code FOREIGN KEY (facet_code) REFERENCES facet.facet(facet_code);


--
-- TOC entry 3891 (class 2606 OID 46293)
-- Name: result_specification_field result_aggregate_field_aggregate_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_aggregate_id_fkey FOREIGN KEY (specification_id) REFERENCES facet.result_specification(specification_id);


--
-- TOC entry 3892 (class 2606 OID 46298)
-- Name: result_specification_field result_aggregate_field_field_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_field_type_id_fkey FOREIGN KEY (field_type_id) REFERENCES facet.result_field_type(field_type_id);


--
-- TOC entry 3893 (class 2606 OID 46303)
-- Name: result_specification_field result_aggregate_field_result_field_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_specification_field
    ADD CONSTRAINT result_aggregate_field_result_field_id_fkey FOREIGN KEY (result_field_id) REFERENCES facet.result_field(result_field_id);


--
-- TOC entry 3894 (class 2606 OID 46308)
-- Name: result_field result_field_field_type_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_field_type_id_fkey FOREIGN KEY (field_type_id) REFERENCES facet.result_field_type(field_type_id);


--
-- TOC entry 3895 (class 2606 OID 46313)
-- Name: result_field result_field_table_name_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.result_field
    ADD CONSTRAINT result_field_table_name_fkey FOREIGN KEY (table_name) REFERENCES facet."table"(table_or_udf_name);


--
-- TOC entry 3896 (class 2606 OID 46318)
-- Name: table_relation table_relation_source_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_source_table_id_fkey FOREIGN KEY (source_table_id) REFERENCES facet."table"(table_id);


--
-- TOC entry 3897 (class 2606 OID 46323)
-- Name: table_relation table_relation_target_table_id_fkey; Type: FK CONSTRAINT; Schema: facet; Owner: -
--

ALTER TABLE ONLY facet.table_relation
    ADD CONSTRAINT table_relation_target_table_id_fkey FOREIGN KEY (target_table_id) REFERENCES facet."table"(table_id);


-- Completed on 2020-05-13 13:01:44

--
-- PostgreSQL database dump complete
--

