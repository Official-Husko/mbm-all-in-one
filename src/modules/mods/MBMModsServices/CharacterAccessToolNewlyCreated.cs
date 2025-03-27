using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    // Token: 0x02000008 RID: 8
    public class CharacterAccessToolNewlyCreated
    {
        // Token: 0x06000019 RID: 25 RVA: 0x0000254E File Offset: 0x0000074E
        public static void RegisterActionsForNewFemale(Action<Female, Character, Character> action)
        {
            ActionsForNewFemale.Add(action);
        }

        // Token: 0x0600001A RID: 26 RVA: 0x0000255B File Offset: 0x0000075B
        public static void RegisterActionsForNewMale(Action<Male, Character, Character> action)
        {
            ActionsForNewMale.Add(action);
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002568 File Offset: 0x00000768
        public static void RegisterActionsForNewCharacter(Action<Character, Character, Character> action)
        {
            ActionsForNewFemale.Add(action);
            ActionsForNewMale.Add(action);
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002580 File Offset: 0x00000780
        [HarmonyPatch(typeof(Female), "Initialize")]
        [HarmonyPatch(new Type[]
        {
            typeof(Character),
            typeof(Character)
        })]
        [HarmonyPostfix]
        public static void NewFemaleCharacterWithParents(Female __instance, Character female, Character male)
        {
            foreach (Action<Female, Character, Character> action in ActionsForNewFemale)
            {
                action(__instance, female, male);
            }
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000025CC File Offset: 0x000007CC
        [HarmonyPatch(typeof(Female), "Initialize")]
        [HarmonyPatch(new Type[] { })]
        [HarmonyPostfix]
        public static void NewFemaleCharacterWithoutParents(Female __instance)
        {
            foreach (Action<Female, Character, Character> action in ActionsForNewFemale)
            {
                action(__instance, null, null);
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002618 File Offset: 0x00000818
        [HarmonyPatch(typeof(Male), "Initialize")]
        [HarmonyPatch(new Type[]
        {
            typeof(Character),
            typeof(Character)
        })]
        [HarmonyPostfix]
        public static void NewMaleCharacterWithParents(Male __instance, Character female, Character male)
        {
            foreach (Action<Male, Character, Character> action in ActionsForNewMale)
            {
                action(__instance, female, male);
            }
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002664 File Offset: 0x00000864
        [HarmonyPatch(typeof(Male), "Initialize")]
        [HarmonyPatch(new Type[] { })]
        [HarmonyPostfix]
        public static void NewMaleCharacterWithoutParents(Male __instance)
        {
            foreach (Action<Male, Character, Character> action in ActionsForNewMale)
            {
                action(__instance, null, null);
            }
        }

        // Token: 0x0400000D RID: 13
        private static readonly IList<Action<Female, Character, Character>> ActionsForNewFemale = new List<Action<Female, Character, Character>>();

        // Token: 0x0400000E RID: 14
        private static readonly IList<Action<Male, Character, Character>> ActionsForNewMale = new List<Action<Male, Character, Character>>();
    }
}
