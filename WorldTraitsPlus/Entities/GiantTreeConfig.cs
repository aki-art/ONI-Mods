using System.Collections.Generic;
using UnityEngine;

namespace WorldTraitsPlus.Entities
{
    public class GiantTreeConfig : IEntityConfig
    {
        public const string ID = "GiantTree";
        public const string SEED_ID = "GiantTreeSeed";
        public const float FERTILIZATION_RATE = 0.01666667f;
        public const float WATER_RATE = 0.1166667f;
        public const float BRANCH_GROWTH_TIME = 2100f;
        public const int NUM_BRANCHES = 7;

        public GameObject CreatePrefab()
        {
            GameObject placedEntity = EntityTemplates.CreatePlacedEntity(
                ID,
                "name",
                global::STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC,
                2f,
                Assets.GetAnim("tree_kanim"),
                "idle_empty",
                Grid.SceneLayer.Building,
                3,
                6,
                TUNING.DECOR.BONUS.TIER1,
                new EffectorValues());

            EntityTemplates.ExtendEntityToBasicPlant(
                placedEntity,
                258.15f,
                288.15f,
                313.15f,
                448.15f,
                crop_id: "WoodLog",
                should_grow_old: false);

            placedEntity.AddOrGet<BuddingTrunk>();
            placedEntity.UpdateComponentRequirement<Harvestable>(false);

            Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;

            EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
            {
              new PlantElementAbsorber.ConsumeInfo()
              {
                tag = tag,
                massConsumptionRate = 0.1166667f
              }
            });
            EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
            {
              new PlantElementAbsorber.ConsumeInfo()
              {
                tag = GameTags.Dirt,
                massConsumptionRate = 0.01666667f
              }
            });

            placedEntity.AddComponent<StandardCropPlant>();
            placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, -1);
            placedEntity.AddOrGet<BuddingTrunk>().budPrefabID = "ForestTreeBranch";

            string domesticateddesc = global::STRINGS.CREATURES.SPECIES.WOOD_TREE.DOMESTICATEDDESC;
            GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(
                placedEntity,
                SeedProducer.ProductionType.Hidden,
                "GiantTreeSeed",
                "seed",
                global::STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC,
                Assets.GetAnim("seed_tree_kanim"),
                additionalTags: new List<Tag>
                {
                    GameTags.CropSeed
                },
                replantGroundTag: default,
                sortOrder: 4,
                domesticatedDescription: domesticateddesc,
                width: 0.3f,
                height: 0.3f);

            EntityTemplates.CreateAndRegisterPreviewForPlant(
                seed,
                "GiantTree_preview",
                Assets.GetAnim("tree_kanim"),
                "place",
                6,
                6);

            return placedEntity;
        }

        public void OnPrefabInit(GameObject inst)
        {
            inst.GetComponent<KBatchedAnimController>().animScale *= 2;
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
