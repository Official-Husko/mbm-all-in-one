namespace mbm_all_in_one.src.modules.mods
{
    public class ModSettingInfo
    {
        public string Key { get; set; } // Unique key for the setting
        public string Label { get; set; } // Label to show in UI
        public string Type { get; set; } // "float" or "string"
        public float DefaultFloat { get; set; } // Default value if float
        public float Min { get; set; } // Min value for float
        public float Max { get; set; } // Max value for float
        public string DefaultString { get; set; } // Default value if string
    }
}
