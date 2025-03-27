using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class ExecuteClearKeepingRoomCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Clear Keeping Room";
        public CheatType Type => CheatType.Execute;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            GameManager.Instance.PlayerData.ClearKeepingRoom();
            GameManager.Instance.AddSystemMessage("Cleared Keeping Room");
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
} 