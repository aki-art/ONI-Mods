using HarmonyLib;

namespace Twitchery.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{

#if DEV
				ModUtil.AddBuildingToPlanScreen(
					CONSTS.BUILD_CATEGORY.BASE,
					PuzzleDoorConfig.ID,
					CONSTS.SUB_BUILD_CATEGORY.Base.DOORS);
#endif
			}
		}
	}
}
