using System;
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

        public PreBuildStep[] PreBuildSteps
        {
            get { return m_preBuild; }
        }
    }  
}