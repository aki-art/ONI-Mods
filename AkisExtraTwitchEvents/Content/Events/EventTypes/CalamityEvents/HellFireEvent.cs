using ONITwitchLib;
using Twitchery.Content.Defs.Calamities;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes.CalamityEvents
{
	public class HellFireEvent() : TwitchEventBase(ID)
	{
		public const string ID = "HellFire";

		public const float DURATION = 120f;

		public override Danger GetDanger() => Danger.Deadly;

		public override bool Condition() => AETE_WorldEventsManager.Instance.CanStartNewEvent(HellFireSpawnerConfig.ID, true);

		public override int GetWeight() => Consts.EventWeight.Uncommon - 1;

		public override void Run()
		{
			var go = AETE_WorldEventsManager.Instance.CreateEvent(HellFireSpawnerConfig.ID, true);

			if (go != null && go.TryGetComponent(out AETE_HellFire calamity))
			{
				calamity.density = 0.1f;
				calamity.radius = 4;
				calamity.durationInSeconds = 120f;

				calamity.Begin();
			}
		}
	}
}
