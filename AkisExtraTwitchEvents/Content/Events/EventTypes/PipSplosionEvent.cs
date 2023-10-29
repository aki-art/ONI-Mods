using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
    public class PipSplosionEvent : ITwitchEvent
    {
        public const string ID = "Pipsplosion";

        public int GetWeight() => TwitchEvents.Weights.COMMON;

        public bool Condition(object data) => true;

        public string GetID() => ID;

        public void Run(object data)
        {
            var splosion = new GameObject("Pipsploder").AddComponent<PipSplosionSpawner>();
            splosion.minPips = 10;
            splosion.maxPips = 15;

            ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.PIPSPLOSION.TOAST, STRINGS.AETE_EVENTS.PIPSPLOSION.DESC);
        }
    }
}
