using DecorPackA.DPBuilding.StainedGlassTile;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DecorPackA.Patches
{
    class BlockTileRendererPatch
    {
        [HarmonyPatch(typeof(Rendering.BlockTileRenderer))]
        [HarmonyPatch("GetCellColour")]
        public static class BlockTileRenderer_GetCellColour_Patch
        {
            public static void Postfix(int cell, SimHashes element, ref Color __result)
            {
                if(Mod.colorOverlays.TryGetValue(cell, out Color color))
                {
                    __result *= color;
                }
            }
        }
    }
}
