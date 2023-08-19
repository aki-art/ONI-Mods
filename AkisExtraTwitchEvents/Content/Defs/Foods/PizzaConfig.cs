using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class PizzaConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_Pizza";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_PIZZA.NAME,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_PIZZA.DESC,
				1f,
				true,
				Assets.GetAnim("aete_pizza_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1f,
				0.45f,
				true);

			EntityTemplates.ExtendEntityToFood(prefab, TFoodInfos.pizza);

			if (prefab.TryGetComponent(out PrimaryElement primaryElement))
				primaryElement.Temperature = GameUtil.GetTemperatureConvertedToKelvin(60, GameUtil.TemperatureUnit.Celsius);


			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
