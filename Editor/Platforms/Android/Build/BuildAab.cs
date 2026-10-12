using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildAab),
        menuName = MenuPaths.Android + nameof(BuildAab)
    )]
    public sealed class BuildAab: UnityBuildStepWithOptions
    {
        public override BuildTarget Target => BuildTarget.Android;

        public override async Task Execute()
        {
            BuildPlayerOptions buildPlayerOptions = Options;
            EditorUserBuildSettings.buildAppBundle=true;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);
            await Task.CompletedTask;

            BuildSummary summary = report.summary;
            if (summary.result != BuildResult.Succeeded)
            {
                throw new Exception($"{nameof(BuildPipeline)}: {name} build ended with {summary.result} ({summary.totalErrors} errors).");
            }
        }
    }
}