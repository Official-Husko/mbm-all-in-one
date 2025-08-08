using mbm_all_in_one.src.modules.mods;

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

        /// <summary>
        /// ModInfo property for reflection-based mod registration.
        /// </summary>
        public static ModInfo ModInfo => new ModInfo {
            Name = Name,
            Author = Author,
            Version = Version,
            Description = Description,
            Category = Category
        };
    }
}
