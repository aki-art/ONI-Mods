using ONITwitchLib;
using System.Linq;
using Twitchery.Content.Scripts;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class RainbowRain2Event() : TwitchEventBase(ID)
	{
		public const string ID = "RainbowRain2";

		public override Danger GetDanger() => Danger.Extreme;

		public override int GetWeight() => Consts.EventWeight.Rare;

		public override string GetName() => STRINGS.AETE_EVENTS.RAINBOWRAIN.TOAST;

		public override bool Condition() => Mod.Settings.Cursed;

		public override void Run()
		{
			var go = new GameObject("rainbow cloud spawner");

			var rain = go.AddComponent<LiquidRainSpawner>();

			rain.totalAmountRangeKg = (2000, 5000);
			rain.durationInSeconds = 80;
			rain.dropletMassKg = 0.03f;
			rain.spawnRadius = 3;

			var potentialElements = MiscUtil.dangerousLiquids;
			potentialElements.Shuffle();

			foreach (var option in potentialElements.Take(Mathf.Min(12, potentialElements.Count)))
			{
				var element = ElementLoader.GetElement(option.id);
				rain.AddElement(element.id, option.weight, option.temperature, element.defaultValues.mass / option.mass);
			}

			go.SetActive(true);

			rain.StartRaining();
			AudioUtil.PlaySound(ModAssets.Sounds.EXCLAIM, ModAssets.GetSFXVolume() * 0.65f);

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.RAINBOWRAIN.TOAST,
				STRINGS.AETE_EVENTS.RAINBOWRAIN.DESC_DEADLY);
		}
	}
}
