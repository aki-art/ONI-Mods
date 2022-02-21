using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using DecorPackA.Buildings.StainedGlassTile;
using FUtility;
using HarmonyLib;

namespace DecorPackA.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                DefaultStainedGlassTileConfig.decor = new EffectorValues(Mod.Settings.GlassTile.Decor.Amount, Mod.Settings.GlassTile.Decor.Range);

                BuildingUtil.AddToPlanScreen(
                    MoodLampConfig.ID,
                    Consts.BUILD_CATEGORY.FURNITURE,
                    Consts.SUB_BUILD_CATEGORY.Furniture.ELECTRONIC_DECOR,
                    FloorLampConfig.ID);

                BuildingUtil.AddToPlanScreen(
                    GlassSculptureConfig.ID,
                    Consts.BUILD_CATEGORY.FURNITURE,
                    Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE,
                    MarbleSculptureConfig.ID);

                BuildingUtil.AddToPlanScreen(
                    DefaultStainedGlassTileConfig.DEFAULT_ID,
                    Consts.BUILD_CATEGORY.BASE,
                    Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE,
                    GlassTileConfig.ID);

                BuildingUtil.AddToResearch(MoodLampConfig.ID, Consts.TECH.DECOR.INTERIOR_DECOR);
                BuildingUtil.AddToResearch(GlassSculptureConfig.ID, Consts.TECH.DECOR.GLASS_FURNISHINGS);
                BuildingUtil.AddToResearch(DefaultStainedGlassTileConfig.DEFAULT_ID, Consts.TECH.DECOR.GLASS_FURNISHINGS);

                StainedGlassTiles.RegisterAll();
            }
        }
    }
}
