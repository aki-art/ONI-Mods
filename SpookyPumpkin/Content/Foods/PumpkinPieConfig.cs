using UnityEngine;

namespace SpookyPumpkinSO.Content.Foods
{
	public class PumpkinPieConfig : IEntityConfig
	{
		public const string ID = "SP_PumpkinPie";
		public static ComplexRecipe recipe;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.NAME,
				STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DESC,
				1f,
				false,
				Assets.GetAnim("sp_pumpkinpie_kanim"),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true);

			return EntityTemplates.ExtendEntityToFood(prefab, SPFoodInfos.pumpkinPie);
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
