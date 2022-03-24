using UnityEditor;

namespace UniTools.Build
{
    [CustomEditor(typeof(EnsureDirectoryEmpty))]
    public sealed class EnsureDirectoryEmptyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField(target.ToString());
            serializedObject.ApplyModifiedProperties();
        }
    }
}