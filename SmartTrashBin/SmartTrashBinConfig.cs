using FUtility;
using TUNING;
using UnityEngine;

namespace SmartTrashBin
{
	public class SmartTrashBinConfig : IBuildingConfig, IModdedBuilding
    {
		public static string ID = "STB_SmartTrashBin";
		public MBInfo Info => new MBInfo(ID, "Base");

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
				NOISE_POLLUTION.NONE );

			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.Overheatable = false;

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
			Prioritizable.AddRef(go);

			Storage storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.allowItemRemoval = false;
			storage.showDescriptor = true;
			storage.storageFilters = StorageFilters.all;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
			//go.AddComponent<SmartTrashBin>();

			go.AddOrGet<TreeFilterable>();
			go.AddOrGet<SmartDisposalBin>();
			go.AddOrGet<UserNameable>();
			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
		}
		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<StorageController.Def>();
		}
	}
}
