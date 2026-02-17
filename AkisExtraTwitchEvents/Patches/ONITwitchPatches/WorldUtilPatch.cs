using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace Twitchery.Patches.ONITwitchPatches
{
	internal class WorldUtilPatch
	{
		public static void TryPatch(Harmony harmony)
		{
			if (!Mod.brokenPockets)
				return;

			Log.Info("Patching Pocket Dimensions to not crash on spawn.");

			var type = Type.GetType("ONITwitchLib.Utils.WorldUtil, ONITwitch");

			if (type != null)
			{
				var original = type.GetMethod("CreateWorldWithTemplate", BindingFlags.Public | BindingFlags.Static);

				if (original == null)
				{
					Log.Warning("ONITwitch.Utils.WorldUtil.CreateWorldWithTemplate doesn't exist or it's signature was changed.");
					return;
				}

				var prefix = AccessTools.DeclaredMethod(typeof(WorldUtilPatch), nameof(Prefix));

				harmony.Patch(original, prefix: new HarmonyMethod(prefix));
			}

			else
				Log.Debug("no class like this");
		}

		public static bool Prefix(GameObject worldGo, Vector2I size, string template, Action<WorldContainer> callback, ref WorldContainer __result)
		{
			if (Grid.GetFreeGridSpace(size, out var offset))
			{
				var clusterInst = Traverse.Create(ClusterManager.Instance);
				var worldId = clusterInst.Method("GetNextWorldId").GetValue<int>();
				worldGo.AddOrGet<WorldInventory>();
				var worldContainer = worldGo.AddOrGet<WorldContainer>();
				worldContainer.SetRocketInteriorWorldDetails(worldId, size, offset);
				Traverse.Create(worldContainer).Field<bool>("isModuleInterior").Value = false;

				var worldEnd = offset + size;
				for (var y = offset.y; y < worldEnd.y; ++y)
				{
					for (var x = offset.x; x < worldEnd.x; ++x)
					{
						var cell = Grid.XYToCell(x, y);
						Grid.WorldIdx[cell] = (byte)worldId;
						Pathfinding.Instance.AddDirtyNavGridCell(cell);
					}
				}

				var pos = new Vector2(size.x / 2 + offset.x, size.y / 2 + offset.y);

				TemplateLoader.Stamp(
					TemplateCache.GetTemplate(template),
					pos,
					() => { callback?.Invoke(worldContainer); }
				);

				ClusterManager.Instance.BoxingTrigger((int)GameHashes.WorldAdded, worldContainer.id);

				__result = worldContainer;
				return false;
			}

			Log.Warning($"Unable to create a world of size {size}");

			__result = null;
			return false;

		}

	}
}
