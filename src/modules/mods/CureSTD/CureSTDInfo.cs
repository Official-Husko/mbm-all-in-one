using System;
using mbm_all_in_one.src.modules.mods;
using System.Collections.Generic;

namespace mbm_all_in_one.src.modules.mods.CureSTD
{
    /// <summary>
    /// Static info and registration for the CureSTD mod.
    /// </summary>
    public static class CureSTDInfo
    {
        public const string Name = "CureSTD";
        public const string Author = "paw_beans";
        public const string Version = "3.0.0";
        public const string Guid = "com.mbmaio.CureSTD";
        public const string Description = "Automatically cures STDs from owned characters.";
        public const string Category = "Stable";
        public const string OriginalAuthor = "SoapBoxHero";
        public static readonly List<string> IncompatibleWith = new List<string> { }; // This mod is standalone and does not conflict with others
        public const bool RequiresRestart = false;

        /// <summary>
        /// ModInfo property for reflection-based mod registration.
        /// </summary>
        public static ModInfo ModInfo => new ModInfo
        {
            Name = Name,
            Author = Author,
            OriginalAuthor = OriginalAuthor,
            Version = Version,
            Description = Description,
            Category = Category,
            IncompatibleWith = IncompatibleWith,
            RequiresRestart = RequiresRestart
        };

        /// <summary>
        /// Self-register this mod's info and factory with the loader.
        /// </summary>
        public static void RegisterMod(Action<string, ModInfo, Func<object>> register, Func<bool> getEnabled, Func<List<string>> getExcludePhrases)
        {
            register(Name, ModInfo, () => new CureSTDPlugin(getEnabled, getExcludePhrases));
        }
    }
}
