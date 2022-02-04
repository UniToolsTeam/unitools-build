using UnityEditor;

namespace UniTools.Build
{
    public sealed class UnityEnvironmentVariableEditorPresenter
    {
        public void Draw(UnityEnvironmentVariableModel model)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(model.Name);
                EditorGUILayout.LabelField(model.Value);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}