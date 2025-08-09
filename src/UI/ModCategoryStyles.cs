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

        public static GUIStyle ModCardStyle {
            get {
                if (_modCardStyle == null) {
                    _modCardStyle = new GUIStyle(GUI.skin.box)
                    {
                        margin = new RectOffset(4, 4, 2, 2),
                        padding = new RectOffset(8, 8, 4, 4),
                        border = new RectOffset(1, 1, 1, 1),
                        alignment = TextAnchor.UpperLeft,
                        fontSize = 14
                    };
                    if (_modCardBg == null) {
                        _modCardBg = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                        // Use a darker gray for better contrast
                        _modCardBg.SetPixel(0, 0, new Color(0.22f, 0.22f, 0.22f, 1f));
                        _modCardBg.Apply();
                    }
                    _modCardStyle.normal.background = _modCardBg;
                }
                return _modCardStyle;
            }
        }

        private static GUIStyle _modCardStyle;
    private static Texture2D _modCardBg;

        public static GUIStyle ModNameStyle => new GUIStyle(GUI.skin.label)
        {
            fontSize = 15,
            fontStyle = FontStyle.Normal,
            normal = { textColor = new Color(0.7f, 0.7f, 0.7f) },
            alignment = TextAnchor.UpperLeft
        };

        public static GUIStyle ModVersionStyle => new GUIStyle(GUI.skin.label)
        {
            fontSize = 12,
            fontStyle = FontStyle.Italic,
            normal = { textColor = new Color(0.5f, 0.5f, 0.5f) },
            alignment = TextAnchor.UpperLeft
        };

        public static GUIStyle ModAuthorStyle => new GUIStyle(GUI.skin.label)
        {
            fontSize = 12,
            fontStyle = FontStyle.Normal,
            normal = { textColor = new Color(0.6f, 0.8f, 1.0f) },
            alignment = TextAnchor.UpperLeft
        };

        public static GUIStyle ModDescriptionStyle => new GUIStyle(GUI.skin.label)
        {
            fontSize = 13,
            fontStyle = FontStyle.Normal,
            wordWrap = true,
            normal = { textColor = Color.white },
            alignment = TextAnchor.UpperLeft
        };
    }
}
