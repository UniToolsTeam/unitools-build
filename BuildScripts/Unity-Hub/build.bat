@echo off
echo CallPSHere


.\build.ps1 -buildTarget Android --pipeline Android_Sanitary


"C:\Program Files\Unity\Hub\Editor\2021.3.2f1\Editor\Unity.exe" -batchMode -skipMissingProjectID -skipMissingUPID -buildTarget Android -logFile - -projectPath "C:\BuildAgent\work\be9cd028bb2b0e90" -quit


"C:\Program Files\Unity\Hub\Editor\2021.3.2f1\Editor\Unity.exe" -batchmode -createManualActivationFile -logfile "C:\BuildAgent\work\be9cd028bb2b0e90\myLog.txt"



--pipeline Android_Development -buildTarget Android