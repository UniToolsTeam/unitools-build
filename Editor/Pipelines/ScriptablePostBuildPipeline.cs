using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptablePostBuildPipeline : ScriptablePreBuildPipeline
    {
        [SerializeField] private ScriptableBuildStep m_build = default;

        [Serializable]
        private sealed class PostBuildStep
        {
            public ScriptablePostBuildStep Step = default;
            public bool Skip = false;
        }

        [SerializeField] private PostBuildStep[] m_postBuild = default;

        public async Task<BuildReport> Build() => await m_build.Execute();

        public async Task PostBuild(string pathToBuiltProject)
        {
            if (m_postBuild == null || m_postBuild.Length == 0)
            {
                Debug.Log($"{nameof(ScriptablePostBuildPipeline)}: no any post build steps");

                return;
            }

            try
            {
                for (int i = 0; i < m_postBuild.Length; i++)
                {
                    EditorUtility.ClearProgressBar();
                    if (m_postBuild[i] == null || m_postBuild[i].Step == null)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The at index {i} is null!");

                        continue;
                    }

                    EditorUtility.DisplayProgressBar($"Post Build ({i + 1}/{m_postBuild.Length})...", $"{m_postBuild[i].Step.name} ", i / (float) m_postBuild.Length);

                    if (m_postBuild[i].Skip)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The {m_postBuild[i].Step.name} was skipped!");

                        continue;
                    }

                    await m_postBuild[i].Step.Execute(pathToBuiltProject);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}