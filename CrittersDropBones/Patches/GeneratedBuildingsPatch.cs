using CrittersDropBones.Buildings.SlowCooker;
using FUtility;
using HarmonyLib;

namespace CrittersDropBones.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.Buildings.RegisterBuildings(
                    typeof(SlowCookerConfig));
            }
        }
    }
}
