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

        private readonly List<BuildPipelinePresenter> m_presenters = new List<BuildPipelinePresenter>();

        private BuildPipelinesProjectSettingsProvider(string path)
            : base(path, SettingsScope.Project)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_presenters.Clear();

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(BuildPipeline)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildPipeline pipline = AssetDatabase.LoadAssetAtPath<BuildPipeline>(path);
                m_presenters.Add(new BuildPipelinePresenter(pipline));
            }
        }

        public override void OnGUI(string searchContext)
        {
            foreach (BuildPipelinePresenter presenter in m_presenters)
            {
                presenter.Draw();
            }
        }
    }
}