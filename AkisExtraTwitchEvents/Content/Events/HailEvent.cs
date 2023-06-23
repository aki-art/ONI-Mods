using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
	public class HailEvent : ITwitchEvent
	{
		public const string ID = "Hail";

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{
			var go = new GameObject("hail cloud spawner");

			var rain = go.AddComponent<PrefabRainSpawner>();

			rain.totalAmountRangeKg = (10000, 30000);
			rain.durationInSeconds = 240;
			rain.prefabTag = SimHashes.Ice.CreateTag();
			rain.spawnRadius = 13;
			rain.SetMass(100f);
			rain.SetTemperature(-30);

			go.SetActive(true);

			rain.StartRaining();
			AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.15f); // its loud

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.HAIL_RAIN.TOAST,
				STRINGS.AETE_EVENTS.HAIL_RAIN.DESC);
		}
	}
}
