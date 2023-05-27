using Bestagons.Content.Map;
using UnityEngine;

namespace Bestagons.Content.Defs
{
    public class PurchasableHexConfig : IEntityConfig
    {
        public const string ID = "Bestagons_PurchasableHex";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateEntity(
                ID, 
                "PurchasableHex");

            prefab.AddComponent<PurchasableHex>();

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
