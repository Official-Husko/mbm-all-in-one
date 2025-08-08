using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MBMScripts;

namespace mbm_all_in_one.src.modules.mods.RestlessGirls
{
    [BepInPlugin(RestlessGirlsInfo.Guid, RestlessGirlsInfo.Name, RestlessGirlsInfo.Version)]
    public class RestlessGirlsPlugin : BaseUnityPlugin
    {
        public static ManualLogSource log;
        public static ConfigEntry<float> RestTime;
        public static ConfigEntry<bool> Enabled;
        private static float _backup;
        private RestlessGirlsService _service;

        /// <summary>
        /// Initializes config and service.
        /// </summary>
        public RestlessGirlsPlugin()
        {
            log = Logger;
            Enabled = Config.Bind("General", "Enabled", true, "Enables RestlessGirls plugin.");
            Enabled.SettingChanged += Enabled_Changed;
            RestTime = Config.Bind("General", "RestTime", 5f, new ConfigDescription("The time that a girl will rest before starting a new activity.", new AcceptableValueRange<float>(1f, 64f)));
            if (GameManager.ConfigData != null && _backup == 0f)
            {
                _backup = GameManager.ConfigData.RestTime;
                Inform($"Original rest time is {_backup}sec.");
            }
            _service = new RestlessGirlsService(RestTime, Enabled, _backup);
        }

        private void Message(string msg) => Logger.LogMessage(msg);
        private void Inform(string msg) => Logger.LogInfo(msg);

        private void Enabled_Changed(object sender, EventArgs e)
        {
            Message($"RestlessGirls is {(Enabled.Value ? "enabled" : "disabled")}.");
        }

        private void Awake()
        {
            Inform("Starting RestlessGirls...");
            new Harmony(RestlessGirlsInfo.Guid).PatchAll(typeof(RestlessGirlsPlugin));
            Inform("RestlessGirls Harmony patches successful.");
        }

        [HarmonyPatch(typeof(ConfigData), "RestTime", MethodType.Getter)]
        [HarmonyPrefix]
        public static bool ConfigData_get_RestTime(ref float __result, ConfigData __instance)
        {
            __result = (RestTime != null && RestTime.Value > 0f && Enabled != null && Enabled.Value) ? RestTime.Value : _backup;
            return false;
        }
    }
}
