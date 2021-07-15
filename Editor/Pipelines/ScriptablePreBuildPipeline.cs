using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptablePreBuildPipeline : ScriptableBuildPipeline
    {
        [Serializable]
        public sealed class PreBuildStep
        {
            public ScriptablePreBuildStep Step = default;
            public bool Skip = false;
        }

        [SerializeField] private PreBuildStep[] m_preBuild = default;

        public IEnumerable<PreBuildStep> PreBuildSteps => m_preBuild;
    }
}