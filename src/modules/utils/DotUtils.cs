using UnityEngine;

namespace mbm_all_in_one.src.modules.utils
{
    public static class DotUtils
    {
        public static void DrawBlueDot()
        {
            GUILayout.Label("•", new GUIStyle { normal = { textColor = Color.blue } });
        }

        public static void DrawRedDot()
        {
            GUILayout.Label("•", new GUIStyle { normal = { textColor = Color.red } });
        }

        public static void DrawGreenDot()
        {
            GUILayout.Label("•", new GUIStyle { normal = { textColor = Color.green } });
        }

        public static void DrawVioletDot()
        {
            GUILayout.Label("•", new GUIStyle { normal = { textColor = new Color(0.5f, 0, 0.5f) } });
        }
    }
} 