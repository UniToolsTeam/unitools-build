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
    {
        [Serializable]
        private sealed class PipelineStep
        {
            public string Name = string.Empty;
            public BuildStep Step = default;
            public bool Skip = false;
        }

        [SerializeField] private PipelineStep[] m_steps = default;

        public int Count => m_steps.Length;

        private void OnValidate()
        {
            foreach (PipelineStep step in m_steps)
            {
                if (step == null) continue;

                if (step.Skip)
                {
                    step.Name = $"{step.Step.name}(Skipped)";
                }
                else
                {
                    step.Name = step.Step.name;
                }
            }
        }

        public async Task RunStep(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new Exception($"{nameof(BuildPipeline)}: Invalid step index {index}!");
            }

            try
            {
                EditorUtility.ClearProgressBar();
                if (m_steps[index] == null || m_steps[index].Step == null)
                {
                    Debug.LogWarning($"{nameof(BuildPipeline)}: The step at index {index} is null!");

                    return;
                }

                EditorUtility.DisplayProgressBar($"Execute {m_steps[index].Step.name} ({index + 1}/{m_steps.Length})...", $"{m_steps[index].Step.name} ", index / (float)Count);

                if (m_steps[index].Skip)
                {
                    Debug.LogWarning($"{nameof(BuildPipeline)}: The {m_steps[index].Step.name} was skipped!");

                    return;
                }

                await m_steps[index].Step.Execute();
                EditorUtility.ClearProgressBar();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        [ContextMenu("Run")]
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
                for (int i = 0; i < Count; i++)
                {
                    await RunStep(i);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                throw e;
            }
        }
    }
}