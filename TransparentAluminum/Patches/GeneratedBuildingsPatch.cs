using HarmonyLib;
using TransparentAluminum.Buildings.Borer;

namespace TransparentAluminum.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.BuildingUtil.Buildings.RegisterBuildings(
                    typeof(BorerConfig));
            }
        }
    }
}
