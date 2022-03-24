using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ScriptableBuildStepWithDefines),
        menuName = MenuPaths.Defines + "BuildWithDefines"
    )]
    public sealed class ScriptableBuildStepWithDefines : ScriptablePlatformBuildStep
    {
        [SerializeField] private ScriptablePlatformBuildStep m_successor = default;
        [SerializeField] private ScriptingDefineSymbols m_symbols = default;

        public override BuildTarget Target => m_successor.Target;

        public override async Task<BuildReport> Execute()
        {
            m_symbols.Apply(UnityEditor.BuildPipeline.GetBuildTargetGroup(Target));

            return await m_successor.Execute();
        }
    }
}