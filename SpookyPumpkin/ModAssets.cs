using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using SpookyPumpkin.Settings;

namespace SpookyPumpkin
{
    public class ModAssets
    {
        public static string ModPath;
        public const string PREFIX = "SP_";
        public const string spookedEffectID = "SP_Spooked";

        public class Prefabs
        {
            public static GameObject sideScreenPrefab;
            public static GameObject settingsDialogPrefab;
        }

        public static void Initialize(string path)
        {
            ModPath = path;
        }

        internal static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("sp_uiasset");
            Prefabs.sideScreenPrefab = bundle.LoadAsset<GameObject>("GhostPipSideScreen");
            FUtility.FUI.TMPConverter.ReplaceAllText(Prefabs.sideScreenPrefab);
        }


        public static void WriteSettingsToFile(object obj, string filename)
        {
            var filePath = Path.Combine(ModAssets.ModPath, filename + ".json");
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    var serializedUserSettings = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    sw.Write(serializedUserSettings);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't write to {filePath}, {e.Message}");
            }
        }       

        public static List<string> ReadPipWorlds()
        {
            var filePath = Path.Combine(ModPath, "pipworlds.json");
            List<string> userSettings = new List<string>();

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    userSettings = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}. Using default settings.");
                return new List<string>();
            }

            return userSettings;
        }

        public static UserSettings ReadUserSettings(string filename)
        {
            var filePath = Path.Combine(ModPath, filename + ".json");
            UserSettings userSettings = new UserSettings();

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    userSettings = JsonConvert.DeserializeObject<UserSettings>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}. Using default settings.");
                return new UserSettings();
            }

            return userSettings;
        }
    }
}
