using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Scripts;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class HotShowerEvent() : TwitchEventBase(ID)
	{
		public const string ID = "HotShower";

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override Danger GetDanger() => Danger.High;

		public override void Run()
		{
			var go = new GameObject("cloud spawner");

			var rain = go.AddComponent<LiquidRainSpawner>();

			rain.totalAmountRangeKg = (2000, 4000);
			rain.durationInSeconds = 120;
			rain.dropletMassKg = 0.1f;
			rain.spawnRadius = 4;

			rain.AddElement(SimHashes.Water, 1f, GameUtil.GetTemperatureConvertedToKelvin(85, GameUtil.TemperatureUnit.Celsius));

			go.SetActive(true);

			//AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.15f); // its loud

			GameScheduler.Instance.Schedule("hot water rain", 3f, _ =>
			{
				rain.StartRaining();
			});

			var pos = PosUtil.ClampedMouseCellWorldPos();
			var cell = GridUtil.NearestEmptyCell(Grid.PosToCell(pos));

			if (GridUtil.IsCellEmpty(cell))
				SimMessages.ReplaceElement(cell, SimHashes.Steam, AGridUtil.cellEvent, 100);

			ToastManager.InstantiateToastWithPosTarget(
				STRINGS.AETE_EVENTS.HOTSHOWER.TOAST,
				STRINGS.AETE_EVENTS.HOTSHOWER.DESC,
				Grid.CellToPos(cell));
		}
	}
}
