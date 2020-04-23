#!/bin/bash

rm -f Dockerfile docker-compose.yml docker-compose.override.yml

wget https://raw.githubusercontent.com/humlab-sead/sead_query_api/master/Dockerfile
wget https://raw.githubusercontent.com/humlab-sead/sead_query_api/master/docker-compose.yml
wget https://raw.githubusercontent.com/humlab-sead/sead_query_api/master/docker-compose.override.yml

if [ ! -d "./conf" ]; then
  echo "Error: ./conf not found. Can not continue."
  exit 1
fi

if [ ! -f "./conf/appsettings.Production.json" ]; then
  echo "Error: ./conf not found. Can not continue."
  exit 1
fi

docker build --rm -f "Dockerfile" -t sead_query_api:latest ./conf