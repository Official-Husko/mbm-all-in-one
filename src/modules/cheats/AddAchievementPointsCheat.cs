using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddAchievementPointsCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Achievement Points";
        public CheatType Type => CheatType.ExecuteWithInput;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            // Logic to add pixy
            GameManager.Instance.PlayerData.AchievementPoint += amount;
            GameManager.Instance.AddSystemMessage($"Achievement points added: {amount}");
        }
        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
} 