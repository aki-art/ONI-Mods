namespace SpookyPumpkin.Settings
{
    public class ModSettings
    {
        public static UserSettings Settings { get; set; }
        private const string FILENAME = "config";

        public static void Load()
        {
            Settings = ModAssets.ReadUserSettings(FILENAME);
        }

        public static void Save()
        {
            ModAssets.WriteSettingsToFile(Settings, FILENAME);
        }
    }
}
