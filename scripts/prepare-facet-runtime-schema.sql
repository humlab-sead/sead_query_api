CREATE SCHEMA IF NOT EXISTS facet;

CREATE TABLE IF NOT EXISTS facet.anchor (
    anchor_id integer NOT NULL,
    table_id integer,
    name text NOT NULL,
    description text NOT NULL
);

CREATE SEQUENCE IF NOT EXISTS facet.anchor_anchor_id_seq
AS integer
START WITH 1
INCREMENT BY 1
NO MINVALUE
NO MAXVALUE
CACHE 1;

ALTER SEQUENCE facet.anchor_anchor_id_seq OWNED BY facet.anchor.anchor_id;
ALTER TABLE ONLY facet.anchor ALTER COLUMN anchor_id SET DEFAULT nextval(
    'facet.anchor_anchor_id_seq'::regclass
);

CREATE TABLE IF NOT EXISTS facet.config_revision (
    revision_id integer NOT NULL,
    config_revision character varying(128) NOT NULL,
    source_commit character varying(128) DEFAULT ''::character varying NOT NULL,
    content_hash character varying(128) NOT NULL,
    imported_at timestamp with time zone NOT NULL,
    imported_by character varying(128) NOT NULL,
    is_active boolean DEFAULT false NOT NULL
);

CREATE SEQUENCE IF NOT EXISTS facet.config_revision_revision_id_seq
AS integer
START WITH 1
INCREMENT BY 1
NO MINVALUE
NO MAXVALUE
CACHE 1;

ALTER SEQUENCE facet.config_revision_revision_id_seq OWNED BY facet.config_revision.revision_id;
ALTER TABLE ONLY facet.config_revision ALTER COLUMN revision_id SET DEFAULT nextval(
    'facet.config_revision_revision_id_seq'::regclass
);

CREATE TABLE IF NOT EXISTS facet.route (
    route_id integer NOT NULL,
    route_name text NOT NULL,
    source_table_id integer NOT NULL,
    target_table_id integer NOT NULL,
    specification text NOT NULL,
    route_alias text
);

CREATE SEQUENCE IF NOT EXISTS facet.route_route_id_seq
AS integer
START WITH 1
INCREMENT BY 1
NO MINVALUE
NO MAXVALUE
CACHE 1;

ALTER SEQUENCE facet.route_route_id_seq OWNED BY facet.route.route_id;
ALTER TABLE ONLY facet.route ALTER COLUMN route_id SET DEFAULT nextval(
    'facet.route_route_id_seq'::regclass
);

CREATE TABLE IF NOT EXISTS facet.route_step (
    route_step_id integer NOT NULL,
    route_id integer NOT NULL,
    sequence_id integer NOT NULL,
    table_id integer NOT NULL,
    key_name text NOT NULL
);

CREATE SEQUENCE IF NOT EXISTS facet.route_step_route_step_id_seq
AS integer
START WITH 1
INCREMENT BY 1
NO MINVALUE
NO MAXVALUE
CACHE 1;

ALTER SEQUENCE facet.route_step_route_step_id_seq OWNED BY facet.route_step.route_step_id;
ALTER TABLE ONLY facet.route_step ALTER COLUMN route_step_id SET DEFAULT nextval(
    'facet.route_step_route_step_id_seq'::regclass
);

CREATE TABLE IF NOT EXISTS facet.facet_anchor (
    facet_anchor_id integer NOT NULL,
    facet_id integer,
    anchor_id integer,
    route_id integer NOT NULL
);

CREATE SEQUENCE IF NOT EXISTS facet.facet_anchor_facet_anchor_id_seq
AS integer
START WITH 1
INCREMENT BY 1
NO MINVALUE
NO MAXVALUE
CACHE 1;

