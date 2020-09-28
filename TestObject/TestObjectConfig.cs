using System.Collections.Generic;
using UnityEngine;

namespace TestObject
{
    class TestObjectConfig : IEntityConfig
    {
        public static string ID = "TestObject";

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                id: ID,
                name: "Test",
                desc: "test",
                mass: 1f,
                unitMass: true,
                anim: Assets.GetAnim("barbeque_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.BuildingBack,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.8f,
                height: 0.45f,
                isPickupable: true,
                sortOrder: 0,
                element: SimHashes.Aerogel,
                additionalTags: new List<Tag>
                {
                    Patches.myCategory
                });

            prefab.AddOrGet<EntitySplitter>();

            return prefab;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            WorldInventory.Instance.Discover(ID, Patches.myCategory);
        }
    }
}