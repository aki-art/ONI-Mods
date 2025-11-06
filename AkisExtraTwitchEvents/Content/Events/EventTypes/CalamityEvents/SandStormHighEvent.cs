using ONITwitchLib;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes.CalamityEvents
{
	public class SandStormHighEvent() : SandStormEventBase(ID)
	{
		public const string ID = "SandStormHigh";

		public override void ConfigureStorm(AETE_SandStorm storm)
		{
			storm.minSmallWorms = 2;
			storm.maxSmallWorms = 4;
			storm.spawnBigWorm = false;
			storm.durationInSeconds = 120f;
			storm.nearSandfallDensity = 0.03f;
		}

		public override Danger GetDanger() => Danger.High;
		protected override string GetExtraMessage() => STRINGS.AETE_EVENTS.SANDSTORMHIGH.DESC_EXTRA;
	}
}
