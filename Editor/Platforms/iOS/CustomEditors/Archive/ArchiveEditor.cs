using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomEditor(typeof(Archive), true)]
    public sealed class ArchiveEditor : Editor
    {
        private SerializedProperty m_teamId = default;
        private SerializedProperty m_projectPath = default;
        private SerializedProperty m_outputPath = default;
        private SerializedProperty m_scheme = default;
        private SerializedProperty m_destination = default;
        private SerializedProperty m_useModernBuildSystem = default;
        private SerializedProperty m_enableBitcode = default;
        private SerializedProperty m_overrideProvisioningProfile = default;
        private SerializedProperty m_provisioningProfileUuid = default;
        private SerializedProperty m_extraArgs = default;

        private void OnEnable()
        {
            m_teamId = serializedObject.FindProperty(nameof(m_teamId));
            m_projectPath = serializedObject.FindProperty(nameof(m_projectPath));
            m_outputPath = serializedObject.FindProperty(nameof(m_outputPath));
            m_scheme = serializedObject.FindProperty(nameof(m_scheme));
            m_destination = serializedObject.FindProperty(nameof(m_destination));
            m_useModernBuildSystem = serializedObject.FindProperty(nameof(m_useModernBuildSystem));
            m_enableBitcode = serializedObject.FindProperty(nameof(m_enableBitcode));
            m_overrideProvisioningProfile = serializedObject.FindProperty(nameof(m_overrideProvisioningProfile));
            m_provisioningProfileUuid = serializedObject.FindProperty(nameof(m_provisioningProfileUuid));
            m_extraArgs = serializedObject.FindProperty(nameof(m_extraArgs));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Make sure that certificate added to the Keychain. The archive will be failed without the certificate.", MessageType.Info);

            EditorGUILayout.PropertyField(m_projectPath);
            EditorGUILayout.PropertyField(m_outputPath);

            bool parseTeamId = false;
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.HelpBox("Team Id can be parsed from the Provisioning Profile file.", MessageType.Info);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PropertyField(m_teamId);
                    parseTeamId = GUILayout.Button("From file");
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(m_scheme);
            EditorGUILayout.PropertyField(m_destination);
            EditorGUILayout.PropertyField(m_useModernBuildSystem);
            EditorGUILayout.PropertyField(m_enableBitcode);
            EditorGUILayout.PropertyField(m_overrideProvisioningProfile);

            bool parseprovisioningProfileUuid = false;
            if (m_overrideProvisioningProfile.boolValue)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.HelpBox("Provisioning Profile must be overridden if xCode project was created without a Provisioning Profile. " +
                        "Try SetManualProvisionProfile step to set it during the build pipeline.\n" +
                        "NOTE: Provisioning Profile can be parsed from the  file.", MessageType.Info);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.PropertyField(m_provisioningProfileUuid);
                        parseprovisioningProfileUuid = GUILayout.Button("From file");
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.PropertyField(m_extraArgs);

            if (parseprovisioningProfileUuid)
            {
                string path = EditorUtility.OpenFilePanel("Test", Application.dataPath, "mobileprovision");
                ProvisioningProfile pp = ProvisioningProfile.Load(path);
                m_provisioningProfileUuid.stringValue = pp.Uuid;
            }

            if (parseTeamId)
            {
                string path = EditorUtility.OpenFilePanel("Test", Application.dataPath, "mobileprovision");
                ProvisioningProfile pp = ProvisioningProfile.Load(path);
                m_teamId.stringValue = pp.TeamIdentifier;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}