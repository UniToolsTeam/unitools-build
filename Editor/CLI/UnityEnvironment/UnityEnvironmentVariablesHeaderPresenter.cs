using UnityEditor;

namespace UniTools.CLI.Editor
{
    public sealed class UnityEnvironmentVariablesHeaderPresenter
    {
        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Name");
                EditorGUILayout.LabelField("Value");
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}