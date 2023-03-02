using System;
using System.Threading.Tasks;
using UnityEditor;
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

            m_symbols.Apply(m_group);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            await Task.CompletedTask;
        }
    }
}