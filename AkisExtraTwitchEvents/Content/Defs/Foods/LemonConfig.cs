using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Foods
{
	public class LemonConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_Lemon";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_LEMON.NAME,
				STRINGS.ITEMS.FOOD.AKISEXTRATWITCHEVENTS_LEMON.DESC,
				1f,
				true,
				Assets.GetAnim("aete_lemon_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1f,
				0.4f,
				true);

			EntityTemplates.ExtendEntityToFood(prefab, TFoodInfos.lemon);
			prefab.AddComponent<Lemon>();

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
