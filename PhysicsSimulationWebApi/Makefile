# Variables
DOCKER_IMAGE_NAME = passive-logic-web-api
DOCKER_IMAGE_TAG = 0.0.0
DOCKER_CONTAINER_NAME = passive-logic-web-api
DOCKER_USERNAME = donrodriguez

# DOCKER ================================================================================================================================
.PHONY: build
build:
	docker build -f PhysicsSimulationWebApi.API/Dockerfile -t $(DOCKER_USERNAME)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG) .

.PHONY: build-amd
build-amd:
	docker buildx build -f API/Dockerfile -t $(DOCKER_USERNAME)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG) .

.PHONY: dev
dev:
	docker run -it -p 8080:80 --name $(DOCKER_CONTAINER_NAME) $(DOCKER_USERNAME)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG)

.PHONY: stop
stop:
	docker stop $(DOCKER_CONTAINER_NAME) || true

.PHONY: remove
remove:
	docker container rm $(DOCKER_CONTAINER_NAME) || true

.PHONY: clean
clean:
	make stop remove
	docker rm $(DOCKER_USERNAME)/$(DOCKER_IMAGE_NAME) || true
	docker rmi $(DOCKER_USERNAME)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG) || true

newimage:
	make clean build

#main methods or whatever
.PHONY: test
test:
	make newimage dev

push:
	make clean build-amd
	docker push $(DOCKER_USERNAME)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG)