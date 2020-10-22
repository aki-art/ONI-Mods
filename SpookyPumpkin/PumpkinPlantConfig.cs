using System.Collections.Generic;
using UnityEngine;
using static SpookyPumpkin.STRINGS.CREATURES.SPECIES;

namespace SpookyPumpkin
{
    public class PumpkinPlantConfig : IEntityConfig
    {
        public const string ID = ModAssets.PREFIX + "PumpkinPlant";
        public const string SEED_ID = ModAssets.PREFIX + "PumpkinSeed";
        public const float FERTILIZATION_RATE = 0.008333334f;

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreatePlacedEntity(
                id: ID,
                name: SP_PUMPKIN.NAME,
                desc: SP_PUMPKIN.DESC,
                mass: 1f,
                anim: Assets.GetAnim("sp_pumpkinplant_kanim"),
                initialAnim: "idle_empty",
                sceneLayer: Grid.SceneLayer.BuildingFront,
                width: 1,
                height: 1,
                decor: TUNING.DECOR.BONUS.TIER1);


            EntityTemplates.ExtendEntityToBasicPlant(
                template: prefab,
                temperature_lethal_low: 228.15f,
                temperature_warning_low: 278.15f,
                temperature_warning_high: 308.15f,
                temperature_lethal_high: 398.15f,
                safe_elements: new SimHashes[]
                {
                  SimHashes.Oxygen,
                  SimHashes.ContaminatedOxygen,
                  SimHashes.CarbonDioxide
                },
                crop_id: Foods.PumpkinConfig.ID);

            EntityTemplates.ExtendPlantToFertilizable(prefab,
                new PlantElementAbsorber.ConsumeInfo[2]
                {
                  new PlantElementAbsorber.ConsumeInfo()
                  {
                    tag = RotPileConfig.ID,
                    massConsumptionRate = 0.0008333334f / 2f
                  },

                  new PlantElementAbsorber.ConsumeInfo()
                  {
                    tag = SimHashes.ToxicSand.CreateTag(),
                    massConsumptionRate = 0.0008333334f / 2f
                  }
                });

            prefab.AddOrGet<StandardCropPlant>();

            GameObject seed = EntityTemplates.CreateAndRegisterSeedForPlant(
                plant: prefab,
                productionType: SeedProducer.ProductionType.Harvest,
                id: SEED_ID,
                name: SEEDS.SP_PUMPKIN.NAME,
                desc: SEEDS.SP_PUMPKIN.DESC,
                anim: Assets.GetAnim("sp_pumpkinseed_kanim"),
                additionalTags: new List<Tag> { GameTags.CropSeed },
                sortOrder: 2,
                domesticatedDescription: SP_PUMPKIN.DOMESTICATEDDESC);

            EntityTemplates.CreateAndRegisterPreviewForPlant(
                seed: seed,
                id: ModAssets.PREFIX + "Pumpkin_preview",
                anim: Assets.GetAnim("sp_pumpkinplant_kanim"),
                initialAnim: "place",
                width: 1,
                height: 1);

            return prefab;
        }

        public void OnPrefabInit(GameObject inst)
        {
            inst.GetComponent<KBatchedAnimController>().randomiseLoopedOffset = true;
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
