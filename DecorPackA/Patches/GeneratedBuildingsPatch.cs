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

                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, MoodLampConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.FURNITURE, GlassSculptureConfig.ID, Consts.SUB_BUILD_CATEGORY.Furniture.SCULPTURE, MarbleSculptureConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, DefaultStainedGlassTileConfig.DEFAULT_ID, Consts.SUB_BUILD_CATEGORY.Base.TILES, GlassTileConfig.ID);

                BuildingUtil.AddToResearch(MoodLampConfig.ID, Consts.TECH.DECOR.INTERIOR_DECOR);
                BuildingUtil.AddToResearch(GlassSculptureConfig.ID, Consts.TECH.DECOR.GLASS_FURNISHINGS);
                BuildingUtil.AddToResearch(DefaultStainedGlassTileConfig.DEFAULT_ID, Consts.TECH.DECOR.GLASS_FURNISHINGS);

                StainedGlassTiles.RegisterAll();

                Log.Debuglog("LOADTEST LoadGeneratedBuildings");
            }
        }
    }
}
