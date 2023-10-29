using Klei.AI;
using ONITwitchLib;

namespace Twitchery.Content.Events.EventTypes
{
	public class CoffeeBreakEvent() : TwitchEventBase(ID)
	{
		public const string ID = "CoffeeBreak";

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => TwitchEvents.Weights.COMMON;

		public override void Run()
		{
			foreach (var minion in Components.LiveMinionIdentities.Items)
			{
				new EmoteChore(
					minion.GetComponent<ChoreProvider>(),
					Db.Get().ChoreTypes.EmoteHighPriority,
					TEmotes.coffeeBreak);

				if (minion.TryGetComponent(out Effects effects))
					effects.Add(TEffects.CAFFEINATED, true);
			}

			AudioUtil.PlaySound(ModAssets.Sounds.TEA, ModAssets.GetSFXVolume() * 0.2f);
			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.COFFEEBREAK.TOAST,
				STRINGS.AETE_EVENTS.COFFEEBREAK.DESC);
		}
	}
}
