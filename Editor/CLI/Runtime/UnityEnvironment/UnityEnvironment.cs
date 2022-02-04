using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UniTools.CLI
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
            return Variables.FirstOrDefault(v => v.Name.Equals(name));
        }

        private static void ModifyPath(string newValue)
        {
            const string pathName = "PATH";

            string path = Environment.GetEnvironmentVariable(pathName);
            if (path.Contains(newValue))
            {
                return;
            }

#if UNITY_EDITOR_OSX
            Environment.SetEnvironmentVariable(pathName, $"{path}:{newValue}"); 
#elif UNITY_EDITOR_WIN
            Environment.SetEnvironmentVariable(pathName, $"{path}{newValue}");
#endif
            m_variables = null;

        }
    }
}