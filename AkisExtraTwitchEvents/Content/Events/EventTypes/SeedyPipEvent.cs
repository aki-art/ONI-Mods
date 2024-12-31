/*using FUtility;
using ONITwitchLib;
using System.Linq;

namespace Twitchery.Content.Events.EventTypes
{
	public class SeedyPipEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SeedyPip";
		public const int TRIES = 3;
		public PlantableSeed selectedSeed;

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override bool Condition() => Game.Instance.roomProber.cavityInfos.Any(IsCavityProbablyEligible);

		public override void Run()
		{
			selectedSeed = Assets.GetPrefab(CylindricaConfig.SEED_ID).GetComponent<PlantableSeed>();

			var attempts = 0;
			var success = false;

			while (attempts++ < TRIES)
			{
				if (TrySpawnPip())
				{
					success = true;
					break;
				}
			}

			if (!success)
				Log.Warning($"SeedyPipEvent.Run Could not spawn pip");
		}

		private bool TrySpawnPip()
		{
			var cavities = Game.Instance.roomProber.cavityInfos;

			foreach (var cavity in cavities)
			{
				if (!IsRoomNotARanch(cavity))
					continue;

				var cell = GetNaturalTilePosition(cavity?.room);

				if (cell <= -1)
					continue;

				Log.Debug(-1);
				var pos = Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures);

				var pipGo = FUtility.Utils.Spawn(SquirrelConfig.ID, pos);

				if (selectedSeed == null)
					selectedSeed = Assets.GetPrefab(CylindricaConfig.SEED_ID).GetComponent<PlantableSeed>();

				var seedGo = FUtility.Utils.Spawn(CylindricaConfig.SEED_ID, pos);

				GameScheduler.Instance.ScheduleNextFrame("", _ =>
				{
					Log.Debug(0);
					pipGo.GetSMI<SeedPlantingMonitor.Instance>().nextSearchTime = -999;

					Log.Debug(1);

					Log.Assert("pipGo", pipGo);

					var seedPlanting = pipGo.GetComponent<ChoreConsumer>().choreTable.GetEntry< // pipGo.GetSMI<SeedPlantingStates.Instance>();
					Log.Assert("seedPlanting", seedPlanting);
					seedPlanting.targetSeed = seedGo.GetComponent<Pickupable>();

					Log.Debug(2);
					SeedPlantingStates.PickupComplete(seedPlanting);
					Log.Debug(3);
					seedPlanting.GoTo(seedPlanting.sm.moveToPlantLocation);
					Log.Debug(4);
				});


				return true;
			}

			return false;
		}

		private Tag GetSeedPrefabId() => CylindricaConfig.ID;

		private bool IsCavityProbablyEligible(CavityInfo cavity)
		{
			return IsRoomNotARanch(cavity) && GetNaturalTilePosition(cavity.room) > -1;
		}

		private static bool IsRoomNotARanch(CavityInfo cavity)
		{
			return cavity != null
				&& cavity.room != null
				&& (cavity.room.roomType == null || cavity.room.roomType.IdHash != Db.Get().RoomTypes.CreaturePen.IdHash);
		}

		private int GetNaturalTilePosition(Room room)
		{
			if (selectedSeed == null || room == null)
				return -1;

			for (int x = room.cavity.minX; x <= room.cavity.maxX; x++)
			{
				for (int y = room.cavity.minY; y <= room.cavity.maxY; y++)
				{
					var cell = Grid.XYToCell(x, y);
					if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
					{
						if (selectedSeed.TestSuitableGround(cell))
							return cell;
					}
				}
			}

			return -1;
		}
	}
}
*/