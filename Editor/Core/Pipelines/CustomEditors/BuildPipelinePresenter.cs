using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public sealed class BuildPipelinePresenter
    {
        private readonly BuildPipeline m_buildPipeline = default;
        private bool m_foldout = false;

        public BuildPipelinePresenter(BuildPipeline buildPipeline)
        {
            m_buildPipeline = buildPipeline;
        }

        public void Draw()
        {
            bool run = false;
            bool select = false;
            if (m_foldout)
            {
                string sh = $"./build.sh --pipeline {m_buildPipeline.name}";
                string ps = $".\\build.ps1 --pipeline {m_buildPipeline.name}";
                bool copySh = false;
                bool copyPs = false;

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        m_foldout = EditorGUILayout.Foldout(m_foldout, m_buildPipeline.name);
                        run = GUILayout.Button("Run", GUILayout.Width(100));
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("CLI commands:");
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"SH: {sh}");
                        copySh = GUILayout.Button("Copy");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"PS: {ps}");
                        copyPs = GUILayout.Button("Copy");
                    }
                    EditorGUILayout.EndHorizontal();

                    select = GUILayout.Button("Select Pipeline", GUILayout.Width(250));

                    if (copySh)
                    {
                        EditorGUIUtility.systemCopyBuffer = sh;
                    }

                    if (copyPs)
                    {
                        EditorGUIUtility.systemCopyBuffer = ps;
                    }
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    m_foldout = EditorGUILayout.Foldout(m_foldout, m_buildPipeline.name);
                    run = GUILayout.Button("Run", GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (run)
            {
                if (EditorUtility.DisplayDialog($"Run {m_buildPipeline.name} pipeline?", "", "Yes", "No"))
                {
                    m_buildPipeline.Run().GetAwaiter().OnCompleted(() => { Debug.Log($"Pipeline {m_buildPipeline} is completed!"); });
                }
            }

            if (select)
            {
                EditorGUIUtility.PingObject(m_buildPipeline);
            }
        }
    }
}