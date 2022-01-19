using System;
using System.IO;
using System.Threading.Tasks;
using UniTools.IO;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(GenerateRandomPath),
        menuName = nameof(UniTools) + "/Build/Steps/Pre/" + nameof(GenerateRandomPath)
    )]
    public sealed class GenerateRandomPath : ScriptableCustomBuildStep
    {
        [SerializeField] private BaseScriptablePath m_initialPath = default;
        [SerializeField] private BaseScriptablePath m_resultPath = default;

        public override async Task Execute()
        {
            m_resultPath.Value = Path.Combine(
                                     m_initialPath.ToString(),
                                     Guid.NewGuid().ToString());

            await Task.CompletedTask;
        }
    }
}