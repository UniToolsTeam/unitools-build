# UniTools Build

Build Unity projects with reusable ScriptableObject steps. Put preparation, player builds, and packaging into one ordered pipeline, then run it from the Editor or a command line.

The package includes Android, iOS, macOS, Windows, and WebGL build steps; path and file operations; version and define helpers; and C# wrappers for external command-line tools. Each step finishes before the next starts, and a failed step stops the pipeline.

## Installation

### Unity Package Manager

Add this dependency to `Packages/manifest.json` to use the `unity6-build-review` branch:

```json
{
  "dependencies": {
    "com.unitools.build": "https://github.com/UniToolsTeam/unitools-build.git#unity6-build-review"
  }
}
```

Add the entry to your existing dependencies rather than replacing the whole manifest. Git must be available on your machine for Unity to resolve the package.

### SSH submodule

From the repository root, add the package at the Unity project's `Packages/com.unitools.build` path. For a repository whose root is also the Unity project root:

```sh
git submodule add -b unity6-build-review \
  git@github.com:UniToolsTeam/unitools-build.git \
  Packages/com.unitools.build
```

Then use a local UPM dependency:

```json
{
  "dependencies": {
    "com.unitools.build": "file:com.unitools.build"
  }
}
```

For a Unity project inside a larger repository, prefix the submodule path, for example `apps/unity3d/Packages/com.unitools.build`. After cloning a repository that uses submodules, initialize them with `git submodule update --init --recursive`. The parent repository pins a specific package commit; tracking a branch does not update that commit automatically.

### Requirements

The package is Editor-only and has no external UPM dependencies. Its manifest declares Unity `2019.1` as the minimum; this branch has been used with Unity `6000.6.3f1`. Install the Unity platform module for the builds you want to create. iOS Xcode-related steps require the iOS module and an active iOS target; archiving and exporting an IPA also require macOS, Xcode, and signing credentials.

## Create and run a pipeline

1. Create a folder for your build configuration, such as `Assets/Build`.
2. Choose **Create → UniTools → Pipeline** and name the asset, for example `Android Development`.
3. Create the step assets you need under **Create → UniTools → Steps**.
4. Add them to the pipeline's **Steps** list in execution order and configure each step in its Inspector.
5. Run the pipeline from its Inspector menu or **Project Settings → UniTools → Build**.

<img width="759" alt="The original Unity Create menu with the Pipeline asset option selected" src="https://user-images.githubusercontent.com/3504465/122220053-a86dbb00-ceb8-11eb-9f2c-85442573aaa8.png">

*Create a pipeline asset from the Project window. This original screenshot shows the older menu layout; the current path is **Create → UniTools → Pipeline**.*

<img width="562" alt="The Run command in a build pipeline's Inspector menu" src="https://user-images.githubusercontent.com/3504465/122220682-3cd81d80-ceb9-11eb-9f07-2a453c82b1b6.png">

*Open the pipeline's Inspector menu and choose **Run** to execute it. The original Inspector separates Pre Build, Build, and Post Build; the current version uses one ordered **Steps** list for all three stages.*

<img width="712" alt="The UniTools Build page in Unity Project Settings listing configured pipelines" src="https://user-images.githubusercontent.com/3504465/122220999-89bbf400-ceb9-11eb-90e9-b25d6a77e41e.png">

*Use **Project Settings → UniTools → Build** to find and run the project's pipelines. Expand a pipeline in the current UI to select its asset and copy command-line examples.*

A pipeline waits for each step's `Task` before continuing. Use **Skip** to omit a configured step temporarily. An empty pipeline or an unassigned step is a configuration error. Build steps use enabled scenes only; enable your starting scene and any additional scenes the player needs in the project's build scene list.

### Example: development APK and ZIP

Create a pipeline named `Android Development` with these steps:

| Order | Step | Inspector configuration |
|---|---|---|
| 1 | `EnsureDirectoryExists` | Path: `Builds/Android` |
| 2 | `SetDebugKeystore` | Use Android's debug signing for local testing |
| 3 | `BuildApk` | Path: `Builds/Android/Game.apk`; Development Build: enabled |
| 4 | `CreateZipArchiveStep` | Directory: `Builds/Android`; Archive File Name: `../Game-Android.zip` |

Run the pipeline to produce `Builds/Android/Game.apk` and `Builds/Game-Android.zip`. The ZIP path is relative to its source directory, so `../Game-Android.zip` places the archive outside that directory. Running the ZIP step again replaces the previous archive, including removal of stale entries.

For an Android App Bundle, replace `BuildApk` with `BuildAab` and use an output path ending in `.aab`. Release signing requires your own keystore configuration.

