using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using DecorPackA.Buildings.StainedGlassTile;
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

				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, MoodLampConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, GlassSculptureConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Furniture.DECOR, MarbleSculptureConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, DefaultStainedGlassTileConfig.DEFAULT_ID, CONSTS.SUB_BUILD_CATEGORY.Base.TILES, GlassTileConfig.ID);

				BuildingUtil.AddToResearch(MoodLampConfig.ID, CONSTS.TECH.DECOR.INTERIOR_DECOR);
				BuildingUtil.AddToResearch(GlassSculptureConfig.ID, CONSTS.TECH.DECOR.GLASS_FURNISHINGS);
				BuildingUtil.AddToResearch(DefaultStainedGlassTileConfig.DEFAULT_ID, CONSTS.TECH.DECOR.GLASS_FURNISHINGS);

				StainedGlassTiles.RegisterAll();
			}
		}
	}
}
