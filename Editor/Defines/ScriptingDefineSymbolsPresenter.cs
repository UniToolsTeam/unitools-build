using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public sealed class ScriptingDefineSymbolsPresenter
    {
        private readonly ScriptingDefineSymbols m_target = default;
        private readonly SerializedObject m_serializedObject = default;
        private readonly SerializedProperty m_defines = default;
        private bool m_foldout = false;
        private BuildTargetGroup m_group;

        public ScriptingDefineSymbolsPresenter(ScriptingDefineSymbols target)
        {
            m_target = target;
            m_serializedObject = new SerializedObject(m_target);
            m_defines = m_serializedObject.FindProperty(nameof(m_defines));

            m_group = EditorUserBuildSettings.selectedBuildTargetGroup;
        }

        public void Draw()
        {
            bool apply = false;
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

                    ScriptingDefineSymbolsEditor.Draw(m_target, m_serializedObject, m_defines);
                    EditorGUILayout.BeginHorizontal();
                    {
                        apply = GUILayout.Button("Apply For");
                        m_group = (BuildTargetGroup)EditorGUILayout.EnumPopup("", m_group);
                    }
                    EditorGUILayout.EndHorizontal();
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

            if (apply)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(m_group, m_target.ToString());

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }

            if (select)
            {
                EditorGUIUtility.PingObject(m_target);
            }
        }
    }
}