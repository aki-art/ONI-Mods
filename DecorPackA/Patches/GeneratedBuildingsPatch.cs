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
                DefaultStainedGlassTileConfig.decor = new EffectorValues(Mod.Settings.GlassTile.Decor.Amount, Mod.Settings.GlassTile.Decor.Range);

                FUtility.Buildings.RegisterBuildings(
                    typeof(GlassSculptureConfig),
                    typeof(MoodLampConfig),
                    typeof(DefaultStainedGlassTileConfig));

                StainedGlassTiles.RegisterAll();
            }
        }
    }
}
