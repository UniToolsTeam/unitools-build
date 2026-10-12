# UniTools Build

An Editor package for building Unity projects sequentially through ScriptableObject steps. UPM `com.unitools.build` `0.1.10-preview`, namespace/assembly `UniTools.Build`, MIT license. Steps run in the Editor or batch mode. AI agent instructions are in [AGENTS.md](AGENTS.md).

## Installation

Standard Git UPM entry:

```json
"com.unitools.build": "https://github.com/UniToolsTeam/unitools-build.git#master"
```

For a managed local copy, add an SSH submodule from the **Unity project root**, with permission from the Git repository owner:

```sh
git submodule add -b master git@github.com:UniToolsTeam/unitools-build.git Packages/com.unitools.build
```

If the Unity project is a subdirectory of another repository, prefix the path relative to that repository's root. Replace the UPM entry with:

```json
"com.unitools.build": "file:com.unitools.build"
```

The submodule pins a specific commit; `branch = master` does not update it automatically. A fresh clone requires an authorized `git submodule update --init --recursive`. The machine owner configures the SSH account/host alias; the package does not.

There are no external UPM dependencies. All code is Editor-only; the iOS Xcode API requires the iOS module and an active iOS target. The upstream minimum in the manifest is Unity `2019.1`; the version currently verified is `6000.6.3f1`, and older versions have not been reverified. Builds require platform modules; iOS archive/export requires macOS, Xcode, and signing credentials.

## Pipelines and parameters

Create an asset through **Create → UniTools → Pipeline**, then add `BuildStep` assets to `m_steps` in the required order. Run it through **Context Menu → Run** or **Project Settings → UniTools → Build → Run**. Each successful step completes its entire Task before the next step starts; an error stops the pipeline. Empty/null steps are configuration errors; `Skip` is an explicit omission. Do not start the same pipeline asset again while `Run` is active.

There are no separate Pre/Post base classes: derive from **`BuildStep`** and add operations to a single ordered list. For example:

```csharp
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Wait")]
public sealed class WaitBuildStep : BuildStep
{
    [SerializeField] private int m_milliseconds = 250;
    public override Task Execute() => Task.Delay(m_milliseconds);
}
```

`UnityBuildStepWithOptions` creates BuildPlayerOptions from enabled scenes and serialized output/development/clean-cache settings. Available steps cover APK/AAB, macOS/Windows, iOS Xcode, WebGL; archive/export/upload; signing; defines; numeric version increments; and paths/copy/delete/ZIP. Only `BuildResult.Succeeded` counts as success. The pipeline does not roll back changes automatically: signing/version/defines and platform settings remain changed.

`StringBuildParameter`, `IntBuildParameter`, and `ScriptingDefineSymbols` can be overridden in batch mode. A key is the asset name without spaces, converted to invariant lowercase and prefixed with `--`; for example, `Release Notes` → `--releasenotes`. Quote values containing spaces. The parser supports `--key=value`, empty strings, and negative numbers; a malformed integer is an error. Parameter keys must be unique and must not conflict with `--pipeline` or Unity arguments.

Version steps use `System.Version`, with 2–4 numeric components and no SemVer suffixes; incrementing preserves the other components.

## Batch mode

From the Unity project root:

```sh
/path/to/Unity -batchmode -projectPath "$PWD" \
  -executeMethod UniTools.Build.BatchModeBuilder.Execute \
  --pipeline "Assets/Build/Release Pipeline.asset" -logFile -
```

