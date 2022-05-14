using UnityEngine;

namespace Slag.Content.Items
{
    public class CottonCandyConfig : IEntityConfig
	{
		public const string ID = "Slag_CottonCandy";
		// public static ComplexRecipe recipe;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.COTTON_CANDY.NAME,
				STRINGS.ITEMS.FOOD.COTTON_CANDY.DESC,
				1f,
				false,
				Assets.GetAnim("cottoncandy_kanim"),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true);

           var foodInfo = new EdiblesManager.FoodInfo(
				ID,
				DlcManager.VANILLA_ID,
				6000000f,
				3,
				255.15f,
				277.15f,
				600f,
				true);

			EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

			return prefab;
		}

        public string[] GetDlcIds()
        {
			return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
