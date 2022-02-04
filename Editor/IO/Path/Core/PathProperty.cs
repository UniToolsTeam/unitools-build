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

            return m_path;
        }
    }
}