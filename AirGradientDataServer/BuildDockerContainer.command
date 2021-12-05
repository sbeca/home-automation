#!/bin/sh

SCRIPTS_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"
cd "${SCRIPTS_DIR}"

# Delete any old build files, just to be absolutely sure they won't be included in the Docker build
BIN_DIR="${SCRIPTS_DIR}/bin"
OBJ_DIR="${SCRIPTS_DIR}/obj"
OUT_DIR="${SCRIPTS_DIR}/out"

if [ -d "${BIN_DIR}" ]; then
    rm -rf "${BIN_DIR}"
fi
if [ -d "${OBJ_DIR}" ]; then
    rm -rf "${OBJ_DIR}"
fi
if [ -d "${OUT_DIR}" ]; then
    rm -rf "${OUT_DIR}"
fi

# Delete any test database files, just to be absolutely sure they won't be included in the Docker build
DB_FILE="${SCRIPTS_DIR}/MeasurementsDB.db"
if [ -f "${DB_FILE}" ]; then
    rm -f "${DB_FILE}"
fi
if [ -f "${DB_FILE}-shm" ]; then
    rm -f "${DB_FILE}-shm"
fi
if [ -f "${DB_FILE}-wal" ]; then
    rm -f "${DB_FILE}-wal"
fi

# Build Docker container
docker build -t airgradientdataserver .
