using System;
using UnityEngine;

namespace mbm_all_in_one.src.modules.utils
{
    public static class CheatUIUtils
    {
        public static void DrawCheat(ICheat cheat, ref string inputText, Action<int> executeAction)
        {
            const int dotWidth = 32;
            const int buttonWidth = 100;
            GUILayout.BeginHorizontal();
            // Dot column
            GUILayout.BeginVertical(GUILayout.Width(dotWidth));
            switch (cheat.Type)
            {
                case CheatType.Toggle:
                    DotUtils.DrawRedDot();
                    break;
                case CheatType.ExecuteWithInput:
                    DotUtils.DrawVioletDot();
                    break;
                case CheatType.Execute:
                    DotUtils.DrawBlueDot();
                    break;
                case CheatType.ListExecute:
                    DotUtils.DrawYellowDot();
                    break;
            }
            GUILayout.EndVertical();
            // Content column
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            switch (cheat.Type)
            {
                case CheatType.Toggle:
                    GUILayout.Label($"Toggle {cheat.Name}", GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Toggle", GUILayout.Width(buttonWidth)))
                        executeAction.Invoke(0);
                    break;
                case CheatType.ExecuteWithInput:
                    GUILayout.Label($"Add {cheat.Name} Amount", GUILayout.Width(180));
                    inputText = GUILayout.TextField(inputText, GUILayout.Width(80));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Execute", GUILayout.Width(buttonWidth)) && int.TryParse(inputText, out int amount) && amount > 0)
                        executeAction.Invoke(amount);
                    break;
                case CheatType.Execute:
                    GUILayout.Label($"{cheat.Name}", GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Execute", GUILayout.Width(buttonWidth)))
                        executeAction.Invoke(0);
                    break;
                case CheatType.ListExecute:
                    GUILayout.Label($"List Execute {cheat.Name}", GUILayout.ExpandWidth(true));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("List Execute", GUILayout.Width(buttonWidth)))
                        executeAction.Invoke(0);
                    break;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}