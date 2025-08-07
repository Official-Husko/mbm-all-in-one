using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using mbm_all_in_one.src.Managers;

namespace mbm_all_in_one.src.modules.cheats
{
    public class ExecuteCallDealerCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Call Dealer";
        public CheatType Type => CheatType.Execute;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            PlayData.Instance.CallMarket(true);
            GameManager.Instance.AddSystemMessage("Dealer called");
        }

        public void Register(mbm_all_in_one.src.Managers.CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
}