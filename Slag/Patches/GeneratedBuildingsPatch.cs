using FUtility;
using HarmonyLib;
using Slag.Content.Buildings.InsulatedWindowTile;

namespace Slag.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, InsulatedWindowTileConfig.ID, Consts.SUB_BUILD_CATEGORY.Base.TILES, GlassTileConfig.ID);
                //BuildingUtil.AddToResearch(InsulatedWindowTileConfig.ID, Consts.TECH.);
            }
        }
    }
}
