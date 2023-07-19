using HarmonyLib;

namespace Moonlet.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			[HarmonyPriority(Priority.Last)]
			[HarmonyPostfix]
			public static void LatePostfix()
			{
				ModDb.BuildingCategories.Register();
			}
		}
	}
}
