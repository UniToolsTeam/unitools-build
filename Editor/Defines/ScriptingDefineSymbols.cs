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
    public sealed class ScriptingDefineSymbols : ScriptableObject
    {
        [SerializeField] private List<string> m_defines = new List<string>();

        /// <summary>
        /// The name of the parameter that can be used inside a command line
        /// </summary>
        public string CliKey => $"--{name.ToLower()}";

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
                builder.Append(m_defines[i]).Append(";");
            }

            builder.Append(m_defines[last]);

            return builder.ToString();
        }
    }
}