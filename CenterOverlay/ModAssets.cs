using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static FUtility.Assets;

namespace CenterOverlay
{
    class ModAssets
    {
        public static GameObject settingsScreenPrefab;
        public static GameObject overlaySettingsButtonPrefab;
        public static string localPath;
        public static Texture2D symmetryOverlayIconTexture;
        public static Tag ModdedBuilding = TagManager.Create("A_CO_ModdedBuilding", "Modded item");
        public static List<string> vanillaBuildings;
        public static bool moddedOnLeft;
        public static int Offset { get; set; }

        public static Color moddedColor;
        public static Color vanillaColor;
        public static Color moddedFullBright;

        public static Settings Settings { get; set; }
        public static void Initialize(string path)
        {
            localPath = path;
            vanillaBuildings = LoadVanillaBuildingsList();

            Settings = ReadSettings();
            SetColors(Settings.VanillaColorHex, Settings.ModdedColorHex);
            moddedOnLeft = Settings.ModdedLeftSide;
            Offset = Settings.Offset;
        }

        public static void SetColors(string vanilla, string modded)
        {
            moddedColor = Util.ColorFromHex(modded);
            vanillaColor = Util.ColorFromHex(vanilla);
            Color.RGBToHSV(moddedColor, out float h, out _, out float v);
            moddedFullBright = Color.HSVToRGB(h, 1, Mathf.Clamp(v, 0.75f, 1f));
        }

        internal static void LateLoadAssets()
        {
            symmetryOverlayIconTexture = LoadTexture("overlay_symmetry");
            settingsScreenPrefab = LoadUIPrefab("centeroverlaysettings", "CenterOverlaySettingsDialog");
            overlaySettingsButtonPrefab = LoadUIPrefab("centeroverlaysettings", "SymmetryDiagram");

            Sprite sprite = Sprite.Create(
                texture: symmetryOverlayIconTexture,
                rect: new Rect(0, 0, symmetryOverlayIconTexture.width, symmetryOverlayIconTexture.height),
                pivot: new Vector2(128, 128));

            Assets.Sprites.Add(new HashedString("overlay_symmetry"), sprite);
        }

        private static List<string> LoadVanillaBuildingsList()
        {
            List<string> list = new List<string>();
            string filePath = Path.Combine(localPath, "assets", "buildings.json");

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    list = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}.");
                return null;
            }

            return list;
        }

        private static Settings ReadSettings()
        {
            Settings settings = new Settings();
            string filePath = Path.Combine(localPath, "symmetry_overlay_settings.json"); 
            
            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}. Using default settings.");
                return new Settings();
            }

            return settings;
        }

        public static void SaveSettings()
        {
            string filePath = Path.Combine(localPath, "symmetry_overlay_settings.json");
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    var serializedUserSettings = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                    sw.Write(serializedUserSettings);
                    Log.Info($"Settings saved to: {filePath}");
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't write to {filePath}, {e.Message}");
            }

        }
    }
}
