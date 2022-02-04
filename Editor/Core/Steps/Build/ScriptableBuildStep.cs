using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// This step must be user to create a build (artifacts: iOS, Android, etc...) using Unity API
    /// </summary>
    public abstract class ScriptablePlatformBuildStep : ScriptableObject
    {
        public abstract BuildTarget Target { get; }

        public abstract Task<BuildReport> Execute();

        [ContextMenu("Run")]
        private void Run()
        {
            if (EditorUtility.DisplayDialog($"Run {name} separately?", "This option must be used for the debug only. Running steps outside of the pipeline can cause unexpected behavior.", "Yes", "No"))
            {
                Debug.Log($"Running {name} separately.");

                try
                {
                    Execute().GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}