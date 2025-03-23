using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddPixyCheat : ICheat
    {
        public string Name => "Add Pixy";

        public void Execute()
        {
            // Logic to add pixy
            GameManager.Instance.PlayerData.FloraPixyCount += 100;
        }
    }
} 