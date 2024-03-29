using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// This type of steps should be user to create any custom behavior for the build pipeline
    /// </summary>
    public abstract class BuildStep : ScriptableObject
    {
        public abstract Task Execute();

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