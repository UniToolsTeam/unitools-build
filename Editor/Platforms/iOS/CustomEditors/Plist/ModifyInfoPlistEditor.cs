using UnityEditor;

namespace UniTools.Build.iOS
{
    [CustomEditor(typeof(ModifyInfoPlist))]
    public sealed class ModifyInfoPlistEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Values below will be added to the root dictionary", MessageType.Info);
            base.OnInspectorGUI();
        }
    }
}
