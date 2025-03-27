using UnityEngine;

namespace mbm_all_in_one.src.modules.utils
{
    public static class UIUtils
    {
        public static void DrawLabel(string text, Color color, Rect position)
        {
            var style = new GUIStyle(GUI.skin.label) { normal = { textColor = color } };
            GUI.Label(position, text, style);
        }

        public static bool DrawButton(string text, Color color, Rect position)
        {
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            bool result = GUI.Button(position, text);
            GUI.backgroundColor = originalColor;
            return result;
        }

        public static Vector2 BeginScrollView(Vector2 scrollPosition, float width, float height)
        {
            return GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));
        }

        public static void EndScrollView()
        {
            GUILayout.EndScrollView();
        }
    }
} 