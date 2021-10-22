using FUtility;
using HarmonyLib;
using TwelveCyclesOfChristmas.Building;

namespace TwelveCyclesOfChristmas.Harmony
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Buildings.RegisterBuildings(typeof(FancyFarmPlotConfig));
            }
        }
    }
}
