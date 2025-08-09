using HarmonyLib;
using MBMScripts;
using System;
using UnityEngine;

namespace mbm_all_in_one.src.modules.mods.NoTitsLimit
{
    // Stateless, in-memory, modular version of NoTitsLimit
    public class NoTitsLimitPlugin
    {
        // Cache the field accessor for m_TitsType
        private static readonly AccessTools.FieldRef<Character, int> m_TitsTypeRef = AccessTools.FieldRefAccess<Character, int>("m_TitsType");

        private readonly Func<bool> _getEnabled;
        private bool _isPatched;
        private Harmony _harmony;
        private const string HarmonyId = NoTitsLimitInfo.Guid;

        public NoTitsLimitPlugin(Func<bool> getEnabled)
        {
            _getEnabled = getEnabled;
        }

        public void Init()
        {
            if (_getEnabled() && !_isPatched)
            {
                _harmony = new Harmony(HarmonyId);
                _harmony.PatchAll(typeof(NoTitsLimitPlugin));
                Debug.Log("[NoTitsLimit] Harmony patches applied.");
                _isPatched = true;
            }
        }

        public void Disable()
        {
            if (_isPatched && _harmony != null)
            {
                _harmony.UnpatchSelf();
                Debug.Log("[NoTitsLimit] Harmony patches removed.");
                _isPatched = false;
            }
        }

        [HarmonyPatch(typeof(Character), "TitsType", MethodType.Getter)]
        [HarmonyPrefix]
        public static bool Character_get_TitsType(ref int __result, Character __instance)
        {
            // Remove the hardcoded limit by directly accessing the field
            __result = m_TitsTypeRef(__instance);
            return false;
        }
    }
}
