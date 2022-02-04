using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public sealed class ScriptingDefineSymbolsPresenter
    {
        private readonly BuildTargetGroup m_target = default;
        private readonly ScriptingDefineSymbols[] m_defines = default;
        private readonly string[] m_names = default;
        private ScriptingDefineSymbols m_current = default;
        private string m_currentSymbols = default;
        private int m_index = 0;
        private bool m_foldout = false;

        public ScriptingDefineSymbolsPresenter(
            BuildTargetGroup target,
            IEnumerable<ScriptingDefineSymbols> defines)
        {
            m_target = target;
            m_defines = defines.ToArray();
            m_names = m_defines.Select(d => d.name).ToArray();
            Refresh();
        }

        public void Draw()
        {
            if (m_current == null)
            {
                EditorGUILayout.HelpBox("No any defines set is applied for this platform!", MessageType.Warning);
            }

            m_foldout = EditorGUILayout.Foldout(m_foldout, m_current != null ? $"Applied set is {m_current.name}" : "Custom defines");
            if (m_foldout)
            {
                EditorGUILayout.LabelField(m_currentSymbols);
            }

            bool apply = false;
            bool select = false;
            EditorGUILayout.BeginHorizontal("box");
            {
                m_index = EditorGUILayout.Popup(m_index, m_names);
                GUI.enabled = m_current == null || !m_current.Equals(m_defines[m_index]);
                apply = GUILayout.Button("Apply", GUILayout.Width(100));
                GUI.enabled = true;
                select = GUILayout.Button("Select", GUILayout.Width(100));
            }
            EditorGUILayout.EndHorizontal();

            if (select)
            {
                EditorGUIUtility.PingObject(m_defines[m_index]);
            }

            if (apply)
            {
                apply = EditorUtility.DisplayDialog("Are you sure?", $"Defines set for {m_target.ToString()} will be changed to {m_defines[m_index].name}.", "Yes", "No");
            }

            if (apply)
            {
                m_defines[m_index].Apply(m_target);
                Refresh();
            }
        }

        private void Refresh()
        {
            m_currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(m_target);
            for (int i = 0; i < m_defines.Length; i++)
            {
                if (m_defines[i].ToString().Equals(m_currentSymbols))
                {
                    m_index = i;
                    m_current = m_defines[i];
                    m_currentSymbols = m_current.ToString();

                    break;
                }
            }
        }
    }
}