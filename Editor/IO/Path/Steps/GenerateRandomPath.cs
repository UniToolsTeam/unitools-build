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
        [SerializeField] private PathProperty m_initial = default;
        [SerializeField] private ScriptablePath m_result = default;

        public override async Task Execute()
        {
            m_result.Value = Path.Combine(
                m_initial.ToString(),
                Guid.NewGuid().ToString());

            await Task.CompletedTask;
        }
    }
}