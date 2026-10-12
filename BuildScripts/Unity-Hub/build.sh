#!/usr/bin/env bash
# Run from the Unity project root. An optional first argument selects the Editor executable.
set -euo pipefail
unitools_project_path="$PWD"
unitools_editor_path="${UNITOOLS_UNITY_PATH:-}"
if [[ $# -gt 0 && -f "$1" ]]; then
    unitools_editor_path="$1"
    shift
fi
if [[ -z "$unitools_editor_path" ]]; then
    unitools_version=$(awk '/^m_EditorVersion:/{print $2; exit}' "$unitools_project_path/ProjectSettings/ProjectVersion.txt")
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
# The asynchronous batch runner exits explicitly with 0/1. Passing -quit would end it early.
for unitools_argument in "$@"; do
    if [[ "$unitools_argument" == '-logFile' ]]; then
        exec "$unitools_editor_path" -batchmode -projectPath "$unitools_project_path" \
            -executeMethod UniTools.Build.BatchModeBuilder.Execute "$@"
    fi
done
exec "$unitools_editor_path" -batchmode -projectPath "$unitools_project_path" \
    -executeMethod UniTools.Build.BatchModeBuilder.Execute -logFile - "$@"
