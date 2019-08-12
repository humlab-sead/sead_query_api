#!/bin/sh

usage() {
	cat <<- EOF
		usage: $0 options

		This script grants read-only privileges to a specified role on all tables, views
		and sequences in a database schema and sets them as default.

		OPTIONS:
		   -h   Show this message
		   -d   Database name
		   -u   Role name
		   -s   Schema (defaults to public)
	EOF
}

pgexec() {
	local cmd=$1
	psql --no-psqlrc --no-align --tuples-only --record-separator=\0 --quiet \
		--echo-queries --command="$cmd" "$db_name"
}


db_name=''
role=''
schema='public'
while getopts 'hd:u:s:' option; do
	case $option in
		h) usage; exit 1;;
		d) db_name=$optarg;;
		u) role=$optarg;;
		s) schema=$optarg;;
	esac
done

if [ -z "$db_name" ] || [ -z "$role" ]; then
	usage
	exit 1
fi
# grant rw to, r/o to querysead_worker, querysead_owner
pgexec "
	grant connect on database $db_name to $role;
	grant usage on schema $schema to $role;
	grant select on all tables in schema $schema to $role;
	grant select on all sequences in schema $schema to $role;
	alter default privileges in schema $schema grant select on tables to $role;
	alter default privileges in schema $schema grant select on sequences to $role;
"

#pgexec "grant execute on all functions in schema $schema to $role;
#alter default privileges in schema $schema grant execute on functions to $role;

    if not exists (select from pg_catalog.pg_roles where rolname = 'querysead_owner') then

        create user querysead_owner
            with login nosuperuser inherit nocreatedb nocreaterole noreplication valid until 'infinity';

    end if;

    if not exists (select from pg_catalog.pg_roles where rolname = 'querysead_worker') then

        create user querysead_worker
            with login nosuperuser inherit nocreatedb nocreaterole noreplication valid until 'infinity';

    end if;

    grant connect on database sead_staging to querysead_worker, querysead_owner;
    grant usage on schema public, metainformation to querysead_owner, querysead_worker, sead_read, sead_write;

    revoke all on all tables in schema public, metainformation from querysead_owner, querysead_worker;

    grant select on all tables in schema public, metainformation to querysead_owner, querysead_worker;
    grant select, usage on all sequences in schema public, metainformation to querysead_owner, querysead_worker;
    grant execute on all functions in schema public, metainformation to querysead_owner, querysead_worker;

    alter default privileges in schema public, metainformation grant select, trigger on tables to querysead_owner, querysead_worker;

    alter default privileges in schema facet grant select on tables to public, querysead_worker, sead_read, sead_write;
    alter default privileges in schema facet grant select, usage on sequences to public, querysead_worker, sead_read, sead_write;
    alter default privileges in schema facet grant execute on functions to public, querysead_worker, sead_read, sead_write;

-- alter user querysead_owner with encrypted password 'xxx';
-- alter user querysead_worker with encrypted password 'xxx';
