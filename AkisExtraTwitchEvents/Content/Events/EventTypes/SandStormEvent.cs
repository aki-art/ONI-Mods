using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs.Calamities;
using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes
{
	public class SandStormEvent(string ID, Danger danger, int level) : TwitchEventBase(ID)
	{
		public const string MEDIUM_ID = "SandStormMedium";
		public const string HIGH_ID = "SandStormHigh";
		public const string DEADLY_ID = "SandStormDeadly";

		public const float DURATION = 15f; // CONSTS.CYCLE_LENGTH;
		private readonly Danger danger = danger;
		private readonly int level = level;

		public override Danger GetDanger() => danger;

		public override bool Condition() => AkisTwitchEvents.Instance.CanGlobalEventStart();

		public override int GetWeight() => Consts.EventWeight.Uncommon - 1;

		public override void Run()
		{
			var go = FUtility.Utils.Spawn(SandstormSpawnerConfig.ID, PosUtil.ClampedMouseWorldPos());
			if (go.TryGetComponent(out AETE_SandStorm sandStorm))
			{
				sandStorm.minSmallWorms = 0;
				sandStorm.maxSmallWorms = 0;
				sandStorm.baseSandfallDensityPerSquare100 = 0.1f;
				sandStorm.nearSandfallDensity = 0.5f;
				sandStorm.intenseRadius = 10;
				sandStorm.spawnBigWorm = level > 1;
				sandStorm.durationInSeconds = 60f;

				sandStorm.Begin();
			}
		}
	}
}
