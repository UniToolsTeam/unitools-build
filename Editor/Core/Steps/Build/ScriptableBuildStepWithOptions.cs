using System.Linq;
using UniTools.Build;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptableBuildStepWithOptions : ScriptablePlatformBuildStep
    {
        [SerializeField] private PathProperty m_path = default;
        [SerializeField] private bool m_developmentBuild = false;

        protected BuildPlayerOptions Options
        {
            get
            {
                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
                buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
                buildPlayerOptions.locationPathName = m_path.ToString();
                buildPlayerOptions.target = Target;
                buildPlayerOptions.targetGroup = UnityEditor.BuildPipeline.GetBuildTargetGroup(Target);

                if (m_developmentBuild)
                {
                    buildPlayerOptions.options = BuildOptions.Development;
                }

                return buildPlayerOptions;
            }
        }
    }
}