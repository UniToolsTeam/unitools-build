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
        [SerializeField] private RandomPathSettings m_randomPathSettings = default;

        public override async Task Execute()
        {
            m_randomPathSettings.GenerateRandomId();
            await Task.CompletedTask;
        }
    }
}