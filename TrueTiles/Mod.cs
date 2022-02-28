using HarmonyLib;
using KMod;
using Rendering;
using System;
using TrueTiles.Patches;
using static Rendering.BlockTileRenderer;

namespace TrueTiles
{
    public class Mod : UserMod2
    {
        public static string ModPath { get; private set; }

        public override void OnLoad(Harmony harmony)
        {
            ModPath = path;

            Setup();

            base.OnLoad(harmony);
        }

        private static void Setup()
        {
            BlockTileRendererPatch.GetRenderInfoLayerMethod = AccessTools.Method(typeof(BlockTileRenderer), "GetRenderInfoLayer", new Type[] { typeof(bool), typeof(SimHashes) });
            BlockTileRendererPatch.GetRenderLayerForTileMethod = AccessTools.Method(typeof(BlockTileRendererPatch), "GetRenderLayerForTile", new Type[] { typeof(RenderInfoLayer), typeof(BuildingDef), typeof(SimHashes) });
        }
    }
}
