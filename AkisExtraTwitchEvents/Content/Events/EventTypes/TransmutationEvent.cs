using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Scripts;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	class TransmutationEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Transmutation";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			if (!FindNearestEligibleElement(out int cursorCell, out Element targetElement))
			{
				var pos = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
				var cell = Grid.PosToCell(pos);

				SimMessages.ReplaceElement(cell, SimHashes.Methane, Toucher.toucherEvent, 50);

				ToastManager.InstantiateToast(
					STRINGS.AETE_EVENTS.TRANSMUTATION.TOAST,
					STRINGS.AETE_EVENTS.TRANSMUTATION.DESC_FAIL);
			}

			var position = Grid.CellToPos(cursorCell);
			var currentTemperature = Grid.Temperature[cursorCell];

			var circle = ProcGen.Util.GetFilledCircle(position, 8);

			var elementToPick = new List<Element>(ElementLoader.elements)
				.Where(e => !e.disabled
				&& e.id != targetElement.id
				&& e.state == targetElement.state
				&& e.highTemp > currentTemperature
				&& e.lowTemp < currentTemperature
				&& e.hardness < byte.MaxValue
				&& !e.HasTag(ExtraTags.OniTwitchSurpriseBoxForceDisabled)
				&& !e.HasTag(TTags.useless));

			if (elementToPick.Count() == 0)
				return;

			var newElement = elementToPick.GetRandom();

			foreach (var pos in circle)
			{
				var cell = Grid.PosToCell(pos);

				if (Grid.IsValidCell(cell)
					&& Grid.Element[cell].id == targetElement.id
					&& GridUtil.IsCellFoundationEmpty(cell))
					AGridUtil.ReplaceElement(cell, targetElement, newElement.id);
			}

			ToastManager.InstantiateToastWithPosTarget(
				STRINGS.AETE_EVENTS.TRANSMUTATION.TOAST,
				string.Format(STRINGS.AETE_EVENTS.TRANSMUTATION.DESC, targetElement.tag.ProperNameStripLink(), newElement.tag.ProperNameStripLink()),
				position);
		}

		private bool FindNearestEligibleElement(out int cell, out Element element)
		{
			// check right under
			var position = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
			cell = Grid.PosToCell(position);
			var originCell = cell;
			element = Grid.Element[cell];


			if (!IsValidElement(element))
			{
				cell = GameUtil.FloodFillFind((int cell, object _) => IsValidElement(Grid.Element[cell]) && Grid.AreCellsInSameWorld(cell, originCell), null, originCell, 40, stop_at_solid: false, stop_at_liquid: false);

				if (Grid.IsValidCell(cell))
					element = Grid.Element[cell];
			}

			return Grid.IsValidCell(cell);
		}

		private static bool IsValidElement(Element element)
		{
			if (element.hardness == byte.MaxValue && element.materialCategory == GameTags.Special)
				return false;

			if (element.id == SimHashes.Vacuum)
				return false;

			return true;
		}
	}
}
