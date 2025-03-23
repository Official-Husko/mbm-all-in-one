using System;
using UnityEngine;

namespace mbm_all_in_one.src.modules.utils
{
    public static class CheatDrawer
    {
        public static void DrawToggleCheat(string label, string cheatName, Action action)
        {
            GUILayout.BeginHorizontal();
            if (CheatStateManager.IsCheatActive(cheatName))
            {
                DotUtils.DrawGreenDot();
            }
            else
            {
                DotUtils.DrawRedDot();
            }

            if (GUILayout.Button(label))
            {
                CheatStateManager.ToggleCheatState(cheatName);
                action.Invoke();
            }
            GUILayout.EndHorizontal();
        }

        public static void DrawInputCheat(string label, ref string inputText, string buttonText, Action action)
        {
            GUILayout.BeginHorizontal();
            DotUtils.DrawVioletDot();
            GUILayout.Label(label);
            inputText = GUILayout.TextField(inputText, GUILayout.Width(40));
            if (int.TryParse(inputText, out int amount) && amount > 0)
            {
                if (GUILayout.Button(buttonText))
                {
                    action.Invoke();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
} 