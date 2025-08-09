using mbm_all_in_one.src.modules.mods;
using System.Collections.Generic;

namespace mbm_all_in_one.src.modules.mods.NoTitsLimit
{
    /// <summary>
    /// Static info and registration for the NoTitsLimit mod.
    /// </summary>
    public static class NoTitsLimitInfo
    {
        public const string Name = "NoTitsLimit";
        public const string Author = "paw_beans";
        public const string OriginalAuthor = "Surgy";
        public const string Version = "3.0.0";
        public const string Guid = "com.mbmaio.NoTitsLimit";
        public const string Description = "Removes the hardcoded limit on character breast size.";
        public const string Category = "Stable";
        public static readonly List<string> IncompatibleWith = new List<string> { };
        public const bool RequiresRestart = false;

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

        public static void RegisterMod(System.Action<string, ModInfo, System.Func<object>> register, System.Func<bool> getEnabled)
        {
            register(Name, ModInfo, () => new NoTitsLimitPlugin(getEnabled));
        }
    }
}
