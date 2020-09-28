using TUNING;
using UnityEngine;

namespace TestObject
{
	class TestBuildingConfig : IBuildingConfig
	{
		public static string ID = "TestBuilding";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"storagelocker_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
				MATERIALS.RAW_MINERALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				BUILDINGS.DECOR.PENALTY.TIER1,
				NOISE_POLLUTION.NONE);

			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.Overheatable = false;

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);

			Storage storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.allowItemRemoval = false;
			storage.showDescriptor = true;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;

			ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
			manualDeliveryKG.SetStorage(storage);
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
			manualDeliveryKG.requestedItemTag = new Tag(TestObjectConfig.ID);
			manualDeliveryKG.refillMass = 1f;
			manualDeliveryKG.minimumMass = 1f;
			manualDeliveryKG.capacity = 1f;

			go.GetComponent<KPrefabID>().prefabSpawnFn += g => Debug.Log("spawned");
			go.GetComponent<KPrefabID>().instantiateFn += g => Debug.Log("instantiated");

			go.AddOrGet<SoundPlayer>();
		}
		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<StorageController.Def>();
		}
	}
}
