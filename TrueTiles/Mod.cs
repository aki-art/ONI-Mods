using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TrueTiles.Patches;
using TrueTiles.Settings;
using static Rendering.BlockTileRenderer;

namespace TrueTiles
{
    public class Mod : UserMod2
    {
        private static SaveDataManager<Config> config;

        public static Config Settings => config.Settings;

        public static string GetExternalSavePath() => Path.Combine(Util.RootFolder(), "mods", "tile_texture_packs");

        public static string GetLocalSavePath() => Path.Combine(ModPath, "tiles");

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
            GenerateData(path);

            base.OnLoad(harmony);
        }

        public static void SaveConfig()
        {
            config.Write(true);
        }

        private void Setup()
        {
            BlockTileRendererPatch.GetRenderInfoLayerMethod = AccessTools.Method(typeof(BlockTileRenderer), "GetRenderInfoLayer", new Type[] { typeof(bool), typeof(SimHashes) });
            BlockTileRendererPatch.GetRenderLayerForTileMethod = AccessTools.Method(typeof(BlockTileRendererPatch), "GetRenderLayerForTile", new Type[] { typeof(RenderInfoLayer), typeof(BuildingDef), typeof(SimHashes) });
        }

        [Conditional("DEBUG")]
        private void GenerateData(string path)
        {
            var root = FileUtil.GetOrCreateDirectory(System.IO.Path.Combine(path, "tiles"));
            new Datagen.PackDataGen(root);
            new Datagen.TileDataGen(root);
        }
    }
}
