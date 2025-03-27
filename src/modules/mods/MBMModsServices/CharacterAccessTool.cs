using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    public class CharacterAccessTool
    {
        // Token: 0x06000006 RID: 6 RVA: 0x0000209D File Offset: 0x0000029D
        [HarmonyPatch(typeof(Female), "OnTickLateUpdate")]
        [HarmonyPostfix]
        public static void FemaleCollector(Female __instance)
        {
            if (__instance != null && !Females.ContainsKey(__instance.UnitId))
            {
                Females.Add(__instance.UnitId, __instance);
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000020C2 File Offset: 0x000002C2
        [HarmonyPatch(typeof(Male), "OnTickLateUpdate")]
        [HarmonyPostfix]
        public static void MaleCollector(Male __instance)
        {
            if (__instance != null && !Males.ContainsKey(__instance.UnitId))
            {
                Males.Add(__instance.UnitId, __instance);
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000020E7 File Offset: 0x000002E7
        [HarmonyPatch(typeof(PlayData), "Load")]
        public static void BeforeLoad()
        {
            Females.Clear();
            Males.Clear();
            CharacterAccessToolMarket.AlreadyProcessedMarketCharacters.Clear();
            CharacterAccessToolMarket.AlreadyProcessedTutorialCharacters.Clear();
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002111 File Offset: 0x00000311
        public static IEnumerable<Female> GetOwnedFemales()
        {
            if (ToolsPlugin.PD == null)
            {
                yield break;
            }
            SeqList<Unit> units = ToolsPlugin.PD.GetUnitList(ESector.Female);
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (Females.TryGetValue(unit.UnitId, out Female female))
                {
                    yield return female;
                }
            }
            yield break;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x0000211A File Offset: 0x0000031A
        public static IEnumerable<Male> GetOwnedMales()
        {
            if (ToolsPlugin.PD == null)
            {
                yield break;
            }
            SeqList<Unit> units = ToolsPlugin.PD.GetUnitList(ESector.Male);
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (Males.TryGetValue(unit.UnitId, out Male male))
                {
                    yield return male;
                }
            }
            yield break;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002123 File Offset: 0x00000323
        public static void ClearUnitDictionaries()
        {
            Females.Clear();
            Males.Clear();
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002139 File Offset: 0x00000339
        internal static void SetupAccessTool()
        {
            float num = 10f;
            Action action = ClearUnitDictionaries;
            ToolsPlugin.RegisterPeriodicAction(num, action, true);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002164 File Offset: 0x00000364
        public static bool hasGameJustStarterd()
        {
            if (ToolsPlugin.PD != null)
            {
                return ToolsPlugin.PD.Days <= STARTING_DAY;
            }
            return false;
        }

        // Token: 0x04000004 RID: 4
        public static int STARTING_DAY = 5;

        // Token: 0x04000005 RID: 5
        public static IDictionary<int, Female> Females = new Dictionary<int, Female>();

        // Token: 0x04000006 RID: 6
        public static IDictionary<int, Male> Males = new Dictionary<int, Male>();

        // Token: 0x04000007 RID: 7
        public const float MaxClearTimer = 10f;

        // Token: 0x04000008 RID: 8
        public static PeriodicAction _first;
    }
}
