using UnityEditor;

namespace UniTools.Build.iOS
{
    [CustomEditor(typeof(UploadToAppStore))]
    public sealed class UploadToAppStoreEditor : DistributeIosApplicationStepEditor
    {
        private SerializedProperty m_archivePath = default;
        private SerializedProperty m_outputPath = default;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_archivePath = serializedObject.FindProperty(nameof(m_archivePath));
            m_outputPath = serializedObject.FindProperty(nameof(m_outputPath));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(m_archivePath);
            EditorGUILayout.PropertyField(m_outputPath);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
