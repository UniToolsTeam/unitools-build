using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// Returns the current value of the application version as a Scriptable Object.
    /// Useful to use in the Path as a composite value 
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(ApplicationVersion),
        menuName = MenuPaths.Versioning + nameof(ApplicationVersion)
    )]
    public sealed class ApplicationVersion : BaseScriptablePath
    {
        public override string ToString()
        {
            return Application.version;
        }
    }
}