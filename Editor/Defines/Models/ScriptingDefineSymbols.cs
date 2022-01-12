using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UniTools.Defines
{
    [CreateAssetMenu(
        fileName = nameof(ScriptingDefineSymbols),
        menuName = nameof(UniTools) + "/" + nameof(Defines) + "/" + nameof(ScriptingDefineSymbols)
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