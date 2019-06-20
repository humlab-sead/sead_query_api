
DO $$
BEGIN

    IF CURRENT_USER <> 'humlab_admin' THEN
        RAISE EXCEPTION '00_script_error: script must be run as humlab_admin';
    END IF;

    IF CURRENT_DATABASE() <> 'sead_staging' THEN
        RAISE EXCEPTION '00_script_error: script must be run on sead_staging';
    END IF;

    IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'querysead_owner') THEN

        CREATE USER querysead_owner
            WITH LOGIN NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION VALID UNTIL 'infinity';

    END IF;

    IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'querysead_worker') THEN

        CREATE USER querysead_worker
            WITH LOGIN NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION VALID UNTIL 'infinity';

    END IF;

    GRANT CONNECT ON DATABASE sead_staging TO querysead_worker, querysead_owner;
    GRANT USAGE ON SCHEMA public, metainformation TO querysead_owner, querysead_worker, sead_read, sead_write;

    REVOKE ALL ON ALL TABLES IN SCHEMA public, metainformation FROM querysead_owner, querysead_worker;

    GRANT SELECT ON ALL TABLES IN SCHEMA public, metainformation TO querysead_owner, querysead_worker;
    GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA public, metainformation TO querysead_owner, querysead_worker;
    GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public, metainformation TO querysead_owner, querysead_worker;

    ALTER DEFAULT PRIVILEGES IN SCHEMA public, metainformation GRANT SELECT, TRIGGER ON TABLES TO querysead_owner, querysead_worker;

    DROP SCHEMA IF EXISTS facet CASCADE;

    CREATE SCHEMA IF NOT EXISTS facet AUTHORIZATION querysead_owner;

    ALTER DEFAULT PRIVILEGES IN SCHEMA facet GRANT SELECT ON TABLES TO public, querysead_worker, sead_read, sead_write;
    ALTER DEFAULT PRIVILEGES IN SCHEMA facet GRANT SELECT, USAGE ON SEQUENCES TO public, querysead_worker, sead_read, sead_write;
    ALTER DEFAULT PRIVILEGES IN SCHEMA facet GRANT EXECUTE ON FUNCTIONS TO public, querysead_worker, sead_read, sead_write;

END $$ LANGUAGE plpgsql;

-- ALTER USER querysead_owner WITH ENCRYPTED PASSWORD 'XXX';
-- ALTER USER querysead_worker WITH ENCRYPTED PASSWORD 'XXX';
