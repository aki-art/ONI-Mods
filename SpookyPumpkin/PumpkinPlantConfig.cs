using System.Collections.Generic;
using UnityEngine;
using static SpookyPumpkin.STRINGS.CREATURES.SPECIES;

namespace SpookyPumpkin
{
    public class PumpkinPlantConfig : IEntityConfig
    {
        public const string ID = "SP_PumpkinPlant";
        public const string SEED_ID = "SP_PumpkinSeed";
        public const float DIRT_PER_CYCLE = 7f / 600f;
        public const float DIRT_PER_CYCLE_NO_ROT = 10f / 600f;
        public const float ROT_PER_CYCLE = 0.05f / 600f;

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
                  SimHashes.CarbonDioxide,
                  SimHashes.ChlorineGas
                },
                crop_id: Foods.PumpkinConfig.ID);

            float dirtConsumption = Settings.ModSettings.Settings.PumpkinRequiresRot ?
                DIRT_PER_CYCLE :
                DIRT_PER_CYCLE_NO_ROT;

            var rot = new PlantElementAbsorber.ConsumeInfo()
            {
                tag = RotPileConfig.ID,
                massConsumptionRate = ROT_PER_CYCLE
            };

            var pollutedDirt = new PlantElementAbsorber.ConsumeInfo()
            {
                tag = GameTags.Dirt,
                massConsumptionRate = dirtConsumption
            };

            var fertilizers = new PlantElementAbsorber.ConsumeInfo[1] { pollutedDirt };

            if(Settings.ModSettings.Settings.PumpkinRequiresRot)
                fertilizers = fertilizers.Append(rot);

            EntityTemplates.ExtendPlantToFertilizable(prefab, fertilizers);

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
                id: "SP_Pumpkin_preview",
                anim: Assets.GetAnim("sp_pumpkinplant_kanim"),
                initialAnim: "place",
                width: 1,
                height: 1);

            return prefab;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
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
