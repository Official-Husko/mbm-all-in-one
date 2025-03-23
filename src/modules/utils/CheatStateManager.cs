using System.Collections.Generic;

namespace mbm_all_in_one.src.modules.utils
{
    public static class CheatStateManager
    {
        private static readonly Dictionary<string, bool> _cheatStates = new();

        public static bool IsCheatActive(string cheatName)
        {
            return _cheatStates.TryGetValue(cheatName, out bool isActive) && isActive;
        }

        public static void ToggleCheatState(string cheatName)
        {
            if (_cheatStates.ContainsKey(cheatName))
            {
                _cheatStates[cheatName] = !_cheatStates[cheatName];
            }
            else
            {
                _cheatStates[cheatName] = true;
            }
        }
    }
} 