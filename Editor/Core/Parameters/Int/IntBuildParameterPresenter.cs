using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public sealed class IntBuildParameterPresenter : BuildParameterPresenter
    {
        private readonly IntBuildParameter m_target = default;
        private readonly SerializedObject m_serializedObject = default;
        private readonly SerializedProperty m_value = default;
        private readonly SerializedProperty m_options = default;
        private bool m_foldout = false;

        public IntBuildParameterPresenter(IntBuildParameter target)
        {
            m_target = target;
            m_serializedObject = new SerializedObject(m_target);
            m_value = m_serializedObject.FindProperty(nameof(m_value));
            m_options = m_serializedObject.FindProperty(nameof(m_options));
        }

        public override void Draw()
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

                    IntBuildParameterEditor.Draw(m_target, m_serializedObject, m_value, m_options);
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    m_foldout = EditorGUILayout.Foldout(m_foldout, m_target.name, Styles.H2_Foldout);
                    select = GUILayout.Button("Select", GUILayout.Width(100));
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