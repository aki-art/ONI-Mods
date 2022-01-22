using CrittersDropBones.Buildings.SlowCooker;
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
                FUtility.BuildingUtil.Buildings.RegisterBuildings(
                    typeof(SlowCookerConfig));
            }
        }
    }
}
