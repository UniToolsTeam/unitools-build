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
In progress..
