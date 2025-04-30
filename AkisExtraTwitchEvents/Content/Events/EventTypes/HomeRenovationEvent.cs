using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class HomeRenovationEvent() : TwitchEventBase(ID)
	{
		public const string ID = "HomeRenovation";

		public override Danger GetDanger() => Danger.None;

		public override bool Condition()
		{
			return MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.CanDemolish.Id);
		}

		public override int GetWeight() => Consts.EventWeight.Common;

		private static readonly List<int> rollTable = [0, 0, 1, 1, 1, 2];

		private static readonly List<Tag> floorProps = [
			"PropGravitasDeskPodium",
			"PropGravitasDisplay4",
			"PropGravitasFloorRobot",
			"PropGravitasHandScanner",
			"PropGravitasLabTable",
			"PropGravitasRoboticTable",
			"PropGravitasToolCrate",
			"PropTallPlant",
			"PropTable",
			"PropFacilityChair",
			"PropDesk",
			"PropElevator",
			"PropExoShelfLong",
			"PropExoShelfShort",
			"PropFacilityCouch",
			"PropFacilityDisplay",
			"PropFacilityDisplay2",
			"PropFacilityDisplay3",
			"PropFacilityGlobeDroors",
			"PropFacilityStatue",
			"PropHumanChesterfieldChair",
			"PropHumanChesterfieldSofa",
			"PropHumanMurphyBed",
			"PropReceptionDesk",
			"PropSkeleton",
			"PropGravitasJar1",
			"PropGravitasJar2"
			];

		private static readonly List<Tag> wallProps = [
			"PropGravitasToolShelf",
			"PropGravitasSmallSeedLocker",
			"PropGravitasShelf",
			"PropGravitasLabWindowHorizontal",
			"PropGravitasLabWindow",
			"PropGravitasFireExtinguisher",
			"PropGravitasDisplay4",
			"PropGravitasDecorativeWindow",
			"PropGravitasCreaturePoster",
			"PropFacilityPainting",
			"PropClock",
			"PropCeresPosterLarge",
			"PropCeresPosterB",
			"PropCeresPosterA",
			];

		private static readonly List<Tag> ceilingProps = [
			// //"GravitasLabLight",
			"PropLight",
			"PropGravitasCeilingRobot",
			"PropFacilityChandelier"
		];

		private static List<Tag> allProps;

		public override void Run()
		{
			var startPosition = PosUtil.ClampedMouseWorldPos();

			allProps ??=
				floorProps
					.Union(ceilingProps)
					.Union(wallProps)
					.Where(t => Assets.GetPrefab(t) != null)
					.ToList();

			var radius = 25;
			var attempts = 256;
			var furnitureToSpawn = Random.Range(10, 16);
			var furnitureSpawned = 0;

			while (attempts-- > 0 && furnitureSpawned < furnitureToSpawn)
			{
				var cell = Grid.PosToCell(startPosition + (Vector3)(Random.insideUnitCircle * radius));
				var worldIdx = Grid.WorldIdx[cell];
				var type = rollTable.GetRandom();
				List<Tag> tags = null;

				switch (type)
				{
					case 0:
						cell = FindFloorCell(cell);
						tags = floorProps;
						break;
					case 1:
						tags = wallProps;
						break;
					case 2:
						cell = FindCeilingCell(cell);
						tags = ceilingProps;
						break;
				}

				if (cell == -1 || !Grid.IsValidCellInWorld(cell, worldIdx))
					continue;

				tags.Shuffle();
				foreach (var tag in tags)
				{
					if (CanFurnitureSpawnHere(cell, tag))
					{
						FUtility.Utils.Spawn(tag, Grid.CellToPos(cell));
						furnitureSpawned++;
						break;
					}
				}
			}

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.HOMERENOVATION.TOAST, STRINGS.AETE_EVENTS.HOMERENOVATION.DESC);
		}

		private int FindCeilingCell(int cell)
		{
			while (GridUtil.IsCellFoundationEmpty(Grid.CellAbove(cell)))
				cell = Grid.CellAbove(cell);

			return cell;
		}

		private int FindFloorCell(int cell)
		{
			while (GridUtil.IsCellFoundationEmpty(Grid.CellBelow(cell)))
				cell = Grid.CellBelow(cell);

			return cell;
		}

		private bool CanFurnitureSpawnHere(int cell, Tag tag)
		{
			var prefab = Assets.GetPrefab(tag);
			if (prefab == null)
				return false;

			if (prefab.TryGetComponent(out OccupyArea area))
			{
				foreach (CellOffset offset in area.OccupiedCellsOffsets)
				{
					var offsetCell = Grid.OffsetCell(cell, offset);
					if (!GridUtil.IsCellEmpty(offsetCell))
						return false;

					if (Grid.ObjectLayers[(int)ObjectLayer.Building].ContainsKey(offsetCell))
						return false;

					if (floorProps.Contains(tag))
					{
						if (!area.TestAreaBelow(cell, null, IsCellSolid))
							return false;
					}

					if (ceilingProps.Contains(tag))
					{
						if (!area.TestAreaAbove(cell, null, IsCellSolid))
							return false;
					}
				}
			}

			return true;
		}

		private bool IsCellSolid(int cell, object _) => !GridUtil.IsCellFoundationEmpty(cell);
	}
}
