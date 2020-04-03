FROM postgres:alpine

# COPY init.sql /docker-entrypoint-initdb.d/

# RUN pg_dumpall -g -h seadserv.humlab.umu.se -U humlab_admin | psql -h 127.0.0.1 -U humlab_admin \
#     && pg_dump --create -d sead_staging --schema public --schema facet --schema sead_utility -h seadserv.humlab.umu.se -F p -U humlab_admin | psql -h 127.0.0.1 -U humlab_admin

# COPY init.sql /docker-entrypoint-initdb.d/

EXPOSE 5432
