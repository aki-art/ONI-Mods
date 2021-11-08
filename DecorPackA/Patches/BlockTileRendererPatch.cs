using HarmonyLib;
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
                if (Mod.colorOverlays.TryGetValue(cell, out Color color))
                {
                    __result *= color;
                }
            }
        }
    }
}
