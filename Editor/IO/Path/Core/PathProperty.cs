using System;
using UnityEngine;

namespace UniTools.Build
{
    [Serializable]
    public sealed class PathProperty
    {
        [SerializeField] private PathTypes m_type = PathTypes.String;
        [SerializeField] private string m_path = default;
        [SerializeField] private BaseScriptablePath m_scriptablePath = default;

        private PathProperty()
        {
        }

        public PathProperty(string path)
        {
            m_path = path;
        }

        public override string ToString()
        {
            if (m_type == PathTypes.Scriptable)
            {
                if (m_scriptablePath == null)
                {
                    return string.Empty;
                }

                return m_scriptablePath.ToString();
            }

            return m_path ?? string.Empty;
        }

        internal static string RequirePath(PathProperty property, string label)
        {
            string path = property?.ToString();
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(label + " is not assigned.");
            return System.IO.Path.GetFullPath(path);
        }

        internal static void ValidateCopyPaths(string source, string destination)
        {
            StringComparison comparison = System.IO.Path.DirectorySeparatorChar == '\\'
                ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            char separator = System.IO.Path.DirectorySeparatorChar;
            source = System.IO.Path.GetFullPath(source).TrimEnd(separator);
            destination = System.IO.Path.GetFullPath(destination).TrimEnd(separator);
            if (string.Equals(source, destination, comparison)
                || source.StartsWith(destination + separator, comparison)
                || destination.StartsWith(source + separator, comparison))
                throw new ArgumentException("Copy source and destination must not overlap.");
        }
    }
}
