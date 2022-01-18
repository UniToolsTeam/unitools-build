using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(CompositeBuildPipeline),
        menuName = nameof(UniTools) + "/Build/Composite Pipeline"
    )]
    public sealed class CompositeBuildPipeline : ScriptablePreBuildPipeline
    {
        [SerializeField] private BuildPipeline[] m_pipelines = default;

        public IEnumerable<BuildPipeline> Pipelines => m_pipelines;

        [ContextMenu("Run")]
        public override async Task Run()
        {
            if (Application.isBatchMode)
            {
                //TODO Implement BatchMode exception
                throw new Exception($"{nameof(CompositeBuildPipeline)}: can not be run from the BatchMode!");
            }

            try
            {
                await PreBuild();

                foreach (BuildPipeline pipeline in m_pipelines)
                {
                    await pipeline.Run();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                throw;
            }
        }
    }
}