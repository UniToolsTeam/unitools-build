using UnityEditor;
using UnityEngine;

namespace UniTools.IO
{
    [CustomEditor(typeof(CompositeScriptablePath))]
    public sealed class CompositeScriptablePathEditor : Editor
    {
        private SerializedProperty m_parent = default;
        private SerializedProperty m_value = default;

        private void OnEnable()
        {
            m_parent = serializedObject.FindProperty(nameof(m_parent));
            m_value = serializedObject.FindProperty(nameof(m_value));
        }

        public override void OnInspectorGUI()
        {
            Color c = GUI.color;

            if (m_parent.objectReferenceValue != null)
            {
                EditorGUILayout.LabelField(target.ToString());
            }

            if (m_parent.objectReferenceValue == null)
            {
                GUI.color = Color.red;
            }

            EditorGUILayout.PropertyField(m_parent);
            GUI.color = c;
            EditorGUILayout.PropertyField(m_value);

            serializedObject.ApplyModifiedProperties();
        }
    }
}