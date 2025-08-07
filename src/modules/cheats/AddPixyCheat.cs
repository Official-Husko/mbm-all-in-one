using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using mbm_all_in_one.src.Managers;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddPixyCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Pixy";
        public CheatType Type => CheatType.ExecuteWithInput;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            // Logic to add pixy
            GameManager.Instance.PlayerData.FloraPixyCount += amount;
            GameManager.Instance.AddSystemMessage($"Pixy added: {amount}");
        }

        public void Register(mbm_all_in_one.src.Managers.CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
}