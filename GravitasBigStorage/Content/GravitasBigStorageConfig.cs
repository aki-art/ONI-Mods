using TUNING;
using UnityEngine;

namespace GravitasBigStorage.Content
{
	public class GravitasBigStorageConfig : IBuildingConfig
	{
		public const string ID = "GravitasBigStorage_Container";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				4,
				6,
				"gravitasbigstorage_container_kanim",
				1000,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER6,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER5,
				MATERIALS.RAW_METALS,
				BUILDINGS.MELTING_POINT_KELVIN.TIER4,
				BuildLocationRule.OnFloor,
				LonelyMinionHouseConfig.HOUSE_DECOR,
				NOISE_POLLUTION.NONE);

			def.DefaultAnimState = "on";
			def.ForegroundLayer = Grid.SceneLayer.BuildingFront;
			def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
			def.AudioSize = AUDIO.SIZE.LARGE;
			def.POIUnlockable = true;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);

			var storage = go.AddOrGet<Storage>();
			storage.capacityKg = Mod.Settings.Capacity;
			storage.showInUI = true;
			storage.allowItemRemoval = true;
			storage.showDescriptor = true;
			storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
			storage.showCapacityStatusItem = true;
			storage.showCapacityAsMainStatus = true;

			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
			go.AddOrGet<StorageLocker>();
			go.AddOrGet<UserNameable>();
			go.AddOrGetDef<RocketUsageRestriction.Def>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<StorageController.Def>();
		}
	}
}
