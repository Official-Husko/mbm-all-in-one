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
    }
}
