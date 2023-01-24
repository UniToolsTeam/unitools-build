using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public abstract class DistributeIosApplicationStepEditor : Editor
    {
        private SerializedProperty m_teamId = default;
        private SerializedProperty m_provisioningProfileName = default;
        private SerializedProperty m_bundleIdentifier = default;
        private SerializedProperty m_uploadBitcode = default;
        private SerializedProperty m_uploadSymbols = default;
        private SerializedProperty m_pathToXCodeProject = default;
        private SerializedProperty m_archivePath = default;
        private SerializedProperty m_outputPath = default;

        protected virtual void OnEnable()
        {
            m_teamId = serializedObject.FindProperty(nameof(m_teamId));
            m_provisioningProfileName = serializedObject.FindProperty(nameof(m_provisioningProfileName));
            m_bundleIdentifier = serializedObject.FindProperty(nameof(m_bundleIdentifier));
            m_uploadBitcode = serializedObject.FindProperty(nameof(m_uploadBitcode));
            m_uploadSymbols = serializedObject.FindProperty(nameof(m_uploadSymbols));
            m_pathToXCodeProject = serializedObject.FindProperty(nameof(m_pathToXCodeProject));
            m_archivePath = serializedObject.FindProperty(nameof(m_archivePath));
            m_outputPath = serializedObject.FindProperty(nameof(m_outputPath));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_pathToXCodeProject);
            EditorGUILayout.PropertyField(m_archivePath);
            EditorGUILayout.PropertyField(m_outputPath);

            bool useCurrentBundle = false;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(m_bundleIdentifier);
                useCurrentBundle = GUILayout.Button("From Player Settings");
            }
            EditorGUILayout.EndHorizontal();

            bool parseTeamId = false;
            bool parseProvisioningProfileName = false;
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.HelpBox("Team Id and Provisioning Profile can be parsed from the Provisioning Profile file.", MessageType.Info);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PropertyField(m_teamId);
                    parseTeamId = GUILayout.Button("From file");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PropertyField(m_provisioningProfileName);
                    parseProvisioningProfileName = GUILayout.Button("From file");
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(m_uploadBitcode);
            EditorGUILayout.PropertyField(m_uploadSymbols);

            if (useCurrentBundle)
            {
                m_bundleIdentifier.stringValue = PlayerSettings.applicationIdentifier;
            }

            if (parseTeamId)
            {
                string path = EditorUtility.OpenFilePanel("Test", Application.dataPath, "mobileprovision");
                ProvisioningProfile pp = ProvisioningProfile.Load(path);
                m_teamId.stringValue = pp.TeamIdentifier;
            }

            if (parseProvisioningProfileName)
            {
                string path = EditorUtility.OpenFilePanel("Test", Application.dataPath, "mobileprovision");
                ProvisioningProfile pp = ProvisioningProfile.Load(path);
                m_provisioningProfileName.stringValue = pp.Name;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}