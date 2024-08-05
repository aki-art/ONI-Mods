using DecorPackB.Content.Defs.Buildings;
using DecorPackB.Content.Scripts;
using HarmonyLib;

namespace DecorPackB.Patches
{
	internal class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, FountainConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Furniture.DECOR, MarbleSculptureConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, FossilDisplayConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Furniture.DECOR, FloorLampConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, PotConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Base.STORAGE, StorageLockerConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, GiantFossilDisplayConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Furniture.DECOR, FossilDisplayConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, DefaultFloorLampConfig.DEFAULT_ID, CONSTS.SUB_BUILD_CATEGORY.Base.TILES, TileConfig.ID);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, MuseumLampConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);

				// BuildingUtil.AddToPlanScreen(OilLanternConfig.ID, CONSTS.BUILD_CATEGORY.FURNITURE, CONSTS.SUB_BUILD_CATEGORY.Furniture.LIGHTS, FloorLampConfig.ID);

				BuildingUtil.AddToResearch(FountainConfig.ID, CONSTS.TECH.DECOR.FINEART);
				BuildingUtil.AddToResearch(FossilDisplayConfig.ID, CONSTS.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
				BuildingUtil.AddToResearch(GiantFossilDisplayConfig.ID, CONSTS.TECH.DECOR.ENVIRONMENTAL_APPRECIATION);
				BuildingUtil.AddToResearch(DefaultFloorLampConfig.DEFAULT_ID, CONSTS.TECH.POWER.IMPROVED_COMBUSTION);

				DefaultFloorLampConfig.decor = TUNING.DECOR.BONUS.TIER1;
				FloorLampFrames.RegisterAll();
			}
		}
	}
}
