using UnityEngine;

namespace Twitchery.Utils
{
	internal class TileUtil
	{
		public delegate bool TileConditionDelegate(GameObject gameObject, int cell);

		public static void ClearTile(int cell, TileConditionDelegate condition = null)
		{
			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (condition == null || condition(go, cell))
						deconstructable.ForceDestroyAndGetMaterials();
				}
			}
		}

		public static bool ReplaceTile(int cell, BuildingDef def, float? temperature = null, TileConditionDelegate condition = null)
		{
			if (Grid.ObjectLayers[(int)ObjectLayer.FoundationTile].TryGetValue(cell, out var go))
			{
				if (go.TryGetComponent(out Deconstructable deconstructable))
				{
					if (condition == null || condition(go, cell))
					{
						var temp = temperature.GetValueOrDefault(go.GetComponent<PrimaryElement>().Temperature);
						GameScheduler.Instance.ScheduleNextFrame("spawn tile", _ => SpawnTile(cell, def.PrefabID, def.DefaultElements().ToArray(), temp));

						deconstructable.ForceDestroyAndGetMaterials();
					}
				}
			}

			return false;
		}

		public static void SpawnTile(int cell, string prefabId, Tag[] elements, float temperature)
		{
			var def = Assets.GetBuildingDef(prefabId);

			if (def == null)
				return;

			def.Build(
				cell,
				Orientation.Neutral,
				null,
				elements,
				temperature,
				false,
				GameClock.Instance.GetTime() + 1);

			World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
		}
	}
}
