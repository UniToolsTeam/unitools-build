using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class ScriptableBuildStep : ScriptableObject
    {
        public abstract BuildTarget Target { get; }

        public abstract Task<BuildReport> Execute();
    }
}