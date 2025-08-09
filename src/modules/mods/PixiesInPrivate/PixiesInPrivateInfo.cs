using mbm_all_in_one.src.modules.mods;
using System.Collections.Generic;

namespace mbm_all_in_one.src.modules.mods.PixiesInPrivate
{
    /// <summary>
    /// Static info and registration for the PixiesInPrivate mod.
    /// </summary>
    public static class PixiesInPrivateInfo
    {
        public const string Name = "PixiesInPrivate";
        public const string Author = "paw_beans";
        public const string OriginalAuthor = "guyverek";
        public const string Version = "3.0.0";
        public const string Guid = "com.mbmaio.PixiesInPrivate";
        public const string Description = "Allows Pixies to be used in private rooms.";
        public const string Category = "Stable";
        public static readonly List<string> IncompatibleWith = new List<string> { "com.mbmaio.HealthPixyPlus" };
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
            register(Name, ModInfo, () => new PixiesInPrivatePlugin(getEnabled));
        }
    }
}
