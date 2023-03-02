using System.Threading.Tasks;
using UniTools.Build;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(DeleteFileOrDirectory),
        menuName = MenuPaths.IO + nameof(DeleteFileOrDirectory)
    )]
    public sealed class DeleteFileOrDirectory : BuildStep
    {
        [SerializeField] private PathProperty m_path = default;

        public override async Task Execute()
        {
            FileUtil.DeleteFileOrDirectory(m_path.ToString());
            await Task.CompletedTask;
        }
    }
}