using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomEditor(typeof(SetManualProvisionProfile))]
    public sealed class SetManualProvisionProfileEditor : Editor
    {
        private SerializedProperty m_name = default;
        private SerializedProperty m_id = default;
        private SerializedProperty m_type = default;

        private void OnEnable()
        {
            m_name = serializedObject.FindProperty(nameof(m_name));
            m_id = serializedObject.FindProperty(nameof(m_id));
            m_type = serializedObject.FindProperty(nameof(m_type));
        }

        public override void OnInspectorGUI()
        {
            bool load = false;

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.HelpBox("You can parse Name and Id from the Provisioning Profile file", MessageType.Info);
                    load = GUILayout.Button("From file");
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.PropertyField(m_name);
                EditorGUILayout.PropertyField(m_id);
                EditorGUILayout.PropertyField(m_type);
            }
            EditorGUILayout.EndVertical();

            if (load)
            {
                string path = EditorUtility.OpenFilePanel("Test", Application.dataPath, "mobileprovision");

                ProvisioningProfile pp = ProvisioningProfile.Load(path);

                m_id.stringValue = pp.Uuid;
                m_name.stringValue = pp.Name;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}