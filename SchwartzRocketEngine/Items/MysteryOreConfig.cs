using System.Collections.Generic;
using UnityEngine;

namespace SchwartzRocketEngine.Items
{
    class MysteryOreConfig : IEntityConfig
    {
        public static string ID = "MysteryOre";
        public static SimHashes chosenElement;
        private static List<SiftResultOption> oreOptions;

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                id: ID,
                name: "Mystery Ore",
                desc: "a mystery.",
                mass: 1f,
                unitMass: true,
                anim: Assets.GetAnim("mystery_ore_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.BuildingBack,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.8f,
                height: 0.45f,
                isPickupable: true,
                sortOrder: 0,
                element: SimHashes.Unobtanium,
                additionalTags: new List<Tag>
                {
                    GameTags.MiscPickupable
                });

            prefab.AddOrGet<EntitySplitter>();

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