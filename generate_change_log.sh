#!/usr/bin/env bash

set -Eeuo pipefail

usage() {
    printf 'Usage: %s <version>\n' "${0##*/}"
    printf 'Creates change/<version>.md from commits since the latest release tag.\n'
}

if (($# != 1)); then
    usage >&2
    exit 2
fi

VERSION=$1
if [[ ! $VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+(-[0-9A-Za-z.-]+)?(\+[0-9A-Za-z.-]+)?$ ]]; then
    printf 'Invalid version: %s\n' "$VERSION" >&2
    exit 2
fi

SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)
CHANGE_DIR=$SCRIPT_DIR/change
CHANGE_FILE=$CHANGE_DIR/$VERSION.md

if [[ -e $CHANGE_FILE ]]; then
    printf 'Change record already exists: %s\n' "$CHANGE_FILE" >&2
    exit 1
fi

previous_tag=$(git -C "$SCRIPT_DIR" tag --merged HEAD --sort=-creatordate | \
    rg -m1 '^(Release)?[0-9]+\.[0-9]+\.[0-9]+([-.+][0-9A-Za-z.-]+)?$' || true)

if [[ -n $previous_tag ]]; then
    commit_range=$previous_tag..HEAD
    heading="Changes since ${previous_tag}"
else
    commit_range=HEAD
    heading='Changes in this release'
fi

mkdir -p "$CHANGE_DIR"
{
    printf '# Release %s\n\n' "$VERSION"
    printf '## %s\n\n' "$heading"
    git -C "$SCRIPT_DIR" log --no-merges --pretty=format:'- %s (`%h`)' "$commit_range"
    printf '\n'
} > "$CHANGE_FILE"

printf 'Generated %s. Review and edit it before committing and publishing.\n' "$CHANGE_FILE"
