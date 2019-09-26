#!/bin/bash

 wget https://raw.githubusercontent.com/humlab-sead/sead_query_api/master/Dockerfile
 wget https://raw.githubusercontent.com/humlab-sead/sead_query_api/master/docker-compose.yml
 wget https://raw.githubusercontent.com/humlab-sead/sead_query_api/master/docker-compose.override.yml

docker build --rm -f "Dockerfile" -t sead_query_api:latest ./conf

