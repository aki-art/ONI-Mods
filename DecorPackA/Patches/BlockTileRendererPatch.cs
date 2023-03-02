﻿using Buildings.StainedGlassTile;
using HarmonyLib;
using Rendering;
using System.Reflection;

namespace Patches
{
    public class BlockTileRendererPatch
    {
        private const string NEUTRONIUM_ALLOY_TILEDEF = "DecorPackA_UnobtaniumAlloyStainedGlassTile";

        [HarmonyPatch]
        public class BlockTileRenderer_RenderInfo_Ctor_Patch
        {
            public static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("Rendering.BlockTileRenderer+RenderInfo");
                return AccessTools.Constructor(type, new[] { typeof(BlockTileRenderer), typeof(int), typeof(int), typeof(BuildingDef), typeof(SimHashes) });
            }

            public static void Postfix(
                BlockTileRenderer.RenderInfo __instance,
                int queryLayer,
                BuildingDef def,
                SimHashes element)
            {
                if (def.PrefabID == NEUTRONIUM_ALLOY_TILEDEF && queryLayer == (int)def.TileLayer && element != SimHashes.Void)
                {
                    Log.Debuglog("render info neutronium");
                    DecorPackAGlassShineColors.neutroniumAlloyMaterial = __instance.material;
                }
            }
        }
    }
}