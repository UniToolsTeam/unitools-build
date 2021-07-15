using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public static class ScriptablePostBuildPipelineExtension
    {
        public static async Task PostBuild(this BuildPipeline pipeline, string pathToBuiltProject)
        {
            ScriptablePostBuildPipeline.PostBuildStep[] postBuildSteps = pipeline.PostBuildSteps.ToArray();

            if (pipeline.PostBuildSteps == null || postBuildSteps.Length == 0)
            {
                Debug.Log($"{nameof(ScriptablePostBuildPipeline)}: no any post build steps");

                return;
            }

            try
            {
                BuildDiagnostics buildDiagnostics = new BuildDiagnostics(nameof(ScriptablePostBuildPipeline));

                for (int i = 0; i < postBuildSteps.Length; i++)
                {
                    EditorUtility.ClearProgressBar();
                    if (postBuildSteps[i] == null || postBuildSteps[i].Step == null)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The step at index {i} is null!");

                        continue;
                    }

                    EditorUtility.DisplayProgressBar($"Post Build ({i + 1}/{postBuildSteps.Length})...", $"{postBuildSteps[i].Step.name} ", i / (float)postBuildSteps.Length);

                    if (postBuildSteps[i].Skip)
                    {
                        Debug.LogWarning($"{nameof(ScriptablePostBuildPipeline)}: The {postBuildSteps[i].Step.name} was skipped!");

                        continue;
                    }

                    buildDiagnostics.StartTrackingStep(postBuildSteps[i].Step.name);

                    await postBuildSteps[i].Step.Execute(pathToBuiltProject);

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
