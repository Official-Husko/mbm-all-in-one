using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.RestlessGirls
{
    /// <summary>
    /// Service logic for RestlessGirls mod.
    /// </summary>
    public class RestlessGirlsService
    {
        private readonly float _restTime;
        private readonly bool _enabled;
        private readonly float _backup;

        public RestlessGirlsService(float restTime, bool enabled, float backup)
        {
            _restTime = restTime;
            _enabled = enabled;
            _backup = backup;
        }

        public float GetRestTime()
        {
            if (_restTime > 0f && _enabled)
                return _restTime;
            return _backup;
        }
    }
}
