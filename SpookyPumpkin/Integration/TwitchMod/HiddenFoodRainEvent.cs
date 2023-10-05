using FUtility;
using ONITwitchLib;
using SpookyPumpkinSO.Content.Foods;
using System.Collections.Generic;
using UnityEngine;

namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public class HiddenFoodRainEvent() : HiddenEventBase(ID)
	{
		public const string ID = "FoodRain";

		private static readonly List<string> foods = new()
		{
			PumpkinPieConfig.ID,
			ToastedPumpkinSeedConfig.ID,
			BerryPieConfig.ID,
			FruitCakeConfig.ID,
		};

		public override Danger GetDanger() => Danger.None;

		public override void Run()
		{
			var food = foods.GetRandom();

			var kcal = Random.Range(20_000_000, 35_000_000);

			var spawnedKcal = 0f;
			var prefab = Assets.GetPrefab(food);
			var kcalPerItem = EdiblesManager.GetFoodInfo(food).CaloriesPerUnit;

			kcalPerItem = Mathf.Max(kcalPerItem, 100);

			GameObject target = null;
			while (spawnedKcal < kcal)
			{
				var cell = ONITwitchLib.Utils.PosUtil.ClampedMouseCell();
				var attempts = 0;

				while (Grid.IsSolidCell(cell) && attempts < 16)
					cell = ONITwitchLib.Utils.PosUtil.RandomCellNearMouse();

				var pos = Grid.CellToPos(cell);

				var go = GameUtil.KInstantiate(prefab, pos, Grid.SceneLayer.Ore);
				go.SetActive(true);

				Utils.YeetRandomly(go, false, 2, 6, true);

				spawnedKcal += kcalPerItem;

				if (target == null)
					target = go;
			}

			ToastManager.InstantiateToastWithGoTarget(
				STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.TRICKORTREAT.TREAT,
				STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.FOODRAIN.TOAST_BODY,
				target);

		}
		public override int GetNiceness() => Intent.GOOD;
	}
}
