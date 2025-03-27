using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using JetBrains.Annotations;
using mbm_all_in_one.src.modules.mods.MBMModsServices;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods
{
    [BepInPlugin("com.mbmaio.CureSTD", "CureSTD", "3.0.0")]
    public class CureSTD : BaseUnityPlugin
    {
        public static string og_author_string = "SoapBoxHero"; // Original author of the mod
        public static string author_string = "paw_beans"; // Person who converted the mod to be compatible with MBMAIOMM
        public static string name_string = "CureSTD"; // Name of the mod
        public static string guid_string = "com.mbmaio.CureSTD"; // GUID of the mod
        public static string version_string = "3.0.0"; // Version of the mod

        public static ManualLogSource log;
        public ConfigEntry<string> ExcludePhrase;
        public ConfigEntry<bool> Enabled;

        private static PeriodicAction _cvd = null;
        private static float _period = 3f;

        public static IDictionary<int, Female> Females => CharacterAccessTool.Females;
        public static GameManager GM => ToolsPlugin.GM;
        public static PlayData PD => ToolsPlugin.PD;

        public CureSTD()
        {
            log = Logger;
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
            _cvd = ToolsPlugin.RegisterPeriodicAction(_period, Run, Enabled.Value);
            Logger.LogMessage($"Registered CureSTD action for period of {_period} sec.");
        }

        public void Awake()
        {
            ConfigureActions();
        }

        public void Run()
        {
            foreach (var female in CharacterAccessTool.GetOwnedFemales())
            {
                CureCharacter(female);
            }
            foreach (var male in CharacterAccessTool.GetOwnedMales())
            {
                CureCharacter(male);
            }
        }

        private void CureCharacter(Character character)
        {
            if (character != null && character.VenerealDisease && !character.DisplayName.Contains(ExcludePhrase.Value))
            {
                string text = $"Curing STD of {character.DisplayName}...";
                character.VenerealDisease = false;
                ToolsPlugin.GameMessage(text, "E07369");
            }
        }

        public ModMetadata GetMetadata()
        {
            return new ModMetadata
            {
                Name = "CureSTD",
                Version = version_string,
                Category = "Stable",
                Incompatibilities = new string[] { "SomeOtherMod" }
            };
        }
    }
}