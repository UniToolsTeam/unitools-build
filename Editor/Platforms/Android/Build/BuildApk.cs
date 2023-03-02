using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildApk),
        menuName = MenuPaths.Android + nameof(BuildApk)
    )]
    public sealed class BuildApk : UnityBuildStepWithOptions
    {
        public override BuildTarget Target => BuildTarget.Android;

        public override async Task Execute()
        {
            BuildPlayerOptions buildPlayerOptions = Options;
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);
            await Task.CompletedTask;

            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Failed)
            {
                throw new Exception($"{nameof(BuildPipeline)}: {name} Build failed!");
            }
        }
    }
}