/*
-- Take only schema backup in .sql file: --create --no-owner --column-inserts --inserts --host=host --port=port  --username=username
pg_dump --schema=facet --dbname=sead_master_8 --clean  --encoding=encoding --format=plain --file=sead_master_8.facet.sql
-- Restore schema from backup .sql file:
psql -d db_name -h localhost -U user_name < backupfile.sql
-- Copy or Restore schema backup into another PostgreSQL Server:
pg_dump source_db_name --schema schema_name | psql -h destination_hostname -U distination_user_name -d distination_db_name
pg_dump -Fc -v --host=<host> --username=<name> --dbname=<database name>
*/


CREATE USER querysead_worker WITH LOGIN NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;
ALTER USER querysead_worker WITH ENCRYPTED PASSWORD 'XXX';

GRANT CONNECT ON DATABASE sead_bugs_import_20180503 TO querysead_worker;
GRANT USAGE ON SCHEMA public, metainformation TO querysead_worker;
GRANT sead_read TO querysead_worker;

GRANT SELECT ON ALL TABLES IN SCHEMA  public, metainformation TO querysead_worker;
GRANT SELECT, USAGE ON ALL SEQUENCES IN SCHEMA  public, metainformation to querysead_worker;
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public, metainformation TO querysead_worker;
ALTER DEFAULT PRIVILEGES IN SCHEMA public, metainformation GRANT SELECT, TRIGGER ON TABLES TO querysead_worker;

DROP SCHEMA IF EXISTS facet CASCADE;
	
CREATE SCHEMA IF NOT EXISTS facet AUTHORIZATION querysead_worker;
