using System.Collections.Generic;

namespace mbm_all_in_one.src.modules.mods
{
    public class ModInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string OriginalAuthor { get; set; } // For mods that are based on or inspired by other mods
        public string Version { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // e.g. "Stable", "Experimental", "Broken"
        public List<string> IncompatibleWith { get; set; } = new(); // List of incompatible mod GUIDs
        public bool RequiresRestart { get; set; } // If true, enabling/disabling requires restart

        // Optional: List of settings for this mod (for UI/config)
        public List<ModSettingInfo> Settings { get; set; } = null;
    }
}
