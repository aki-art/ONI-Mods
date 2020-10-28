using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentAluminium
{
    public class GlassTilePatches
    {
        // replaces the building tool to build transparent aluminium glass instead of regular glass
        [HarmonyPatch(typeof(BuildTool), "Activate")]
        public static class BuildTool_Activate_Patch
        {
            public static void Prefix(BuildTool __instance, ref BuildingDef def, IList<Tag> selected_elements)
            {
                if (def.PrefabID == GlassTileConfig.ID)
                {
                    RemoveVisualizer(__instance);
                    if (selected_elements.Contains(ModAssets.TransparentAluminum))
                        def = Assets.GetBuildingDef(TransparentAluminiumTileConfig.ID);
                }
            }

            // this prevents ghost preview blocks from appearing
            private static void RemoveVisualizer(BuildTool __instance)
            {
                if (__instance.visualizer != null)
                {
                    Traverse.Create(__instance).Method("ClearTilePreview").GetValue();
                    UnityEngine.Object.Destroy(__instance.visualizer);
                }
            }
        }

        // Makes the Copy Building button target default glass in the build menu.
        [HarmonyPatch(typeof(PlanScreen), "OnClickCopyBuilding")]
        public static class PlanScreen_OnClickCopyBuilding_Patch
        {
            public static void Prefix(GameObject ___copyBuildingButton)
            {
                if (SelectTool.Instance.selected != null)
                {
                    Building building = SelectTool.Instance.selected.GetComponent<Building>();
                    if (building != null && building.Def.name == TransparentAluminiumTileConfig.ID)
                    {
                        var tempBuilding = UnityEngine.Object.Instantiate(building);
                        tempBuilding.Def = Assets.GetBuildingDef(GlassTileConfig.ID);
                        PlanScreen.Instance.CopyBuildingOrder(tempBuilding);
                        UnityEngine.Object.Destroy(tempBuilding);
                        ___copyBuildingButton.SetActive(false);
                    }
                }
            }
        }
    }
}
