using HarmonyLib;
using MBMScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace mbm_all_in_one.src.modules.mods.PixiesInPrivate
{
    // Stateless, in-memory, modular version of PixiesInPrivate
    public class PixiesInPrivatePlugin
    {
        private readonly Func<bool> _getEnabled;
        private bool _isPatched;
        private Harmony _harmony;
        private const string HarmonyId = PixiesInPrivateInfo.Guid;

        private static readonly MethodInfo mIP = AccessTools.Method(typeof(Unit), "get_IsPrivate");
        private static readonly MethodInfo mIS = AccessTools.Method(typeof(Unit), "get_IsSelected");
        private static readonly MethodInfo GCRL = AccessTools.Method(typeof(PlayData), "GetClosedRoomList");

        public PixiesInPrivatePlugin(Func<bool> getEnabled)
        {
            _getEnabled = getEnabled;
        }

        public void Init()
        {
            if (_getEnabled() && !_isPatched)
            {
                _harmony = new Harmony(HarmonyId);
                _harmony.PatchAll(typeof(PixiesInPrivatePlugin));
                Debug.Log("[PixiesInPrivate] Harmony patches applied.");
                _isPatched = true;
            }
        }

        public void Disable()
        {
            if (_isPatched && _harmony != null)
            {
                _harmony.UnpatchSelf();
                Debug.Log("[PixiesInPrivate] Harmony patches removed.");
                _isPatched = false;
            }
        }

        [HarmonyPatch(typeof(Female), "UpdatePixy")]
        public static class UpdatePixyPatch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                // Access the mod's enabled/debug state via static delegates if needed
                var enabled = true; // Default to true for static context; replace with delegate if needed
                var debug = false;
                // If you want to wire up live settings, use static delegates or events

                if (!enabled)
                {
                    return instructions;
                }
                bool flag = false;
                bool flag2 = false;
                List<CodeInstruction> list = new List<CodeInstruction>();
                foreach (CodeInstruction codeInstruction in instructions)
                {
                    if (!flag && (codeInstruction.operand as MethodInfo) != null && ((MethodInfo)codeInstruction.operand).Equals(mIP))
                    {
                        codeInstruction.operand = mIS;
                        flag = true;
                    }
                    if (!flag2 && (codeInstruction.operand as MethodInfo) != null && ((MethodInfo)codeInstruction.operand).Equals(GCRL))
                    {
                        if (list.Count > 0) list.Remove(list.Last());
                        list.Add(new CodeInstruction(OpCodes.Ldarg_0));
                        list.Add(new CodeInstruction(OpCodes.Callvirt, mIP));
                        flag2 = true;
                    }
                    list.Add(codeInstruction);
                }
                if (debug)
                {
                    // Optionally log opcodes for debugging
                    Debug.Log("[PixiesInPrivate] Patched opcodes: " + string.Join(", ", list.Select(ci => ci.opcode.ToString())));
                }
                return list.AsEnumerable();
            }
        }
    }
}
