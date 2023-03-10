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

        private readonly List<BuildPipelinePresenter> m_pipelinePresenters = new List<BuildPipelinePresenter>();
        private readonly List<BuildParameterPresenter> m_parameterPresenters = new List<BuildParameterPresenter>();

        private BuildPipelinesProjectSettingsProvider(string path)
            : base(path, SettingsScope.Project)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            //Find parameters
            m_parameterPresenters.Clear();
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(StringBuildParameter)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                StringBuildParameter parameter = AssetDatabase.LoadAssetAtPath<StringBuildParameter>(path);
                m_parameterPresenters.Add(new StringBuildParameterPresenter(parameter));
            }

            guids = AssetDatabase.FindAssets($"t:{nameof(IntBuildParameter)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                IntBuildParameter parameter = AssetDatabase.LoadAssetAtPath<IntBuildParameter>(path);
                m_parameterPresenters.Add(new IntBuildParameterPresenter(parameter));
            }

            //Find pipelines
            m_pipelinePresenters.Clear();
            guids = AssetDatabase.FindAssets($"t:{nameof(BuildPipeline)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildPipeline pipline = AssetDatabase.LoadAssetAtPath<BuildPipeline>(path);
                m_pipelinePresenters.Add(new BuildPipelinePresenter(pipline));
            }
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.LabelField("Parameters");

            foreach (BuildParameterPresenter presenter in m_parameterPresenters)
            {
                presenter.Draw();
            }

            EditorGUILayout.LabelField("Pipelines");

            foreach (BuildPipelinePresenter presenter in m_pipelinePresenters)
            {
                presenter.Draw();
            }
        }
    }
}