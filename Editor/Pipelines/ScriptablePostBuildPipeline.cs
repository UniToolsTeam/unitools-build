using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptablePostBuildPipeline : ScriptablePreBuildPipeline
    {
        [SerializeField] private ScriptableBuildStep m_build = default;

        [Serializable]
        public sealed class PostBuildStep
        {
            public ScriptablePostBuildStep Step = default;
            public bool Skip = false;
        }

        [SerializeField] private PostBuildStep[] m_postBuild = default;

        public IEnumerable<PostBuildStep> PostBuildSteps => m_postBuild;

        public async Task<BuildReport> Build() => await m_build.Execute();
    }
}