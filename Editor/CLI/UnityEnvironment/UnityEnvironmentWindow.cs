using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UniTools.CLI.Editor
{
    public sealed class UnityEnvironmentWindow : EditorWindow
    {
        private static readonly UnityEnvironmentVariablesHeaderPresenter Header = new UnityEnvironmentVariablesHeaderPresenter();
        private readonly List<UnityEnvironmentVariableEditorPresenter> m_presenters = new List<UnityEnvironmentVariableEditorPresenter>();
        private Vector2 scrollPos = default;

        [MenuItem("Tools/CLI/" + nameof(UnityEnvironment))]
        public static void Open()
        {
            UnityEnvironmentWindow w = GetWindow<UnityEnvironmentWindow>();
            w.name = nameof(UnityEnvironment);
            w.Show();
        }

        public void OnFocus()
        {
            IEnumerable<UnityEnvironmentVariableModel> variables = UnityEnvironment.Variables;
            int count = variables.Count() - m_presenters.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    m_presenters.Add(new UnityEnvironmentVariableEditorPresenter());
                }
            }
        }

        public void OnGUI()
        {
            Header.Draw();
            IEnumerable<UnityEnvironmentVariableModel> variables = UnityEnvironment.Variables;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < variables.Count(); i++)
            {
                m_presenters[i].Draw(variables.ElementAt(i));
            }          

            EditorGUILayout.EndScrollView();
        }
    }
}