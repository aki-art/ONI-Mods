using UnityEngine;

namespace Twitchery.Content.Defs.Debris
{
	public class FakeBarbequeConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.FakeBarbeque;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);
			prefab.GetComponent<KPrefabID>().prefabSpawnFn += go =>
			{
				var debris = FUtility.Utils.Spawn(CookedMeatConfig.ID, go);
				debris.TryGetComponent(out PrimaryElement debrisPrimaryElement);
				go.TryGetComponent(out PrimaryElement elemPrimaryElement);

				debrisPrimaryElement.AddDisease(elemPrimaryElement.DiseaseIdx, elemPrimaryElement.DiseaseCount, "copy fake meat");
				debrisPrimaryElement.SetMass(elemPrimaryElement.Mass);
				debrisPrimaryElement.SetTemperature(elemPrimaryElement.Temperature);

				Util.KDestroyGameObject(go);
			};

			return prefab;
		}
	}
}
