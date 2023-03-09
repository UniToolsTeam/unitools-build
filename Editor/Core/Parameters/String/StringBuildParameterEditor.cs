using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomEditor(typeof(StringBuildParameter))]
    public sealed class StringBuildParameterEditor : Editor
    {
        private StringBuildParameter m_target = default;
        private SerializedProperty m_value = default;

        private void OnEnable()
        {
            m_value = serializedObject.FindProperty(nameof(m_value));
            m_target = target as StringBuildParameter;
        }

        public override void OnInspectorGUI()
        {
            bool copyToClipboard = false;

            EditorGUILayout.PropertyField(m_value);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField($"CLI: {m_target.Name} {m_value.stringValue}");
                copyToClipboard = GUILayout.Button("Copy");
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

            if (copyToClipboard)
            {
                EditorGUIUtility.systemCopyBuffer = $"{m_target.Name} {m_value.stringValue}";
            }
        }
    }
}