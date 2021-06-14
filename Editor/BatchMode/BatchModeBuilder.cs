using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    public static class BatchModeBuilder
    {
        [CommandLineParameter] private static int MyParam0 = 1;
        [CommandLineParameter] private static int MyParam1 = 2;
        private static int MyParam2 = default;
        private static int MyParam3 = default;

        public static async void Execute()
        {
            if (!Application.isBatchMode)
            {
                //TODO Implement BatchMode exception
                throw new Exception($"{nameof(BatchModeBuilder)}: can be run only from the BatchMode!");
            }

            BatchModeParameters parameters = CommandLineParser.Parse<BatchModeParameters>(Environment.CommandLine);

            string pipelineName = parameters.Pipeline;

            //check if the selected pipeline is composite
            if (Find<CompositeBuildPipeline>(pipelineName) != null)
            {
                await RunCompositeBuildPipeline(pipelineName);
            }
            //check if the selected pipeline is simple
            else if (Find<BuildPipeline>(pipelineName) != null)
            {
                await RunBuildPipeline(pipelineName);
            }
            else
            {
                throw new Exception($"Failed to find a pipeline with name {pipelineName}!");
            }
        }

        private static TPipeline Find<TPipeline>(string pipelineName) where TPipeline : ScriptableBuildPipeline
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(ScriptableBuildPipeline)}");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TPipeline pipeline = AssetDatabase.LoadAssetAtPath<TPipeline>(path);
                if (pipeline == null)
                {
                    continue;
                }

                if (pipeline.name.Equals(pipelineName))
                {
                    return pipeline;
                }
            }

            return null;
        }

        private static async Task RunCompositeBuildPipeline(string name)
        {
            CompositeBuildPipeline pipeline = Find<CompositeBuildPipeline>(name);
            if (pipeline == null)
            {
                throw new Exception($"Failed to find a pipeline with name {name}!");
            }

            string[] childNames = pipeline.Pipelines.Select(p => p.name).ToArray();

            foreach (string s in childNames)
            {
                Debug.Log($"{nameof(RunCompositeBuildPipeline)}: {s}");
            }

            await pipeline.PreBuild();

            foreach (string childName in childNames)
            {
                await RunBuildPipeline(childName);
            }
        }

        private static async Task RunBuildPipeline(string name)
        {
            BuildPipeline pipeline = Find<BuildPipeline>(name);
            if (pipeline == null)
            {
                throw new Exception($"Failed to find a pipeline with name {name}!");
            }

            await pipeline.PreBuild();
            BuildReport report = await pipeline.Build();
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Failed)
            {
                throw new Exception($"{nameof(BatchModeBuilder)}: {name} failed!");
            }

            //after the build we have to find a pipeline again due to unity clean all serialize fields and previous reference is invalid
            pipeline = Find<BuildPipeline>(name);
            if (pipeline == null)
            {
                throw new Exception($"Failed to find a pipeline with name {name}!");
            }

            await pipeline.PostBuild(summary.outputPath);
        }
    }
}