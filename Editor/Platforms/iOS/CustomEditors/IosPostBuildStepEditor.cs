using System;
using UnityEditor;
using UnityEngine;

namespace UniTools.Build.iOS
{
    public abstract class IosPostBuildStepEditor : Editor
    {
        private SerializedProperty m_teamId = default;
        private SerializedProperty m_provisioningProfileName = default;
        private SerializedProperty m_provisioningProfileUuid = default;

        protected virtual void OnEnable()
        {
            m_teamId = serializedObject.FindProperty(nameof(m_teamId));
            m_provisioningProfileName = serializedObject.FindProperty(nameof(m_provisioningProfileName));
            m_provisioningProfileUuid = serializedObject.FindProperty(nameof(m_provisioningProfileUuid));
        }

        public override void OnInspectorGUI()
        {

            EditorGUILayout.HelpBox("Make sure that certificate added to the Keychain", MessageType.Info);
            bool load = false;

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Provisioning Profile:");
                    load = GUILayout.Button("Load");
                }
                EditorGUILayout.EndHorizontal();
                GUI.enabled = false;
                EditorGUILayout.PropertyField(m_teamId);
                EditorGUILayout.PropertyField(m_provisioningProfileName);
                EditorGUILayout.PropertyField(m_provisioningProfileUuid);
                GUI.enabled = true;
            }
            EditorGUILayout.EndVertical();

            if (load)
            {
                string path = EditorUtility.OpenFilePanel("Test", Application.dataPath, "mobileprovision");

                ProvisioningProfile pp = ProvisioningProfile.Load(path);

                m_teamId.stringValue = pp.TeamIdentifier;
                m_provisioningProfileUuid.stringValue = pp.Uuid;
                m_provisioningProfileName.stringValue = pp.Name;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
