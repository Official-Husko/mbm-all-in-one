using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddSoulCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Soul";
        public CheatType Type => CheatType.ExecuteWithInput;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            // Logic to add the specified amount of gold
            GameManager.Instance.PlayerData.Soul += amount;
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
} 