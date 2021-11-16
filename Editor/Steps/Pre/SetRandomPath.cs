using System;
using System.IO;
using System.Threading.Tasks;
using UniTools.IO;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(SetRandomPath),
        menuName = nameof(UniTools) + "/Build/Steps/Pre/" + nameof(SetRandomPath)
    )]
    public sealed class SetRandomPath : ScriptablePreBuildStep
    {
        [SerializeField] private BaseScriptablePath m_parentPath = default;
        [SerializeField] private BaseScriptablePath m_buildPath = default;

        [SerializeField] private BaseScriptablePath m_randomPath = default;

        public override async Task Execute()
        {
            m_randomPath.Value = Path.Combine(
                                     m_parentPath.ToString(),
                                     Guid.NewGuid().ToString(),
                                     m_buildPath.ToString());

            await Task.CompletedTask;
        }
    }
}