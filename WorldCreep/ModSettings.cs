using FUtility;
using Newtonsoft.Json;
using System;
using System.IO;
using WorldCreep.Settings;

namespace WorldCreep
{
    public class ModSettings
    {
        public static MeteorSettings Meteors;
        public static GenericSettings Generic;
        public static TraitSettings Traits;
        public static WorldEventSettings WorldEvents;

        private static readonly string settingsRoot = Path.Combine(ModAssets.ModPath, "settings");
        private static readonly string eventsConfigPath = Path.Combine(settingsRoot, "worldevents.json");
        private static readonly string traitsConfigPath = Path.Combine(settingsRoot, "traits.json");
        private static readonly string genericConfigpath = Path.Combine(settingsRoot, "generic.json");
        private static readonly string meteorConfigpath = Path.Combine(settingsRoot, "meteors.json");

        public static void Load()
        {
            LoadGlobalEventSettings();

            Generic = TryReadJSON(genericConfigpath, out string json)
                ? JsonConvert.DeserializeObject<GenericSettings>(json, ConverterSettings)
                : new GenericSettings();
        }

        public static void LoadGlobalEventSettings()
        {
            if (TryReadJSON(eventsConfigPath, out string json))
            {
                WorldEvents = JsonConvert.DeserializeObject<WorldEventSettings>(json, ConverterSettings);
            }
            else
            {
                WorldEvents = new WorldEventSettings();
            }
        }

        public static void SaveGlobalEventSettings()
        {
            if (WorldEvents == null)
            {
                Log.Debuglog("Nothing to save here");
                return;
            }

            string world = JsonConvert.SerializeObject(WorldEvents, ConverterSettings);
            File.WriteAllText(eventsConfigPath, world);
        }

        public static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };

        private static bool TryReadJSON(string path, out string result, bool log = true)
        {
            result = default;
            try
            {
                result = File.ReadAllText(path);
                return true;
            }
            catch (Exception e)
            {
                if (log)
                    Log.Warning($"Couldn't read {path}, {e.Message}. Using defaults.");
                return false;
            }
        }

    }
}
