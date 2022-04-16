SHELL:=/bin/bash

.DEFAULT_GOAL=info

IMAGE_NAME=sead_query_api

ifeq (,$(wildcard ./conf/appsettings.Production.json))
    $(error ./conf/appsettings.Production.json does not exist!)
endif


ifeq (,$(wildcard ./conf/.env))
    $(error ./conf/.env does not exist!)
endif

include conf/.env

ifndef SEAD_QUERY_API_TAG
	$(error SEAD_QUERY_API_TAG is undefined)
endif

.PHONY: clean up down build fetch help follow info

clean:
	@rm -f ./Dockerfile ./docker-compose.yml

fetch: clean
	@wget -nv -O ./Dockerfile https://raw.githubusercontent.com/humlab-sead/sead_query_api/$(SEAD_QUERY_API_TAG)/docker/Dockerfile
	@wget -nv -O ./docker-compose.yml https://raw.githubusercontent.com/humlab-sead/sead_query_api/$(SEAD_QUERY_API_TAG)/docker/docker-compose.yml

fetch-makefile:
	@wget -o Makefile https://raw.githubusercontent.com/humlab-sead/sead_query_api/$(SEAD_QUERY_API_TAG)/docker/Makefile

image:
	@echo "Building image using $(SEAD_QUERY_API_TAG)..."
	@docker build \
		--build-arg BRANCH=$(SEAD_QUERY_API_TAG) \
        -t $(IMAGE_NAME):latest \
        -t $(IMAGE_NAME):$(SEAD_QUERY_API_TAG) \
		-f ./Dockerfile .

down:
	-docker-compose -f docker-compose.yml down

up:
	@docker-compose -f docker-compose.yml up -d

follow:
	@docker logs $(IMAGE_NAME):$(SEAD_QUERY_API_TAG) --follow

info:
	@echo "Target branch: $(SEAD_QUERY_API_TAG)"

nuke: down clean
	@docker network prune
