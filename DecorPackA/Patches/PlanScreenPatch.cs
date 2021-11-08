using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;

namespace DecorPackA.Patches
{
    class PlanScreenPatch
    {
        // Makes the Copy Building button target default glass in the build menu.
        [HarmonyPatch(typeof(PlanScreen), "OnClickCopyBuilding")]
        public static class PlanScreen_OnClickCopyBuilding_Patch
        {
            public static void Prefix()
            {
                if (SelectTool.Instance.selected is object)
                {
                    if (SelectTool.Instance.selected.TryGetComponent(out Building building))
                    {
                        if (building.HasTag(ModAssets.Tags.stainedGlass) && building.Def.name != DefaultStainedGlassTileConfig.ID)
                        {
                            OpenBuildMenu(building);
                        }
                    }
                }
            }

            // Open build menu to a specific Building type
            private static void OpenBuildMenu(Building building)
            {
                foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
                {
                    if (planInfo.data.Contains(DefaultStainedGlassTileConfig.ID))
                    {
                        BuildingDef defaultStainedDef = Assets.GetBuildingDef(DefaultStainedGlassTileConfig.ID);

                        PlanScreen.Instance.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
                        PlanScreen.Instance.OnSelectBuilding(PlanScreen.Instance.ActiveToggles[defaultStainedDef].gameObject, defaultStainedDef);

                        var infoScreen = Traverse.Create(PlanScreen.Instance).Field<ProductInfoScreen>("productInfoScreen");

                        if (infoScreen == null) return;

                        infoScreen.Value.materialSelectionPanel.SelectSourcesMaterials(building);

                        if (building.TryGetComponent(out Rotatable rotatable))
                        {
                            BuildTool.Instance.SetToolOrientation(rotatable.GetOrientation());
                        }

                        return;
                    }
                }
            }
        }
    }
}
