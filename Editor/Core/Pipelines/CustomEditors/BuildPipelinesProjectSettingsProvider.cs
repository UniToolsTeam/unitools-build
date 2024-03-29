using System.Collections.Generic;
using System.Linq;
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
        private readonly List<string> m_allParameterKeys = new List<string>();
        private readonly List<ScriptingDefineSymbolsPresenter> m_defineSymbolsPresenters = new List<ScriptingDefineSymbolsPresenter>();

        private BuildPipelinesProjectSettingsProvider(string path)
            : base(path, SettingsScope.Project)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            //Find parameters
            List<ScriptableBuildParameter> parameters = new List<ScriptableBuildParameter>();
            m_parameterPresenters.Clear();
            m_allParameterKeys.Clear();
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(StringBuildParameter)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                StringBuildParameter parameter = AssetDatabase.LoadAssetAtPath<StringBuildParameter>(path);
                m_parameterPresenters.Add(new StringBuildParameterPresenter(parameter));
                parameters.Add(parameter);
            }

            guids = AssetDatabase.FindAssets($"t:{nameof(IntBuildParameter)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                IntBuildParameter parameter = AssetDatabase.LoadAssetAtPath<IntBuildParameter>(path);
                m_parameterPresenters.Add(new IntBuildParameterPresenter(parameter));
                parameters.Add(parameter);
            }

            m_allParameterKeys.AddRange(m_parameterPresenters.Select(k => k.CliKey));

            //Find defines
            m_defineSymbolsPresenters.Clear();
            guids = AssetDatabase.FindAssets($"t:{nameof(ScriptingDefineSymbols)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptingDefineSymbols define = AssetDatabase.LoadAssetAtPath<ScriptingDefineSymbols>(path);
                m_defineSymbolsPresenters.Add(new ScriptingDefineSymbolsPresenter(define));
                parameters.Add(define);
            }

            //Find pipelines
            m_pipelinePresenters.Clear();
            guids = AssetDatabase.FindAssets($"t:{nameof(BuildPipeline)}");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildPipeline pipline = AssetDatabase.LoadAssetAtPath<BuildPipeline>(path);
                m_pipelinePresenters.Add(new BuildPipelinePresenter(pipline, parameters));
            }
        }

        public override void OnGUI(string searchContext)
        {
            if (m_parameterPresenters.Count > 0)
            {
                EditorGUILayout.LabelField("Parameters", Styles.H1);

                foreach (BuildParameterPresenter presenter in m_parameterPresenters)
                {
                    presenter.Draw(m_allParameterKeys.Count(k => k.Equals(presenter.CliKey)) > 1);
                }
            }
            else
            {
                //TODO add a link to the documentation
            }

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Defines", Styles.H1);
            EditorGUILayout.Space(5);
            foreach (ScriptingDefineSymbolsPresenter presenter in m_defineSymbolsPresenters)
            {
                presenter.Draw();
            }

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Pipelines", Styles.H1);
            EditorGUILayout.Space(5);
            foreach (BuildPipelinePresenter presenter in m_pipelinePresenters)
            {
                presenter.Draw();
            }
        }
    }
}