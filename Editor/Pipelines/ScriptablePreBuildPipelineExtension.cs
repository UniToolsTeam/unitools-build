using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public static class ScriptablePreBuildPipelineExtension
    {
        //TODO: Change to BuildPipeline extension and adjust composite build pipeline for it
        public static async Task PreBuild(this ScriptablePreBuildPipeline pipeline)
        {
            if (pipeline.PreBuildSteps == null || pipeline.PreBuildSteps.Length == 0)
            {
                Debug.Log($"{nameof(ScriptablePreBuildPipeline)}: no any pre build steps");

                return;
            }

            try
            {
                BuildDiagnostics buildDiagnostics = new BuildDiagnostics(nameof(ScriptablePreBuildPipeline));

                for (int i = 0; i < pipeline.PreBuildSteps.Length; i++)
                {
                    EditorUtility.ClearProgressBar();

                    if (pipeline.PreBuildSteps[i] == null || pipeline.PreBuildSteps[i].Step == null)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePreBuildPipeline)}: The step at index {i} is null!");
                        continue;
                    }

                    EditorUtility.DisplayProgressBar($"Pre Build ({i + 1}/{pipeline.PreBuildSteps.Length})...", $"{pipeline.PreBuildSteps[i].Step.name} ", i / (float)pipeline.PreBuildSteps.Length);

                    if (pipeline.PreBuildSteps[i].Skip)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePreBuildPipeline)}: The {pipeline.PreBuildSteps[i].Step.name} was skipped!");
                        continue;
                    }

                    buildDiagnostics.StartTrackingStep(pipeline.PreBuildSteps[i].Step.name);

                    await pipeline.PreBuildSteps[i].Step.Execute();

                    buildDiagnostics.StopTrackingStep();
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
