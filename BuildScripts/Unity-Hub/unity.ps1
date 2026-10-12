# Generic Windows Editor wrapper. Run from the Unity project root.
$ErrorActionPreference = 'Stop'
$unitoolsArguments = @($args)
$unitoolsEditorPath = $env:UNITOOLS_UNITY_PATH
if ($unitoolsArguments.Count -gt 0 -and (Test-Path -LiteralPath $unitoolsArguments[0] -PathType Leaf)) {
    $unitoolsEditorPath = $unitoolsArguments[0]
    $unitoolsArguments = @($unitoolsArguments | Select-Object -Skip 1)
}
if ([string]::IsNullOrWhiteSpace($unitoolsEditorPath)) {
    $unitoolsVersion = (Select-String -Path 'ProjectSettings/ProjectVersion.txt' -Pattern '^m_EditorVersion: (.+)$').Matches.Groups[1].Value.Trim()
    $unitoolsEditorPath = Join-Path $env:ProgramFiles "Unity/Hub/Editor/$unitoolsVersion/Editor/Unity.exe"
}
if (!(Test-Path -LiteralPath $unitoolsEditorPath -PathType Leaf)) {
    throw "Unity executable not found: $unitoolsEditorPath"
}
function ConvertTo-UnityArgument([string]$value) {
    # Windows native argv quoting: preserve embedded quotes and trailing backslashes.
    $value = [regex]::Replace($value, '(\\*)"', '$1$1\"')
    $value = [regex]::Replace($value, '(\\+)$', '$1$1')
    return '"' + $value + '"'
}
$unitoolsArgumentString = ($unitoolsArguments | ForEach-Object { ConvertTo-UnityArgument ([string]$_) }) -join ' '
$unitoolsProcess = Start-Process -FilePath $unitoolsEditorPath -ArgumentList $unitoolsArgumentString -Wait -PassThru
exit $unitoolsProcess.ExitCode
