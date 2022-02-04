using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace UniTools.Build
{
    public sealed class BuildPipelinesProjectSettingsProvider : SettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider Register() =>
            new BuildPipelinesProjectSettingsProvider($"Project/{nameof(UniTools)}/Build");

        private readonly List<ScriptableBuildPipelinePresenter> m_presenters = new List<ScriptableBuildPipelinePresenter>();

        private BuildPipelinesProjectSettingsProvider(string path)
            : base(path, SettingsScope.Project)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_presenters.Clear();

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(ScriptableBuildPipeline)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableBuildPipeline pipline = AssetDatabase.LoadAssetAtPath<ScriptableBuildPipeline>(path);
                m_presenters.Add(new ScriptableBuildPipelinePresenter(pipline));
            }
        }

        public override void OnGUI(string searchContext)
        {
            foreach (ScriptableBuildPipelinePresenter presenter in m_presenters)
            {
                presenter.Draw();
            }
        }
    }
}