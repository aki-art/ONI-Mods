using CrittersDropBones.Buildings.SlowCooker;
using FUtility;
using HarmonyLib;

namespace CrittersDropBones.Patches
{
    internal class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(
                    Consts.BUILD_CATEGORY.FOOD,
                    SlowCookerConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Food.COOKING,
                    GourmetCookingStationConfig.ID);
            }
        }
    }
}
