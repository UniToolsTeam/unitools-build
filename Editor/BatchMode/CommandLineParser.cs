using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UniTools.Build
{
    public static class CommandLineParser
    {
        private const string KeyStart = "-";

        public static T Parse<T>(string commandLine) where T : class, new()
        {
            T t = new T();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string[] args = commandLine.Split(' ');
            for (int i = 0; i < args.Length; i++)
            {
                if (IsKey(args[i]))
                {
                    string key = args[i];
                    object val = null;
                    int next = i + 1;
                    if (next < args.Length && !IsKey(args[next]))
                    {
                        val = args[next];
                    }

                    key = key.Replace(KeyStart, string.Empty).ToLower();
                    if (parameters.ContainsKey(key))
                    {
                        Debug.LogWarning($"{nameof(CommandLineParser)}: duplication of the parameter {key}");
                        parameters[key] = val;
                    }
                    else
                    {
                        parameters.Add(key, val);
                    }
                }
            }

            FieldInfo[] fields =
                    t.GetType()
                     .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                     .Where(f => f.IsDefined(typeof(CommandLineParameterAttribute), false))
                     .ToArray()
                ;

            foreach (FieldInfo info in fields)
            {
                string fieldName = info.Name.ToLower();
                if (parameters.TryGetValue(fieldName, out object p))
                {
                    info.SetValue(t, p ?? true);
                }
            }

            return t;
        }

        private static bool IsKey(string param)
        {
            return !string.IsNullOrEmpty(param) && param.StartsWith(KeyStart);
        }
    }
}