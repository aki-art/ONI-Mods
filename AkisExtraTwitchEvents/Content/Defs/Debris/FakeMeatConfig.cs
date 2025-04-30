/*using UnityEngine;

namespace Twitchery.Content.Defs.Debris
{
	public class FakeMeatConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.FakeMeat;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);
			prefab.GetComponent<KPrefabID>().prefabSpawnFn += go =>
			{
				var debris = FUtility.Utils.Spawn(MeatConfig.ID, go);
				debris.TryGetComponent(out PrimaryElement debrisPrimaryElement);
				go.TryGetComponent(out PrimaryElement elemPrimaryElement);

				debrisPrimaryElement.AddDisease(elemPrimaryElement.DiseaseIdx, elemPrimaryElement.DiseaseCount, "copy fake meat");
				debrisPrimaryElement.SetMass(elemPrimaryElement.Mass / 100f); // otherwise the amount gets insane
				debrisPrimaryElement.SetTemperature(elemPrimaryElement.Temperature);

				Util.KDestroyGameObject(go);
			};

			return prefab;
		}
	}
}
*/