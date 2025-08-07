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
using mbm_all_in_one.src.Managers;
using mbm_all_in_one.src; // Add this to ensure TabDefinition is found

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
            GameManager.Instance.AddSystemMessage("MBM All In One v" + MyPluginInfo.PLUGIN_VERSION + " loaded");
        }
    }

    // UI Manager to handle rendering
    public partial class ModMenuUI : MonoBehaviour
    {
        private CheatManager _cheatManager;
        private bool _showMenu;
        private Rect _menuRect = new Rect(20, 20, 450, 600);
        private Tab _currentTab = Tab.Player;

        private readonly Dictionary<string, string> _cheatAmountTexts = new Dictionary<string, string>();

        private PopupManager _popupManager;
        private ExecuteEventCheat _executeEventCheat;

        private bool _showDropdown = false;
        private int _selectedNpcIndex = 0;
        private Vector2 _dropdownScrollPosition = Vector2.zero;

        private List<TabDefinition> _tabs;

        private void Start()
        {
            _cheatManager = new CheatManager();
            RegisterAllCheats();

            _popupManager = gameObject.AddComponent<PopupManager>();
            _popupManager.Initialize(Enum.GetNames(typeof(EItemType)), (selectedItem, amount) =>
            {
                GameManager.Instance.PlayerData.NewItem(selectedItem, ESector.Inventory, new ValueTuple<int, int>(0, 0), -1, amount);
            });

            _executeEventCheat = new ExecuteEventCheat();

            // Initialize dynamic tab definitions
            _tabs = new List<TabDefinition>
            {
                new TabDefinition(Tab.Player, "Player", DrawPlayerTab),
                new TabDefinition(Tab.Events, "Events", DrawEventsTab),
                new TabDefinition(Tab.NPCs, "NPCs", DrawNPCsTab),
                new TabDefinition(Tab.Experimental, "Experimental", DrawExperimentalTab),
                new TabDefinition(Tab.Mods, "Mods", DrawModsTab)
            };
        }

        private void RegisterAllCheats()
        {
            _cheatManager.RegisterAllCheats();
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

            // Draw tab buttons dynamically
            GUILayout.BeginHorizontal();
            foreach (var tabDef in _tabs)
            {
                GUI.backgroundColor = _currentTab == tabDef.Tab ? Color.white : Color.grey;
                if (GUILayout.Button(tabDef.Label))
                    _currentTab = tabDef.Tab;
            }
            GUILayout.EndHorizontal();

            // Draw content for the selected tab
            _tabs.First(t => t.Tab == _currentTab).DrawContent();

            GUILayout.EndVertical();
        }

        private void DrawPlayerTab()
        {
            GUILayout.BeginVertical();

            foreach (var cheat in _cheatManager.GetCheats().Where(c => c.DisplayTab == Tab.Player))
            {
                string cheatName = cheat.Name;

                if (!_cheatAmountTexts.ContainsKey(cheatName))
                {
                    _cheatAmountTexts[cheatName] = "0";
                }

                string inputText = _cheatAmountTexts[cheatName];

                CheatUIUtils.DrawCheat(cheat, ref inputText, amount => cheat.Execute(amount));

                _cheatAmountTexts[cheatName] = inputText;
            }

            GUILayout.EndVertical();

            if (GUILayout.Button("Specific Item Spawner", GUILayout.ExpandWidth(true)))
            {
                _popupManager.ShowPopup();
            }
        }
        
        private void DrawEventsTab()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Launch Specific Events");

            foreach (var cheat in _cheatManager.GetCheats().Where(c => c.DisplayTab == Tab.Events))
            {
                if (cheat is ExecuteEventCheat eventCheat)
                {
                    eventCheat.DrawEventSelector();
                }
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Execute Event", GUILayout.ExpandWidth(true)))
            {
                foreach (var cheat in _cheatManager.GetCheats().Where(c => c.DisplayTab == Tab.Events))
                {
                    if (cheat is ExecuteEventCheat eventCheat)
                    {
                        eventCheat.Execute(0);
                        break;
                    }
                }
            }

            GUILayout.EndVertical();
        }
        
        private void DrawNPCsTab()
        {
            GUILayout.BeginVertical();

            var spawnSpecialSlaveCheat = _cheatManager.GetCheats().OfType<SpawnSpecialSlaveCheat>().FirstOrDefault();
            if (spawnSpecialSlaveCheat != null)
            {
                GUILayout.Label("Spawn Specific NPC", GUILayout.ExpandWidth(true));

                // Dropdown button
                if (GUILayout.Button(spawnSpecialSlaveCheat.GetCurrentTypeName(), GUILayout.ExpandWidth(true)))
                {
                    _showDropdown = !_showDropdown;
                }

                // Dropdown menu
                if (_showDropdown)
                {
                    _dropdownScrollPosition = GUILayout.BeginScrollView(_dropdownScrollPosition, GUILayout.Height(100));
                    GUILayout.BeginVertical("box");
                    for (int i = 0; i < spawnSpecialSlaveCheat.GetSpawnableTypes().Length; i++)
                    {
                        if (GUILayout.Button(spawnSpecialSlaveCheat.GetSpawnableTypes()[i].ToString()))
                        {
                            _selectedNpcIndex = i;
                            spawnSpecialSlaveCheat.SetCurrentTypeIndex(i);
                            _showDropdown = false;
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                }

                // Spawn button
                if (GUILayout.Button("Spawn " + spawnSpecialSlaveCheat.GetCurrentTypeName(), GUILayout.ExpandWidth(true)))
                {
                    spawnSpecialSlaveCheat.Execute(0);
                }
            }
            else
            {
                GUILayout.Label("No special slave cheat available.", GUILayout.ExpandWidth(true));
            }

            GUILayout.EndVertical();
        }

        private void DrawExperimentalTab()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Experimental:");
            // Add experimental-related UI elements here

            GUILayout.EndVertical();
        }

        private void DrawModsTab()
        {
            GUILayout.BeginVertical();

            // Use styles from ModCategoryStyles
            var stableLabelStyle = UI.ModCategoryStyles.StableLabelStyle;
            var experimentalLabelStyle = UI.ModCategoryStyles.ExperimentalLabelStyle;
            var brokenLabelStyle = UI.ModCategoryStyles.BrokenLabelStyle;

            GUILayout.BeginVertical(GUI.skin.box);

            // Stable Mods Section
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Stable Mods:", stableLabelStyle);
            GUILayout.EndVertical();
            // Add stable mod-related UI elements here
            GUILayout.Label("Mod 1: Description of stable mod 1");
            GUILayout.Label("Mod 2: Description of stable mod 2");

            GUILayout.Space(10);

            // Experimental Mods Section
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Experimental Mods:", experimentalLabelStyle);
            GUILayout.EndVertical();
            // Add experimental mod-related UI elements here
            GUILayout.Label("Mod 3: Description of experimental mod 3");
            GUILayout.Label("Mod 4: Description of experimental mod 4");

            GUILayout.Space(10);

            // Broken Mods Section
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Broken Mods:", brokenLabelStyle);
            GUILayout.EndVertical();
            // Add broken mod-related UI elements here
            GUILayout.Label("Mod 5: Description of broken mod 5");
            GUILayout.Label("Mod 6: Description of broken mod 6");

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        private class TabDefinition
        {
            public Tab Tab { get; }
            public string Label { get; }
            private readonly Action _drawContent;

            public TabDefinition(Tab tab, string label, Action drawContent)
            {
                Tab = tab;
                Label = label;
                _drawContent = drawContent;
            }

            public void DrawContent()
            {
                _drawContent.Invoke();
            }
        }
    }
}