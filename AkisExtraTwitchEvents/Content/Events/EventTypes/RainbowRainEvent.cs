using ONITwitchLib;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class RainbowRainEvent() : TwitchEventBase(ID)
	{
		public const string ID = "RainbowRain";

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => Consts.EventWeight.Common;

		public static HashSet<Tag> ignoredElementIds = [
			SimHashes.ViscoGel.CreateTag(),
			"ITCE_Inverse_Water_Placeholder",
			"ITCE_CreepyLiquid",
			"ITCE_VoidLiquid",
			"Beached_SulfurousWater"
			];

		public override void Run()
		{
			var go = new GameObject("rainbow cloud spawner");

			var rain = go.AddComponent<LiquidRainSpawner>();

			rain.totalAmountRangeKg = (3000, 9000);
			rain.durationInSeconds = 240;
			rain.dropletMassKg = 0.03f;
			rain.spawnRadius = 12;

			var minTemp = GameUtil.GetTemperatureConvertedToKelvin(20, GameUtil.TemperatureUnit.Celsius);
			var maxTemp = GameUtil.GetTemperatureConvertedToKelvin(50, GameUtil.TemperatureUnit.Celsius);

			foreach (var element in ElementLoader.elements)
			{
				if (element.disabled
					|| !element.IsLiquid
					|| element.highTemp < minTemp
					|| element.lowTemp > maxTemp
					|| ignoredElementIds.Contains(element.tag))
					continue;

				var debris = Assets.GetPrefab(element.tag);
				if (debris == null || debris.HasTag(ExtraTags.OniTwitchSurpriseBoxForceDisabled))
					continue;

				rain.AddElement(element.id);
			}


			go.SetActive(true);

			GameScheduler.Instance.Schedule("rainbow rain", 1.5f, _ =>
			{
				rain.StartRaining();
				AudioUtil.PlaySound(ModAssets.Sounds.TEA, ModAssets.GetSFXVolume() * 0.15f);
			});

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.RAINBOWRAIN.TOAST,
				STRINGS.AETE_EVENTS.RAINBOWRAIN.DESC);
		}
	}
}
