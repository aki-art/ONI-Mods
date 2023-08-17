using Klei.AI;
using UnityEngine;

namespace Twitchery.Content.Events
{
    public class CoffeeBreakEvent : ITwitchEvent
    {
        public string GetID() => "CoffeeBreak";

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public bool Condition(object data) => true;

        public void Run(object data)
        {
            foreach (MinionIdentity minion in Components.LiveMinionIdentities)
            {
                new EmoteChore(
                    minion.GetComponent<ChoreProvider>(), 
                    Db.Get().ChoreTypes.EmoteHighPriority, 
                    TEmotes.coffeeBreak);

                if (minion.TryGetComponent(out Effects effects))
                {
                    effects.Add(TEffects.CAFFEINATED, true);
                }
            }

            AudioUtil.PlaySound(ModAssets.Sounds.TEA, ModAssets.GetSFXVolume() * 0.2f);
            ONITwitchLib.ToastManager.InstantiateToast(
                STRINGS.AETE_EVENTS.COFFEE_BREAK.TOAST,
                STRINGS.AETE_EVENTS.COFFEE_BREAK.DESC);
        }
    }
}