### Available steps

| Task | Steps |
|---|---|
| Build a player | `BuildApk`, `BuildAab`, `BuildMacOS`, `BuildWindows`, `BuildWindows64`, `BuildXCode`, `BuildWebGL` |
| Prepare directories and files | `EnsureDirectoryExists`, `EnsureDirectoryEmpty`, `CopyDirectory`, `CopyFileToDirectory`, `DeleteFileOrDirectory` |
| Package output | `CreateZipArchiveStep` |
| Configure Android signing | `SetDebugKeystore`, `SetKeystorePassword`, `SetAlias` |
| Configure iOS signing | `SetAutomaticSigning`, `SetManualProvisionProfile`, `SetTeamId` |
| Work with Xcode output | `ModifyInfoPlist`, `ArchiveProject`, `ArchiveWorkspace`, `ExportIpa`, `UploadToAppStore` |
| Set defines | `ScriptingDefineSymbols`, `ApplyDefines` |
| Increment a numeric version | `BumpSemanticMajorValue`, `BumpSemanticMinorValue`, `BumpSemanticBuildValue`, `BumpSemanticRevisionValue` |

Platform build steps expose their output path and supported options in the Inspector. Only `BuildResult.Succeeded` counts as a successful build. Signing, defines, version, and platform steps change project settings; those changes remain after the pipeline completes.

## Custom build steps

Save custom step scripts under an Editor folder, such as `Assets/Editor/Build`. If you use an assembly definition, make it Editor-only and reference `UniTools.Build`.

Derive from `BuildStep` and implement `Task Execute()`. The step's position in the pipeline determines whether it runs before or after the player build. Use serialized fields for values you want to configure in the Inspector.

### Example: wait before the next step

Save this as `WaitBuildStep.cs`:

```csharp
using System;
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Build/Wait")]
public sealed class WaitBuildStep : BuildStep
{
    [SerializeField] private int m_milliseconds = 250;

    public override Task Execute()
    {
        if (m_milliseconds < 0)
            throw new ArgumentOutOfRangeException(nameof(m_milliseconds));

        return Task.Delay(m_milliseconds);
    }
}
```

Create the asset through **Create → Example → Build → Wait**, set its duration, and add it to your pipeline. Returning the delay's Task keeps the next step waiting. For an operation with several asynchronous actions, implement `async Task Execute()` and await each action.

## Parameters and paths

Create reusable values under **Create → UniTools → Parameters → String** or **Int**. A parameter's Inspector stores its default value; an optional options list lets you select from predefined values. Custom steps can reference the parameter asset and read its `Value`.

In batch mode, the asset name determines its argument key: remove spaces, convert to lowercase, and add `--`.

| Asset name | Argument | Example value |
|---|---|---|
| `Release Notes` | `--releasenotes` | `"Internal test build"` |
| `Build Number` | `--buildnumber` | `42` |
| `Build Defines` | `--builddefines` | `"MY_FEATURE;EXTRA_LOGGING"` |

`Build Defines` in this table is a `ScriptingDefineSymbols` asset. Create it through **Create → UniTools → Steps → Defines → DefineSymbols** and assign it to an `ApplyDefines` step when you want to apply those symbols to a target group.

An override applies to the matching parameter when a step reads it. Parameter keys must be unique and must not conflict with Unity arguments or `--pipeline`. Quote values containing spaces; `--key=value`, empty strings, and negative integers are supported. An invalid integer is an error.

### Example: write release notes from a parameter

Create a `StringBuildParameter` asset named `Release Notes`, then save this script as `WriteReleaseNotesStep.cs`:

```csharp
using System;
using System.IO;
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Build/Write Release Notes")]
public sealed class WriteReleaseNotesStep : BuildStep
{
    [SerializeField] private StringBuildParameter m_notes = default;
    [SerializeField] private PathProperty m_output =
        new PathProperty("Builds/release-notes.txt");

    public override Task Execute()
    {
        if (m_notes == null)
            throw new InvalidOperationException("Assign the Release Notes parameter.");

        string path = Path.GetFullPath(m_output.ToString());
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, m_notes.Value ?? string.Empty);
        return Task.CompletedTask;
    }
}
```

Create the step asset, assign `Release Notes` to its **Notes** field, and add it to your pipeline. Editor runs use the parameter's saved value; a batch override such as `--releasenotes "Internal test build"` writes that text instead.

### Example: reuse a versioned output directory

A `PathProperty` can hold a string or reference a path asset. Create a **UniTools → Steps → IO → Composite Path** asset with these parts:

