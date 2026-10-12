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
    public sealed class CopyFileToDirectory : BuildStep
    {
        [SerializeField] private PathProperty m_filePath = default;
        [SerializeField] private PathProperty m_destination = default;

        public override async Task Execute()
        {
            string source = PathProperty.RequirePath(m_filePath, "Source file");
            string directory = PathProperty.RequirePath(m_destination, "Destination directory");
            if (!File.Exists(source)) throw new FileNotFoundException("Copy source does not exist.", source);
            string dest = Path.Combine(directory, Path.GetFileName(source));
            PathProperty.ValidateCopyPaths(source, dest);
            Directory.CreateDirectory(directory);
            FileUtil.DeleteFileOrDirectory(dest);
            FileUtil.CopyFileOrDirectory(source, dest);
            await Task.CompletedTask;
        }
    }
}
