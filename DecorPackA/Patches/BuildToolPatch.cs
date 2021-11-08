using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Patches
{
    class BuildToolPatch
    {
        // Change building def places by build tool based on what material is selected
        [HarmonyPatch(typeof(BuildTool), "Activate")]
        public static class BuildTool_Activate_Patch
        {
            public static void Prefix(BuildTool __instance, ref BuildingDef def, IList<Tag> selected_elements)
            {
                if (def.PrefabID == DefaultStainedGlassTileConfig.ID)
                {
                    RemoveVisualizer(__instance);
                    if (selected_elements.Contains(SimHashes.Iron.CreateTag()))
                    {
                        def = Assets.GetBuildingDef(IronSGTConfig.ID);
                    }

                    foreach (Tag tag in selected_elements)
                    {
                        if (ModAssets.tiles.TryGetValue(tag, out Tag buildingTag))
                        {
                            def = Assets.GetBuildingDef(buildingTag.ToString());
                            break;
                        }
                    }
                }
            }

            // this prevents ghost preview blocks from appearing
            private static void RemoveVisualizer(BuildTool __instance)
            {
                if (__instance.visualizer != null)
                {
                    Traverse.Create(__instance).Method("ClearTilePreview").GetValue();
                    Object.Destroy(__instance.visualizer);
                }
            }
        }
    }
}
