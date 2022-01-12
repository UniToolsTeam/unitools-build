using UnityEditor;

namespace UniTools.Build.iOS
{
    [CustomEditor(typeof(Archive))]
    public sealed class ArchiveEditor : IosPostBuildStepEditor
    {
        private SerializedProperty m_projectPath = default;
        private SerializedProperty m_outputPath = default;
        private SerializedProperty m_scheme = default;
        private SerializedProperty m_useModernBuildSystem = default;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_projectPath = serializedObject.FindProperty(nameof(m_projectPath));
            m_outputPath = serializedObject.FindProperty(nameof(m_outputPath));
            m_scheme = serializedObject.FindProperty(nameof(m_scheme));
            m_useModernBuildSystem = serializedObject.FindProperty(nameof(m_useModernBuildSystem));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Use Development Provisioning Profile for the Archive step.", MessageType.Info);

            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(m_projectPath);
            EditorGUILayout.PropertyField(m_outputPath);
            EditorGUILayout.PropertyField(m_scheme);
            EditorGUILayout.PropertyField(m_useModernBuildSystem);

            serializedObject.ApplyModifiedProperties();
        }
    }
}