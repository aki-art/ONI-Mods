using FUtility;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace Twitchery.Patches
{
	public class PocketDimensionPatch
	{
		public static void TryPatch(Harmony harmony)
		{
			var type = Type.GetType("ONITwitch.Cmps.PocketDimension.PocketDimension, ONITwitch");

			if (type != null)
			{
				var original = type.GetMethod("Sim200ms", BindingFlags.Public | BindingFlags.Instance);

				if (original == null)
				{
					Log.Warning("ONITwitch.Cmps.PocketDimension.PocketDimension.DestroyWorld doesn't exist or it's signature was changed.");
					return;
				}

				var prefix = typeof(PocketDimensionPatch).GetMethod(nameof(Prefix), new[] { typeof(WorldContainer), typeof(float) });
				//harmony.Patch(original, prefix: new HarmonyMethod(prefix));
			}
		}

		public static void Prefix(WorldContainer ___world, float ___Lifetime, bool ___enabled)
		{
			if (___enabled && ___Lifetime - 0.25f < 0)
				RescueEntities(___world);
		}

		private static void RescueEntities(WorldContainer ___world)
		{
			if (ClusterManager.Instance == null) return;

			var startWorld = ClusterManager.Instance.GetStartWorld();
			var pad = GameUtil.GetTelepad(startWorld.id) ?? GameUtil.GetActiveTelepad();

			var exitPos = pad != null
				? Grid.CellToPos(Grid.CellAbove(Grid.PosToCell(pad)))
				: (Vector3)(startWorld.minimumBounds + startWorld.maximumBounds) / 2;

			exitPos = exitPos with { z = Grid.GetLayerZ(Grid.SceneLayer.Move) };

			foreach (var poly in Mod.polys.GetWorldItems(___world.id))
			{
				MoveEntity(poly.gameObject, exitPos);
			}

			foreach (var entity in Mod.midasContainers.GetWorldItems(___world.id))
			{
				MoveEntity(entity.gameObject, exitPos);
			}
		}

		private static void MoveEntity(GameObject entity, Vector3 pos)
		{
			entity.transform.SetPosition(pos);

			if (entity.TryGetComponent(out Storage storage))
			{
				foreach (var item in storage.items)
					item.transform.SetPosition(pos);
			}
		}
	}
}
