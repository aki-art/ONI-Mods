using DecorPackB.Buildings.FossilDisplay;
using DecorPackB.Buildings.Fountain;
using DecorPackB.Buildings.OilLantern;
using HarmonyLib;
using FUtility;

namespace DecorPackB.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtil.AddToPlanScreen(FountainConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, FloorLampConfig.ID);
                BuildingUtil.AddToPlanScreen(FossilDisplayConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, FountainConfig.ID);
                BuildingUtil.AddToPlanScreen(GiantFossilDisplayConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, FossilDisplayConfig.ID);
                BuildingUtil.AddToPlanScreen(OilLanternConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);

                BuildingUtil.AddToResearch(FountainConfig.ID, Consts.TECH.DECOR.FINEART);
                BuildingUtil.AddToResearch(FossilDisplayConfig.ID, Consts.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
                BuildingUtil.AddToResearch(GiantFossilDisplayConfig.ID, Consts.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
                BuildingUtil.AddToResearch(OilLanternConfig.ID, Consts.TECH.POWER.IMPROVED_COMBUSTION);
            }
        }
    }
}
