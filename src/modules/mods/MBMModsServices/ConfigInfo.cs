using System;
using System.Runtime.CompilerServices;
using BepInEx.Configuration;

namespace mbm_all_in_one.src.modules.mods.MBMModsServices
{
    public struct ConfigInfo<T>
    {
        // Token: 0x0400000F RID: 15
        public string Section;

        // Token: 0x04000010 RID: 16
        public string Name;

        // Token: 0x04000011 RID: 17
        public string Description;

        // Token: 0x04000012 RID: 18
        public T DefaultValue;

        // Token: 0x04000013 RID: 19
        public AcceptableValueBase AcceptableValues;
    }
}
