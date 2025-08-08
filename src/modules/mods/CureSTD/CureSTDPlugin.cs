using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using mbm_all_in_one.src.modules.mods.CureSTD;
using mbm_all_in_one.src.modules.mods.MBMModsServices;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.CureSTD
{
    /// <summary>
    /// BepInEx plugin entry for CureSTD.
    /// </summary>
    [BepInPlugin(CureSTDInfo.Guid, CureSTDInfo.Name, CureSTDInfo.Version)]
    public class CureSTDPlugin : BaseUnityPlugin
    {
        public ConfigEntry<string> ExcludePhrase { get; private set; }
        public ConfigEntry<bool> Enabled { get; private set; }
        private static PeriodicAction _cvd;
        private static float _period = 3f;
        private CureSTDService _service;

        /// <summary>
        /// Static ModInfo property for reflection-based mod registration.
        /// </summary>
        public static ModInfo ModInfo => CureSTDInfo.ModInfo;

        /// <summary>
        /// Initializes config and service.
        /// </summary>
        public CureSTDPlugin()
        {
            Enabled = Config.Bind("General", "Enabled", true, "Enables CureSTD plugin.");
            Enabled.SettingChanged += Enabled_Changed;
            ExcludePhrase = Config.Bind("General", "ExcludePhrase", "sickly", "If this exact phrase is in a character's name, they won't be cured.");
        }

        private void Enabled_Changed(object sender, EventArgs e)
        {
            ConfigureActions();
        }

        private void ConfigureActions()
        {
            if (_cvd != null)
            {
                _cvd.enabled = Enabled.Value;
                Logger.LogInfo($"{(Enabled.Value ? "Enabled" : "Disabled")} CureSTD action.");
                return;
            }
            _service = new CureSTDService(Logger,
                () => CharacterAccessTool.GetOwnedFemales(),
                () => CharacterAccessTool.GetOwnedMales(),
                () => ExcludePhrase.Value,
                ToolsPlugin.GameMessage);
            _cvd = ToolsPlugin.RegisterPeriodicAction(_period, _service.Run, Enabled.Value);
            Logger.LogMessage($"Registered CureSTD action for period of {_period} sec.");
        }

        public void Awake()
        {
            ConfigureActions();
        }
    }
}
