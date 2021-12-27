using HarmonyLib;

namespace GlassCase.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.Buildings.RegisterBuildings(
                    typeof(Buildings.GlassCaseConfig),
                    typeof(Buildings.GlassCaseValveConfig));
            }
        }
    }
}
