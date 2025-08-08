using mbm_all_in_one.src.modules.mods;

namespace mbm_all_in_one.src.modules.mods.RestlessGirls
{
    /// <summary>
    /// Static info and registration for the RestlessGirls mod.
    /// </summary>
    public static class RestlessGirlsInfo
    {
        public const string Name = "RestlessGirls";
        public const string Author = "paw_beans";
        public const string OriginalAuthor = "SoapBoxHero";
        public const string Version = "3.0.0";
        public const string Guid = "com.mbmaio.RestlessGirls";
        public const string Description = "Reduces rest time for girls before starting a new activity.";
        public const string Category = "Stable";

        /// <summary>
        /// ModInfo property for reflection-based mod registration.
        /// </summary>
        public static ModInfo ModInfo => new ModInfo {
            Name = Name,
            Author = Author,
            OriginalAuthor = OriginalAuthor,
            Version = Version,
            Description = Description,
            Category = Category
        };
    }
}
