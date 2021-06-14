using System.Threading.Tasks;
using UniTools.IO;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(DeleteFileOrDirectory),
        menuName = nameof(UniTools) + "/Build/Steps/IO/Pre/" + nameof(DeleteFileOrDirectory)
    )]
    public sealed class DeleteFileOrDirectory : ScriptablePreBuildStep
    {
        [SerializeField] private PathProperty m_path = default;

        public override async Task Execute()
        {
            FileUtil.DeleteFileOrDirectory(m_path.ToString());
            await Task.CompletedTask;
        }
    }
}