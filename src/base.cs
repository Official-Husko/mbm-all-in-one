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
using mbm_all_in_one.src.modules.mods.CureSTD;
using mbm_all_in_one.src.modules.mods.RestlessGirls;
using mbm_all_in_one.src.modules.mods.NoTitsLimit;
using mbm_all_in_one.src.modules.mods.PixiesInPrivate;

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
        // Scroll positions for mod categories
        private Vector2 _stableModsScroll;
        private Vector2 _experimentalModsScroll;
        private Vector2 _brokenModsScroll;
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
        private static Dictionary<string, ModInfo> _allModInfos = new();
        private static Dictionary<string, Func<object>> _modFactories = new();

        // In-memory mod state/settings (stateless)
        private Dictionary<string, bool> _modEnabledStates = new();
        private Dictionary<string, string> _modStringSettings = new();
        private Dictionary<string, float> _modFloatSettings = new();

        private bool _restartRequired = false;

        // Called by each mod's RegisterMod
        private static void RegisterMod(string name, ModInfo info, Func<object> factory)
        {
            _allModInfos[name] = info;
            _modFactories[name] = factory;
        }

        // Register mods with correct delegates for live settings
        private void RegisterAllMods()
        {
            // CureSTD
            CureSTDInfo.RegisterMod(
                (name, info, factory) => RegisterMod(name, info, factory),
                () => _modEnabledStates.TryGetValue("CureSTD", out var en) && en,
                () => {
                    var csv = _modStringSettings.TryGetValue("curestd_exclude_phrases", out var s) ? s : "sickly";
                    var arr = csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var list = new List<string>();
                    foreach (var str in arr) { var trimmed = str.Trim(); if (!string.IsNullOrEmpty(trimmed)) list.Add(trimmed); }
                    return list;
                }
            );
            // RestlessGirls
            RestlessGirlsInfo.RegisterMod(
                (name, info, factory) => RegisterMod(name, info, factory),
                () => _modEnabledStates.TryGetValue("RestlessGirls", out var en) && en,
                () => _modFloatSettings.TryGetValue("restlessgirls_resttime", out var f) ? f : 5f
            );
            // NoTitsLimit
            NoTitsLimitInfo.RegisterMod(
                (name, info, factory) => RegisterMod(name, info, factory),
                () => _modEnabledStates.TryGetValue("NoTitsLimit", out var en) && en
            );
            // PixiesInPrivate
            PixiesInPrivateInfo.RegisterMod(
                (name, info, factory) => RegisterMod(name, info, factory),
                () => _modEnabledStates.TryGetValue("PixiesInPrivate", out var en) && en
            );
        }

        private void Start()
        {
            RegisterAllMods();

            // Populate mod category lists for UI
            _stableMods = _allModInfos.Values.Where(m => string.Equals(m.Category, "Stable", StringComparison.OrdinalIgnoreCase)).ToList();
            _experimentalMods = _allModInfos.Values.Where(m => string.Equals(m.Category, "Experimental", StringComparison.OrdinalIgnoreCase)).ToList();
            _brokenMods = _allModInfos.Values.Where(m => string.Equals(m.Category, "Broken", StringComparison.OrdinalIgnoreCase)).ToList();

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
            var stableLabelStyle = UI.ModCategoryStyles.StableLabelStyle;
            var experimentalLabelStyle = UI.ModCategoryStyles.ExperimentalLabelStyle;
            var brokenLabelStyle = UI.ModCategoryStyles.BrokenLabelStyle;

            // Helper to draw mod cards with toggles, settings, and status
            void DrawModList(IEnumerable<ModInfo> mods)
            {
                foreach (var mod in mods)
                {
                    GUILayout.BeginVertical(UI.ModCategoryStyles.ModCardStyle);
                    GUILayout.Label($"<color=#b3b3b3ff>{mod.Name}</color> <size=12><color=#888888ff>v{mod.Version}</color></size>", UI.ModCategoryStyles.ModNameStyle);
                    GUILayout.Label($"by {mod.OriginalAuthor} & {mod.Author}", UI.ModCategoryStyles.ModAuthorStyle);
                    GUILayout.Label(mod.Description, UI.ModCategoryStyles.ModDescriptionStyle);

                    // Enable/disable toggle in menu
                    bool prev = _modEnabledStates.TryGetValue(mod.Name, out var enabled) ? enabled : false;
                    bool next = GUILayout.Toggle(prev, prev ? "Enabled" : "Disabled");
                    if (next != prev)
                    {
                        _modEnabledStates[mod.Name] = next;
                        if (_modFactories.TryGetValue(mod.Name, out var factory))
                        {
                            var instance = factory();
                            var initMethod = instance.GetType().GetMethod("Init");
                            var disableMethod = instance.GetType().GetMethod("Disable");
                            if (next && initMethod != null) initMethod.Invoke(instance, null);
                            if (!next && disableMethod != null) disableMethod.Invoke(instance, null);
                        }
                    }

                    // Show status
                    GUILayout.Label(next ? "<color=green>Enabled</color>" : "<color=red>Disabled</color>");

                    // Show mod settings if enabled
                    object modInstance = null;
                    if (_modFactories.TryGetValue(mod.Name, out var modFactory))
                    {
                        modInstance = modFactory();
                    }
                    if (next && mod.Settings != null)
                    {
                        foreach (var setting in mod.Settings)
                        {
                            if (setting.Type == "float")
                            {
                                float val = _modFloatSettings.TryGetValue($"{mod.Name}_{setting.Key}", out var f) ? f : setting.DefaultFloat;
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(setting.Label, GUILayout.Width(200));
                                float newVal = GUILayout.HorizontalSlider(val, setting.Min, setting.Max, GUILayout.Width(120));
                                if (Math.Abs(newVal - val) > 0.01f)
                                {
                                    _modFloatSettings[$"{mod.Name}_{setting.Key}"] = newVal;
                                    val = newVal;
                                    // Notify mod of setting change
                                    modInstance?.GetType().GetMethod("UpdateSettings")?.Invoke(modInstance, null);
                                }
                                GUILayout.Label($"{val:F1}", GUILayout.Width(60));
                                GUILayout.EndHorizontal();
                            }
                            else if (setting.Type == "string")
                            {
                                string sval = _modStringSettings.TryGetValue($"{mod.Name}_{setting.Key}", out var s) ? s : setting.DefaultString;
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(setting.Label, GUILayout.Width(200));
                                string newSval = GUILayout.TextField(sval, GUILayout.Width(200));
                                if (newSval != sval)
                                {
                                    _modStringSettings[$"{mod.Name}_{setting.Key}"] = newSval;
                                    // Notify mod of setting change
                                    modInstance?.GetType().GetMethod("UpdateSettings")?.Invoke(modInstance, null);
                                }
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                    // Legacy hardcoded settings for backward compatibility
                    if (next)
                    {
                        if (mod.Name == "RestlessGirls" && (mod.Settings == null || !mod.Settings.Any(s => s.Key == "resttime")))
                        {
                            float restTime = _modFloatSettings.TryGetValue("restlessgirls_resttime", out var f) ? f : 5f;
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Rest Time:", GUILayout.Width(100));
                            float newVal = GUILayout.HorizontalSlider(restTime, 1f, 64f, GUILayout.Width(120));
                            if (Math.Abs(newVal - restTime) > 0.01f)
                            {
                                _modFloatSettings["restlessgirls_resttime"] = newVal;
                                restTime = newVal;
                            }
                            GUILayout.Label($"{restTime:F1} sec", GUILayout.Width(60));
                            GUILayout.EndHorizontal();
                        }
                        if (mod.Name == "CureSTD" && (mod.Settings == null || !mod.Settings.Any(s => s.Key == "exclude_phrases")))
                        {
                            string excludeCsv = _modStringSettings.TryGetValue("curestd_exclude_phrases", out var s) ? s : "sickly";
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Exclude Phrases (comma separated):", GUILayout.Width(200));
                            string newCsv = GUILayout.TextField(excludeCsv, GUILayout.Width(200));
                            if (newCsv != excludeCsv)
                            {
                                _modStringSettings["curestd_exclude_phrases"] = newCsv;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.Space(4);
                }
            }

            mbm_all_in_one.src.modules.utils.UIUtils.DrawSection("Stable Mods:", stableLabelStyle, () => {
                if (_stableMods.Count > 0) {
                    _stableModsScroll = GUILayout.BeginScrollView(_stableModsScroll, GUILayout.Height(300));
                    DrawModList(_stableMods);
                    GUILayout.EndScrollView();
                }
            });
            GUILayout.Space(10);
            mbm_all_in_one.src.modules.utils.UIUtils.DrawSection("Experimental Mods:", experimentalLabelStyle, () => {
                if (_experimentalMods.Count > 0) {
                    _experimentalModsScroll = GUILayout.BeginScrollView(_experimentalModsScroll, GUILayout.Height(300));
                    DrawModList(_experimentalMods);
                    GUILayout.EndScrollView();
                }
            });
            GUILayout.Space(10);
            mbm_all_in_one.src.modules.utils.UIUtils.DrawSection("Broken Mods:", brokenLabelStyle, () => {
                if (_brokenMods.Count > 0)
                {
                    _brokenModsScroll = GUILayout.BeginScrollView(_brokenModsScroll, GUILayout.Height(300));
                    DrawModList(_brokenMods);
                    GUILayout.EndScrollView();
                }
            });
            GUILayout.EndVertical();
        }
    }
}