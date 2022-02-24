using Beached.Buildings.SeashellChime;
using FUtility;
using HarmonyLib;

namespace Beached.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtil.AddToPlanScreen(
                    SeashellChimeConfig.ID,
                    Consts.BUILD_CATEGORY.FURNITURE);
            }
        }
    }
}
