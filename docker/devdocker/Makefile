# Copyright (c) Jupyter Development Team.
# Distributed under the terms of the Modified BSD License.

include .env

.DEFAULT_GOAL=build

postgres_tag = sead_query_api_postgres_dev

build: postgresql
	docker-compose build

rebuild: down clear_volumes down build up follow
	@echo "Rebuild done"
	@exit 0

# check-files: userlist secrets/.env.oauth2 # $(cert_files) secrets/postgres.env

postgresql:
	docker build -t $(postgres_tag) -f postgresql/Dockerfile postgresql

bash:
	-@docker exec -it -t `docker ps -f "ancestor=$(postgres_tag)" -q --all | head -1` /bin/bash

clear_volumes:
	-docker volume rm `docker volume ls -q | grep devdocker` >/dev/null 2>&1

clean: down clear_volumes rm_containers

rm_containers: down clear_volumes
	-docker rm `docker ps -f "ancestor=$(postgres_tag)" -q --all` >/dev/null 2>&1

down:
	-docker-compose down

up:
	@docker-compose up -d

follow:
	@docker logs $(postgres_tag) --follow

restart: down up follow

nuke:
	-docker stop `docker ps --all -q`
	-docker rm -fv `docker ps --all -q`
	-docker images -q --filter "dangling=true" | xargs docker rmi

.PHONY: bash clear_volumes clean down up follow build restart nuke network
