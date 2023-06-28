using UnityEngine;

namespace Twitchery.Content.Defs.Debris
{
    public class FakeLumberConfig : IOreConfig
    {
        public SimHashes ElementID => Elements.FakeLumber;

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateSolidOreEntity(ElementID);
            prefab.GetComponent<KPrefabID>().prefabSpawnFn += go =>
            {
                var log = FUtility.Utils.Spawn(WoodLogConfig.ID, go);
                log.TryGetComponent(out PrimaryElement logPrimaryElement);
                go.TryGetComponent(out PrimaryElement elemPrimaryElement);

                logPrimaryElement.AddDisease(elemPrimaryElement.DiseaseIdx, elemPrimaryElement.DiseaseCount, "copy fake lumber");
                logPrimaryElement.SetMass(elemPrimaryElement.Mass);
                logPrimaryElement.SetTemperature(elemPrimaryElement.Temperature);

                Util.KDestroyGameObject(go);
            };

            return prefab;
        }
    }
}
