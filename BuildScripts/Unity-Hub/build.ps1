Write-Output "PowerShell $($PSVersionTable.PSEdition) version $($PSVersionTable.PSVersion)"
#The default quit timeout is 300 seconds (5 mins). The full build process could take more time, that's why we set this value bigger
$QuitTimeout=86400
#Define the project path
$ProjectPath=$(pwd)
#Parse UnityVersion from the version file
$UnityVersion = Get-Content "$ProjectPath/ProjectSettings/ProjectVersion.txt" -Raw
$UnityVersion = $UnityVersion -split " "
$UnityVersion = $UnityVersion[1]
$UnityVersion = $UnityVersion -split "\n"
$UnityVersion = $UnityVersion[0]
$UnityVersion = $UnityVersion.Trim()
#Create a path to the Unity Folder
$UnityPath="$Env:Programfiles\Unity\Hub\Editor\$UnityVersion\Editor\Unity.exe"
#Path to log file
$BuildLog="PipelineResults.log"


# Remove previous Editor.log.
Remove-Item "$BuildLog" -Recurse -ErrorAction Ignore
# Start the Unity process in background thread.
Start-Process -FilePath "$UnityPath" -ArgumentList "-quit -quitTimeout $QuitTimeout -batchmode -projectPath $ProjectPath -executeMethod UniTools.Build.BatchModeBuilder.Execute -logFile $BuildLog $args"
# Wait for Editor.log to be created.
while (!(Test-Path "$BuildLog")) {
    Start-Sleep -m 10
}
# Output Editor.log until Unity is done.
Get-Content -Path "$BuildLog" -Tail 1 -Wait | Where-Object {
    Write-Host $_

    if ($_ -match "Application will terminate with return code") {
        [int]$exitCode = $_.Substring($_.get_Length()-1)
        Write-Host "Exit code is: "
        Write-Host $exitCode
        exit $exitCode
    }
}
