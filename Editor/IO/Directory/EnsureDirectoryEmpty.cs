using System;
using System.IO;
using System.Threading.Tasks;
using UniTools.IO;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(EnsureDirectoryEmpty),
        menuName = nameof(UniTools) + "/Build/IO/" + nameof(EnsureDirectoryEmpty)
    )]
    public sealed class EnsureDirectoryEmpty : ScriptableCustomBuildStep
    {
        [SerializeField] private PathProperty m_path = default;

        public override async Task Execute()
        {
            DirectoryInfo di = new DirectoryInfo(m_path.ToString());

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
    }
}