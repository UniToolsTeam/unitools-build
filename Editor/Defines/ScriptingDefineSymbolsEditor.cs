using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomEditor(typeof(ScriptingDefineSymbols))]
    public sealed class ScriptingDefineSymbolsEditor : Editor
    {
        private ScriptingDefineSymbols m_target = default;

        private void OnEnable()
        {
            m_target = target as ScriptingDefineSymbols;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            bool copyToClipboard = false;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField($"CLI: {m_target.CliKey} \"{m_target.ToString()}\"");
                copyToClipboard = GUILayout.Button("Copy");
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

            if (copyToClipboard)
            {
                EditorGUIUtility.systemCopyBuffer = $"{m_target.CliKey} \"{m_target.ToString()}\"";
            }
        }
    }
}