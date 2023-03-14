using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    [CustomPropertyDrawer(typeof(ScriptingDefineSymbols.DefineSymbol))]
    public sealed class DefineSymbolPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int boolSize = 25;
            Rect enabledRect = new Rect(position.x, position.y, boolSize, position.height);
            Rect valueRect = new Rect(position.x + boolSize, position.y, position.width - boolSize, position.height);
            SerializedProperty value = property.FindPropertyRelative("Value");
            SerializedProperty enabled = property.FindPropertyRelative("Enabled");
            enabled.boolValue = EditorGUI.Toggle(enabledRect, enabled.boolValue);
            GUI.enabled = enabled.boolValue;
            value.stringValue = EditorGUI.TextField(valueRect, value.stringValue).Replace(" ", "_");
            GUI.enabled = true;
        }
    }
}