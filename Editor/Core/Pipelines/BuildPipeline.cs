using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildPipeline),
        menuName = MenuPaths.Pipelines + "Pipeline"
    )]
    public sealed class BuildPipeline : ScriptableObject
        //ScriptablePostBuildPipeline
    {
        [Serializable]
        private sealed class PipelineStep
        {
            public ScriptableCustomBuildStep Step = default;
            public bool Skip = false;
        }

        [SerializeField] private PipelineStep[] m_steps = default;

        [ContextMenu("Run")]
        // public override async Task Run()
        public async Task Run()
        {
            if (Application.isBatchMode)
            {
                throw new Exception($"{nameof(BuildPipeline)}: can not be run from the BatchMode!");
            }

            if (m_steps == null || m_steps.Length == 0)
            {
                Debug.Log($"{nameof(BuildPipeline)}: no any post build steps");

                return;
            }

            try
            {
                for (int i = 0; i < m_steps.Length; i++)
                {
                    EditorUtility.ClearProgressBar();
                    if (m_steps[i] == null || m_steps[i].Step == null)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The step at index {i} is null!");

                        continue;
                    }

                    EditorUtility.DisplayProgressBar($"Execute {m_steps[i].Step.name} ({i + 1}/{m_steps.Length})...", $"{m_steps[i].Step.name} ", i / (float)m_steps.Length);

                    if (m_steps[i].Skip)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The {m_steps[i].Step.name} was skipped!");

                        continue;
                    }

                    await m_steps[i].Step.Execute();
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            // try
            // {
            //     await PreBuild();
            //     BuildReport report = await Build();
            //     BuildSummary summary = report.summary;
            //     if (summary.result == BuildResult.Failed)
            //     {
            //         throw new Exception($"{nameof(BuildPipeline)}: {name} Build failed!");
            //     }
            //
            //     await PostBuild();
            // }
            // catch (Exception e)
            // {
            //     Debug.LogException(e);
            //
            //     throw;
            // }
        }
    }
}