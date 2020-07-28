using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FUtility;

namespace Slag.Buildings
{
    class BuildingPatches
    {
        [HarmonyPatch(typeof(PlanScreen))]
        [HarmonyPatch("OnRecipeElementsFullySelected")]
        public static class PlanScreen_OnRecipeElementsFullySelected_Patch
        {
            public static bool Prefix(PlanScreen __instance, KToggle ___currentlySelectedToggle, ProductInfoScreen ___productInfoScreen)
            {
                BuildingDef def = __instance.ActiveToggles.FirstOrDefault(t => t.Value == ___currentlySelectedToggle).Key;
                if (def.name == GlassTileConfig.ID)
                {
                    var elements = ___productInfoScreen.materialSelectionPanel.GetSelectedElementAsList;

                    if (elements.Contains("SlagGlass"))
                    {
                        var newDef = Assets.GetBuildingDef(SlagGlassTileConfig.ID);
                        var tool = PlayerController.Instance.ActiveTool;
                        if (newDef == null || tool == null) return true;

                        Type tool_type = tool.GetType();
                        if (tool_type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(tool_type))
                        {
                            tool.DeactivateTool();
                        }

                        BuildTool.Instance.Activate(newDef, elements);
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PlanScreen), "OnClickCopyBuilding")]
        public static class PlanScreen_OnClickCopyBuilding_Patch
        {
            public static void Prefix()
            {
                Building building = SelectTool.Instance.selected.GetComponent<Building>();
                if (building == null) return;

                if (building.Def.name == SlagGlassTileConfig.ID)
                {
                    building.gameObject.SetActive(false);

                    var buildingDefault = UnityEngine.Object.Instantiate(building);
                    buildingDefault.gameObject.SetActive(true);
                    buildingDefault.Def = Assets.GetBuildingDef(GlassTileConfig.ID);

                    PlanScreen.Instance.CopyBuildingOrder(buildingDefault);

                    buildingDefault.gameObject.SetActive(false);
                    UnityEngine.Object.Destroy(buildingDefault);

                    GameObject copyBuildingButton = Traverse.Create(PlanScreen.Instance).Field("copyBuildingButton").GetValue<GameObject>();
                    copyBuildingButton.SetActive(false);
                }
            }
        }
    }
}
