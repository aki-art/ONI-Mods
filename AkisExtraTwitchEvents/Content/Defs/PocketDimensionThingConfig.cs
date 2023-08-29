using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class PocketDimensionThingConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_PocketDimensionThing";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, "Pocket Dimension Thing", false);

			prefab.AddComponent<SaveLoadRoot>();

			var kbac = prefab.AddComponent<KBatchedAnimController>();
			kbac.AnimFiles = new[] { Assets.GetAnim("aete_nether_bg_kanim") };
			kbac.sceneLayer = Grid.SceneLayer.Background;
			kbac.initialAnim = "object";

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
