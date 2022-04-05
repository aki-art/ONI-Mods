using AsphaltStairs.Buildings;
using FUtility;
using HarmonyLib;

namespace AsphaltStairs.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, AsphaltStairsConfig.ID, Consts.SUB_BUILD_CATEGORY.Base.LADDERS, FirePoleConfig.ID);
                BuildingUtil.AddToResearch(AsphaltStairsConfig.ID, Consts.TECH.SOLIDS.REFINED_OBJECTS);
            }
        }
    }
}