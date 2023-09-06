using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class UnityBuildStepWithOptions : UnityBuildStep
    {
        [SerializeField] private PathProperty m_path = default;
        [SerializeField] private bool m_developmentBuild = false;
#if UNITY_2021_2_OR_NEWER
        [SerializeField] private bool m_cleanBuildCache = false;
#endif
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

#if UNITY_2021_2_OR_NEWER
                if (m_cleanBuildCache)
                {
                    buildPlayerOptions.options |= BuildOptions.CleanBuildCache;
                }
#endif

                return buildPlayerOptions;
            }
        }
    }
}