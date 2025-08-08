using System;
using System.Collections.Generic;
using MBMScripts;
using BepInEx.Logging;

namespace mbm_all_in_one.src.modules.mods.CureSTD
{
    /// <summary>
    /// Service logic for curing STDs from owned characters.
    /// </summary>
    public class CureSTDService
    {
        private readonly ManualLogSource _log;
        private readonly Func<IEnumerable<Female>> _getFemales;
        private readonly Func<IEnumerable<Male>> _getMales;
        private readonly Func<string> _excludePhrase;
        private readonly Action<string, string> _gameMessage;

        /// <summary>
        /// Initializes the service with required dependencies.
        /// </summary>
        public CureSTDService(ManualLogSource log,
            Func<IEnumerable<Female>> getFemales,
            Func<IEnumerable<Male>> getMales,
            Func<string> excludePhrase,
            Action<string, string> gameMessage)
        {
            _log = log;
            _getFemales = getFemales;
            _getMales = getMales;
            _excludePhrase = excludePhrase;
            _gameMessage = gameMessage;
        }

        /// <summary>
        /// Runs the STD cure logic for all owned characters.
        /// </summary>
        public void Run()
        {
            foreach (var female in _getFemales())
                CureCharacter(female);
            foreach (var male in _getMales())
                CureCharacter(male);
        }

        /// <summary>
        /// Cures a character if they have an STD and do not match the exclude phrase.
        /// </summary>
        private void CureCharacter(Character character)
        {
            if (character == null || !character.VenerealDisease) return;
            var exclude = _excludePhrase();
            if (!string.IsNullOrEmpty(exclude) && character.DisplayName.Contains(exclude)) return;
            character.VenerealDisease = false;
            _gameMessage($"Curing STD of {character.DisplayName}...", "E07369");
            _log?.LogInfo($"Cured STD for {character.DisplayName}");
        }
    }
}
