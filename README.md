# UniTools Build
This packages is a core for the configurable build pipeline using Unity Editor. 

# Features
- Customizable build pipeline with Pre and Post build steps using Scriptable Objects
- Asynchronous build steps
- Composite pipelines to build several targets simultaneously 
- Run pipelines from the editor and batch mode

# Dependencies
- [UniTools CLI](https://github.com/Rinal/unitools-cli)
- [UniTools IO](https://github.com/Rinal/unitools-io)

# Related packages
Those packages include functionality to customize your pipeline for different platforms. Check those packages before creating custom build steps, probably, desired functionality already created. :)
- [UniTools Build iOS]()
- [UniTools Build Android]()
- [UniTools Build Defines]()
- [UniTools Build Versioning Semantic]()
- [UniTools Build AppCenter]()

# Installation

### Download
[Latest Releases](../../releases/latest)

### Unity Package Manager (UPM)

> You will need to have git installed and set in your system PATH.

> Check package [dependencies](https://github.com/Rinal/unitools-build/blob/master/package.json)

Add the following to `Packages/manifest.json` where x.x.x the version (tag) check [Latest Releases](../../releases/latest):

```
{
  "dependencies": {
    "com.unitools.cli": "https://github.com/Rinal/unitools-cli.git#x.x.x",
    "com.unitools.io": "https://github.com/Rinal/unitools-io.git#x.x.x",
    "com.unitools.build": "https://github.com/Rinal/unitools-build.git#x.x.x",
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

To create a build pipeline call UniTools/Build/Pipeline from the Create Asset menu. (ADD SCREANSHOT).

To Run a pipeline from the Unity Editor call Run command from the Context Menu (ADD SCREEANSHOT).

To see all pipelines in the current project go to the ProjectSettings/UniTools/Build tab (ADD SCREEANSHOT).
## Build Steps
Before create a custom build step make sure that this is not exist at [Related Packages](). 
To create a custom build step ScriptablePostBuildStep or ScriptablePreBuildStep base class should be used. Example:

```
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
With CreateAssetMenu attribute can be created as a Scriptable Object in Unity Editor and added to the Build Pipeline.(ADD SCREEANSHOT). 