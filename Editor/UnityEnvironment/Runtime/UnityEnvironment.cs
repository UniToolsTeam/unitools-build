using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UniTools.Build
{
    public static class UnityEnvironment
    {
        private static List<UnityEnvironmentVariableModel> m_variables = null;

        static UnityEnvironment()
        {
            List<PathVariableAttribute> declared = ReflectionHelper.Find<PathVariableAttribute>().ToList();

            foreach (PathVariableAttribute attribute in declared)
            {
                ModifyPath(attribute.Value);
            }
        }

        public static IEnumerable<UnityEnvironmentVariableModel> Variables
        {
            get
            {
                if (m_variables == null)
                {
                    m_variables = new List<UnityEnvironmentVariableModel>();
                    foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
                    {
                        m_variables.Add(new UnityEnvironmentVariableModel(entry.Key.ToString(), entry.Value.ToString()));
                    }
                }

                return m_variables;
            }
        }

        public static UnityEnvironmentVariableModel Get(string name)
        {
            return Variables.FirstOrDefault(v => v.Name.Equals(name, System.IO.Path.PathSeparator == ';' ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }

        private static void ModifyPath(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue)) return;
            const string pathName = "PATH";
            string path = Environment.GetEnvironmentVariable(pathName) ?? string.Empty;
            char separator = System.IO.Path.PathSeparator;
            StringComparison comparison = separator == ';' ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (path.Split(separator).Any(entry => string.Equals(entry, newValue, comparison))) return;
            Environment.SetEnvironmentVariable(pathName, string.IsNullOrEmpty(path) ? newValue : path + separator + newValue);
            m_variables = null;
        }
    }
}
