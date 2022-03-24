using UnityEditor;

namespace UniTools.Build
{
    [CustomEditor(typeof(ApplicationVersion))]
    public sealed class ApplicationVersionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(target.ToString());
        }
    }
}