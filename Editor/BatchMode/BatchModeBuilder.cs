using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public static class BatchModeBuilder
    {
        public static async void Execute()
        {
            if (!Application.isBatchMode)
            {
                throw new Exception($"{nameof(BatchModeBuilder)}: can be run only from the BatchMode!");
            }

            BatchModeParameters parameters = CommandLineParser.Parse<BatchModeParameters>(Environment.CommandLine);

            string pipelineName = parameters.Pipeline;

            if (Find(pipelineName) != null)
            {
                await RunBuildPipeline(pipelineName);
            }
            else
            {
                throw new Exception($"Failed to find a pipeline with name {pipelineName}!");
            }
        }

        private static BuildPipeline Find(string pipelineName)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(BuildPipeline)}");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildPipeline pipeline = AssetDatabase.LoadAssetAtPath<BuildPipeline>(path);
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

        private static async Task RunBuildPipeline(string name)
        {
            BuildPipeline pipeline = Find(name);
            if (pipeline == null)
            {
                throw new Exception($"Failed to find a pipeline with name {name}!");
            }

            int stepsCount = pipeline.Count;
            for (int i = 0; i < stepsCount; i++)
            {
                await pipeline.RunStep(i);
                pipeline = Find(name);
                if (pipeline == null)
                {
                    throw new Exception($"Failed to find a pipeline with name {name}!");
                }
            }
        }
    }
}