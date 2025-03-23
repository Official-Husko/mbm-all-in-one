using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddGoldCheat : ICheat
    {
        public string Name => "Add Gold";

        public void Execute()
        {
            // Logic to add gold
            GameManager.Instance.PlayerData.Gold += 1000;
        }
    }
} 