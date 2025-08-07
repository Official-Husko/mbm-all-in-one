using UnityEngine;

namespace mbm_all_in_one.src.UI
{
    public static class ModCategoryStyles
    {
        public static GUIStyle StableLabelStyle => new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = Color.green },
            alignment = TextAnchor.MiddleCenter
        };

        public static GUIStyle ExperimentalLabelStyle => new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = new Color(1.0f, 0.65f, 0.0f) },
            alignment = TextAnchor.MiddleCenter
        };

        public static GUIStyle BrokenLabelStyle => new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = Color.red },
            alignment = TextAnchor.MiddleCenter
        };
    }
}
