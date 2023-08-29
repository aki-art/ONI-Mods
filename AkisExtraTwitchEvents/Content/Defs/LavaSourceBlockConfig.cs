using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class LavaSourceBlockConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_LavaSourceBlock";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Lava Source",
				"...",
				100f,
				 Assets.GetAnim("barbeque_kanim"),
				"none",
				Grid.SceneLayer.Creatures,
				1,
				1,
				default,
				defaultTemperature: 1700);

			prefab.AddComponent<SaveLoadRoot>();

			prefab.AddOrGet<PocketDimensionHandler>();

			var emitter = prefab.AddOrGet<ElementEmitter>();
			emitter.outputElement = new ElementConverter.OutputElement(5, SimHashes.Magma, 1700);
			emitter.maxPressure = 10;

			prefab.GetComponent<KSelectable>().selectable = false;

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
