using System.Threading.Tasks;
using UniTools.Defines;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ScriptableBuildStepWithDefines),
        menuName = nameof(UniTools) + "/Build/Steps/BuildWithDefines"
    )]
    public sealed class ScriptableBuildStepWithDefines : ScriptableBuildStep
    {
        [SerializeField] private ScriptableBuildStep m_successor = default;
        [SerializeField] private ScriptingDefineSymbols m_symbols = default;

        public override BuildTarget Target => m_successor.Target;

        public override async Task<BuildReport> Execute()
        {
            m_symbols.Apply(UnityEditor.BuildPipeline.GetBuildTargetGroup(Target));

            return await m_successor.Execute();
        }
    }
}