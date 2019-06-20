--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

DO $$
BEGIN

	IF current_role != 'querysead_owner' THEN
		RAISE EXCEPTION 'This script must be run as querysead_worker!';
	END IF;

END $$ LANGUAGE plpgsql;


CREATE SCHEMA facet;

ALTER SCHEMA facet OWNER TO seadworker;

SET search_path = facet, pg_catalog;

SET default_with_oids = false;

--
-- Name: facet; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE facet (
    facet_id integer NOT NULL,
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


ALTER TABLE facet.facet OWNER TO querysead_owner;

--
-- Name: facet_condition_clause; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE facet_condition_clause (
    facet_source_table_id integer NOT NULL,
    facet_id integer NOT NULL,
    clause character varying(512)
);


ALTER TABLE facet.facet_condition_clause OWNER TO querysead_owner;

--
-- Name: facet_condition_clause_facet_source_table_id_seq; Type: SEQUENCE; Schema: facet; Owner: querysead_owner
--

CREATE SEQUENCE facet_condition_clause_facet_source_table_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE facet.facet_condition_clause_facet_source_table_id_seq OWNER TO querysead_owner;

--
-- Name: facet_condition_clause_facet_source_table_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: querysead_owner
--

ALTER SEQUENCE facet_condition_clause_facet_source_table_id_seq OWNED BY facet_condition_clause.facet_source_table_id;


--
-- Name: facet_group; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE facet_group (
    facet_group_id integer NOT NULL,
    facet_group_key character varying(80) NOT NULL,
    display_title character varying(80) NOT NULL,
    is_applicable boolean NOT NULL,
    is_default boolean NOT NULL
);


ALTER TABLE facet.facet_group OWNER TO querysead_owner;

--
-- Name: facet_table; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE facet_table (
    facet_table_id integer NOT NULL,
    facet_id integer NOT NULL,
    sequence_id integer NOT NULL,
    schema_name character varying(80),
    table_name character varying(80),
    alias character varying(80)
);


ALTER TABLE facet.facet_table OWNER TO querysead_owner;

--
-- Name: facet_table_facet_table_id_seq; Type: SEQUENCE; Schema: facet; Owner: querysead_owner
--

CREATE SEQUENCE facet_table_facet_table_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE facet.facet_table_facet_table_id_seq OWNER TO querysead_owner;

--
-- Name: facet_table_facet_table_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: querysead_owner
--

ALTER SEQUENCE facet_table_facet_table_id_seq OWNED BY facet_table.facet_table_id;


--
-- Name: facet_type; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE facet_type (
    facet_type_id integer NOT NULL,
    facet_type_name character varying(80) NOT NULL,
    reload_as_target boolean DEFAULT false NOT NULL
);


ALTER TABLE facet.facet_type OWNER TO querysead_owner;

--
-- Name: graph_table; Type: TABLE; Schema: facet; Owner: seadworker
--

CREATE TABLE graph_table (
    table_id integer NOT NULL,
    table_name information_schema.sql_identifier NOT NULL
);


ALTER TABLE facet.graph_table OWNER TO seadworker;

--
-- Name: graph_table_relation; Type: TABLE; Schema: facet; Owner: seadworker
--

CREATE TABLE graph_table_relation (
    relation_id integer NOT NULL,
    source_table_id integer NOT NULL,
    target_table_id integer NOT NULL,
    weight integer DEFAULT 0 NOT NULL,
    source_column_name information_schema.sql_identifier NOT NULL,
    target_column_name information_schema.sql_identifier NOT NULL
);


ALTER TABLE facet.graph_table_relation OWNER TO seadworker;

--
-- Name: graph_table_relation_relation_id_seq; Type: SEQUENCE; Schema: facet; Owner: seadworker
--

CREATE SEQUENCE graph_table_relation_relation_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE facet.graph_table_relation_relation_id_seq OWNER TO seadworker;

--
-- Name: graph_table_relation_relation_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: seadworker
--

ALTER SEQUENCE graph_table_relation_relation_id_seq OWNED BY graph_table_relation.relation_id;


--
-- Name: graph_table_table_id_seq; Type: SEQUENCE; Schema: facet; Owner: seadworker
--

CREATE SEQUENCE graph_table_table_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE facet.graph_table_table_id_seq OWNER TO seadworker;

--
-- Name: graph_table_table_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: seadworker
--

ALTER SEQUENCE graph_table_table_id_seq OWNED BY graph_table.table_id;


--
-- Name: report_site; Type: VIEW; Schema: facet; Owner: querysead_owner
--

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


ALTER TABLE facet.report_site OWNER TO querysead_owner;

--
-- Name: result_aggregate; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE result_aggregate (
    aggregate_id integer NOT NULL,
    aggregate_key character varying(40) NOT NULL,
    display_text character varying(80) NOT NULL,
    is_applicable boolean DEFAULT false NOT NULL,
    is_activated boolean DEFAULT true NOT NULL,
    input_type character varying(40) DEFAULT 'checkboxes'::character varying NOT NULL,
    has_selector boolean DEFAULT true NOT NULL
);


ALTER TABLE facet.result_aggregate OWNER TO querysead_owner;

--
-- Name: result_aggregate_field; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE result_aggregate_field (
    aggregate_field_id integer NOT NULL,
    aggregate_id integer NOT NULL,
    result_field_id integer NOT NULL,
    field_type_id character varying(40) DEFAULT 'single_item'::character varying NOT NULL,
    sequence_id integer DEFAULT 0 NOT NULL
);


ALTER TABLE facet.result_aggregate_field OWNER TO querysead_owner;

--
-- Name: result_definition_field_result_definition_field_id_seq; Type: SEQUENCE; Schema: facet; Owner: querysead_owner
--

CREATE SEQUENCE result_definition_field_result_definition_field_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE facet.result_definition_field_result_definition_field_id_seq OWNER TO querysead_owner;

--
-- Name: result_definition_field_result_definition_field_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: querysead_owner
--

ALTER SEQUENCE result_definition_field_result_definition_field_id_seq OWNED BY result_aggregate_field.aggregate_field_id;


--
-- Name: result_field; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE result_field (
    result_field_id integer NOT NULL,
    result_field_key character varying(40) NOT NULL,
    table_name character varying(80),
    column_name character varying(80) NOT NULL,
    display_text character varying(80) NOT NULL,
    field_type_id character varying(20) NOT NULL,
    activated boolean NOT NULL,
    link_url character varying(256),
    link_label character varying(256)
);


ALTER TABLE facet.result_field OWNER TO querysead_owner;

--
-- Name: result_field_result_field_id_seq; Type: SEQUENCE; Schema: facet; Owner: querysead_owner
--

CREATE SEQUENCE result_field_result_field_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE facet.result_field_result_field_id_seq OWNER TO querysead_owner;

--
-- Name: result_field_result_field_id_seq; Type: SEQUENCE OWNED BY; Schema: facet; Owner: querysead_owner
--

ALTER SEQUENCE result_field_result_field_id_seq OWNED BY result_field.result_field_id;


--
-- Name: result_field_type; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE result_field_type (
    field_type_id character varying(40) NOT NULL,
    is_result_value boolean DEFAULT true NOT NULL,
    sql_field_compiler character varying(40) DEFAULT ''::character varying NOT NULL,
    is_aggregate_field boolean DEFAULT false NOT NULL,
    is_sort_field boolean DEFAULT false NOT NULL,
    is_item_field boolean DEFAULT false NOT NULL,
    sql_template character varying(256) DEFAULT '{0}'::character varying NOT NULL
);


ALTER TABLE facet.result_field_type OWNER TO querysead_owner;

--
-- Name: result_view_type; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE result_view_type (
    view_type_id character varying(40) NOT NULL,
    view_name character varying(40) NOT NULL,
    is_cachable boolean DEFAULT true NOT NULL
);


ALTER TABLE facet.result_view_type OWNER TO querysead_owner;

--
-- Name: tbl_result_fields; Type: TABLE; Schema: facet; Owner: querysead_owner
--

CREATE TABLE tbl_result_fields (
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


ALTER TABLE facet.tbl_result_fields OWNER TO querysead_owner;

--
-- Name: view_state; Type: TABLE; Schema: facet; Owner: seadworker
--

CREATE TABLE view_state (
    view_state_key character varying(80) NOT NULL,
    view_state_data text NOT NULL,
    create_time timestamp with time zone DEFAULT clock_timestamp()
);


ALTER TABLE facet.view_state OWNER TO seadworker;

--
-- Name: viewstate; Type: TABLE; Schema: facet; Owner: clearinghouse_worker
--

CREATE TABLE viewstate (
    viewstate_key character varying(80) NOT NULL,
    viewstate_data text NOT NULL
);


ALTER TABLE facet.viewstate OWNER TO clearinghouse_worker;

--
-- Name: facet_source_table_id; Type: DEFAULT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_condition_clause ALTER COLUMN facet_source_table_id SET DEFAULT nextval('facet_condition_clause_facet_source_table_id_seq'::regclass);


--
-- Name: facet_table_id; Type: DEFAULT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_table ALTER COLUMN facet_table_id SET DEFAULT nextval('facet_table_facet_table_id_seq'::regclass);


--
-- Name: table_id; Type: DEFAULT; Schema: facet; Owner: seadworker
--

ALTER TABLE ONLY graph_table ALTER COLUMN table_id SET DEFAULT nextval('graph_table_table_id_seq'::regclass);


--
-- Name: relation_id; Type: DEFAULT; Schema: facet; Owner: seadworker
--

ALTER TABLE ONLY graph_table_relation ALTER COLUMN relation_id SET DEFAULT nextval('graph_table_relation_relation_id_seq'::regclass);


--
-- Name: aggregate_field_id; Type: DEFAULT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_aggregate_field ALTER COLUMN aggregate_field_id SET DEFAULT nextval('result_definition_field_result_definition_field_id_seq'::regclass);


--
-- Name: result_field_id; Type: DEFAULT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_field ALTER COLUMN result_field_id SET DEFAULT nextval('result_field_result_field_id_seq'::regclass);


--
-- Name: facet_condition_clause_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_condition_clause
    ADD CONSTRAINT facet_condition_clause_pk PRIMARY KEY (facet_source_table_id);


--
-- Name: facet_group_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_group
    ADD CONSTRAINT facet_group_pk PRIMARY KEY (facet_group_id);


--
-- Name: facet_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet
    ADD CONSTRAINT facet_pk PRIMARY KEY (facet_id);


--
-- Name: facet_table_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_table
    ADD CONSTRAINT facet_table_pk PRIMARY KEY (facet_table_id);


--
-- Name: facet_type_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_type
    ADD CONSTRAINT facet_type_pk PRIMARY KEY (facet_type_id);


--
-- Name: graph_table_pk; Type: CONSTRAINT; Schema: facet; Owner: seadworker
--

ALTER TABLE ONLY graph_table
    ADD CONSTRAINT graph_table_pk PRIMARY KEY (table_id);


--
-- Name: graph_table_relation_pk; Type: CONSTRAINT; Schema: facet; Owner: seadworker
--

ALTER TABLE ONLY graph_table_relation
    ADD CONSTRAINT graph_table_relation_pk PRIMARY KEY (relation_id);


--
-- Name: result_definition_field_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_aggregate_field
    ADD CONSTRAINT result_definition_field_pk PRIMARY KEY (aggregate_field_id);


--
-- Name: result_definition_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_aggregate
    ADD CONSTRAINT result_definition_pk PRIMARY KEY (aggregate_id);


--
-- Name: result_field_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_field
    ADD CONSTRAINT result_field_pk PRIMARY KEY (result_field_id);


--
-- Name: result_field_type_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_field_type
    ADD CONSTRAINT result_field_type_pk PRIMARY KEY (field_type_id);


--
-- Name: view_state_key_pk; Type: CONSTRAINT; Schema: facet; Owner: seadworker
--

ALTER TABLE ONLY view_state
    ADD CONSTRAINT view_state_key_pk PRIMARY KEY (view_state_key);


--
-- Name: view_type_pk; Type: CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_view_type
    ADD CONSTRAINT view_type_pk PRIMARY KEY (view_type_id);


--
-- Name: viewstate_pk; Type: CONSTRAINT; Schema: facet; Owner: clearinghouse_worker
--

ALTER TABLE ONLY viewstate
    ADD CONSTRAINT viewstate_pk PRIMARY KEY (viewstate_key);


--
-- Name: idx_graph_table_relation_fk1; Type: INDEX; Schema: facet; Owner: seadworker
--

CREATE INDEX idx_graph_table_relation_fk1 ON graph_table_relation USING btree (source_table_id);


--
-- Name: idx_graph_table_relation_fk2; Type: INDEX; Schema: facet; Owner: seadworker
--

CREATE INDEX idx_graph_table_relation_fk2 ON graph_table_relation USING btree (target_table_id);


--
-- Name: facet_condition_clause_facet_fk; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_condition_clause
    ADD CONSTRAINT facet_condition_clause_facet_fk FOREIGN KEY (facet_id) REFERENCES facet(facet_id);


--
-- Name: facet_facet_group_fk; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet
    ADD CONSTRAINT facet_facet_group_fk FOREIGN KEY (facet_group_id) REFERENCES facet_group(facet_group_id);


--
-- Name: facet_facet_type_fk; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet
    ADD CONSTRAINT facet_facet_type_fk FOREIGN KEY (facet_type_id) REFERENCES facet_type(facet_type_id);


--
-- Name: facet_table_facet_fk; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY facet_table
    ADD CONSTRAINT facet_table_facet_fk FOREIGN KEY (facet_id) REFERENCES facet(facet_id);


--
-- Name: graph_table_relation_fk1; Type: FK CONSTRAINT; Schema: facet; Owner: seadworker
--

ALTER TABLE ONLY graph_table_relation
    ADD CONSTRAINT graph_table_relation_fk1 FOREIGN KEY (source_table_id) REFERENCES graph_table(table_id);


--
-- Name: graph_table_relation_fk2; Type: FK CONSTRAINT; Schema: facet; Owner: seadworker
--

ALTER TABLE ONLY graph_table_relation
    ADD CONSTRAINT graph_table_relation_fk2 FOREIGN KEY (target_table_id) REFERENCES graph_table(table_id);


--
-- Name: result_definition_fieldfk1; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_aggregate_field
    ADD CONSTRAINT result_definition_fieldfk1 FOREIGN KEY (aggregate_id) REFERENCES result_aggregate(aggregate_id);


--
-- Name: result_definition_fieldfk2; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_aggregate_field
    ADD CONSTRAINT result_definition_fieldfk2 FOREIGN KEY (result_field_id) REFERENCES result_field(result_field_id);


--
-- Name: result_definition_fieldfk3; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_aggregate_field
    ADD CONSTRAINT result_definition_fieldfk3 FOREIGN KEY (field_type_id) REFERENCES result_field_type(field_type_id);


--
-- Name: result_fieldfk3; Type: FK CONSTRAINT; Schema: facet; Owner: querysead_owner
--

ALTER TABLE ONLY result_field
    ADD CONSTRAINT result_fieldfk3 FOREIGN KEY (field_type_id) REFERENCES result_field_type(field_type_id);


--
-- Name: facet; Type: ACL; Schema: -; Owner: seadworker
--

REVOKE ALL ON SCHEMA facet FROM PUBLIC;
REVOKE ALL ON SCHEMA facet FROM seadworker;
GRANT ALL ON SCHEMA facet TO seadworker;
GRANT USAGE ON SCHEMA facet TO PUBLIC;
GRANT USAGE ON SCHEMA facet TO seadread;


--
-- Name: facet; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE facet FROM PUBLIC;
REVOKE ALL ON TABLE facet FROM querysead_owner;
GRANT ALL ON TABLE facet TO querysead_owner;
GRANT SELECT ON TABLE facet TO seadread;


--
-- Name: facet_condition_clause; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE facet_condition_clause FROM PUBLIC;
REVOKE ALL ON TABLE facet_condition_clause FROM querysead_owner;
GRANT ALL ON TABLE facet_condition_clause TO querysead_owner;
GRANT SELECT ON TABLE facet_condition_clause TO seadread;


--
-- Name: facet_group; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE facet_group FROM PUBLIC;
REVOKE ALL ON TABLE facet_group FROM querysead_owner;
GRANT ALL ON TABLE facet_group TO querysead_owner;
GRANT SELECT ON TABLE facet_group TO seadread;


--
-- Name: facet_table; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE facet_table FROM PUBLIC;
REVOKE ALL ON TABLE facet_table FROM querysead_owner;
GRANT ALL ON TABLE facet_table TO querysead_owner;
GRANT SELECT ON TABLE facet_table TO seadread;


--
-- Name: facet_type; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE facet_type FROM PUBLIC;
REVOKE ALL ON TABLE facet_type FROM querysead_owner;
GRANT ALL ON TABLE facet_type TO querysead_owner;
GRANT SELECT ON TABLE facet_type TO seadread;


--
-- Name: graph_table; Type: ACL; Schema: facet; Owner: seadworker
--

REVOKE ALL ON TABLE graph_table FROM PUBLIC;
REVOKE ALL ON TABLE graph_table FROM seadworker;
GRANT ALL ON TABLE graph_table TO seadworker;
GRANT SELECT ON TABLE graph_table TO readers;
GRANT SELECT ON TABLE graph_table TO seadread;


--
-- Name: graph_table_relation; Type: ACL; Schema: facet; Owner: seadworker
--

REVOKE ALL ON TABLE graph_table_relation FROM PUBLIC;
REVOKE ALL ON TABLE graph_table_relation FROM seadworker;
GRANT ALL ON TABLE graph_table_relation TO seadworker;
GRANT SELECT ON TABLE graph_table_relation TO readers;
GRANT SELECT ON TABLE graph_table_relation TO seadread;


--
-- Name: result_aggregate; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE result_aggregate FROM PUBLIC;
REVOKE ALL ON TABLE result_aggregate FROM querysead_owner;
GRANT ALL ON TABLE result_aggregate TO querysead_owner;
GRANT SELECT ON TABLE result_aggregate TO seadread;
GRANT SELECT ON TABLE result_aggregate TO seadworker;


--
-- Name: result_aggregate_field; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE result_aggregate_field FROM PUBLIC;
REVOKE ALL ON TABLE result_aggregate_field FROM querysead_owner;
GRANT ALL ON TABLE result_aggregate_field TO querysead_owner;
GRANT SELECT ON TABLE result_aggregate_field TO seadread;
GRANT SELECT ON TABLE result_aggregate_field TO seadworker;


--
-- Name: result_field; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE result_field FROM PUBLIC;
REVOKE ALL ON TABLE result_field FROM querysead_owner;
GRANT ALL ON TABLE result_field TO querysead_owner;
GRANT SELECT ON TABLE result_field TO seadread;
GRANT SELECT ON TABLE result_field TO seadworker;


--
-- Name: result_field_type; Type: ACL; Schema: facet; Owner: querysead_owner
--

REVOKE ALL ON TABLE result_field_type FROM PUBLIC;
REVOKE ALL ON TABLE result_field_type FROM querysead_owner;
GRANT ALL ON TABLE result_field_type TO querysead_owner;
GRANT SELECT ON TABLE result_field_type TO seadread;
GRANT SELECT ON TABLE result_field_type TO seadworker;


--
-- Name: view_state; Type: ACL; Schema: facet; Owner: seadworker
--

REVOKE ALL ON TABLE view_state FROM PUBLIC;
REVOKE ALL ON TABLE view_state FROM seadworker;
GRANT ALL ON TABLE view_state TO seadworker;
GRANT SELECT ON TABLE view_state TO readers;
GRANT SELECT,INSERT,UPDATE ON TABLE view_state TO seadread;


--
-- PostgreSQL database dump complete
--

