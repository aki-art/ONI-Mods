﻿using DecorPackB.Content.Scripts;
using TUNING;
using UnityEngine;

namespace DecorPackB.Content.Defs.Buildings
{
	public class PotConfig : IBuildingConfig
	{
		public const string ID = "DecorPackB_Pot";

		public override BuildingDef CreateBuildingDef()
		{
			var prefab = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"decorpackb_pot_generic_tall_kanim",
				BUILDINGS.HITPOINTS.TIER1,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				new[] { SimHashes.Clay.ToString() },
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				DECOR.NONE, // added by the artable
				NOISE_POLLUTION.NONE);

			prefab.Floodable = false;
			prefab.Overheatable = false;

			return prefab;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);

			Prioritizable.AddRef(go);

			var storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.allowItemRemoval = true;
			storage.showDescriptor = true;
			storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
			storage.showCapacityStatusItem = true;
			storage.showCapacityAsMainStatus = true;
			storage.capacityKg = 5000;

			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
			go.AddOrGet<StorageLocker>();
			go.AddOrGet<UserNameable>();
			go.AddOrGetDef<RocketUsageRestriction.Def>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<StorageController.Def>();
			go.AddOrGet<Pot>();
		}
	}
}
