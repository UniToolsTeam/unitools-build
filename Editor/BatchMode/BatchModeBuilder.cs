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

            const string key = "--pipeline";
            string pipeline = string.Empty;

            if (!CommandLineParser.TryToParseValue(Environment.CommandLine, key, out object v))
            {
                throw new Exception($"The pipeline is not assigned from the CommandLine. Use {key} to define a pipeline for execution.");
            }

            pipeline = v.ToString();

            if (Find(pipeline) != null)
            {
                await RunBuildPipeline(pipeline);
            }
            else
            {
                throw new Exception($"Failed to find a pipeline with name {pipeline}!");
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