using Asphalt.Settings;
using FUtility;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Asphalt
{
    public class ModSettings
    {
        public static UserSettings Asphalt;
        public static bool speedChanged = false;
        public static bool Uninstalling { get; set; } = false;
        private static string local;
        private static string exterior;
        private const string FILENAME = "config.json";

        public static void Read()
        {
            local = Path.Combine(ModAssets.ModPath, FILENAME);
            exterior = Path.Combine(ModAssets.ExternalSettingsPath, FILENAME);

            Asphalt = ReadJson(out string json) ? JsonConvert.DeserializeObject<UserSettings>(json) : new UserSettings();
        }

        private static bool ReadJson(out string result)
        {
            result = default;

            if (File.Exists(exterior))
                result = TryRead(exterior);
            if (result.IsNullOrWhiteSpace() && File.Exists(local))
                result = TryRead(local);

            return !result.IsNullOrWhiteSpace();
        }

        public static void Write()
        {
            try
            {
                UpdateFolder();
                string json = JsonConvert.SerializeObject(Asphalt, Formatting.Indented);
                File.WriteAllText(GetPath(), json);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning("Could not write settings. " + e.Message);
            }
        }

        private static string GetPath() => Asphalt.UseSafeFolder ? exterior : local;

        private static void UpdateFolder()
        {
            if (Asphalt.UseSafeFolder)
                CreateDirectory(ModAssets.ExternalSettingsPath);
            else
                DeleteExternalSettingsFolder();
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static string TryRead(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                return null;
            }
        }

        private static void DeleteExternalSettingsFolder()
        {
            try
            {
                if (Directory.Exists(ModAssets.ExternalSettingsPath))
                {
                    Directory.Delete(ModAssets.ExternalSettingsPath, true);
                    DirectoryInfo settingsDir = new DirectoryInfo(Path.GetDirectoryName(ModAssets.ExternalSettingsPath));
                    settingsDir.Delete(recursive: false);
                }
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                // do nothing
            }
        }
    }
}