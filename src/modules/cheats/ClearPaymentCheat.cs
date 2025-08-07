using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using mbm_all_in_one.src.Managers;

namespace mbm_all_in_one.src.modules.cheats
{
    public class ClearPaymentCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Clear Payment";
        public CheatType Type => CheatType.Execute;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            // Logic to add the specified amount of gold
            PlayData.Instance.PaySeqList.Clear();
            GameManager.Instance.AddSystemMessage("Payment cleared");
        }

        public void Register(mbm_all_in_one.src.Managers.CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
}