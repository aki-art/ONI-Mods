using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class SolarStormEventMedium() : TwitchEventBase(ID)
	{
		public const string ID = "SolarStormMedium";

		public override Danger GetDanger() => Danger.Medium;

		public override bool Condition() => AkisTwitchEvents.MaxDanger > Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			AkisTwitchEvents.Instance.BeginSolarStorm(0, Mod.Settings.SolarStorm_Duration_Cycles * 600f, true, true);
		}
	}
}
