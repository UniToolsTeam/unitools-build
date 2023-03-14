using UnityEditor;
using UnityEngine;

namespace UniTools.Build
{
    public static class Styles
    {
        public static readonly GUIStyle H1 = CreateStyle(16, FontStyle.Bold, new Color(0.7333333f, 0.7333333f, 0.7333333f));

        public static readonly GUIStyle H2_Foldout = CustomFoldout(14, FontStyle.Italic, Color.white);

        private static GUIStyle CreateStyle(int size, FontStyle fontStyle, Color textColor)
        {
            // this is setting up all kinds of styles for any given field:
            GUIStyle style = new GUIStyle();
            style.fontSize = size;
            style.fontStyle = fontStyle;
            style.normal.textColor = textColor;

            return style;
        }

        private static GUIStyle CustomFoldout(int size, FontStyle fontStyle, Color textColor)
        {
            GUIStyle style = new GUIStyle(EditorStyles.foldout);

            style.fontSize = size;
            style.fontStyle = fontStyle;
            style.normal.textColor = textColor;
            style.active.textColor = textColor;
            style.focused.textColor = textColor;

            return style;
        }
    }
}