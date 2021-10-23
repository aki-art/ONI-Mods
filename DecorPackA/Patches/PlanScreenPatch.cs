using DecorPackA.DPBuilding.StainedGlassTile;
using HarmonyLib;
using UnityEngine;

namespace DecorPackA.Patches
{
    class PlanScreenPatch
    {
        // Makes the Copy Building button target default glass in the build menu.
        [HarmonyPatch(typeof(PlanScreen), "OnClickCopyBuilding")]
        public static class PlanScreen_OnClickCopyBuilding_Patch
        {
            public static void Prefix(GameObject ___copyBuildingButton)
            {
                if (SelectTool.Instance.selected is object)
                {
                    if (SelectTool.Instance.selected.TryGetComponent(out Building building))
                    {
                        if (building.HasTag(ModAssets.Tags.stainedGlass) && building.Def.name != DefaultStainedGlassTileConfig.ID)
                        {
                            var tempBuilding = (Building)Object.Instantiate((Object)building, building.transform, false);
                            tempBuilding.Def = Assets.GetBuildingDef(DefaultStainedGlassTileConfig.ID);
                            PlanScreen.Instance.CopyBuildingOrder(tempBuilding);
                            Object.Destroy(tempBuilding);

                            ___copyBuildingButton.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
