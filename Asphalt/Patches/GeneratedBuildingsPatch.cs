using Asphalt.Buildings;
using FUtility;
using HarmonyLib;

namespace Asphalt.Patches
{
	class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, AsphaltTileConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Base.TILES, MetalTileConfig.ID);
				BuildingUtil.AddToResearch(AsphaltTileConfig.ID, CONSTS.TECH.POWER.IMPROVED_COMBUSTION);
			}
		}
	}
}
