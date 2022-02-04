using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public sealed class CliToolEditorPresenter
    {
        private readonly BaseCliTool m_model = default;
        private readonly string m_name = default;
        private readonly string m_version = string.Empty;
        private readonly string m_helpLink = string.Empty;

        public CliToolEditorPresenter(BaseCliTool model)
        {
            m_model = model;
            m_name = model.GetType().Name;
            if (model is ICliToolFriendlyName name)
            {
                m_name = name.Name;
            }


            if (model.IsInstalled)
            {
                if (model is ICliToolVersion version)
                {
                    m_version = version.Version;
                }
            }

            if (model is ICliToolHelpLink help)
            {
                m_helpLink = help.Link;
            }
        }

        private bool m_foldout = false;
        private const int MinHeight = 25;

        public void Draw()
        {
            if (!m_model.IsInstalled)
            {
                Color c = GUI.color;
                bool help = false;
                EditorGUILayout.BeginHorizontal("box");
                {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField($"{m_name} is not found!", GUILayout.MinHeight(MinHeight));
                    GUI.color = c;

                    if (!string.IsNullOrEmpty(m_helpLink))
                    {
                        help = GUILayout.Button("Help", GUILayout.Width(100));
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (help)
                {
                    Application.OpenURL(m_helpLink);
                }

                return;
            }

            if (m_foldout)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    m_foldout = EditorGUILayout.Foldout(m_foldout, m_name);
                    EditorGUILayout.LabelField($"{nameof(BaseCliTool.Path)}: {m_model.Path}", GUILayout.MinHeight(MinHeight));

                    if (!string.IsNullOrEmpty(m_version))
                    {
                        EditorGUILayout.LabelField($"{nameof(ICliToolVersion.Version)}: {m_version}", GUILayout.MinHeight(MinHeight));
                    }
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    m_foldout = EditorGUILayout.Foldout(m_foldout, m_name);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}