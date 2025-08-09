using System;
using UnityEngine;
using System.Collections.Generic;
using mbm_all_in_one.src.modules.mods.CureSTD;
using mbm_all_in_one.src.modules.mods.MBMModsServices;
using MBMScripts;


namespace mbm_all_in_one.src.modules.mods.CureSTD
{
    /// <summary>
    /// Modular CureSTD logic, only started if enabled by the loader.
    /// </summary>
    public class CureSTDPlugin
    {
        private static PeriodicAction _cvd;
        private static float _period = 3f;
        private CureSTDService _service;
        private Func<bool> _getEnabled;
        private Func<List<string>> _getExcludePhrases;

        public CureSTDPlugin(Func<bool> getEnabled, Func<List<string>> getExcludePhrases)
        {
            _getEnabled = getEnabled;
            _getExcludePhrases = getExcludePhrases;
        }

        public void Init()
        {
            DebugPrint("[CureSTD] Enabled");
            ConfigureActions();
        }

        public void Disable()
        {
            DebugPrint("[CureSTD] Disabled");
            if (_cvd != null) _cvd.enabled = false;
        }

        public void UpdateSettings()
        {
            if (_cvd != null)
            {
                _cvd.enabled = _getEnabled();
            }
            if (_service != null)
            {
                // If needed, update service with new exclude phrases, etc.
            }
        }

        private void ConfigureActions()
        {
            if (_cvd != null)
            {
                _cvd.enabled = _getEnabled();
                DebugPrint($"[CureSTD] {(_getEnabled() ? "Enabled" : "Disabled")} CureSTD action.");
                return;
            }
            _service = new CureSTDService(
                msg => DebugPrint("[CureSTD] " + msg),
                () => CharacterAccessTool.GetOwnedFemales(),
                () => CharacterAccessTool.GetOwnedMales(),
                _getExcludePhrases,
                ToolsPlugin.GameMessage);
            _cvd = ToolsPlugin.RegisterPeriodicAction(_period, _service.Run, _getEnabled());
            DebugPrint($"[CureSTD] Registered CureSTD action for period of {_period} sec.");
        }

        private void DebugPrint(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
    }
}
