using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src.modules.cheats
{
    public class ExecuteBrainwashSpecialNpcsCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Brainwash Special NPCs";
        public CheatType Type => CheatType.Execute;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            GameManager.Instance.BrainwashAmilia();
            GameManager.Instance.BrainwashBarbara();
            GameManager.Instance.BrainwashFlora();
            GameManager.Instance.BrainwashLena();
            GameManager.Instance.BrainwashNiel();
            GameManager.Instance.BrainwashSena();
            GameManager.Instance.AddSystemMessage("Special NPCs brainwashed");
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
} 