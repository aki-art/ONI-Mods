using FUtility;
using HarmonyLib;

namespace SolidWaterPump.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtil.AddToPlanScreen(
                    Buildings.SolidWaterPumpConfig.ID,
                    Consts.BUILD_CATEGORY.PLUMBING,
                    LiquidPumpingStationConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Plumbing.PUMPS);
            }
        }
    }
}
