#!/bin/bash

source .env

create_database_flag=NO
create_users_flag=NO
create_new_snapshot_flag=NO
create_sqlite_schema_flag=NO

snapshot_filename="${SNAPSHOT_DBNAME}_devdocker.sql.gz"
generate_sqlite_schema_filename="generate_sead_sqlite_model.sql"
sqlite_model_filename="sead_staging_sqlite_schema.sql"

if [ ! -f "$snapshot_filename" ]; then
    create_new_snapshot=YES
fi

usage_message=$(cat <<EOF
usage: create_dev_db.sh OPTIONS...

    --create-database       Create new database (drops existing)
    --create-sqlite-schema  Write Sqlite schema to file
    --create-users          Create database users
    --create-new-snapshot   Create new snapshot of source staging database.

EOF
)

POSITIONAL=()
while [[ $# -gt 0 ]]
do
    key="$1"
    case $key in
        --host)
            dbhost="$2"; shift; shift
        ;;
        --create-database)
            create_database_flag="YES"
            create_sqlite_schema_flag="YES"
            shift
        ;;
        --create-users)
            create_users_flag="YES"; shift;
        ;;
        --create-new-snapshot)
            create_new_snapshot_flag="YES"; shift;
        ;;
        --create-sqlite-schema)
            create_sqlite_schema_flag="YES"; shift;
        ;;
        *)
            POSITIONAL+=("$1");  shift
        ;;
    esac
done

function dev_exec_sql() {
    dbhost=$1
    dbname=$2
    sql=$3
    echo $sql
    export PGPASSWORD=${DEV_PASSWORD}
    psql -v ON_ERROR_STOP=1 --quiet --tuples-only --host=$dbhost --username=$DEV_USER --no-password --dbname=$dbname --command "$sql"
    if [ $? -ne 0 ];  then
        echo "FATAL: psql command failed! Deploy aborted." >&2
        exit 64
    fi
}

function dev_exec_gz() {
    dbhost=$1
    dbname=$2
    gz_file=$3
    echo "Executing file $gz_file..."
    export PGPASSWORD=${DEV_PASSWORD}
    zcat $gz_file | psql -v ON_ERROR_STOP=1 --quiet --tuples-only --host=$dbhost --username=$DEV_USER --no-password --dbname=$dbname
    if [ $? -ne 0 ];  then
        echo "FATAL: psql command failed! Deploy aborted." >&2
        exit 64
    fi
}

function kick_out_users() {
    echo "Kicking out users..."
    sql=$(cat <<____EOF
        select pg_terminate_backend(pg_stat_activity.pid)
        from pg_stat_activity
        where pg_stat_activity.datname in ('${DEV_DBNAME}')
          and pid <> pg_backend_pid();
____EOF
    )
    dev_exec_sql "$DEV_HOST" "postgres" "$sql"
}

function re_create_database() {
    echo "Dropping database..."
    sql="drop database $DEV_DBNAME;"
    dev_exec_sql "$DEV_HOST" "postgres" "$sql"
    sql="create database $DEV_DBNAME;"
    dev_exec_sql "$DEV_HOST" "postgres" "$sql"
}

function dump_snapshot_database() {
    echo "Dumping snapshot..."
    export PGPASSWORD=${SNAPSHOT_PASSWORD}
    pg_dump --clean --create --dbname=$SNAPSHOT_DBNAME --schema=public --schema=facet --schema=sead_utility \
        --host=$SNAPSHOT_HOST --format=p --compress=9 --username=$SNAPSHOT_USER --file=$snapshot_filename
    unset PGPASSWORD
}

# function dump_compressed_snapshot_database() {
#     echo "Dumping snapshot..."
#     export PGPASSWORD=${SNAPSHOT_PASSWORD}
#     pg_dump --clean --create --dbname=$SNAPSHOT_DBNAME --schema=public --schema=facet --schema=sead_utility \
#              --host=$SNAPSHOT_HOST --format=c --username=$SNAPSHOT_USER --compress=5 \
#              --no-comments --no-sync --no-tablespaces \
#              --file=$snapshot_compressed_filename
#     unset PGPASSWORD
# }

function create_dev_database() {
    echo "Loading snapshot..."
    export PGPASSWORD=${DEV_PASSWORD}
    zcat $snapshot_filename | psql --quiet --host=$DEV_HOST --username=$DEV_USER --no-password --dbname=postgres
    unset PGPASSWORD
}

function create_sqlight_schema() {
    export PGPASSWORD=${DEV_PASSWORD}
    cat $generate_sqlite_schema_filename | psql --host=$DEV_HOST --username=$DEV_USER --no-password --dbname=$SNAPSHOT_DBNAME
    sql="select * from fn_script_to_sqlite_tables(FALSE);"
    psql -v ON_ERROR_STOP=1 --quiet --tuples-only --no-align --host=$DEV_HOST --username=$DEV_USER --no-password --dbname=$SNAPSHOT_DBNAME --command "$sql" -o $sqlite_model_filename
    unset PGPASSWORD
}

function create_user() {
    arg_user=$1
    arg_opts=$2
    sql=$(cat <<____EOF
    DO \$\$ BEGIN
       CREATE ROLE ${arg_user} WITH ${arg_opts};
         EXCEPTION WHEN DUPLICATE_OBJECT THEN RAISE NOTICE 'skipping ${arg_user} (exists)';
    END \$\$ ;
____EOF
    )
    PGPASSWORD=${POSTGRES_PASSWORD}
    dev_exec_sql "$DEV_HOST" "postgres" "$sql" > /dev/null
    unset PGPASSWORD
}


function create_users() {

    echo "Creating users..."

    create_user "anonymous_rest_user" "NOLOGIN"
    create_user "sead_read" "LOGIN"
    create_user "sead_write" "LOGIN"

    create_user "humlab_read" "NOINHERIT LOGIN PASSWORD '${DEV_PASSWORD}'"
    create_user "querysead_worker" "LOGIN PASSWORD '${DEV_PASSWORD}'"
    create_user "sead_user_api" "LOGIN PASSWORD '${DEV_PASSWORD}'"

    create_user "clearinghouse_worker" "CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}'"
    create_user "querysead_owner" "LOGIN PASSWORD '${DEV_PASSWORD}' VALID UNTIL 'infinity'"

    create_user "humlab_admin" "SUPERUSER CREATEROLE CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}'"
    create_user "postgres" "SUPERUSER CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS"

    create_user "sead_master" "CREATEROLE CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}'"

    create_user "seadwrite" "SUPERUSER CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}'"
    create_user "sead" "NOLOGIN VALID UNTIL 'infinity'"

    dev_exec_sql "$DEV_HOST" "postgres" "GRANT querysead_owner TO humlab_admin GRANTED BY humlab_admin;"
    dev_exec_sql "$DEV_HOST" "postgres" "GRANT sead_read TO sead_master GRANTED BY seadwrite;"

    unset PGPASSWORD
}

if [ "$create_users_flag" == "YES" ]; then
    create_users
fi

if [ "$create_new_snapshot_flag" == "YES" ]; then
    dump_snapshot_database
    #dump_compressed_snapshot_database
fi

if [ "$create_database_flag" == "YES" ]; then
    kick_out_users
    re_create_database
    create_dev_database
    create_sqlight_schema
fi

if [ "$create_sqlite_schema_flag" == "YES" ]; then
    create_sqlight_schema
fi
