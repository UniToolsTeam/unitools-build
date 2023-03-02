using UnityEditor;

namespace UniTools.Build
{
    /// <summary>
    /// This step must be user to create a build (artifacts: iOS, Android, etc...) using Unity API
    /// </summary>
    public abstract class UnityBuildStep : BuildStep
    {
        public abstract BuildTarget Target { get; }
    }
}