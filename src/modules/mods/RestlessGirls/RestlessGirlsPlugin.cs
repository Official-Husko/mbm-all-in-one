using System;
using UnityEngine;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.RestlessGirls
{
    /// <summary>
    /// Modular RestlessGirls logic, only started if enabled by the loader.
    /// </summary>
    public class RestlessGirlsPlugin
    {
        private float _backup;
        private RestlessGirlsService _service;
        private Func<bool> _getEnabled;
        private Func<float> _getRestTime;

        public RestlessGirlsPlugin(Func<bool> getEnabled, Func<float> getRestTime)
        {
            _getEnabled = getEnabled;
            _getRestTime = getRestTime;
        }

        public void Init()
        {
            DebugPrint("[RestlessGirls] Enabled");
            ConfigureActions();
        }

        public void Disable()
        {
            DebugPrint("[RestlessGirls] Disabled");
        }

        public void UpdateSettings()
        {
            if (_service != null)
            {
                // If needed, update service with new rest time, etc.
            }
        }

        private void ConfigureActions()
        {
            if (_service == null)
            {
                _service = new RestlessGirlsService(_getRestTime(), _getEnabled(), _backup);
            }
        }

        private void DebugPrint(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
    }
}
