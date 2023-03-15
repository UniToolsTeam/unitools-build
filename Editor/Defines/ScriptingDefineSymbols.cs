using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UniTools.Build
{
    [CreateAssetMenu(
        fileName = nameof(ScriptingDefineSymbols),
        menuName = MenuPaths.Defines + "DefineSymbols"
    )]
    public sealed class ScriptingDefineSymbols : ScriptableBuildParameter
    {
        [Serializable]
        public sealed class DefineSymbol
        {
            public string Value = "MY_DEFINE";
            public bool Enabled = true;
        }

        [SerializeField] private List<DefineSymbol> m_defines = new List<DefineSymbol>();
        public override string CliCommand => $"{CliKey} \"{ToString()}\"";

        public override string ToString()
        {
            //in case 
            if (Application.isBatchMode && CommandLineParser.TryToParseValue(Environment.CommandLine, CliKey, out object obj))
            {
                return obj.ToString();
            }

            if (m_defines.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            int last = m_defines.Count - 1;

            for (int i = 0; i < last; i++)
            {
                if (m_defines[i].Enabled)
                {
                    builder.Append(m_defines[i].Value).Append(";");
                }
            }

            if (m_defines[last].Enabled)
            {
                builder.Append(m_defines[last].Value);
            }

            return builder.ToString();
        }
    }
}