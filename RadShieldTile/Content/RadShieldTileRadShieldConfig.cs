using FUtility;
using UnityEngine;

namespace RadShieldTile.Content
{
	public class RadShieldTileRadShieldConfig : IOreConfig
	{
		public SimHashes ElementID => RSTElements.RadShield;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);

			prefab.AddOrGet<KPrefabID>().prefabSpawnFn += OnPrefabSpawn;

			return prefab;
		}

		private void OnPrefabSpawn(GameObject go)
		{
			if (go.TryGetComponent(out PrimaryElement element) && element.ElementID != ElementID)
			{
				Log.Info("Found invalid rad shield chunk");

				ElementLoader.FindElementByHash(ElementID).substance.SpawnResource(
					go.transform.position,
					element.Mass,
					element.Temperature,
					element.DiseaseIdx,
					element.DiseaseCount,
					true);

				Util.KDestroyGameObject(go);
			}
		}
	}
}
