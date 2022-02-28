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
                    FloorLampConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Furniture.ELECTRONIC_DECOR);


                BuildingUtil.AddToPlanScreen(
                    GlassSculptureConfig.ID,
                    Consts.BUILD_CATEGORY.FURNITURE,
                    MarbleSculptureConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE);

                BuildingUtil.AddToPlanScreen(
                    DefaultStainedGlassTileConfig.DEFAULT_ID,
                    Consts.BUILD_CATEGORY.BASE,
                    GlassTileConfig.ID,
                    Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE);


                BuildingUtil.AddToResearch(MoodLampConfig.ID, Consts.TECH.DECOR.INTERIOR_DECOR);
                BuildingUtil.AddToResearch(GlassSculptureConfig.ID, Consts.TECH.DECOR.GLASS_FURNISHINGS);
                BuildingUtil.AddToResearch(DefaultStainedGlassTileConfig.DEFAULT_ID, Consts.TECH.DECOR.GLASS_FURNISHINGS);

                StainedGlassTiles.RegisterAll();
            }
        }
    }
}
