using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomEditor(typeof(SetTeamId))]
    public sealed class SetTeamIdEditor : Editor
    {
        private SerializedProperty m_teamId = default;

        private void OnEnable()
        {
            m_teamId = serializedObject.FindProperty(nameof(m_teamId));
        }

        public override void OnInspectorGUI()
        {
            bool load = false;

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.HelpBox("You can parse Team Id from the Provisioning Profile file", MessageType.Info);
                    load = GUILayout.Button("Try");
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.PropertyField(m_teamId);
            }
            EditorGUILayout.EndVertical();

            if (load)
            {
                string path = EditorUtility.OpenFilePanel("Test", Application.dataPath, "mobileprovision");

                ProvisioningProfile pp = ProvisioningProfile.Load(path);

                m_teamId.stringValue = pp.TeamIdentifier;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}