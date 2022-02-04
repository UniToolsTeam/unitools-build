using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build.Standalone
{
    [CreateAssetMenu(
        fileName = nameof(BuildWindows),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(Standalone) + "/" + nameof(BuildWindows)
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