using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace DecorPackA.Buildings.Aquarium
{
    class AquariumConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.PREFIX + "Aquarium";
        public MBInfo Info => new MBInfo(ID, "Furniture", "Luxury");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               2,
               3,
               "aquarium_kanim",
               100,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               MATERIALS.TRANSPARENTS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               decor: new EffectorValues(20, 8),
               NOISE_POLLUTION.NONE
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

        public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
        {
            Storage fishStorage = go.AddOrGet<Storage>();
            fishStorage.allowItemRemoval = false;
            fishStorage.showDescriptor = true;
            fishStorage.storageFilters = STORAGEFILTERS.SWIMMING_CREATURES;
            fishStorage.workAnims = new HashedString[] { "working_pre" };
            fishStorage.overrideAnims = new[] { Assets.GetAnim("anim_interacts_fishrelocator_kanim") };
            fishStorage.workAnimPlayMode = KAnim.PlayMode.Once;
            fishStorage.synchronizeAnims = false;
            fishStorage.useGunForDelivery = false;
            fishStorage.allowSettingOnlyFetchMarkedItems = false;
            fishStorage.requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;

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

            FishReceptable singleEntityReceptacle = go.AddOrGet<FishReceptable>();
            //singleEntityReceptacle.AddDepositTag(GameTags.SwimmingCreature);
            singleEntityReceptacle.occupyingObjectRelativePosition = new Vector3(0.5f, 1.3f, 0.2f);

            //receptacle.possibleDepositObjectTags = STORAGEFILTERS.SWIMMING_CREATURES;
            go.AddTag(GameTags.Decoration);
            go.AddOrGet<DecorProvider>();

            Aquarium aquarium = go.AddOrGet<Aquarium>();
            //aquarium.waterStorage = waterStorage;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}