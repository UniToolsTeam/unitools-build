using UnityEditor;
using UnityEngine;

namespace UniTools.Build.iOS
{
    public abstract class DistributeIosApplicationStepEditor : IosPostBuildStepEditor
    {
        private SerializedProperty m_bundleIdentifier = default;
        private SerializedProperty m_teamId = default;

        protected override void OnEnable()
        {
            m_bundleIdentifier = serializedObject.FindProperty(nameof(m_bundleIdentifier));
            m_teamId = serializedObject.FindProperty(nameof(m_teamId));
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool useCurrentBundle = false;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(m_bundleIdentifier);
                useCurrentBundle = GUILayout.Button("Current");
            }
            EditorGUILayout.EndHorizontal();

            if (useCurrentBundle)
            {
                m_bundleIdentifier.stringValue = PlayerSettings.applicationIdentifier;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}