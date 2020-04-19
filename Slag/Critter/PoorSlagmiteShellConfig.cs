using System.Collections.Generic;
using UnityEngine;

namespace Slag.Critter
{
    class PoorSlagmiteShellConfig : IEntityConfig
    {

        public static string ID = "PoorSlagmiteShell";
        public static List<WeightedMetalOption> metalOptions;
        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                id: ID,
                name: "Slag Wool",
                desc: "Slag wool desc.",
                mass: 1f,
                unitMass: true,
                anim: Assets.GetAnim("swampreedwool_kanim"),
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
                    GameTags.IndustrialIngredient
                });

            prefab.AddOrGet<EntitySplitter>();

            return prefab;
        }

        public void OnPrefabInit(GameObject inst)
        {
            metalOptions = new List<WeightedMetalOption>()
            {
                new WeightedMetalOption(SimHashes.AluminumOre,  .6f),
                new WeightedMetalOption(SimHashes.Cuprite,      .6f),
                new WeightedMetalOption(SimHashes.Electrum,     .6f),
                new WeightedMetalOption(SimHashes.FoolsGold,    .6f),
                new WeightedMetalOption(SimHashes.GoldAmalgam,  .6f),
                new WeightedMetalOption(SimHashes.IronOre,      .6f),
                new WeightedMetalOption(SimHashes.Wolframite,   .6f)
            };
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
