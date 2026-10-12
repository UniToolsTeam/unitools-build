using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildPipeline),
        menuName = MenuPaths.Core + "Pipeline"
    )]
    public sealed class BuildPipeline : ScriptableObject
    {
        //TODO Notes for the custom editor 
        //https://forum.unity.com/threads/solved-propertydrawer-on-array-elements-with-different-heights.542194/
        [Serializable]
        private sealed class PipelineStep
        {
            public string Name = string.Empty;
            public BuildStep Step = default;
            public bool Skip = false;
        }

        [SerializeField] private PipelineStep[] m_steps = default;

        private bool m_running;

        public int Count => m_steps?.Length ?? 0;

        private void OnValidate()
        {
            if (m_steps == null) return;

            foreach (PipelineStep step in m_steps)
            {
                if (step == null) continue;
                if (step.Step == null) continue;

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
                    throw new BuildStepFailedException($"{nameof(BuildPipeline)}: The step at index {index} is not assigned!");
                }

                EditorUtility.DisplayProgressBar($"Execute {m_steps[index].Step.name} ({index + 1}/{m_steps.Length})...", $"{m_steps[index].Step.name} ", index / (float)Count);

                if (m_steps[index].Skip)
                {
                    Debug.Log($"{nameof(BuildPipeline)}: The {m_steps[index].Step.name} was skipped.");

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

        public async Task Run()
        {
            if (Application.isBatchMode)
            {
                throw new Exception($"{nameof(BuildPipeline)}: can not be run from the BatchMode!");
            }

            if (m_running)
            {
                throw new InvalidOperationException($"{nameof(BuildPipeline)}: {name} is already running.");
            }

            if (Count == 0)
            {
                throw new BuildStepFailedException($"{nameof(BuildPipeline)}: {name} has no steps.");
            }

            m_running = true;
            try
            {
                for (int i = 0; i < Count; i++)
                {
                    await RunStep(i);
                }
            }
            finally
            {
                m_running = false;
            }
        }

        // Unity context menus require a void callback; callers of Run still receive its Task.
        [ContextMenu("Run")]
        private async void RunFromMenu()
        {
            try { await Run(); }
            catch (Exception exception) { Debug.LogException(exception); }
        }
    }
}
