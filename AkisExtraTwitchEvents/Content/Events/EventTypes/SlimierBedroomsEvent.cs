using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class SlimierBedroomsEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SlimierBedrooms";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		private static HashSet<string> bedroomTypes =
			[
			"Barracks",
			"Bedroom",
			"PrivateBedroom"
			];

		public override bool Condition() => Game.Instance.roomProber.rooms.Any(r => bedroomTypes.Contains(r.roomType.Id));

		public static CellElementEvent cellEvent = new("AETE_Slimierbedroom_CellEVent", "Twitch AETE Spawn", true);

		public override void Run()
		{
			HashSet<int> slimeCells = [];
			HashSet<int> goopCells = [];
			HashSet<int> morbCells = [];

			foreach (var room in Game.Instance.roomProber.rooms)
			{
				if (bedroomTypes.Contains(room.roomType.Id))
				{
					HashSet<int> visitedCells = [];
					var roomCells = GetRoomCells(room);


					HashSet<int> slimeCellsTemp = [];
					foreach (var building in room.buildings)
					{
						if (building.TryGetComponent(out Bed _))
						{
							var baseCell = building.NaturalBuildingCell();
							slimeCellsTemp.Add(baseCell);
							slimeCellsTemp.Add(Grid.CellRight(baseCell));
							slimeCellsTemp.Add(Grid.CellLeft(baseCell));
							slimeCellsTemp.Add(Grid.CellAbove(baseCell));
						}
					}

					slimeCellsTemp.RemoveWhere(c => !roomCells.Contains(c));
					slimeCells.UnionWith(slimeCellsTemp);

					foreach (var cell in roomCells)
					{
						if (slimeCells.Contains(cell))
							continue;

						if (!GridUtil.IsCellEmpty(cell))
							continue;

						if (Random.value < 0.1f)
							goopCells.Add(cell);
						else if (Random.value < 0.1f)
							morbCells.Add(cell);
					}
				}
			}

			foreach (var cell in slimeCells)
				SimMessages.ReplaceElement(cell, SimHashes.SlimeMold, cellEvent, 100);

			foreach (var cell in goopCells)
				SimMessages.ReplaceElement(cell, Elements.PinkSlime, cellEvent, 10f);

			foreach (var cell in morbCells)
				FUtility.Utils.Spawn(GlomConfig.ID, Grid.CellToPosCCC(cell, Grid.SceneLayer.Creatures));

			AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.15f, 0.7f);
			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.SLIMIERBEDROOMS.TOAST, STRINGS.AETE_EVENTS.SLIMIERBEDROOMS.DESC);
		}

		private static HashSet<int> GetRoomCells(Room room)
		{
			var result = new HashSet<int>();

			for (int x = room.cavity.minX; x <= room.cavity.maxX; x++)
			{
				for (int y = room.cavity.minY; y <= room.cavity.maxY; y++)
				{
					var cell = Grid.XYToCell(x, y);
					if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
						result.Add(cell);
				}
			}

			return result;
		}
	}
}
