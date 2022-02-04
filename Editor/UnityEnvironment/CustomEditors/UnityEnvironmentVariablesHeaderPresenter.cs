using UnityEditor;

namespace UniTools.Build
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