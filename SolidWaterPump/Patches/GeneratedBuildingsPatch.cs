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
                ModUtil.AddBuildingToPlanScreen(
                    CONSTS.BUILD_CATEGORY.PLUMBING,
                    Buildings.SolidWaterPumpConfig.ID,
                    CONSTS.SUB_BUILD_CATEGORY.Plumbing.PUMPS,
                    LiquidPumpingStationConfig.ID
                    );
            }
        }
    }
}
