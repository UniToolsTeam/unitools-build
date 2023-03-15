using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomEditor(typeof(ScriptingDefineSymbols))]
    public sealed class ScriptingDefineSymbolsEditor : Editor
    {
        private ScriptingDefineSymbols m_target = default;
        private SerializedProperty m_defines = default;

        private void OnEnable()
        {
            m_defines = serializedObject.FindProperty(nameof(m_defines));
            m_target = target as ScriptingDefineSymbols;
        }

        public override void OnInspectorGUI()
        {
            Draw(m_target, serializedObject, m_defines);
        }

        public static void Draw(ScriptingDefineSymbols target, SerializedObject serializedObject, SerializedProperty defines)
        {
            EditorGUILayout.PropertyField(defines);
            bool copyToClipboard = false;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField($"CLI: {target.CliKey} \"{target.ToString()}\"");
                copyToClipboard = GUILayout.Button("Copy");
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

            if (copyToClipboard)
            {
                EditorGUIUtility.systemCopyBuffer = $"{target.CliKey} \"{target.ToString()}\"";
            }
        }
    }
}