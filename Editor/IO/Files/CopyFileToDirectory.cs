using System.IO;
using System.Threading.Tasks;
using UniTools.Build;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(CopyFileToDirectory),
        menuName = MenuPaths.IO + nameof(CopyFileToDirectory)
    )]
    public sealed class CopyFileToDirectory : ScriptableCustomBuildStep
    {
        [SerializeField] private PathProperty m_filePath = default;
        [SerializeField] private PathProperty m_destination = default;

        public override async Task Execute()
        {
            string fileName = Path.GetFileName(m_filePath.ToString());
            string dest = Path.Combine(m_destination.ToString(), fileName);
            FileUtil.DeleteFileOrDirectory(dest);
            FileUtil.CopyFileOrDirectory(m_filePath.ToString(), dest);
            await Task.CompletedTask;
        }
    }
}