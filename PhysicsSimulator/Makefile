# Variables
DOCKER_IMAGE_NAME=passivelogic-physics-simulator-web-app
DOCKER_IMAGE_TAG= 0.0.0
DOCKER_CONTAINER_NAME=passivelogic-physics-simulator-web-app
DOCKER_REGISTRY=donrodriguez

.PHONY: build
build:
	docker build -t $(DOCKER_REGISTRY)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG) .

.PHONY: build-amd
build-amd:
	docker buildx build --platform linux/amd64 -t $(DOCKER_REGISTRY)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG) .

.PHONY: build-arm
build-arm:
	docker buildx build --platform linux/arm64 -t $(DOCKER_REGISTRY)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG) .

.PHONY: dev
dev:
	docker run -it -p 8080:80 --name $(DOCKER_CONTAINER_NAME) $(DOCKER_REGISTRY)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG)

.PHONY: stop
stop:
	docker stop $(DOCKER_CONTAINER_NAME) || true

.PHONY: remove
remove:
	docker container rm $(DOCKER_CONTAINER_NAME) || true

.PHONY: clean
clean:
	make stop remove
	docker rm $(DOCKER_REGISTRY)/$(DOCKER_IMAGE_NAME) || true
	docker rmi $(DOCKER_REGISTRY)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG) || true

newimage:
	make clean build

#main methods or whatever
.PHONY: test
test:
	make newimage dev

push:
	make clean build-arm
	docker push $(DOCKER_REGISTRY)/$(DOCKER_IMAGE_NAME):$(DOCKER_IMAGE_TAG)