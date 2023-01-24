using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// Creates iOS archive from the xCode workspace using CLI tools
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(ArchiveWorkspace),
        menuName = MenuPaths.IOS + nameof(ArchiveWorkspace)
    )]
    public sealed class ArchiveWorkspace : Archive
    {
        protected override string CommandStart => "workspace";
    }
}