using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using mbm_all_in_one.src.Managers;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddReputationCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Reputation";
        public CheatType Type => CheatType.ExecuteWithInput;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            // Logic to add pixy
            GameManager.Instance.PlayerData.Reputation += amount;
            GameManager.Instance.AddSystemMessage($"Reputation added: {amount}");
        }

        public void Register(mbm_all_in_one.src.Managers.CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
}