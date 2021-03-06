﻿using FUtility;
using System.IO;
using System.Reflection;
using UnityEngine;
using Attribute = Klei.AI.Attribute;
using Newtonsoft.Json;
using System;

namespace TransparentAluminium
{
    class ModAssets
    {
        public static string ModPath;
        public static StatusItem UpgradeableSolarWattStatus { get; set; }

        public static readonly SimHashes transparentAluminumHash = (SimHashes)Hash.SDBMLower("TransparentAluminum");
        public static readonly Tag TransparentAluminum = TagManager.Create("TransparentAluminum");
        public static Attribute HardnessAttribute = new Attribute("Armored", true, Attribute.Display.General, false);
        public static Database.SpaceDestinationType AquaPlanet;
        public static Sprite aquaPlanetSprite;

        public static object JsonConvert { get; private set; }

        internal static void MakeStatusItem()
        {
            var item = new StatusItem(
                           id: "UpgradeableSolarPanelWattage",
                           prefix: "BUILDING",
                           icon: "",
                           icon_type: StatusItem.IconType.Info,
                           notification_type: NotificationType.Neutral,
                           allow_multiples: false,
                           render_overlay: OverlayModes.Power.ID);

           item.resolveStringCallback = (str, data) => {
                UpgradeableSolarPanel solarPanel = (UpgradeableSolarPanel)data;
                str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(solarPanel.CurrentWattage));
                return str;
            };

            UpgradeableSolarWattStatus = item;
        }

        public static void LateLoadAssets()
        {
            Texture2D tex = GetTexture("aqua_planet");
            if(tex != null)
            {
                aquaPlanetSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                Assets.Sprites.Add("aquaplanet", aquaPlanetSprite);
            }
            else
            {
                Log.Warning("Could not load Aqua Planets sprite.");
                Assets.Sprites.Add("aquaplanet", Assets.GetSprite("asteroid"));
            }
        }

        public static TextureAtlas GetCustomAtlas(string fileName, TextureAtlas tileAtlas)
        {
            var tex = GetTexture(fileName, tileAtlas.texture.width, tileAtlas.texture.height);
            if (tex == null) return null;

            TextureAtlas atlas;
            atlas = ScriptableObject.CreateInstance<TextureAtlas>();
            atlas.texture = tex;
            atlas.vertexScale = tileAtlas.vertexScale;
            atlas.items = tileAtlas.items;

            return atlas;
        }

        public static Texture2D GetTexture(string name, int width = 1, int height = 1)
        {
            Texture2D tex = null;
            string texFile = Path.Combine(ModPath, "assets", name) + ".png";

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                tex = new Texture2D(width, height);
                tex.LoadImage(data);
            }

            return tex;
        }
        public static void WriteSettingsToFile(object obj, string filename)
        {
            var filePath = Path.Combine(ModPath, filename + ".json");
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    var serializedUserSettings = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented);
                    sw.Write(serializedUserSettings);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't write to {filePath}, {e.Message}");
            }
        }

        public static bool ReadJSON(string filename, out string json, bool log = true)
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
                if (log) Log.Warning($"Couldn't read {filename}.json, {e.Message}. Using defaults.");
                return false;
            }

            return true;
        }
    }
}
