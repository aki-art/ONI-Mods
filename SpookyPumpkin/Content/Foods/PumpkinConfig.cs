using System.Collections.Generic;
using UnityEngine;
using static SpookyPumpkinSO.STRINGS.ITEMS.FOOD;

namespace SpookyPumpkinSO.Content.Foods
{
	public class PumpkinConfig : IEntityConfig
	{
		public const string ID = "SP_Pumpkin";

		public GameObject CreatePrefab()
		{
			var looseEntity = EntityTemplates.CreateLooseEntity(
				ID,
				SP_PUMPKIN.NAME,
				SP_PUMPKIN.DESC,
				1f,
				false,
				Assets.GetAnim("sp_itempumpkin_kanim"),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true,
				additionalTags: new List<Tag>
				{
					ModAssets.buildingPumpkinTag
				});

			return EntityTemplates.ExtendEntityToFood(looseEntity, SPFoodInfos.pumpkin);
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
