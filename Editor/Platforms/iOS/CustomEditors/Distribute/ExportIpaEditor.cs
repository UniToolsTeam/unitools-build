using UnityEditor;

namespace UniTools.Build.iOS
{
    
    [CustomEditor(typeof(ExportIpa))]
    public sealed class ExportIpaEditor : DistributeIosApplicationStepEditor
    {
        private SerializedProperty m_method = default;
        private SerializedProperty m_archivePath = default;
        private SerializedProperty m_outputPath = default;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_method = serializedObject.FindProperty(nameof(m_method));
            m_archivePath = serializedObject.FindProperty(nameof(m_archivePath));
            m_outputPath = serializedObject.FindProperty(nameof(m_outputPath));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(m_method);
            EditorGUILayout.PropertyField(m_archivePath);
            EditorGUILayout.PropertyField(m_outputPath);
            serializedObject.ApplyModifiedProperties();
        }
    }
}