A unique exact asset name can be supplied instead of a path. Duplicate names are rejected. The runner exits the Editor with **exit 0** only after all Tasks complete, or **exit 1** on error. **Do not pass `-quit`** to this entry point: it manages its own lifetime to await asynchronous completion. General rules for `-executeMethod`, `-batchmode`, and Editor termination are described in the [Unity CLI reference](https://docs.unity3d.com/6000.0/Documentation/Manual/EditorCommandLineArguments.html).

The runner stores the pipeline GUID and next step in SessionState; after a synchronous `ApplyDefines`, it waits for compilation/domain reload and continues. A custom async step must not reload assemblies before its Task completes. Use batch mode for pipelines whose Editor Run would change defines. Recovery after a crash/restart and cancellation are not implemented; an external runner must provide the CI timeout.

Wrappers in `BuildScripts/Unity-Hub/` can be run without copying them, but the working directory must be the Unity project root:

```sh
bash Packages/com.unitools.build/BuildScripts/Unity-Hub/build.sh \
  "/path with spaces/to/Unity" --pipeline "Release Pipeline"
```

`build.sh` locates the installed Hub Editor using ProjectVersion.txt; an optional first argument or `UNITOOLS_UNITY_PATH` selects the executable. `unity.sh` is a generic wrapper; its optional first argument is an executable or a Unity version, and all remaining arguments are passed unchanged. PowerShell `build.ps1`/`unity.ps1` and `build.bat` use the Windows Hub path or `UNITOOLS_UNITY_PATH`, wait for the process to finish, and return its exit code. The PowerShell build writes `PipelineResults.log`.

## CLI

**Project Settings → UniTools → CLI** shows registered tools; **Tools → CLI → UnityEnvironment** shows the environment inherited by the Editor. `[assembly: PathVariable("/your/tool/directory")]` appends to the process PATH using the standard separator. Do not add machine-specific paths to a reusable package.

```csharp
ToolResult result = CommandLine.Default.Execute("echo Hello");
if (result.ExitCode != 0) throw new BuildStepFailedException(result.Error);
```

`CommandLine.Execute` runs a shell command synchronously. stdout/stderr are read concurrently; a long command still blocks its caller, and timeout/cancellation is not built in. Wrap literal values/paths with `CommandLine.QuoteArgument(value)`. Raw command strings and `Archive.m_extraArgs` must be trusted configuration. Windows cmd has its own expansion rules; QuoteArgument does not make arbitrary shell input safe.

`Cli.Tool<T>()` returns the first registered tool of that type or null; `Cli.Tool(name)` requires the exact registered executable key. Available tools are the shell, path resolver, Tar, Zip, and XCodeBuild (registered only on the iOS target). `IsInstalled` checks file existence, not whether the executable can run successfully.

For a custom tool, derive from `BaseCliTool`, create a `BaseCliToolAttribute` with `Create()`, and add an assembly attribute. For a generic tool, use `[assembly: CliTool("executable")]` or `[assembly: CliToolPath("executable", "/absolute/path")]`. Shell paths containing spaces are supported. Check `ToolResult.ExitCode` in every build step.

IO copy operations validate the source before deleting the destination and reject overlapping paths. The ZIP step uses .NET, includes hidden files, and creates an archive outside the source tree. `m_archiveFileName` can be absolute or relative to the source folder, for example `../output.zip`. Output inside the source is rejected; an existing ZIP is replaced rather than updated with stale entries. Authors must check symlink aliases themselves.

## Review and verification — 2026-10-12

Reviewed from upstream `master`, commit `0e8f482178de0c3b2a8c8ff0c6631d0631cc3417`: fixed async callbacks/CI exit and reload continuation, parsing/quoting, process pipe deadlocks/disposal, PATH, build results/disabled scenes, null configuration, copy overlap, ZIP, iOS plist parsing, and obsolete Unity APIs. Public type/assembly identities, the version, and original GUIDs are preserved. Outdated README examples using Pre/Post classes were replaced with the actual API.

Verified with Unity `6000.6.3f1` on macOS:

- Host compilation and `dotnet build UniTools.Build.csproj --no-restore` (.NET SDK `10.0.401`): **0 warnings / 0 errors**. The current Unity Console also reports 0/0; the history of transient compiler/Pipeline diagnostics was retained.
- A minimal host containing only this package, without game/Test Framework dependencies: Standalone Editor compilation reports **0 compiler warnings/errors** after the UAC0005 fix. An actual `BuildMacOS` batch run produced `Smoke.app` (72 MB), with exit **0**; a disabled scene with a nonexistent path was excluded from build options.
- **51/51** temporary behavioral checks: awaited execution order/stopping/skip/null/concurrent Run, parser/parameters, IO overlap, ZIP hidden files and overwrite, shell quoting, missing executables, stdout/stderr including 128 KiB stderr without deadlock, and PATH. **16/16** iOS checks: provisioning XML, the first TeamIdentifier, invalid input, archive/export arguments containing spaces and literal shell characters, and fake Xcode failure/missing-tool behavior.
- **10/10** Bash wrapper checks: exact argv with spaces, optional executable/version, log overrides, and exit propagation; both scripts pass `bash -n` with the standard macOS Bash.
- Actual batch launches: async success → **0**; failed step → **1**, without executing the next marker; missing pipeline → **1**; ApplyDefines → compilation/domain reload → next step → **0**, with before/after each executed once.
- Consumer game-1: **217/217 Prototype1.Core.Tests**, 0 failed/skipped; Android (**50 assemblies**) and iOS (**48**) player scripts compile with `Prototype1.Core`/`Prototype1.Runtime` and without `UniTools.Build`/test assemblies. IDE projects were regenerated through Unity.
- All **201 original .meta** files are byte-identical to upstream; the package version and assembly GUID are unchanged.

Windows/PowerShell, Linux, and older Unity Editors were not run. iOS CLI checks used a local fake executable: no signing credentials, App Store uploads, or device QA were involved. The minimal host's build artifact and the consumer's player-script compilation do not constitute a mobile release of the game. Temporary fixtures/outputs are not part of the package.

Fixes were saved in a separate review commit on top of upstream master; the consumer's Git state pins it through a gitlink. The integration permission granted on 2026-10-12 does not authorize future Git operations.
