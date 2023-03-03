#!/usr/bin/env bash
#This script is usefull to run Unity CLI commands


#Read a Unity Version as a first arg (can be empty) in this case the version will be defined from the Project settings.
UNITY_VERSION="$1"
#Build Unity Path according to the OS
if [[ "$OSTYPE" == "darwin"* ]]
then
    echo "The platform is OSX"
    #Path to the Unity Editor executable
    UNITY_PATH="/Applications/Unity/Hub/Editor/$UNITY_VERSION/Unity.app/Contents/MacOS/Unity"
else
    echo "This is Windows. Should be Windows :)"
    UNITY_PATH="C:/Program Files/Unity/Hub/Editor/$UNITY_VERSION/Editor/Unity.exe"
fi
#Checking if the Unity Path
echo "Checking if Unity is available at path $UNITY_PATH"
if [ -f "$UNITY_PATH" ]
then
    echo "The Unity Editor is found at Path $UNITY_PATH"
else
    echo "Unity is not found. Going to generate it based on the project version"
    #This is a current project directory.
    PROJECT_PATH=$(pwd)
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

#Execute the command
"$UNITY_PATH" "$@"



#$UNITY_PATH -quit -quitTimeout $QUIT_TIMEOUT -batchmode -projectPath $PROJECT_PATH -executeMethod UniTools.Build.BatchModeBuilder.Execute -logFile - "$@"
