using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class ExecuteUnlockEstateCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Unlock Estate";
        public CheatType Type => CheatType.Execute;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            GameManager.Instance.PlayerData.Home = true;
            GameManager.Instance.AddSystemMessage("Estate unlocked");
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
} 