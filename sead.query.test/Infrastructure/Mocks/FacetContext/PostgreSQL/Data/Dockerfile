FROM postgres:15-alpine

COPY 00-public.sql /docker-entrypoint-initdb.d/
COPY 01-utility.sql /docker-entrypoint-initdb.d/
COPY 01-facet.sql /docker-entrypoint-initdb.d/
COPY 01-data.sql /docker-entrypoint-initdb.d/

