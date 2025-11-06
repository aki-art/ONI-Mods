using ONITwitchLib;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes.CalamityEvents
{
	public class SandStormDeadlyEvent() : SandStormEventBase(ID)
	{
		public const string ID = "SandStormDeadly";

		public override void ConfigureStorm(AETE_SandStorm storm)
		{
			storm.minSmallWorms = 1;
			storm.maxSmallWorms = 3;
			storm.spawnBigWorm = true;
			storm.durationInSeconds = 240f;
			storm.nearSandfallDensity = 0.05f;
		}

		public override Danger GetDanger() => Danger.Deadly;

		protected override string GetExtraMessage() => STRINGS.AETE_EVENTS.SANDSTORMDEADLY.DESC_EXTRA;
	}
}
