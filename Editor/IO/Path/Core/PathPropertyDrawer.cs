using UnityEditor;
using UnityEngine;

namespace UniTools.IO
{
    [CustomPropertyDrawer(typeof(PathProperty))]
    public sealed class PathPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            const int shift = 200;
            // Calculate rects
            Rect firstRect = new Rect(position.x, position.y, shift, position.height);
            Rect secondRect = new Rect(position.x + shift, position.y, position.width - shift, position.height);

            SerializedProperty typeProperty = property.FindPropertyRelative("m_type");

            PathTypes t1 = (PathTypes) typeProperty.enumValueIndex;

            if (t1 == PathTypes.String)
            {
                SerializedProperty p = property.FindPropertyRelative("m_path");
                Color c = GUI.color;
                if (string.IsNullOrEmpty(p.stringValue))
                {
                    GUI.color = Color.red;
                }

                EditorGUI.PropertyField(firstRect, p, GUIContent.none);
                GUI.color = c;
            }

            if (t1 == PathTypes.Scriptable)
            {
                SerializedProperty p = property.FindPropertyRelative("m_scriptablePath");

                Color c = GUI.color;
                if (p.objectReferenceValue == null)
                {
                    GUI.color = Color.red;
                }

                EditorGUI.PropertyField(firstRect, p, GUIContent.none);
                GUI.color = c;
            }

            EditorGUI.PropertyField(secondRect, typeProperty, GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}