ALTER SEQUENCE facet.facet_anchor_facet_anchor_id_seq OWNED BY facet.facet_anchor.facet_anchor_id;
ALTER TABLE ONLY facet.facet_anchor ALTER COLUMN facet_anchor_id SET DEFAULT nextval(
    'facet.facet_anchor_facet_anchor_id_seq'::regclass
);

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'anchor_pkey'
          AND conrelid = 'facet.anchor'::regclass
    ) THEN
        ALTER TABLE ONLY facet.anchor
            ADD CONSTRAINT anchor_pkey PRIMARY KEY (anchor_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'config_revision_config_revision_key'
          AND conrelid = 'facet.config_revision'::regclass
    ) THEN
        ALTER TABLE ONLY facet.config_revision
            ADD CONSTRAINT config_revision_config_revision_key UNIQUE (config_revision);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'config_revision_pkey'
          AND conrelid = 'facet.config_revision'::regclass
    ) THEN
        ALTER TABLE ONLY facet.config_revision
            ADD CONSTRAINT config_revision_pkey PRIMARY KEY (revision_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'facet_anchor_pkey'
          AND conrelid = 'facet.facet_anchor'::regclass
    ) THEN
        ALTER TABLE ONLY facet.facet_anchor
            ADD CONSTRAINT facet_anchor_pkey PRIMARY KEY (facet_anchor_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'route_pkey'
          AND conrelid = 'facet.route'::regclass
    ) THEN
        ALTER TABLE ONLY facet.route
            ADD CONSTRAINT route_pkey PRIMARY KEY (route_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'route_route_name_key'
          AND conrelid = 'facet.route'::regclass
    ) THEN
        ALTER TABLE ONLY facet.route
            ADD CONSTRAINT route_route_name_key UNIQUE (route_name);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'route_step_pkey'
          AND conrelid = 'facet.route_step'::regclass
    ) THEN
        ALTER TABLE ONLY facet.route_step
            ADD CONSTRAINT route_step_pkey PRIMARY KEY (route_step_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'route_step_table_id_key'
          AND conrelid = 'facet.route_step'::regclass
    ) THEN
        ALTER TABLE ONLY facet.route_step
            ADD CONSTRAINT route_step_table_id_key UNIQUE (table_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'anchor_table_id_fkey'
          AND conrelid = 'facet.anchor'::regclass
    ) THEN
        ALTER TABLE ONLY facet.anchor
            ADD CONSTRAINT anchor_table_id_fkey FOREIGN KEY (table_id) REFERENCES facet."table"(table_id) DEFERRABLE;
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'facet_anchor_anchor_id_fkey'
          AND conrelid = 'facet.facet_anchor'::regclass
    ) THEN
        ALTER TABLE ONLY facet.facet_anchor
            ADD CONSTRAINT facet_anchor_anchor_id_fkey FOREIGN KEY (anchor_id) REFERENCES facet.anchor(anchor_id) DEFERRABLE;
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'facet_anchor_facet_id_fkey'
          AND conrelid = 'facet.facet_anchor'::regclass
    ) THEN
        ALTER TABLE ONLY facet.facet_anchor
            ADD CONSTRAINT facet_anchor_facet_id_fkey FOREIGN KEY (facet_id) REFERENCES facet.facet(facet_id) DEFERRABLE;
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'facet_anchor_route_id_fkey'
          AND conrelid = 'facet.facet_anchor'::regclass
    ) THEN
        ALTER TABLE ONLY facet.facet_anchor
            ADD CONSTRAINT facet_anchor_route_id_fkey FOREIGN KEY (route_id) REFERENCES facet.route(route_id) DEFERRABLE;
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'route_source_table_id_fkey'
          AND conrelid = 'facet.route'::regclass
    ) THEN
        ALTER TABLE ONLY facet.route
            ADD CONSTRAINT route_source_table_id_fkey FOREIGN KEY (source_table_id) REFERENCES facet."table"(table_id) DEFERRABLE;
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'route_target_table_id_fkey'
          AND conrelid = 'facet.route'::regclass
    ) THEN
        ALTER TABLE ONLY facet.route
            ADD CONSTRAINT route_target_table_id_fkey FOREIGN KEY (target_table_id) REFERENCES facet."table"(table_id) DEFERRABLE;
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'route_step_route_id_fkey'
          AND conrelid = 'facet.route_step'::regclass
    ) THEN
        ALTER TABLE ONLY facet.route_step
            ADD CONSTRAINT route_step_route_id_fkey FOREIGN KEY (route_id) REFERENCES facet.route(route_id) DEFERRABLE;
    END IF;
END $$;