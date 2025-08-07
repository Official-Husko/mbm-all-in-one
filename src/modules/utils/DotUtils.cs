using UnityEngine;

namespace mbm_all_in_one.src.modules.utils
{
    public static class DotUtils
    {
        private static GUIStyle CreateDotStyle(Color color)
        {
            return new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = color },
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20, // Slightly larger than label for visibility
                padding = new RectOffset(0, 0, 2, 0) // Minimal top padding
            };
        }

        public static void DrawBlueDot()
        {
            GUILayout.Label("•", CreateDotStyle(Color.blue));
        }

        public static void DrawRedDot()
        {
            GUILayout.Label("•", CreateDotStyle(Color.red));
        }

        public static void DrawGreenDot()
        {
            GUILayout.Label("•", CreateDotStyle(Color.green));
        }

        public static void DrawVioletDot()
        {
            GUILayout.Label("•", CreateDotStyle(new Color(0.5f, 0, 0.5f)));
        }

        public static void DrawYellowDot()
        {
            GUILayout.Label("•", CreateDotStyle(Color.yellow));
        }
    }
}