using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class GoldRainHoneyEvent() : TwitchEventBase(ID)
	{
		public const string ID = "GoldRainHoney";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var go = new GameObject("honey cloud spawner");

			var rain = go.AddComponent<LiquidRainSpawner>();

			rain.totalAmountRangeKg = (2000, 4000);
			rain.durationInSeconds = 120;
			rain.dropletMassKg = 0.5f;
			rain.AddElement(Elements.Honey);
			rain.spawnRadius = 6;

			go.SetActive(true);

			GameScheduler.Instance.Schedule("honey rain", 1.5f, _ =>
			{
				rain.StartRaining();
				AudioUtil.PlaySound(ModAssets.Sounds.GOLD, ModAssets.GetSFXVolume() * 0.15f); // its loud
			});

			ToastManager.InstantiateToast(
				Strings.Get("STRINGS.AETE_EVENTS.GOLDRAINHONEY.TOAST"),
				STRINGS.AETE_EVENTS.GOLDRAINHONEY.DESC);
		}
	}
}
