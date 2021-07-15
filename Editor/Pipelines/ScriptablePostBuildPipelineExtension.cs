using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public static class ScriptablePostBuildPipelineExtension
    {
        public static async Task PostBuild(this BuildPipeline pipeline, string pathToBuiltProject)
        {
            if (pipeline.PostBuildSteps == null || pipeline.PostBuildSteps.Length == 0)
            {
                Debug.Log($"{nameof(ScriptablePostBuildPipeline)}: no any post build steps");

                return;
            }

            try
            {
                BuildDiagnostics buildDiagnostics = new BuildDiagnostics(nameof(ScriptablePostBuildPipeline));

                for (int i = 0; i < pipeline.PostBuildSteps.Length; i++)
                {
                    EditorUtility.ClearProgressBar();
                    if (pipeline.PostBuildSteps[i] == null || pipeline.PostBuildSteps[i].Step == null)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The step at index {i} is null!");

                        continue;
                    }

                    EditorUtility.DisplayProgressBar($"Post Build ({i + 1}/{pipeline.PostBuildSteps.Length})...", $"{pipeline.PostBuildSteps[i].Step.name} ", i / (float)pipeline.PostBuildSteps.Length);

                    if (pipeline.PostBuildSteps[i].Skip)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The {pipeline.PostBuildSteps[i].Step.name} was skipped!");

                        continue;
                    }

                    buildDiagnostics.StartTrackingStep(pipeline.PostBuildSteps[i].Step.name);

                    await pipeline.PostBuildSteps[i].Step.Execute(pathToBuiltProject);

                    buildDiagnostics.StopTrackingStep();
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
