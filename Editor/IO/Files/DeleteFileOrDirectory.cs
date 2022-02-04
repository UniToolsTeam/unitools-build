using System.Threading.Tasks;
using UniTools.Build;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(DeleteFileOrDirectory),
        menuName = nameof(UniTools) + "/Build/Steps/Pre/" + nameof(DeleteFileOrDirectory)
    )]
    public sealed class DeleteFileOrDirectory : ScriptableCustomBuildStep
    {
        [SerializeField] private PathProperty m_path = default;

        public override async Task Execute()
        {
            FileUtil.DeleteFileOrDirectory(m_path.ToString());
            await Task.CompletedTask;
        }
    }
}