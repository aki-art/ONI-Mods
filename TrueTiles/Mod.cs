using FUtility.SaveData;
using HarmonyLib;
using KMod;
using Rendering;
using System;
using System.Collections.Generic;
using TrueTiles.Patches;
using TrueTiles.Settings;
using static Rendering.BlockTileRenderer;

namespace TrueTiles
{
    public class Mod : UserMod2
    {
        private static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public static string ModPath { get; private set; }

        public static List<string> addonPacks;

        // mods can call this with reflection to register any extra packs. 
        // make sure you do it BEFORE Db.Init. OnAllModsLoaded is the recommended entry point.
        public static void AddPack(string pack)
        {
            if(addonPacks == null)
            {
                addonPacks = new List<string>();
            }

            addonPacks.Add(pack);
        }

        public override void OnLoad(Harmony harmony)
        {
            ModPath = path;
            config = new SaveDataManager<Config>(path);
            Setup();

            base.OnLoad(harmony);
        }

        public static void SaveConfig()
        {
            config.Write();
        }

        private static void Setup()
        {
            BlockTileRendererPatch.GetRenderInfoLayerMethod = AccessTools.Method(typeof(BlockTileRenderer), "GetRenderInfoLayer", new Type[] { typeof(bool), typeof(SimHashes) });
            BlockTileRendererPatch.GetRenderLayerForTileMethod = AccessTools.Method(typeof(BlockTileRendererPatch), "GetRenderLayerForTile", new Type[] { typeof(RenderInfoLayer), typeof(BuildingDef), typeof(SimHashes) });
        }
    }
}
