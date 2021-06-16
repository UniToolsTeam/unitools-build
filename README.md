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
- [UniTools Build iOS](https://github.com/Rinal/unitools-build-ios)
- [UniTools Build Android](https://github.com/Rinal/unitools-build-android)
- [UniTools Build Defines](https://github.com/Rinal/unitools-build-defines)
- [UniTools Build AppCenter](https://github.com/Rinal/unitools-build-appcenter)
- [UniTools Build Versioning Semantic](https://github.com/Rinal/unitools-build-versioning-semantic)

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

To create a build pipeline call **UniTools/Build/Pipeline** from the **Create Asset** menu.
<img width="759" alt="image" src="https://user-images.githubusercontent.com/3504465/122220053-a86dbb00-ceb8-11eb-9f2c-85442573aaa8.png">

To Run a pipeline from the Unity Editor call **Run** command from the **Context Menu**.
<img width="562" alt="image" src="https://user-images.githubusercontent.com/3504465/122220682-3cd81d80-ceb9-11eb-9f07-2a453c82b1b6.png">

To see all pipelines in the current project go to the ProjectSettings/UniTools/Build tab.
<img width="712" alt="image" src="https://user-images.githubusercontent.com/3504465/122220999-89bbf400-ceb9-11eb-90e9-b25d6a77e41e.png">

## Build Steps
Before create a custom build step make sure that this is not exist at [Related Packages](#related-packages). 
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
With CreateAssetMenu attribute the build step can be created as a Scriptable Object in Unity Editor and added to the Build Pipeline.
