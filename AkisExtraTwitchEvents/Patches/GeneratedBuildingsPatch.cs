using FUtility;
using HarmonyLib;
using Twitchery.Content.Defs.Buildings;


namespace Twitchery.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, PuzzleDoorConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Base.DOORS);
				ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.FURNITURE, LeafWallConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Furniture.DECOR);
			}
		}
	}
}
