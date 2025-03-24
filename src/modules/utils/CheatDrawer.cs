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

        public static void DrawInputCheat(string label, ref string inputText, string buttonText, Action<int> action)
        {
            GUILayout.BeginHorizontal();
            DotUtils.DrawVioletDot();
            GUILayout.Label(label);
            inputText = GUILayout.TextField(inputText, GUILayout.Width(40));

            // Check if the input is valid
            bool isValidInput = int.TryParse(inputText, out int amount) && amount > 0;

            // Enable or disable the button based on input validity
            GUI.enabled = isValidInput;
            if (GUILayout.Button(buttonText) && isValidInput)
            {
                action.Invoke(amount);
            }
            GUI.enabled = true; // Re-enable GUI for other elements

            GUILayout.EndHorizontal();
        }
    }
} 