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
                BuildingUtil.AddToPlanScreen(
                    AsphaltTileConfig.ID,
                    Consts.BUILD_CATEGORY.BASE,
                    MetalTileConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Base.TILES);

                BuildingUtil.AddToResearch(AsphaltTileConfig.ID, Consts.TECH.POWER.IMPROVED_COMBUSTION);
            }
        }
    }
}