| Part | Type | Value |
|---|---|---|
| 1 | String | `Builds` |
| 2 | String | `Android` |
| 3 | Scriptable | An `ApplicationVersion` asset created under **UniTools → Steps → Versioning** |

With the project version set to `1.2.3`, the composite resolves to `Builds/Android/1.2.3`. Assign it to a directory step by selecting **Scriptable** in the path field. A `ScriptablePath` asset can store a shared fixed directory; `GenerateRandomPath` writes a unique subdirectory to a designated `ScriptablePath` asset.

Version increment steps use `System.Version` with two to four numeric components, such as `1.2.3`. Incrementing one component preserves the others; prerelease suffixes such as `-beta` are not supported.

## Batch mode

Run these commands from the Unity project root. Use a pipeline's asset path or a unique exact asset name.

### Example: run an Android pipeline with release notes

```sh
/path/to/Unity -batchmode -projectPath "$PWD" \
  -executeMethod UniTools.Build.BatchModeBuilder.Execute \
  --pipeline "Assets/Build/Android Development.asset" \
  --releasenotes "Internal test build" \
  -logFile -
```

The runner exits with **0** after all steps finish and **1** if a step fails or the pipeline cannot be found. **Do not pass `-quit`** to this entry point: the runner closes Unity after awaiting the pipeline. Set a timeout in your CI runner for jobs that take too long. See the [Unity command-line reference](https://docs.unity3d.com/6000.0/Documentation/Manual/EditorCommandLineArguments.html) for the standard Editor arguments.

### Bash and PowerShell wrappers

The wrappers in `BuildScripts/Unity-Hub/` locate a Hub-installed Editor using `ProjectSettings/ProjectVersion.txt`. You can also supply the executable as the first argument or set `UNITOOLS_UNITY_PATH`.

Bash, using an explicit Editor executable:

```sh
mkdir -p Builds
bash Packages/com.unitools.build/BuildScripts/Unity-Hub/build.sh \
  "/Applications/Unity/Hub/Editor/6000.6.3f1/Unity.app/Contents/MacOS/Unity" \
  --pipeline "Android Development" \
  --releasenotes "Build from the command line" \
  -logFile "Builds/pipeline.log"
```

PowerShell, using the installed Hub Editor:

```powershell
& .\Packages\com.unitools.build\BuildScripts\Unity-Hub\build.ps1 `
  --pipeline "Android Development" `
  --releasenotes "Build from PowerShell"
```

The PowerShell build writes `PipelineResults.log`; pass `-logFile` to select another file. `unity.sh` and `unity.ps1` are generic wrappers for other Editor commands. All wrappers wait for the Editor process and return its exit code.

Use batch mode for a pipeline containing `ApplyDefines`. The runner remembers the next step, waits for the resulting compilation/domain reload, and continues. Custom asynchronous steps must finish their Task before triggering an assembly reload. Recovery after an Editor crash/restart and pipeline cancellation are not provided.

## Command-line tools

Open **Project Settings → UniTools → CLI** to inspect the registered external tools.

<img width="341" alt="The UniTools CLI page selected in Unity Project Settings" src="https://user-images.githubusercontent.com/3504465/149134318-8d366d7e-4a7f-4358-bb42-61dc3d4e01e0.png">

*The **CLI** page lists registered tools, their installation status, and any version or help information provided by the tool wrapper.*

The package provides a shell through `CommandLine.Default`, a `PathResolver`, and wrappers for `Tar`, `Zip`, and `XCodeBuild`. XCodeBuild is registered when the iOS target is active. `Cli.Tool<T>()` returns the first registered tool of that type or null; `Cli.Tool(name)` looks up an exact registered executable key.

### Example: execute a shell command

```csharp
using UniTools.Build;

public static class ShellExample
{
    public static ToolResult Run()
    {
        ToolResult result = CommandLine.Default.Execute("echo Hello world");
        if (result.ExitCode != 0)
            throw new BuildStepFailedException(result.Error);

        return result;
    }
}
```

`Execute` accepts a shell command and returns stdout, stderr, and the exit code. It reads both output streams concurrently but blocks its caller until the process exits. For long-running tools, account for that blocking behavior in your step.

Use `CommandLine.QuoteArgument(value)` when inserting a literal path or value into a command. Raw commands and extra arguments remain shell expressions.

### Example: register Node.js as a generic tool

Install Node.js separately and make it available on Unity's PATH. Save this as an Editor script:

```csharp
using UniTools.Build;

[assembly: CliTool("node")]

public static class NodeVersionExample
{
    public static string GetVersion()
    {
        BaseCliTool node = Cli.Tool("node");
        if (!node.IsInstalled)
            throw new ToolNotInstalledException();

        ToolResult result = node.Execute("--version");
        if (result.ExitCode != 0)
            throw new BuildStepFailedException(result.Error);

        return result.Output.Trim();
    }
}
```

The assembly attribute registers the executable; `Cli.Tool("node")` retrieves its wrapper. As an alternative, specify an executable path instead of searching PATH:

```csharp
using UniTools.Build;

[assembly: CliToolPath("node", "/absolute/path/to/node")]
```

Use one registration per executable. Choose either PATH discovery or an explicit path for this example.

### Example: add a typed tool with a reusable command

A typed tool can expose operations such as `RunScript` instead of repeating command construction in every step. Use this registration **instead of** either generic Node registration above. Save the types under an Editor folder:

```csharp
using UniTools.Build;

[assembly: NodeTool]

public sealed class NodeToolAttribute : BaseCliToolAttribute
{
    public NodeToolAttribute() : base("node") { }

    public override BaseCliTool Create()
    {
        BaseCliTool executable = new CliToolAttribute(Executable).Create();
        return new NodeTool(executable);
    }
}

public sealed class NodeTool : BaseCliTool, ICliToolFriendlyName
{
    private readonly BaseCliTool m_executable;

    public NodeTool(BaseCliTool executable) => m_executable = executable;

    public string Name => "Node.js";
    public override string Path => m_executable.Path;

    public override ToolResult Execute(
        string arguments = null, string workingDirectory = null)
    {
        return m_executable.Execute(arguments, workingDirectory);
    }

    public ToolResult RunScript(string scriptPath, string workingDirectory = null)
    {
        return Execute(CommandLine.QuoteArgument(scriptPath), workingDirectory);
    }
}
```

Then use the typed tool in a pipeline step. Save this as `RunNodeScriptStep.cs`:

```csharp
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Build/Run Node Script")]
public sealed class RunNodeScriptStep : BuildStep
{
    [SerializeField] private PathProperty m_script =
        new PathProperty("BuildTools/package-build.cjs");

    public override Task Execute()
    {
        NodeTool node = Cli.Tool<NodeTool>();
        if (node == null || !node.IsInstalled)
            throw new ToolNotInstalledException();

        ToolResult result = node.RunScript(m_script.ToString(), ProjectPath.Value);
        if (result.ExitCode != 0)
            throw new BuildStepFailedException(result.Error);

        Debug.Log(result.Output);
        return Task.CompletedTask;
    }
}
```

Assign a script that exists in your project. The example uses the Unity project root as the working directory, quotes the script path, and stops the pipeline if Node returns a nonzero exit code.

## Unity environment and PATH

Unity inherits its environment when the Editor starts. A tool available in your terminal may be missing from the Editor's PATH.

<img width="349" alt="The Tools menu command for opening the UnityEnvironment window" src="https://user-images.githubusercontent.com/3504465/149128605-479aaba2-d7fc-429a-81e1-793b0360d29c.png">

*Choose **Tools → CLI → UnityEnvironment** to inspect the environment variables visible to Unity, including the PATH used to locate external tools.*

<img width="436" alt="The CLI settings page showing missing command-line tools in red" src="https://user-images.githubusercontent.com/3504465/149134823-29a80a53-b131-417c-b6d2-20b2f8edacba.png">

*Tools shown in red could not be found at an existing absolute executable path. Install the required tool, check Unity's PATH, or provide an explicit path. The names shown here are from the original screenshot; your list depends on the registered tools.*

You can append a directory to Unity's process PATH with an assembly attribute in an Editor script:

```csharp
using UniTools.Build;

[assembly: PathVariable("/your/tool/directory")]
```

Use a real directory on your machine. Recompile/reopen Unity after adding registrations or changing its startup environment; call `Cli.Refresh()` to repeat discovery within the current session. `IsInstalled` checks whether the executable file exists, while executing it and checking `ExitCode` confirms whether the command succeeds.

## Troubleshooting

| Symptom | What to check |
|---|---|
| Pipeline cannot be found | Pass the asset path or a unique exact asset name. Duplicate names require a path. |
| No enabled scenes | Enable the scenes needed by your player in the project's build scene list. |
| Tool is not found | Check the CLI settings page, Unity's PATH, and the executable's installed location. |
| A step fails but later output is missing | The pipeline stops on errors. Check the failing step's Console entry or CLI stderr. |
| Copy operation rejects its paths | The source must exist, and source/destination must not overlap or contain one another. |
| ZIP output is rejected | Use a `.zip` output path outside the source directory, such as `../output.zip`. |
| Defines interrupt an Editor run | Run that pipeline in batch mode so it can continue after compilation/domain reload. |

## License

[MIT](LICENSE).
