using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class EggEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Egg";

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => TwitchEvents.Weights.COMMON;

		public override void Run()
		{
			AkisTwitchEvents.Instance.eggFx.Activate();
		}
	}
}
