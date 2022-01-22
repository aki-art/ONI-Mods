using DecorPackB.Buildings.FossilDisplay;
using DecorPackB.Buildings.Fountain;
using DecorPackB.Buildings.OilLantern;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.BuildingUtil.Buildings.RegisterBuildings(
                    typeof(FossilDisplayConfig),
                    typeof(GiantFossilDisplayConfig),
                    typeof(OilLanternConfig),
                    typeof(FountainConfig));
            }
        }
    }
}
