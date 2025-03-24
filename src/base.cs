using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using MBMScripts;
using UnityEngine;
using mbm_all_in_one.src.modules.cheats; // Updated namespace
using mbm_all_in_one.src.modules.utils;
using System.Runtime.InteropServices; // Updated namespace

namespace mbm_all_in_one.src
{
    [BepInPlugin("husko.monsterblackmarket.cheats", "Monster Black Market Cheats", MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private ModMenuUI _modMenuUI;

        private void Awake()
        {
            // Check if the ModMenuUI component already exists
            _modMenuUI = gameObject.GetComponent<ModMenuUI>();
            if (_modMenuUI != null)
            {
                // If it exists, destroy it
                Destroy(_modMenuUI);
            }

            // Add a new ModMenuUI component
            _modMenuUI = gameObject.AddComponent<ModMenuUI>();
        }
    }

    // Define the CheatManager class
    public class CheatManager
    {
        private List<ICheat> _cheats = new List<ICheat>();

        public void RegisterCheat(ICheat cheat)
        {
            _cheats.Add(cheat);
        }

        public IEnumerable<ICheat> GetCheats()
        {
            return _cheats;
        }

        public void ExecuteCheat(string name, int amount)
        {
            var cheat = _cheats.FirstOrDefault(c => c.Name == name);
            cheat?.Execute(amount);
        }
    }

    // UI Manager to handle rendering
    public class ModMenuUI : MonoBehaviour
    {
        private CheatManager _cheatManager;
        private bool _showMenu;
        private Rect _menuRect = new Rect(20, 20, 450, 300);
        private Tab _currentTab = Tab.Player;

        private enum Tab
        {
            Player,
            Events,
            NPCs,
            Mods
        }

        private Dictionary<string, string> _cheatAmountTexts = new Dictionary<string, string>();

        private readonly int LabelWidth = 250;
        private readonly int ButtonWidth = 100;

        private void Start()
        {
            _cheatManager = new CheatManager();
            RegisterAllCheats();
        }

        private void RegisterAllCheats()
        {
            var cheatTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IRegisterableCheat).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in cheatTypes)
            {
                var cheat = (IRegisterableCheat)Activator.CreateInstance(type);
                cheat.Register(_cheatManager);
            }
        }

        private void Update()
        {
            // Toggle menu visibility with Insert or F1 key
            if (Input.GetKeyDown(KeyCode.Insert) || Input.GetKeyDown(KeyCode.F1))
            {
                _showMenu = !_showMenu;
            }
        }

        private void OnGUI()
        {
            if (!_showMenu) return;

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
            _menuRect = GUI.Window(0, _menuRect, MenuWindow, "----< Mod Menu >----");

            // Draw version label at bottom left corner
            float versionLabelX = _menuRect.xMin + 10; // 10 pixels from left edge
            float versionLabelY = _menuRect.yMax - 30; // 30 pixels from bottom edge
            UIUtils.DrawLabel("v" + MyPluginInfo.PLUGIN_VERSION, new Color(0.5f, 0.5f, 0.5f), new Rect(versionLabelX, versionLabelY, 100, 20));

            // Calculate the width of the author label
            float authorLabelWidth = GUI.skin.label.CalcSize(new GUIContent("by Official-Husko")).x + 12;
            float authorLabelX = _menuRect.xMax - authorLabelWidth - 10; // 10 pixels from right edge
            float authorLabelY = versionLabelY; // Align with version label

            // Draw the author label as a clickable label
            if (UIUtils.DrawButton("<color=cyan>by</color> <color=yellow>Official-Husko</color>", Color.clear, new Rect(authorLabelX, authorLabelY, authorLabelWidth, 20)))
            {
                Application.OpenURL("https://github.com/Official-Husko/mbm-all-in-one");
            }
        }

        private void MenuWindow(int windowID)
        {
            // Make the whole window draggable
            GUI.DragWindow(new Rect(0, 0, _menuRect.width, 20));

            GUILayout.BeginVertical();

            // Draw tabs
            GUILayout.BeginHorizontal();
            DrawTabButton(Tab.Player, "Player");
            DrawTabButton(Tab.Events, "Events");
            DrawTabButton(Tab.NPCs, "NPCs");
            DrawTabButton(Tab.Mods, "Mods");
            GUILayout.EndHorizontal();

            // Draw content based on the selected tab
            switch (_currentTab)
            {
                case Tab.Player:
                    DrawPlayerTab();
                    break;
                case Tab.Events:
                    DrawEventsTab();
                    break;
                case Tab.NPCs:
                    DrawNPCsTab();
                    break;
                case Tab.Mods:
                    DrawModsTab();
                    break;
            }

            GUILayout.EndVertical();
        }

        private void DrawTabButton(Tab tab, string label)
        {
            GUI.backgroundColor = _currentTab == tab ? Color.white : Color.grey;
            if (GUILayout.Button(label))
            {
                _currentTab = tab;
            }
        }

        private void DrawPlayerTab()
        {
            GUILayout.BeginVertical();

            foreach (var cheat in _cheatManager.GetCheats())
            {
                string cheatName = cheat.Name;

                if (!_cheatAmountTexts.ContainsKey(cheatName))
                {
                    _cheatAmountTexts[cheatName] = "0";
                }

                string inputText = _cheatAmountTexts[cheatName];

                GUILayout.BeginHorizontal();
                switch (cheat.Type)
                {
                    case CheatType.Toggle:
                        DotUtils.DrawRedDot(); // or DrawGreenDot based on state
                        GUILayout.Label($"Toggle {cheatName}", GUILayout.Width(LabelWidth)); // Fixed width for label
                        if (GUILayout.Button("Toggle", GUILayout.Width(ButtonWidth))) // Fixed width for button
                        {
                            // Toggle logic here
                        }
                        break;

                    case CheatType.ExecuteWithInput:
                        DotUtils.DrawVioletDot();
                        GUILayout.Label($"Add {cheatName} Amount", GUILayout.Width(LabelWidth)); // Fixed width for label
                        inputText = GUILayout.TextField(inputText, GUILayout.Width(40)); // Fixed width for input
                        if (GUILayout.Button("Execute", GUILayout.Width(ButtonWidth)) && int.TryParse(inputText, out int amount) && amount > 0) // Fixed width for button
                        {
                            cheat.Execute(amount);
                        }
                        break;

                    case CheatType.Execute:
                        DotUtils.DrawBlueDot();
                        GUILayout.Label($"Execute {cheatName}", GUILayout.Width(LabelWidth)); // Fixed width for label
                        if (GUILayout.Button("Execute", GUILayout.Width(ButtonWidth))) // Fixed width for button
                        {
                            cheat.Execute(0);
                        }
                        break;

                    case CheatType.ListExecute:
                        DotUtils.DrawYellowDot();
                        GUILayout.Label($"List Execute {cheatName}", GUILayout.Width(LabelWidth)); // Fixed width for label
                        if (GUILayout.Button("List Execute", GUILayout.Width(ButtonWidth))) // Fixed width for button
                        {
                            // List execute logic here
                        }
                        break;
                }
                GUILayout.EndHorizontal();

                _cheatAmountTexts[cheatName] = inputText;
            }

        private void DrawEventsTab()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Events:");
            // Add event-related UI elements here
            GUILayout.EndVertical();
        }

        private void DrawNPCsTab()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("NPCs:");
            // Add NPC-related UI elements here
            GUILayout.EndVertical();
        }

        private void DrawModsTab()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Enabling and disabling mods requires a restart of the game!");
            // Add mod-related UI elements here
            GUILayout.EndVertical();
        }
    }
}