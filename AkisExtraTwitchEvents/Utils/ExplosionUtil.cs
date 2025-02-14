using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Utils
{
	public class ExplosionUtil
	{
		public static void Explode(int cell, float power, Vector2 explosionSpeedRange, List<CellOffset> targetTiles)
		{
			SendDebrisFlying(cell, explosionSpeedRange);
			DamageTiles(cell, targetTiles, power);
		}

		static float Remap(float value, float inMin, float inMax, float outMin, float outMax)
		{
			return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
		}

		public static List<int> ExplodeInRadius(int cell, float power, Vector2 explosionSpeedRange, int radius)
		{
			var cells = ProcGen.Util.GetFilledCircle(Grid.CellToPosCCC(cell, Grid.SceneLayer.Building), radius);
			var centerPos = Grid.CellToPos(cell);

			var affectedCells = new List<int>();

			foreach (var pos in cells)
			{
				var targetCell = Grid.PosToCell(pos);

				if (Grid.IsValidCell(targetCell))
				{
					var dist = Vector2.Distance(centerPos, pos);
					var t = 1f - (dist / radius);

					float localPower = power * Remap(t, 0, 0.7f, 0f, 1f);
					localPower = Mathf.Clamp01(localPower);

					WorldDamage.Instance.ApplyDamage(targetCell, localPower, -1);

					if (localPower >= 1)
						affectedCells.Add(targetCell);
				}
			}

			WorldDamage.Instance.ApplyDamage(cell, 1f, -1);

			SendDebrisFlying(cell, explosionSpeedRange);

			return affectedCells;
		}

		private static void DamageTiles(int impactCell, List<Vector2> targetTiles, float power)
		{
			foreach (var offset in targetTiles)
			{
				var cell = Grid.PosToCell(offset);

				if (Grid.IsValidCell(cell))
					WorldDamage.Instance.ApplyDamage(cell, power, -1);
			}

			WorldDamage.Instance.ApplyDamage(impactCell, 1f, -1);
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
