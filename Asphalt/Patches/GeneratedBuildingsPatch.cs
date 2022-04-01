using Asphalt.Buildings;
using FUtility;
using HarmonyLib;

namespace Asphalt.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, AsphaltTileConfig.ID, Consts.SUB_BUILD_CATEGORY.Base.TILES, MetalTileConfig.ID);
                BuildingUtil.AddToResearch(AsphaltTileConfig.ID, Consts.TECH.POWER.IMPROVED_COMBUSTION);
            }
        }
    }
}
