using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptablePostBuildStep : ScriptableObject
    {
        public abstract Task Execute(string pathToBuiltProject);

        [ContextMenu("Run")]
        private void Run()
        {
            if (EditorUtility.DisplayDialog($"Run {name} separately?", "This option must be used for the debug only. Running steps outside of the pipeline can cause unexpected behavior.", "Yes", "No"))
            {
                Debug.Log($"Running {name} separately.");

                try
                {
                    Execute(string.Empty).GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}