using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ScriptableBuildStepWithDefines),
        menuName = MenuPaths.Defines + "BuildWithDefines"
    )]
    public sealed class ScriptableBuildStepWithDefines : ScriptableBuildStep
    {
        [SerializeField] private ScriptableBuildStep m_successor = default;
        [SerializeField] private ScriptingDefineSymbols m_symbols = default;

        public override BuildTarget Target => m_successor.Target;

        public override async Task Execute()
        {
            m_symbols.Apply(UnityEditor.BuildPipeline.GetBuildTargetGroup(Target));

            await m_successor.Execute();
        }
    }
}