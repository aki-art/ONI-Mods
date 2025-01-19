using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class JelloRainEvent : ITwitchEvent
	{
		public const string ID = "JelloRain";

		public int GetWeight() => Consts.EventWeight.Uncommon;

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{
			var go = new GameObject("jello cloud spawner");

			var rain = go.AddComponent<LiquidRainSpawner>();

			var danger = (float)AkisTwitchEvents.MaxDanger / 6f;
			var minKcal = Mathf.Lerp(5000, 30000, danger);
			var maxKcal = Mathf.Lerp(10000, 60000, danger);

			rain.totalAmountRangeKg = (minKcal / TFoodInfos.JELLO_KCAL_PER_KG, maxKcal / TFoodInfos.JELLO_KCAL_PER_KG);
			rain.durationInSeconds = 240;
			rain.dropletMassKg = 0.01f;
			rain.spawnRadius = 10;

			rain.AddElement(Elements.Jello);

			go.SetActive(true);

			GameScheduler.Instance.Schedule("jello rain", 3f, _ =>
			{
				rain.StartRaining();
				AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.15f); // its loud
			});

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.JELLO_RAIN.TOAST,
				STRINGS.AETE_EVENTS.JELLO_RAIN.DESC);
		}
	}
}
