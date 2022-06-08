using System.Threading.Tasks;
using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// Creates iOS archive from the xCode project using CLI tools
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(ArchiveProject),
        menuName = MenuPaths.IOS + nameof(ArchiveProject)
    )]
    public sealed class ArchiveProject : Archive
    {
        protected override string CommandStart => "project";
    }
}