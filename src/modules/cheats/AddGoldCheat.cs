using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddGoldCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Gold";
        public CheatType Type => CheatType.ExecuteWithInput;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            // Logic to add the specified amount of gold
            GameManager.Instance.PlayerData.Gold += amount;
            GameManager.Instance.AddSystemMessage($"Gold added: {amount}");
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
} 