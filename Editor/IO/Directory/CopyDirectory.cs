using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(CopyDirectory),
        menuName = MenuPaths.IO + nameof(CopyDirectory)
    )]
    public sealed class CopyDirectory : BuildStep
    {
        [SerializeField] private PathProperty m_source = default;
        [SerializeField] private PathProperty m_destination = default;

        public override async Task Execute()
        {
            string source = PathProperty.RequirePath(m_source, "Source directory");
            string destination = PathProperty.RequirePath(m_destination, "Destination directory");
            if (!System.IO.Directory.Exists(source)) throw new System.IO.DirectoryNotFoundException(source);
            PathProperty.ValidateCopyPaths(source, destination);
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destination));
            FileUtil.DeleteFileOrDirectory(destination);
            FileUtil.CopyFileOrDirectory(source, destination);
            await Task.CompletedTask;
        }
    }
}
