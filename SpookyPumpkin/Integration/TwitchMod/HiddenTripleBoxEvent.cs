using FUtility;
using ONITwitchLib;
using UnityEngine;

namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public class HiddenTripleBoxEvent() : HiddenEventBase(ID)
	{
		public const string BOX_ID = "ONITwitch.SurpriseBoxConfig";
		public const string ID = "TripleBox";

		public override Danger GetDanger() => Danger.None;

		public override int GetNiceness() => Intent.GOOD;

		public override void Run()
		{
			GameObject target = null;

			var cell = ONITwitchLib.Utils.PosUtil.ClampedMouseCell();
			var attempts = 0;

			while (Grid.IsSolidCell(cell) && attempts < 16)
				cell = ONITwitchLib.Utils.PosUtil.RandomCellNearMouse();

			for (int i = 0; i < 3; i++)
			{
				var box = Utils.Spawn(BOX_ID, Grid.CellToPos(cell));
				Utils.YeetRandomly(box, true, 2, 5, false);

				target ??= box;
			}

			ToastManager.InstantiateToastWithGoTarget(
				STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.TRICKORTREAT.TREAT,
				STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.TRIPLEBOX.TOAST_BODY,
				target);
		}
	}
}
