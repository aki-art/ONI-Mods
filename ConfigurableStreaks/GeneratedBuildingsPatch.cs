using HarmonyLib;

namespace ConfigurableStreaks
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			[HarmonyPostfix]
			[HarmonyPriority(Priority.Last)]
			public static void LatePostfix()
			{
				foreach (var building in Assets.BuildingDefs)
				{
					if (building.BuildingComplete.TryGetComponent(out Light2D _))
					{
						building.BuildingComplete.AddOrGet<StreakToggler>();
					}
				}
			}
		}
	}
}
