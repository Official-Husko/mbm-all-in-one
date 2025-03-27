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
            GameManager.Instance.AddSystemMessage("MBM All In One v" + MyPluginInfo.PLUGIN_VERSION + " loaded");
        }
    }

    // Define the CheatManager class
    public class CheatManager
    {
        private readonly List<ICheat> _cheats = new List<ICheat>();

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
        private Rect _menuRect = new Rect(20, 20, 450, 600);
        private Tab _currentTab = Tab.Player;

        private readonly Dictionary<string, string> _cheatAmountTexts = new Dictionary<string, string>();

        private PopupManager _popupManager;
        private ExecuteEventCheat _executeEventCheat;

        private bool _showDropdown = false;
        private int _selectedNpcIndex = 0;
        private Vector2 _dropdownScrollPosition = Vector2.zero;

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

            // Top row of tabs
            GUILayout.BeginHorizontal();
            DrawTabButton(Tab.Player, "Player");
            DrawTabButton(Tab.Events, "Events");
            DrawTabButton(Tab.NPCs, "NPCs");
            GUILayout.EndHorizontal();

            // Bottom row of tabs
            GUILayout.BeginHorizontal();
            DrawTabButton(Tab.Experimental, "Experimental");
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
                case Tab.Experimental:
                    DrawExperimentalTab();
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

            // Define styles for each category label with centered text
            GUIStyle stableLabelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.green },
                alignment = TextAnchor.MiddleCenter
            };
            GUIStyle experimentalLabelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = new Color(1.0f, 0.65f, 0.0f) }, // Orange
                alignment = TextAnchor.MiddleCenter
            };
            GUIStyle brokenLabelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.red },
                alignment = TextAnchor.MiddleCenter
            };

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
        }
    }
}