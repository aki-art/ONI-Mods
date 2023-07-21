using HarmonyLib;

namespace Moonlet.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				foreach (var mod in Mod.modLoaders)
					mod.entitiesLoader.LoadBuildings();
			}

			[HarmonyPriority(Priority.Last)]
			[HarmonyPostfix]
			public static void LatePostfix()
			{
				ModDb.BuildingCategories.Register();
			}
		}
	}
}
