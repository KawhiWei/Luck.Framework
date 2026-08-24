#!/usr/bin/env bash

set -Eeuo pipefail

usage() {
    printf 'Usage: %s <version> [--no-restore]\n' "${0##*/}"
    printf '  <version>  NuGet SemVer, for example 2.0.10 or 2.0.10-beta.1\n'
    printf '  --no-restore  Skip solution restore when dependencies are already restored\n'
}

if (($# < 1 || $# > 2)); then
    usage >&2
    exit 2
fi

VERSION=$1
SKIP_RESTORE=false

if (($# == 2)); then
    if [[ $2 != --no-restore ]]; then
        printf 'Unknown option: %s\n' "$2" >&2
        usage >&2
        exit 2
    fi
    SKIP_RESTORE=true
fi

validate_semver() {
    local version=$1
    local core prerelease token
    local -a core_parts prerelease_parts

    # NuGet package versions use SemVer with a three-part numeric core.
    if [[ ! $version =~ ^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(-[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?(\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$ ]]; then
        return 1
    fi

    core=$version
    core=${core%%-*}
    core=${core%%+*}
    local IFS=.
    read -r -a core_parts <<< "$core"
    for token in "${core_parts[@]}"; do
        if [[ $token =~ ^0[0-9]+$ ]]; then
            return 1
        fi
    done

    if [[ $version == *-* ]]; then
        prerelease=${version#*-}
        prerelease=${prerelease%%+*}
        read -r -a prerelease_parts <<< "$prerelease"
        for token in "${prerelease_parts[@]}"; do
            if [[ $token =~ ^[0-9]+$ && $token =~ ^0[0-9]+$ ]]; then
                return 1
            fi
        done
    fi
}

if ! validate_semver "$VERSION"; then
    printf 'Invalid version: %s (expected SemVer such as 2.0.10 or 2.0.10-rc.1+build.5)\n' "$VERSION" >&2
    exit 2
fi

SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)
REPO_ROOT=$SCRIPT_DIR
PACKAGE_DIR=$REPO_ROOT/artifacts/nuget

if [[ $SKIP_RESTORE != true ]]; then
    printf 'Restoring solution dependencies\n'
    dotnet restore "$REPO_ROOT/Luck.sln"
fi

# Keep this list explicit so adding or removing a published package is a single, reviewable change.
PROJECTS=(
    src/framework/Luck.Framework/Luck.Framework.csproj
    src/framework/Luck.AppModule/Luck.AppModule.csproj
    src/framework/Luck.AutoDependencyInjection/Luck.AutoDependencyInjection.csproj
    src/framework/Luck.AspNetCore/Luck.AspNetCore.csproj
    src/framework/Luck.EntityFrameworkCore/Luck.EntityFrameworkCore.csproj
    src/framework/Luck.EntityFrameworkCore.MySQL/Luck.EntityFrameworkCore.MySQL.csproj
    src/framework/Luck.EntityFrameworkCore.PostgreSQL/Luck.EntityFrameworkCore.PostgreSQL.csproj
    src/framework/Luck.EntityFrameworkCore.MemoryDataBase/Luck.EntityFrameworkCore.MemoryDataBase.csproj
    src/framework/Luck.Dapper/Luck.Dapper.csproj
    src/framework/Luck.Dapper.ClickHouse/Luck.Dapper.ClickHouse.csproj
    src/framework/Luck.MongoDB/Luck.MongoDB.csproj
    src/framework/Luck.EventBus.RabbitMQ/Luck.EventBus.RabbitMQ.csproj
    src/framework/Luck.EventBus.Kafka/Luck.EventBus.Kafka.csproj
    src/framework/Luck.EventBus.OpenTelemetry/Luck.EventBus.OpenTelemetry.csproj
    src/framework/Luck.Redis.StackExchange/Luck.Redis.StackExchange.csproj
    src/framework/Luck.DDD.Domain/Luck.DDD.Domain.csproj
    src/framework/Luck.Pipeline/Luck.Pipeline.csproj
    src/framework/Luck.Logging.Serilog/Luck.Logging.Serilog.csproj
)

rm -rf -- "$PACKAGE_DIR"
mkdir -p -- "$PACKAGE_DIR"

for project in "${PROJECTS[@]}"; do
    project_path=$REPO_ROOT/$project
    if [[ ! -f $project_path ]]; then
        printf 'Project file not found: %s\n' "$project_path" >&2
        exit 1
    fi

    printf 'Packing %s (version %s)\n' "$project" "$VERSION"
    dotnet pack "$project_path" \
        --configuration Release \
        --no-restore \
        "-p:PackageVersion=$VERSION" \
        --output "$PACKAGE_DIR"
done

shopt -s nullglob
packages=("$PACKAGE_DIR"/*.nupkg)
if ((${#packages[@]} == 0)); then
    printf 'No .nupkg files were generated in %s\n' "$PACKAGE_DIR" >&2
    exit 1
fi

printf 'Generated %d NuGet package(s) in %s\n' "${#packages[@]}" "$PACKAGE_DIR"
