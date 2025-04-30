using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class SubnauticaEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Subnautica";

		public override int GetWeight() => Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.None;


		public override bool Condition() => !AkisTwitchEvents.Instance.IsFakeFloodActive;

		public override void Run()
		{
			AkisTwitchEvents.Instance.WaterOverlayActive = true;
			AkisTwitchEvents.Instance.ToggleOverlay(-1, OverlayRenderer.WATER, true, false);
			GameScheduler.Instance.Schedule("remove water overlay", 120f, _ => AkisTwitchEvents.Instance.ToggleOverlay(-1, OverlayRenderer.WATER, false, false));

			AudioUtil.PlaySound(ModAssets.Sounds.SUBNAUTICA, ModAssets.GetSFXVolume());
		}
	}
}
