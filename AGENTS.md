# Instructions for AI agents — UniTools Build

This file is self-contained for work on the package. Additional rules from a consumer repository apply when the package is installed there. Before making changes, read [README.md](README.md), [package.json](package.json), and `Editor/UniTools.Build.asmdef`. Write package documentation, including README.md and AGENTS.md, in English.

## Purpose and compatibility

- UPM ID `com.unitools.build`, version `0.1.10-preview`, assembly and namespace `UniTools.Build`. The assembly contains Editor code only; do not reference it from runtime/player assemblies.
- The package provides a sequential pipeline of `BuildStep` ScriptableObjects, parameters, platform build steps, IO, and CLI tools. It is independent of gameplay, VContainer, Commands, BehaviorTree, and any particular game.
- `package.json` retains the upstream minimum Unity version of `2019.1`; the version verified after this review is `6000.6.3f1`. Preserve version guards for newer APIs. Compilation in Unity 6 does not establish compatibility with older Editors.
- There are no external UPM dependencies. Standard Editor/IMGUI/UIElements and .NET APIs are required. iOS code using `UnityEditor.iOS.Xcode` compiles only under `UNITY_IOS` and requires the installed iOS module and an active iOS target. The CLI supports a macOS/Linux shell and Windows cmd; verification on macOS does not establish Windows/Linux compatibility.
- Actual builds require the corresponding Unity module. Archive/Export require macOS, Xcode, certificates, and provisioning profiles; installing the package neither installs these prerequisites nor authorizes publication.

## Navigation

| Path | Responsibility |
|---|---|
| `Editor/Core/Pipelines/` | `BuildPipeline`, execution order, and Project Settings UI |
| `Editor/Core/Steps/` | `BuildStep.Execute(): Task`, `UnityBuildStep`, and build options |
| `Editor/Core/Parameters/` | String/int assets, CLI overrides, and Inspectors |
| `Editor/BatchMode/` | Arguments, `BatchModeBuilder.Execute`, CI exit codes, and resuming after reload |
| `Editor/Platforms/` | Android, iOS, Standalone, and WebGL |
| `Editor/IO/` | Paths, copying, deletion, and ZIP |
| `Editor/Defines/`, `Editor/Versioning/` | ProjectSettings and version changes |
| `Editor/CliTools/`, `Editor/UnityEnvironment/` | Processes, PATH discovery, and shell quoting |
| `BuildScripts/Unity-Hub/` | Bash/PowerShell/bat wrappers, run from the Unity project root |

## Contracts

