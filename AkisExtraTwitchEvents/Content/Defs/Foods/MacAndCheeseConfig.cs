using UnityEngine;

namespace Twitchery.Content.Defs.Foods
{
	internal class MacAndCheeseConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_MacAndCheese";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_MACANDCHEESE.NAME,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_MACANDCHEESE.DESC,
				1f,
				true,
				Assets.GetAnim("aete_macandcheese_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1f,
				0.6f,
				true);

			EntityTemplates.ExtendEntityToFood(prefab, TFoodInfos.macNCheese);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
