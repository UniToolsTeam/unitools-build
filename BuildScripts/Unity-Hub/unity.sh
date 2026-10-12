#!/usr/bin/env bash
# Generic Unity wrapper. Optional first argument: an Editor executable or Unity version.
set -euo pipefail
unitools_editor_path="${UNITOOLS_UNITY_PATH:-}"
unitools_version=''
if [[ $# -gt 0 && -f "$1" ]]; then
    unitools_editor_path="$1"
    shift
elif [[ $# -gt 0 && "$1" =~ ^[0-9]+\.[0-9]+\.[0-9]+[abfp][0-9]+$ ]]; then
    unitools_version="$1"
    shift
fi
if [[ -z "$unitools_editor_path" ]]; then
    if [[ -z "$unitools_version" ]]; then
        unitools_version=$(awk '/^m_EditorVersion:/{print $2; exit}' ProjectSettings/ProjectVersion.txt)
    fi
    case "$OSTYPE" in
        darwin*) unitools_editor_path="/Applications/Unity/Hub/Editor/$unitools_version/Unity.app/Contents/MacOS/Unity" ;;
        msys*|cygwin*) unitools_editor_path="C:/Program Files/Unity/Hub/Editor/$unitools_version/Editor/Unity.exe" ;;
        *) echo 'Set UNITOOLS_UNITY_PATH to the Editor executable on this platform.' >&2; exit 1 ;;
    esac
fi
if [[ ! -f "$unitools_editor_path" ]]; then
    echo "Unity executable not found: $unitools_editor_path" >&2
    exit 1
fi
exec "$unitools_editor_path" "$@"
