#!/usr/bin/env bash
#This file must be located at the project root.

#The default quit timeout is 300 seconds (5 mins). The full build process could take more time, that's why we set this value bigger
QUIT_TIMEOUT=86400
#This is a current project directory.
PROJECT_PATH=$(pwd)
#Parse the Unity version from the ProjectVersion.txt file
UNITY_VERSION=$(awk '{ print $2 ; exit}' "$PROJECT_PATH/ProjectSettings/ProjectVersion.txt")
#Path to the Unity Editor executable
UNITY_PATH="/Applications/Unity/Hub/Editor/$UNITY_VERSION/Unity.app/Contents/MacOS/Unity"
#Execute unity in batchmode with desire configuration

$UNITY_PATH -quit -quitTimeout $QUIT_TIMEOUT -batchmode -projectPath $PROJECT_PATH -executeMethod UniTools.Build.BatchModeBuilder.Execute -logFile - "$@"

#TODO For the future
# -stackTraceLogType None
