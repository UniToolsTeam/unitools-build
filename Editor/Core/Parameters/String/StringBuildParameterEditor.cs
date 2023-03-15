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
            Draw(m_target, serializedObject, m_value, m_options);
        }

        /// <summary>
        /// This method is using to draw the same editor in different windows 
        /// </summary>
        public static void Draw(StringBuildParameter target, SerializedObject serializedObject, SerializedProperty value, SerializedProperty options)
        {
            bool copyToClipboard = false;

            if (target.Options == null || target.Options.Count <= 0)
            {
                EditorGUILayout.PropertyField(value);
            }
            else
            {
                int index = target.Options.IndexOf(value.stringValue);
                index = EditorGUILayout.Popup(index, target.Options.Select(o => o.ToString()).ToArray());

                if (index >= 0 && index <= target.Options.Count)
                {
                    value.stringValue = target.Options[index];
                }
            }

            EditorGUILayout.PropertyField(options);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField($"CLI: {target.CliCommand}");
                copyToClipboard = GUILayout.Button("Copy");
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();

            if (copyToClipboard)
            {
                EditorGUIUtility.systemCopyBuffer = $"{target.CliCommand}";
            }
        }
    }
}