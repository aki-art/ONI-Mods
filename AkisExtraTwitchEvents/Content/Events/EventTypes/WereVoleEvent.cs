using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	public class WereVoleEvent() : TwitchEventBase(ID)
	{
		public const string ID = "WereVole";
		public static EventInfo ev;

		public override bool Condition() => Mod.wereVoles.Count < Components.MinionIdentities.Count;

		public string GetID() => ID;

		public override int GetWeight() => Mod.wereVoles.Count > 0 ? Consts.EventWeight.Rare : Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.Small;

		public override void Run()
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