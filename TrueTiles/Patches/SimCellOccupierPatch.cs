using HarmonyLib;

namespace TrueTiles.Patches
{
	public class SimCellOccupierPatch
	{
		[HarmonyPatch(typeof(SimCellOccupier), "OnSpawn")]
		public class SimCellOccupier_OnSpawn_Patch
		{
			public static void Postfix(SimCellOccupier __instance)
			{
				if (!__instance.HasTag(ModAssets.Tags.texturedTile))
					return;

				var cell = Grid.PosToCell(__instance);

				if (__instance.GetComponent<PrimaryElement>() is PrimaryElement primaryElement)
					ElementGrid.Add(cell, primaryElement.ElementID);

				// tiles like airflow tiles need a frame delay to update
				if (!__instance.doReplaceElement)
					GameScheduler.Instance.ScheduleNextFrame("refresh cell", obj => RefreshCell(cell));
			}

			private static void RefreshCell(int cell)
			{
				TileVisualizer.RefreshCell(cell, ObjectLayer.FoundationTile, ObjectLayer.ReplacementTile);
			}
		}

		[HarmonyPatch(typeof(SimCellOccupier), "OnModifyComplete")]
		public class SimCellOccupier_OnModifyComplete_Patch
		{
			public static void Postfix(SimCellOccupier __instance)
			{
				if (!__instance.doReplaceElement)
					return;

				if (!__instance.HasTag(ModAssets.Tags.texturedTile))
					return;

				var cell = Grid.PosToCell(__instance);

				TileVisualizer.RefreshCell(cell, ObjectLayer.FoundationTile, ObjectLayer.ReplacementTile);
			}
		}

		[HarmonyPatch(typeof(SimCellOccupier), "OnCleanUp")]
		public class SimCellOccupier_OnCleanup_Patch
		{
			public static void Prefix(SimCellOccupier __instance)
			{
				if (!__instance.HasTag(ModAssets.Tags.texturedTile))
					return;

				ElementGrid.Remove(Grid.PosToCell(__instance));
			}
		}
	}
}
