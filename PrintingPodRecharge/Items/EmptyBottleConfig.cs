/*using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Items
{
    public class EmptyBottleConfig : IEntityConfig
    {
        public const string ID = "PrintingPodRecharge_EmptyBottle";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.EMPTY_BOTTLE.NAME,
                STRINGS.ITEMS.EMPTY_BOTTLE.DESC,
                1f,
                false,
                Assets.GetAnim("gas_tank_kanim"),
                "idle1",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.9f,
                true,
                0,
                SimHashes.Vacuum,
                additionalTags: new List<Tag>()
                {
                    GameTags.PedestalDisplayable
                });

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
            inst.AddComponent<Dumpable>().SetWorkTime(5f);
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
*/