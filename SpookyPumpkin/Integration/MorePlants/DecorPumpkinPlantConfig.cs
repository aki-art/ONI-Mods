/*using FUtility;
using System;
using System.Collections.Generic;
using UnityEngine;
using TUNING;
using static SpookyPumpkinSO.STRINGS.CREATURES.SPECIES;

namespace SpookyPumpkinSO.Integration.MorePlants
{
    // only registered if More Plants is also around
    internal class DecorPumpkinPlantConfig
    {
        public const string ID = "SP_PumpkinPlant_Decor";
        public const string SEED_ID = "SP_PumpkinPlant_DecorSeed";
        public const int DECOR_BONUS = 15;

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                SP_PUMPKIN.DECOR_NAME,
                SP_PUMPKIN.DESC,
                1f,
                Assets.GetAnim("sp_pumpkinplant_kanim"),
                "idle_empty",
                Grid.SceneLayer.BuildingFront,
                1,
                1,
                new EffectorValues(DECOR.BONUS.TIER1.amount + DECOR_BONUS, DECOR.BONUS.TIER1.radius));

            EntityTemplates.ExtendEntityToBasicPlant(
                prefab,
                200f,
                273.15f,
                373.15f,
                400f,
                new SimHashes[]
                {
                    SimHashes.Oxygen,
                    SimHashes.ContaminatedOxygen,
                    SimHashes.CarbonDioxide,
                    SimHashes.ChlorineGas,
                    SimHashes.Hydrogen
                },
                max_radiation: PLANTS.RADIATION_THRESHOLDS.TIER_3,
                baseTraitName: SP_PUMPKIN.DECOR_NAME,
                baseTraitId: ID + "Original");


            var t_StandardMorePlants = Type.GetType("StandardMorePlants", false, false);
            prefab.AddComponent(t_StandardMorePlants);

            var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
                    prefab,
                    SeedProducer.ProductionType.Sterile,
                    SEED_ID,
                    "Decor Pumpkin Seed",
                    SEEDS.SP_PUMPKIN.DESC,
                    Assets.GetAnim("sp_pumpkinseed_kanim"),
                    additionalTags: new List<Tag> {
                        GameTags.DecorSeed
                    },
                    sortOrder: 2,
                    domesticatedDescription: SP_PUMPKIN.DOMESTICATEDDESC);

            EntityTemplates.CreateAndRegisterPreviewForPlant(
                seed,
                ID + "_preview",
                Assets.GetAnim("sp_pumpkinplant_kanim"),
                "place",
                1,
                1);

            /*
            RecipeBuilder.Create("moreplants_SeedMaker", "desc", 15f)
                .Input(SEED_ID, 1f)
                .Input(RotPileConfig.ID, 1f)
                .Output(SEED_ID, 1f)
                .Build();
            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            if (inst.TryGetComponent(out ReceptacleMonitor receptacleMonitor))
            {
                Log.Debuglog($"I have been planted in a {receptacleMonitor.GetReceptacle()?.PrefabID()}");
            }
        }
    }
}
*/