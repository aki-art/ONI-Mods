using FUtility;
using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class RetroVisionEvent() : TwitchEventBase(ID)
	{
		public const string ID = "RetroVision";

		public override int GetWeight() => Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.None;

		public override void Run()
		{
			if (AETE_DitherPostFx.Instance == null)
			{
				Log.Warning("AETE_CameraPostFx.Instance is null.");
				return;
			}

			AETE_DitherPostFx.Instance.DoDither();

			ONITwitchLib.ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.RETROVISION.TOAST,
				STRINGS.AETE_EVENTS.RETROVISION.DESC);
		}
	}
}
