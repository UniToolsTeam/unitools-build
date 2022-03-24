using UnityEditor;

namespace UniTools.Build
{
    [CustomEditor(typeof(EnsureDirectoryExists))]
    public sealed class EnsureDirectoryExistsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField(target.ToString());
            serializedObject.ApplyModifiedProperties();
        }
    }
}