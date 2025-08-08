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
using mbm_all_in_one.src.modules.mods; // Add this using directive
using mbm_all_in_one.src.modules.mods; // Duplicate for partial class visibility

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
        private Rect _menuRect = new(20, 20, 450, 600);
        private Tab _currentTab = Tab.Player;

        private readonly Dictionary<string, string> _cheatAmountTexts = new();

        private PopupManager _popupManager;
        private ExecuteEventCheat _executeEventCheat;

        private bool _showDropdown = false;
        private int _selectedNpcIndex = 0;
        private Vector2 _dropdownScrollPosition = Vector2.zero;

        private List<mbm_all_in_one.src.TabDefinition> _tabs;

        private static List<ModInfo> _stableMods = new();
        private static List<ModInfo> _experimentalMods = new();
        private static List<ModInfo> _brokenMods = new();

        static ModMenuUI()
        {
            // Reflection-based mod discovery
            var modInfoType = typeof(ModInfo);
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && t.IsPublic && t.GetProperty("ModInfo", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public) != null);
            foreach (var type in allTypes)
            {
                var modInfoProp = type.GetProperty("ModInfo", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (modInfoProp != null && modInfoProp.PropertyType == modInfoType)
                {
                    var modInfo = (ModInfo)modInfoProp.GetValue(null);
                    if (modInfo != null)
                    {
                        switch (modInfo.Category?.ToLowerInvariant())
                        {
                            case "stable": _stableMods.Add(modInfo); break;
                            case "experimental": _experimentalMods.Add(modInfo); break;
                            case "broken": _brokenMods.Add(modInfo); break;
                        }
                    }
                }
            }
        }

        private void Start()
        {
            _cheatManager = new CheatManager();
            RegisterAllCheats();

            _popupManager = gameObject.AddComponent<PopupManager>();
            _popupManager.Initialize(ItemUtils.GetAllItemTypes().ToArray(), (selectedItem, amount) =>
            {
                GameManager.Instance.PlayerData.NewItem(selectedItem, ESector.Inventory, new ValueTuple<int, int>(0, 0), -1, amount);
            });

            _executeEventCheat = new ExecuteEventCheat();

            // Initialize dynamic tab definitions
            _tabs = new List<mbm_all_in_one.src.TabDefinition>
            {
                new(Tab.Player, "Player", DrawPlayerTab),
                new(Tab.Events, "Events", DrawEventsTab),
                new(Tab.NPCs, "NPCs", DrawNPCsTab),
                new(Tab.Experimental, "Experimental", DrawExperimentalTab),
                new(Tab.Mods, "Mods", DrawModsTab)
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

            // Draw version and author footer using utility
            UIUtils.DrawFooter(MyPluginInfo.PLUGIN_VERSION, "<color=cyan>by</color> <color=yellow>Official-Husko</color>", "https://github.com/Official-Husko/mbm-all-in-one", _menuRect);
        }

        private void MenuWindow(int windowID)
        {
            // Make the whole window draggable
            GUI.DragWindow(new Rect(0, 0, _menuRect.width, 20));

            GUILayout.BeginVertical();

            // Draw tab buttons dynamically
            GUILayout.BeginHorizontal();
            mbm_all_in_one.src.modules.utils.UITabUtils.DrawTabButtons(_tabs, _currentTab, tab => _currentTab = tab);
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

            mbm_all_in_one.src.modules.utils.UIUtils.DrawSection("Stable Mods:", stableLabelStyle, () => {
                foreach (var mod in _stableMods)
                {
                    GUILayout.BeginVertical(UI.ModCategoryStyles.ModCardStyle);
                    GUILayout.Label($"<color=#b3b3b3ff>{mod.Name}</color> <size=12><color=#888888ff>v{mod.Version}</color></size>", UI.ModCategoryStyles.ModNameStyle);
                    GUILayout.Label($"by {mod.OriginalAuthor} & {mod.Author}", UI.ModCategoryStyles.ModAuthorStyle);
                    GUILayout.Label(mod.Description, UI.ModCategoryStyles.ModDescriptionStyle);
                    GUILayout.EndVertical();
                    GUILayout.Space(4);
                }
            });

            GUILayout.Space(10);

            mbm_all_in_one.src.modules.utils.UIUtils.DrawSection("Experimental Mods:", experimentalLabelStyle, () => {
                foreach (var mod in _experimentalMods)
                {
                    GUILayout.BeginVertical(UI.ModCategoryStyles.ModCardStyle);
                    GUILayout.Label($"<color=#b3b3b3ff>{mod.Name}</color> <size=12><color=#888888ff>v{mod.Version}</color></size>", UI.ModCategoryStyles.ModNameStyle);
                    GUILayout.Label($"by {mod.OriginalAuthor} & {mod.Author}", UI.ModCategoryStyles.ModAuthorStyle);
                    GUILayout.Label(mod.Description, UI.ModCategoryStyles.ModDescriptionStyle);
                    GUILayout.EndVertical();
                    GUILayout.Space(4);
                }
            });

            GUILayout.Space(10);

            mbm_all_in_one.src.modules.utils.UIUtils.DrawSection("Broken Mods:", brokenLabelStyle, () => {
                foreach (var mod in _brokenMods)
                {
                    GUILayout.BeginVertical(UI.ModCategoryStyles.ModCardStyle);
                    GUILayout.Label($"<color=#b3b3b3ff>{mod.Name}</color> <size=12><color=#888888ff>v{mod.Version}</color></size>", UI.ModCategoryStyles.ModNameStyle);
                    GUILayout.Label($"by {mod.OriginalAuthor} & {mod.Author}", UI.ModCategoryStyles.ModAuthorStyle);
                    GUILayout.Label(mod.Description, UI.ModCategoryStyles.ModDescriptionStyle);
                    GUILayout.EndVertical();
                    GUILayout.Space(4);
                }
            });

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }
    }
}