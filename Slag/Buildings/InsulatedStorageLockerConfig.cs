using System.Collections.Generic;
using TUNING;
using UnityEngine;
using FUtility;

namespace Slag.Buildings
{
	[StringsPath(typeof(SlagStrings.SLAGSTRINGS.BUILDINGS.PREFABS.INSULATED_STORAGE_LOCKER))]
	[BuildMenu("Base")]
	class InsulatedStorageLockerConfig : IBuildingConfig
	{
		public const string ID = "InsulatedStorageLocker"; 
		private static readonly List<Storage.StoredItemModifier> storedItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Seal
		};
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 2,
				anim: "storagelocker_kanim",
				hitpoints: 30,
				construction_time: 10f,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
				construction_materials: MATERIALS.RAW_MINERALS,
				melting_point: 1600f,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NONE);

			def.Floodable = false;
			def.AudioCategory = "Metal";
			def.Overheatable = false;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
			Prioritizable.AddRef(go);
			Storage storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.allowItemRemoval = true;
			storage.showDescriptor = true;
			storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
			storage.SetDefaultStoredItemModifiers(storedItemModifiers);

			CopyBuildingSettings cbs = go.AddOrGet<CopyBuildingSettings>();
			cbs.copyGroupTag = GameTags.StorageLocker;
			go.AddOrGet<StorageLocker>();
			go.AddOrGet<UserNameable>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<StorageController.Def>();
		}
	}
}

