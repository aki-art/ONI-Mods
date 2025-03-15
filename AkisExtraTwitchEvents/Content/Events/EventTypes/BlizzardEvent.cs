using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs.Calamities;
using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes
{
	public class BlizzardEvent(string ID, Danger danger, int level) : TwitchEventBase(ID)
	{
		public const string MEDIUM_ID = "BlizzardMedium";
		public const string DEADLY_ID = "BlizzardDeadly";

		public const float DURATION = 15f; // CONSTS.CYCLE_LENGTH;
		private readonly Danger danger = danger;
		private readonly int level = level;

		public override Danger GetDanger() => danger;

		public override bool Condition() => AkisTwitchEvents.Instance.CanGlobalEventStart();

		public override int GetWeight() => Consts.EventWeight.Uncommon - 1;

		public override void Run()
		{
			var go = FUtility.Utils.Spawn(BlizzardSpawnerConfig.ID, PosUtil.ClampedMouseWorldPos());
			if (go.TryGetComponent(out AETE_Blizzard snowStorm))
			{
				snowStorm.baseSnowfallDensityPerSquare100 = 0.1f;
				snowStorm.nearSnowfallDensity = 0.5f;
				snowStorm.intenseRadius = 10;
				snowStorm.durationInSeconds = 60f;

				snowStorm.Begin();
			}
		}
	}
}
