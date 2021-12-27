using DecorPackB.Buildings.FossilDisplay;
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
                FUtility.Buildings.RegisterBuildings(
                    typeof(FossilDisplayConfig) );
            }
        }
    }
}
