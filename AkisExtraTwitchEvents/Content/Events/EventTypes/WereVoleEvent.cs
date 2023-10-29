using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	public class WereVoleEvent : ITwitchEvent
	{
		public const string ID = "WereVole";
		public static EventInfo ev;

		public bool Condition(object data) => Mod.wereVoles.Count < Components.MinionIdentities.Count;

		public string GetID() => ID;

		public int GetWeight() => Mod.wereVoles.Count > 0 ? TwitchEvents.Weights.RARE : TwitchEvents.Weights.COMMON;

		public void Run(object data)
		{
			var potentials = Components.LiveMinionIdentities.Items
				.FindAll(identity => identity.GetComponent<AETE_MinionStorage>().IsWereVole);

			if (potentials.Count == 0)
			{
				ToastManager.InstantiateToast("Problem", "Something went wrong, there are no Duplicants left to target.\n\nSkipping event.");
				return;
			}

			var target = potentials.GetRandom();
			target.GetComponent<AETE_MinionStorage>().BecomeWereVole();

			ToastManager.InstantiateToastWithGoTarget(
				STRINGS.AETE_EVENTS.WEREVOLE.TOAST,
				STRINGS.AETE_EVENTS.WEREVOLE.DESC.Replace("{Name}", target.GetProperName()),
				target.gameObject);

			ev.Group.SetWeight(ev, GetWeight());
		}
	}
}