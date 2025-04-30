using ONITwitchLib;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	public class RemoveLiquidsEvent() : TwitchEventBase(ID)
	{
		public const string ID = "RemoveLiquids";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var position = ONITwitchLib.Utils.PosUtil.ClampedMouseCellWorldPos();
			var circle = ProcGen.Util.GetFilledCircle(position, 3);

			foreach (var pos in circle)
			{
				var cell = Grid.PosToCell(pos);
				if (Grid.IsLiquid(cell))
					SimMessages.ReplaceElement(cell, SimHashes.Vacuum, AGridUtil.cellEvent, 0f);
			}

			AudioUtil.PlaySound(ModAssets.Sounds.SLURP, ModAssets.GetSFXVolume() * 0.7f);

			ToastManager.InstantiateToastWithPosTarget(STRINGS.AETE_EVENTS.REMOVELIQUIDS.TOAST, STRINGS.AETE_EVENTS.REMOVELIQUIDS.DESC, position);
		}
	}
}
