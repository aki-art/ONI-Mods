using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class BrackeneRainEvent : ITwitchEvent
	{
		public const string ID = "BrackeneRain";

		public bool Condition(object data) => DiscoveredResources.Instance.IsDiscovered(SimHashes.Milk.CreateTag());

		public string GetID() => ID;

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public void Run(object data)
		{
			var go = new GameObject("brackene cloud spawner");

			var rain = go.AddComponent<LiquidRainSpawner>();

			rain.totalAmountRangeKg = (1000, 3000);
			rain.durationInSeconds = 240;
			rain.dropletMassKg = 0.05f;
			rain.elementId = SimHashes.Milk;
			rain.spawnRadius = 15;

			go.SetActive(true);

			GameScheduler.Instance.Schedule("milk rain", 1.5f, _ =>
			{
				rain.StartRaining();
				AudioUtil.PlaySound(ModAssets.Sounds.TEA, ModAssets.GetSFXVolume() * 0.15f); // its loud
			});

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.BRACKENE_RAIN.TOAST,
				STRINGS.AETE_EVENTS.BRACKENE_RAIN.DESC);
		}
	}
}
