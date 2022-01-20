using UnityEditor;

namespace UniTools.IO
{
    [CustomEditor(typeof(CompositeScriptablePath))]
    public sealed class CompositeScriptablePathEditor : Editor
    {
        private SerializedProperty m_paths = default;

        private void OnEnable()
        {
            m_paths = serializedObject.FindProperty(nameof(m_paths));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(target.ToString());
            EditorGUILayout.PropertyField(m_paths);
            serializedObject.ApplyModifiedProperties();
        }
    }
}