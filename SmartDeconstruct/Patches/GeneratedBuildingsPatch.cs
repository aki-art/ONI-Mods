using HarmonyLib;
using System.Collections.Generic;

namespace SmartDeconstruct.Patches
{
	public class GeneratedBuildingsPatch
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			private static HashSet<string> deconstructableIds = [];

			public static void Postfix()
			{
				foreach (var def in Assets.BuildingDefs)
				{
					if (deconstructableIds.Contains(def.PrefabID)
					|| def.BuildingComplete.TryGetComponent(out Ladder _))
					{
						def.BuildingComplete.gameObject.AddOrGet<SD_Deconstructable>();
					}
				}
			}
		}
	}
}