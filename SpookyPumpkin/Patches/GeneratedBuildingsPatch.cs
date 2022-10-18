using FUtility;
using HarmonyLib;
using SpookyPumpkinSO.Content.Buildings;

namespace SpookyPumpkinSO.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, SpookyPumpkinConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, FriendlyPumpkinConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);
            }
        }
    }
}
