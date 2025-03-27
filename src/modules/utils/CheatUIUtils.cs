using System;
using UnityEngine;

namespace mbm_all_in_one.src.modules.utils
{
    public static class CheatUIUtils
    {
        public static void DrawCheat(ICheat cheat, ref string inputText, Action<int> executeAction)
        {
            GUILayout.BeginHorizontal();
            switch (cheat.Type)
            {
                case CheatType.Toggle:
                    DotUtils.DrawRedDot(); // or DrawGreenDot based on state
                    GUILayout.Label($"Toggle {cheat.Name}", GUILayout.Width(250)); // Fixed width for label
                    if (GUILayout.Button("Toggle", GUILayout.Width(100))) // Fixed width for button
                    {
                        executeAction.Invoke(0);
                    }
                    break;

                case CheatType.ExecuteWithInput:
                    DotUtils.DrawVioletDot();
                    GUILayout.Label($"Add {cheat.Name} Amount", GUILayout.Width(250)); // Fixed width for label
                    inputText = GUILayout.TextField(inputText, GUILayout.Width(40)); // Fixed width for input
                    if (GUILayout.Button("Execute", GUILayout.Width(100)) && int.TryParse(inputText, out int amount) && amount > 0) // Fixed width for button
                    {
                        executeAction.Invoke(amount);
                    }
                    break;

                case CheatType.Execute:
                    DotUtils.DrawBlueDot();
                    GUILayout.Label($"{cheat.Name}", GUILayout.Width(290)); // Fixed width for label
                    if (GUILayout.Button("Execute", GUILayout.Width(100))) // Fixed width for button
                    {
                        executeAction.Invoke(0);
                    }
                    break;

                case CheatType.ListExecute:
                    DotUtils.DrawYellowDot();
                    GUILayout.Label($"List Execute {cheat.Name}", GUILayout.Width(250)); // Fixed width for label
                    if (GUILayout.Button("List Execute", GUILayout.Width(100))) // Fixed width for button
                    {
                        executeAction.Invoke(0);
                    }
                    break;
            }
            GUILayout.EndHorizontal();
        }
    }
} 