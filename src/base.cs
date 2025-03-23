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

        private string _addGoldAmountText = "0";

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

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
            _menuRect = GUI.Window(0, _menuRect, MenuWindow, "----< Mod Menu >----");

            // Draw version label at bottom left corner
            float versionLabelX = _menuRect.xMin + 10; // 10 pixels from left edge
            float versionLabelY = _menuRect.yMax - 30; // 30 pixels from bottom edge
            UIUtils.DrawLabel("v" + MyPluginInfo.PLUGIN_VERSION, new Color(0.5f, 0.5f, 0.5f), new Rect(versionLabelX, versionLabelY, 100, 20));

            // Calculate the width of the author label
            float authorLabelWidth = GUI.skin.label.CalcSize(new GUIContent("by Official-Husko")).x + 10;
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
            CheatDrawer.DrawToggleCheat("Add Gold", "Add Gold", () => _cheatManager.ExecuteCheat("Add Gold"));
            CheatDrawer.DrawInputCheat("Add Gold Amount", ref _addGoldAmountText, "Execute", () =>
            {
                if (int.TryParse(_addGoldAmountText, out int amount) && amount > 0)
                {
                    GameManager.Instance.PlayerData.Gold += amount;
                }
            });
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