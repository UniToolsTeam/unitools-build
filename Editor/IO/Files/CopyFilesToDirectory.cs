using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(CopyFilesToDirectory),
        menuName = MenuPaths.IO + nameof(CopyFilesToDirectory)
    )]
    public sealed class CopyFilesToDirectory : BuildStep
    {
        [SerializeField] private PathProperty m_sourceDirectory = default;
        [SerializeField] private PathProperty[] m_sourceFiles = default;
        [SerializeField] private PathProperty m_destinationDirectory = default;

        public override async Task Execute()
        {
            var sourceFolder = m_sourceDirectory.ToString();
            var hasNoSourceFolder = string.IsNullOrEmpty(sourceFolder);
            var destinationFolder = m_destinationDirectory.ToString();

            if (!hasNoSourceFolder && !Directory.Exists(sourceFolder))
            {
                throw new DirectoryNotFoundException($"Source folder: {sourceFolder} does not exist!");
            }
            if (!Directory.Exists(destinationFolder))
            {
                throw new DirectoryNotFoundException($"Destination folder {destinationFolder} does not exist!");
            }
            
            foreach (var sourceFile in m_sourceFiles)
            {
                var filePath = sourceFile.ToString();
                var fileName = Path.GetFileName(filePath);
                var source = hasNoSourceFolder ? filePath : Path.Combine(sourceFolder, filePath);
                var destination = Path.Combine(destinationFolder, fileName);
                FileUtil.DeleteFileOrDirectory(destination);
                FileUtil.CopyFileOrDirectory(source, destination);
            }
            
            await Task.CompletedTask;
        }
    }
}