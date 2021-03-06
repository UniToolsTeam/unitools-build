using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildWindows),
        menuName = MenuPaths.Standalone + nameof(BuildWindows)
    )]
    public sealed class BuildWindows : ScriptableBuildStepWithOptions
    {
        public override BuildTarget Target => BuildTarget.StandaloneWindows;

        public override async Task<BuildReport> Execute()
        {
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(Options);

            await Task.CompletedTask;

            return report;
        }
    }
}