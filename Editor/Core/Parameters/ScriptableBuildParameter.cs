using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniTools.Build
{
    /// <summary>
    /// Any value that can used inside a build pipeline and can be overriden from the command line
    /// </summary>
    public abstract class ScriptableBuildParameter<TValue> : ScriptableObject
    {
        [SerializeField] private TValue m_value = default;

        /// <summary>
        /// Is this collection if not empty the values can be selected as enum
        /// </summary>
        [SerializeField, Tooltip("Add values to this collection to create a popup")] private List<TValue> m_options = default;

        /// <summary>
        /// The name of the parameter that can be used inside a command line
        /// </summary>
        public string Name => $"--{name.ToLower()}";

        public TValue Value
        {
            get
            {
                if (Application.isBatchMode && TryParseFromCommandLine(Environment.CommandLine, out TValue v))
                {
                    return v;
                }

                return m_value;
            }
        }

        public List<TValue> Options => m_options;

        protected abstract bool TryParseFromCommandLine(string commandLine, out TValue v);
    }
}