using FUtility;
using HarmonyLib;

namespace SpookyPumpkin
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtil.AddToPlanScreen(
                    Buildings.SpookyPumpkinConfig.ID,
                    Consts.BUILD_CATEGORY.FURNITURE,
                    Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS,
                    FloorLampConfig.ID);
            }
        }
    }
}
