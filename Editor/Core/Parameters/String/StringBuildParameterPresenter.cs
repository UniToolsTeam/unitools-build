using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public sealed class StringBuildParameterPresenter : BuildParameterPresenter
    {
        private readonly StringBuildParameter m_target = default;
        private readonly SerializedObject m_serializedObject = default;
        private readonly SerializedProperty m_value = default;
        private readonly SerializedProperty m_options = default;

        private bool m_foldout = false;

        public StringBuildParameterPresenter(StringBuildParameter target)
        {
            m_target = target;
            m_serializedObject = new SerializedObject(m_target);
            m_value = m_serializedObject.FindProperty(nameof(m_value));
            m_options = m_serializedObject.FindProperty(nameof(m_options));
        }

        public override string CliKey => m_target.CliKey;

        public override void Draw(bool duplicated)
        {
            bool select = false;
            if (m_foldout)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        m_foldout = EditorGUILayout.Foldout(m_foldout, m_target.name, Styles.H2_Foldout);
                        select = GUILayout.Button("Select", GUILayout.Width(100));
                    }
                    EditorGUILayout.EndHorizontal();
                    if (duplicated)
                    {
                        EditorGUILayout.HelpBox("The parameter name is duplicated! Logical errors can happen during the batch mode call!", MessageType.Warning);
                    }

                    StringBuildParameterEditor.Draw(m_target, m_serializedObject, m_value, m_options);
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    Color color = GUI.color;

                    if (duplicated)
                    {
                        GUI.color = Color.red;
                    }

                    m_foldout = EditorGUILayout.Foldout(m_foldout, m_target.name, Styles.H2_Foldout);
                    select = GUILayout.Button("Select", GUILayout.Width(100));

                    GUI.color = color;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (select)
            {
                EditorGUIUtility.PingObject(m_target);
            }
        }
    }
}