using UnityEngine;
using System;

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

        public static void DrawSection(string label, GUIStyle style, Action content)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(label, style);
            content?.Invoke();
            GUILayout.EndVertical();
        }

        public static void DrawFooter(string version, string authorLabel, string authorUrl, Rect menuRect)
        {
            float versionLabelX = menuRect.xMin + 10; // 10 pixels from left edge
            float versionLabelY = menuRect.yMax - 30; // 30 pixels from bottom edge
            DrawLabel("v" + version, new Color(0.5f, 0.5f, 0.5f), new Rect(versionLabelX, versionLabelY, 100, 20));

            float authorLabelWidth = GUI.skin.label.CalcSize(new GUIContent(authorLabel)).x + 12;
            float authorLabelX = menuRect.xMax - authorLabelWidth - 10; // 10 pixels from right edge
            float authorLabelY = versionLabelY; // Align with version label

            if (DrawButton(authorLabel, Color.clear, new Rect(authorLabelX, authorLabelY, authorLabelWidth, 20)))
            {
                Application.OpenURL(authorUrl);
            }
        }
    }
}