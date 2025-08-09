using System;
using System.Collections.Generic;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.CureSTD
{
    /// <summary>
    /// Service logic for curing STDs from owned characters.
    /// </summary>
    public class CureSTDService
    {
        private readonly Action<string> _log;
        private readonly Func<IEnumerable<Female>> _getFemales;
        private readonly Func<IEnumerable<Male>> _getMales;
    private readonly Func<List<string>> _excludePhrases;
        private readonly Action<string, string> _gameMessage;

        /// <summary>
        /// Initializes the service with required dependencies.
        /// </summary>
        public CureSTDService(Action<string> log,
            Func<IEnumerable<Female>> getFemales,
            Func<IEnumerable<Male>> getMales,
            Func<List<string>> excludePhrases,
            Action<string, string> gameMessage)
        {
            _log = log;
            _getFemales = getFemales;
            _getMales = getMales;
            _excludePhrases = excludePhrases;
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
            var excludeList = _excludePhrases();
            if (excludeList != null && excludeList.Count > 0)
            {
                foreach (var phrase in excludeList)
                {
                    if (!string.IsNullOrEmpty(phrase) && character.DisplayName.Contains(phrase)) return;
                }
            }
            character.VenerealDisease = false;
            _gameMessage($"Curing STD of {character.DisplayName}...", "E07369");
            _log?.Invoke($"Cured STD for {character.DisplayName}");
        }
    }
}
