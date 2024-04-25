SHELL:=/bin/bash

.DEFAULT_GOAL=info

DEPLOY_CLUSTER=/home/sead/supersead.humlab.umu.se

# ifeq (,$(wildcard ./conf/appsettings.Production.json))
#     $(error ./conf/appsettings.Production.json does not exist!)
# endif

ifeq (,$(wildcard .env))
    $(error build .env does not found!)
endif

include .env

ifndef SEAD_QUERY_API_TAG
	$(error SEAD_QUERY_API_TAG is undefined)
endif

.PHONY: image bash

image:
	@echo "Building image using $(SEAD_QUERY_API_TAG)..."
	@docker build \
		--build-arg BRANCH=$(SEAD_QUERY_API_TAG) \
		--build-arg BUILD_MODE=$(BUILD_MODE) \
        -t $(IMAGE_NAME):$(IMAGE_TAG) \
        -t $(IMAGE_NAME):$(SEAD_QUERY_API_TAG) \
		-f ./Dockerfile .

image-host-compiled:
	@echo "Building image using $(SEAD_QUERY_API_TAG) and binaries from ./publish ..."
	@rm -rf ./publish
	@dotnet publish "../sead.query.api/sead.query.api.csproj" -c $(BUILD_MODE) -o ./publish --no-restore
	@docker build \
		--build-arg BRANCH=$(SEAD_QUERY_API_TAG) \
		--build-arg BUILD_MODE=$(BUILD_MODE) \
		--no-cache \
		-t $(IMAGE_NAME):$(IMAGE_TAG) \
		-t $(IMAGE_NAME):$(SEAD_QUERY_API_TAG) \
		-f ./Dockerfile.compiled .
	@rm -rf ./publish


bash:
	@docker run --rm -it \
		-v "./conf:/app/config" \
		--env-file ./conf/.env \
			$(IMAGE_NAME):$(IMAGE_TAG) /bin/bash

jump-into-docker-compose-container:
	@pushd . \
		&& cd $(DEPLOY_CLUSTER) \
		&& sudo docker-compose run --rm --entrypoint /bin/bash sead_query_api \
		&& popd

restart-docker-compose-cluster:
	@pushd . \
		&& cd $(DEPLOY_CLUSTER) \
		&& sudo docker-compose down \
		&& sudo docker-compose up -d \
		&& popd
	