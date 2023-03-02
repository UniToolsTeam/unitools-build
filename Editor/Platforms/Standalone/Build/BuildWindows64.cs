using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildWindows64),
        menuName = MenuPaths.Standalone + nameof(BuildWindows64)
    )]
    public sealed class BuildWindows64 : UnityBuildStepWithOptions
    {
        public override BuildTarget Target => BuildTarget.StandaloneWindows64;

        public override async Task Execute()
        {
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(Options);

            await Task.CompletedTask;

            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Failed)
            {
                throw new Exception($"{nameof(BuildPipeline)}: {name} Build failed!");
            }
        }
    }
}