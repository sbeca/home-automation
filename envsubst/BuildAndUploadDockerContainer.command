#!/bin/sh

SCRIPTS_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"
cd "${SCRIPTS_DIR}"

# Build Docker containers for both x86-64 and ARM64 and upload them to Docker Hub
docker buildx build --platform linux/amd64,linux/arm64 --tag scottbeca/envsubst:latest --push .
