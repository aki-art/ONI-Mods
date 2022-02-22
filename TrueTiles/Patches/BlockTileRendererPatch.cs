using FUtility;
using HarmonyLib;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using static Rendering.BlockTileRenderer;

namespace TrueTiles.Patches
{
    public class BlockTileRendererPatch
    {
        public static MethodInfo GetRenderInfoLayerMethod;
        public static MethodInfo GetRenderLayerForTileMethod;

        [HarmonyPatch(typeof(BlockTileRenderer), MethodType.Constructor)]
        public static class BlockTileRenderer_Ctor_Patch
        {
            public static void Postfix()
            {
                GetRenderInfoLayerMethod = AccessTools.Method(typeof(BlockTileRenderer), "GetRenderInfoLayer", new Type[] { typeof(bool), typeof(SimHashes) });
                GetRenderLayerForTileMethod = AccessTools.Method(typeof(BlockTileRendererPatch), "GetRenderLayerForTile", new Type[] { typeof(RenderInfoLayer), typeof(BuildingDef), typeof(SimHashes) });
            }
        }

        [HarmonyPatch(typeof(BlockTileRenderer), "AddBlock")]
        public static class Rendering_BlockTileRenderer_AddBlock_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                int index = FindEntryPoint(codes);

                // didn't find anything, return original
                if (index == -1)
                {
                    return codes;
                }

                //insert after
                index++;

                // RenderInfoLayer is loaded to stack
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_2)); // load BuildingDef
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_S, 4)); // load SimHashes
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, GetRenderLayerForTileMethod));  // call GetRenderLayerForTile

                Log.PrintInstructions(codes);
                return codes;
            }
        }

        [HarmonyPatch(typeof(BlockTileRenderer), "RemoveBlock")]
        public static class Rendering_BlockTileRenderer_RemoveBlock_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                var index = FindEntryPoint(codes);

                if (index == -1)
                {
                    return codes;
                }

                index++;

                // RenderInfoLayer is loaded to stack
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_1)); // load BuildingDef
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_3)); // load SimHashes
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, GetRenderLayerForTileMethod)); // call GetRenderLayerForTile

                Log.PrintInstructions(codes);
                return codes;
            }
        }

        private static int FindEntryPoint(List<CodeInstruction> codes)
        {
            return codes.FindIndex(c => c.operand is MethodInfo m && m == GetRenderInfoLayerMethod);
        }

        internal static RenderInfoLayer GetRenderLayerForTile(RenderInfoLayer layer, BuildingDef def, SimHashes elementId)
        {
            if (layer == RenderInfoLayer.Built && TileAssets.Instance.ContainsDef(def.PrefabID))
            {
                // Assign an element specific render info layer with a random offset so there is no overlap
                return (RenderInfoLayer)(elementId + 451);
            }

            return layer;
        }
    }
}
