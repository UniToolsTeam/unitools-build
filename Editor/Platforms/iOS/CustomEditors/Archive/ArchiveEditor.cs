using UnityEditor;

namespace UniTools.Build
{
    [CustomEditor(typeof(Archive), true)]
    public sealed class ArchiveEditor : IosBuildStepEditor
    {
        private SerializedProperty m_projectPath = default;
        private SerializedProperty m_outputPath = default;
        private SerializedProperty m_scheme = default;
        private SerializedProperty m_useModernBuildSystem = default;
        private SerializedProperty m_enableBitcode = default;
        private SerializedProperty m_overrideTeamId = default;
        private SerializedProperty m_overrideProvisioningProfile = default;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_projectPath = serializedObject.FindProperty(nameof(m_projectPath));
            m_outputPath = serializedObject.FindProperty(nameof(m_outputPath));
            m_scheme = serializedObject.FindProperty(nameof(m_scheme));
            m_useModernBuildSystem = serializedObject.FindProperty(nameof(m_useModernBuildSystem));
            m_enableBitcode = serializedObject.FindProperty(nameof(m_enableBitcode));

            m_overrideTeamId = serializedObject.FindProperty(nameof(m_overrideTeamId));
            m_overrideProvisioningProfile = serializedObject.FindProperty(nameof(m_overrideProvisioningProfile));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Use Development Provisioning Profile for the Archive step.", MessageType.Info);

            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(m_projectPath);
            EditorGUILayout.PropertyField(m_outputPath);
            EditorGUILayout.PropertyField(m_scheme);
            EditorGUILayout.PropertyField(m_useModernBuildSystem);
            EditorGUILayout.PropertyField(m_enableBitcode);
            EditorGUILayout.PropertyField(m_overrideTeamId);
            EditorGUILayout.PropertyField(m_overrideProvisioningProfile);
            serializedObject.ApplyModifiedProperties();
        }
    }
}