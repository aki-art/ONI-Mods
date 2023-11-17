using UnityEngine;

namespace SpookyPumpkinSO.Content.Foods
{
	public class DehydratedPumpkinPieConfig : IEntityConfig
	{
		public const string ID = "SP_DehydratedPumpkinPie";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DEHYDRATED.NAME,
				STRINGS.ITEMS.FOOD.SP_PUMPKINPIE.DEHYDRATED.DESC,
				1f,
				true,
				Assets.GetAnim("sp_dehydrated_pumpkinpie_kanim"),
				"idle",
				Grid.SceneLayer.BuildingFront,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.6f,
				0.7f,
				true,
				1,
				SimHashes.Polypropylene);

			EntityTemplates.ExtendEntityToDehydratedFoodPackage(prefab, SPFoodInfos.pumpkinPie);

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
