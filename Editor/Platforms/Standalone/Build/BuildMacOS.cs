using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
#if UNITY_EDITOR_OSX
using UnityEditor.OSXStandalone;
#endif
using UnityEngine;

namespace UniTools.Build.Standalone
{
    [CreateAssetMenu(
        fileName = nameof(BuildMacOS),
        menuName = nameof(UniTools) + "/Build/Steps/" + nameof(Standalone) + "/" + nameof(BuildMacOS)
    )]
    public sealed class BuildMacOS : ScriptableBuildStepWithOptions
    {
#if UNITY_EDITOR_OSX
        [SerializeField] private MacOSArchitecture m_architecture = default;
#endif
        [SerializeField] private bool m_createXcodeProject = false;

        public override BuildTarget Target => BuildTarget.StandaloneOSX;

        public override async Task<BuildReport> Execute()
        {
#if UNITY_EDITOR_OSX
            UserBuildSettings.architecture = m_architecture;
            UserBuildSettings.createXcodeProject = m_createXcodeProject;
#endif
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(Options);

            await Task.CompletedTask;

            return report;
        }
    }
}