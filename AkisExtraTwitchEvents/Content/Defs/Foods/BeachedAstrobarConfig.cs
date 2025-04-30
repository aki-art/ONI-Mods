using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Defs.Foods
{
	public class BeachedAstrobarConfig : IMultiEntityConfig
	{
		public const string ID = "Beached_Astrobar";

		public List<GameObject> CreatePrefabs() => Mod.isBeachedHere ? [] : [CreateAstrobarPrefab()];

		private GameObject CreateAstrobarPrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.BEACHED_ASTROBAR.NAME,
				STRINGS.ITEMS.FOOD.BEACHED_ASTROBAR.DESC,
				1f,
				false,
				Assets.GetAnim("aete_beached_astrobar_kanim"),
				"object",
				Grid.SceneLayer.Front,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.5f,
				true);

			EntityTemplates.ExtendEntityToFood(prefab, TFoodInfos.beachedAstrobar);

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
