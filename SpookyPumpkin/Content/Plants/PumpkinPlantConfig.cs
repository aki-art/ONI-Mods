using System.Collections.Generic;
using UnityEngine;
using static SpookyPumpkinSO.STRINGS.CREATURES.SPECIES;

namespace SpookyPumpkinSO.Content.Plants
{
	public class PumpkinPlantConfig : IEntityConfig
	{
		public const string ID = "SP_PumpkinPlant";
		public const string BASE_TRAIT_ID = "SP_PumpkinPlantOriginal";
		public const string SEED_ID = "SP_PumpkinSeed";
		public const float DIRT_PER_CYCLE = 7f / 600f;
		public const float DIRT_PER_CYCLE_NO_ROT = 10f / 600f;
		public const float ROT_PER_CYCLE = 0.05f / 600f;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				SP_PUMPKIN.NAME,
				SP_PUMPKIN.DESC,
				1f,
				Assets.GetAnim("sp_pumpkinplant_kanim"),
				"idle_empty",
				Grid.SceneLayer.BuildingFront,
				1,
				1,
				TUNING.DECOR.BONUS.TIER1);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,
				228.15f,
				278.15f,
				308.15f,
				398.15f,
				new SimHashes[]
				{
					SimHashes.Oxygen,
					SimHashes.ContaminatedOxygen,
					SimHashes.CarbonDioxide,
					SimHashes.ChlorineGas
				},
				crop_id: Foods.PumpkinConfig.ID,
				max_radiation: TUNING.PLANTS.RADIATION_THRESHOLDS.TIER_3,
				baseTraitId: BASE_TRAIT_ID,
				baseTraitName: SP_PUMPKIN.NAME);

			var dirtConsumption = Mod.Config.UseRot ? DIRT_PER_CYCLE : DIRT_PER_CYCLE_NO_ROT;

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

			var fertilizers = new PlantElementAbsorber.ConsumeInfo[] { pollutedDirt };

			if (Mod.Config.UseRot)
				fertilizers = fertilizers.Append(rot);

			EntityTemplates.ExtendPlantToFertilizable(prefab, fertilizers);

			prefab.AddOrGet<StandardCropPlant>();

			var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
				prefab,
				SeedProducer.ProductionType.Harvest,
				SEED_ID,
				SEEDS.SP_PUMPKIN.NAME,
				SEEDS.SP_PUMPKIN.DESC,
				Assets.GetAnim("sp_pumpkinseed_kanim"),
				additionalTags: new List<Tag> { GameTags.CropSeed },
				sortOrder: 2,
				domesticatedDescription: SP_PUMPKIN.DOMESTICATEDDESC);

			EntityTemplates.CreateAndRegisterPreviewForPlant(
				seed,
				"SP_Pumpkin_preview",
				Assets.GetAnim("sp_pumpkinplant_kanim"),
				"place",
				1,
				1);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
			inst.GetComponent<KBatchedAnimController>().randomiseLoopedOffset = true;
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
