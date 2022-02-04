using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniTools.Build
{
    public sealed class ScriptingDefineSymbolsProvider : SettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider Register() =>
            new ScriptingDefineSymbolsProvider($"Project/{nameof(UniTools)}/Configuration/Scripting Define Symbols");

        private static readonly BuildTargetGroup[] Targets =
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Android
        };

        private readonly string[] m_toolbarOptions = default;

        private readonly Dictionary<BuildTargetGroup, ScriptingDefineSymbolsPresenter> m_presenters = new Dictionary<BuildTargetGroup, ScriptingDefineSymbolsPresenter>();
        private ScriptingDefineSymbolsPresenter m_presenter = default;
        private int m_currentToolbarIndex = 0;

        private ScriptingDefineSymbolsProvider(string path)
            : base(path, SettingsScope.Project)
        {
            m_toolbarOptions = Targets.Select(t => t.ToString()).ToArray();
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            Refresh();
            base.OnActivate(searchContext, rootElement);
        }

        public override void OnDeactivate()
        {
            m_presenters.Clear();
            m_presenter = default;
            base.OnDeactivate();
        }

        public override void OnGUI(string searchContext)
        {
            int prev = m_currentToolbarIndex;
            bool refresh = false;

            EditorGUILayout.BeginHorizontal();
            {
                m_currentToolbarIndex = GUILayout.Toolbar(m_currentToolbarIndex, m_toolbarOptions);
                refresh = GUILayout.Button("R", GUILayout.Width(25));
            }
            EditorGUILayout.EndHorizontal();

            if (refresh)
            {
                Refresh();

                return;
            }

            if (prev != m_currentToolbarIndex || m_presenter == null)
            {
                m_presenter = m_presenters[ToolboxIndexToTarget(m_currentToolbarIndex)];
            }

            if (EditorApplication.isCompiling)
            {
                EditorGUILayout.LabelField("Compiling, please wait...");

                return;
            }

            EditorGUILayout.BeginVertical("box");
            {
                m_presenter.Draw();
            }
            EditorGUILayout.EndVertical();
        }

        private void Refresh()
        {
            m_presenters.Clear();
            m_presenter = default;
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(ScriptingDefineSymbols)}");
            ScriptingDefineSymbols[] defines = new ScriptingDefineSymbols[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                defines[i] = AssetDatabase.LoadAssetAtPath<ScriptingDefineSymbols>(path);
            }

            foreach (BuildTargetGroup targetGroup in Targets)
            {
                m_presenters.Add(targetGroup, new ScriptingDefineSymbolsPresenter(targetGroup, defines));
            }
        }

        private static BuildTargetGroup ToolboxIndexToTarget(int index)
        {
            if (index < Targets.Length)
            {
                return Targets[index];
            }

            throw new Exception($"Unsupported index {index}!");
        }
    }
}