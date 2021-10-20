using FUtility;
using FUtility.FUI;
using Newtonsoft.Json;
using SpookyPumpkin.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SpookyPumpkin
{
    public class ModAssets
    {
        public static string ModPath { get; set; }
        public const string spookedEffectID = "SP_Spooked";
        public static readonly Tag buildingPumpkinTag = TagManager.Create("SP_BuildPumpkin", STRINGS.ITEMS.FOOD.SP_PUMPKIN.NAME);
        public static Dictionary<string, bool> pipWorlds = new Dictionary<string, bool>();

        public class Prefabs
        {
            public static GameObject sideScreenPrefab;
            public static GameObject settingsDialogPrefab;
        }

        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                ModPath = path;
                Log.PrintVersion();
                ModSettings.Load();
                pipWorlds = ReadPipWorlds();
            }
        }

        public static void SetPipWorld(bool shouldExist)
        {
            string id = SaveLoader.Instance.GameInfo.colonyGuid.ToString();
            pipWorlds[id] = shouldExist;
            WriteSettingsToFile(pipWorlds, "pipworlds");
        }

        internal static void LateLoadAssets()
        {
            AssetBundle bundle = FUtility.Assets.LoadAssetBundle("sp_uiasset");
            Prefabs.sideScreenPrefab = bundle.LoadAsset<GameObject>("GhostPipSideScreen");
            Prefabs.settingsDialogPrefab = bundle.LoadAsset<GameObject>("SpookyOptions");
            //TMPConverter.ReplaceAllText(Prefabs.sideScreenPrefab);
            //TMPConverter.ReplaceAllText(Prefabs.settingsDialogPrefab);
        }

        public static void WriteSettingsToFile(object obj, string filename)
        {
            var filePath = Path.Combine(ModPath, filename + ".json");
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

        public static List<string> ReadPipTreats()
        {
            if (ReadJSON("piptreats", out string json))
                return JsonConvert.DeserializeObject<List<string>>(json);
            else return new List<string>();
        }

        public static Dictionary<string, bool> ReadPipWorlds()
        {
            if(ReadJSON("pipworlds", out string json, false))
                return JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
            else return new Dictionary<string, bool>();
        }

        public static UserSettings ReadUserSettings(string filename)
        {
            if (ReadJSON(filename, out string json))
                return JsonConvert.DeserializeObject<UserSettings>(json);
            else return new UserSettings(); ;
        }

        private static bool ReadJSON(string filename, out string json, bool log = true)
        {
            json = null;
            try
            {
                using (var r = new StreamReader(Path.Combine(ModPath, filename + ".json")))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                if(log) Log.Warning($"Couldn't read {filename}.json, {e.Message}. Using defaults.");
                return false;
            }

            return true;
        }
    }
}
