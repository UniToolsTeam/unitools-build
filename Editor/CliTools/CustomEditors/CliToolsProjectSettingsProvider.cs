using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniTools.Build
{
    public sealed class CliToolsProjectSettingsProvider : SettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider Register() =>
            new CliToolsProjectSettingsProvider($"Project/{nameof(UniTools)}/CLI");

        private readonly List<CliToolEditorPresenter> m_presenters = new List<CliToolEditorPresenter>();

        private CliToolsProjectSettingsProvider(string path)
            : base(path, SettingsScope.Project)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            Show();
        }

        public override void OnGUI(string searchContext)
        {
            foreach (CliToolEditorPresenter presenter in m_presenters)
            {
                presenter.Draw();
                EditorGUILayout.Space(1);
            }

            EditorGUILayout.HelpBox("If the CLI tool is installed but not visible, try to: \n-recompile the code base or restart Unity Editor\n-check the PATH in the \"Tools/CLI/UnityEnvironment\".", MessageType.Info);
#if UNITY_2021_1_OR_NEWER
            if (EditorGUILayout.LinkButton("How to change PATH in UnityEnvironment?"))
            {
                Application.OpenURL("https://github.com/UniToolsTeam/unitools-cli#unity-enviroment");
            }
#endif
        }

        public override void OnDeactivate()
        {
            m_presenters.Clear();
            base.OnDeactivate();
        }

        private void Show()
        {
            IEnumerable<BaseCliTool> tools = Cli.All;
            m_presenters.Clear();
            foreach (BaseCliTool tool in tools)
            {
                m_presenters.Add(new CliToolEditorPresenter(tool));
            }
        }
    }
}