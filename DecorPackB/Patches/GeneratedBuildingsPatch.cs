using DecorPackB.Content.Buildings.FossilDisplays;
using FUtility;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                // ModUtil.AddBuildingToPlanScreen(FountainConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, FloorLampConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, FossilDisplayConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, FloorLampConfig.ID);
                //ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, GiantFossilDisplayConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, FossilDisplayConfig.ID);
                // BuildingUtil.AddToPlanScreen(OilLanternConfig.ID, Consts.BUILD_CATEGORY.FURNITURE, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);

                //BuildingUtil.AddToResearch(FountainConfig.ID, Consts.TECH.DECOR.FINEART);
                BuildingUtil.AddToResearch(FossilDisplayConfig.ID, Consts.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
                //BuildingUtil.AddToResearch(GiantFossilDisplayConfig.ID, Consts.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
                // BuildingUtil.AddToResearch(OilLanternConfig.ID, Consts.TECH.POWER.IMPROVED_COMBUSTION)
            }
        }
    }
}