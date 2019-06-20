-- Kill active connections
SELECT pg_terminate_backend(pid)
FROM pg_stat_activity
WHERE datname='sead_production';

DO $$
BEGIN
    IF current_database() != 'postgres' THEN
        RAISE EXCEPTION 'This script must be run in postgres DB!';
    END IF;
END $$;

-- This must be run as a standalone SQL statement
CREATE DATABASE sead_staging
  WITH OWNER = sead_master
       TEMPLATE = sead_production
       ENCODING = 'UTF8'
       LC_COLLATE = 'en_US.UTF-8'
       LC_CTYPE = 'en_US.UTF-8'
       TABLESPACE = pg_default;

GRANT TEMPORARY, CONNECT ON DATABASE sead_staging TO PUBLIC;
GRANT ALL ON DATABASE sead_staging TO sead_master;