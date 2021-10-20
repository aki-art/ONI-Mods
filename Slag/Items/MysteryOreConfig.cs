using ProcGen;
using Slag.Critter;
using System.Collections.Generic;
using UnityEngine;
using FUtility;

namespace Slag.Items
{
    class MysteryOreConfig : IEntityConfig
    {
        public static string ID = "MysteryOre";
        public static SimHashes chosenElement;
        private static List<WeightedMetalOption> oreOptions;

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
                element: ModAssets.slagSimHash,
                additionalTags: new List<Tag>
                {
                    GameTags.MiscPickupable
                });

            prefab.AddOrGet<EntitySplitter>();

            return prefab;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
/*            // chose a random metal
            chosenElement = WeightedRandom.Choose(oreOptions, ModAssets.miteRandom).element;

            Log.Debuglog($"Spawned a mystery ore, the chosen element is {chosenElement}");

            // spawn the random ore
            var original = inst.GetComponent<PrimaryElement>();
            var element = ElementLoader.FindElementByHash(chosenElement);

            var result = element.substance.SpawnResource(
                position: inst.transform.position,
                mass: original.Mass,
                temperature: original.Temperature,
                disease_idx: original.DiseaseIdx,
                disease_count: original.DiseaseCount,
                prevent_merge: false,
                forceTemperature: false,
                manual_activation: false);

            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, element.name, result.transform);

            // self destruct
        }
    }
}
*/