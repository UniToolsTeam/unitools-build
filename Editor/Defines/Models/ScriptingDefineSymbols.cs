using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ScriptingDefineSymbols),
        menuName = MenuPaths.Defines + "DefineSymbols"
    )]
    public sealed class ScriptingDefineSymbols : ScriptableObject
    {
        [SerializeField] private List<string> m_defines = new List<string>();

        public void Apply(BuildTargetGroup buildTargetGroup)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, ToString());
        }

        public override string ToString()
        {
            if (m_defines.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            int last = m_defines.Count - 1;

            for (int i = 0; i < last; i++)
            {
                builder.Append(m_defines[i]).Append(";");
            }

            builder.Append(m_defines[last]);

            return builder.ToString();
        }
    }
}