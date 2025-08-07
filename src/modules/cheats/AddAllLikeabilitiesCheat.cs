using System;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using mbm_all_in_one.src.Managers;

namespace mbm_all_in_one.src.modules.cheats
{
    public class AddAllLikeabilitiesCheat : ICheat, IRegisterableCheat
    {
        public string Name => "All Likeabilities";
        public CheatType Type => CheatType.Execute;
        public Tab DisplayTab => Tab.Player;

        public void Execute(int amount)
        {
            // Fetch all available likeabilities
            var likeabilities = LikeabilitiesUtils.FetchAvailableLikeabilities();

            // Set each likeability to a high value
            foreach (var likeability in likeabilities)
            {
                PlayData.Instance.SetLikeability(likeability, 9999);
                GameManager.Instance.AddSystemMessage($"Likeability {likeability} set to 9999");
            }

        }

        public void Register(mbm_all_in_one.src.Managers.CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }
    }
}
