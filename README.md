# UniTools Build
This packages is a core for the configurable build pipeline using Unity Editor. 

# Features
- Customizable build pipeline with Pre and Post build steps using Scriptable Objects
- Asynchronous build steps
- Composite pipelines to build several targets simultaneously 
- Run pipelines from the editor and batch mode
- Use command line interface (CLI) with C# interface on OSX and Windows
- Add custom CLI tools


# Related packages
Those packages include functionality to customize your pipeline for different platforms. Check those packages before creating custom build steps, probably, desired functionality already created. :)
- [UniTools Build AppCenter](https://github.com/UniToolsTeam/unitools-build-appcenter)

# Installation

### Download
[Latest Releases](../../releases/latest)

### Unity Package Manager (UPM)

> You will need to have git installed and set in your system PATH.

> Check package [dependencies](https://github.com/UniToolsTeam/unitools-build/blob/master/package.json)

Add the following to `Packages/manifest.json` where x.x.x the version (tag) check [Latest Releases](../../releases/latest):

```json
{
  "dependencies": {
    "com.unitools.build": "https://github.com/UniToolsTeam/unitools-build.git#x.x.x",
    "...": "..."
  }
}
```

# Getting Started
## Build Pipeline
Build Pipeline is a Scriptable Object that contains a sequence of commands (Build Steps) executed in the strict order. 
There are several types of steps:
- Pre Build Steps. Executed before the build phase.
- Build Steps. Creates a build artifact.
- Post Build Steps. Executed after the build phase. 

To create a build pipeline call **UniTools/Build/Pipeline** from the **Create Asset** menu.
<img width="759" alt="image" src="https://user-images.githubusercontent.com/3504465/122220053-a86dbb00-ceb8-11eb-9f2c-85442573aaa8.png">

To Run a pipeline from the Unity Editor call **Run** command from the **Context Menu**.
<img width="562" alt="image" src="https://user-images.githubusercontent.com/3504465/122220682-3cd81d80-ceb9-11eb-9f07-2a453c82b1b6.png">

To see all pipelines in the current project go to the ProjectSettings/UniTools/Build tab.
<img width="712" alt="image" src="https://user-images.githubusercontent.com/3504465/122220999-89bbf400-ceb9-11eb-90e9-b25d6a77e41e.png">

## Build Steps
Before create a custom build step make sure that this is not exist at [Related Packages](#related-packages). 
To create a custom build step ScriptablePostBuildStep or ScriptablePreBuildStep base class should be used. Example:

```csharp
[CreateAssetMenu(
fileName = nameof(WaitPostBuildStep),
menuName = nameof(UniTools) + "/Build/Steps" + "/Post/Wait"
)]
public sealed class WaitPostBuildStep : ScriptablePostBuildStep
{
    [SerializeField] private int m_seconds = 1;

    public override async Task Execute(string pathToBuiltProject)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        Debug.Log($"{nameof(WaitPostBuildStep)}: started ");
        
        await Task.Delay(TimeSpan.FromSeconds(m_seconds));
        
        stopwatch.Stop();
        Debug.Log($"{nameof(WaitPostBuildStep)}: completed {stopwatch.Elapsed.TotalSeconds}");

    }
}
```
With CreateAssetMenu attribute the build step can be created as a Scriptable Object in Unity Editor and added to the Build Pipeline.

# CLI
The C# interface to work with CLI from Unity3D Editor. To check all available tools open the CLI tab in the Project Settings.

<img width="341" alt="image" src="https://user-images.githubusercontent.com/3504465/149134318-8d366d7e-4a7f-4358-bb42-61dc3d4e01e0.png">

## Unity Enviroment
CLI Tools package is working with Unity Environment. It is using the PATH variable to identify Tools paths. You can check your Unity Environment with "Tools/CLI/UnityEnvironment"

<img width="349" alt="image" src="https://user-images.githubusercontent.com/3504465/149128605-479aaba2-d7fc-429a-81e1-793b0360d29c.png">

Sometimes the tool can't be found due to a missing PATH in the Unity Environment.
<img width="436" alt="image" src="https://user-images.githubusercontent.com/3504465/149134823-29a80a53-b131-417c-b6d2-20b2f8edacba.png">

To change the PATH value in the Unity Environment you can use [PathVariable](https://github.com/UniToolsTeam/unitools-cli/blob/master/Runtime/UnityEnvironment/PathVariableAttribute.cs) attribute. Simply add this attribute with the desired path to any C# class. For example:

```csharp
[assembly: PathVariable("/usr/local/bin")]
```

## Using the existing tool
All tools available in the code via a Cli class. For example:
```csharp
var terminal = Cli.Tool<OsxTerminal>();
var result = terminal.Execute("echo Hello world!");
Debug.Log(result);

```

## Custom tools
Custom tools allow creating any custom methods together with Execute inside the tool class for more convenient usage in the code. Before creating a custom tool, please check existing [packages](https://github.com/UniToolsTeam), probably, the tool was already created. :)
### Create a custom tool
To create a custom tool you need to inherit [BaseCliTool](https://github.com/UniToolsTeam/unitools-cli/blob/master/Runtime/CliTools/Base/Tools/BaseCliTool.cs) class and create a tool attribute that also must be inherited from the [BaseCliToolAttribute](https://github.com/UniToolsTeam/unitools-cli/blob/master/Runtime/CliTools/Base/Attributes/BaseCliToolAttribute.cs).
For example:
```csharp
public sealed class AppCenterAttribute : BaseCliToolAttribute
{
    public AppCenterAttribute() : base(AppCenter.ToolName)
    {
    }

    public override BaseCliTool Create()
    {
        return new AppCenter(
           PathResolver.Default.Execute(AppCenter.ToolName).Output.Split(Environment.NewLine.ToCharArray())?[0],
           CommandLine.Default);
    }
}

[assembly: AppCenter]
public sealed class AppCenter : BaseCliTool
    , ICliToolVersion
    , ICliToolFriendlyName
    , ICliToolHelpLink
{
    public const string ToolName = "appcenter";

    private readonly CommandLine m_commandLine = default;
    private string m_version = string.Empty;

    public AppCenter(string path, CommandLine commandLine)
    {
        Path = path;
        m_commandLine = commandLine;
    }

    public string Name => nameof(AppCenter);
    public string Link => "https://docs.microsoft.com/en-us/appcenter/cli/";

    public string Version
    {
        get
        {
            if (string.IsNullOrEmpty(m_version))
            {
                m_version = Execute("--version").Output;
            }

            return m_version;
        }
    }

    public override string Path { get; } = default;

    public override ToolResult Execute(string arguments = null, string workingDirectory = null)
    {
        if (!IsInstalled)
        {
            throw new ToolNotInstalledException();
        }
        
        if (string.IsNullOrEmpty(arguments))
        {
            arguments = string.Empty;
        }

        if (string.IsNullOrEmpty(workingDirectory))
        {
            workingDirectory = string.Empty;
        }

        return m_commandLine.Execute($"{Path} {arguments}", workingDirectory);
    }

    public override string ToString()
    {
        return $"{nameof(AppCenter)}: {Path}, {Version}";
    }
}

```
### Using a custom tool
Using the custom tool is the same as any existing tool.
```csharp
var myTool = Cli.Tool<MyCustomTool>();
var result = terminal.Execute("-a test");
Debug.Log(result);

```

## Generic tools
Sometimes it is enough to have only an Execute method for the tool. In this case, all that is needed is to add [CliToolAttribute](https://github.com/UniToolsTeam/unitools-cli/blob/master/Runtime/CliTools/GenericTool/CliToolAttribute.cs) to the assembly with a tool executable name.
For example:
```csharp
//create a tool
[assembly: CliTool("aws")]

//use a tool
void Foo()
{
    var aws = Cli.Tool("aws");
    var result = aws.Execute("s3 ls --profile myprofile");
    Debug.Log(result);
}

```
