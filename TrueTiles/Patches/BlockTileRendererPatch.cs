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
        protected static Dictionary<KeyValuePair<BuildingDef, SimHashes>, object> renderInfos = new Dictionary<KeyValuePair<BuildingDef, SimHashes>, object>();

        [HarmonyPatch]
        public static class Rendering_BlockTileRenderer_Patch
        {
            // Private type so it needs to be targeted like this
            public static MethodBase TargetMethod()
            {
                Type type = AccessTools.TypeByName("Rendering.BlockTileRenderer+RenderInfo");
                return AccessTools.Constructor(type, new Type[] { typeof(BlockTileRenderer), typeof(int), typeof(int), typeof(BuildingDef), typeof(SimHashes) });
            }

            public static void Postfix(BuildingDef def, SimHashes element, Material ___material, object ___decorRenderInfo)
            {
                bool overrides = ModAssets.texturesByName.TryGetValue(def.PrefabID.ToLowerInvariant(), out Dictionary<string, TileTextures> skins);

                if (overrides && skins.TryGetValue(element.ToString().ToLower(), out TileTextures tex))
                {
                    if(tex.main != null)
                    {
                        ___material.SetTexture("_MainTex", tex.main);
                    }

                    if (tex.spec != null)
                    {
                        ___material.SetTexture("_SpecularTex", tex.spec);
                        ___material.EnableKeyword("ENABLE_SHINE");
                    }

                    if(tex.top != null || tex.topSpec != null)
                    {
                        // private type also so needs to be traversed
                        Material topMaterial = Traverse.Create(___decorRenderInfo).Field<Material>("material").Value;

                        if (tex.top != null)
                        {
                            topMaterial.SetTexture("_MainTex", tex.top);
                        }

                        if (tex.topSpec != null)
                        {
                            topMaterial.SetTexture("_SpecularTex", tex.topSpec);
                            topMaterial.EnableKeyword("ENABLE_SHINE");
                        }
                    }
                }
            }
        }

        private static RenderInfoLayer GetRenderLayerForTile(RenderInfoLayer layer, BuildingDef def, SimHashes element)
        {
            if (layer == RenderInfoLayer.Built && ModAssets.texturesByName.ContainsKey(def.PrefabID.ToLower()))
            {
                // Assign an element specific render info layer with a random offset so there is no overlap
                return (RenderInfoLayer)(element + 451);
            }

            return layer;
        }

        [HarmonyPatch(typeof(BlockTileRenderer), "AddBlock")]
        public static class Rendering_BlockTileRenderer_AddBlock_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var GetRenderInfoLayer = AccessTools.Method(typeof(BlockTileRenderer), "GetRenderInfoLayer", new Type[] { typeof(bool), typeof(SimHashes) });
                var GetRenderLayerForTile = AccessTools.Method(typeof(BlockTileRendererPatch), "GetRenderLayerForTile", new Type[] { typeof(RenderInfoLayer), typeof(BuildingDef), typeof(SimHashes) });

                var codes = orig.ToList();
                var index = codes.FindIndex(c => c.operand is MethodInfo m && m == GetRenderInfoLayer);

                // didn't find anything, return original
                if (index == -1) return codes;

                //insert after
                index++;

                // RenderInfoLayer is loaded to stack
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_2)); // load BuildingDef
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_S, 4)); // load SimHashes
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, GetRenderLayerForTile));

                Log.PrintInstructions(codes);
                return codes;
            }
        }


        [HarmonyPatch(typeof(BlockTileRenderer), "RemoveBlock")]
        public static class Rendering_BlockTileRenderer_RemoveBlock_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var GetRenderInfoLayer = AccessTools.Method(typeof(BlockTileRenderer), "GetRenderInfoLayer", new Type[] { typeof(bool), typeof(SimHashes) });
                var GetRenderLayerForTile = AccessTools.Method(typeof(BlockTileRendererPatch), "GetRenderLayerForTile", new Type[] { typeof(RenderInfoLayer), typeof(BuildingDef), typeof(SimHashes) });

                var codes = orig.ToList();
                var index = codes.FindIndex(c => c.operand is MethodInfo m && m == GetRenderInfoLayer);

                // didn't find anything, return original
                if (index == -1) return codes;

                //insert after
                index++;

                // RenderInfoLayer is loaded to stack
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_1)); // load BuildingDef
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_3)); // load SimHashes
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, GetRenderLayerForTile));

                Log.PrintInstructions(codes);
                return codes;
            }
        }
    }
}
