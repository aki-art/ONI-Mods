using FUtility;
using HarmonyLib;
using RadShieldTile.Content;

namespace RadShieldTile.Patches
{
	class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, RadShieldTileConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Base.TILES, MetalTileConfig.ID);

				BuildingUtil.AddToResearch(RadShieldTileConfig.ID, CONSTS.TECH.COLONY_DEVELOPMENT.ADVANCED_NUCLEAR_RESEARCH);
			}
		}
	}
}
