using Backwalls.Buildings;
using Backwalls.Cmps;
using Backwalls.Settings;
using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backwalls
{
    public class Mod : UserMod2
    {
        public static BackwallRenderer renderer;
        public static bool isTrueTilesHere;
        public static bool isNoZoneTintHere;
        public static List<BackwallPattern> variants = new List<BackwallPattern>();
        private static SaveDataManager<Config> config;
        public static Config Settings => config.Settings;

        public static void SaveSettings()
        {
            config.Write();
        }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Log.PrintVersion();

            config = new SaveDataManager<Config>(Utils.ModPath);
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach (var mod in mods)
            {
                if (mod.IsEnabledForActiveDlc())
                {
                    if (mod.staticID == "TrueTiles")
                    {
                        isTrueTilesHere = true;
                    }
                    else if (mod.staticID == "NoZoneTint")
                    {
                        isNoZoneTintHere = true;
                    }

                    if (isNoZoneTintHere && isTrueTilesHere)
                    {
                        break;
                    }
                }
            }

            if (isTrueTilesHere)
            {
                ModAssets.uiSprites = new Dictionary<string, Sprite>();
                Integration.TrueTilesPatches.Patch(harmony);
            }

            //Integration.Blueprints.BluePrintsPatch.TryPatch(harmony);

            switch (Settings.Layer)
            {
                case Config.WallLayer.Automatic:
                    // this mod doesn't have a static ID
                    var drywallMod = Type.GetType("DrywallHidesPipes.DrywallPatch, DrywallHidesPipes", false, false) != null;
                    Settings.SceneLayer = drywallMod ? Grid.SceneLayer.InteriorWall : Grid.SceneLayer.Backwall;
                    break;
                case Config.WallLayer.HidePipes:
                    Settings.SceneLayer = Grid.SceneLayer.InteriorWall;
                    break;
                case Config.WallLayer.BehindPipes:
                default:
                    Settings.SceneLayer = Grid.SceneLayer.Backwall;
                    break;
            }
        }
    }
}
