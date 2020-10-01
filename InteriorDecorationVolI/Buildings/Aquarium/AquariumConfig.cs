using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    class AquariumConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = ModAssets.PREFIX + "Aquarium";
        public MBInfo Info => new MBInfo(ID, "Furniture", "Luxury");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 2,
               height: 3,
               anim: "aquarium_kanim",
               hitpoints: 100,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.TRANSPARENTS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.OnFloor,
               decor: new EffectorValues(20, 8),
               noise: NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Entombable = true;
            def.AudioCategory = "Glass";
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "off";
            def.PermittedRotations = PermittedRotations.FlipH;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag _)
        {
            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);

            Storage storage = go.AddOrGet<Storage>();
            storage.allowItemRemoval = false;
            storage.showDescriptor = true;
            storage.storageFilters = STORAGEFILTERS.SWIMMING_CREATURES;
            storage.workAnims = new HashedString[] { "working_pre" };
            storage.overrideAnims = new KAnimFile[] { Assets.GetAnim("anim_interacts_fishrelocator_kanim") };
            storage.workAnimPlayMode = KAnim.PlayMode.Once;
            storage.synchronizeAnims = false;
            storage.useGunForDelivery = false;
            storage.allowSettingOnlyFetchMarkedItems = false;
            storage.requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;

            var storageModifiers = new List<Storage.StoredItemModifier>
            {
                Storage.StoredItemModifier.Hide,
                Storage.StoredItemModifier.Seal
            };

            Storage waterStorage = go.AddComponent<Storage>();
            waterStorage.capacityKg = 360f;
            waterStorage.showInUI = true;
            waterStorage.SetDefaultStoredItemModifiers(storageModifiers);
            waterStorage.allowItemRemoval = false;
            waterStorage.storageFilters = new List<Tag> { SimHashes.Water.CreateTag() };

            var waterDelivery = go.AddComponent<ManualDeliveryKG>();
            waterDelivery.SetStorage(waterStorage);
            waterDelivery.requestedItemTag = SimHashes.Water.CreateTag();
            waterDelivery.capacity = 100f;
            waterDelivery.refillMass = 100f;
            waterDelivery.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;

            //go.AddComponent<FishTank>();
		    SingleEntityReceptacle singleEntityReceptacle = go.AddOrGet<SingleEntityReceptacle>();
		    singleEntityReceptacle.AddDepositTag(GameTags.SwimmingCreature);
		    singleEntityReceptacle.occupyingObjectRelativePosition = new Vector3(0f, 1.2f, -1f);

            //receptacle.possibleDepositObjectTags = STORAGEFILTERS.SWIMMING_CREATURES;
            go.AddTag(GameTags.Decoration);
            go.AddOrGet<DecorProvider>();
            go.AddOrGet<Aquarium>();
        }

        public override void DoPostConfigureComplete(GameObject go) 
        {
            go.GetComponent<KBatchedAnimController>().animScale *= 1.5f; // temporary
        }
    }
}
