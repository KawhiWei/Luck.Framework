#!/usr/bin/env bash

set -Eeuo pipefail

if (($# != 0)); then
    printf 'Usage: %s\n' "${0##*/}" >&2
    exit 2
fi

if [[ -z ${NUGET_API_KEY:-} ]]; then
    printf 'NUGET_API_KEY is required; refusing to read it interactively.\n' >&2
    exit 2
fi

SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)
PACKAGE_DIR=$SCRIPT_DIR/artifacts/nuget

shopt -s nullglob
packages=("$PACKAGE_DIR"/*.nupkg)
if ((${#packages[@]} == 0)); then
    printf 'No .nupkg files found in %s. Run build_package.sh first.\n' "$PACKAGE_DIR" >&2
    exit 1
fi

for package in "${packages[@]}"; do
    printf 'Pushing %s\n' "${package##*/}"
    dotnet nuget push "$package" \
        --api-key "$NUGET_API_KEY" \
        --source https://api.nuget.org/v3/index.json \
        --skip-duplicate
done

printf 'NuGet push completed.\n'
