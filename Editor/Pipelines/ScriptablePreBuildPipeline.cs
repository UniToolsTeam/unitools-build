using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptablePreBuildPipeline : ScriptableBuildPipeline
    {
        [Serializable]
        private sealed class PreBuildStep
        {
            public ScriptablePreBuildStep Step = default;
            public bool Skip = false;
        }

        [SerializeField] private PreBuildStep[] m_preBuild = default;

        public async Task PreBuild()
        {
            if (m_preBuild == null || m_preBuild.Length == 0)
            {
                Debug.Log($"{nameof(ScriptablePreBuildPipeline)}: no any pre build steps");

                return;
            }

            try
            {
                for (int i = 0; i < m_preBuild.Length; i++)
                {
                    EditorUtility.ClearProgressBar();

                    if (m_preBuild[i] == null || m_preBuild[i].Step == null)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePreBuildPipeline)}: The step at index {i} is null!");

                        continue;
                    }

                    EditorUtility.DisplayProgressBar($"Pre Build ({i + 1}/{m_preBuild.Length})...", $"{m_preBuild[i].Step.name} ", i / (float) m_preBuild.Length);

                    if (m_preBuild[i].Skip)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePreBuildPipeline)}: The {m_preBuild[i].Step.name} was skipped!");

                        continue;
                    }

                    await m_preBuild[i].Step.Execute();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}