using ONITwitchLib;
using System.Collections.Generic;
using System.Linq;

namespace Twitchery.Content.Events.EventTypes
{
	public class AltSnowyBedroomsEvent() : TwitchEventBase(ID)
	{
		public const string ID = "AltSnowyBedrooms";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Common;

		private static readonly HashSet<string> bedroomTypes =
			[
			"Barracks",
			"Bedroom",
			"PrivateBedroom"
			];

		public override bool Condition() => Game.Instance.roomProber.rooms.Any(r => bedroomTypes.Contains(r.roomType.Id));

		public static CellElementEvent cellEvent = new("AETE_Snowy2bedroom_CellEVent", "Twitch AETE Spawn", true);

		public override void Run()
		{
			HashSet<int> cells = [];

			foreach (var room in Game.Instance.roomProber.rooms)
			{
				if (bedroomTypes.Contains(room.roomType.Id))
				{
					HashSet<int> visitedCells = [];
					var roomCells = GetRoomCells(room);


					HashSet<int> cellsTemp = [];
					foreach (var building in room.buildings)
					{
						if (building.TryGetComponent(out Bed _))
						{
							var baseCell = building.NaturalBuildingCell();
							cellsTemp.Add(baseCell);
							cellsTemp.Add(Grid.CellRight(baseCell));
							cellsTemp.Add(Grid.CellLeft(baseCell));
							cellsTemp.Add(Grid.CellAbove(baseCell));
						}
					}

					cellsTemp.RemoveWhere(c => !roomCells.Contains(c));
					cells.UnionWith(cellsTemp);
				}
			}

			foreach (var cell in cells)
				SimMessages.ReplaceElement(cell, Elements.YellowSnow, cellEvent, 100);

			AudioUtil.PlaySound(ModAssets.Sounds.SNOW_IMPACT, ModAssets.GetSFXVolume() * 0.95f, 1f);
			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.ALTSNOWYBEDROOMS.TOAST, STRINGS.AETE_EVENTS.ALTSNOWYBEDROOMS.DESC);
		}

		private static HashSet<int> GetRoomCells(Room room)
		{
			var result = new HashSet<int>();

			for (var x = room.cavity.minX; x <= room.cavity.maxX; x++)
			{
				for (var y = room.cavity.minY; y <= room.cavity.maxY; y++)
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
