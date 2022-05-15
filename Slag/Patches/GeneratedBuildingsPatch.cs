using FUtility;
using HarmonyLib;
using Slag.Content.Buildings.InsulatedWindowTile;
using Slag.Content.Buildings.Spinner;

namespace Slag.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(
                    Consts.BUILD_CATEGORY.BASE,
                    InsulatedWindowTileConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Base.TILES,
                    GlassTileConfig.ID);

                ModUtil.AddBuildingToPlanScreen(
                    Consts.BUILD_CATEGORY.REFINING,
                    SpinnerConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Refining.MATERIALS,
                    RockCrusherConfig.ID);

                //BuildingUtil.AddToResearch(InsulatedWindowTileConfig.ID, Consts.TECH.);
            }
        }
    }
}
