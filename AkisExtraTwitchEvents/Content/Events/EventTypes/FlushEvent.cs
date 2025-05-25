using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	public class FlushEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Flush";
		private const float PERCENT_TO_REMOVE = 0.1f;

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			foreach (var cell in GridUtil.ActiveSimCells())
			{
				if (Grid.IsValidCell(cell) && Grid.IsVisible(cell) && (Grid.WorldIdx[cell] != byte.MaxValue))
				{
					if (Grid.IsLiquid(cell))
					{
						// straight up stolen from Uninclude Oxy event
						var mass = Grid.Mass[cell];
						if (mass > 0.05f)
						{
							SimMessages.ModifyMass(
								cell,
								-(mass * PERCENT_TO_REMOVE),
								byte.MaxValue,
								0,
								AGridUtil.modifyEvent,
								Grid.Temperature[cell],
								Grid.Element[cell].id
							);
						}
					}
				}
			}

			AudioUtil.PlaySound(ModAssets.Sounds.FLUSH, ModAssets.GetSFXVolume());

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.FLUSH.TOAST,
				STRINGS.AETE_EVENTS.FLUSH.DESC.Replace("{Percent}", GameUtil.GetFormattedPercent(PERCENT_TO_REMOVE * 100f)));
		}
	}
}
