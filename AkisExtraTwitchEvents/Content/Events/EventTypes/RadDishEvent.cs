using FUtility;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class RadDishEvent : ITwitchEvent
	{
		public const string ID = "RadDish";
		private OccupyArea prefabOccupyArea;
		private static readonly CellOffset[] smallerArea = EntityTemplates.GenerateOffsets(3, 2);

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public bool Condition(object data)
		{
			if (AkisTwitchEvents.Instance.lastRadishSpawn + 100f > GameClock.Instance.GetTimeInCycles())
				return false;

			var minKcal = GetMinKcal();

			foreach (var world in ClusterManager.Instance.WorldContainers)
			{
				if (IsWorldEligible(world, minKcal))
					return true;
			}

			return ClusterManager.Instance.WorldContainers.Any(world => IsWorldEligible(world, minKcal));
		}

		private static int GetMinKcal()
		{
			return !AkisTwitchEvents.Instance.hasRaddishSpawnedBefore ? 300_000_000 : 100_000_000;
		}

		private bool IsWorldEligible(WorldContainer world, float minKcal)
		{
			//Log.Debuglog("world worldname: " + world.worldName);
			//Log.Debuglog("world name: " + world.name);

			if (world.WorldSize.x <= 32 || world.worldSize.y <= 32)
				return false;

			if (!IsInhabited(world))
			{
				return false;
			}

			var rationPerWorld = RationTracker.Get().CountRations(null, world.worldInventory);
			//Log.Debuglog($"checking {world.GetProperName()} {rationPerWorld} {minKcal}");

			return rationPerWorld <= minKcal;
		}

		private bool IsInhabited(WorldContainer world)
		{
			if (!world.isDiscovered)
			{
				Log.Debuglog("not discovered");
				return false;
			}

			var minions = Components.MinionIdentities.GetWorldItems(world.id);
			return minions != null && minions.Count != 0;
		}

		public string GetID() => ID;

		private float GetCaloriesPerDupe(WorldContainer world)
		{
			var totalRations = RationTracker.Get().CountRations(null, world.worldInventory);
			var minions = Components.LiveMinionIdentities.GetWorldItems(world.id).Count;

			return totalRations / minions;
		}

		public void Run(object data)
		{
			AkisTwitchEvents.Instance.lastRadishSpawn = GameClock.Instance.GetTimeInCycles();

			prefabOccupyArea = Assets.GetPrefab(GiantRadishConfig.ID).GetComponent<OccupyArea>();

			var rationTracker = RationTracker.Get();

			var worlds = new List<WorldContainer>(ClusterManager.Instance.WorldContainers)
				.Where(IsInhabited)
				.OrderBy(GetCaloriesPerDupe);

			var targetWorld = worlds.First();

			if (targetWorld == null)
			{
				Log.Warning("something went wrong trying to find the hungriest asteroid");
				return;
			}

			var cavities = new List<CavityInfo>();
			foreach (CavityInfo cavity in Game.Instance.roomProber.cavityInfos)
			{
				var middle = Grid.PosToCell(cavity.GetCenter());

				if (!Grid.IsVisible(middle) || !Grid.IsValidCellInWorld(middle, targetWorld.id))
				{
					continue;
				}

				cavities.Add(cavity);
			}

			if (cavities.Count == 0)
			{
				Log.Warning("No cavities in this world apparently. " + targetWorld.GetProperName());
				return;
			}

			cavities.Shuffle();

			var potentialButLameCavities = new List<CavityInfo>();

			foreach (CavityInfo cavity in cavities)
			{
				if (cavity.maxX - cavity.minX < 3)
				{
					continue;
				}

				var height = cavity.maxY - cavity.minY;

				if (height < 4)
				{
					continue;
				}

				if (height < 6)
				{
					// save for later in case we dont find a really good spot
					potentialButLameCavities.Add(cavity);
					continue;
				}

				var cell = GetValidPlacementInCavity(cavity);

				if (cell == -1)
				{
					continue;
				}

				SpawnRadish(cell, targetWorld);

				return;
			}

			if (potentialButLameCavities.Count > 0)
			{
				foreach (var cavity in potentialButLameCavities)
				{
					var lameCell = GetSortofValidPlacementInCavity(cavity);
					if (lameCell != -1)
					{
						SpawnRadish(lameCell, targetWorld);
						return;
					}
				}
			}

			ONITwitchLib.ToastManager.InstantiateToast("Rad dish...?", "But something went wrong, there was nowhere to spawn it. :(");
		}


		private static void SpawnRadish(int cell, WorldContainer world)
		{
			var radish = FUtility.Utils.Spawn(GiantRadishConfig.ID, Grid.CellToPos(cell));

			ONITwitchLib.ToastManager.InstantiateToastWithGoTarget(
				STRINGS.AETE_EVENTS.RAD_DISH.TOAST,
				STRINGS.AETE_EVENTS.RAD_DISH.DESC.Replace("{Asteroid}", world?.GetProperName()),
				radish);

			AkisTwitchEvents.Instance.hasRaddishSpawnedBefore = true;
		}

		private int GetValidPlacementInCavity(CavityInfo cavity)
		{
			var minX = cavity.minX + 1; // no need to check up against wall
			var maxX = cavity.maxX - 1;
			var minY = cavity.minY;
			var maxY = cavity.maxY - 5;

			for (var x = minX; x <= maxX; x++)
			{
				for (var y = maxY; y >= minY; y--)
				{
					var cell = Grid.XYToCell(x, y);
					if (prefabOccupyArea.TestArea(cell, null, (cell, _) => Grid.IsValidCell(cell)
						&& !Grid.Solid[cell])
						&& Grid.IsValidCell(Grid.CellBelow(cell))
						&& prefabOccupyArea.CanOccupyArea(cell, ObjectLayer.Building))
					{
						return cell;
					}
				}

#if DEBUG

				Log.Debug("debug");
#endif
			}

			return -1;
		}

		private int GetSortofValidPlacementInCavity(CavityInfo cavity)
		{
			var minX = cavity.minX + 1; // no need to check up against wall
			var maxX = cavity.maxX - 1;
			var minY = cavity.minY;
			var maxY = cavity.maxY - 2;

			for (var x = minX; x <= maxX; x++)
			{
				for (var y = maxY; y >= minY; y--)
				{
					var cell = Grid.XYToCell(x, y);

					bool invalid = false;

					foreach (CellOffset offset in smallerArea)
					{
						var offsetCell = Grid.OffsetCell(cell, offset);
						if (!Grid.IsValidCell(offsetCell)
							|| Grid.Solid[offsetCell]
							|| Grid.Objects[offsetCell, (int)ObjectLayer.Building] != null)
						{
							invalid = true;
							break;
						}
					}

					if (!invalid)
						return cell;
				}
			}

			return -1;
		}
	}
}
