using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(EnsureDirectoryEmpty),
        menuName = MenuPaths.IO + nameof(EnsureDirectoryEmpty)
    )]
    public sealed class EnsureDirectoryEmpty : BuildStep
    {
        [SerializeField] private PathProperty m_path = default;

        public override async Task Execute()
        {
            DirectoryInfo di = new DirectoryInfo(PathProperty.RequirePath(m_path, "Directory path"));

            if (!di.Exists)
            {
                throw new Exception($"{nameof(EnsureDirectoryEmpty)}: failed due to directory does not exists!");
            }

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            await Task.CompletedTask;
        }

        public override string ToString()
        {
            return m_path?.ToString() ?? string.Empty;
        }
    }
}
