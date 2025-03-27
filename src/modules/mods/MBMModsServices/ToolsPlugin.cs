using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using BepInEx;
using HarmonyLib;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    [BepInPlugin("com.mbmaio.mbmmodservices", "MBMModsServices", "2.0.0")]
    public class ToolsPlugin : BaseUnityPlugin
    {
        public static string og_author_string = "Memacile & SoapBoxHero"; // Original authors of the mod
        public static string author_string = "paw_beans"; // Person who converted the mod to be compatible with MBMAIOMM
        public static string name_string = "MBMModsServices"; // Name of the mod
        public static string guid_string = "com.mbmaio.mbmmodservices"; // GUID of the mod
        public static string version_string = "3.0.0"; // Version of the mod

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000025 RID: 37 RVA: 0x000029A9 File Offset: 0x00000BA9
        public static PlayData PD => PlayData.Instance;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000026 RID: 38 RVA: 0x000029B0 File Offset: 0x00000BB0
        public static GameManager GM => GameManager.Instance;

        // Token: 0x06000027 RID: 39 RVA: 0x000029B8 File Offset: 0x00000BB8
        private static bool RunMarketActions()
        {
            bool flag = false;
            foreach (PeriodicAction periodicAction in MarketActions)
            {
                try
                {
                    if (periodicAction.enabled)
                    {
                        periodicAction.perform();
                    }
                    flag |= true;
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"Error in MBM plugin MBMModsServices's market action: {ex.Message}");
                    flag = false;
                }
            }
            return flag;
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002A90 File Offset: 0x00000C90
        private static void SetDealyedMarketFlag()
        {
            if (timer != null)
            {
                return;
            }
            timer = new Timer(delegate (object obj)
            {
                ModifyMarket = true;
                timer?.Dispose();
                timer = null;
            }, null, 1000, -1);
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002ACA File Offset: 0x00000CCA
        [HarmonyPatch(typeof(PlayData), "CallMarket")]
        [HarmonyPostfix]
        public static void OnMarketUpdateCall(bool free)
        {
            SetDealyedMarketFlag();
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002AD1 File Offset: 0x00000CD1
        [HarmonyPatch(typeof(PlayData), "SetMarketRate")]
        [HarmonyPostfix]
        public static void OnMarketUpdateRate()
        {
            SetDealyedMarketFlag();
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002AD8 File Offset: 0x00000CD8
        [HarmonyPatch(typeof(PlayData), "Update")]
        [HarmonyPostfix]
        public static void OnUpdate(float deltaTime)
        {
            foreach (var keyValuePair in PeriodicActionGroups)
            {
                var value = keyValuePair.Value;
                value.timeSinceRun += deltaTime;
                if (value.timeSinceRun > value.period)
                {
                    try
                    {
                        value.Act();
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogError($"Error in MBM plugin MBMModsServices's periodic action: {ex.Message}");
                    }
                    value.timeSinceRun = 0f;
                }
            }
            if (ModifyMarket && RunMarketActions())
            {
                ModifyMarket = false;
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00002BDC File Offset: 0x00000DDC
        public ToolsPlugin()
        {
            Load();
        }

        // Token: 0x0600002D RID: 45 RVA: 0x00002BF8 File Offset: 0x00000DF8
        private void Load()
        {
            try
            {
                Harmony harmony = new Harmony("com.Memacile.SoapBoxHero.MBMModsServices");
                harmony.PatchAll(typeof(ToolsPlugin));
                harmony.PatchAll(typeof(CharacterAccessTool));
                harmony.PatchAll(typeof(CharacterAccessToolNewlyCreated));
                UnityEngine.Debug.Log("Harmony patch for MBMModsServices succeeded.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("Harmony patch for MBMModsServices failed!");
                UnityEngine.Debug.LogError(ex.Message);
            }
            CharacterAccessTool.SetupAccessTool();
            CharacterAccessToolMarket.SetupAccessTool();
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00002CE4 File Offset: 0x00000EE4
        public void Message(string msg)
        {
            Logger.LogMessage(msg);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x00002CF2 File Offset: 0x00000EF2
        public static string _T(string message)
        {
            return SeqLocalization.Localize(message);
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00002CFC File Offset: 0x00000EFC
        public static void GameMessage(string message, string color = "E07369")
        {
            string text = "\\#\\w+";
            message = Regex.Replace(message, text, match => $"<color=#{color}>{_T(match.ToString())}</color>");
            GM?.AddLogMessage(message);
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00002D44 File Offset: 0x00000F44
        public static PeriodicAction RegisterMarketAction(Action act, bool On = true)
        {
            var periodicAction = MarketActions.FirstOrDefault(a => a?.perform == act);
            if (periodicAction == null)
            {
                periodicAction = new PeriodicAction(act) { enabled = On };
                MarketActions.Add(periodicAction);
            }
            return periodicAction;
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002D98 File Offset: 0x00000F98
        public static void UnegisterMarketAction(Action act)
        {
            var periodicAction = MarketActions.FirstOrDefault(a => a?.perform == act);
            if (periodicAction != null)
            {
                MarketActions.Remove(periodicAction);
            }
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00002DD8 File Offset: 0x00000FD8
        public static PeriodicAction RegisterPeriodicAction(float period, Action act, bool On = true)
        {
            var periodicAction = new PeriodicAction(act) { enabled = On };
            if (PeriodicActionGroups.TryGetValue(period, out var periodicActionGroup))
            {
                periodicActionGroup.actions.Add(periodicAction);
            }
            else
            {
                PeriodicActionGroups.Add(period, new PeriodicActionGroup(period, periodicAction));
            }
            return periodicAction;
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00002E24 File Offset: 0x00001024
        public static void UnregisterPeriodicAction(float period, PeriodicAction act)
        {
            if (PeriodicActionGroups.TryGetValue(period, out var periodicActionGroup))
            {
                if (act != null)
                {
                    periodicActionGroup.RemoveAction(act);
                }
                if (periodicActionGroup.actions.Count == 0)
                {
                    PeriodicActionGroups.Remove(period);
                }
            }
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00002E64 File Offset: 0x00001064
        public static object GetPProperty(object inst, string name)
        {
            return Traverse.Create(inst).Field(name).GetValue();
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00002E77 File Offset: 0x00001077
        public static void SetPProperty(object inst, string name, object value)
        {
            Traverse.Create(inst).Field(name).SetValue(value);
        }

        // Token: 0x0400003E RID: 62
        public const string AUTHOR = "Memacile.SoapBoxHero";

        // Token: 0x0400003F RID: 63
        public const string GUID = "com.Memacile.SoapBoxHero.MBMModsServices";

        private static readonly IDictionary<float, PeriodicActionGroup> PeriodicActionGroups = new Dictionary<float, PeriodicActionGroup>();
        private static readonly IList<PeriodicAction> MarketActions = new List<PeriodicAction>();
        private static Timer timer = null;
        private static bool ModifyMarket = false;
    }
}
