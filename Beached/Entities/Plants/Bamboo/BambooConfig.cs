using Beached.Components;
using TUNING;
using UnityEngine;

namespace Beached.Entities.Plants.Bamboo
{
    internal class BambooConfig : IEntityConfig
    {
		public const string ID = "Beached_Bamboo";

        public GameObject CreatePrefab()
        {
            GameObject prefab = EntityTemplates.CreatePlacedEntity(
                ID,
                "Bamboo",
                "",
                100f,
                Assets.GetAnim("muckroot_kanim"),
                "idle",
                Grid.SceneLayer.BuildingBack,
                1,
                1,
                DECOR.BONUS.TIER1);

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
			{
			    ObjectLayer.Building
			};

			prefab.AddOrGet<EntombVulnerable>();
			prefab.AddOrGet<DrowningMonitor>();
			prefab.AddOrGet<Prioritizable>();
			prefab.AddOrGet<Uprootable>();

            var toppleMonitor = prefab.AddOrGet<ToppleMonitor>();
            toppleMonitor.validFoundationTag = ModAssets.Tags.Bamboo;
            toppleMonitor.objectLayer = ObjectLayer.Building;

            prefab.AddOrGet<Harvestable>();
			prefab.AddOrGet<HarvestDesignatable>();
			prefab.AddOrGet<SeedProducer>().Configure("BasicForagePlant", SeedProducer.ProductionType.DigOnly, 1);
			prefab.AddOrGet<BasicForagePlantPlanted>();
			prefab.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
            prefab.AddOrGet<SelfDuplicatorPlant>();

            Growing growing = prefab.AddOrGet<Growing>();
            growing.shouldGrowOld = false;
            growing.maxAge = 60f;

            prefab.AddTag(ModAssets.Tags.Bamboo);

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
