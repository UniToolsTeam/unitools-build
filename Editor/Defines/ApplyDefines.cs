using System;
using System.Threading.Tasks;
using UnityEditor;
#if UNITY_2021_2_OR_NEWER
using UnityEditor.Build;
#endif
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ApplyDefines),
        menuName = MenuPaths.Defines + "ApplyDefines"
    )]
    public sealed class ApplyDefines : BuildStep
    {
        [SerializeField] private ScriptingDefineSymbols m_symbols = default;
        [SerializeField] private BuildTargetGroup m_group = default;

        public override async Task Execute()
        {
            if (m_symbols == null)
            {
                throw new Exception("Invalid define symbols added!");
            }

#if UNITY_2021_2_OR_NEWER
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(m_group), m_symbols.ToString());
#else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(m_group, m_symbols.ToString());
#endif

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}
