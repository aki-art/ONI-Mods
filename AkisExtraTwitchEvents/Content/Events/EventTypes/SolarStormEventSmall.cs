using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class SolarStormEventSmall() : TwitchEventBase(ID)
	{
		public const string ID = "SolarStormSmall";

		public override Danger GetDanger() => Danger.Small;

		public override bool Condition() => AkisTwitchEvents.MaxDanger <= Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			AkisTwitchEvents.Instance.BeginSolarStorm(Mod.Settings.SolarStorm_Duration_Cycles * 600f, false, false);
		}
	}
}
