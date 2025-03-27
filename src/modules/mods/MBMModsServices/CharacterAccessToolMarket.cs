using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    // Token: 0x02000007 RID: 7
    public class CharacterAccessToolMarket : CharacterAccessTool
    {
        // Token: 0x06000010 RID: 16 RVA: 0x000021B4 File Offset: 0x000003B4
        public static void RegisterActionsForNewCharacterInMarket(Action<Character> action)
        {
            ActionsForNewCharactersInMarket.Add(action);
        }

        // Token: 0x06000011 RID: 17 RVA: 0x000021C1 File Offset: 0x000003C1
        public static void RegisterActionsForFirstCharacters(Action<Character> action)
        {
            ActionsForFirstCharacters.Add(action);
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000021D0 File Offset: 0x000003D0
        internal new static void SetupAccessTool()
        {
            STARTING_DAY = 2;
            Action action = __ExecuteActionsForNewMarketCharacters ??= new Action(ExecuteActionsForNewMarketCharacters);
            ToolsPlugin.RegisterMarketAction(action, true);

            Action action2 = __ExecuteActionsForFirstCharacters ??= new Action(ExecuteActionsForFirstCharacters);
            _first = ToolsPlugin.RegisterMarketAction(action2, true);
        }

        // Token: 0x06000013 RID: 19 RVA: 0x0000222C File Offset: 0x0000042C
        private static void ExecuteActionsForNewMarketCharacters()
        {
            PlayData pd = ToolsPlugin.PD;
            if (pd != null && pd.DayState == EDayState.Day)
            {
                using (IEnumerator<Female> enumerator = Females.Values.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Female female = enumerator.Current;
                        ProcessAllCharactersInMarket(female);
                    }
                    return;
                }
            }
            PlayData pd2 = ToolsPlugin.PD;
            if (pd2 != null && pd2.DayState == EDayState.Night)
            {
                foreach (Male male in Males.Values)
                {
                    ProcessAllCharactersInMarket(male);
                }
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000022E4 File Offset: 0x000004E4
        private static void ExecuteActionsForFirstCharacters()
        {
            int num = 0;
            bool flag = ToolsPlugin.PD != null;
            if (flag)
            {
                num = ToolsPlugin.PD.Days;
            }
            if (hasGameJustStarterd() && flag && (double)ToolsPlugin.PD.DayGauge >= 0.05 && (double)ToolsPlugin.PD.DayGauge <= 0.1)
            {
                foreach (Female female in Females.Values)
                {
                    ProcessFirstUnitsNotInUpdatedMarkedList(female);
                }
                foreach (Male male in Males.Values)
                {
                    ProcessFirstUnitsNotInUpdatedMarkedList(male);
                }
            }
            if (_first != null && _first.enabled && num > STARTING_DAY)
            {
                _first.enabled = false;
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000023E8 File Offset: 0x000005E8
        private static void ProcessAllCharactersInMarket(Character character)
        {
            if (AlreadyProcessedMarketCharacters.Contains(character.UnitId) || character.Sector != ESector.Market)
            {
                return;
            }
            foreach (Action<Character> action in ActionsForNewCharactersInMarket)
            {
                action(character);
            }
            AlreadyProcessedMarketCharacters.Enqueue(character.UnitId);
            if (AlreadyProcessedMarketCharacters.Count > 8)
            {
                AlreadyProcessedMarketCharacters.Dequeue();
            }
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002478 File Offset: 0x00000678
        private static void ProcessFirstUnitsNotInUpdatedMarkedList(Character character)
        {
            bool flag = character.UnitType == EUnitType.Goblin || character.UnitType == EUnitType.Human;
            if (AlreadyProcessedTutorialCharacters.Contains(character.UnitId) || !flag)
            {
                return;
            }
            foreach (Action<Character> action in ActionsForFirstCharacters)
            {
                action(character);
            }
            AlreadyProcessedTutorialCharacters.Enqueue(character.UnitId);
            if (AlreadyProcessedTutorialCharacters.Count > 8)
            {
                AlreadyProcessedTutorialCharacters.Dequeue();
            }
        }

        // Token: 0x04000009 RID: 9
        public static Queue<int> AlreadyProcessedMarketCharacters = new Queue<int>(10);

        // Token: 0x0400000A RID: 10
        public static Queue<int> AlreadyProcessedTutorialCharacters = new Queue<int>(10);

        // Token: 0x0400000B RID: 11
        private static readonly IList<Action<Character>> ActionsForNewCharactersInMarket = new List<Action<Character>>();

        // Token: 0x0400000C RID: 12
        private static readonly IList<Action<Character>> ActionsForFirstCharacters = new List<Action<Character>>();

        public static Action __ExecuteActionsForNewMarketCharacters;

        // Token: 0x04000064 RID: 100
        public static Action __ExecuteActionsForFirstCharacters;
    }
}