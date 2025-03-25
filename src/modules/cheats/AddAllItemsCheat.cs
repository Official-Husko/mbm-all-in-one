using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using UnityEngine;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddAllItemsCheat : ICheat, IRegisterableCheat
    {
        public string Name => "All Items";
        public CheatType Type => CheatType.ExecuteWithInput;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            foreach (var itemType in ItemUtils.GetAllItemTypes())
            {
                if (itemType == EItemType.None)
                {
                    // Skip none item type
                    continue;
                }

                Debug.Log($"Adding {itemType} to inventory");
                try
                {
                    GameManager.Instance.PlayerData.NewItem(itemType, ESector.Inventory, new ValueTuple<int, int>(0, 0), -1, amount);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to add {itemType}: {ex.Message}");
                }
            }
            GameManager.Instance.PlayerData.NewItem(EItemType.Item_HumanDna, ESector.Inventory, new ValueTuple<int, int>(0, 0), -1, amount);
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
} 