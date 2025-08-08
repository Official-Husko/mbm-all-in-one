using BepInEx.Configuration;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.RestlessGirls
{
    /// <summary>
    /// Service logic for RestlessGirls mod.
    /// </summary>
    public class RestlessGirlsService
    {
        private readonly ConfigEntry<float> _restTime;
        private readonly ConfigEntry<bool> _enabled;
        private readonly float _backup;

        public RestlessGirlsService(ConfigEntry<float> restTime, ConfigEntry<bool> enabled, float backup)
        {
            _restTime = restTime;
            _enabled = enabled;
            _backup = backup;
        }

        public float GetRestTime()
        {
            if (_restTime != null && _restTime.Value > 0f && _enabled != null && _enabled.Value)
                return _restTime.Value;
            return _backup;
        }
    }
}
