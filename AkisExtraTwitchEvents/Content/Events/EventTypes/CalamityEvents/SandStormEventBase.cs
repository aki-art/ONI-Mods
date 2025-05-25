using ONITwitchLib;
using Twitchery.Content.Defs.Calamities;
using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes.CalamityEvents
{
	public abstract class SandStormEventBase(string ID) : TwitchEventBase(ID)
	{
		public override bool Condition() => AkisTwitchEvents.Instance.CanGlobalEventStart();

		public override int GetWeight() => Consts.EventWeight.Uncommon - 1;

		public abstract void ConfigureStorm(AETE_SandStorm storm);

		public override void Run()
		{
			var go = AETE_WorldEventsManager.Instance.CreateEvent(SandstormSpawnerConfig.ID, true);

			if (go.TryGetComponent(out AETE_SandStorm sandStorm))
			{
				sandStorm.baseSandfallDensityPerSquare100 = 0.1f;
				sandStorm.nearSandfallDensity = 0.5f;
				sandStorm.intenseRadius = 10;
				sandStorm.durationInSeconds = 60f;

				ConfigureStorm(sandStorm);

				sandStorm.Begin();
			}
		}
	}
}
