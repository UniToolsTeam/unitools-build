#!/usr/bin/env bash
#This file must be located at the project root.

#The default quit timeout is 300 seconds (5 mins). The full build process could take more time, that's why we set this value bigger
QUIT_TIMEOUT=86400
#This is a current project directory.
PROJECT_PATH=$(pwd)

#Checking if the Unity Path was delivered with from CLI
UNITY_PATH="$1"
if [ -f "$UNITY_PATH" ]
then
    echo "The Unity Editor is found at Path $UNITY_PATH from CLI"
else
    echo "Unity path is not assigned from the CLI. Going to generate it based on the project version"
    #Parse the Unity version from the ProjectVersion.txt file
    UNITY_VERSION=$(awk '{ print $2 ; exit}' "$PROJECT_PATH/ProjectSettings/ProjectVersion.txt")
    if [[ "$OSTYPE" == "darwin"* ]]
    then
        echo "The platform is OSX"
        #Path to the Unity Editor executable
        UNITY_PATH="/Applications/Unity/Hub/Editor/$UNITY_VERSION/Unity.app/Contents/MacOS/Unity"
    else
        echo "This is Windows. Should be Windows :)"
        UNITY_PATH="C:/Program Files/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity.exe"
    fi
fi

echo "Unity path is $UNITY_PATH"

$UNITY_PATH -quit -quitTimeout $QUIT_TIMEOUT -batchmode -projectPath $PROJECT_PATH -executeMethod UniTools.Build.BatchModeBuilder.Execute -logFile - "$@"

#TODO For the future
# -stackTraceLogType None
