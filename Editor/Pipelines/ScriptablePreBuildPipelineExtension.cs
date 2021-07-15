using System;
using System.Linq;
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
            ScriptablePreBuildPipeline.PreBuildStep[] preBuildSteps = pipeline.PreBuildSteps.ToArray();

            if (pipeline.PreBuildSteps == null || preBuildSteps.Length == 0)
            {
                Debug.Log($"{nameof(ScriptablePreBuildPipeline)}: no any pre build steps");

                return;
            }

            try
            {
                BuildDiagnostics buildDiagnostics = new BuildDiagnostics(nameof(ScriptablePreBuildPipeline));

                for (int i = 0; i < preBuildSteps.Length; i++)
                {
                    EditorUtility.ClearProgressBar();

                    if (preBuildSteps[i] == null || preBuildSteps[i].Step == null)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePreBuildPipeline)}: The step at index {i} is null!");
                        continue;
                    }

                    EditorUtility.DisplayProgressBar($"Pre Build ({i + 1}/{preBuildSteps.Length})...", $"{preBuildSteps[i].Step.name} ", i / (float)preBuildSteps.Length);

                    if (pipeline.PreBuildSteps.ToArray()[i].Skip)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePreBuildPipeline)}: The {pipeline.PreBuildSteps.ToArray()[i].Step.name} was skipped!");
                        continue;
                    }

                    buildDiagnostics.StartTrackingStep(preBuildSteps[i].Step.name);

                    await preBuildSteps[i].Step.Execute();

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
