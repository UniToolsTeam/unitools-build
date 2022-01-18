using System;
using System.Threading.Tasks;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(BuildPipeline),
        menuName = nameof(UniTools) + "/Build/Pipeline"
    )]
    public sealed class BuildPipeline : ScriptablePostBuildPipeline
    {
        [ContextMenu("Run")]
        public override async Task Run()
        {
            if (Application.isBatchMode)
            {
                //TODO Implement BatchMode exception
                throw new Exception($"{nameof(BuildPipeline)}: can not be run from the BatchMode!");
            }

            try
            {
                await PreBuild();
                BuildReport report = await Build();
                BuildSummary summary = report.summary;
                if (summary.result == BuildResult.Failed)
                {
                    //TODO Create a separate build exception
                    throw new Exception($"{nameof(BuildPipeline)}: {name} Build failed!");
                }

                await PostBuild(summary.outputPath);
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                throw;
            }
        }
    }
}