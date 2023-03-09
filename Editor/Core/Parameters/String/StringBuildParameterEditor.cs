using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomEditor(typeof(StringBuildParameter))]
    public sealed class StringBuildParameterEditor : Editor
    {
        private StringBuildParameter m_target = default;
        private SerializedProperty m_value = default;
        private SerializedProperty m_options = default;

        private void OnEnable()
        {
            m_value = serializedObject.FindProperty(nameof(m_value));
            m_options = serializedObject.FindProperty(nameof(m_options));

            m_target = target as StringBuildParameter;
        }

        public override void OnInspectorGUI()
        {
            bool copyToClipboard = false;

            if (m_target.Options == null || m_target.Options.Count <= 0)
            {
                EditorGUILayout.PropertyField(m_value);
            }
            else
            {
                int index = m_target.Options.IndexOf(m_value.stringValue);
                index = EditorGUILayout.Popup(index, m_target.Options.Select(o => o.ToString()).ToArray());

                if (index >= 0 && index <= m_target.Options.Count)
                {
                    m_value.stringValue = m_target.Options[index];
                }
            }

            EditorGUILayout.PropertyField(m_options);

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