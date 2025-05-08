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
-- Name: public; Type: SCHEMA; Schema: -; Owner: -
--




SET default_table_access_method = heap;

--
-- Name: tbl_abundances; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_abundances (
    abundance_id bigint NOT NULL,
    taxon_id integer NOT NULL,
    analysis_entity_id bigint NOT NULL,
    abundance_element_id integer,
    abundance integer DEFAULT 0 NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_abundance_elements; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_abundance_elements (
    abundance_element_id integer NOT NULL,
    record_type_id integer,
    element_name character varying(100) NOT NULL,
    element_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_abundance_ident_levels; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_abundance_ident_levels (
    abundance_ident_level_id integer NOT NULL,
    abundance_id bigint NOT NULL,
    identification_level_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_abundance_modifications; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_abundance_modifications (
    abundance_modification_id integer NOT NULL,
    abundance_id integer NOT NULL,
    modification_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_activity_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_activity_types (
    activity_type_id integer NOT NULL,
    activity_type character varying(50) DEFAULT NULL::character varying,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_age_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_age_types (
    age_type_id integer NOT NULL,
    age_type character varying(150) NOT NULL,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_aggregate_datasets; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_aggregate_datasets (
    aggregate_dataset_id integer NOT NULL,
    aggregate_order_type_id integer NOT NULL,
    biblio_id integer,
    aggregate_dataset_name character varying(255),
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    aggregate_dataset_uuid text null
);


--
-- Name: tbl_aggregate_order_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_aggregate_order_types (
    aggregate_order_type_id integer NOT NULL,
    aggregate_order_type character varying(60) NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text
);


--
-- Name: tbl_aggregate_samples; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_aggregate_samples (
    aggregate_sample_id integer NOT NULL,
    aggregate_dataset_id integer NOT NULL,
    analysis_entity_id bigint NOT NULL,
    aggregate_sample_name character varying(50),
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_aggregate_sample_ages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_aggregate_sample_ages (
    aggregate_sample_age_id integer NOT NULL,
    aggregate_dataset_id integer NOT NULL,
    analysis_entity_age_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_alt_ref_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_alt_ref_types (
    alt_ref_type_id integer NOT NULL,
    alt_ref_type character varying(50) NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text
);


--
-- Name: tbl_analysis_boolean_values; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_boolean_values (
    analysis_boolean_value_id integer NOT NULL,
    analysis_value_id bigint NOT NULL,
    qualifier text,
    value boolean
);


--
-- Name: tbl_analysis_categorical_values; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_categorical_values (
    analysis_categorical_value_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    value_type_item_id integer NOT NULL,
    value numeric(20,10) DEFAULT NULL::numeric,
    is_variant boolean
);


--
-- Name: tbl_analysis_dating_ranges; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_dating_ranges (
    analysis_dating_range_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    low_value integer,
    high_value integer,
    low_is_uncertain boolean,
    high_is_uncertain boolean,
    low_qualifier text,
    high_qualifier text,
    age_type_id integer DEFAULT 1 NOT NULL,
    season_id integer,
    dating_uncertainty_id integer,
    is_variant boolean
);


--
-- Name: tbl_analysis_entities; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_entities (
    analysis_entity_id bigint NOT NULL,
    physical_sample_id integer,
    dataset_id integer,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_analysis_entity_ages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_entity_ages (
    analysis_entity_age_id integer NOT NULL,
    age numeric(20,5),
    age_older numeric(20,5),
    age_younger numeric(20,5),
    analysis_entity_id bigint,
    chronology_id integer,
    date_updated timestamp with time zone DEFAULT now(),
    dating_specifier text,
    age_range int4range GENERATED ALWAYS AS (
CASE
    WHEN ((age_younger IS NULL) AND (age_older IS NULL)) THEN NULL::int4range
    ELSE int4range(COALESCE((age_younger)::integer, (age_older)::integer), (COALESCE((age_older)::integer, (age_younger)::integer) + 1))
END) STORED
);


--
-- Name: tbl_analysis_entity_dimensions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_entity_dimensions (
    analysis_entity_dimension_id integer NOT NULL,
    analysis_entity_id bigint NOT NULL,
    dimension_id integer NOT NULL,
    dimension_value numeric NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_analysis_entity_prep_methods; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_entity_prep_methods (
    analysis_entity_prep_method_id integer NOT NULL,
    analysis_entity_id bigint NOT NULL,
    method_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_analysis_identifiers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_identifiers (
    analysis_identifier_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    value text NOT NULL
);


--
-- Name: tbl_analysis_integer_ranges; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_integer_ranges (
    analysis_integer_range_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    low_value integer,
    high_value integer,
    low_is_uncertain boolean,
    high_is_uncertain boolean,
    low_qualifier text,
    high_qualifier text,
    is_variant boolean
);


--
-- Name: tbl_analysis_integer_values; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_integer_values (
    analysis_integer_value_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    qualifier text,
    value integer,
    is_variant boolean
);


--
-- Name: tbl_analysis_notes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_notes (
    analysis_note_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    value text NOT NULL
);


--
-- Name: tbl_analysis_numerical_ranges; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_numerical_ranges (
    analysis_numerical_range_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    value numrange NOT NULL,
    low_is_uncertain boolean,
    high_is_uncertain boolean,
    low_qualifier text,
    high_qualifier text,
    is_variant boolean
);


--
-- Name: tbl_analysis_numerical_values; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_numerical_values (
    analysis_numerical_value_id bigint NOT NULL,
    analysis_value_id bigint NOT NULL,
    qualifier text,
    value numeric(20,10),
    is_variant boolean
);


--
-- Name: tbl_analysis_taxon_counts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_taxon_counts (
    analysis_taxon_count_id integer NOT NULL,
    analysis_value_id bigint NOT NULL,
    taxon_id integer NOT NULL,
    value numeric(20,10) NOT NULL
);


--
-- Name: tbl_analysis_values; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_values (
    analysis_value_id bigint NOT NULL,
    value_class_id integer NOT NULL,
    analysis_entity_id bigint NOT NULL,
    analysis_value text,
    boolean_value boolean,
    is_boolean boolean,
    is_uncertain boolean,
    is_undefined boolean,
    is_not_analyzed boolean,
    is_indeterminable boolean,
    is_anomaly boolean
);


--
-- Name: tbl_analysis_value_dimensions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_analysis_value_dimensions (
    analysis_value_dimension_id integer NOT NULL,
    analysis_value_id bigint NOT NULL,
    dimension_id integer NOT NULL,
    value numeric(20,10) NOT NULL
);


--
-- Name: tbl_biblio; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_biblio (
    biblio_id integer NOT NULL,
    bugs_reference character varying(60) DEFAULT NULL::character varying,
    date_updated timestamp with time zone DEFAULT now(),
    doi character varying(255) DEFAULT NULL::character varying,
    isbn character varying(128) DEFAULT NULL::character varying,
    notes text,
    title character varying,
    year character varying(255) DEFAULT NULL::character varying,
    authors character varying,
    full_reference text DEFAULT ''::text NOT NULL,
    url character varying,
    biblio_uuid text null
);


--
-- Name: tbl_ceramics; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_ceramics (
    ceramics_id integer NOT NULL,
    analysis_entity_id integer NOT NULL,
    measurement_value character varying NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    ceramics_lookup_id integer NOT NULL
);


--
-- Name: tbl_ceramics_lookup; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_ceramics_lookup (
    ceramics_lookup_id integer NOT NULL,
    method_id integer NOT NULL,
    description text,
    name character varying NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_ceramics_measurements; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_ceramics_measurements (
    ceramics_measurement_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    method_id integer
);


--
-- Name: tbl_chronologies; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_chronologies (
    chronology_id integer NOT NULL,
    age_model text,
    relative_age_type_id integer,
    chronology_name text,
    contact_id integer,
    date_prepared timestamp(0) without time zone,
    date_updated timestamp with time zone DEFAULT now(),
    notes text
);


--
-- Name: tbl_colours; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_colours (
    colour_id integer NOT NULL,
    colour_name character varying(30) NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    method_id integer NOT NULL,
    rgb integer
);


--
-- Name: tbl_contacts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_contacts (
    contact_id integer NOT NULL,
    address_1 character varying(255),
    address_2 character varying(255),
    location_id integer,
    email character varying,
    first_name character varying(50),
    last_name character varying(100),
    phone_number character varying(50),
    url text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_contact_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_contact_types (
    contact_type_id integer NOT NULL,
    contact_type_name character varying(150) NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text
);


--
-- Name: tbl_coordinate_method_dimensions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_coordinate_method_dimensions (
    coordinate_method_dimension_id integer NOT NULL,
    dimension_id integer NOT NULL,
    method_id integer NOT NULL,
    limit_upper numeric(18,10),
    limit_lower numeric(18,10),
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_data_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_data_types (
    data_type_id integer NOT NULL,
    data_type_group_id integer NOT NULL,
    data_type_name character varying(25) DEFAULT NULL::character varying,
    date_updated timestamp with time zone DEFAULT now(),
    definition text
);


--
-- Name: tbl_data_type_groups; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_data_type_groups (
    data_type_group_id integer NOT NULL,
    data_type_group_name character varying(25),
    date_updated timestamp with time zone DEFAULT now(),
    description text
);


--
-- Name: tbl_datasets; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_datasets (
    dataset_id integer NOT NULL,
    master_set_id integer,
    data_type_id integer NOT NULL,
    method_id integer,
    biblio_id integer,
    updated_dataset_id integer,
    project_id integer,
    dataset_name character varying(50) NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    dataset_uuid text null
);


--
-- Name: tbl_dataset_contacts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dataset_contacts (
    dataset_contact_id integer NOT NULL,
    contact_id integer NOT NULL,
    contact_type_id integer NOT NULL,
    dataset_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dataset_masters; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dataset_masters (
    master_set_id integer NOT NULL,
    contact_id integer,
    biblio_id integer,
    master_name character varying(100),
    master_notes text,
    url text,
    date_updated timestamp with time zone DEFAULT now(),
    master_set_uuid text null
);


--
-- Name: tbl_dataset_methods; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dataset_methods (
    dataset_method_id integer NOT NULL,
    dataset_id integer NOT NULL,
    method_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dataset_submissions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dataset_submissions (
    dataset_submission_id integer NOT NULL,
    dataset_id integer NOT NULL,
    submission_type_id integer NOT NULL,
    contact_id integer NOT NULL,
    date_submitted text,
    notes text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dataset_submission_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dataset_submission_types (
    submission_type_id integer NOT NULL,
    submission_type character varying(60) NOT NULL,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dating_labs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dating_labs (
    dating_lab_id integer NOT NULL,
    contact_id integer,
    international_lab_id character varying(10) NOT NULL,
    lab_name character varying(100) DEFAULT NULL::character varying,
    country_id integer,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dating_material; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dating_material (
    dating_material_id integer NOT NULL,
    geochron_id integer NOT NULL,
    taxon_id integer,
    material_dated character varying,
    description text,
    abundance_element_id integer,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dating_uncertainty; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dating_uncertainty (
    dating_uncertainty_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    uncertainty character varying
);


--
-- Name: tbl_dendro; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dendro (
    dendro_id integer NOT NULL,
    analysis_entity_id integer NOT NULL,
    measurement_value character varying NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    dendro_lookup_id integer NOT NULL
);


--
-- Name: tbl_dendro_dates; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dendro_dates (
    dendro_date_id integer NOT NULL,
    season_id integer,
    dating_uncertainty_id integer,
    dendro_lookup_id integer NOT NULL,
    age_type_id integer NOT NULL,
    analysis_entity_id integer NOT NULL,
    age_older integer,
    age_younger integer,
    date_updated timestamp with time zone DEFAULT now(),
    age_range int4range GENERATED ALWAYS AS (
CASE
    WHEN ((age_younger IS NULL) AND (age_older IS NULL)) THEN NULL::int4range
    ELSE int4range(COALESCE(age_older, age_younger), (COALESCE(age_younger, age_older) + 1))
END) STORED
);


--
-- Name: tbl_dendro_date_notes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dendro_date_notes (
    dendro_date_note_id integer NOT NULL,
    dendro_date_id integer NOT NULL,
    note text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dendro_lookup; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dendro_lookup (
    dendro_lookup_id integer NOT NULL,
    method_id integer,
    name character varying NOT NULL,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_dimensions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_dimensions (
    dimension_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    dimension_abbrev character varying(40),
    dimension_description text,
    dimension_name character varying(50) NOT NULL,
    unit_id integer,
    method_group_id integer
);


--
-- Name: tbl_ecocodes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_ecocodes (
    ecocode_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    ecocode_definition_id integer DEFAULT 0,
    taxon_id integer DEFAULT 0
);


--
-- Name: tbl_ecocode_definitions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_ecocode_definitions (
    ecocode_definition_id integer NOT NULL,
    abbreviation character varying(10) DEFAULT NULL::character varying,
    date_updated timestamp with time zone DEFAULT now(),
    definition text,
    ecocode_group_id integer DEFAULT 0,
    name character varying(150) DEFAULT NULL::character varying,
    notes text,
    sort_order smallint DEFAULT 0
);


--
-- Name: tbl_ecocode_groups; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_ecocode_groups (
    ecocode_group_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    definition text DEFAULT NULL::character varying,
    ecocode_system_id integer DEFAULT 0,
    name character varying(200) DEFAULT NULL::character varying,
    abbreviation character varying(255)
);


--
-- Name: tbl_ecocode_systems; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_ecocode_systems (
    ecocode_system_id integer NOT NULL,
    biblio_id integer,
    date_updated timestamp with time zone DEFAULT now(),
    definition text DEFAULT NULL::character varying,
    name character varying(50) DEFAULT NULL::character varying,
    notes text,
    ecocode_system_uuid text null
);


--
-- Name: tbl_features; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_features (
    feature_id integer NOT NULL,
    feature_type_id integer NOT NULL,
    feature_name character varying,
    feature_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_feature_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_feature_types (
    feature_type_id integer NOT NULL,
    feature_type_name character varying(128),
    feature_type_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_geochron_refs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_geochron_refs (
    geochron_ref_id integer NOT NULL,
    geochron_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_geochronology; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_geochronology (
    geochron_id integer NOT NULL,
    analysis_entity_id bigint NOT NULL,
    dating_lab_id integer,
    lab_number character varying(40),
    age numeric(20,5),
    error_older numeric(20,5),
    error_younger numeric(20,5),
    delta_13c numeric(10,5),
    notes text,
    date_updated timestamp with time zone DEFAULT now(),
    dating_uncertainty_id integer,
    geochron_uuid text null
);


--
-- Name: tbl_horizons; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_horizons (
    horizon_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    horizon_name character varying(15) NOT NULL,
    method_id integer NOT NULL
);


--
-- Name: tbl_identification_levels; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_identification_levels (
    identification_level_id integer NOT NULL,
    identification_level_abbrev character varying(50) DEFAULT NULL::character varying,
    identification_level_name character varying(50) DEFAULT NULL::character varying,
    notes text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_image_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_image_types (
    image_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    image_type character varying(40) NOT NULL
);


--
-- Name: tbl_imported_taxa_replacements; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_imported_taxa_replacements (
    imported_taxa_replacement_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    imported_name_replaced character varying(100) NOT NULL,
    taxon_id integer NOT NULL
);


--
-- Name: tbl_isotopes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_isotopes (
    isotope_id integer NOT NULL,
    analysis_entity_id integer NOT NULL,
    isotope_measurement_id integer NOT NULL,
    isotope_standard_id integer,
    measurement_value text,
    unit_id integer NOT NULL,
    isotope_value_specifier_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_isotope_measurements; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_isotope_measurements (
    isotope_measurement_id integer NOT NULL,
    isotope_standard_id integer,
    method_id integer,
    isotope_type_id integer,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_isotope_standards; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_isotope_standards (
    isotope_standard_id integer NOT NULL,
    isotope_ration character varying,
    international_scale character varying,
    accepted_ratio_xe6 character varying,
    error_of_ratio character varying,
    reference character varying,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_isotope_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_isotope_types (
    isotope_type_id integer NOT NULL,
    designation character varying,
    abbreviation character varying,
    atomic_number numeric,
    description text,
    alternative_designation character varying,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_isotope_value_specifiers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_isotope_value_specifiers (
    isotope_value_specifier_id integer NOT NULL,
    name character varying NOT NULL,
    description text NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_languages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_languages (
    language_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    language_name_english character varying(100) DEFAULT NULL::character varying,
    language_name_native character varying(100) DEFAULT NULL::character varying
);


--
-- Name: tbl_lithology; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_lithology (
    lithology_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    depth_bottom numeric(20,5),
    depth_top numeric(20,5) NOT NULL,
    description text NOT NULL,
    lower_boundary character varying(255),
    sample_group_id integer NOT NULL
);


--
-- Name: tbl_locations; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_locations (
    location_id integer NOT NULL,
    location_name character varying(255) NOT NULL,
    location_type_id integer NOT NULL,
    default_lat_dd numeric(18,10),
    default_long_dd numeric(18,10),
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_location_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_location_types (
    location_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    location_type character varying(40)
);


--
-- Name: tbl_mcr_names; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_mcr_names (
    taxon_id integer NOT NULL,
    comparison_notes character varying(255) DEFAULT NULL::character varying,
    date_updated timestamp with time zone DEFAULT now(),
    mcr_name_trim character varying(80) DEFAULT NULL::character varying,
    mcr_number smallint DEFAULT 0,
    mcr_species_name character varying(200) DEFAULT NULL::character varying
);


--
-- Name: tbl_mcr_summary_data; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_mcr_summary_data (
    mcr_summary_data_id integer NOT NULL,
    cog_mid_tmax smallint DEFAULT 0,
    cog_mid_trange smallint DEFAULT 0,
    date_updated timestamp with time zone DEFAULT now(),
    taxon_id integer NOT NULL,
    tmax_hi smallint DEFAULT 0,
    tmax_lo smallint DEFAULT 0,
    tmin_hi smallint DEFAULT 0,
    tmin_lo smallint DEFAULT 0,
    trange_hi smallint DEFAULT 0,
    trange_lo smallint DEFAULT 0
);


--
-- Name: tbl_mcrdata_birmbeetledat; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_mcrdata_birmbeetledat (
    mcrdata_birmbeetledat_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    mcr_data text,
    mcr_row smallint NOT NULL,
    taxon_id integer NOT NULL
);


--
-- Name: tbl_measured_values; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_measured_values (
    measured_value_id bigint NOT NULL,
    analysis_entity_id bigint NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    measured_value numeric(20,10) NOT NULL
);


--
-- Name: tbl_measured_value_dimensions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_measured_value_dimensions (
    measured_value_dimension_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    dimension_id integer NOT NULL,
    dimension_value numeric(18,10) NOT NULL,
    measured_value_id bigint NOT NULL
);


--
-- Name: tbl_methods; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_methods (
    method_id integer NOT NULL,
    biblio_id integer,
    date_updated timestamp with time zone DEFAULT now(),
    description text NOT NULL,
    method_abbrev_or_alt_name character varying(50),
    method_group_id integer NOT NULL,
    method_name character varying(50) NOT NULL,
    record_type_id integer,
    unit_id integer,
    method_uuid text null
);


--
-- Name: tbl_method_groups; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_method_groups (
    method_group_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text NOT NULL,
    group_name character varying(100) NOT NULL
);


--
-- Name: tbl_modification_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_modification_types (
    modification_type_id integer NOT NULL,
    modification_type_name character varying(128),
    modification_type_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_physical_samples; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_physical_samples (
    physical_sample_id integer NOT NULL,
    sample_group_id integer DEFAULT 0 NOT NULL,
    alt_ref_type_id integer,
    sample_type_id integer NOT NULL,
    sample_name character varying(50) NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    date_sampled character varying
);


--
-- Name: tbl_physical_sample_features; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_physical_sample_features (
    physical_sample_feature_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    feature_id integer NOT NULL,
    physical_sample_id integer NOT NULL
);


--
-- Name: tbl_projects; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_projects (
    project_id integer NOT NULL,
    project_type_id integer,
    project_stage_id integer,
    project_name character varying(150),
    project_abbrev_name character varying(25),
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_project_stages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_project_stages (
    project_stage_id integer NOT NULL,
    stage_name character varying,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_project_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_project_types (
    project_type_id integer NOT NULL,
    project_type_name character varying,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_rdb; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_rdb (
    rdb_id integer NOT NULL,
    location_id integer NOT NULL,
    rdb_code_id integer,
    taxon_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_rdb_codes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_rdb_codes (
    rdb_code_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    rdb_category character varying(4) DEFAULT NULL::character varying,
    rdb_definition character varying(200) DEFAULT NULL::character varying,
    rdb_system_id integer DEFAULT 0
);


--
-- Name: tbl_rdb_systems; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_rdb_systems (
    rdb_system_id integer NOT NULL,
    biblio_id integer NOT NULL,
    location_id integer NOT NULL,
    rdb_first_published smallint,
    rdb_system character varying(10) DEFAULT NULL::character varying,
    rdb_system_date integer,
    rdb_version character varying(10) DEFAULT NULL::character varying,
    date_updated timestamp with time zone DEFAULT now(),
    rdb_system_uuid text null
);


--
-- Name: tbl_record_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_record_types (
    record_type_id integer NOT NULL,
    record_type_name character varying(50) DEFAULT NULL::character varying,
    record_type_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_relative_ages; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_relative_ages (
    relative_age_id integer NOT NULL,
    relative_age_type_id integer,
    relative_age_name character varying(50),
    description text,
    c14_age_older numeric(20,5),
    c14_age_younger numeric(20,5),
    cal_age_older numeric(20,5),
    cal_age_younger numeric(20,5),
    notes text,
    date_updated timestamp with time zone DEFAULT now(),
    location_id integer,
    abbreviation character varying,
    relative_age_uuid text null
);


--
-- Name: tbl_relative_age_refs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_relative_age_refs (
    relative_age_ref_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    relative_age_id integer NOT NULL
);


--
-- Name: tbl_relative_age_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_relative_age_types (
    relative_age_type_id integer NOT NULL,
    age_type character varying,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_relative_dates; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_relative_dates (
    relative_date_id integer NOT NULL,
    relative_age_id integer,
    method_id integer,
    notes text,
    date_updated timestamp with time zone DEFAULT now(),
    dating_uncertainty_id integer,
    analysis_entity_id integer NOT NULL
);


--
-- Name: tbl_sample_alt_refs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_alt_refs (
    sample_alt_ref_id integer NOT NULL,
    alt_ref character varying(80) NOT NULL,
    alt_ref_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    physical_sample_id integer NOT NULL
);


--
-- Name: tbl_sample_colours; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_colours (
    sample_colour_id integer NOT NULL,
    colour_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    physical_sample_id integer NOT NULL
);


--
-- Name: tbl_sample_coordinates; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_coordinates (
    sample_coordinate_id integer NOT NULL,
    physical_sample_id integer NOT NULL,
    coordinate_method_dimension_id integer NOT NULL,
    measurement numeric(20,10) NOT NULL,
    accuracy numeric(20,10),
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_descriptions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_descriptions (
    sample_description_id integer NOT NULL,
    sample_description_type_id integer NOT NULL,
    physical_sample_id integer NOT NULL,
    description character varying,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_description_sample_group_contexts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_description_sample_group_contexts (
    sample_description_sample_group_context_id integer NOT NULL,
    sampling_context_id integer,
    sample_description_type_id integer,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_description_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_description_types (
    sample_description_type_id integer NOT NULL,
    type_name character varying(255),
    type_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_dimensions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_dimensions (
    sample_dimension_id integer NOT NULL,
    physical_sample_id integer NOT NULL,
    dimension_id integer NOT NULL,
    method_id integer NOT NULL,
    dimension_value numeric(20,10) NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    qualifier_id integer
);


--
-- Name: tbl_sample_groups; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_groups (
    sample_group_id integer NOT NULL,
    site_id integer DEFAULT 0,
    sampling_context_id integer,
    method_id integer NOT NULL,
    sample_group_name character varying(100) DEFAULT NULL::character varying,
    sample_group_description text,
    date_updated timestamp with time zone DEFAULT now(),
    sample_group_uuid text null
);


--
-- Name: tbl_sample_group_coordinates; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_coordinates (
    sample_group_position_id integer NOT NULL,
    coordinate_method_dimension_id integer NOT NULL,
    sample_group_position numeric(20,10),
    position_accuracy character varying(128),
    sample_group_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_group_descriptions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_descriptions (
    sample_group_description_id integer NOT NULL,
    group_description character varying,
    sample_group_description_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    sample_group_id integer
);


--
-- Name: tbl_sample_group_description_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_description_types (
    sample_group_description_type_id integer NOT NULL,
    type_name character varying(255),
    type_description character varying,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_group_description_type_sampling_contexts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_description_type_sampling_contexts (
    sample_group_description_type_sampling_context_id integer NOT NULL,
    sampling_context_id integer NOT NULL,
    sample_group_description_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_group_dimensions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_dimensions (
    sample_group_dimension_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    dimension_id integer NOT NULL,
    dimension_value numeric(20,5) NOT NULL,
    sample_group_id integer NOT NULL,
    qualifier_id integer
);


--
-- Name: tbl_sample_group_images; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_images (
    sample_group_image_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    image_location text NOT NULL,
    image_name character varying(80),
    image_type_id integer NOT NULL,
    sample_group_id integer NOT NULL
);


--
-- Name: tbl_sample_group_notes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_notes (
    sample_group_note_id integer NOT NULL,
    sample_group_id integer NOT NULL,
    note character varying,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_group_references; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_references (
    sample_group_reference_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    sample_group_id integer DEFAULT 0
);


--
-- Name: tbl_sample_group_sampling_contexts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_group_sampling_contexts (
    sampling_context_id integer NOT NULL,
    sampling_context character varying(80) NOT NULL,
    description text,
    sort_order smallint DEFAULT 0 NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_horizons; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_horizons (
    sample_horizon_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    horizon_id integer NOT NULL,
    physical_sample_id integer NOT NULL
);


--
-- Name: tbl_sample_images; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_images (
    sample_image_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    image_location text NOT NULL,
    image_name character varying(80),
    image_type_id integer NOT NULL,
    physical_sample_id integer NOT NULL
);


--
-- Name: tbl_sample_locations; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_locations (
    sample_location_id integer NOT NULL,
    sample_location_type_id integer NOT NULL,
    physical_sample_id integer NOT NULL,
    location character varying(255),
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_location_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_location_types (
    sample_location_type_id integer NOT NULL,
    location_type character varying(255),
    location_type_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_location_type_sampling_contexts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_location_type_sampling_contexts (
    sample_location_type_sampling_context_id integer NOT NULL,
    sampling_context_id integer NOT NULL,
    sample_location_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_notes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_notes (
    sample_note_id integer NOT NULL,
    physical_sample_id integer NOT NULL,
    note_type character varying,
    note text NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_sample_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sample_types (
    sample_type_id integer NOT NULL,
    type_name character varying(40) NOT NULL,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_seasons; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_seasons (
    season_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    season_name character varying(20) DEFAULT NULL::character varying,
    season_type character varying(30) DEFAULT NULL::character varying,
    season_type_id integer,
    sort_order smallint DEFAULT 0
);


--
-- Name: tbl_season_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_season_types (
    season_type_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    season_type character varying(30)
);


--
-- Name: tbl_sites; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_sites (
    site_id integer NOT NULL,
    altitude numeric(18,10),
    latitude_dd numeric(18,10),
    longitude_dd numeric(18,10),
    national_site_identifier character varying(255),
    site_description text DEFAULT NULL::character varying,
    site_name character varying(60) DEFAULT NULL::character varying,
    site_preservation_status_id integer,
    site_location_accuracy character varying,
    date_updated timestamp with time zone DEFAULT now(),
    site_uuid text null
);


--
-- Name: tbl_site_images; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_site_images (
    site_image_id integer NOT NULL,
    contact_id integer,
    credit character varying(100),
    date_taken date,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    image_location text NOT NULL,
    image_name character varying(80),
    image_type_id integer NOT NULL,
    site_id integer NOT NULL
);


--
-- Name: tbl_site_locations; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_site_locations (
    site_location_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    location_id integer NOT NULL,
    site_id integer NOT NULL
);


--
-- Name: tbl_site_natgridrefs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_site_natgridrefs (
    site_natgridref_id integer NOT NULL,
    site_id integer NOT NULL,
    method_id integer NOT NULL,
    natgridref character varying NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_site_other_records; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_site_other_records (
    site_other_records_id integer NOT NULL,
    site_id integer,
    biblio_id integer,
    record_type_id integer,
    description text,
    date_updated timestamp with time zone DEFAULT now(),
    site_other_records_uuid text null
);


--
-- Name: tbl_site_preservation_status; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_site_preservation_status (
    site_preservation_status_id integer NOT NULL,
    site_id integer,
    preservation_status_or_threat character varying,
    description text,
    assessment_type character varying,
    assessment_author_contact_id integer,
    date_updated timestamp with time zone DEFAULT now(),
    "Evaluation_date" date
);


--
-- Name: tbl_site_references; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_site_references (
    site_reference_id integer NOT NULL,
    site_id integer DEFAULT 0,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_species_associations; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_species_associations (
    species_association_id integer NOT NULL,
    associated_taxon_id integer NOT NULL,
    biblio_id integer,
    date_updated timestamp with time zone DEFAULT now(),
    taxon_id integer NOT NULL,
    association_type_id integer,
    referencing_type text,
    species_association_uuid text null
);


--
-- Name: tbl_species_association_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_species_association_types (
    association_type_id integer NOT NULL,
    association_type_name character varying(255),
    association_description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_taxa_common_names; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_common_names (
    taxon_common_name_id integer NOT NULL,
    common_name character varying(255) DEFAULT NULL::character varying,
    date_updated timestamp with time zone DEFAULT now(),
    language_id integer DEFAULT 0,
    taxon_id integer DEFAULT 0
);


--
-- Name: tbl_taxa_images; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_images (
    taxa_images_id integer NOT NULL,
    image_name character varying,
    description text,
    image_location text,
    image_type_id integer,
    taxon_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_taxa_measured_attributes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_measured_attributes (
    measured_attribute_id integer NOT NULL,
    attribute_measure character varying(255) DEFAULT NULL::character varying,
    attribute_type character varying(255) DEFAULT NULL::character varying,
    attribute_units character varying(10) DEFAULT NULL::character varying,
    data numeric(18,10) DEFAULT 0,
    date_updated timestamp with time zone DEFAULT now(),
    taxon_id integer NOT NULL
);


--
-- Name: tbl_taxa_reference_specimens; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_reference_specimens (
    taxa_reference_specimen_id integer NOT NULL,
    taxon_id integer NOT NULL,
    contact_id integer NOT NULL,
    notes text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_taxa_seasonality; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_seasonality (
    seasonality_id integer NOT NULL,
    activity_type_id integer NOT NULL,
    season_id integer DEFAULT 0,
    taxon_id integer NOT NULL,
    location_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_taxa_synonyms; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_synonyms (
    synonym_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    family_id integer,
    genus_id integer,
    notes text DEFAULT NULL::character varying,
    taxon_id integer,
    author_id integer,
    synonym character varying(255),
    reference_type character varying,
    synonym_uuid text null
);


--
-- Name: tbl_taxa_tree_authors; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_tree_authors (
    author_id integer NOT NULL,
    author_name character varying(100) DEFAULT NULL::character varying,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: tbl_taxa_tree_families; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_tree_families (
    family_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    family_name character varying(100) DEFAULT NULL::character varying,
    order_id integer NOT NULL
);


--
-- Name: tbl_taxa_tree_genera; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_tree_genera (
    genus_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    family_id integer,
    genus_name character varying(100) DEFAULT NULL::character varying
);


--
-- Name: tbl_taxa_tree_master; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_tree_master (
    taxon_id integer NOT NULL,
    author_id integer,
    date_updated timestamp with time zone DEFAULT now(),
    genus_id integer,
    species character varying(255) DEFAULT NULL::character varying
);


--
-- Name: tbl_taxa_tree_orders; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxa_tree_orders (
    order_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    order_name character varying(50) DEFAULT NULL::character varying,
    record_type_id integer,
    sort_order integer
);


--
-- Name: tbl_taxonomic_order; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxonomic_order (
    taxonomic_order_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    taxon_id integer DEFAULT 0,
    taxonomic_code numeric(18,10) DEFAULT 0,
    taxonomic_order_system_id integer DEFAULT 0
);


--
-- Name: tbl_taxonomic_order_biblio; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxonomic_order_biblio (
    taxonomic_order_biblio_id integer NOT NULL,
    biblio_id integer DEFAULT 0,
    date_updated timestamp with time zone DEFAULT now(),
    taxonomic_order_system_id integer DEFAULT 0
);


--
-- Name: tbl_taxonomic_order_systems; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxonomic_order_systems (
    taxonomic_order_system_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    system_description text,
    system_name character varying(50) DEFAULT NULL::character varying,
    taxonomic_order_system_uuid text null
);


--
-- Name: tbl_taxonomy_notes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_taxonomy_notes (
    taxonomy_notes_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    taxon_id integer NOT NULL,
    taxonomy_notes text,
    taxonomy_notes_uuid text null
);


--
-- Name: tbl_temperatures; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_temperatures (
    record_id integer NOT NULL,
    years_bp integer NOT NULL,
    d180_gisp2 numeric
);


--
-- Name: tbl_tephras; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_tephras (
    tephra_id integer NOT NULL,
    c14_age numeric(20,5),
    c14_age_older numeric(20,5),
    c14_age_younger numeric(20,5),
    cal_age numeric(20,5),
    cal_age_older numeric(20,5),
    cal_age_younger numeric(20,5),
    date_updated timestamp with time zone DEFAULT now(),
    notes text,
    tephra_name character varying(80),
    tephra_uuid text null
);


--
-- Name: tbl_tephra_dates; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_tephra_dates (
    tephra_date_id integer NOT NULL,
    analysis_entity_id bigint NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    notes text,
    tephra_id integer NOT NULL,
    dating_uncertainty_id integer
);


--
-- Name: tbl_tephra_refs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_tephra_refs (
    tephra_ref_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    tephra_id integer NOT NULL
);


--
-- Name: tbl_text_biology; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_text_biology (
    biology_id integer NOT NULL,
    biblio_id integer NOT NULL,
    biology_text text,
    date_updated timestamp with time zone DEFAULT now(),
    taxon_id integer NOT NULL,
    biology_uuid text null
);


--
-- Name: tbl_text_distribution; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_text_distribution (
    distribution_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    distribution_text text,
    taxon_id integer NOT NULL,
    distribution_uuid text null
);


--
-- Name: tbl_text_identification_keys; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_text_identification_keys (
    key_id integer NOT NULL,
    biblio_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    key_text text,
    taxon_id integer NOT NULL,
    key_uuid text null
);


--
-- Name: tbl_units; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_units (
    unit_id integer NOT NULL,
    date_updated timestamp with time zone DEFAULT now(),
    description text,
    unit_abbrev character varying(15),
    unit_name character varying(50) NOT NULL
);


--
-- Name: tbl_updates_log; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_updates_log (
    updates_log_id integer NOT NULL,
    table_name character varying(150) NOT NULL,
    last_updated date NOT NULL
);


--
-- Name: tbl_value_classes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_value_classes (
    value_class_id integer NOT NULL,
    value_type_id integer NOT NULL,
    method_id integer NOT NULL,
    parent_id integer,
    name character varying(80) NOT NULL,
    description text NOT NULL,
    value_class_uuid text null
);


--
-- Name: tbl_value_qualifiers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_value_qualifiers (
    qualifier_id integer NOT NULL,
    symbol text NOT NULL,
    description text NOT NULL,
    qualifier_uuid text null
);


--
-- Name: tbl_value_qualifier_symbols; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_value_qualifier_symbols (
    qualifier_symbol_id integer NOT NULL,
    symbol text NOT NULL,
    cardinal_qualifier_id integer NOT NULL,
    qualifier_uuid text null
);


--
-- Name: tbl_value_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_value_types (
    value_type_id integer NOT NULL,
    unit_id integer,
    data_type_id integer,
    name text NOT NULL,
    base_type text NOT NULL,
    "precision" integer,
    description text NOT NULL,
    value_type_uuid text null
);


--
-- Name: tbl_value_type_items; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_value_type_items (
    value_type_item_id integer NOT NULL,
    value_type_id integer NOT NULL,
    name character varying(80) DEFAULT NULL::character varying,
    description text
);


--
-- Name: tbl_years_types; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tbl_years_types (
    years_type_id integer NOT NULL,
    name character varying NOT NULL,
    description text,
    date_updated timestamp with time zone DEFAULT now()
);


--
-- Name: table_dependency_levels(); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION public.table_dependency_levels() RETURNS TABLE(schema_name text, table_name text, level integer)
    LANGUAGE plpgsql
    AS $$
declare
	v_level int;
	v_count int;
begin

	drop table if exists table_level;
	drop view  if exists all_fks_aggs;
	drop view  if exists all_tables;
	drop view  if exists all_fks;

	create temporary view all_tables as (
		select s.nspname as schema_name, p.relname as table_name
		from pg_class p
		join pg_namespace s on s.oid = p.relnamespace
		where true
		  and p.relkind = 'r'
		  and s.nspname = 'public'
	);

	create temporary view all_fks as (
		select rs.nspname as schema_name, ref.relname as table_name, p.relname as referenced_table_name
		from pg_class ref
		join pg_namespace rs on rs.oid = ref.relnamespace
		join pg_constraint c on c.contype = 'f' and c.conrelid = ref.oid
		join pg_class p on p.oid = c.confrelid
		join pg_namespace s on s.oid = p.relnamespace
		where true
		  and rs.nspname = 'public'
		  and ref.relname <> p.relname
	);

	create temporary view all_fks_aggs as
		select table_name, array_agg(referenced_table_name) as referenced_table_names
		from all_fks
		group by table_name;

	create temporary table table_level as

		/* All tables with no dependendencies */
		select t.schema_name::text as schema_name, t.table_name::text as table_name, 0::int as level
		from all_tables t
		left join all_fks r
		  on t.schema_name = r.schema_name
		 and t.table_name = r.table_name
		where r.table_name is null;

	v_level = 0;

	loop

		v_level = v_level + 1;

		insert into table_level
			with processed_count as (
				select f.schema_name, f.table_name
				from all_fks f
				left join table_level x
				  on x.schema_name = f.schema_name
				 and x.table_name = f.referenced_table_name
				group by f.schema_name, f.table_name
				having count(f.referenced_table_name) = count(x.table_name)
			) select processed_count.schema_name, processed_count.table_name, v_level
			  from processed_count
			  left join table_level using (schema_name, table_name)
			  where table_level.table_name is null;


		get diagnostics v_count = row_count;

		exit when v_count = 0 or v_level = 10;

		-- raise notice '%', v_level;

	end loop;

	return query
		select t.schema_name, t.table_name, t.level
		from table_level t;

end $$;


--
-- Name: adna_analysis_values; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.adna_analysis_values AS
 WITH datasets AS (
         SELECT ds.dataset_id
           FROM public.tbl_datasets ds
          WHERE (true AND (ds.method_id IN ( SELECT m.method_id
                   FROM (public.tbl_methods m
                     JOIN public.tbl_record_types r USING (record_type_id))
                  WHERE ((r.record_type_name)::text = 'DNA'::text))) AND (ds.project_id IN ( SELECT tbl_projects.project_id
                   FROM public.tbl_projects
                  WHERE ((tbl_projects.project_name)::text = 'SciLifeLab Ancient DNA Unit project 017'::text))))
        ), analysis_entities AS (
         SELECT ae.analysis_entity_id
           FROM (public.tbl_analysis_entities ae
             JOIN datasets USING (dataset_id))
        ), analysis_values AS (
         SELECT av.analysis_value_id,
            av.analysis_entity_id,
            av.value_class_id,
            av.analysis_value,
            vt_1.base_type
           FROM (((public.tbl_analysis_values av
             JOIN public.tbl_value_classes vs USING (value_class_id))
             JOIN public.tbl_value_types vt_1 USING (value_type_id))
             JOIN analysis_entities USING (analysis_entity_id))
        ), typed_values AS (
         SELECT analysis_values_1.analysis_value_id,
                CASE
                    WHEN (analysis_values_1.base_type = 'integer'::text) THEN
                    CASE
                        WHEN sead_utility.is_integer(analysis_values_1.analysis_value) THEN (analysis_values_1.analysis_value)::integer
                        WHEN sead_utility.is_numeric(analysis_values_1.analysis_value) THEN (round((analysis_values_1.analysis_value)::numeric(20,10)))::integer
                        ELSE NULL::integer
                    END
                    ELSE NULL::integer
                END AS integer_value,
                CASE
                    WHEN ((analysis_values_1.base_type = 'integer'::text) AND (NOT sead_utility.is_integer(analysis_values_1.analysis_value)) AND (NOT sead_utility.is_numeric(analysis_values_1.analysis_value))) THEN true
                    ELSE NULL::boolean
                END AS integer_is_anomaly,
                CASE
                    WHEN ((analysis_values_1.base_type = 'decimal'::text) AND sead_utility.is_numeric(analysis_values_1.analysis_value)) THEN (analysis_values_1.analysis_value)::numeric(20,10)
                    ELSE NULL::numeric
                END AS decimal_value,
                CASE
                    WHEN ((analysis_values_1.base_type = 'decimal'::text) AND (NOT sead_utility.is_numeric(analysis_values_1.analysis_value))) THEN true
                    ELSE NULL::boolean
                END AS decimal_is_anomaly
           FROM analysis_values analysis_values_1
        )
 SELECT analysis_values.analysis_value_id,
    analysis_values.analysis_value,
    analysis_values.value_class_id,
    vc.name AS value_class_name,
    vt.name AS value_type_name,
    vt.base_type,
    tv.decimal_value,
    tv.decimal_is_anomaly,
    tv.integer_value,
    tv.integer_is_anomaly
   FROM (((analysis_values
     JOIN typed_values tv USING (analysis_value_id))
     JOIN public.tbl_value_classes vc USING (value_class_id))
     JOIN public.tbl_value_types vt USING (value_type_id));


--
-- Name: tbl_abundance_elements_abundance_element_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_abundance_elements_abundance_element_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_abundance_elements_abundance_element_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_abundance_elements_abundance_element_id_seq OWNED BY public.tbl_abundance_elements.abundance_element_id;


--
-- Name: tbl_abundance_ident_levels_abundance_ident_level_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_abundance_ident_levels_abundance_ident_level_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_abundance_ident_levels_abundance_ident_level_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_abundance_ident_levels_abundance_ident_level_id_seq OWNED BY public.tbl_abundance_ident_levels.abundance_ident_level_id;


--
-- Name: tbl_abundance_modifications_abundance_modification_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_abundance_modifications_abundance_modification_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_abundance_modifications_abundance_modification_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_abundance_modifications_abundance_modification_id_seq OWNED BY public.tbl_abundance_modifications.abundance_modification_id;


--
-- Name: tbl_abundances_abundance_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_abundances_abundance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_abundances_abundance_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_abundances_abundance_id_seq OWNED BY public.tbl_abundances.abundance_id;


--
-- Name: tbl_activity_types_activity_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_activity_types_activity_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_activity_types_activity_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_activity_types_activity_type_id_seq OWNED BY public.tbl_activity_types.activity_type_id;


--
-- Name: tbl_age_types_age_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_age_types_age_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_age_types_age_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_age_types_age_type_id_seq OWNED BY public.tbl_age_types.age_type_id;


--
-- Name: tbl_aggregate_datasets_aggregate_dataset_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_aggregate_datasets_aggregate_dataset_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_aggregate_datasets_aggregate_dataset_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_aggregate_datasets_aggregate_dataset_id_seq OWNED BY public.tbl_aggregate_datasets.aggregate_dataset_id;


--
-- Name: tbl_aggregate_order_types_aggregate_order_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_aggregate_order_types_aggregate_order_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_aggregate_order_types_aggregate_order_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_aggregate_order_types_aggregate_order_type_id_seq OWNED BY public.tbl_aggregate_order_types.aggregate_order_type_id;


--
-- Name: tbl_aggregate_sample_ages_aggregate_sample_age_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_aggregate_sample_ages_aggregate_sample_age_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_aggregate_sample_ages_aggregate_sample_age_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_aggregate_sample_ages_aggregate_sample_age_id_seq OWNED BY public.tbl_aggregate_sample_ages.aggregate_sample_age_id;


--
-- Name: tbl_aggregate_samples_aggregate_sample_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_aggregate_samples_aggregate_sample_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_aggregate_samples_aggregate_sample_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_aggregate_samples_aggregate_sample_id_seq OWNED BY public.tbl_aggregate_samples.aggregate_sample_id;


--
-- Name: tbl_alt_ref_types_alt_ref_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_alt_ref_types_alt_ref_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_alt_ref_types_alt_ref_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_alt_ref_types_alt_ref_type_id_seq OWNED BY public.tbl_alt_ref_types.alt_ref_type_id;


--
-- Name: tbl_analysis_boolean_values_analysis_boolean_value_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_boolean_values_analysis_boolean_value_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_boolean_values_analysis_boolean_value_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_boolean_values_analysis_boolean_value_id_seq OWNED BY public.tbl_analysis_boolean_values.analysis_boolean_value_id;


--
-- Name: tbl_analysis_categorical_valu_analysis_categorical_value_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_categorical_valu_analysis_categorical_value_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_categorical_valu_analysis_categorical_value_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_categorical_valu_analysis_categorical_value_id_seq OWNED BY public.tbl_analysis_categorical_values.analysis_categorical_value_id;


--
-- Name: tbl_analysis_dating_ranges_analysis_dating_range_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_dating_ranges_analysis_dating_range_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_dating_ranges_analysis_dating_range_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_dating_ranges_analysis_dating_range_id_seq OWNED BY public.tbl_analysis_dating_ranges.analysis_dating_range_id;


--
-- Name: tbl_analysis_entities_analysis_entity_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_entities_analysis_entity_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_entities_analysis_entity_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_entities_analysis_entity_id_seq OWNED BY public.tbl_analysis_entities.analysis_entity_id;


--
-- Name: tbl_analysis_entity_ages_analysis_entity_age_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_entity_ages_analysis_entity_age_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_entity_ages_analysis_entity_age_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_entity_ages_analysis_entity_age_id_seq OWNED BY public.tbl_analysis_entity_ages.analysis_entity_age_id;


--
-- Name: tbl_analysis_entity_dimensions_analysis_entity_dimension_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_entity_dimensions_analysis_entity_dimension_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_entity_dimensions_analysis_entity_dimension_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_entity_dimensions_analysis_entity_dimension_id_seq OWNED BY public.tbl_analysis_entity_dimensions.analysis_entity_dimension_id;


--
-- Name: tbl_analysis_entity_prep_meth_analysis_entity_prep_method_i_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_entity_prep_meth_analysis_entity_prep_method_i_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_entity_prep_meth_analysis_entity_prep_method_i_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_entity_prep_meth_analysis_entity_prep_method_i_seq OWNED BY public.tbl_analysis_entity_prep_methods.analysis_entity_prep_method_id;


--
-- Name: tbl_analysis_identifiers_analysis_identifier_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_identifiers_analysis_identifier_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_identifiers_analysis_identifier_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_identifiers_analysis_identifier_id_seq OWNED BY public.tbl_analysis_identifiers.analysis_identifier_id;


--
-- Name: tbl_analysis_integer_ranges_analysis_integer_range_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_integer_ranges_analysis_integer_range_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_integer_ranges_analysis_integer_range_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_integer_ranges_analysis_integer_range_id_seq OWNED BY public.tbl_analysis_integer_ranges.analysis_integer_range_id;


--
-- Name: tbl_analysis_integer_values_analysis_integer_value_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_integer_values_analysis_integer_value_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_integer_values_analysis_integer_value_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_integer_values_analysis_integer_value_id_seq OWNED BY public.tbl_analysis_integer_values.analysis_integer_value_id;


--
-- Name: tbl_analysis_notes_analysis_note_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_notes_analysis_note_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_notes_analysis_note_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_notes_analysis_note_id_seq OWNED BY public.tbl_analysis_notes.analysis_note_id;


--
-- Name: tbl_analysis_numerical_ranges_analysis_numerical_range_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_numerical_ranges_analysis_numerical_range_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_numerical_ranges_analysis_numerical_range_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_numerical_ranges_analysis_numerical_range_id_seq OWNED BY public.tbl_analysis_numerical_ranges.analysis_numerical_range_id;


--
-- Name: tbl_analysis_numerical_values_analysis_numerical_value_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_numerical_values_analysis_numerical_value_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_numerical_values_analysis_numerical_value_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_numerical_values_analysis_numerical_value_id_seq OWNED BY public.tbl_analysis_numerical_values.analysis_numerical_value_id;


--
-- Name: tbl_analysis_taxon_counts_analysis_taxon_count_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_taxon_counts_analysis_taxon_count_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_taxon_counts_analysis_taxon_count_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_taxon_counts_analysis_taxon_count_id_seq OWNED BY public.tbl_analysis_taxon_counts.analysis_taxon_count_id;


--
-- Name: tbl_analysis_value_dimensions_analysis_value_dimension_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_value_dimensions_analysis_value_dimension_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_value_dimensions_analysis_value_dimension_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_value_dimensions_analysis_value_dimension_id_seq OWNED BY public.tbl_analysis_value_dimensions.analysis_value_dimension_id;


--
-- Name: tbl_analysis_values_analysis_value_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_analysis_values_analysis_value_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_analysis_values_analysis_value_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_analysis_values_analysis_value_id_seq OWNED BY public.tbl_analysis_values.analysis_value_id;


--
-- Name: tbl_biblio_biblio_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_biblio_biblio_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_biblio_biblio_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_biblio_biblio_id_seq OWNED BY public.tbl_biblio.biblio_id;


--
-- Name: tbl_ceramics_ceramics_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_ceramics_ceramics_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_ceramics_ceramics_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_ceramics_ceramics_id_seq OWNED BY public.tbl_ceramics.ceramics_id;


--
-- Name: tbl_ceramics_lookup_ceramics_lookup_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_ceramics_lookup_ceramics_lookup_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_ceramics_lookup_ceramics_lookup_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_ceramics_lookup_ceramics_lookup_id_seq OWNED BY public.tbl_ceramics_lookup.ceramics_lookup_id;


--
-- Name: tbl_ceramics_measurements_ceramics_measurement_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_ceramics_measurements_ceramics_measurement_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_ceramics_measurements_ceramics_measurement_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_ceramics_measurements_ceramics_measurement_id_seq OWNED BY public.tbl_ceramics_measurements.ceramics_measurement_id;


--
-- Name: tbl_chronologies_chronology_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_chronologies_chronology_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_chronologies_chronology_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_chronologies_chronology_id_seq OWNED BY public.tbl_chronologies.chronology_id;


--
-- Name: tbl_colours_colour_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_colours_colour_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_colours_colour_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_colours_colour_id_seq OWNED BY public.tbl_colours.colour_id;


--
-- Name: tbl_contact_types_contact_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_contact_types_contact_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_contact_types_contact_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_contact_types_contact_type_id_seq OWNED BY public.tbl_contact_types.contact_type_id;


--
-- Name: tbl_contacts_contact_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_contacts_contact_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_contacts_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_contacts_contact_id_seq OWNED BY public.tbl_contacts.contact_id;


--
-- Name: tbl_coordinate_method_dimensi_coordinate_method_dimension_i_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_coordinate_method_dimensi_coordinate_method_dimension_i_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_coordinate_method_dimensi_coordinate_method_dimension_i_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_coordinate_method_dimensi_coordinate_method_dimension_i_seq OWNED BY public.tbl_coordinate_method_dimensions.coordinate_method_dimension_id;


--
-- Name: tbl_data_type_groups_data_type_group_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_data_type_groups_data_type_group_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_data_type_groups_data_type_group_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_data_type_groups_data_type_group_id_seq OWNED BY public.tbl_data_type_groups.data_type_group_id;


--
-- Name: tbl_data_types_data_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_data_types_data_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_data_types_data_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_data_types_data_type_id_seq OWNED BY public.tbl_data_types.data_type_id;


--
-- Name: tbl_dataset_contacts_dataset_contact_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dataset_contacts_dataset_contact_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dataset_contacts_dataset_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dataset_contacts_dataset_contact_id_seq OWNED BY public.tbl_dataset_contacts.dataset_contact_id;


--
-- Name: tbl_dataset_masters_master_set_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dataset_masters_master_set_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dataset_masters_master_set_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dataset_masters_master_set_id_seq OWNED BY public.tbl_dataset_masters.master_set_id;


--
-- Name: tbl_dataset_methods_dataset_method_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dataset_methods_dataset_method_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dataset_methods_dataset_method_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dataset_methods_dataset_method_id_seq OWNED BY public.tbl_dataset_methods.dataset_method_id;


--
-- Name: tbl_dataset_submission_types_submission_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dataset_submission_types_submission_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dataset_submission_types_submission_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dataset_submission_types_submission_type_id_seq OWNED BY public.tbl_dataset_submission_types.submission_type_id;


--
-- Name: tbl_dataset_submissions_dataset_submission_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dataset_submissions_dataset_submission_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dataset_submissions_dataset_submission_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dataset_submissions_dataset_submission_id_seq OWNED BY public.tbl_dataset_submissions.dataset_submission_id;


--
-- Name: tbl_datasets_dataset_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_datasets_dataset_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_datasets_dataset_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_datasets_dataset_id_seq OWNED BY public.tbl_datasets.dataset_id;


--
-- Name: tbl_dating_labs_dating_lab_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dating_labs_dating_lab_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dating_labs_dating_lab_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dating_labs_dating_lab_id_seq OWNED BY public.tbl_dating_labs.dating_lab_id;


--
-- Name: tbl_dating_material_dating_material_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dating_material_dating_material_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dating_material_dating_material_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dating_material_dating_material_id_seq OWNED BY public.tbl_dating_material.dating_material_id;


--
-- Name: tbl_dating_uncertainty_dating_uncertainty_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dating_uncertainty_dating_uncertainty_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dating_uncertainty_dating_uncertainty_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dating_uncertainty_dating_uncertainty_id_seq OWNED BY public.tbl_dating_uncertainty.dating_uncertainty_id;


--
-- Name: tbl_dendro_date_notes_dendro_date_note_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dendro_date_notes_dendro_date_note_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dendro_date_notes_dendro_date_note_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dendro_date_notes_dendro_date_note_id_seq OWNED BY public.tbl_dendro_date_notes.dendro_date_note_id;


--
-- Name: tbl_dendro_dates_dendro_date_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dendro_dates_dendro_date_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dendro_dates_dendro_date_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dendro_dates_dendro_date_id_seq OWNED BY public.tbl_dendro_dates.dendro_date_id;


--
-- Name: tbl_dendro_dendro_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dendro_dendro_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dendro_dendro_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dendro_dendro_id_seq OWNED BY public.tbl_dendro.dendro_id;


--
-- Name: tbl_dendro_lookup_dendro_lookup_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dendro_lookup_dendro_lookup_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dendro_lookup_dendro_lookup_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dendro_lookup_dendro_lookup_id_seq OWNED BY public.tbl_dendro_lookup.dendro_lookup_id;


--
-- Name: tbl_dimensions_dimension_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_dimensions_dimension_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_dimensions_dimension_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_dimensions_dimension_id_seq OWNED BY public.tbl_dimensions.dimension_id;


--
-- Name: tbl_ecocode_definitions_ecocode_definition_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_ecocode_definitions_ecocode_definition_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_ecocode_definitions_ecocode_definition_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_ecocode_definitions_ecocode_definition_id_seq OWNED BY public.tbl_ecocode_definitions.ecocode_definition_id;


--
-- Name: tbl_ecocode_groups_ecocode_group_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_ecocode_groups_ecocode_group_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_ecocode_groups_ecocode_group_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_ecocode_groups_ecocode_group_id_seq OWNED BY public.tbl_ecocode_groups.ecocode_group_id;


--
-- Name: tbl_ecocode_systems_ecocode_system_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_ecocode_systems_ecocode_system_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_ecocode_systems_ecocode_system_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_ecocode_systems_ecocode_system_id_seq OWNED BY public.tbl_ecocode_systems.ecocode_system_id;


--
-- Name: tbl_ecocodes_ecocode_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_ecocodes_ecocode_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_ecocodes_ecocode_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_ecocodes_ecocode_id_seq OWNED BY public.tbl_ecocodes.ecocode_id;


--
-- Name: tbl_feature_types_feature_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_feature_types_feature_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_feature_types_feature_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_feature_types_feature_type_id_seq OWNED BY public.tbl_feature_types.feature_type_id;


--
-- Name: tbl_features_feature_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_features_feature_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_features_feature_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_features_feature_id_seq OWNED BY public.tbl_features.feature_id;


--
-- Name: tbl_geochron_refs_geochron_ref_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_geochron_refs_geochron_ref_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_geochron_refs_geochron_ref_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_geochron_refs_geochron_ref_id_seq OWNED BY public.tbl_geochron_refs.geochron_ref_id;


--
-- Name: tbl_geochronology_geochron_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_geochronology_geochron_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_geochronology_geochron_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_geochronology_geochron_id_seq OWNED BY public.tbl_geochronology.geochron_id;


--
-- Name: tbl_horizons_horizon_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_horizons_horizon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_horizons_horizon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_horizons_horizon_id_seq OWNED BY public.tbl_horizons.horizon_id;


--
-- Name: tbl_identification_levels_identification_level_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_identification_levels_identification_level_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_identification_levels_identification_level_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_identification_levels_identification_level_id_seq OWNED BY public.tbl_identification_levels.identification_level_id;


--
-- Name: tbl_image_types_image_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_image_types_image_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_image_types_image_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_image_types_image_type_id_seq OWNED BY public.tbl_image_types.image_type_id;


--
-- Name: tbl_imported_taxa_replacements_imported_taxa_replacement_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_imported_taxa_replacements_imported_taxa_replacement_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_imported_taxa_replacements_imported_taxa_replacement_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_imported_taxa_replacements_imported_taxa_replacement_id_seq OWNED BY public.tbl_imported_taxa_replacements.imported_taxa_replacement_id;


--
-- Name: tbl_isotope_measurements_isotope_measurement_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_isotope_measurements_isotope_measurement_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_isotope_measurements_isotope_measurement_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_isotope_measurements_isotope_measurement_id_seq OWNED BY public.tbl_isotope_measurements.isotope_measurement_id;


--
-- Name: tbl_isotope_standards_isotope_standard_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_isotope_standards_isotope_standard_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_isotope_standards_isotope_standard_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_isotope_standards_isotope_standard_id_seq OWNED BY public.tbl_isotope_standards.isotope_standard_id;


--
-- Name: tbl_isotope_types_isotope_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_isotope_types_isotope_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_isotope_types_isotope_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_isotope_types_isotope_type_id_seq OWNED BY public.tbl_isotope_types.isotope_type_id;


--
-- Name: tbl_isotopes_isotope_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_isotopes_isotope_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_isotopes_isotope_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_isotopes_isotope_id_seq OWNED BY public.tbl_isotopes.isotope_id;


--
-- Name: tbl_languages_language_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_languages_language_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_languages_language_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_languages_language_id_seq OWNED BY public.tbl_languages.language_id;


--
-- Name: tbl_lithology_lithology_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_lithology_lithology_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_lithology_lithology_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_lithology_lithology_id_seq OWNED BY public.tbl_lithology.lithology_id;


--
-- Name: tbl_location_types_location_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_location_types_location_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_location_types_location_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_location_types_location_type_id_seq OWNED BY public.tbl_location_types.location_type_id;


--
-- Name: tbl_locations_location_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_locations_location_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_locations_location_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_locations_location_id_seq OWNED BY public.tbl_locations.location_id;


--
-- Name: tbl_mcr_names_taxon_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_mcr_names_taxon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_mcr_names_taxon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_mcr_names_taxon_id_seq OWNED BY public.tbl_mcr_names.taxon_id;


--
-- Name: tbl_mcr_summary_data_mcr_summary_data_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_mcr_summary_data_mcr_summary_data_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_mcr_summary_data_mcr_summary_data_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_mcr_summary_data_mcr_summary_data_id_seq OWNED BY public.tbl_mcr_summary_data.mcr_summary_data_id;


--
-- Name: tbl_mcrdata_birmbeetledat_mcrdata_birmbeetledat_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_mcrdata_birmbeetledat_mcrdata_birmbeetledat_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_mcrdata_birmbeetledat_mcrdata_birmbeetledat_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_mcrdata_birmbeetledat_mcrdata_birmbeetledat_id_seq OWNED BY public.tbl_mcrdata_birmbeetledat.mcrdata_birmbeetledat_id;


--
-- Name: tbl_measured_value_dimensions_measured_value_dimension_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_measured_value_dimensions_measured_value_dimension_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_measured_value_dimensions_measured_value_dimension_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_measured_value_dimensions_measured_value_dimension_id_seq OWNED BY public.tbl_measured_value_dimensions.measured_value_dimension_id;


--
-- Name: tbl_measured_values_measured_value_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_measured_values_measured_value_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_measured_values_measured_value_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_measured_values_measured_value_id_seq OWNED BY public.tbl_measured_values.measured_value_id;


--
-- Name: tbl_method_groups_method_group_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_method_groups_method_group_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_method_groups_method_group_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_method_groups_method_group_id_seq OWNED BY public.tbl_method_groups.method_group_id;


--
-- Name: tbl_methods_method_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_methods_method_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_methods_method_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_methods_method_id_seq OWNED BY public.tbl_methods.method_id;


--
-- Name: tbl_modification_types_modification_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_modification_types_modification_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_modification_types_modification_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_modification_types_modification_type_id_seq OWNED BY public.tbl_modification_types.modification_type_id;


--
-- Name: tbl_physical_sample_features_physical_sample_feature_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_physical_sample_features_physical_sample_feature_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_physical_sample_features_physical_sample_feature_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_physical_sample_features_physical_sample_feature_id_seq OWNED BY public.tbl_physical_sample_features.physical_sample_feature_id;


--
-- Name: tbl_physical_samples_physical_sample_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_physical_samples_physical_sample_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_physical_samples_physical_sample_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_physical_samples_physical_sample_id_seq OWNED BY public.tbl_physical_samples.physical_sample_id;


--
-- Name: tbl_project_stages_project_stage_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_project_stages_project_stage_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_project_stages_project_stage_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_project_stages_project_stage_id_seq OWNED BY public.tbl_project_stages.project_stage_id;


--
-- Name: tbl_project_types_project_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_project_types_project_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_project_types_project_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_project_types_project_type_id_seq OWNED BY public.tbl_project_types.project_type_id;


--
-- Name: tbl_projects_project_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_projects_project_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_projects_project_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_projects_project_id_seq OWNED BY public.tbl_projects.project_id;


--
-- Name: tbl_rdb_codes_rdb_code_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_rdb_codes_rdb_code_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_rdb_codes_rdb_code_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_rdb_codes_rdb_code_id_seq OWNED BY public.tbl_rdb_codes.rdb_code_id;


--
-- Name: tbl_rdb_rdb_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_rdb_rdb_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_rdb_rdb_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_rdb_rdb_id_seq OWNED BY public.tbl_rdb.rdb_id;


--
-- Name: tbl_rdb_systems_rdb_system_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_rdb_systems_rdb_system_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_rdb_systems_rdb_system_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_rdb_systems_rdb_system_id_seq OWNED BY public.tbl_rdb_systems.rdb_system_id;


--
-- Name: tbl_record_types_record_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_record_types_record_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_record_types_record_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_record_types_record_type_id_seq OWNED BY public.tbl_record_types.record_type_id;


--
-- Name: tbl_relative_age_refs_relative_age_ref_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_relative_age_refs_relative_age_ref_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_relative_age_refs_relative_age_ref_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_relative_age_refs_relative_age_ref_id_seq OWNED BY public.tbl_relative_age_refs.relative_age_ref_id;


--
-- Name: tbl_relative_age_types_relative_age_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_relative_age_types_relative_age_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_relative_age_types_relative_age_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_relative_age_types_relative_age_type_id_seq OWNED BY public.tbl_relative_age_types.relative_age_type_id;


--
-- Name: tbl_relative_ages_relative_age_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_relative_ages_relative_age_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_relative_ages_relative_age_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_relative_ages_relative_age_id_seq OWNED BY public.tbl_relative_ages.relative_age_id;


--
-- Name: tbl_relative_dates_relative_date_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_relative_dates_relative_date_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_relative_dates_relative_date_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_relative_dates_relative_date_id_seq OWNED BY public.tbl_relative_dates.relative_date_id;


--
-- Name: tbl_sample_alt_refs_sample_alt_ref_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_alt_refs_sample_alt_ref_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_alt_refs_sample_alt_ref_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_alt_refs_sample_alt_ref_id_seq OWNED BY public.tbl_sample_alt_refs.sample_alt_ref_id;


--
-- Name: tbl_sample_colours_sample_colour_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_colours_sample_colour_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_colours_sample_colour_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_colours_sample_colour_id_seq OWNED BY public.tbl_sample_colours.sample_colour_id;


--
-- Name: tbl_sample_coordinates_sample_coordinate_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_coordinates_sample_coordinate_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_coordinates_sample_coordinate_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_coordinates_sample_coordinate_id_seq OWNED BY public.tbl_sample_coordinates.sample_coordinate_id;


--
-- Name: tbl_sample_description_sample_sample_description_sample_gro_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_description_sample_sample_description_sample_gro_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_description_sample_sample_description_sample_gro_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_description_sample_sample_description_sample_gro_seq OWNED BY public.tbl_sample_description_sample_group_contexts.sample_description_sample_group_context_id;


--
-- Name: tbl_sample_description_types_sample_description_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_description_types_sample_description_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_description_types_sample_description_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_description_types_sample_description_type_id_seq OWNED BY public.tbl_sample_description_types.sample_description_type_id;


--
-- Name: tbl_sample_descriptions_sample_description_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_descriptions_sample_description_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_descriptions_sample_description_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_descriptions_sample_description_id_seq OWNED BY public.tbl_sample_descriptions.sample_description_id;


--
-- Name: tbl_sample_dimensions_sample_dimension_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_dimensions_sample_dimension_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_dimensions_sample_dimension_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_dimensions_sample_dimension_id_seq OWNED BY public.tbl_sample_dimensions.sample_dimension_id;


--
-- Name: tbl_sample_group_coordinates_sample_group_position_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_coordinates_sample_group_position_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_coordinates_sample_group_position_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_coordinates_sample_group_position_id_seq OWNED BY public.tbl_sample_group_coordinates.sample_group_position_id;


--
-- Name: tbl_sample_group_description__sample_group_description_typ_seq1; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_description__sample_group_description_typ_seq1
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_description__sample_group_description_typ_seq1; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_description__sample_group_description_typ_seq1 OWNED BY public.tbl_sample_group_description_types.sample_group_description_type_id;


--
-- Name: tbl_sample_group_description__sample_group_description_type_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_description__sample_group_description_type_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_description__sample_group_description_type_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_description__sample_group_description_type_seq OWNED BY public.tbl_sample_group_description_type_sampling_contexts.sample_group_description_type_sampling_context_id;


--
-- Name: tbl_sample_group_descriptions_sample_group_description_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_descriptions_sample_group_description_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_descriptions_sample_group_description_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_descriptions_sample_group_description_id_seq OWNED BY public.tbl_sample_group_descriptions.sample_group_description_id;


--
-- Name: tbl_sample_group_dimensions_sample_group_dimension_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_dimensions_sample_group_dimension_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_dimensions_sample_group_dimension_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_dimensions_sample_group_dimension_id_seq OWNED BY public.tbl_sample_group_dimensions.sample_group_dimension_id;


--
-- Name: tbl_sample_group_images_sample_group_image_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_images_sample_group_image_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_images_sample_group_image_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_images_sample_group_image_id_seq OWNED BY public.tbl_sample_group_images.sample_group_image_id;


--
-- Name: tbl_sample_group_notes_sample_group_note_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_notes_sample_group_note_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_notes_sample_group_note_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_notes_sample_group_note_id_seq OWNED BY public.tbl_sample_group_notes.sample_group_note_id;


--
-- Name: tbl_sample_group_references_sample_group_reference_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_references_sample_group_reference_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_references_sample_group_reference_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_references_sample_group_reference_id_seq OWNED BY public.tbl_sample_group_references.sample_group_reference_id;


--
-- Name: tbl_sample_group_sampling_contexts_sampling_context_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_group_sampling_contexts_sampling_context_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_group_sampling_contexts_sampling_context_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_group_sampling_contexts_sampling_context_id_seq OWNED BY public.tbl_sample_group_sampling_contexts.sampling_context_id;


--
-- Name: tbl_sample_groups_sample_group_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_groups_sample_group_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_groups_sample_group_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_groups_sample_group_id_seq OWNED BY public.tbl_sample_groups.sample_group_id;


--
-- Name: tbl_sample_horizons_sample_horizon_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_horizons_sample_horizon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_horizons_sample_horizon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_horizons_sample_horizon_id_seq OWNED BY public.tbl_sample_horizons.sample_horizon_id;


--
-- Name: tbl_sample_images_sample_image_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_images_sample_image_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_images_sample_image_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_images_sample_image_id_seq OWNED BY public.tbl_sample_images.sample_image_id;


--
-- Name: tbl_sample_location_type_samp_sample_location_type_sampling_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_location_type_samp_sample_location_type_sampling_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_location_type_samp_sample_location_type_sampling_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_location_type_samp_sample_location_type_sampling_seq OWNED BY public.tbl_sample_location_type_sampling_contexts.sample_location_type_sampling_context_id;


--
-- Name: tbl_sample_location_types_sample_location_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_location_types_sample_location_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_location_types_sample_location_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_location_types_sample_location_type_id_seq OWNED BY public.tbl_sample_location_types.sample_location_type_id;


--
-- Name: tbl_sample_locations_sample_location_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_locations_sample_location_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_locations_sample_location_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_locations_sample_location_id_seq OWNED BY public.tbl_sample_locations.sample_location_id;


--
-- Name: tbl_sample_notes_sample_note_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_notes_sample_note_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_notes_sample_note_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_notes_sample_note_id_seq OWNED BY public.tbl_sample_notes.sample_note_id;


--
-- Name: tbl_sample_types_sample_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sample_types_sample_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sample_types_sample_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sample_types_sample_type_id_seq OWNED BY public.tbl_sample_types.sample_type_id;


--
-- Name: tbl_season_types_season_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_season_types_season_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_season_types_season_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_season_types_season_type_id_seq OWNED BY public.tbl_season_types.season_type_id;


--
-- Name: tbl_seasons_season_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_seasons_season_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_seasons_season_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_seasons_season_id_seq OWNED BY public.tbl_seasons.season_id;


--
-- Name: tbl_site_images_site_image_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_site_images_site_image_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_site_images_site_image_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_site_images_site_image_id_seq OWNED BY public.tbl_site_images.site_image_id;


--
-- Name: tbl_site_locations_site_location_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_site_locations_site_location_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_site_locations_site_location_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_site_locations_site_location_id_seq OWNED BY public.tbl_site_locations.site_location_id;


--
-- Name: tbl_site_natgridrefs_site_natgridref_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_site_natgridrefs_site_natgridref_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_site_natgridrefs_site_natgridref_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_site_natgridrefs_site_natgridref_id_seq OWNED BY public.tbl_site_natgridrefs.site_natgridref_id;


--
-- Name: tbl_site_other_records_site_other_records_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_site_other_records_site_other_records_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_site_other_records_site_other_records_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_site_other_records_site_other_records_id_seq OWNED BY public.tbl_site_other_records.site_other_records_id;


--
-- Name: tbl_site_preservation_status_site_preservation_status_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_site_preservation_status_site_preservation_status_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_site_preservation_status_site_preservation_status_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_site_preservation_status_site_preservation_status_id_seq OWNED BY public.tbl_site_preservation_status.site_preservation_status_id;


--
-- Name: tbl_site_references_site_reference_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_site_references_site_reference_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_site_references_site_reference_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_site_references_site_reference_id_seq OWNED BY public.tbl_site_references.site_reference_id;


--
-- Name: tbl_sites_site_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_sites_site_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_sites_site_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_sites_site_id_seq OWNED BY public.tbl_sites.site_id;


--
-- Name: tbl_species_association_types_association_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_species_association_types_association_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_species_association_types_association_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_species_association_types_association_type_id_seq OWNED BY public.tbl_species_association_types.association_type_id;


--
-- Name: tbl_species_associations_species_association_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_species_associations_species_association_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_species_associations_species_association_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_species_associations_species_association_id_seq OWNED BY public.tbl_species_associations.species_association_id;


--
-- Name: tbl_taxa_common_names_taxon_common_name_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_common_names_taxon_common_name_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_common_names_taxon_common_name_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_common_names_taxon_common_name_id_seq OWNED BY public.tbl_taxa_common_names.taxon_common_name_id;


--
-- Name: tbl_taxa_images_taxa_images_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_images_taxa_images_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_images_taxa_images_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_images_taxa_images_id_seq OWNED BY public.tbl_taxa_images.taxa_images_id;


--
-- Name: tbl_taxa_measured_attributes_measured_attribute_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_measured_attributes_measured_attribute_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_measured_attributes_measured_attribute_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_measured_attributes_measured_attribute_id_seq OWNED BY public.tbl_taxa_measured_attributes.measured_attribute_id;


--
-- Name: tbl_taxa_reference_specimens_taxa_reference_specimen_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_reference_specimens_taxa_reference_specimen_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_reference_specimens_taxa_reference_specimen_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_reference_specimens_taxa_reference_specimen_id_seq OWNED BY public.tbl_taxa_reference_specimens.taxa_reference_specimen_id;


--
-- Name: tbl_taxa_seasonality_seasonality_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_seasonality_seasonality_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_seasonality_seasonality_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_seasonality_seasonality_id_seq OWNED BY public.tbl_taxa_seasonality.seasonality_id;


--
-- Name: tbl_taxa_synonyms_synonym_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_synonyms_synonym_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_synonyms_synonym_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_synonyms_synonym_id_seq OWNED BY public.tbl_taxa_synonyms.synonym_id;


--
-- Name: tbl_taxa_tree_authors_author_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_tree_authors_author_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_tree_authors_author_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_tree_authors_author_id_seq OWNED BY public.tbl_taxa_tree_authors.author_id;


--
-- Name: tbl_taxa_tree_families_family_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_tree_families_family_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_tree_families_family_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_tree_families_family_id_seq OWNED BY public.tbl_taxa_tree_families.family_id;


--
-- Name: tbl_taxa_tree_genera_genus_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_tree_genera_genus_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_tree_genera_genus_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_tree_genera_genus_id_seq OWNED BY public.tbl_taxa_tree_genera.genus_id;


--
-- Name: tbl_taxa_tree_master_taxon_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_tree_master_taxon_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_tree_master_taxon_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_tree_master_taxon_id_seq OWNED BY public.tbl_taxa_tree_master.taxon_id;


--
-- Name: tbl_taxa_tree_orders_order_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxa_tree_orders_order_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxa_tree_orders_order_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxa_tree_orders_order_id_seq OWNED BY public.tbl_taxa_tree_orders.order_id;


--
-- Name: tbl_taxonomic_order_biblio_taxonomic_order_biblio_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxonomic_order_biblio_taxonomic_order_biblio_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxonomic_order_biblio_taxonomic_order_biblio_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxonomic_order_biblio_taxonomic_order_biblio_id_seq OWNED BY public.tbl_taxonomic_order_biblio.taxonomic_order_biblio_id;


--
-- Name: tbl_taxonomic_order_systems_taxonomic_order_system_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxonomic_order_systems_taxonomic_order_system_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxonomic_order_systems_taxonomic_order_system_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxonomic_order_systems_taxonomic_order_system_id_seq OWNED BY public.tbl_taxonomic_order_systems.taxonomic_order_system_id;


--
-- Name: tbl_taxonomic_order_taxonomic_order_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxonomic_order_taxonomic_order_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxonomic_order_taxonomic_order_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxonomic_order_taxonomic_order_id_seq OWNED BY public.tbl_taxonomic_order.taxonomic_order_id;


--
-- Name: tbl_taxonomy_notes_taxonomy_notes_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_taxonomy_notes_taxonomy_notes_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_taxonomy_notes_taxonomy_notes_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_taxonomy_notes_taxonomy_notes_id_seq OWNED BY public.tbl_taxonomy_notes.taxonomy_notes_id;


--
-- Name: tbl_temperatures_record_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_temperatures_record_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_temperatures_record_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_temperatures_record_id_seq OWNED BY public.tbl_temperatures.record_id;


--
-- Name: tbl_tephra_dates_tephra_date_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_tephra_dates_tephra_date_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_tephra_dates_tephra_date_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_tephra_dates_tephra_date_id_seq OWNED BY public.tbl_tephra_dates.tephra_date_id;


--
-- Name: tbl_tephra_refs_tephra_ref_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_tephra_refs_tephra_ref_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_tephra_refs_tephra_ref_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_tephra_refs_tephra_ref_id_seq OWNED BY public.tbl_tephra_refs.tephra_ref_id;


--
-- Name: tbl_tephras_tephra_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_tephras_tephra_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_tephras_tephra_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_tephras_tephra_id_seq OWNED BY public.tbl_tephras.tephra_id;


--
-- Name: tbl_text_biology_biology_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_text_biology_biology_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_text_biology_biology_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_text_biology_biology_id_seq OWNED BY public.tbl_text_biology.biology_id;


--
-- Name: tbl_text_distribution_distribution_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_text_distribution_distribution_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_text_distribution_distribution_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_text_distribution_distribution_id_seq OWNED BY public.tbl_text_distribution.distribution_id;


--
-- Name: tbl_text_identification_keys_key_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_text_identification_keys_key_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_text_identification_keys_key_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_text_identification_keys_key_id_seq OWNED BY public.tbl_text_identification_keys.key_id;


--
-- Name: tbl_units_unit_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_units_unit_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_units_unit_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_units_unit_id_seq OWNED BY public.tbl_units.unit_id;


--
-- Name: tbl_years_types_years_type_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.tbl_years_types_years_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: tbl_years_types_years_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tbl_years_types_years_type_id_seq OWNED BY public.tbl_years_types.years_type_id;


--
-- Name: view_typed_analysis_tables; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.view_typed_analysis_tables AS
 SELECT table_id,
    table_name,
    base_type
   FROM ( VALUES (1,'tbl_analysis_boolean_values'::text,'boolean'::text), (2,'tbl_analysis_categorical_values'::text,'category'::text), (3,'tbl_analysis_dating_ranges'::text,'dating_range'::text), (4,'tbl_analysis_identifiers'::text,'identifier'::text), (5,'tbl_analysis_integer_ranges'::text,'integer_range'::text), (6,'tbl_analysis_integer_values'::text,'integer'::text), (7,'tbl_analysis_notes'::text,'note'::text), (8,'tbl_analysis_numerical_ranges'::text,'decimal_range'::text), (9,'tbl_analysis_numerical_values'::text,'decimal'::text), (10,'tbl_analysis_taxon_counts'::text,'taxon_count'::text), (11,'tbl_analysis_value_dimensions'::text,'dimension'::text)) t(table_id, table_name, base_type);


--
-- Name: view_typed_analysis_values; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.view_typed_analysis_values AS
 SELECT 1 AS table_id,
    tbl_analysis_boolean_values.analysis_value_id
   FROM public.tbl_analysis_boolean_values
UNION
 SELECT 2 AS table_id,
    tbl_analysis_categorical_values.analysis_value_id
   FROM public.tbl_analysis_categorical_values
UNION
 SELECT 3 AS table_id,
    tbl_analysis_dating_ranges.analysis_value_id
   FROM public.tbl_analysis_dating_ranges
UNION
 SELECT 4 AS table_id,
    tbl_analysis_identifiers.analysis_value_id
   FROM public.tbl_analysis_identifiers
UNION
 SELECT 5 AS table_id,
    tbl_analysis_integer_ranges.analysis_value_id
   FROM public.tbl_analysis_integer_ranges
UNION
 SELECT 6 AS table_id,
    tbl_analysis_integer_values.analysis_value_id
   FROM public.tbl_analysis_integer_values
UNION
 SELECT 7 AS table_id,
    tbl_analysis_notes.analysis_value_id
   FROM public.tbl_analysis_notes
UNION
 SELECT 8 AS table_id,
    tbl_analysis_numerical_ranges.analysis_value_id
   FROM public.tbl_analysis_numerical_ranges
UNION
 SELECT 9 AS table_id,
    tbl_analysis_numerical_values.analysis_value_id
   FROM public.tbl_analysis_numerical_values
UNION
 SELECT 10 AS table_id,
    tbl_analysis_taxon_counts.analysis_value_id
   FROM public.tbl_analysis_taxon_counts
UNION
 SELECT 11 AS table_id,
    tbl_analysis_value_dimensions.analysis_value_id
   FROM public.tbl_analysis_value_dimensions;


--
-- Name: typed_analysis_values; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.typed_analysis_values AS
 SELECT view_typed_analysis_values.analysis_value_id,
    view_typed_analysis_tables.table_name,
    view_typed_analysis_tables.base_type
   FROM (public.view_typed_analysis_values
     JOIN public.view_typed_analysis_tables USING (table_id));


--
-- Name: view_bibliography_references; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.view_bibliography_references AS
 SELECT e.dataset_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_datasets e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.rdb_system_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_rdb_systems e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.sample_group_uuid AS uuid,
    b.biblio_uuid
   FROM ((public.tbl_sample_group_references r
     JOIN public.tbl_sample_groups e USING (sample_group_id))
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.relative_age_uuid AS uuid,
    b.biblio_uuid
   FROM ((public.tbl_relative_age_refs r
     JOIN public.tbl_relative_ages e USING (relative_age_id))
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.taxonomy_notes_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_taxonomy_notes e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.species_association_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_species_associations e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.distribution_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_text_distribution e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.tephra_uuid AS uuid,
    b.biblio_uuid
   FROM ((public.tbl_tephra_refs r
     JOIN public.tbl_tephras e USING (tephra_id))
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.ecocode_system_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_ecocode_systems e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.master_set_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_dataset_masters e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.site_other_records_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_site_other_records e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.key_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_text_identification_keys e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.geochron_uuid AS uuid,
    b.biblio_uuid
   FROM ((public.tbl_geochron_refs r
     JOIN public.tbl_geochronology e USING (geochron_id))
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.site_uuid AS uuid,
    b.biblio_uuid
   FROM ((public.tbl_site_references r
     JOIN public.tbl_sites e USING (site_id))
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.synonym_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_taxa_synonyms e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.biology_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_text_biology e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.aggregate_dataset_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_aggregate_datasets e
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.taxonomic_order_system_uuid AS uuid,
    b.biblio_uuid
   FROM ((public.tbl_taxonomic_order_biblio r
     JOIN public.tbl_taxonomic_order_systems e USING (taxonomic_order_system_id))
     JOIN public.tbl_biblio b USING (biblio_id))
UNION
 SELECT e.method_uuid AS uuid,
    b.biblio_uuid
   FROM (public.tbl_methods e
     JOIN public.tbl_biblio b USING (biblio_id));


--
-- Name: view_taxa_alphabetically; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW public.view_taxa_alphabetically AS
 SELECT o.order_id,
    o.order_name AS "order",
    f.family_id,
    f.family_name AS family,
    g.genus_id,
    g.genus_name AS genus,
    s.taxon_id,
    s.species,
    a.author_id,
    a.author_name AS author
   FROM ((((public.tbl_taxa_tree_master s
     JOIN public.tbl_taxa_tree_genera g ON ((s.genus_id = g.genus_id)))
     JOIN public.tbl_taxa_tree_families f ON ((g.family_id = f.family_id)))
     JOIN public.tbl_taxa_tree_orders o ON ((f.order_id = o.order_id)))
     LEFT JOIN public.tbl_taxa_tree_authors a ON ((s.author_id = a.author_id)))
  ORDER BY o.order_name, f.family_name, g.genus_name, s.species;


--
-- Name: tbl_abundance_elements abundance_element_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_elements ALTER COLUMN abundance_element_id SET DEFAULT nextval('public.tbl_abundance_elements_abundance_element_id_seq'::regclass);


--
-- Name: tbl_abundance_ident_levels abundance_ident_level_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_ident_levels ALTER COLUMN abundance_ident_level_id SET DEFAULT nextval('public.tbl_abundance_ident_levels_abundance_ident_level_id_seq'::regclass);


--
-- Name: tbl_abundance_modifications abundance_modification_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_modifications ALTER COLUMN abundance_modification_id SET DEFAULT nextval('public.tbl_abundance_modifications_abundance_modification_id_seq'::regclass);


--
-- Name: tbl_abundances abundance_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundances ALTER COLUMN abundance_id SET DEFAULT nextval('public.tbl_abundances_abundance_id_seq'::regclass);


--
-- Name: tbl_activity_types activity_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_activity_types ALTER COLUMN activity_type_id SET DEFAULT nextval('public.tbl_activity_types_activity_type_id_seq'::regclass);


--
-- Name: tbl_age_types age_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_age_types ALTER COLUMN age_type_id SET DEFAULT nextval('public.tbl_age_types_age_type_id_seq'::regclass);


--
-- Name: tbl_aggregate_datasets aggregate_dataset_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_datasets ALTER COLUMN aggregate_dataset_id SET DEFAULT nextval('public.tbl_aggregate_datasets_aggregate_dataset_id_seq'::regclass);


--
-- Name: tbl_aggregate_order_types aggregate_order_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_order_types ALTER COLUMN aggregate_order_type_id SET DEFAULT nextval('public.tbl_aggregate_order_types_aggregate_order_type_id_seq'::regclass);


--
-- Name: tbl_aggregate_sample_ages aggregate_sample_age_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_sample_ages ALTER COLUMN aggregate_sample_age_id SET DEFAULT nextval('public.tbl_aggregate_sample_ages_aggregate_sample_age_id_seq'::regclass);


--
-- Name: tbl_aggregate_samples aggregate_sample_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_samples ALTER COLUMN aggregate_sample_id SET DEFAULT nextval('public.tbl_aggregate_samples_aggregate_sample_id_seq'::regclass);


--
-- Name: tbl_alt_ref_types alt_ref_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_alt_ref_types ALTER COLUMN alt_ref_type_id SET DEFAULT nextval('public.tbl_alt_ref_types_alt_ref_type_id_seq'::regclass);


--
-- Name: tbl_analysis_boolean_values analysis_boolean_value_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_boolean_values ALTER COLUMN analysis_boolean_value_id SET DEFAULT nextval('public.tbl_analysis_boolean_values_analysis_boolean_value_id_seq'::regclass);


--
-- Name: tbl_analysis_categorical_values analysis_categorical_value_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_categorical_values ALTER COLUMN analysis_categorical_value_id SET DEFAULT nextval('public.tbl_analysis_categorical_valu_analysis_categorical_value_id_seq'::regclass);


--
-- Name: tbl_analysis_dating_ranges analysis_dating_range_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges ALTER COLUMN analysis_dating_range_id SET DEFAULT nextval('public.tbl_analysis_dating_ranges_analysis_dating_range_id_seq'::regclass);


--
-- Name: tbl_analysis_entities analysis_entity_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entities ALTER COLUMN analysis_entity_id SET DEFAULT nextval('public.tbl_analysis_entities_analysis_entity_id_seq'::regclass);


--
-- Name: tbl_analysis_entity_ages analysis_entity_age_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_ages ALTER COLUMN analysis_entity_age_id SET DEFAULT nextval('public.tbl_analysis_entity_ages_analysis_entity_age_id_seq'::regclass);


--
-- Name: tbl_analysis_entity_dimensions analysis_entity_dimension_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_dimensions ALTER COLUMN analysis_entity_dimension_id SET DEFAULT nextval('public.tbl_analysis_entity_dimensions_analysis_entity_dimension_id_seq'::regclass);


--
-- Name: tbl_analysis_entity_prep_methods analysis_entity_prep_method_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_prep_methods ALTER COLUMN analysis_entity_prep_method_id SET DEFAULT nextval('public.tbl_analysis_entity_prep_meth_analysis_entity_prep_method_i_seq'::regclass);


--
-- Name: tbl_analysis_identifiers analysis_identifier_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_identifiers ALTER COLUMN analysis_identifier_id SET DEFAULT nextval('public.tbl_analysis_identifiers_analysis_identifier_id_seq'::regclass);


--
-- Name: tbl_analysis_integer_ranges analysis_integer_range_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_ranges ALTER COLUMN analysis_integer_range_id SET DEFAULT nextval('public.tbl_analysis_integer_ranges_analysis_integer_range_id_seq'::regclass);


--
-- Name: tbl_analysis_integer_values analysis_integer_value_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_values ALTER COLUMN analysis_integer_value_id SET DEFAULT nextval('public.tbl_analysis_integer_values_analysis_integer_value_id_seq'::regclass);


--
-- Name: tbl_analysis_notes analysis_note_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_notes ALTER COLUMN analysis_note_id SET DEFAULT nextval('public.tbl_analysis_notes_analysis_note_id_seq'::regclass);


--
-- Name: tbl_analysis_numerical_ranges analysis_numerical_range_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_ranges ALTER COLUMN analysis_numerical_range_id SET DEFAULT nextval('public.tbl_analysis_numerical_ranges_analysis_numerical_range_id_seq'::regclass);


--
-- Name: tbl_analysis_numerical_values analysis_numerical_value_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_values ALTER COLUMN analysis_numerical_value_id SET DEFAULT nextval('public.tbl_analysis_numerical_values_analysis_numerical_value_id_seq'::regclass);


--
-- Name: tbl_analysis_taxon_counts analysis_taxon_count_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_taxon_counts ALTER COLUMN analysis_taxon_count_id SET DEFAULT nextval('public.tbl_analysis_taxon_counts_analysis_taxon_count_id_seq'::regclass);


--
-- Name: tbl_analysis_value_dimensions analysis_value_dimension_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_value_dimensions ALTER COLUMN analysis_value_dimension_id SET DEFAULT nextval('public.tbl_analysis_value_dimensions_analysis_value_dimension_id_seq'::regclass);


--
-- Name: tbl_analysis_values analysis_value_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_values ALTER COLUMN analysis_value_id SET DEFAULT nextval('public.tbl_analysis_values_analysis_value_id_seq'::regclass);


--
-- Name: tbl_biblio biblio_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_biblio ALTER COLUMN biblio_id SET DEFAULT nextval('public.tbl_biblio_biblio_id_seq'::regclass);


--
-- Name: tbl_ceramics ceramics_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics ALTER COLUMN ceramics_id SET DEFAULT nextval('public.tbl_ceramics_ceramics_id_seq'::regclass);


--
-- Name: tbl_ceramics_lookup ceramics_lookup_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics_lookup ALTER COLUMN ceramics_lookup_id SET DEFAULT nextval('public.tbl_ceramics_lookup_ceramics_lookup_id_seq'::regclass);


--
-- Name: tbl_ceramics_measurements ceramics_measurement_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics_measurements ALTER COLUMN ceramics_measurement_id SET DEFAULT nextval('public.tbl_ceramics_measurements_ceramics_measurement_id_seq'::regclass);


--
-- Name: tbl_chronologies chronology_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_chronologies ALTER COLUMN chronology_id SET DEFAULT nextval('public.tbl_chronologies_chronology_id_seq'::regclass);


--
-- Name: tbl_colours colour_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_colours ALTER COLUMN colour_id SET DEFAULT nextval('public.tbl_colours_colour_id_seq'::regclass);


--
-- Name: tbl_contact_types contact_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_contact_types ALTER COLUMN contact_type_id SET DEFAULT nextval('public.tbl_contact_types_contact_type_id_seq'::regclass);


--
-- Name: tbl_contacts contact_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_contacts ALTER COLUMN contact_id SET DEFAULT nextval('public.tbl_contacts_contact_id_seq'::regclass);


--
-- Name: tbl_coordinate_method_dimensions coordinate_method_dimension_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_coordinate_method_dimensions ALTER COLUMN coordinate_method_dimension_id SET DEFAULT nextval('public.tbl_coordinate_method_dimensi_coordinate_method_dimension_i_seq'::regclass);


--
-- Name: tbl_data_type_groups data_type_group_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_type_groups ALTER COLUMN data_type_group_id SET DEFAULT nextval('public.tbl_data_type_groups_data_type_group_id_seq'::regclass);


--
-- Name: tbl_data_types data_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_types ALTER COLUMN data_type_id SET DEFAULT nextval('public.tbl_data_types_data_type_id_seq'::regclass);


--
-- Name: tbl_dataset_contacts dataset_contact_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_contacts ALTER COLUMN dataset_contact_id SET DEFAULT nextval('public.tbl_dataset_contacts_dataset_contact_id_seq'::regclass);


--
-- Name: tbl_dataset_masters master_set_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_masters ALTER COLUMN master_set_id SET DEFAULT nextval('public.tbl_dataset_masters_master_set_id_seq'::regclass);


--
-- Name: tbl_dataset_methods dataset_method_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_methods ALTER COLUMN dataset_method_id SET DEFAULT nextval('public.tbl_dataset_methods_dataset_method_id_seq'::regclass);


--
-- Name: tbl_dataset_submission_types submission_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_submission_types ALTER COLUMN submission_type_id SET DEFAULT nextval('public.tbl_dataset_submission_types_submission_type_id_seq'::regclass);


--
-- Name: tbl_dataset_submissions dataset_submission_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_submissions ALTER COLUMN dataset_submission_id SET DEFAULT nextval('public.tbl_dataset_submissions_dataset_submission_id_seq'::regclass);


--
-- Name: tbl_datasets dataset_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets ALTER COLUMN dataset_id SET DEFAULT nextval('public.tbl_datasets_dataset_id_seq'::regclass);


--
-- Name: tbl_dating_labs dating_lab_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_labs ALTER COLUMN dating_lab_id SET DEFAULT nextval('public.tbl_dating_labs_dating_lab_id_seq'::regclass);


--
-- Name: tbl_dating_material dating_material_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_material ALTER COLUMN dating_material_id SET DEFAULT nextval('public.tbl_dating_material_dating_material_id_seq'::regclass);


--
-- Name: tbl_dating_uncertainty dating_uncertainty_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_uncertainty ALTER COLUMN dating_uncertainty_id SET DEFAULT nextval('public.tbl_dating_uncertainty_dating_uncertainty_id_seq'::regclass);


--
-- Name: tbl_dendro dendro_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro ALTER COLUMN dendro_id SET DEFAULT nextval('public.tbl_dendro_dendro_id_seq'::regclass);


--
-- Name: tbl_dendro_date_notes dendro_date_note_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_date_notes ALTER COLUMN dendro_date_note_id SET DEFAULT nextval('public.tbl_dendro_date_notes_dendro_date_note_id_seq'::regclass);


--
-- Name: tbl_dendro_dates dendro_date_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_dates ALTER COLUMN dendro_date_id SET DEFAULT nextval('public.tbl_dendro_dates_dendro_date_id_seq'::regclass);


--
-- Name: tbl_dendro_lookup dendro_lookup_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_lookup ALTER COLUMN dendro_lookup_id SET DEFAULT nextval('public.tbl_dendro_lookup_dendro_lookup_id_seq'::regclass);


--
-- Name: tbl_dimensions dimension_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dimensions ALTER COLUMN dimension_id SET DEFAULT nextval('public.tbl_dimensions_dimension_id_seq'::regclass);


--
-- Name: tbl_ecocode_definitions ecocode_definition_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_definitions ALTER COLUMN ecocode_definition_id SET DEFAULT nextval('public.tbl_ecocode_definitions_ecocode_definition_id_seq'::regclass);


--
-- Name: tbl_ecocode_groups ecocode_group_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_groups ALTER COLUMN ecocode_group_id SET DEFAULT nextval('public.tbl_ecocode_groups_ecocode_group_id_seq'::regclass);


--
-- Name: tbl_ecocode_systems ecocode_system_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_systems ALTER COLUMN ecocode_system_id SET DEFAULT nextval('public.tbl_ecocode_systems_ecocode_system_id_seq'::regclass);


--
-- Name: tbl_ecocodes ecocode_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocodes ALTER COLUMN ecocode_id SET DEFAULT nextval('public.tbl_ecocodes_ecocode_id_seq'::regclass);


--
-- Name: tbl_feature_types feature_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_feature_types ALTER COLUMN feature_type_id SET DEFAULT nextval('public.tbl_feature_types_feature_type_id_seq'::regclass);


--
-- Name: tbl_features feature_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_features ALTER COLUMN feature_id SET DEFAULT nextval('public.tbl_features_feature_id_seq'::regclass);


--
-- Name: tbl_geochron_refs geochron_ref_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochron_refs ALTER COLUMN geochron_ref_id SET DEFAULT nextval('public.tbl_geochron_refs_geochron_ref_id_seq'::regclass);


--
-- Name: tbl_geochronology geochron_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochronology ALTER COLUMN geochron_id SET DEFAULT nextval('public.tbl_geochronology_geochron_id_seq'::regclass);


--
-- Name: tbl_horizons horizon_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_horizons ALTER COLUMN horizon_id SET DEFAULT nextval('public.tbl_horizons_horizon_id_seq'::regclass);


--
-- Name: tbl_identification_levels identification_level_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_identification_levels ALTER COLUMN identification_level_id SET DEFAULT nextval('public.tbl_identification_levels_identification_level_id_seq'::regclass);


--
-- Name: tbl_image_types image_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_image_types ALTER COLUMN image_type_id SET DEFAULT nextval('public.tbl_image_types_image_type_id_seq'::regclass);


--
-- Name: tbl_imported_taxa_replacements imported_taxa_replacement_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_imported_taxa_replacements ALTER COLUMN imported_taxa_replacement_id SET DEFAULT nextval('public.tbl_imported_taxa_replacements_imported_taxa_replacement_id_seq'::regclass);


--
-- Name: tbl_isotope_measurements isotope_measurement_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_measurements ALTER COLUMN isotope_measurement_id SET DEFAULT nextval('public.tbl_isotope_measurements_isotope_measurement_id_seq'::regclass);


--
-- Name: tbl_isotope_standards isotope_standard_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_standards ALTER COLUMN isotope_standard_id SET DEFAULT nextval('public.tbl_isotope_standards_isotope_standard_id_seq'::regclass);


--
-- Name: tbl_isotope_types isotope_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_types ALTER COLUMN isotope_type_id SET DEFAULT nextval('public.tbl_isotope_types_isotope_type_id_seq'::regclass);


--
-- Name: tbl_isotopes isotope_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotopes ALTER COLUMN isotope_id SET DEFAULT nextval('public.tbl_isotopes_isotope_id_seq'::regclass);


--
-- Name: tbl_languages language_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_languages ALTER COLUMN language_id SET DEFAULT nextval('public.tbl_languages_language_id_seq'::regclass);


--
-- Name: tbl_lithology lithology_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_lithology ALTER COLUMN lithology_id SET DEFAULT nextval('public.tbl_lithology_lithology_id_seq'::regclass);


--
-- Name: tbl_location_types location_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_location_types ALTER COLUMN location_type_id SET DEFAULT nextval('public.tbl_location_types_location_type_id_seq'::regclass);


--
-- Name: tbl_locations location_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_locations ALTER COLUMN location_id SET DEFAULT nextval('public.tbl_locations_location_id_seq'::regclass);


--
-- Name: tbl_mcr_names taxon_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcr_names ALTER COLUMN taxon_id SET DEFAULT nextval('public.tbl_mcr_names_taxon_id_seq'::regclass);


--
-- Name: tbl_mcr_summary_data mcr_summary_data_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcr_summary_data ALTER COLUMN mcr_summary_data_id SET DEFAULT nextval('public.tbl_mcr_summary_data_mcr_summary_data_id_seq'::regclass);


--
-- Name: tbl_mcrdata_birmbeetledat mcrdata_birmbeetledat_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcrdata_birmbeetledat ALTER COLUMN mcrdata_birmbeetledat_id SET DEFAULT nextval('public.tbl_mcrdata_birmbeetledat_mcrdata_birmbeetledat_id_seq'::regclass);


--
-- Name: tbl_measured_value_dimensions measured_value_dimension_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_measured_value_dimensions ALTER COLUMN measured_value_dimension_id SET DEFAULT nextval('public.tbl_measured_value_dimensions_measured_value_dimension_id_seq'::regclass);


--
-- Name: tbl_measured_values measured_value_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_measured_values ALTER COLUMN measured_value_id SET DEFAULT nextval('public.tbl_measured_values_measured_value_id_seq'::regclass);


--
-- Name: tbl_method_groups method_group_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_method_groups ALTER COLUMN method_group_id SET DEFAULT nextval('public.tbl_method_groups_method_group_id_seq'::regclass);


--
-- Name: tbl_methods method_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods ALTER COLUMN method_id SET DEFAULT nextval('public.tbl_methods_method_id_seq'::regclass);


--
-- Name: tbl_modification_types modification_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_modification_types ALTER COLUMN modification_type_id SET DEFAULT nextval('public.tbl_modification_types_modification_type_id_seq'::regclass);


--
-- Name: tbl_physical_sample_features physical_sample_feature_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_sample_features ALTER COLUMN physical_sample_feature_id SET DEFAULT nextval('public.tbl_physical_sample_features_physical_sample_feature_id_seq'::regclass);


--
-- Name: tbl_physical_samples physical_sample_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_samples ALTER COLUMN physical_sample_id SET DEFAULT nextval('public.tbl_physical_samples_physical_sample_id_seq'::regclass);


--
-- Name: tbl_project_stages project_stage_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_stages ALTER COLUMN project_stage_id SET DEFAULT nextval('public.tbl_project_stages_project_stage_id_seq'::regclass);


--
-- Name: tbl_project_types project_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_types ALTER COLUMN project_type_id SET DEFAULT nextval('public.tbl_project_types_project_type_id_seq'::regclass);


--
-- Name: tbl_projects project_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_projects ALTER COLUMN project_id SET DEFAULT nextval('public.tbl_projects_project_id_seq'::regclass);


--
-- Name: tbl_rdb rdb_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb ALTER COLUMN rdb_id SET DEFAULT nextval('public.tbl_rdb_rdb_id_seq'::regclass);


--
-- Name: tbl_rdb_codes rdb_code_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_codes ALTER COLUMN rdb_code_id SET DEFAULT nextval('public.tbl_rdb_codes_rdb_code_id_seq'::regclass);


--
-- Name: tbl_rdb_systems rdb_system_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_systems ALTER COLUMN rdb_system_id SET DEFAULT nextval('public.tbl_rdb_systems_rdb_system_id_seq'::regclass);


--
-- Name: tbl_record_types record_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_record_types ALTER COLUMN record_type_id SET DEFAULT nextval('public.tbl_record_types_record_type_id_seq'::regclass);


--
-- Name: tbl_relative_age_refs relative_age_ref_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_refs ALTER COLUMN relative_age_ref_id SET DEFAULT nextval('public.tbl_relative_age_refs_relative_age_ref_id_seq'::regclass);


--
-- Name: tbl_relative_age_types relative_age_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_types ALTER COLUMN relative_age_type_id SET DEFAULT nextval('public.tbl_relative_age_types_relative_age_type_id_seq'::regclass);


--
-- Name: tbl_relative_ages relative_age_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_ages ALTER COLUMN relative_age_id SET DEFAULT nextval('public.tbl_relative_ages_relative_age_id_seq'::regclass);


--
-- Name: tbl_relative_dates relative_date_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_dates ALTER COLUMN relative_date_id SET DEFAULT nextval('public.tbl_relative_dates_relative_date_id_seq'::regclass);


--
-- Name: tbl_sample_alt_refs sample_alt_ref_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_alt_refs ALTER COLUMN sample_alt_ref_id SET DEFAULT nextval('public.tbl_sample_alt_refs_sample_alt_ref_id_seq'::regclass);


--
-- Name: tbl_sample_colours sample_colour_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_colours ALTER COLUMN sample_colour_id SET DEFAULT nextval('public.tbl_sample_colours_sample_colour_id_seq'::regclass);


--
-- Name: tbl_sample_coordinates sample_coordinate_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_coordinates ALTER COLUMN sample_coordinate_id SET DEFAULT nextval('public.tbl_sample_coordinates_sample_coordinate_id_seq'::regclass);


--
-- Name: tbl_sample_description_sample_group_contexts sample_description_sample_group_context_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_sample_group_contexts ALTER COLUMN sample_description_sample_group_context_id SET DEFAULT nextval('public.tbl_sample_description_sample_sample_description_sample_gro_seq'::regclass);


--
-- Name: tbl_sample_description_types sample_description_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_types ALTER COLUMN sample_description_type_id SET DEFAULT nextval('public.tbl_sample_description_types_sample_description_type_id_seq'::regclass);


--
-- Name: tbl_sample_descriptions sample_description_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_descriptions ALTER COLUMN sample_description_id SET DEFAULT nextval('public.tbl_sample_descriptions_sample_description_id_seq'::regclass);


--
-- Name: tbl_sample_dimensions sample_dimension_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_dimensions ALTER COLUMN sample_dimension_id SET DEFAULT nextval('public.tbl_sample_dimensions_sample_dimension_id_seq'::regclass);


--
-- Name: tbl_sample_group_coordinates sample_group_position_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_coordinates ALTER COLUMN sample_group_position_id SET DEFAULT nextval('public.tbl_sample_group_coordinates_sample_group_position_id_seq'::regclass);


--
-- Name: tbl_sample_group_description_type_sampling_contexts sample_group_description_type_sampling_context_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_type_sampling_contexts ALTER COLUMN sample_group_description_type_sampling_context_id SET DEFAULT nextval('public.tbl_sample_group_description__sample_group_description_type_seq'::regclass);


--
-- Name: tbl_sample_group_description_types sample_group_description_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_types ALTER COLUMN sample_group_description_type_id SET DEFAULT nextval('public.tbl_sample_group_description__sample_group_description_typ_seq1'::regclass);


--
-- Name: tbl_sample_group_descriptions sample_group_description_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_descriptions ALTER COLUMN sample_group_description_id SET DEFAULT nextval('public.tbl_sample_group_descriptions_sample_group_description_id_seq'::regclass);


--
-- Name: tbl_sample_group_dimensions sample_group_dimension_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_dimensions ALTER COLUMN sample_group_dimension_id SET DEFAULT nextval('public.tbl_sample_group_dimensions_sample_group_dimension_id_seq'::regclass);


--
-- Name: tbl_sample_group_images sample_group_image_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_images ALTER COLUMN sample_group_image_id SET DEFAULT nextval('public.tbl_sample_group_images_sample_group_image_id_seq'::regclass);


--
-- Name: tbl_sample_group_notes sample_group_note_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_notes ALTER COLUMN sample_group_note_id SET DEFAULT nextval('public.tbl_sample_group_notes_sample_group_note_id_seq'::regclass);


--
-- Name: tbl_sample_group_references sample_group_reference_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_references ALTER COLUMN sample_group_reference_id SET DEFAULT nextval('public.tbl_sample_group_references_sample_group_reference_id_seq'::regclass);


--
-- Name: tbl_sample_group_sampling_contexts sampling_context_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_sampling_contexts ALTER COLUMN sampling_context_id SET DEFAULT nextval('public.tbl_sample_group_sampling_contexts_sampling_context_id_seq'::regclass);


--
-- Name: tbl_sample_groups sample_group_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_groups ALTER COLUMN sample_group_id SET DEFAULT nextval('public.tbl_sample_groups_sample_group_id_seq'::regclass);


--
-- Name: tbl_sample_horizons sample_horizon_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_horizons ALTER COLUMN sample_horizon_id SET DEFAULT nextval('public.tbl_sample_horizons_sample_horizon_id_seq'::regclass);


--
-- Name: tbl_sample_images sample_image_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_images ALTER COLUMN sample_image_id SET DEFAULT nextval('public.tbl_sample_images_sample_image_id_seq'::regclass);


--
-- Name: tbl_sample_location_type_sampling_contexts sample_location_type_sampling_context_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_type_sampling_contexts ALTER COLUMN sample_location_type_sampling_context_id SET DEFAULT nextval('public.tbl_sample_location_type_samp_sample_location_type_sampling_seq'::regclass);


--
-- Name: tbl_sample_location_types sample_location_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_types ALTER COLUMN sample_location_type_id SET DEFAULT nextval('public.tbl_sample_location_types_sample_location_type_id_seq'::regclass);


--
-- Name: tbl_sample_locations sample_location_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_locations ALTER COLUMN sample_location_id SET DEFAULT nextval('public.tbl_sample_locations_sample_location_id_seq'::regclass);


--
-- Name: tbl_sample_notes sample_note_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_notes ALTER COLUMN sample_note_id SET DEFAULT nextval('public.tbl_sample_notes_sample_note_id_seq'::regclass);


--
-- Name: tbl_sample_types sample_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_types ALTER COLUMN sample_type_id SET DEFAULT nextval('public.tbl_sample_types_sample_type_id_seq'::regclass);


--
-- Name: tbl_season_types season_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_season_types ALTER COLUMN season_type_id SET DEFAULT nextval('public.tbl_season_types_season_type_id_seq'::regclass);


--
-- Name: tbl_seasons season_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_seasons ALTER COLUMN season_id SET DEFAULT nextval('public.tbl_seasons_season_id_seq'::regclass);


--
-- Name: tbl_site_images site_image_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_images ALTER COLUMN site_image_id SET DEFAULT nextval('public.tbl_site_images_site_image_id_seq'::regclass);


--
-- Name: tbl_site_locations site_location_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_locations ALTER COLUMN site_location_id SET DEFAULT nextval('public.tbl_site_locations_site_location_id_seq'::regclass);


--
-- Name: tbl_site_natgridrefs site_natgridref_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_natgridrefs ALTER COLUMN site_natgridref_id SET DEFAULT nextval('public.tbl_site_natgridrefs_site_natgridref_id_seq'::regclass);


--
-- Name: tbl_site_other_records site_other_records_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_other_records ALTER COLUMN site_other_records_id SET DEFAULT nextval('public.tbl_site_other_records_site_other_records_id_seq'::regclass);


--
-- Name: tbl_site_preservation_status site_preservation_status_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_preservation_status ALTER COLUMN site_preservation_status_id SET DEFAULT nextval('public.tbl_site_preservation_status_site_preservation_status_id_seq'::regclass);


--
-- Name: tbl_site_references site_reference_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_references ALTER COLUMN site_reference_id SET DEFAULT nextval('public.tbl_site_references_site_reference_id_seq'::regclass);


--
-- Name: tbl_sites site_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sites ALTER COLUMN site_id SET DEFAULT nextval('public.tbl_sites_site_id_seq'::regclass);


--
-- Name: tbl_species_association_types association_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_association_types ALTER COLUMN association_type_id SET DEFAULT nextval('public.tbl_species_association_types_association_type_id_seq'::regclass);


--
-- Name: tbl_species_associations species_association_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_associations ALTER COLUMN species_association_id SET DEFAULT nextval('public.tbl_species_associations_species_association_id_seq'::regclass);


--
-- Name: tbl_taxa_common_names taxon_common_name_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_common_names ALTER COLUMN taxon_common_name_id SET DEFAULT nextval('public.tbl_taxa_common_names_taxon_common_name_id_seq'::regclass);


--
-- Name: tbl_taxa_images taxa_images_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_images ALTER COLUMN taxa_images_id SET DEFAULT nextval('public.tbl_taxa_images_taxa_images_id_seq'::regclass);


--
-- Name: tbl_taxa_measured_attributes measured_attribute_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_measured_attributes ALTER COLUMN measured_attribute_id SET DEFAULT nextval('public.tbl_taxa_measured_attributes_measured_attribute_id_seq'::regclass);


--
-- Name: tbl_taxa_reference_specimens taxa_reference_specimen_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_reference_specimens ALTER COLUMN taxa_reference_specimen_id SET DEFAULT nextval('public.tbl_taxa_reference_specimens_taxa_reference_specimen_id_seq'::regclass);


--
-- Name: tbl_taxa_seasonality seasonality_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_seasonality ALTER COLUMN seasonality_id SET DEFAULT nextval('public.tbl_taxa_seasonality_seasonality_id_seq'::regclass);


--
-- Name: tbl_taxa_synonyms synonym_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms ALTER COLUMN synonym_id SET DEFAULT nextval('public.tbl_taxa_synonyms_synonym_id_seq'::regclass);


--
-- Name: tbl_taxa_tree_authors author_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_authors ALTER COLUMN author_id SET DEFAULT nextval('public.tbl_taxa_tree_authors_author_id_seq'::regclass);


--
-- Name: tbl_taxa_tree_families family_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_families ALTER COLUMN family_id SET DEFAULT nextval('public.tbl_taxa_tree_families_family_id_seq'::regclass);


--
-- Name: tbl_taxa_tree_genera genus_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_genera ALTER COLUMN genus_id SET DEFAULT nextval('public.tbl_taxa_tree_genera_genus_id_seq'::regclass);


--
-- Name: tbl_taxa_tree_master taxon_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_master ALTER COLUMN taxon_id SET DEFAULT nextval('public.tbl_taxa_tree_master_taxon_id_seq'::regclass);


--
-- Name: tbl_taxa_tree_orders order_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_orders ALTER COLUMN order_id SET DEFAULT nextval('public.tbl_taxa_tree_orders_order_id_seq'::regclass);


--
-- Name: tbl_taxonomic_order taxonomic_order_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order ALTER COLUMN taxonomic_order_id SET DEFAULT nextval('public.tbl_taxonomic_order_taxonomic_order_id_seq'::regclass);


--
-- Name: tbl_taxonomic_order_biblio taxonomic_order_biblio_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order_biblio ALTER COLUMN taxonomic_order_biblio_id SET DEFAULT nextval('public.tbl_taxonomic_order_biblio_taxonomic_order_biblio_id_seq'::regclass);


--
-- Name: tbl_taxonomic_order_systems taxonomic_order_system_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order_systems ALTER COLUMN taxonomic_order_system_id SET DEFAULT nextval('public.tbl_taxonomic_order_systems_taxonomic_order_system_id_seq'::regclass);


--
-- Name: tbl_taxonomy_notes taxonomy_notes_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomy_notes ALTER COLUMN taxonomy_notes_id SET DEFAULT nextval('public.tbl_taxonomy_notes_taxonomy_notes_id_seq'::regclass);


--
-- Name: tbl_temperatures record_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_temperatures ALTER COLUMN record_id SET DEFAULT nextval('public.tbl_temperatures_record_id_seq'::regclass);


--
-- Name: tbl_tephra_dates tephra_date_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_dates ALTER COLUMN tephra_date_id SET DEFAULT nextval('public.tbl_tephra_dates_tephra_date_id_seq'::regclass);


--
-- Name: tbl_tephra_refs tephra_ref_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_refs ALTER COLUMN tephra_ref_id SET DEFAULT nextval('public.tbl_tephra_refs_tephra_ref_id_seq'::regclass);


--
-- Name: tbl_tephras tephra_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephras ALTER COLUMN tephra_id SET DEFAULT nextval('public.tbl_tephras_tephra_id_seq'::regclass);


--
-- Name: tbl_text_biology biology_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_biology ALTER COLUMN biology_id SET DEFAULT nextval('public.tbl_text_biology_biology_id_seq'::regclass);


--
-- Name: tbl_text_distribution distribution_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_distribution ALTER COLUMN distribution_id SET DEFAULT nextval('public.tbl_text_distribution_distribution_id_seq'::regclass);


--
-- Name: tbl_text_identification_keys key_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_identification_keys ALTER COLUMN key_id SET DEFAULT nextval('public.tbl_text_identification_keys_key_id_seq'::regclass);


--
-- Name: tbl_units unit_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_units ALTER COLUMN unit_id SET DEFAULT nextval('public.tbl_units_unit_id_seq'::regclass);


--
-- Name: tbl_years_types years_type_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_years_types ALTER COLUMN years_type_id SET DEFAULT nextval('public.tbl_years_types_years_type_id_seq'::regclass);


--
-- Name: tbl_abundance_elements tbl_abundance_elements_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_elements
    ADD CONSTRAINT tbl_abundance_elements_pkey PRIMARY KEY (abundance_element_id);


--
-- Name: tbl_abundance_ident_levels tbl_abundance_ident_levels_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_ident_levels
    ADD CONSTRAINT tbl_abundance_ident_levels_pkey PRIMARY KEY (abundance_ident_level_id);


--
-- Name: tbl_abundance_modifications tbl_abundance_modifications_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_modifications
    ADD CONSTRAINT tbl_abundance_modifications_pkey PRIMARY KEY (abundance_modification_id);


--
-- Name: tbl_abundances tbl_abundances_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundances
    ADD CONSTRAINT tbl_abundances_pkey PRIMARY KEY (abundance_id);


--
-- Name: tbl_activity_types tbl_activity_types_activity_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_activity_types
    ADD CONSTRAINT tbl_activity_types_activity_type_key UNIQUE (activity_type);


--
-- Name: tbl_activity_types tbl_activity_types_activity_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_activity_types
    ADD CONSTRAINT tbl_activity_types_activity_type_unique UNIQUE (activity_type);


--
-- Name: tbl_activity_types tbl_activity_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_activity_types
    ADD CONSTRAINT tbl_activity_types_pkey PRIMARY KEY (activity_type_id);


--
-- Name: tbl_age_types tbl_age_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_age_types
    ADD CONSTRAINT tbl_age_types_pkey PRIMARY KEY (age_type_id);


--
-- Name: tbl_aggregate_datasets tbl_aggregate_datasets_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_datasets
    ADD CONSTRAINT tbl_aggregate_datasets_pkey PRIMARY KEY (aggregate_dataset_id);


--
-- Name: tbl_aggregate_order_types tbl_aggregate_order_types_aggregate_order_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_order_types
    ADD CONSTRAINT tbl_aggregate_order_types_aggregate_order_type_key UNIQUE (aggregate_order_type);


--
-- Name: tbl_aggregate_order_types tbl_aggregate_order_types_aggregate_order_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_order_types
    ADD CONSTRAINT tbl_aggregate_order_types_aggregate_order_type_unique UNIQUE (aggregate_order_type);


--
-- Name: tbl_aggregate_order_types tbl_aggregate_order_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_order_types
    ADD CONSTRAINT tbl_aggregate_order_types_pkey PRIMARY KEY (aggregate_order_type_id);


--
-- Name: tbl_aggregate_sample_ages tbl_aggregate_sample_ages_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_sample_ages
    ADD CONSTRAINT tbl_aggregate_sample_ages_pkey PRIMARY KEY (aggregate_sample_age_id);


--
-- Name: tbl_aggregate_samples tbl_aggregate_samples_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_samples
    ADD CONSTRAINT tbl_aggregate_samples_pkey PRIMARY KEY (aggregate_sample_id);


--
-- Name: tbl_alt_ref_types tbl_alt_ref_types_alt_ref_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_alt_ref_types
    ADD CONSTRAINT tbl_alt_ref_types_alt_ref_type_key UNIQUE (alt_ref_type);


--
-- Name: tbl_alt_ref_types tbl_alt_ref_types_alt_ref_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_alt_ref_types
    ADD CONSTRAINT tbl_alt_ref_types_alt_ref_type_unique UNIQUE (alt_ref_type);


--
-- Name: tbl_alt_ref_types tbl_alt_ref_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_alt_ref_types
    ADD CONSTRAINT tbl_alt_ref_types_pkey PRIMARY KEY (alt_ref_type_id);


--
-- Name: tbl_analysis_boolean_values tbl_analysis_boolean_values_analysis_value_id_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_boolean_values
    ADD CONSTRAINT tbl_analysis_boolean_values_analysis_value_id_key UNIQUE (analysis_value_id);


--
-- Name: tbl_analysis_boolean_values tbl_analysis_boolean_values_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_boolean_values
    ADD CONSTRAINT tbl_analysis_boolean_values_pkey PRIMARY KEY (analysis_boolean_value_id);


--
-- Name: tbl_analysis_categorical_values tbl_analysis_categorical_values_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_categorical_values
    ADD CONSTRAINT tbl_analysis_categorical_values_pkey PRIMARY KEY (analysis_categorical_value_id);


--
-- Name: tbl_analysis_dating_ranges tbl_analysis_dating_ranges_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges
    ADD CONSTRAINT tbl_analysis_dating_ranges_pkey PRIMARY KEY (analysis_dating_range_id);


--
-- Name: tbl_analysis_entities tbl_analysis_entities_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entities
    ADD CONSTRAINT tbl_analysis_entities_pkey PRIMARY KEY (analysis_entity_id);


--
-- Name: tbl_analysis_entity_ages tbl_analysis_entity_ages_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_ages
    ADD CONSTRAINT tbl_analysis_entity_ages_pkey PRIMARY KEY (analysis_entity_age_id);


--
-- Name: tbl_analysis_entity_dimensions tbl_analysis_entity_dimensions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_dimensions
    ADD CONSTRAINT tbl_analysis_entity_dimensions_pkey PRIMARY KEY (analysis_entity_dimension_id);


--
-- Name: tbl_analysis_entity_prep_methods tbl_analysis_entity_prep_methods_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_prep_methods
    ADD CONSTRAINT tbl_analysis_entity_prep_methods_pkey PRIMARY KEY (analysis_entity_prep_method_id);


--
-- Name: tbl_analysis_identifiers tbl_analysis_identifiers_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_identifiers
    ADD CONSTRAINT tbl_analysis_identifiers_pkey PRIMARY KEY (analysis_identifier_id);


--
-- Name: tbl_analysis_integer_ranges tbl_analysis_integer_ranges_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_ranges
    ADD CONSTRAINT tbl_analysis_integer_ranges_pkey PRIMARY KEY (analysis_integer_range_id);


--
-- Name: tbl_analysis_integer_values tbl_analysis_integer_values_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_values
    ADD CONSTRAINT tbl_analysis_integer_values_pkey PRIMARY KEY (analysis_integer_value_id);


--
-- Name: tbl_analysis_notes tbl_analysis_notes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_notes
    ADD CONSTRAINT tbl_analysis_notes_pkey PRIMARY KEY (analysis_note_id);


--
-- Name: tbl_analysis_numerical_ranges tbl_analysis_numerical_ranges_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_ranges
    ADD CONSTRAINT tbl_analysis_numerical_ranges_pkey PRIMARY KEY (analysis_numerical_range_id);


--
-- Name: tbl_analysis_numerical_values tbl_analysis_numerical_values_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_values
    ADD CONSTRAINT tbl_analysis_numerical_values_pkey PRIMARY KEY (analysis_numerical_value_id);


--
-- Name: tbl_analysis_taxon_counts tbl_analysis_taxon_counts_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_taxon_counts
    ADD CONSTRAINT tbl_analysis_taxon_counts_pkey PRIMARY KEY (analysis_taxon_count_id);


--
-- Name: tbl_analysis_value_dimensions tbl_analysis_value_dimensions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_value_dimensions
    ADD CONSTRAINT tbl_analysis_value_dimensions_pkey PRIMARY KEY (analysis_value_dimension_id);


--
-- Name: tbl_analysis_values tbl_analysis_values_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_values
    ADD CONSTRAINT tbl_analysis_values_pkey PRIMARY KEY (analysis_value_id);


--
-- Name: tbl_biblio tbl_biblio_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_biblio
    ADD CONSTRAINT tbl_biblio_pkey PRIMARY KEY (biblio_id);


--
-- Name: tbl_ceramics_lookup tbl_ceramics_lookup_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics_lookup
    ADD CONSTRAINT tbl_ceramics_lookup_pkey PRIMARY KEY (ceramics_lookup_id);


--
-- Name: tbl_ceramics_measurements tbl_ceramics_measurements_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics_measurements
    ADD CONSTRAINT tbl_ceramics_measurements_pkey PRIMARY KEY (ceramics_measurement_id);


--
-- Name: tbl_ceramics tbl_ceramics_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics
    ADD CONSTRAINT tbl_ceramics_pkey PRIMARY KEY (ceramics_id);


--
-- Name: tbl_chronologies tbl_chronologies_chronology_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_chronologies
    ADD CONSTRAINT tbl_chronologies_chronology_name_key UNIQUE (chronology_name);


--
-- Name: tbl_chronologies tbl_chronologies_chronology_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_chronologies
    ADD CONSTRAINT tbl_chronologies_chronology_name_unique UNIQUE (chronology_name);


--
-- Name: tbl_chronologies tbl_chronologies_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_chronologies
    ADD CONSTRAINT tbl_chronologies_pkey PRIMARY KEY (chronology_id);


--
-- Name: tbl_colours tbl_colours_colour_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_colours
    ADD CONSTRAINT tbl_colours_colour_name_key UNIQUE (colour_name);


--
-- Name: tbl_colours tbl_colours_colour_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_colours
    ADD CONSTRAINT tbl_colours_colour_name_unique UNIQUE (colour_name);


--
-- Name: tbl_colours tbl_colours_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_colours
    ADD CONSTRAINT tbl_colours_pkey PRIMARY KEY (colour_id);


--
-- Name: tbl_contact_types tbl_contact_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_contact_types
    ADD CONSTRAINT tbl_contact_types_pkey PRIMARY KEY (contact_type_id);


--
-- Name: tbl_contacts tbl_contacts_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_contacts
    ADD CONSTRAINT tbl_contacts_pkey PRIMARY KEY (contact_id);


--
-- Name: tbl_coordinate_method_dimensions tbl_coordinate_method_dimensions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_coordinate_method_dimensions
    ADD CONSTRAINT tbl_coordinate_method_dimensions_pkey PRIMARY KEY (coordinate_method_dimension_id);


--
-- Name: tbl_data_type_groups tbl_data_type_groups_data_type_group_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_type_groups
    ADD CONSTRAINT tbl_data_type_groups_data_type_group_name_key UNIQUE (data_type_group_name);


--
-- Name: tbl_data_type_groups tbl_data_type_groups_data_type_group_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_type_groups
    ADD CONSTRAINT tbl_data_type_groups_data_type_group_name_unique UNIQUE (data_type_group_name);


--
-- Name: tbl_data_type_groups tbl_data_type_groups_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_type_groups
    ADD CONSTRAINT tbl_data_type_groups_pkey PRIMARY KEY (data_type_group_id);


--
-- Name: tbl_data_types tbl_data_types_data_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_types
    ADD CONSTRAINT tbl_data_types_data_type_name_key UNIQUE (data_type_name);


--
-- Name: tbl_data_types tbl_data_types_data_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_types
    ADD CONSTRAINT tbl_data_types_data_type_name_unique UNIQUE (data_type_name);


--
-- Name: tbl_data_types tbl_data_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_types
    ADD CONSTRAINT tbl_data_types_pkey PRIMARY KEY (data_type_id);


--
-- Name: tbl_dataset_contacts tbl_dataset_contacts_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_contacts
    ADD CONSTRAINT tbl_dataset_contacts_pkey PRIMARY KEY (dataset_contact_id);


--
-- Name: tbl_dataset_masters tbl_dataset_masters_master_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_masters
    ADD CONSTRAINT tbl_dataset_masters_master_name_key UNIQUE (master_name);


--
-- Name: tbl_dataset_masters tbl_dataset_masters_master_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_masters
    ADD CONSTRAINT tbl_dataset_masters_master_name_unique UNIQUE (master_name);


--
-- Name: tbl_dataset_masters tbl_dataset_masters_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_masters
    ADD CONSTRAINT tbl_dataset_masters_pkey PRIMARY KEY (master_set_id);


--
-- Name: tbl_dataset_methods tbl_dataset_methods_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_methods
    ADD CONSTRAINT tbl_dataset_methods_pkey PRIMARY KEY (dataset_method_id);


--
-- Name: tbl_dataset_submission_types tbl_dataset_submission_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_submission_types
    ADD CONSTRAINT tbl_dataset_submission_types_pkey PRIMARY KEY (submission_type_id);


--
-- Name: tbl_dataset_submissions tbl_dataset_submissions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_submissions
    ADD CONSTRAINT tbl_dataset_submissions_pkey PRIMARY KEY (dataset_submission_id);


--
-- Name: tbl_datasets tbl_datasets_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT tbl_datasets_pkey PRIMARY KEY (dataset_id);


--
-- Name: tbl_dating_labs tbl_dating_labs_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_labs
    ADD CONSTRAINT tbl_dating_labs_pkey PRIMARY KEY (dating_lab_id);


--
-- Name: tbl_dating_material tbl_dating_material_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_material
    ADD CONSTRAINT tbl_dating_material_pkey PRIMARY KEY (dating_material_id);


--
-- Name: tbl_dating_uncertainty tbl_dating_uncertainty_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_uncertainty
    ADD CONSTRAINT tbl_dating_uncertainty_pkey PRIMARY KEY (dating_uncertainty_id);


--
-- Name: tbl_dating_uncertainty tbl_dating_uncertainty_uncertainty_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_uncertainty
    ADD CONSTRAINT tbl_dating_uncertainty_uncertainty_key UNIQUE (uncertainty);


--
-- Name: tbl_dating_uncertainty tbl_dating_uncertainty_uncertainty_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_uncertainty
    ADD CONSTRAINT tbl_dating_uncertainty_uncertainty_unique UNIQUE (uncertainty);


--
-- Name: tbl_dendro_date_notes tbl_dendro_date_notes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_date_notes
    ADD CONSTRAINT tbl_dendro_date_notes_pkey PRIMARY KEY (dendro_date_note_id);


--
-- Name: tbl_dendro_dates tbl_dendro_dates_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_dates
    ADD CONSTRAINT tbl_dendro_dates_pkey PRIMARY KEY (dendro_date_id);


--
-- Name: tbl_dendro_lookup tbl_dendro_lookup_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_lookup
    ADD CONSTRAINT tbl_dendro_lookup_pkey PRIMARY KEY (dendro_lookup_id);


--
-- Name: tbl_dendro tbl_dendro_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro
    ADD CONSTRAINT tbl_dendro_pkey PRIMARY KEY (dendro_id);


--
-- Name: tbl_dimensions tbl_dimensions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dimensions
    ADD CONSTRAINT tbl_dimensions_pkey PRIMARY KEY (dimension_id);


--
-- Name: tbl_ecocode_definitions tbl_ecocode_definitions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_definitions
    ADD CONSTRAINT tbl_ecocode_definitions_pkey PRIMARY KEY (ecocode_definition_id);


--
-- Name: tbl_ecocode_groups tbl_ecocode_groups_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_groups
    ADD CONSTRAINT tbl_ecocode_groups_name_key UNIQUE (name);


--
-- Name: tbl_ecocode_groups tbl_ecocode_groups_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_groups
    ADD CONSTRAINT tbl_ecocode_groups_name_unique UNIQUE (name);


--
-- Name: tbl_ecocode_groups tbl_ecocode_groups_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_groups
    ADD CONSTRAINT tbl_ecocode_groups_pkey PRIMARY KEY (ecocode_group_id);


--
-- Name: tbl_ecocode_systems tbl_ecocode_systems_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_systems
    ADD CONSTRAINT tbl_ecocode_systems_name_key UNIQUE (name);


--
-- Name: tbl_ecocode_systems tbl_ecocode_systems_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_systems
    ADD CONSTRAINT tbl_ecocode_systems_name_unique UNIQUE (name);


--
-- Name: tbl_ecocode_systems tbl_ecocode_systems_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_systems
    ADD CONSTRAINT tbl_ecocode_systems_pkey PRIMARY KEY (ecocode_system_id);


--
-- Name: tbl_ecocodes tbl_ecocodes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocodes
    ADD CONSTRAINT tbl_ecocodes_pkey PRIMARY KEY (ecocode_id);


--
-- Name: tbl_feature_types tbl_feature_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_feature_types
    ADD CONSTRAINT tbl_feature_types_pkey PRIMARY KEY (feature_type_id);


--
-- Name: tbl_features tbl_features_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_features
    ADD CONSTRAINT tbl_features_pkey PRIMARY KEY (feature_id);


--
-- Name: tbl_geochron_refs tbl_geochron_refs_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochron_refs
    ADD CONSTRAINT tbl_geochron_refs_pkey PRIMARY KEY (geochron_ref_id);


--
-- Name: tbl_geochronology tbl_geochronology_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochronology
    ADD CONSTRAINT tbl_geochronology_pkey PRIMARY KEY (geochron_id);


--
-- Name: tbl_horizons tbl_horizons_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_horizons
    ADD CONSTRAINT tbl_horizons_pkey PRIMARY KEY (horizon_id);


--
-- Name: tbl_identification_levels tbl_identification_levels_identification_level_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_identification_levels
    ADD CONSTRAINT tbl_identification_levels_identification_level_name_key UNIQUE (identification_level_name);


--
-- Name: tbl_identification_levels tbl_identification_levels_identification_level_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_identification_levels
    ADD CONSTRAINT tbl_identification_levels_identification_level_name_unique UNIQUE (identification_level_name);


--
-- Name: tbl_identification_levels tbl_identification_levels_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_identification_levels
    ADD CONSTRAINT tbl_identification_levels_pkey PRIMARY KEY (identification_level_id);


--
-- Name: tbl_image_types tbl_image_types_image_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_image_types
    ADD CONSTRAINT tbl_image_types_image_type_key UNIQUE (image_type);


--
-- Name: tbl_image_types tbl_image_types_image_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_image_types
    ADD CONSTRAINT tbl_image_types_image_type_unique UNIQUE (image_type);


--
-- Name: tbl_image_types tbl_image_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_image_types
    ADD CONSTRAINT tbl_image_types_pkey PRIMARY KEY (image_type_id);


--
-- Name: tbl_imported_taxa_replacements tbl_imported_taxa_replacements_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_imported_taxa_replacements
    ADD CONSTRAINT tbl_imported_taxa_replacements_pkey PRIMARY KEY (imported_taxa_replacement_id);


--
-- Name: tbl_isotope_measurements tbl_isotope_measurements_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_measurements
    ADD CONSTRAINT tbl_isotope_measurements_pkey PRIMARY KEY (isotope_measurement_id);


--
-- Name: tbl_isotope_standards tbl_isotope_standards_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_standards
    ADD CONSTRAINT tbl_isotope_standards_pkey PRIMARY KEY (isotope_standard_id);


--
-- Name: tbl_isotope_types tbl_isotope_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_types
    ADD CONSTRAINT tbl_isotope_types_pkey PRIMARY KEY (isotope_type_id);


--
-- Name: tbl_isotope_value_specifiers tbl_isotope_value_specifiers_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_value_specifiers
    ADD CONSTRAINT tbl_isotope_value_specifiers_pkey PRIMARY KEY (isotope_value_specifier_id);


--
-- Name: tbl_isotopes tbl_isotopes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotopes
    ADD CONSTRAINT tbl_isotopes_pkey PRIMARY KEY (isotope_id);


--
-- Name: tbl_languages tbl_languages_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_languages
    ADD CONSTRAINT tbl_languages_pkey PRIMARY KEY (language_id);


--
-- Name: tbl_lithology tbl_lithology_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_lithology
    ADD CONSTRAINT tbl_lithology_pkey PRIMARY KEY (lithology_id);


--
-- Name: tbl_location_types tbl_location_types_location_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_location_types
    ADD CONSTRAINT tbl_location_types_location_type_key UNIQUE (location_type);


--
-- Name: tbl_location_types tbl_location_types_location_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_location_types
    ADD CONSTRAINT tbl_location_types_location_type_unique UNIQUE (location_type);


--
-- Name: tbl_location_types tbl_location_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_location_types
    ADD CONSTRAINT tbl_location_types_pkey PRIMARY KEY (location_type_id);


--
-- Name: tbl_locations tbl_locations_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_locations
    ADD CONSTRAINT tbl_locations_pkey PRIMARY KEY (location_id);


--
-- Name: tbl_mcr_names tbl_mcr_names_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcr_names
    ADD CONSTRAINT tbl_mcr_names_pkey PRIMARY KEY (taxon_id);


--
-- Name: tbl_mcr_summary_data tbl_mcr_summary_data_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcr_summary_data
    ADD CONSTRAINT tbl_mcr_summary_data_pkey PRIMARY KEY (mcr_summary_data_id);


--
-- Name: tbl_mcrdata_birmbeetledat tbl_mcrdata_birmbeetledat_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcrdata_birmbeetledat
    ADD CONSTRAINT tbl_mcrdata_birmbeetledat_pkey PRIMARY KEY (mcrdata_birmbeetledat_id);


--
-- Name: tbl_measured_value_dimensions tbl_measured_value_dimensions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_measured_value_dimensions
    ADD CONSTRAINT tbl_measured_value_dimensions_pkey PRIMARY KEY (measured_value_dimension_id);


--
-- Name: tbl_measured_values tbl_measured_values_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_measured_values
    ADD CONSTRAINT tbl_measured_values_pkey PRIMARY KEY (measured_value_id);


--
-- Name: tbl_method_groups tbl_method_groups_group_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_method_groups
    ADD CONSTRAINT tbl_method_groups_group_name_key UNIQUE (group_name);


--
-- Name: tbl_method_groups tbl_method_groups_group_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_method_groups
    ADD CONSTRAINT tbl_method_groups_group_name_unique UNIQUE (group_name);


--
-- Name: tbl_method_groups tbl_method_groups_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_method_groups
    ADD CONSTRAINT tbl_method_groups_pkey PRIMARY KEY (method_group_id);


--
-- Name: tbl_methods tbl_methods_method_abbrev_or_alt_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT tbl_methods_method_abbrev_or_alt_name_key UNIQUE (method_abbrev_or_alt_name);


--
-- Name: tbl_methods tbl_methods_method_abbrev_or_alt_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT tbl_methods_method_abbrev_or_alt_name_unique UNIQUE (method_abbrev_or_alt_name);


--
-- Name: tbl_methods tbl_methods_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT tbl_methods_pkey PRIMARY KEY (method_id);


--
-- Name: tbl_modification_types tbl_modification_types_modification_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_modification_types
    ADD CONSTRAINT tbl_modification_types_modification_type_name_key UNIQUE (modification_type_name);


--
-- Name: tbl_modification_types tbl_modification_types_modification_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_modification_types
    ADD CONSTRAINT tbl_modification_types_modification_type_name_unique UNIQUE (modification_type_name);


--
-- Name: tbl_modification_types tbl_modification_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_modification_types
    ADD CONSTRAINT tbl_modification_types_pkey PRIMARY KEY (modification_type_id);


--
-- Name: tbl_physical_sample_features tbl_physical_sample_features_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_sample_features
    ADD CONSTRAINT tbl_physical_sample_features_pkey PRIMARY KEY (physical_sample_feature_id);


--
-- Name: tbl_physical_samples tbl_physical_samples_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_samples
    ADD CONSTRAINT tbl_physical_samples_pkey PRIMARY KEY (physical_sample_id);


--
-- Name: tbl_project_stages tbl_project_stages_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_stages
    ADD CONSTRAINT tbl_project_stages_pkey PRIMARY KEY (project_stage_id);


--
-- Name: tbl_project_stages tbl_project_stages_stage_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_stages
    ADD CONSTRAINT tbl_project_stages_stage_name_key UNIQUE (stage_name);


--
-- Name: tbl_project_stages tbl_project_stages_stage_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_stages
    ADD CONSTRAINT tbl_project_stages_stage_name_unique UNIQUE (stage_name);


--
-- Name: tbl_project_types tbl_project_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_types
    ADD CONSTRAINT tbl_project_types_pkey PRIMARY KEY (project_type_id);


--
-- Name: tbl_project_types tbl_project_types_project_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_types
    ADD CONSTRAINT tbl_project_types_project_type_name_key UNIQUE (project_type_name);


--
-- Name: tbl_project_types tbl_project_types_project_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_project_types
    ADD CONSTRAINT tbl_project_types_project_type_name_unique UNIQUE (project_type_name);


--
-- Name: tbl_projects tbl_projects_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_projects
    ADD CONSTRAINT tbl_projects_pkey PRIMARY KEY (project_id);


--
-- Name: tbl_projects tbl_projects_project_abbrev_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_projects
    ADD CONSTRAINT tbl_projects_project_abbrev_name_unique UNIQUE (project_abbrev_name);


--
-- Name: tbl_rdb_codes tbl_rdb_codes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_codes
    ADD CONSTRAINT tbl_rdb_codes_pkey PRIMARY KEY (rdb_code_id);


--
-- Name: tbl_rdb tbl_rdb_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb
    ADD CONSTRAINT tbl_rdb_pkey PRIMARY KEY (rdb_id);


--
-- Name: tbl_rdb_systems tbl_rdb_systems_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_systems
    ADD CONSTRAINT tbl_rdb_systems_pkey PRIMARY KEY (rdb_system_id);


--
-- Name: tbl_record_types tbl_record_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_record_types
    ADD CONSTRAINT tbl_record_types_pkey PRIMARY KEY (record_type_id);


--
-- Name: tbl_record_types tbl_record_types_record_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_record_types
    ADD CONSTRAINT tbl_record_types_record_type_name_key UNIQUE (record_type_name);


--
-- Name: tbl_record_types tbl_record_types_record_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_record_types
    ADD CONSTRAINT tbl_record_types_record_type_name_unique UNIQUE (record_type_name);


--
-- Name: tbl_relative_age_refs tbl_relative_age_refs_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_refs
    ADD CONSTRAINT tbl_relative_age_refs_pkey PRIMARY KEY (relative_age_ref_id);


--
-- Name: tbl_relative_age_types tbl_relative_age_types_age_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_types
    ADD CONSTRAINT tbl_relative_age_types_age_type_key UNIQUE (age_type);


--
-- Name: tbl_relative_age_types tbl_relative_age_types_age_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_types
    ADD CONSTRAINT tbl_relative_age_types_age_type_unique UNIQUE (age_type);


--
-- Name: tbl_relative_age_types tbl_relative_age_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_types
    ADD CONSTRAINT tbl_relative_age_types_pkey PRIMARY KEY (relative_age_type_id);


--
-- Name: tbl_relative_ages tbl_relative_ages_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_ages
    ADD CONSTRAINT tbl_relative_ages_pkey PRIMARY KEY (relative_age_id);


--
-- Name: tbl_relative_dates tbl_relative_dates_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_dates
    ADD CONSTRAINT tbl_relative_dates_pkey PRIMARY KEY (relative_date_id);


--
-- Name: tbl_sample_alt_refs tbl_sample_alt_refs_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_alt_refs
    ADD CONSTRAINT tbl_sample_alt_refs_pkey PRIMARY KEY (sample_alt_ref_id);


--
-- Name: tbl_sample_colours tbl_sample_colours_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_colours
    ADD CONSTRAINT tbl_sample_colours_pkey PRIMARY KEY (sample_colour_id);


--
-- Name: tbl_sample_coordinates tbl_sample_coordinates_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_coordinates
    ADD CONSTRAINT tbl_sample_coordinates_pkey PRIMARY KEY (sample_coordinate_id);


--
-- Name: tbl_sample_description_sample_group_contexts tbl_sample_description_sample_group_contexts_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_sample_group_contexts
    ADD CONSTRAINT tbl_sample_description_sample_group_contexts_pkey PRIMARY KEY (sample_description_sample_group_context_id);


--
-- Name: tbl_sample_description_types tbl_sample_description_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_types
    ADD CONSTRAINT tbl_sample_description_types_pkey PRIMARY KEY (sample_description_type_id);


--
-- Name: tbl_sample_description_types tbl_sample_description_types_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_types
    ADD CONSTRAINT tbl_sample_description_types_type_name_key UNIQUE (type_name);


--
-- Name: tbl_sample_description_types tbl_sample_description_types_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_types
    ADD CONSTRAINT tbl_sample_description_types_type_name_unique UNIQUE (type_name);


--
-- Name: tbl_sample_descriptions tbl_sample_descriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_descriptions
    ADD CONSTRAINT tbl_sample_descriptions_pkey PRIMARY KEY (sample_description_id);


--
-- Name: tbl_sample_dimensions tbl_sample_dimensions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_dimensions
    ADD CONSTRAINT tbl_sample_dimensions_pkey PRIMARY KEY (sample_dimension_id);


--
-- Name: tbl_sample_group_coordinates tbl_sample_group_coordinates_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_coordinates
    ADD CONSTRAINT tbl_sample_group_coordinates_pkey PRIMARY KEY (sample_group_position_id);


--
-- Name: tbl_sample_group_description_type_sampling_contexts tbl_sample_group_description_type_sampling_contexts_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_type_sampling_contexts
    ADD CONSTRAINT tbl_sample_group_description_type_sampling_contexts_pkey PRIMARY KEY (sample_group_description_type_sampling_context_id);


--
-- Name: tbl_sample_group_description_types tbl_sample_group_description_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_types
    ADD CONSTRAINT tbl_sample_group_description_types_pkey PRIMARY KEY (sample_group_description_type_id);


--
-- Name: tbl_sample_group_description_types tbl_sample_group_description_types_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_types
    ADD CONSTRAINT tbl_sample_group_description_types_type_name_key UNIQUE (type_name);


--
-- Name: tbl_sample_group_description_types tbl_sample_group_description_types_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_types
    ADD CONSTRAINT tbl_sample_group_description_types_type_name_unique UNIQUE (type_name);


--
-- Name: tbl_sample_group_descriptions tbl_sample_group_descriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_descriptions
    ADD CONSTRAINT tbl_sample_group_descriptions_pkey PRIMARY KEY (sample_group_description_id);


--
-- Name: tbl_sample_group_dimensions tbl_sample_group_dimensions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_dimensions
    ADD CONSTRAINT tbl_sample_group_dimensions_pkey PRIMARY KEY (sample_group_dimension_id);


--
-- Name: tbl_sample_group_images tbl_sample_group_images_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_images
    ADD CONSTRAINT tbl_sample_group_images_pkey PRIMARY KEY (sample_group_image_id);


--
-- Name: tbl_sample_group_notes tbl_sample_group_notes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_notes
    ADD CONSTRAINT tbl_sample_group_notes_pkey PRIMARY KEY (sample_group_note_id);


--
-- Name: tbl_sample_group_references tbl_sample_group_references_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_references
    ADD CONSTRAINT tbl_sample_group_references_pkey PRIMARY KEY (sample_group_reference_id);


--
-- Name: tbl_sample_group_sampling_contexts tbl_sample_group_sampling_contexts_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_sampling_contexts
    ADD CONSTRAINT tbl_sample_group_sampling_contexts_pkey PRIMARY KEY (sampling_context_id);


--
-- Name: tbl_sample_groups tbl_sample_groups_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_groups
    ADD CONSTRAINT tbl_sample_groups_pkey PRIMARY KEY (sample_group_id);


--
-- Name: tbl_sample_horizons tbl_sample_horizons_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_horizons
    ADD CONSTRAINT tbl_sample_horizons_pkey PRIMARY KEY (sample_horizon_id);


--
-- Name: tbl_sample_images tbl_sample_images_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_images
    ADD CONSTRAINT tbl_sample_images_pkey PRIMARY KEY (sample_image_id);


--
-- Name: tbl_sample_location_type_sampling_contexts tbl_sample_location_type_sampling_contexts_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_type_sampling_contexts
    ADD CONSTRAINT tbl_sample_location_type_sampling_contexts_pkey PRIMARY KEY (sample_location_type_sampling_context_id);


--
-- Name: tbl_sample_location_types tbl_sample_location_types_location_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_types
    ADD CONSTRAINT tbl_sample_location_types_location_type_key UNIQUE (location_type);


--
-- Name: tbl_sample_location_types tbl_sample_location_types_location_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_types
    ADD CONSTRAINT tbl_sample_location_types_location_type_unique UNIQUE (location_type);


--
-- Name: tbl_sample_location_types tbl_sample_location_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_types
    ADD CONSTRAINT tbl_sample_location_types_pkey PRIMARY KEY (sample_location_type_id);


--
-- Name: tbl_sample_locations tbl_sample_locations_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_locations
    ADD CONSTRAINT tbl_sample_locations_pkey PRIMARY KEY (sample_location_id);


--
-- Name: tbl_sample_notes tbl_sample_notes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_notes
    ADD CONSTRAINT tbl_sample_notes_pkey PRIMARY KEY (sample_note_id);


--
-- Name: tbl_sample_types tbl_sample_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_types
    ADD CONSTRAINT tbl_sample_types_pkey PRIMARY KEY (sample_type_id);


--
-- Name: tbl_sample_types tbl_sample_types_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_types
    ADD CONSTRAINT tbl_sample_types_type_name_key UNIQUE (type_name);


--
-- Name: tbl_sample_types tbl_sample_types_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_types
    ADD CONSTRAINT tbl_sample_types_type_name_unique UNIQUE (type_name);


--
-- Name: tbl_season_types tbl_season_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_season_types
    ADD CONSTRAINT tbl_season_types_pkey PRIMARY KEY (season_type_id);


--
-- Name: tbl_season_types tbl_season_types_season_type_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_season_types
    ADD CONSTRAINT tbl_season_types_season_type_key UNIQUE (season_type);


--
-- Name: tbl_season_types tbl_season_types_season_type_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_season_types
    ADD CONSTRAINT tbl_season_types_season_type_unique UNIQUE (season_type);


--
-- Name: tbl_seasons tbl_seasons_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_seasons
    ADD CONSTRAINT tbl_seasons_pkey PRIMARY KEY (season_id);


--
-- Name: tbl_seasons tbl_seasons_season_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_seasons
    ADD CONSTRAINT tbl_seasons_season_name_key UNIQUE (season_name);


--
-- Name: tbl_seasons tbl_seasons_season_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_seasons
    ADD CONSTRAINT tbl_seasons_season_name_unique UNIQUE (season_name);


--
-- Name: tbl_site_images tbl_site_images_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_images
    ADD CONSTRAINT tbl_site_images_pkey PRIMARY KEY (site_image_id);


--
-- Name: tbl_site_locations tbl_site_locations_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_locations
    ADD CONSTRAINT tbl_site_locations_pkey PRIMARY KEY (site_location_id);


--
-- Name: tbl_site_natgridrefs tbl_site_natgridrefs_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_natgridrefs
    ADD CONSTRAINT tbl_site_natgridrefs_pkey PRIMARY KEY (site_natgridref_id);


--
-- Name: tbl_site_other_records tbl_site_other_records_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_other_records
    ADD CONSTRAINT tbl_site_other_records_pkey PRIMARY KEY (site_other_records_id);


--
-- Name: tbl_site_preservation_status tbl_site_preservation_status_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_preservation_status
    ADD CONSTRAINT tbl_site_preservation_status_pkey PRIMARY KEY (site_preservation_status_id);


--
-- Name: tbl_site_references tbl_site_references_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_references
    ADD CONSTRAINT tbl_site_references_pkey PRIMARY KEY (site_reference_id);


--
-- Name: tbl_sites tbl_sites_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sites
    ADD CONSTRAINT tbl_sites_pkey PRIMARY KEY (site_id);


--
-- Name: tbl_species_association_types tbl_species_association_types_association_type_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_association_types
    ADD CONSTRAINT tbl_species_association_types_association_type_name_key UNIQUE (association_type_name);


--
-- Name: tbl_species_association_types tbl_species_association_types_association_type_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_association_types
    ADD CONSTRAINT tbl_species_association_types_association_type_name_unique UNIQUE (association_type_name);


--
-- Name: tbl_species_association_types tbl_species_association_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_association_types
    ADD CONSTRAINT tbl_species_association_types_pkey PRIMARY KEY (association_type_id);


--
-- Name: tbl_species_associations tbl_species_associations_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_associations
    ADD CONSTRAINT tbl_species_associations_pkey PRIMARY KEY (species_association_id);


--
-- Name: tbl_taxa_common_names tbl_taxa_common_names_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_common_names
    ADD CONSTRAINT tbl_taxa_common_names_pkey PRIMARY KEY (taxon_common_name_id);


--
-- Name: tbl_taxa_images tbl_taxa_images_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_images
    ADD CONSTRAINT tbl_taxa_images_pkey PRIMARY KEY (taxa_images_id);


--
-- Name: tbl_taxa_measured_attributes tbl_taxa_measured_attributes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_measured_attributes
    ADD CONSTRAINT tbl_taxa_measured_attributes_pkey PRIMARY KEY (measured_attribute_id);


--
-- Name: tbl_taxa_reference_specimens tbl_taxa_reference_specimens_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_reference_specimens
    ADD CONSTRAINT tbl_taxa_reference_specimens_pkey PRIMARY KEY (taxa_reference_specimen_id);


--
-- Name: tbl_taxa_seasonality tbl_taxa_seasonality_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_seasonality
    ADD CONSTRAINT tbl_taxa_seasonality_pkey PRIMARY KEY (seasonality_id);


--
-- Name: tbl_taxa_synonyms tbl_taxa_synonyms_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms
    ADD CONSTRAINT tbl_taxa_synonyms_pkey PRIMARY KEY (synonym_id);


--
-- Name: tbl_taxa_tree_authors tbl_taxa_tree_authors_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_authors
    ADD CONSTRAINT tbl_taxa_tree_authors_pkey PRIMARY KEY (author_id);


--
-- Name: tbl_taxa_tree_families tbl_taxa_tree_families_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_families
    ADD CONSTRAINT tbl_taxa_tree_families_pkey PRIMARY KEY (family_id);


--
-- Name: tbl_taxa_tree_genera tbl_taxa_tree_genera_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_genera
    ADD CONSTRAINT tbl_taxa_tree_genera_pkey PRIMARY KEY (genus_id);


--
-- Name: tbl_taxa_tree_master tbl_taxa_tree_master_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_master
    ADD CONSTRAINT tbl_taxa_tree_master_pkey PRIMARY KEY (taxon_id);


--
-- Name: tbl_taxa_tree_orders tbl_taxa_tree_orders_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_orders
    ADD CONSTRAINT tbl_taxa_tree_orders_pkey PRIMARY KEY (order_id);


--
-- Name: tbl_taxonomic_order_biblio tbl_taxonomic_order_biblio_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order_biblio
    ADD CONSTRAINT tbl_taxonomic_order_biblio_pkey PRIMARY KEY (taxonomic_order_biblio_id);


--
-- Name: tbl_taxonomic_order tbl_taxonomic_order_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order
    ADD CONSTRAINT tbl_taxonomic_order_pkey PRIMARY KEY (taxonomic_order_id);


--
-- Name: tbl_taxonomic_order_systems tbl_taxonomic_order_systems_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order_systems
    ADD CONSTRAINT tbl_taxonomic_order_systems_pkey PRIMARY KEY (taxonomic_order_system_id);


--
-- Name: tbl_taxonomy_notes tbl_taxonomy_notes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomy_notes
    ADD CONSTRAINT tbl_taxonomy_notes_pkey PRIMARY KEY (taxonomy_notes_id);


--
-- Name: tbl_temperatures tbl_temperatures_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_temperatures
    ADD CONSTRAINT tbl_temperatures_pkey PRIMARY KEY (record_id);


--
-- Name: tbl_tephra_dates tbl_tephra_dates_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_dates
    ADD CONSTRAINT tbl_tephra_dates_pkey PRIMARY KEY (tephra_date_id);


--
-- Name: tbl_tephra_refs tbl_tephra_refs_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_refs
    ADD CONSTRAINT tbl_tephra_refs_pkey PRIMARY KEY (tephra_ref_id);


--
-- Name: tbl_tephras tbl_tephras_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephras
    ADD CONSTRAINT tbl_tephras_pkey PRIMARY KEY (tephra_id);


--
-- Name: tbl_text_biology tbl_text_biology_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_biology
    ADD CONSTRAINT tbl_text_biology_pkey PRIMARY KEY (biology_id);


--
-- Name: tbl_text_distribution tbl_text_distribution_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_distribution
    ADD CONSTRAINT tbl_text_distribution_pkey PRIMARY KEY (distribution_id);


--
-- Name: tbl_text_identification_keys tbl_text_identification_keys_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_identification_keys
    ADD CONSTRAINT tbl_text_identification_keys_pkey PRIMARY KEY (key_id);


--
-- Name: tbl_units tbl_units_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_units
    ADD CONSTRAINT tbl_units_pkey PRIMARY KEY (unit_id);


--
-- Name: tbl_units tbl_units_unit_abbrev_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_units
    ADD CONSTRAINT tbl_units_unit_abbrev_key UNIQUE (unit_abbrev);


--
-- Name: tbl_units tbl_units_unit_abbrev_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_units
    ADD CONSTRAINT tbl_units_unit_abbrev_unique UNIQUE (unit_abbrev);


--
-- Name: tbl_units tbl_units_unit_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_units
    ADD CONSTRAINT tbl_units_unit_name_key UNIQUE (unit_name);


--
-- Name: tbl_units tbl_units_unit_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_units
    ADD CONSTRAINT tbl_units_unit_name_unique UNIQUE (unit_name);


--
-- Name: tbl_updates_log tbl_updates_log_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_updates_log
    ADD CONSTRAINT tbl_updates_log_pkey PRIMARY KEY (updates_log_id);


--
-- Name: tbl_value_classes tbl_value_classes_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_classes
    ADD CONSTRAINT tbl_value_classes_pkey PRIMARY KEY (value_class_id);


--
-- Name: tbl_value_qualifier_symbols tbl_value_qualifier_symbols_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_qualifier_symbols
    ADD CONSTRAINT tbl_value_qualifier_symbols_pkey PRIMARY KEY (qualifier_symbol_id);


--
-- Name: tbl_value_qualifier_symbols tbl_value_qualifier_symbols_symbol_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_qualifier_symbols
    ADD CONSTRAINT tbl_value_qualifier_symbols_symbol_key UNIQUE (symbol);


--
-- Name: tbl_value_qualifiers tbl_value_qualifiers_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_qualifiers
    ADD CONSTRAINT tbl_value_qualifiers_pkey PRIMARY KEY (qualifier_id);


--
-- Name: tbl_value_qualifiers tbl_value_qualifiers_symbol_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_qualifiers
    ADD CONSTRAINT tbl_value_qualifiers_symbol_key UNIQUE (symbol);


--
-- Name: tbl_value_type_items tbl_value_type_items_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_type_items
    ADD CONSTRAINT tbl_value_type_items_pkey PRIMARY KEY (value_type_item_id);


--
-- Name: tbl_value_types tbl_value_types_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_types
    ADD CONSTRAINT tbl_value_types_name_key UNIQUE (name);


--
-- Name: tbl_value_types tbl_value_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_types
    ADD CONSTRAINT tbl_value_types_pkey PRIMARY KEY (value_type_id);


--
-- Name: tbl_years_types tbl_years_types_name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_years_types
    ADD CONSTRAINT tbl_years_types_name_key UNIQUE (name);


--
-- Name: tbl_years_types tbl_years_types_name_unique; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_years_types
    ADD CONSTRAINT tbl_years_types_name_unique UNIQUE (name);


--
-- Name: tbl_years_types tbl_years_types_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_years_types
    ADD CONSTRAINT tbl_years_types_pkey PRIMARY KEY (years_type_id);


--
-- Name: tbl_aggregate_datasets unique_tbl_aggregate_datasets_aggregate_dataset_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_datasets
    ADD CONSTRAINT unique_tbl_aggregate_datasets_aggregate_dataset_uuid UNIQUE (aggregate_dataset_uuid);


--
-- Name: tbl_biblio unique_tbl_biblio_biblio_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_biblio
    ADD CONSTRAINT unique_tbl_biblio_biblio_uuid UNIQUE (biblio_uuid);


--
-- Name: tbl_dataset_masters unique_tbl_dataset_masters_master_set_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_masters
    ADD CONSTRAINT unique_tbl_dataset_masters_master_set_uuid UNIQUE (master_set_uuid);


--
-- Name: tbl_datasets unique_tbl_datasets_dataset_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT unique_tbl_datasets_dataset_uuid UNIQUE (dataset_uuid);


--
-- Name: tbl_ecocode_systems unique_tbl_ecocode_systems_ecocode_system_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_systems
    ADD CONSTRAINT unique_tbl_ecocode_systems_ecocode_system_uuid UNIQUE (ecocode_system_uuid);


--
-- Name: tbl_geochronology unique_tbl_geochronology_geochron_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochronology
    ADD CONSTRAINT unique_tbl_geochronology_geochron_uuid UNIQUE (geochron_uuid);


--
-- Name: tbl_methods unique_tbl_methods_method_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT unique_tbl_methods_method_uuid UNIQUE (method_uuid);


--
-- Name: tbl_rdb_systems unique_tbl_rdb_systems_rdb_system_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_systems
    ADD CONSTRAINT unique_tbl_rdb_systems_rdb_system_uuid UNIQUE (rdb_system_uuid);


--
-- Name: tbl_relative_ages unique_tbl_relative_ages_relative_age_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_ages
    ADD CONSTRAINT unique_tbl_relative_ages_relative_age_uuid UNIQUE (relative_age_uuid);


--
-- Name: tbl_sample_groups unique_tbl_sample_groups_sample_group_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_groups
    ADD CONSTRAINT unique_tbl_sample_groups_sample_group_uuid UNIQUE (sample_group_uuid);


--
-- Name: tbl_site_other_records unique_tbl_site_other_records_site_other_records_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_other_records
    ADD CONSTRAINT unique_tbl_site_other_records_site_other_records_uuid UNIQUE (site_other_records_uuid);


--
-- Name: tbl_sites unique_tbl_sites_site_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sites
    ADD CONSTRAINT unique_tbl_sites_site_uuid UNIQUE (site_uuid);


--
-- Name: tbl_species_associations unique_tbl_species_associations_species_association_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_associations
    ADD CONSTRAINT unique_tbl_species_associations_species_association_uuid UNIQUE (species_association_uuid);


--
-- Name: tbl_taxa_synonyms unique_tbl_taxa_synonyms_synonym_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms
    ADD CONSTRAINT unique_tbl_taxa_synonyms_synonym_uuid UNIQUE (synonym_uuid);


--
-- Name: tbl_taxonomic_order_systems unique_tbl_taxonomic_order_systems_taxonomic_order_system_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order_systems
    ADD CONSTRAINT unique_tbl_taxonomic_order_systems_taxonomic_order_system_uuid UNIQUE (taxonomic_order_system_uuid);


--
-- Name: tbl_taxonomy_notes unique_tbl_taxonomy_notes_taxonomy_notes_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomy_notes
    ADD CONSTRAINT unique_tbl_taxonomy_notes_taxonomy_notes_uuid UNIQUE (taxonomy_notes_uuid);


--
-- Name: tbl_tephras unique_tbl_tephras_tephra_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephras
    ADD CONSTRAINT unique_tbl_tephras_tephra_uuid UNIQUE (tephra_uuid);


--
-- Name: tbl_text_biology unique_tbl_text_biology_biology_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_biology
    ADD CONSTRAINT unique_tbl_text_biology_biology_uuid UNIQUE (biology_uuid);


--
-- Name: tbl_text_distribution unique_tbl_text_distribution_distribution_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_distribution
    ADD CONSTRAINT unique_tbl_text_distribution_distribution_uuid UNIQUE (distribution_uuid);


--
-- Name: tbl_text_identification_keys unique_tbl_text_identification_keys_key_uuid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_identification_keys
    ADD CONSTRAINT unique_tbl_text_identification_keys_key_uuid UNIQUE (key_uuid);


--
-- Name: tbl_site_references uq_site_references; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_references
    ADD CONSTRAINT uq_site_references UNIQUE (site_id, biblio_id);


--
-- Name: tbl_sample_alt_refs uq_tbl_sample_alt_refs; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_alt_refs
    ADD CONSTRAINT uq_tbl_sample_alt_refs UNIQUE (physical_sample_id, alt_ref, alt_ref_type_id);


--
-- Name: idx_abundance_modifications_abundance_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_abundance_modifications_abundance_id ON public.tbl_abundance_modifications USING btree (abundance_id);


--
-- Name: idx_abundance_modifications_modification_type_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_abundance_modifications_modification_type_id ON public.tbl_abundance_modifications USING btree (modification_type_id);


--
-- Name: idx_abundances_abundance_element_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_abundances_abundance_element_id ON public.tbl_abundances USING btree (abundance_element_id);


--
-- Name: idx_abundances_analysis_entity_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_abundances_analysis_entity_id ON public.tbl_abundances USING btree (analysis_entity_id);


--
-- Name: idx_abundances_taxon_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_abundances_taxon_id ON public.tbl_abundances USING btree (taxon_id);


--
-- Name: idx_analysis_entities_dataset_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_analysis_entities_dataset_id ON public.tbl_analysis_entities USING btree (dataset_id);


--
-- Name: idx_analysis_entities_physical_sample_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_analysis_entities_physical_sample_id ON public.tbl_analysis_entities USING btree (physical_sample_id);


--
-- Name: idx_analysis_entity_prep_methods_analysis_entity_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_analysis_entity_prep_methods_analysis_entity_id ON public.tbl_analysis_entity_prep_methods USING btree (analysis_entity_id);


--
-- Name: idx_analysis_entity_prep_methods_method_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_analysis_entity_prep_methods_method_id ON public.tbl_analysis_entity_prep_methods USING btree (method_id);


--
-- Name: idx_biblio_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_biblio_id ON public.tbl_sample_group_references USING btree (biblio_id);


--
-- Name: idx_datasets_biblio_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_datasets_biblio_id ON public.tbl_datasets USING btree (biblio_id);


--
-- Name: idx_datasets_data_type_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_datasets_data_type_id ON public.tbl_datasets USING btree (data_type_id);


--
-- Name: idx_datasets_master_set_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_datasets_master_set_id ON public.tbl_datasets USING btree (master_set_id);


--
-- Name: idx_datasets_method_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_datasets_method_id ON public.tbl_datasets USING btree (method_id);


--
-- Name: idx_datasets_project_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_datasets_project_id ON public.tbl_datasets USING btree (project_id);


--
-- Name: idx_datasets_updated_dataset_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_datasets_updated_dataset_id ON public.tbl_datasets USING btree (updated_dataset_id);


--
-- Name: idx_ecocode_groups_ecocode_system_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_ecocode_groups_ecocode_system_id ON public.tbl_ecocode_groups USING btree (ecocode_system_id);


--
-- Name: idx_ecocode_groups_name; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_ecocode_groups_name ON public.tbl_ecocode_groups USING btree (name);


--
-- Name: idx_ecocode_systems_biblio_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_ecocode_systems_biblio_id ON public.tbl_ecocode_systems USING btree (biblio_id);


--
-- Name: idx_ecocode_systems_ecocode_group_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_ecocode_systems_ecocode_group_id ON public.tbl_ecocode_systems USING btree (name);


--
-- Name: idx_ecocodes_ecocode_definition_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_ecocodes_ecocode_definition_id ON public.tbl_ecocodes USING btree (ecocode_definition_id);


--
-- Name: idx_ecocodes_taxon_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_ecocodes_taxon_id ON public.tbl_ecocodes USING btree (taxon_id);


--
-- Name: idx_features_feature_type_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_features_feature_type_id ON public.tbl_features USING btree (feature_type_id);


--
-- Name: idx_languages_language_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_languages_language_id ON public.tbl_languages USING btree (language_id);


--
-- Name: idx_locations_location_type_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_locations_location_type_id ON public.tbl_locations USING btree (location_type_id);


--
-- Name: idx_measured_values_analysis_entity_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_measured_values_analysis_entity_id ON public.tbl_measured_values USING btree (analysis_entity_id);


--
-- Name: idx_physical_sample_features_feature_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_physical_sample_features_feature_id ON public.tbl_physical_sample_features USING btree (feature_id);


--
-- Name: idx_physical_sample_features_physical_sample_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_physical_sample_features_physical_sample_id ON public.tbl_physical_sample_features USING btree (physical_sample_id);


--
-- Name: idx_physical_samples_alt_ref_type_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_physical_samples_alt_ref_type_id ON public.tbl_physical_samples USING btree (alt_ref_type_id);


--
-- Name: idx_physical_samples_sample_group_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_physical_samples_sample_group_id ON public.tbl_physical_samples USING btree (sample_group_id);


--
-- Name: idx_physical_samples_sample_type_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_physical_samples_sample_type_id ON public.tbl_physical_samples USING btree (sample_type_id);


--
-- Name: idx_sample_alt_refs_alt_ref_type_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_alt_refs_alt_ref_type_id ON public.tbl_sample_alt_refs USING btree (alt_ref_type_id);


--
-- Name: idx_sample_alt_refs_physical_sample_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_alt_refs_physical_sample_id ON public.tbl_sample_alt_refs USING btree (physical_sample_id);


--
-- Name: idx_sample_coordinates_coordinate_method_dimension_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_coordinates_coordinate_method_dimension_id ON public.tbl_sample_coordinates USING btree (coordinate_method_dimension_id);


--
-- Name: idx_sample_coordinates_physical_sample_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_coordinates_physical_sample_id ON public.tbl_sample_coordinates USING btree (physical_sample_id);


--
-- Name: idx_sample_group_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_group_id ON public.tbl_sample_group_references USING btree (sample_group_id);


--
-- Name: idx_sample_horizons_horizon_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_horizons_horizon_id ON public.tbl_sample_horizons USING btree (horizon_id);


--
-- Name: idx_sample_horizons_physical_sample_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_horizons_physical_sample_id ON public.tbl_sample_horizons USING btree (physical_sample_id);


--
-- Name: idx_sample_notes_physical_sample_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sample_notes_physical_sample_id ON public.tbl_sample_notes USING btree (physical_sample_id);


--
-- Name: idx_site_locations_location_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_site_locations_location_id ON public.tbl_site_locations USING btree (location_id);


--
-- Name: idx_site_locations_site_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_site_locations_site_id ON public.tbl_site_locations USING btree (site_id);


--
-- Name: idx_taxa_common_names_language_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_common_names_language_id ON public.tbl_taxa_common_names USING btree (language_id);


--
-- Name: idx_taxa_common_names_taxon_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_common_names_taxon_id ON public.tbl_taxa_common_names USING btree (taxon_id);


--
-- Name: idx_taxa_tree_authors_name; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_authors_name ON public.tbl_taxa_tree_authors USING btree (author_name);


--
-- Name: idx_taxa_tree_families_name; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_families_name ON public.tbl_taxa_tree_families USING btree (family_name);


--
-- Name: idx_taxa_tree_families_order_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_families_order_id ON public.tbl_taxa_tree_families USING btree (order_id);


--
-- Name: idx_taxa_tree_genera_family_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_genera_family_id ON public.tbl_taxa_tree_genera USING btree (family_id);


--
-- Name: idx_taxa_tree_genera_name; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_genera_name ON public.tbl_taxa_tree_genera USING btree (genus_name);


--
-- Name: idx_taxa_tree_master_author_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_master_author_id ON public.tbl_taxa_tree_master USING btree (author_id);


--
-- Name: idx_taxa_tree_master_genus_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_master_genus_id ON public.tbl_taxa_tree_master USING btree (genus_id);


--
-- Name: idx_taxa_tree_orders_order_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxa_tree_orders_order_id ON public.tbl_taxa_tree_orders USING btree (order_id);


--
-- Name: idx_taxonomic_order_biblio_biblio_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_biblio_biblio_id ON public.tbl_taxonomic_order_biblio USING btree (biblio_id);


--
-- Name: idx_taxonomic_order_biblio_taxonomic_order_biblio_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_biblio_taxonomic_order_biblio_id ON public.tbl_taxonomic_order_biblio USING btree (taxonomic_order_biblio_id);


--
-- Name: idx_taxonomic_order_biblio_taxonomic_order_system_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_biblio_taxonomic_order_system_id ON public.tbl_taxonomic_order_biblio USING btree (taxonomic_order_system_id);


--
-- Name: idx_taxonomic_order_systems_taxonomic_system_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_systems_taxonomic_system_id ON public.tbl_taxonomic_order_systems USING btree (taxonomic_order_system_id);


--
-- Name: idx_taxonomic_order_taxon_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_taxon_id ON public.tbl_taxonomic_order USING btree (taxon_id);


--
-- Name: idx_taxonomic_order_taxonomic_code; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_taxonomic_code ON public.tbl_taxonomic_order USING btree (taxonomic_code);


--
-- Name: idx_taxonomic_order_taxonomic_order_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_taxonomic_order_id ON public.tbl_taxonomic_order USING btree (taxonomic_order_id);


--
-- Name: idx_taxonomic_order_taxonomic_system_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_taxonomic_order_taxonomic_system_id ON public.tbl_taxonomic_order USING btree (taxonomic_order_system_id);


--
-- Name: idx_tbl_physical_sample_features_feature_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_tbl_physical_sample_features_feature_id ON public.tbl_physical_sample_features USING btree (feature_id);


--
-- Name: tbl_ecocode_groups_idx_name; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX tbl_ecocode_groups_idx_name ON public.tbl_ecocode_groups USING btree (name);


--
-- Name: tbl_abundance_elements fk_abundance_elements_record_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_elements
    ADD CONSTRAINT fk_abundance_elements_record_type_id FOREIGN KEY (record_type_id) REFERENCES public.tbl_record_types(record_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_abundance_ident_levels fk_abundance_ident_levels_abundance_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_ident_levels
    ADD CONSTRAINT fk_abundance_ident_levels_abundance_id FOREIGN KEY (abundance_id) REFERENCES public.tbl_abundances(abundance_id) DEFERRABLE;


--
-- Name: tbl_abundance_ident_levels fk_abundance_ident_levels_identification_level_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_ident_levels
    ADD CONSTRAINT fk_abundance_ident_levels_identification_level_id FOREIGN KEY (identification_level_id) REFERENCES public.tbl_identification_levels(identification_level_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_abundance_modifications fk_abundance_modifications_abundance_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_modifications
    ADD CONSTRAINT fk_abundance_modifications_abundance_id FOREIGN KEY (abundance_id) REFERENCES public.tbl_abundances(abundance_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_abundance_modifications fk_abundance_modifications_modification_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundance_modifications
    ADD CONSTRAINT fk_abundance_modifications_modification_type_id FOREIGN KEY (modification_type_id) REFERENCES public.tbl_modification_types(modification_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_abundances fk_abundances_abundance_elements_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundances
    ADD CONSTRAINT fk_abundances_abundance_elements_id FOREIGN KEY (abundance_element_id) REFERENCES public.tbl_abundance_elements(abundance_element_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_abundances fk_abundances_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundances
    ADD CONSTRAINT fk_abundances_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_abundances fk_abundances_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_abundances
    ADD CONSTRAINT fk_abundances_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_aggregate_samples fk_aggragate_samples_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_samples
    ADD CONSTRAINT fk_aggragate_samples_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_aggregate_datasets fk_aggregate_datasets_aggregate_order_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_datasets
    ADD CONSTRAINT fk_aggregate_datasets_aggregate_order_type_id FOREIGN KEY (aggregate_order_type_id) REFERENCES public.tbl_aggregate_order_types(aggregate_order_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_aggregate_datasets fk_aggregate_datasets_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_datasets
    ADD CONSTRAINT fk_aggregate_datasets_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_aggregate_sample_ages fk_aggregate_sample_ages_aggregate_dataset_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_sample_ages
    ADD CONSTRAINT fk_aggregate_sample_ages_aggregate_dataset_id FOREIGN KEY (aggregate_dataset_id) REFERENCES public.tbl_aggregate_datasets(aggregate_dataset_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_aggregate_sample_ages fk_aggregate_sample_ages_analysis_entity_age_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_sample_ages
    ADD CONSTRAINT fk_aggregate_sample_ages_analysis_entity_age_id FOREIGN KEY (analysis_entity_age_id) REFERENCES public.tbl_analysis_entity_ages(analysis_entity_age_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_aggregate_samples fk_aggregate_samples_aggregate_dataset_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_aggregate_samples
    ADD CONSTRAINT fk_aggregate_samples_aggregate_dataset_id FOREIGN KEY (aggregate_dataset_id) REFERENCES public.tbl_aggregate_datasets(aggregate_dataset_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_entities fk_analysis_entities_dataset_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entities
    ADD CONSTRAINT fk_analysis_entities_dataset_id FOREIGN KEY (dataset_id) REFERENCES public.tbl_datasets(dataset_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_entities fk_analysis_entities_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entities
    ADD CONSTRAINT fk_analysis_entities_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) DEFERRABLE;


--
-- Name: tbl_analysis_entity_ages fk_analysis_entity_ages_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_ages
    ADD CONSTRAINT fk_analysis_entity_ages_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_entity_ages fk_analysis_entity_ages_chronology_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_ages
    ADD CONSTRAINT fk_analysis_entity_ages_chronology_id FOREIGN KEY (chronology_id) REFERENCES public.tbl_chronologies(chronology_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_entity_dimensions fk_analysis_entity_dimensions_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_dimensions
    ADD CONSTRAINT fk_analysis_entity_dimensions_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_entity_dimensions fk_analysis_entity_dimensions_dimension_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_dimensions
    ADD CONSTRAINT fk_analysis_entity_dimensions_dimension_id FOREIGN KEY (dimension_id) REFERENCES public.tbl_dimensions(dimension_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_entity_prep_methods fk_analysis_entity_prep_methods_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_prep_methods
    ADD CONSTRAINT fk_analysis_entity_prep_methods_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) DEFERRABLE;


--
-- Name: tbl_analysis_entity_prep_methods fk_analysis_entity_prep_methods_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_entity_prep_methods
    ADD CONSTRAINT fk_analysis_entity_prep_methods_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_ceramics fk_ceramics_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics
    ADD CONSTRAINT fk_ceramics_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) DEFERRABLE;


--
-- Name: tbl_ceramics fk_ceramics_ceramics_lookup_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics
    ADD CONSTRAINT fk_ceramics_ceramics_lookup_id FOREIGN KEY (ceramics_lookup_id) REFERENCES public.tbl_ceramics_lookup(ceramics_lookup_id) DEFERRABLE;


--
-- Name: tbl_ceramics_lookup fk_ceramics_lookup_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics_lookup
    ADD CONSTRAINT fk_ceramics_lookup_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_ceramics_measurements fk_ceramics_measurements_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ceramics_measurements
    ADD CONSTRAINT fk_ceramics_measurements_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_chronologies fk_chronologies_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_chronologies
    ADD CONSTRAINT fk_chronologies_contact_id FOREIGN KEY (contact_id) REFERENCES public.tbl_contacts(contact_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_colours fk_colours_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_colours
    ADD CONSTRAINT fk_colours_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_coordinate_method_dimensions fk_coordinate_method_dimensions_dimensions_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_coordinate_method_dimensions
    ADD CONSTRAINT fk_coordinate_method_dimensions_dimensions_id FOREIGN KEY (dimension_id) REFERENCES public.tbl_dimensions(dimension_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_coordinate_method_dimensions fk_coordinate_method_dimensions_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_coordinate_method_dimensions
    ADD CONSTRAINT fk_coordinate_method_dimensions_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_data_types fk_data_types_data_type_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_data_types
    ADD CONSTRAINT fk_data_types_data_type_group_id FOREIGN KEY (data_type_group_id) REFERENCES public.tbl_data_type_groups(data_type_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dataset_contacts fk_dataset_contacts_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_contacts
    ADD CONSTRAINT fk_dataset_contacts_contact_id FOREIGN KEY (contact_id) REFERENCES public.tbl_contacts(contact_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dataset_contacts fk_dataset_contacts_contact_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_contacts
    ADD CONSTRAINT fk_dataset_contacts_contact_type_id FOREIGN KEY (contact_type_id) REFERENCES public.tbl_contact_types(contact_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dataset_contacts fk_dataset_contacts_dataset_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_contacts
    ADD CONSTRAINT fk_dataset_contacts_dataset_id FOREIGN KEY (dataset_id) REFERENCES public.tbl_datasets(dataset_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dataset_masters fk_dataset_masters_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_masters
    ADD CONSTRAINT fk_dataset_masters_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) DEFERRABLE;


--
-- Name: tbl_dataset_masters fk_dataset_masters_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_masters
    ADD CONSTRAINT fk_dataset_masters_contact_id FOREIGN KEY (contact_id) REFERENCES public.tbl_contacts(contact_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dataset_submissions fk_dataset_submission_submission_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_submissions
    ADD CONSTRAINT fk_dataset_submission_submission_type_id FOREIGN KEY (submission_type_id) REFERENCES public.tbl_dataset_submission_types(submission_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dataset_submissions fk_dataset_submissions_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_submissions
    ADD CONSTRAINT fk_dataset_submissions_contact_id FOREIGN KEY (contact_id) REFERENCES public.tbl_contacts(contact_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dataset_submissions fk_dataset_submissions_dataset_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_submissions
    ADD CONSTRAINT fk_dataset_submissions_dataset_id FOREIGN KEY (dataset_id) REFERENCES public.tbl_datasets(dataset_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_datasets fk_datasets_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT fk_datasets_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_datasets fk_datasets_data_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT fk_datasets_data_type_id FOREIGN KEY (data_type_id) REFERENCES public.tbl_data_types(data_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_datasets fk_datasets_master_set_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT fk_datasets_master_set_id FOREIGN KEY (master_set_id) REFERENCES public.tbl_dataset_masters(master_set_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_datasets fk_datasets_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT fk_datasets_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_datasets fk_datasets_project_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT fk_datasets_project_id FOREIGN KEY (project_id) REFERENCES public.tbl_projects(project_id) DEFERRABLE;


--
-- Name: tbl_datasets fk_datasets_updated_dataset_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_datasets
    ADD CONSTRAINT fk_datasets_updated_dataset_id FOREIGN KEY (updated_dataset_id) REFERENCES public.tbl_datasets(dataset_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_dating_labs fk_dating_labs_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_labs
    ADD CONSTRAINT fk_dating_labs_contact_id FOREIGN KEY (contact_id) REFERENCES public.tbl_contacts(contact_id) DEFERRABLE;


--
-- Name: tbl_dating_material fk_dating_material_abundance_elements_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_material
    ADD CONSTRAINT fk_dating_material_abundance_elements_id FOREIGN KEY (abundance_element_id) REFERENCES public.tbl_abundance_elements(abundance_element_id) DEFERRABLE;


--
-- Name: tbl_dating_material fk_dating_material_geochronology_geochron_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_material
    ADD CONSTRAINT fk_dating_material_geochronology_geochron_id FOREIGN KEY (geochron_id) REFERENCES public.tbl_geochronology(geochron_id) DEFERRABLE;


--
-- Name: tbl_dating_material fk_dating_material_taxa_tree_master_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dating_material
    ADD CONSTRAINT fk_dating_material_taxa_tree_master_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) DEFERRABLE;


--
-- Name: tbl_dendro fk_dendro_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro
    ADD CONSTRAINT fk_dendro_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) DEFERRABLE;


--
-- Name: tbl_dendro_date_notes fk_dendro_date_notes_dendro_date_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_date_notes
    ADD CONSTRAINT fk_dendro_date_notes_dendro_date_id FOREIGN KEY (dendro_date_id) REFERENCES public.tbl_dendro_dates(dendro_date_id) DEFERRABLE;


--
-- Name: tbl_dendro_dates fk_dendro_dates_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_dates
    ADD CONSTRAINT fk_dendro_dates_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) DEFERRABLE;


--
-- Name: tbl_dendro_dates fk_dendro_dates_dating_uncertainty_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_dates
    ADD CONSTRAINT fk_dendro_dates_dating_uncertainty_id FOREIGN KEY (dating_uncertainty_id) REFERENCES public.tbl_dating_uncertainty(dating_uncertainty_id) DEFERRABLE;


--
-- Name: tbl_dendro fk_dendro_dendro_lookup_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro
    ADD CONSTRAINT fk_dendro_dendro_lookup_id FOREIGN KEY (dendro_lookup_id) REFERENCES public.tbl_dendro_lookup(dendro_lookup_id) DEFERRABLE;


--
-- Name: tbl_dendro_dates fk_dendro_lookup_dendro_lookup_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_dates
    ADD CONSTRAINT fk_dendro_lookup_dendro_lookup_id FOREIGN KEY (dendro_lookup_id) REFERENCES public.tbl_dendro_lookup(dendro_lookup_id) DEFERRABLE;


--
-- Name: tbl_dendro_lookup fk_dendro_lookup_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_lookup
    ADD CONSTRAINT fk_dendro_lookup_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_dimensions fk_dimensions_method_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dimensions
    ADD CONSTRAINT fk_dimensions_method_group_id FOREIGN KEY (method_group_id) REFERENCES public.tbl_method_groups(method_group_id) DEFERRABLE;


--
-- Name: tbl_dimensions fk_dimensions_unit_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dimensions
    ADD CONSTRAINT fk_dimensions_unit_id FOREIGN KEY (unit_id) REFERENCES public.tbl_units(unit_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_ecocode_definitions fk_ecocode_definitions_ecocode_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_definitions
    ADD CONSTRAINT fk_ecocode_definitions_ecocode_group_id FOREIGN KEY (ecocode_group_id) REFERENCES public.tbl_ecocode_groups(ecocode_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_ecocode_groups fk_ecocode_groups_ecocode_system_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_groups
    ADD CONSTRAINT fk_ecocode_groups_ecocode_system_id FOREIGN KEY (ecocode_system_id) REFERENCES public.tbl_ecocode_systems(ecocode_system_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_ecocode_systems fk_ecocode_systems_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocode_systems
    ADD CONSTRAINT fk_ecocode_systems_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_ecocodes fk_ecocodes_ecocodedef_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocodes
    ADD CONSTRAINT fk_ecocodes_ecocodedef_id FOREIGN KEY (ecocode_definition_id) REFERENCES public.tbl_ecocode_definitions(ecocode_definition_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_ecocodes fk_ecocodes_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_ecocodes
    ADD CONSTRAINT fk_ecocodes_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_features fk_feature_type_id_feature_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_features
    ADD CONSTRAINT fk_feature_type_id_feature_type_id FOREIGN KEY (feature_type_id) REFERENCES public.tbl_feature_types(feature_type_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_geochron_refs fk_geochron_refs_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochron_refs
    ADD CONSTRAINT fk_geochron_refs_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_geochron_refs fk_geochron_refs_geochron_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochron_refs
    ADD CONSTRAINT fk_geochron_refs_geochron_id FOREIGN KEY (geochron_id) REFERENCES public.tbl_geochronology(geochron_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_geochronology fk_geochronology_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochronology
    ADD CONSTRAINT fk_geochronology_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_geochronology fk_geochronology_dating_labs_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochronology
    ADD CONSTRAINT fk_geochronology_dating_labs_id FOREIGN KEY (dating_lab_id) REFERENCES public.tbl_dating_labs(dating_lab_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_geochronology fk_geochronology_dating_uncertainty_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_geochronology
    ADD CONSTRAINT fk_geochronology_dating_uncertainty_id FOREIGN KEY (dating_uncertainty_id) REFERENCES public.tbl_dating_uncertainty(dating_uncertainty_id) DEFERRABLE;


--
-- Name: tbl_horizons fk_horizons_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_horizons
    ADD CONSTRAINT fk_horizons_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_imported_taxa_replacements fk_imported_taxa_replacements_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_imported_taxa_replacements
    ADD CONSTRAINT fk_imported_taxa_replacements_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_isotope_measurements fk_isotope_isotope_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_measurements
    ADD CONSTRAINT fk_isotope_isotope_type_id FOREIGN KEY (isotope_type_id) REFERENCES public.tbl_isotope_types(isotope_type_id) DEFERRABLE;


--
-- Name: tbl_isotope_measurements fk_isotope_measurements_isotope_standard_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_measurements
    ADD CONSTRAINT fk_isotope_measurements_isotope_standard_id FOREIGN KEY (isotope_standard_id) REFERENCES public.tbl_isotope_standards(isotope_standard_id) DEFERRABLE;


--
-- Name: tbl_isotope_measurements fk_isotope_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotope_measurements
    ADD CONSTRAINT fk_isotope_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_isotopes fk_isotopes_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotopes
    ADD CONSTRAINT fk_isotopes_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) DEFERRABLE;


--
-- Name: tbl_isotopes fk_isotopes_isotope_measurement_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotopes
    ADD CONSTRAINT fk_isotopes_isotope_measurement_id FOREIGN KEY (isotope_measurement_id) REFERENCES public.tbl_isotope_measurements(isotope_measurement_id) DEFERRABLE;


--
-- Name: tbl_isotopes fk_isotopes_isotope_standard_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotopes
    ADD CONSTRAINT fk_isotopes_isotope_standard_id FOREIGN KEY (isotope_standard_id) REFERENCES public.tbl_isotope_standards(isotope_standard_id) DEFERRABLE;


--
-- Name: tbl_isotopes fk_isotopes_isotope_value_specifier_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotopes
    ADD CONSTRAINT fk_isotopes_isotope_value_specifier_id FOREIGN KEY (isotope_value_specifier_id) REFERENCES public.tbl_isotope_value_specifiers(isotope_value_specifier_id) DEFERRABLE;


--
-- Name: tbl_isotopes fk_isotopes_unit_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_isotopes
    ADD CONSTRAINT fk_isotopes_unit_id FOREIGN KEY (unit_id) REFERENCES public.tbl_units(unit_id) DEFERRABLE;


--
-- Name: tbl_lithology fk_lithology_sample_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_lithology
    ADD CONSTRAINT fk_lithology_sample_group_id FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_locations fk_locations_location_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_locations
    ADD CONSTRAINT fk_locations_location_id FOREIGN KEY (location_id) REFERENCES public.tbl_locations(location_id) DEFERRABLE;


--
-- Name: tbl_locations fk_locations_location_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_locations
    ADD CONSTRAINT fk_locations_location_type_id FOREIGN KEY (location_type_id) REFERENCES public.tbl_location_types(location_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_locations fk_locations_site_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_locations
    ADD CONSTRAINT fk_locations_site_id FOREIGN KEY (site_id) REFERENCES public.tbl_sites(site_id) DEFERRABLE;


--
-- Name: tbl_mcr_names fk_mcr_names_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcr_names
    ADD CONSTRAINT fk_mcr_names_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_mcr_summary_data fk_mcr_summary_data_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcr_summary_data
    ADD CONSTRAINT fk_mcr_summary_data_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_mcrdata_birmbeetledat fk_mcrdata_birmbeetledat_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_mcrdata_birmbeetledat
    ADD CONSTRAINT fk_mcrdata_birmbeetledat_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_measured_value_dimensions fk_measured_value_dimensions_dimension_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_measured_value_dimensions
    ADD CONSTRAINT fk_measured_value_dimensions_dimension_id FOREIGN KEY (dimension_id) REFERENCES public.tbl_dimensions(dimension_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_measured_values fk_measured_values_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_measured_values
    ADD CONSTRAINT fk_measured_values_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) DEFERRABLE;


--
-- Name: tbl_measured_value_dimensions fk_measured_weights_value_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_measured_value_dimensions
    ADD CONSTRAINT fk_measured_weights_value_id FOREIGN KEY (measured_value_id) REFERENCES public.tbl_measured_values(measured_value_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_methods fk_methods_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT fk_methods_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_methods fk_methods_method_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT fk_methods_method_group_id FOREIGN KEY (method_group_id) REFERENCES public.tbl_method_groups(method_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_methods fk_methods_record_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT fk_methods_record_type_id FOREIGN KEY (record_type_id) REFERENCES public.tbl_record_types(record_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_methods fk_methods_unit_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_methods
    ADD CONSTRAINT fk_methods_unit_id FOREIGN KEY (unit_id) REFERENCES public.tbl_units(unit_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_physical_sample_features fk_physical_sample_features_feature_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_sample_features
    ADD CONSTRAINT fk_physical_sample_features_feature_id FOREIGN KEY (feature_id) REFERENCES public.tbl_features(feature_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_physical_sample_features fk_physical_sample_features_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_sample_features
    ADD CONSTRAINT fk_physical_sample_features_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_physical_samples fk_physical_samples_sample_name_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_samples
    ADD CONSTRAINT fk_physical_samples_sample_name_type_id FOREIGN KEY (alt_ref_type_id) REFERENCES public.tbl_alt_ref_types(alt_ref_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_physical_samples fk_physical_samples_sample_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_samples
    ADD CONSTRAINT fk_physical_samples_sample_type_id FOREIGN KEY (sample_type_id) REFERENCES public.tbl_sample_types(sample_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_projects fk_projects_project_stage_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_projects
    ADD CONSTRAINT fk_projects_project_stage_id FOREIGN KEY (project_stage_id) REFERENCES public.tbl_project_stages(project_stage_id) DEFERRABLE;


--
-- Name: tbl_projects fk_projects_project_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_projects
    ADD CONSTRAINT fk_projects_project_type_id FOREIGN KEY (project_type_id) REFERENCES public.tbl_project_types(project_type_id) DEFERRABLE;


--
-- Name: tbl_rdb_codes fk_rdb_codes_rdb_system_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_codes
    ADD CONSTRAINT fk_rdb_codes_rdb_system_id FOREIGN KEY (rdb_system_id) REFERENCES public.tbl_rdb_systems(rdb_system_id) DEFERRABLE;


--
-- Name: tbl_rdb fk_rdb_rdb_code_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb
    ADD CONSTRAINT fk_rdb_rdb_code_id FOREIGN KEY (rdb_code_id) REFERENCES public.tbl_rdb_codes(rdb_code_id) DEFERRABLE;


--
-- Name: tbl_rdb_systems fk_rdb_systems_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_systems
    ADD CONSTRAINT fk_rdb_systems_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_rdb_systems fk_rdb_systems_location_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb_systems
    ADD CONSTRAINT fk_rdb_systems_location_id FOREIGN KEY (location_id) REFERENCES public.tbl_locations(location_id) DEFERRABLE;


--
-- Name: tbl_rdb fk_rdb_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb
    ADD CONSTRAINT fk_rdb_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_relative_age_refs fk_relative_age_refs_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_refs
    ADD CONSTRAINT fk_relative_age_refs_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_relative_age_refs fk_relative_age_refs_relative_age_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_age_refs
    ADD CONSTRAINT fk_relative_age_refs_relative_age_id FOREIGN KEY (relative_age_id) REFERENCES public.tbl_relative_ages(relative_age_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_relative_ages fk_relative_ages_location_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_ages
    ADD CONSTRAINT fk_relative_ages_location_id FOREIGN KEY (location_id) REFERENCES public.tbl_locations(location_id) DEFERRABLE;


--
-- Name: tbl_relative_ages fk_relative_ages_relative_age_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_ages
    ADD CONSTRAINT fk_relative_ages_relative_age_type_id FOREIGN KEY (relative_age_type_id) REFERENCES public.tbl_relative_age_types(relative_age_type_id) DEFERRABLE;


--
-- Name: tbl_relative_dates fk_relative_dates_dating_uncertainty_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_dates
    ADD CONSTRAINT fk_relative_dates_dating_uncertainty_id FOREIGN KEY (dating_uncertainty_id) REFERENCES public.tbl_dating_uncertainty(dating_uncertainty_id) DEFERRABLE;


--
-- Name: tbl_relative_dates fk_relative_dates_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_dates
    ADD CONSTRAINT fk_relative_dates_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_relative_dates fk_relative_dates_relative_age_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_dates
    ADD CONSTRAINT fk_relative_dates_relative_age_id FOREIGN KEY (relative_age_id) REFERENCES public.tbl_relative_ages(relative_age_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_alt_refs fk_sample_alt_refs_alt_ref_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_alt_refs
    ADD CONSTRAINT fk_sample_alt_refs_alt_ref_type_id FOREIGN KEY (alt_ref_type_id) REFERENCES public.tbl_alt_ref_types(alt_ref_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_alt_refs fk_sample_alt_refs_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_alt_refs
    ADD CONSTRAINT fk_sample_alt_refs_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_colours fk_sample_colours_colour_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_colours
    ADD CONSTRAINT fk_sample_colours_colour_id FOREIGN KEY (colour_id) REFERENCES public.tbl_colours(colour_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_colours fk_sample_colours_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_colours
    ADD CONSTRAINT fk_sample_colours_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_coordinates fk_sample_coordinates_coordinate_method_dimension_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_coordinates
    ADD CONSTRAINT fk_sample_coordinates_coordinate_method_dimension_id FOREIGN KEY (coordinate_method_dimension_id) REFERENCES public.tbl_coordinate_method_dimensions(coordinate_method_dimension_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_coordinates fk_sample_coordinates_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_coordinates
    ADD CONSTRAINT fk_sample_coordinates_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) DEFERRABLE;


--
-- Name: tbl_sample_description_sample_group_contexts fk_sample_description_sample_group_contexts_sampling_context_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_sample_group_contexts
    ADD CONSTRAINT fk_sample_description_sample_group_contexts_sampling_context_id FOREIGN KEY (sampling_context_id) REFERENCES public.tbl_sample_group_sampling_contexts(sampling_context_id) DEFERRABLE;


--
-- Name: tbl_sample_description_sample_group_contexts fk_sample_description_types_sample_group_context_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_description_sample_group_contexts
    ADD CONSTRAINT fk_sample_description_types_sample_group_context_id FOREIGN KEY (sample_description_type_id) REFERENCES public.tbl_sample_description_types(sample_description_type_id) DEFERRABLE;


--
-- Name: tbl_sample_descriptions fk_sample_descriptions_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_descriptions
    ADD CONSTRAINT fk_sample_descriptions_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) DEFERRABLE;


--
-- Name: tbl_sample_descriptions fk_sample_descriptions_sample_description_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_descriptions
    ADD CONSTRAINT fk_sample_descriptions_sample_description_type_id FOREIGN KEY (sample_description_type_id) REFERENCES public.tbl_sample_description_types(sample_description_type_id) DEFERRABLE;


--
-- Name: tbl_sample_dimensions fk_sample_dimensions_dimension_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_dimensions
    ADD CONSTRAINT fk_sample_dimensions_dimension_id FOREIGN KEY (dimension_id) REFERENCES public.tbl_dimensions(dimension_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_dimensions fk_sample_dimensions_measurement_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_dimensions
    ADD CONSTRAINT fk_sample_dimensions_measurement_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_dimensions fk_sample_dimensions_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_dimensions
    ADD CONSTRAINT fk_sample_dimensions_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_description_type_sampling_contexts fk_sample_group_description_type_sampling_context_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_type_sampling_contexts
    ADD CONSTRAINT fk_sample_group_description_type_sampling_context_id FOREIGN KEY (sample_group_description_type_id) REFERENCES public.tbl_sample_group_description_types(sample_group_description_type_id) DEFERRABLE;


--
-- Name: tbl_sample_group_descriptions fk_sample_group_descriptions_sample_group_description_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_descriptions
    ADD CONSTRAINT fk_sample_group_descriptions_sample_group_description_type_id FOREIGN KEY (sample_group_description_type_id) REFERENCES public.tbl_sample_group_description_types(sample_group_description_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_dimensions fk_sample_group_dimensions_dimension_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_dimensions
    ADD CONSTRAINT fk_sample_group_dimensions_dimension_id FOREIGN KEY (dimension_id) REFERENCES public.tbl_dimensions(dimension_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_dimensions fk_sample_group_dimensions_sample_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_dimensions
    ADD CONSTRAINT fk_sample_group_dimensions_sample_group_id FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_images fk_sample_group_images_image_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_images
    ADD CONSTRAINT fk_sample_group_images_image_type_id FOREIGN KEY (image_type_id) REFERENCES public.tbl_image_types(image_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_images fk_sample_group_images_sample_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_images
    ADD CONSTRAINT fk_sample_group_images_sample_group_id FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) DEFERRABLE;


--
-- Name: tbl_sample_group_coordinates fk_sample_group_positions_coordinate_method_dimension_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_coordinates
    ADD CONSTRAINT fk_sample_group_positions_coordinate_method_dimension_id FOREIGN KEY (coordinate_method_dimension_id) REFERENCES public.tbl_coordinate_method_dimensions(coordinate_method_dimension_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_coordinates fk_sample_group_positions_sample_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_coordinates
    ADD CONSTRAINT fk_sample_group_positions_sample_group_id FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) DEFERRABLE;


--
-- Name: tbl_sample_group_references fk_sample_group_references_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_references
    ADD CONSTRAINT fk_sample_group_references_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_references fk_sample_group_references_sample_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_references
    ADD CONSTRAINT fk_sample_group_references_sample_group_id FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_groups fk_sample_group_sampling_context_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_groups
    ADD CONSTRAINT fk_sample_group_sampling_context_id FOREIGN KEY (sampling_context_id) REFERENCES public.tbl_sample_group_sampling_contexts(sampling_context_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_description_type_sampling_contexts fk_sample_group_sampling_context_id0; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_description_type_sampling_contexts
    ADD CONSTRAINT fk_sample_group_sampling_context_id0 FOREIGN KEY (sampling_context_id) REFERENCES public.tbl_sample_group_sampling_contexts(sampling_context_id) DEFERRABLE;


--
-- Name: tbl_sample_groups fk_sample_groups_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_groups
    ADD CONSTRAINT fk_sample_groups_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_group_descriptions fk_sample_groups_sample_group_descriptions_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_descriptions
    ADD CONSTRAINT fk_sample_groups_sample_group_descriptions_id FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_groups fk_sample_groups_site_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_groups
    ADD CONSTRAINT fk_sample_groups_site_id FOREIGN KEY (site_id) REFERENCES public.tbl_sites(site_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_horizons fk_sample_horizons_horizon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_horizons
    ADD CONSTRAINT fk_sample_horizons_horizon_id FOREIGN KEY (horizon_id) REFERENCES public.tbl_horizons(horizon_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_horizons fk_sample_horizons_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_horizons
    ADD CONSTRAINT fk_sample_horizons_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_images fk_sample_images_image_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_images
    ADD CONSTRAINT fk_sample_images_image_type_id FOREIGN KEY (image_type_id) REFERENCES public.tbl_image_types(image_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_images fk_sample_images_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_images
    ADD CONSTRAINT fk_sample_images_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sample_location_type_sampling_contexts fk_sample_location_sampling_contexts_sampling_context_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_type_sampling_contexts
    ADD CONSTRAINT fk_sample_location_sampling_contexts_sampling_context_id FOREIGN KEY (sample_location_type_id) REFERENCES public.tbl_sample_location_types(sample_location_type_id) DEFERRABLE;


--
-- Name: tbl_sample_location_type_sampling_contexts fk_sample_location_type_sampling_context_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_location_type_sampling_contexts
    ADD CONSTRAINT fk_sample_location_type_sampling_context_id FOREIGN KEY (sampling_context_id) REFERENCES public.tbl_sample_group_sampling_contexts(sampling_context_id) DEFERRABLE;


--
-- Name: tbl_sample_locations fk_sample_locations_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_locations
    ADD CONSTRAINT fk_sample_locations_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) DEFERRABLE;


--
-- Name: tbl_sample_locations fk_sample_locations_sample_location_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_locations
    ADD CONSTRAINT fk_sample_locations_sample_location_type_id FOREIGN KEY (sample_location_type_id) REFERENCES public.tbl_sample_location_types(sample_location_type_id) DEFERRABLE;


--
-- Name: tbl_sample_notes fk_sample_notes_physical_sample_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_notes
    ADD CONSTRAINT fk_sample_notes_physical_sample_id FOREIGN KEY (physical_sample_id) REFERENCES public.tbl_physical_samples(physical_sample_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_physical_samples fk_samples_sample_group_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_physical_samples
    ADD CONSTRAINT fk_samples_sample_group_id FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_seasons fk_seasons_season_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_seasons
    ADD CONSTRAINT fk_seasons_season_type_id FOREIGN KEY (season_type_id) REFERENCES public.tbl_season_types(season_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_images fk_site_images_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_images
    ADD CONSTRAINT fk_site_images_contact_id FOREIGN KEY (contact_id) REFERENCES public.tbl_contacts(contact_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_images fk_site_images_image_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_images
    ADD CONSTRAINT fk_site_images_image_type_id FOREIGN KEY (image_type_id) REFERENCES public.tbl_image_types(image_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_images fk_site_images_site_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_images
    ADD CONSTRAINT fk_site_images_site_id FOREIGN KEY (site_id) REFERENCES public.tbl_sites(site_id) DEFERRABLE;


--
-- Name: tbl_site_natgridrefs fk_site_natgridrefs_method_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_natgridrefs
    ADD CONSTRAINT fk_site_natgridrefs_method_id FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_site_natgridrefs fk_site_natgridrefs_sites_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_natgridrefs
    ADD CONSTRAINT fk_site_natgridrefs_sites_id FOREIGN KEY (site_id) REFERENCES public.tbl_sites(site_id) DEFERRABLE;


--
-- Name: tbl_site_other_records fk_site_other_records_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_other_records
    ADD CONSTRAINT fk_site_other_records_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_other_records fk_site_other_records_record_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_other_records
    ADD CONSTRAINT fk_site_other_records_record_type_id FOREIGN KEY (record_type_id) REFERENCES public.tbl_record_types(record_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_other_records fk_site_other_records_site_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_other_records
    ADD CONSTRAINT fk_site_other_records_site_id FOREIGN KEY (site_id) REFERENCES public.tbl_sites(site_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_preservation_status fk_site_preservation_status_site_id ; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_preservation_status
    ADD CONSTRAINT "fk_site_preservation_status_site_id " FOREIGN KEY (site_id) REFERENCES public.tbl_sites(site_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_sites fk_site_preservation_status_site_preservation_status_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sites
    ADD CONSTRAINT fk_site_preservation_status_site_preservation_status_id FOREIGN KEY (site_preservation_status_id) REFERENCES public.tbl_site_preservation_status(site_preservation_status_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_references fk_site_references_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_references
    ADD CONSTRAINT fk_site_references_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_site_references fk_site_references_site_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_site_references
    ADD CONSTRAINT fk_site_references_site_id FOREIGN KEY (site_id) REFERENCES public.tbl_sites(site_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_species_associations fk_species_associations_associated_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_associations
    ADD CONSTRAINT fk_species_associations_associated_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_species_associations fk_species_associations_association_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_associations
    ADD CONSTRAINT fk_species_associations_association_type_id FOREIGN KEY (association_type_id) REFERENCES public.tbl_species_association_types(association_type_id) DEFERRABLE;


--
-- Name: tbl_species_associations fk_species_associations_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_associations
    ADD CONSTRAINT fk_species_associations_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_species_associations fk_species_associations_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_species_associations
    ADD CONSTRAINT fk_species_associations_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) DEFERRABLE;


--
-- Name: tbl_taxa_common_names fk_taxa_common_names_language_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_common_names
    ADD CONSTRAINT fk_taxa_common_names_language_id FOREIGN KEY (language_id) REFERENCES public.tbl_languages(language_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_common_names fk_taxa_common_names_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_common_names
    ADD CONSTRAINT fk_taxa_common_names_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_images fk_taxa_images_image_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_images
    ADD CONSTRAINT fk_taxa_images_image_type_id FOREIGN KEY (image_type_id) REFERENCES public.tbl_image_types(image_type_id) DEFERRABLE;


--
-- Name: tbl_taxa_images fk_taxa_images_taxa_tree_master_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_images
    ADD CONSTRAINT fk_taxa_images_taxa_tree_master_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) DEFERRABLE;


--
-- Name: tbl_taxa_measured_attributes fk_taxa_measured_attributes_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_measured_attributes
    ADD CONSTRAINT fk_taxa_measured_attributes_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_reference_specimens fk_taxa_reference_specimens_contact_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_reference_specimens
    ADD CONSTRAINT fk_taxa_reference_specimens_contact_id FOREIGN KEY (contact_id) REFERENCES public.tbl_contacts(contact_id) DEFERRABLE;


--
-- Name: tbl_taxa_reference_specimens fk_taxa_reference_specimens_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_reference_specimens
    ADD CONSTRAINT fk_taxa_reference_specimens_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) DEFERRABLE;


--
-- Name: tbl_taxa_seasonality fk_taxa_seasonality_activity_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_seasonality
    ADD CONSTRAINT fk_taxa_seasonality_activity_type_id FOREIGN KEY (activity_type_id) REFERENCES public.tbl_activity_types(activity_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_seasonality fk_taxa_seasonality_location_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_seasonality
    ADD CONSTRAINT fk_taxa_seasonality_location_id FOREIGN KEY (location_id) REFERENCES public.tbl_locations(location_id) DEFERRABLE;


--
-- Name: tbl_taxa_seasonality fk_taxa_seasonality_season_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_seasonality
    ADD CONSTRAINT fk_taxa_seasonality_season_id FOREIGN KEY (season_id) REFERENCES public.tbl_seasons(season_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_seasonality fk_taxa_seasonality_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_seasonality
    ADD CONSTRAINT fk_taxa_seasonality_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_synonyms fk_taxa_synonyms_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms
    ADD CONSTRAINT fk_taxa_synonyms_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_synonyms fk_taxa_synonyms_family_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms
    ADD CONSTRAINT fk_taxa_synonyms_family_id FOREIGN KEY (family_id) REFERENCES public.tbl_taxa_tree_families(family_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_synonyms fk_taxa_synonyms_genus_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms
    ADD CONSTRAINT fk_taxa_synonyms_genus_id FOREIGN KEY (genus_id) REFERENCES public.tbl_taxa_tree_genera(genus_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_synonyms fk_taxa_synonyms_taxa_tree_author_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms
    ADD CONSTRAINT fk_taxa_synonyms_taxa_tree_author_id FOREIGN KEY (author_id) REFERENCES public.tbl_taxa_tree_authors(author_id) DEFERRABLE;


--
-- Name: tbl_taxa_synonyms fk_taxa_synonyms_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_synonyms
    ADD CONSTRAINT fk_taxa_synonyms_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_tree_families fk_taxa_tree_families_order_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_families
    ADD CONSTRAINT fk_taxa_tree_families_order_id FOREIGN KEY (order_id) REFERENCES public.tbl_taxa_tree_orders(order_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_tree_genera fk_taxa_tree_genera_family_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_genera
    ADD CONSTRAINT fk_taxa_tree_genera_family_id FOREIGN KEY (family_id) REFERENCES public.tbl_taxa_tree_families(family_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_tree_master fk_taxa_tree_master_author_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_master
    ADD CONSTRAINT fk_taxa_tree_master_author_id FOREIGN KEY (author_id) REFERENCES public.tbl_taxa_tree_authors(author_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_tree_master fk_taxa_tree_master_genus_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_master
    ADD CONSTRAINT fk_taxa_tree_master_genus_id FOREIGN KEY (genus_id) REFERENCES public.tbl_taxa_tree_genera(genus_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_taxa_tree_orders fk_taxa_tree_orders_record_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxa_tree_orders
    ADD CONSTRAINT fk_taxa_tree_orders_record_type_id FOREIGN KEY (record_type_id) REFERENCES public.tbl_record_types(record_type_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxonomic_order_biblio fk_taxonomic_order_biblio_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order_biblio
    ADD CONSTRAINT fk_taxonomic_order_biblio_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxonomic_order_biblio fk_taxonomic_order_biblio_taxonomic_order_system_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order_biblio
    ADD CONSTRAINT fk_taxonomic_order_biblio_taxonomic_order_system_id FOREIGN KEY (taxonomic_order_system_id) REFERENCES public.tbl_taxonomic_order_systems(taxonomic_order_system_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxonomic_order fk_taxonomic_order_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order
    ADD CONSTRAINT fk_taxonomic_order_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_taxonomic_order fk_taxonomic_order_taxonomic_order_system_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomic_order
    ADD CONSTRAINT fk_taxonomic_order_taxonomic_order_system_id FOREIGN KEY (taxonomic_order_system_id) REFERENCES public.tbl_taxonomic_order_systems(taxonomic_order_system_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxonomy_notes fk_taxonomy_notes_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomy_notes
    ADD CONSTRAINT fk_taxonomy_notes_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_taxonomy_notes fk_taxonomy_notes_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_taxonomy_notes
    ADD CONSTRAINT fk_taxonomy_notes_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_dendro_dates fk_tbl_age_types_age_type_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dendro_dates
    ADD CONSTRAINT fk_tbl_age_types_age_type_id FOREIGN KEY (age_type_id) REFERENCES public.tbl_age_types(age_type_id) DEFERRABLE;


--
-- Name: tbl_dataset_methods fk_tbl_dataset_methods_to_tbl_datasets; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_methods
    ADD CONSTRAINT fk_tbl_dataset_methods_to_tbl_datasets FOREIGN KEY (dataset_id) REFERENCES public.tbl_datasets(dataset_id) DEFERRABLE;


--
-- Name: tbl_dataset_methods fk_tbl_dataset_methods_to_tbl_methods; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_dataset_methods
    ADD CONSTRAINT fk_tbl_dataset_methods_to_tbl_methods FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_rdb fk_tbl_rdb_tbl_location_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_rdb
    ADD CONSTRAINT fk_tbl_rdb_tbl_location_id FOREIGN KEY (location_id) REFERENCES public.tbl_locations(location_id) DEFERRABLE;


--
-- Name: tbl_relative_dates fk_tbl_relative_dates_to_tbl_analysis_entities; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_relative_dates
    ADD CONSTRAINT fk_tbl_relative_dates_to_tbl_analysis_entities FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) DEFERRABLE;


--
-- Name: tbl_sample_group_notes fk_tbl_sample_group_notes_sample_groups; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_notes
    ADD CONSTRAINT fk_tbl_sample_group_notes_sample_groups FOREIGN KEY (sample_group_id) REFERENCES public.tbl_sample_groups(sample_group_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_tephra_dates fk_tephra_dates_analysis_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_dates
    ADD CONSTRAINT fk_tephra_dates_analysis_entity_id FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_tephra_dates fk_tephra_dates_dating_uncertainty_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_dates
    ADD CONSTRAINT fk_tephra_dates_dating_uncertainty_id FOREIGN KEY (dating_uncertainty_id) REFERENCES public.tbl_dating_uncertainty(dating_uncertainty_id) DEFERRABLE;


--
-- Name: tbl_tephra_dates fk_tephra_dates_tephra_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_dates
    ADD CONSTRAINT fk_tephra_dates_tephra_id FOREIGN KEY (tephra_id) REFERENCES public.tbl_tephras(tephra_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_tephra_refs fk_tephra_refs_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_refs
    ADD CONSTRAINT fk_tephra_refs_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_tephra_refs fk_tephra_refs_tephra_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_tephra_refs
    ADD CONSTRAINT fk_tephra_refs_tephra_id FOREIGN KEY (tephra_id) REFERENCES public.tbl_tephras(tephra_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_text_biology fk_text_biology_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_biology
    ADD CONSTRAINT fk_text_biology_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_text_biology fk_text_biology_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_biology
    ADD CONSTRAINT fk_text_biology_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_text_distribution fk_text_distribution_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_distribution
    ADD CONSTRAINT fk_text_distribution_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_text_distribution fk_text_distribution_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_distribution
    ADD CONSTRAINT fk_text_distribution_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_text_identification_keys fk_text_identification_keys_biblio_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_identification_keys
    ADD CONSTRAINT fk_text_identification_keys_biblio_id FOREIGN KEY (biblio_id) REFERENCES public.tbl_biblio(biblio_id) ON UPDATE CASCADE DEFERRABLE;


--
-- Name: tbl_text_identification_keys fk_text_identification_keys_taxon_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_text_identification_keys
    ADD CONSTRAINT fk_text_identification_keys_taxon_id FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) ON UPDATE CASCADE ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_boolean_values tbl_analysis_boolean_values_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_boolean_values
    ADD CONSTRAINT tbl_analysis_boolean_values_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_categorical_values tbl_analysis_categorical_values_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_categorical_values
    ADD CONSTRAINT tbl_analysis_categorical_values_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_categorical_values tbl_analysis_categorical_values_value_type_item_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_categorical_values
    ADD CONSTRAINT tbl_analysis_categorical_values_value_type_item_id_fkey FOREIGN KEY (value_type_item_id) REFERENCES public.tbl_value_type_items(value_type_item_id) DEFERRABLE;


--
-- Name: tbl_analysis_dating_ranges tbl_analysis_dating_ranges_age_type_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges
    ADD CONSTRAINT tbl_analysis_dating_ranges_age_type_id_fkey FOREIGN KEY (age_type_id) REFERENCES public.tbl_age_types(age_type_id) DEFERRABLE;


--
-- Name: tbl_analysis_dating_ranges tbl_analysis_dating_ranges_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges
    ADD CONSTRAINT tbl_analysis_dating_ranges_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_dating_ranges tbl_analysis_dating_ranges_dating_uncertainty_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges
    ADD CONSTRAINT tbl_analysis_dating_ranges_dating_uncertainty_id_fkey FOREIGN KEY (dating_uncertainty_id) REFERENCES public.tbl_dating_uncertainty(dating_uncertainty_id) DEFERRABLE;


--
-- Name: tbl_analysis_dating_ranges tbl_analysis_dating_ranges_high_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges
    ADD CONSTRAINT tbl_analysis_dating_ranges_high_qualifier_fkey FOREIGN KEY (high_qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_dating_ranges tbl_analysis_dating_ranges_low_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges
    ADD CONSTRAINT tbl_analysis_dating_ranges_low_qualifier_fkey FOREIGN KEY (low_qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_dating_ranges tbl_analysis_dating_ranges_season_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_dating_ranges
    ADD CONSTRAINT tbl_analysis_dating_ranges_season_id_fkey FOREIGN KEY (season_id) REFERENCES public.tbl_seasons(season_id) DEFERRABLE;


--
-- Name: tbl_analysis_identifiers tbl_analysis_identifiers_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_identifiers
    ADD CONSTRAINT tbl_analysis_identifiers_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_integer_ranges tbl_analysis_integer_ranges_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_ranges
    ADD CONSTRAINT tbl_analysis_integer_ranges_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_integer_ranges tbl_analysis_integer_ranges_high_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_ranges
    ADD CONSTRAINT tbl_analysis_integer_ranges_high_qualifier_fkey FOREIGN KEY (high_qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_integer_ranges tbl_analysis_integer_ranges_low_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_ranges
    ADD CONSTRAINT tbl_analysis_integer_ranges_low_qualifier_fkey FOREIGN KEY (low_qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_integer_values tbl_analysis_integer_values_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_values
    ADD CONSTRAINT tbl_analysis_integer_values_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_integer_values tbl_analysis_integer_values_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_integer_values
    ADD CONSTRAINT tbl_analysis_integer_values_qualifier_fkey FOREIGN KEY (qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_notes tbl_analysis_notes_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_notes
    ADD CONSTRAINT tbl_analysis_notes_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_numerical_ranges tbl_analysis_numerical_ranges_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_ranges
    ADD CONSTRAINT tbl_analysis_numerical_ranges_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_numerical_ranges tbl_analysis_numerical_ranges_high_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_ranges
    ADD CONSTRAINT tbl_analysis_numerical_ranges_high_qualifier_fkey FOREIGN KEY (high_qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_numerical_ranges tbl_analysis_numerical_ranges_low_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_ranges
    ADD CONSTRAINT tbl_analysis_numerical_ranges_low_qualifier_fkey FOREIGN KEY (low_qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_numerical_values tbl_analysis_numerical_values_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_values
    ADD CONSTRAINT tbl_analysis_numerical_values_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_numerical_values tbl_analysis_numerical_values_qualifier_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_numerical_values
    ADD CONSTRAINT tbl_analysis_numerical_values_qualifier_fkey FOREIGN KEY (qualifier) REFERENCES public.tbl_value_qualifier_symbols(symbol) DEFERRABLE;


--
-- Name: tbl_analysis_taxon_counts tbl_analysis_taxon_counts_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_taxon_counts
    ADD CONSTRAINT tbl_analysis_taxon_counts_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_taxon_counts tbl_analysis_taxon_counts_taxon_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_taxon_counts
    ADD CONSTRAINT tbl_analysis_taxon_counts_taxon_id_fkey FOREIGN KEY (taxon_id) REFERENCES public.tbl_taxa_tree_master(taxon_id) DEFERRABLE;


--
-- Name: tbl_analysis_value_dimensions tbl_analysis_value_dimensions_analysis_value_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_value_dimensions
    ADD CONSTRAINT tbl_analysis_value_dimensions_analysis_value_id_fkey FOREIGN KEY (analysis_value_id) REFERENCES public.tbl_analysis_values(analysis_value_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_value_dimensions tbl_analysis_value_dimensions_dimension_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_value_dimensions
    ADD CONSTRAINT tbl_analysis_value_dimensions_dimension_id_fkey FOREIGN KEY (dimension_id) REFERENCES public.tbl_dimensions(dimension_id) DEFERRABLE;


--
-- Name: tbl_analysis_values tbl_analysis_values_analysis_entity_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_values
    ADD CONSTRAINT tbl_analysis_values_analysis_entity_id_fkey FOREIGN KEY (analysis_entity_id) REFERENCES public.tbl_analysis_entities(analysis_entity_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_analysis_values tbl_analysis_values_value_class_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_analysis_values
    ADD CONSTRAINT tbl_analysis_values_value_class_id_fkey FOREIGN KEY (value_class_id) REFERENCES public.tbl_value_classes(value_class_id) DEFERRABLE;


--
-- Name: tbl_sample_dimensions tbl_sample_dimensions_qualifier_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_dimensions
    ADD CONSTRAINT tbl_sample_dimensions_qualifier_id_fkey FOREIGN KEY (qualifier_id) REFERENCES public.tbl_value_qualifiers(qualifier_id) DEFERRABLE;


--
-- Name: tbl_sample_group_dimensions tbl_sample_group_dimensions_qualifier_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_sample_group_dimensions
    ADD CONSTRAINT tbl_sample_group_dimensions_qualifier_id_fkey FOREIGN KEY (qualifier_id) REFERENCES public.tbl_value_qualifiers(qualifier_id) DEFERRABLE;


--
-- Name: tbl_value_classes tbl_value_classes_method_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_classes
    ADD CONSTRAINT tbl_value_classes_method_id_fkey FOREIGN KEY (method_id) REFERENCES public.tbl_methods(method_id) DEFERRABLE;


--
-- Name: tbl_value_classes tbl_value_classes_parent_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_classes
    ADD CONSTRAINT tbl_value_classes_parent_id_fkey FOREIGN KEY (parent_id) REFERENCES public.tbl_value_classes(value_class_id) DEFERRABLE;


--
-- Name: tbl_value_classes tbl_value_classes_value_type_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_classes
    ADD CONSTRAINT tbl_value_classes_value_type_id_fkey FOREIGN KEY (value_type_id) REFERENCES public.tbl_value_types(value_type_id) DEFERRABLE;


--
-- Name: tbl_value_qualifier_symbols tbl_value_qualifier_symbols_cardinal_qualifier_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_qualifier_symbols
    ADD CONSTRAINT tbl_value_qualifier_symbols_cardinal_qualifier_id_fkey FOREIGN KEY (cardinal_qualifier_id) REFERENCES public.tbl_value_qualifiers(qualifier_id) DEFERRABLE;


--
-- Name: tbl_value_type_items tbl_value_type_items_value_type_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_type_items
    ADD CONSTRAINT tbl_value_type_items_value_type_id_fkey FOREIGN KEY (value_type_id) REFERENCES public.tbl_value_types(value_type_id) ON DELETE CASCADE DEFERRABLE;


--
-- Name: tbl_value_types tbl_value_types_data_type_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_types
    ADD CONSTRAINT tbl_value_types_data_type_id_fkey FOREIGN KEY (data_type_id) REFERENCES public.tbl_data_types(data_type_id) DEFERRABLE;


--
-- Name: tbl_value_types tbl_value_types_unit_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tbl_value_types
    ADD CONSTRAINT tbl_value_types_unit_id_fkey FOREIGN KEY (unit_id) REFERENCES public.tbl_units(unit_id) DEFERRABLE;


--
-- PostgreSQL database dump complete
--

