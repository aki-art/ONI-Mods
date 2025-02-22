using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class InvisibleLiquidsEvent() : TwitchEventBase(ID)
	{
		public const string ID = "InvisibleLiquids";

		public override int GetWeight() => TwitchEvents.Weights.COMMON;

		public override Danger GetDanger() => Danger.None;

		public override void Run()
		{
			AkisTwitchEvents.Instance.hideLiquids = true;
			AkisTwitchEvents.Instance.ApplyLiquidTransparency(WaterCubes.Instance);

			GameScheduler.Instance.Schedule("test", ModTuning.INVISIBLE_LIQUIDS_DURATION, _ =>
			{
				AkisTwitchEvents.Instance.hideLiquids = false;
				AkisTwitchEvents.Instance.ApplyLiquidTransparency(WaterCubes.Instance);
			});

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.INVISIBLELIQUIDS.TOAST,
				STRINGS.AETE_EVENTS.INVISIBLELIQUIDS.DESC);
		}
	}
}
