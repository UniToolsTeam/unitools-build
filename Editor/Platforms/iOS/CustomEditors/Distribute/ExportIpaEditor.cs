using UnityEditor;

namespace UniTools.Build
{
    [CustomEditor(typeof(ExportIpa))]
    public sealed class ExportIpaEditor : DistributeIosApplicationStepEditor
    {
        private SerializedProperty m_method = default;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_method = serializedObject.FindProperty(nameof(m_method));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(m_method);

            serializedObject.ApplyModifiedProperties();
        }
    }
}