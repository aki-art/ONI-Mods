using HarmonyLib;

namespace Moonlet.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Mod.buildingsLoader.ApplyToActiveLoaders(t => t.CreateAndRegister());
				Mod.tilesLoader.ApplyToActiveLoaders(t => t.CreateAndRegister());
			}


			[HarmonyPriority(Priority.Last)]
			[HarmonyPostfix]
			public static void LatePostfix()
			{
				//ModDb.BuildingCategories.Register();
			}
		}
	}
}
