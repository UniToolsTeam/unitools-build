using UnityEditor;

namespace UniTools.Build.Android
{
    [CustomEditor(typeof(SetAlias))]
    public sealed class SetAliasEditor : Editor
    {
        private SerializedProperty m_password = default;
        private SerializedProperty m_alias = default;

        private bool m_showPassword = false;

        private void OnEnable()
        {
            m_password = serializedObject.FindProperty(nameof(m_password));
            m_alias = serializedObject.FindProperty(nameof(m_alias));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PropertyField(m_alias);

                if (m_showPassword)
                {
                    EditorGUILayout.PropertyField(m_password);
                }
                else
                {
                    m_password.stringValue = EditorGUILayout.PasswordField("Password", m_password.stringValue);
                }

                m_showPassword = EditorGUILayout.Toggle("Show", m_showPassword);
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}