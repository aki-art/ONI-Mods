using HarmonyLib;
using DuctTapePipes.Buildings;

namespace DuctTapePipes.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.Buildings.RegisterBuildings(
                    typeof(LiquidOutputLeechConfig),
                    typeof(LiquidInputLeechConfig));
            }
        }
    }
}
