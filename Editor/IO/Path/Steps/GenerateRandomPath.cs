using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(GenerateRandomPath),
        menuName = MenuPaths.IO + nameof(GenerateRandomPath)
    )]
    public sealed class GenerateRandomPath : BuildStep
    {
        [SerializeField] private PathProperty m_initial = default;
        [SerializeField] private ScriptablePath m_result = default;

        public override async Task Execute()
        {
            if (m_result == null) throw new System.InvalidOperationException("Random path result asset is not assigned.");
            m_result.Value = Path.Combine(
                PathProperty.RequirePath(m_initial, "Initial directory"),
                Guid.NewGuid().ToString());

            await Task.CompletedTask;
        }
    }
}
