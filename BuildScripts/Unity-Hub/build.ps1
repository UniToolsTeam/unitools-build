# Async batch runner owns the exit code; do not pass -quit.
$ErrorActionPreference = 'Stop'
$unitoolsProjectPath = (Get-Location).Path
$unitoolsArguments = @($args)
$unitoolsEditorArgument = @()
if ($unitoolsArguments.Count -gt 0 -and (Test-Path -LiteralPath $unitoolsArguments[0] -PathType Leaf)) {
    $unitoolsEditorArgument = @($unitoolsArguments[0])
    $unitoolsArguments = @($unitoolsArguments | Select-Object -Skip 1)
}
$unitoolsLogArguments = @('-logFile', 'PipelineResults.log')
if ($unitoolsArguments -contains '-logFile') { $unitoolsLogArguments = @() }
& "$PSScriptRoot/unity.ps1" @unitoolsEditorArgument -batchmode -projectPath $unitoolsProjectPath -executeMethod UniTools.Build.BatchModeBuilder.Execute @unitoolsLogArguments @unitoolsArguments
exit $LASTEXITCODE
