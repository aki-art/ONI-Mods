using UnityEngine;

namespace Twitchery.Utils
{
	public class ExplosionUtil
	{
		public static void Explode(int cell, float power, Vector2 explosionSpeedRange, System.Collections.Generic.List<CellOffset> targetTiles)
		{
			SendDebrisFlying(cell, explosionSpeedRange);
			DamageTiles(cell, targetTiles, power);
		}

		private static void DamageTiles(int impactCell, System.Collections.Generic.List<CellOffset> targetTiles, float power)
		{
			foreach (var offset in targetTiles)
			{
				var cell = Grid.OffsetCell(impactCell, offset);

				if (Grid.IsValidCell(cell))
					WorldDamage.Instance.ApplyDamage(cell, power, -1);
			}

			WorldDamage.Instance.ApplyDamage(impactCell, 1f, -1);
		}

		private static void SendDebrisFlying(int cell, Vector2 explosionSpeedRange)
		{
			var pos = Grid.CellToPos(cell);
			var nearbyPickupables = ListPool<ScenePartitionerEntry, Comet>.Allocate();
			GameScenePartitioner.Instance.GatherEntries((int)pos.x - 2, (int)pos.y - 2, 4, 4, GameScenePartitioner.Instance.pickupablesLayer, nearbyPickupables);

			foreach (var partitionerEntry in nearbyPickupables)
			{
				var gameObject = (partitionerEntry.obj as Pickupable).gameObject;

				var notSentient = gameObject.GetComponent<Navigator>() == null;

				if (notSentient)
				{
					var vec = (Vector2)(gameObject.transform.GetPosition() - pos);
					vec = vec.normalized;
					var velocity = (vec + new Vector2(0.0f, 0.55f)) * (0.5f * Random.Range(explosionSpeedRange.x, explosionSpeedRange.y));

					if (GameComps.Fallers.Has(gameObject))
						GameComps.Fallers.Remove(gameObject);

					if (GameComps.Gravities.Has(gameObject))
						GameComps.Gravities.Remove(gameObject);

					GameComps.Fallers.Add(gameObject, velocity);
				}
			}

			nearbyPickupables.Recycle();
		}

	}
}
