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
        [SerializeField] private PathProperty[] m_filePaths = default;
        [System.Obsolete, HideInInspector]
        [SerializeField] private PathProperty m_filePath = default;
        [SerializeField] private PathProperty m_destination = default;

        public override async Task Execute()
        {
            foreach (var filePath in m_filePaths)
            {
                string fileName = Path.GetFileName(filePath.ToString());
                string dest = Path.Combine(m_destination.ToString(), fileName);
                FileUtil.DeleteFileOrDirectory(dest);
                FileUtil.CopyFileOrDirectory(filePath.ToString(), dest);
            }
            
            await Task.CompletedTask;
        }

        private void OnValidate()
        {
            if (m_filePaths.Length == 0 && !string.IsNullOrEmpty(m_filePath.ToString()))
            {
                m_filePaths = new[] { m_filePath };
            }
        }
    }
}