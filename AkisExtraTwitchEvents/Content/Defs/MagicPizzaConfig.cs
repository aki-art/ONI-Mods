using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class MagicPizzaConfig : IEntityConfig
	{
		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				"magicPizza",
				"Pizza",
				"",
				200f,
				true,
				Assets.GetAnim("aete_pizza_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1,
				1,
				false);

			prefab.AddComponent<Rigidbody2D>();
			prefab.AddOrGet<HomingTest>();
			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.isMovable = true;
			kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kbac.isVisible = true;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
