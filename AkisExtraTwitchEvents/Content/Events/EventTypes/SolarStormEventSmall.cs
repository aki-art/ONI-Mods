using ONITwitchLib;
using Twitchery.Content.Defs.Calamities;
using Twitchery.Content.Scripts.WorldEvents;

namespace Twitchery.Content.Events.EventTypes
{
	public class SolarStormEventSmall() : TwitchEventBase(ID)
	{
		public const string ID = "SolarStormSmall";

		public override Danger GetDanger() => Danger.Small;

		public override bool Condition() => AETE_WorldEventsManager.Instance.CanStartNewEvent(SolarStormSpawnerConfig.ID, true);

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var go = AETE_WorldEventsManager.Instance.CreateEvent(SolarStormSpawnerConfig.ID, true);

			if (go.TryGetComponent(out AETE_SolarStorm storm))
			{
				storm.durationInSeconds = Mod.Settings.SolarStorm_Duration_Cycles * 600f;
				storm.isAggressive = false;

				storm.Begin();
			}

			var world = go.GetMyWorld();
			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.SOLARSTORMSMALL.TOAST, string.Format(STRINGS.AETE_EVENTS.SOLARSTORMSMALL.DESC, world == null ? "n/a" : world.GetProperName()));
		}
	}
}
