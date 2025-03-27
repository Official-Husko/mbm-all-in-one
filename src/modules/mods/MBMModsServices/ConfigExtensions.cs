using System;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    // Token: 0x0200000A RID: 10
    public static class ConfigExtensions
    {
        // Token: 0x06000022 RID: 34 RVA: 0x000026CE File Offset: 0x000008CE
        public static ConfigEntry<T> Bind<T>(this ConfigFile file, ConfigInfo<T> info)
        {
            return file.Bind(new ConfigDefinition(info.Section, info.Name), info.DefaultValue, new ConfigDescription(info.Description, info.AcceptableValues, Array.Empty<object>()));
        }
    }
}
