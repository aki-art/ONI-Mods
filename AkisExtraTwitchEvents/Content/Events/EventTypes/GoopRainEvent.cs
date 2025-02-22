using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class GoopRainEvent() : TwitchEventBase(ID)
	{
		public const string ID = "GoopRain";

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override Danger GetDanger() => Danger.Medium;

		public override void Run()
		{
			var go = new GameObject("jello cloud spawner");

			var rain = go.AddComponent<LiquidRainSpawner>();

			rain.totalAmountRangeKg = (1000, 3000);
			rain.durationInSeconds = 240;
			rain.dropletMassKg = 0.05f;
			rain.AddElement(Elements.PinkSlime);
			rain.spawnRadius = 15;

			go.SetActive(true);

			GameScheduler.Instance.Schedule("goop rain", 1.5f, _ =>
			{
				rain.StartRaining();
				AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.15f); // its loud
			});

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.GOOPRAIN.TOAST,
				STRINGS.AETE_EVENTS.GOOPRAIN.DESC);
		}
	}
}