- Preserve public APIs, assembly identity, serialized field names, enum values, and every existing `.meta` GUID. Move a file together with its `.meta`; a new Unity file requires a new unique GUID. Do not change the package version without an agreed versioning task.
- Implement each new operation as a separate `BuildStep`; use serialized fields or a parameter asset for configuration. A custom step returns a `Task` that completes only when all its work is finished. Errors and nonzero exit codes must stop the pipeline; do not hide them with a log message or a successful Task.
- `BuildPipeline.Run` is intended for the Editor; `RunStep` also serves the batch runner. An empty pipeline, a null step, or an invalid index is a configuration error. `Skip` is an explicit authoring choice and is logged without a warning. Steps run sequentially; do not start the same asset again while `Run` is active.
- Do not call blocking `.Wait()` / `.GetAwaiter().GetResult()` on an unfinished Task on the Unity main thread. Use `async void` only for Unity menu/UI callbacks with exception handling; operation APIs return Task. The batch runner reads a Task result only after `IsCompleted`.
- The batch runner calls `EditorApplication.Exit(0/1)` itself. **Do not add `-quit`** to this executeMethod. Identify a pipeline by its asset path or a unique exact name; duplicate names are rejected. Arguments are read from OS-tokenized argv; the string parser remains part of the public parameter API.
- Batch progress is stored in `SessionState`: the asset GUID and the next index. After a synchronous step that changes defines, the runner waits for compilation/reload and continues with the next step. A custom async step must not trigger a domain reload while its Task is unfinished; split such operations into separate steps. Resuming works within the Editor process, not after a crash/restart.
- There is no automatic cancellation or rollback of completed steps. Run pipelines that change defines and trigger domain reload through batch mode. Do not promise parallel Unity builds or restoration of ProjectSettings after a pipeline.
- Build options include enabled scenes only. Only `BuildResult.Succeeded` is a successful Unity build; Failed, Cancelled, Unknown, or an exception stops the pipeline.
- `CommandLine.Execute` accepts an intentional shell command, not an argv array. Wrap literal paths/values with `CommandLine.QuoteArgument`; raw extra arguments are trusted author configuration. Read stdout and stderr concurrently and dispose the process. The CLI API is synchronous and has no timeout/cancellation; long custom CLI steps must define their own policy explicitly.
- `IsInstalled` means that a file exists at an absolute path; it does not establish usability, executability, or license availability. Discovery selects the first existing executable from a separate output line. Append PATH entries using `Path.PathSeparator`, without substring comparisons.
- Validate IO paths **before** deleting a destination. Copy source and destination must not coincide or contain one another. Create ZIP archives through the .NET API outside the source tree, include hidden files, and replace the previous archive. Path validation is lexical; the author is responsible for symlink aliases and permitted build-output directories.
- Signing/defines/version/platform steps intentionally change ProjectSettings. Do not run an unverified pipeline in a consumer project. Never log passwords or put them in documentation/fixtures; supply them to CI through authorized local or secret inputs.
- Version steps use `System.Version` (2–4 nonnegative numeric components), not full SemVer/prerelease syntax. Increment does not reset other components; a missing build/revision is normalized to 0 before incrementing; overflow is an error.
- Do not hide compiler warnings with a broad `#pragma warning disable`. The narrow CS0414 suppression beside iOS serialized fields exists because the Inspector retains them on other targets.

## Verification and documentation

1. Compile **the entire package** through the Unity Editor and check compiler warnings/errors, including with Android and iOS active targets. Do not edit `Library`, `Temp`, generated `.csproj` files, or compiler output.
2. Verify a standalone minimal host with the local UPM entry `"com.unitools.build": "file:<absolute-package-path>"`, without game packages. The package has no Test Framework dependency or checked-in test suite; use temporary C# probes/the consumer's Editor Test Runner, and do not substitute static searches for execution.
3. For changed contracts, check execution order/awaiting, failure stopping subsequent steps, explicit Skip/null steps, quoted/empty/negative CLI values, large stderr output, nonzero exit codes, missing tools, paths with spaces, invalid copy paths, and ZIP overwrite/hidden files. Create fixtures and output only in a disposable temporary directory and remove them after verification.
4. An actual batch launch must exit with 0 on success and 1 for a missing pipeline/failed step. Check an asynchronous delay and a step that changes defines → reload → next step. Scripts must preserve exact arguments and exit codes; Bash: `bash -n BuildScripts/Unity-Hub/{build,unity}.sh`. Verify PowerShell/Windows on their own host.
5. In a consumer project, check UPM resolution, compilation, and relevant existing tests; confirm that `UniTools.Build` is absent from player assemblies. A build artifact, player-script compilation, and device QA are separate forms of evidence.
6. Record results, the Editor version, actual limitations, and behavior changes in the existing README. Do not add separate review/report files or Python verification tools. Do not clear the Console to hide diagnostics.

## Git and permissions

- This is a separate upstream Git repository. Changes to Git state/index/history/remotes/config, commit/push, LFS, and submodule operations require explicit user permission for the corresponding actions. Permission from a previous review does not apply to future tasks.
- Run read-only Git commands with `GIT_OPTIONAL_LOCKS=0`. Ordinary source edits within the task scope are permitted.
- A package commit and the host gitlink are separate actions. Local fixes in a dirty submodule **are not included** in its pinned commit and cannot be reproduced by a fresh clone. Clearly report that state at handoff; do not pin an unpublished commit without an accessible remote.
- For an authorized commit, use an English message and `Stanislav Felinskyi <felinskyi.sg@gmail.com>` for both author and committer. Verify both identities before publication. Review/integration does not authorize publishing a build, creating a release/tag, or messaging third parties.
