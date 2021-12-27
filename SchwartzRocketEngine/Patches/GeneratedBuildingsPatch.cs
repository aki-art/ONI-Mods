using HarmonyLib;

namespace SchwartzRocketEngine.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.Buildings.RegisterBuildings(
                    typeof(Buildings.SandComberConfig));
            }
        }
    }
}
