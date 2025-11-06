using ONITwitchLib;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes.CalamityEvents
{
	public class SandStormMediumEvent() : SandStormEventBase(ID)
	{
		public const string ID = "SandStormMedium";

		public override void ConfigureStorm(AETE_SandStorm storm)
		{
			storm.minSmallWorms = 0;
			storm.maxSmallWorms = 0;
			storm.spawnBigWorm = false;
			storm.durationInSeconds = 60f;
			storm.nearSandfallDensity = 0.01f;
		}

		public override Danger GetDanger() => Danger.Medium;

		protected override string GetExtraMessage() => string.Empty;
	}
}
