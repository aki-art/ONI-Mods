using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;

namespace DecorPackA.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.Buildings.RegisterBuildings(
                    typeof(GlassSculptureConfig),
                    typeof(MoodLampConfig),
                    typeof(DefaultStainedGlassTileConfig));

                StainedGlassTiles.RegisterAll();
            }
        }
    }
}
