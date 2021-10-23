using HarmonyLib;
using FUtility;
using DecorPackA.DPBuilding.GlassSculpture;
using DecorPackA.DPBuilding.MoodLamp;
using DecorPackA.DPBuilding.StainedGlassTile;

namespace DecorPackA.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Buildings.RegisterBuildings(
                    typeof(GlassSculptureConfig),
                    typeof(MoodLampConfig),
                    typeof(DefaultStainedGlassTileConfig));
            }
        }
    }
}
