using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using MBMScripts;
using UnityEngine;
using mbm_all_in_one.src.modules.cheats; // Updated namespace
using mbm_all_in_one.src.modules.utils; // Updated namespace

namespace mbm_all_in_one.src
{
    [BepInPlugin("husko.monsterblackmarket.cheats", "Monster Black Market Cheats", MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private ModMenuUI _modMenuUI;

        private void Awake()
        {
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

        public void ExecuteCheat(string name)
        {
            var cheat = _cheats.FirstOrDefault(c => c.Name == name);
            cheat?.Execute();
        }
    }

    // UI Manager to handle rendering
    public class ModMenuUI : MonoBehaviour
    {
        private CheatManager _cheatManager;
        private bool _showMenu;
        private Rect _menuRect = new Rect(20, 20, 450, 300);
        private Tab _currentTab = Tab.Cheats;

        private enum Tab
        {
            Cheats,
            Mods
        }

        private void Start()
        {
            _cheatManager = new CheatManager();
            _cheatManager.RegisterCheat(new AddGoldCheat());
            _cheatManager.RegisterCheat(new AddPixyCheat());
            // Register other cheats
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

            // Apply dark mode GUI style
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f);

            // Draw the IMGUI window
            _menuRect = GUI.Window(0, _menuRect, MenuWindow, "----< Mod Menu >----");

            // Calculate position for version label at bottom left corner
            float versionLabelX = _menuRect.xMin + 10; // 10 pixels from left edge
            float versionLabelY = _menuRect.yMax - 20; // 20 pixels from bottom edge

            // Draw version label at bottom left corner
            GUI.contentColor = new Color(0.5f, 0.5f, 0.5f); // Dark grey silver color
            GUI.Label(new Rect(versionLabelX, versionLabelY, 100, 20), "v" + MyPluginInfo.PLUGIN_VERSION);

            // Calculate the width of the author label
            float authorLabelWidth = GUI.skin.label.CalcSize(new GUIContent("by Official-Husko")).x + 10; // Add some extra width for padding

            // Calculate position for author label at bottom right corner
            float authorLabelX = _menuRect.xMax - authorLabelWidth; // 10 pixels from right edge
            float authorLabelY = versionLabelY + 2; // Align with version label

            // Draw the author label as a clickable label
            if (GUI.Button(new Rect(authorLabelX, authorLabelY, authorLabelWidth, 20), "<color=cyan>by</color> <color=yellow>Official-Husko</color>", GUIStyle.none))
            {
                // Open a link in the user's browser when the label is clicked
                Application.OpenURL("https://github.com/Official-Husko/mbm-cheats-menu");
            }
        }

        private void MenuWindow(int windowID)
        {
            // Make the whole window draggable
            GUI.DragWindow(new Rect(0, 0, _menuRect.width, 20));

            GUILayout.BeginVertical();

            // Draw tabs
            GUILayout.BeginHorizontal();
            DrawTabButton(Tab.Cheats, "Cheats");
            DrawTabButton(Tab.Mods, "Mods");
            GUILayout.EndHorizontal();

            // Draw content based on the selected tab
            switch (_currentTab)
            {
                case Tab.Cheats:
                    DrawCheatsTab();
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

        private void DrawCheatsTab()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Cheats:");
            if (GUILayout.Button("Add Gold"))
            {
                _cheatManager.ExecuteCheat("Add Gold");
            }
            if (GUILayout.Button("Add Pixy"))
            {
                _cheatManager.ExecuteCheat("Add Pixy");
            }
            // Add more cheat buttons here

            GUILayout.EndVertical();
        }

        private void DrawModsTab()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Mods:");
            // Add mod-related UI elements here

            GUILayout.EndVertical();
        }
    }
}