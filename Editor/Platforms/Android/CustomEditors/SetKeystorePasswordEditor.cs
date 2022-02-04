using UnityEditor;

namespace UniTools.Build.Android
{
    [CustomEditor(typeof(SetKeystorePassword))]
    public sealed class SetKeystorePasswordEditor : Editor
    {
        private SerializedProperty m_password = default;
        private bool m_showPassword = false;

        private void OnEnable()
        {
            m_password = serializedObject.FindProperty(nameof(m_password));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            {
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