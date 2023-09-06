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
            FileUtil.DeleteFileOrDirectory(m_destination.ToString());
            FileUtil.CopyFileOrDirectory(m_source.ToString(), m_destination.ToString());
            await Task.CompletedTask;
        }
    }
}