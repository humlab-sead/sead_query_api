#!/bin/bash

source .env

create_database_flag=NO
create_users_flag=NO
create_new_snapshot_flag=NO

snapshot_filename="${SNAPSHOT_DBNAME}_devdocker.sql.gz"

if [ ! -f "$snapshot_filename" ]; then
    create_new_snapshot=YES
fi

usage_message=$(cat <<EOF
usage: create_dev_db.sh OPTIONS...

    --create-database       Create new database (drops existing)
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
            create_database_flag="YES"; shift;
        ;;
        --create-users)
            create_users_flag="YES"; shift;
        ;;
        --create-new-snapshot)
            create_new_snapshot_flag="YES"; shift;
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
    psql -v ON_ERROR_STOP=1 --host=$dbhost --username=$DEV_USER --no-password --dbname=$dbname --command "$sql"
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
    zcat $gz_file | psql -v ON_ERROR_STOP=1 --host=$dbhost --username=$DEV_USER --no-password --dbname=$dbname
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

function drop_dev_database() {
    echo "Dropping database..."
    sql="drop database $DEV_DBNAME;"
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
    zcat $snapshot_filename | psql --host=$DEV_HOST --username=$DEV_USER --no-password --dbname=postgres
    unset PGPASSWORD
}


function create_users() {
    echo "Creating users..."
    sql=$(cat <<____EOF
    CREATE ROLE anonymous_rest_user WITH NOLOGIN;

    CREATE ROLE sead_read WITH LOGIN;
    CREATE ROLE sead_write WITH LOGIN;

    CREATE ROLE humlab_read WITH NOINHERIT LOGIN PASSWORD '${DEV_PASSWORD}';
    CREATE ROLE querysead_worker WITH LOGIN PASSWORD '${DEV_PASSWORD}';
    CREATE ROLE sead_user_api WITH LOGIN PASSWORD '${DEV_PASSWORD}';

    CREATE ROLE clearinghouse_worker WITH CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}';
    CREATE ROLE querysead_owner WITH LOGIN PASSWORD '${DEV_PASSWORD}' VALID UNTIL 'infinity';

    CREATE ROLE humlab_admin WITH SUPERUSER CREATEROLE CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}';
    CREATE ROLE postgres WITH SUPERUSER CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS;

    CREATE ROLE sead_master WITH CREATEROLE CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}';

    GRANT querysead_owner TO humlab_admin GRANTED BY humlab_admin;

    CREATE ROLE seadwrite WITH SUPERUSER CREATEDB LOGIN PASSWORD '${DEV_PASSWORD}';
    CREATE ROLE sead WITH NOLOGIN VALID UNTIL 'infinity';
    GRANT sead_read TO sead_master GRANTED BY seadwrite;
____EOF
    )
    PGPASSWORD=${POSTGRES_PASSWORD}
    dev_exec_sql "$DEV_HOST" "postgres" "$sql" > /dev/null
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
    # drop_dev_database
    create_dev_database
fi
