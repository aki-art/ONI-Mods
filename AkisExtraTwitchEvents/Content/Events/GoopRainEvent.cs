using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
    public class GoopRainEvent : ITwitchEvent
    {
        public const string ID = "GoopRain";

        public bool Condition(object data) => true;

        public string GetID() => ID;

        public void Run(object data)
        {
            var go = new GameObject("jello cloud spawner");

            var rain = go.AddComponent<LiquidRainSpawner>();

            rain.totalAmountRangeKg = (10000, 30000);
            rain.durationInSeconds = 240;
            rain.dropletMassKg = 0.5f;
            rain.elementId = Elements.PinkSlime;
            rain.spawnRadius = 15;

            go.SetActive(true);

            GameScheduler.Instance.Schedule("goop rain", 1.5f, _ =>
            {
                rain.StartRaining();
                AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.15f); // its loud
            });

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.SLIME_RAIN.TOAST,
				STRINGS.AETE_EVENTS.SLIME_RAIN.DESC);
		}
    }
}
